using System.Data;
using Newtonsoft.Json;
using Serilog;
using SyncDataSqlToWebApi.Models;

namespace SyncDataSqlToWebApi.Services;

public class SyncJobExecutor
{
    private readonly SqlDataService _sqlService;
    private readonly ApiService _apiService;
    private readonly SyncStateManager _stateManager;

    public SyncJobExecutor(SqlDataService sqlService, ApiService apiService, SyncStateManager stateManager)
    {
        _sqlService = sqlService;
        _apiService = apiService;
        _stateManager = stateManager;
    }

    public async Task<bool> ExecuteJobAsync(SyncJob job)
    {
        try
        {
            Log.Information("========================================");
            Log.Information("[{JobName}] Starting job: {Description}", job.JobName, job.Description);

            // Step 1: Execute SQL query
            var dataTable = await _sqlService.ExecuteQueryAsync(job.Query);
            if (dataTable.Rows.Count == 0)
            {
                Log.Information("[{JobName}] No records returned from query", job.JobName);
                return true;
            }

            Log.Information("[{JobName}] Query returned {Count} records", job.JobName, dataTable.Rows.Count);

            // Step 2: Filter records based on sync state (if enabled)
            var (recordsToSync, skippedCount) = await FilterRecordsAsync(job, dataTable);

            if (recordsToSync.Count == 0)
            {
                Log.Information("[{JobName}] All {Total} records already synced, nothing to do", 
                    job.JobName, dataTable.Rows.Count);
                return true;
            }

            Log.Information("[{JobName}] Filtered: {ToSync} records need sync, {Skipped} already synced", 
                job.JobName, recordsToSync.Count, skippedCount);

            // Step 3: Convert to parameter list and mark as pending
            var (parameters, hashes, recordKeys) = await PrepareDataForSyncAsync(job, recordsToSync);

            // Step 4: Mark all as PENDING before sending
            await MarkRecordsAsPendingAsync(job.JobName, hashes, recordKeys);

            // Step 5: Send data in batches to API
            var (successCount, failureCount) = await _apiService.SendDataInBatchesAsync(
                job.JobName, 
                job.Procedure, 
                parameters, 
                hashes, 
                _stateManager);

            // Step 6: Update job metadata
            await _stateManager.UpdateJobMetadataAsync(job.JobName, successCount, failureCount);

            Log.Information("[{JobName}] Job completed: {Success} succeeded, {Failed} failed, {Skipped} skipped", 
                job.JobName, successCount, failureCount, skippedCount);

            return failureCount == 0;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "[{JobName}] Job execution failed", job.JobName);
            return false;
        }
    }

    private async Task<(List<DataRow> toSync, int skipped)> FilterRecordsAsync(SyncJob job, DataTable dataTable)
    {
        if (!job.SkipSyncedRecords)
        {
            // If skip is disabled, sync all records
            var allRecords = dataTable.Rows.Cast<DataRow>().ToList();
            return (allRecords, 0);
        }

        // Get already synced hashes
        var syncedHashes = await _stateManager.GetSyncedHashesAsync(job.JobName, job.RevalidateAfterDays);

        var recordsToSync = new List<DataRow>();
        int skippedCount = 0;

        foreach (DataRow row in dataTable.Rows)
        {
            var hash = _stateManager.GenerateRecordHash(row, job.UniqueKeyColumns);
            
            if (syncedHashes.Contains(hash))
            {
                skippedCount++;
            }
            else
            {
                recordsToSync.Add(row);
            }
        }

        return (recordsToSync, skippedCount);
    }

    private async Task<(List<List<object>> parameters, List<string> hashes, List<string> recordKeys)> 
        PrepareDataForSyncAsync(SyncJob job, List<DataRow> records)
    {
        var parameters = new List<List<object>>();
        var hashes = new List<string>();
        var recordKeys = new List<string>();

        foreach (var row in records)
        {
            // Generate hash for tracking
            var hash = _stateManager.GenerateRecordHash(row, job.UniqueKeyColumns);
            hashes.Add(hash);

            // Generate record keys for logging
            var keyDict = new Dictionary<string, object>();
            foreach (var col in job.UniqueKeyColumns)
            {
                keyDict[col] = row[col];
            }
            recordKeys.Add(JsonConvert.SerializeObject(keyDict));

            // Build parameter list according to ParameterColumns
            var paramList = new List<object>();
            foreach (var col in job.ParameterColumns)
            {
                var value = row[col];
                // Handle DBNull similar to JavaScript: (q[p] || q[p] == 0 ? q[p] : '')
                if (value == null || value == DBNull.Value)
                {
                    paramList.Add("");
                }
                else
                {
                    paramList.Add(value);
                }
            }

            parameters.Add(paramList);
        }

        return (parameters, hashes, recordKeys);
    }

    private async Task MarkRecordsAsPendingAsync(string jobName, List<string> hashes, List<string> recordKeys)
    {
        Log.Information("[{JobName}] Marking {Count} records as PENDING", jobName, hashes.Count);

        for (int i = 0; i < hashes.Count; i++)
        {
            await _stateManager.InsertPendingRecordAsync(jobName, hashes[i], recordKeys[i]);
        }
    }

    public async Task ExecuteAllJobsAsync(List<SyncJob> jobs)
    {
        var enabledJobs = jobs.Where(j => j.Enabled).ToList();
        
        if (enabledJobs.Count == 0)
        {
            Log.Warning("No enabled jobs found");
            return;
        }

        Log.Information("Found {Total} jobs, {Enabled} enabled", jobs.Count, enabledJobs.Count);

        foreach (var job in enabledJobs)
        {
            await ExecuteJobAsync(job);
        }

        Log.Information("All jobs completed");
    }
}
