using System.Data;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Data.Sqlite;
using Newtonsoft.Json;
using Serilog;
using SyncDataSqlToWebApi.Models;

namespace SyncDataSqlToWebApi.Services;

public class SyncStateManager : IDisposable
{
    private readonly string _connectionString;
    private readonly string _hashAlgorithm;
    private SqliteConnection? _connection;

    public SyncStateManager(string databasePath, string hashAlgorithm = "SHA256")
    {
        _connectionString = $"Data Source={databasePath}";
        _hashAlgorithm = hashAlgorithm;
    }

    public async Task InitializeDatabaseAsync()
    {
        _connection = new SqliteConnection(_connectionString);
        await _connection.OpenAsync();

        var createTableCommand = _connection.CreateCommand();
        createTableCommand.CommandText = @"
            CREATE TABLE IF NOT EXISTS SyncStates (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                JobName TEXT NOT NULL,
                RecordHash TEXT NOT NULL,
                RecordKeys TEXT NOT NULL,
                SyncedAt DATETIME NOT NULL,
                ApiResponse TEXT,
                Status TEXT NOT NULL,
                RetryCount INTEGER DEFAULT 0,
                LastError TEXT,
                UNIQUE(JobName, RecordHash)
            );

            CREATE INDEX IF NOT EXISTS idx_job_status ON SyncStates(JobName, Status);
            CREATE INDEX IF NOT EXISTS idx_synced_at ON SyncStates(SyncedAt);

            CREATE TABLE IF NOT EXISTS JobMetadata (
                JobName TEXT PRIMARY KEY,
                LastSyncTime DATETIME,
                TotalRecordsSynced INTEGER DEFAULT 0,
                LastSuccessCount INTEGER DEFAULT 0,
                LastFailureCount INTEGER DEFAULT 0
            );
        ";
        await createTableCommand.ExecuteNonQueryAsync();

        Log.Information("Sync state database initialized at {Path}", _connectionString);
    }

    public string GenerateRecordHash(DataRow row, List<string> keyColumns)
    {
        var keyValues = new List<string>();
        foreach (var col in keyColumns)
        {
            var value = row[col]?.ToString() ?? "";
            keyValues.Add(value);
        }

        string concatenated = string.Join("|", keyValues);

        if (_hashAlgorithm.ToUpper() == "SHA256")
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(concatenated);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
        else
        {
            using var md5 = MD5.Create();
            var bytes = Encoding.UTF8.GetBytes(concatenated);
            var hash = md5.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }

    public async Task<bool> IsRecordSyncedAsync(string jobName, string hash, int revalidateDays)
    {
        if (_connection == null) throw new InvalidOperationException("Database not initialized");

        var command = _connection.CreateCommand();
        command.CommandText = @"
            SELECT COUNT(*) FROM SyncStates 
            WHERE JobName = @jobName 
            AND RecordHash = @hash 
            AND Status = @status
            AND (@revalidateDays = 0 OR SyncedAt >= @cutoffDate)
        ";
        command.Parameters.AddWithValue("@jobName", jobName);
        command.Parameters.AddWithValue("@hash", hash);
        command.Parameters.AddWithValue("@status", SyncStatus.Success);
        command.Parameters.AddWithValue("@revalidateDays", revalidateDays);
        command.Parameters.AddWithValue("@cutoffDate", DateTime.Now.AddDays(-revalidateDays));

        var result = await command.ExecuteScalarAsync();
        return Convert.ToInt32(result) > 0;
    }

    public async Task<HashSet<string>> GetSyncedHashesAsync(string jobName, int revalidateDays)
    {
        if (_connection == null) throw new InvalidOperationException("Database not initialized");

        var hashes = new HashSet<string>();
        var command = _connection.CreateCommand();
        command.CommandText = @"
            SELECT RecordHash FROM SyncStates 
            WHERE JobName = @jobName 
            AND Status = @status
            AND (@revalidateDays = 0 OR SyncedAt >= @cutoffDate)
        ";
        command.Parameters.AddWithValue("@jobName", jobName);
        command.Parameters.AddWithValue("@status", SyncStatus.Success);
        command.Parameters.AddWithValue("@revalidateDays", revalidateDays);
        command.Parameters.AddWithValue("@cutoffDate", DateTime.Now.AddDays(-revalidateDays));

        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            hashes.Add(reader.GetString(0));
        }

        return hashes;
    }

    public async Task InsertPendingRecordAsync(string jobName, string hash, string recordKeys)
    {
        if (_connection == null) throw new InvalidOperationException("Database not initialized");

        var command = _connection.CreateCommand();
        command.CommandText = @"
            INSERT OR REPLACE INTO SyncStates (JobName, RecordHash, RecordKeys, SyncedAt, Status, RetryCount)
            VALUES (@jobName, @hash, @keys, @syncedAt, @status, 0)
        ";
        command.Parameters.AddWithValue("@jobName", jobName);
        command.Parameters.AddWithValue("@hash", hash);
        command.Parameters.AddWithValue("@keys", recordKeys);
        command.Parameters.AddWithValue("@syncedAt", DateTime.Now);
        command.Parameters.AddWithValue("@status", SyncStatus.Pending);

        await command.ExecuteNonQueryAsync();
    }

    public async Task UpdateRecordStatusAsync(string jobName, string hash, string status, string? response = null, string? error = null)
    {
        if (_connection == null) throw new InvalidOperationException("Database not initialized");

        var command = _connection.CreateCommand();
        command.CommandText = @"
            UPDATE SyncStates 
            SET Status = @status, 
                ApiResponse = @response, 
                LastError = @error,
                SyncedAt = @syncedAt,
                RetryCount = RetryCount + CASE WHEN @status = @failedStatus THEN 1 ELSE 0 END
            WHERE JobName = @jobName AND RecordHash = @hash
        ";
        command.Parameters.AddWithValue("@jobName", jobName);
        command.Parameters.AddWithValue("@hash", hash);
        command.Parameters.AddWithValue("@status", status);
        command.Parameters.AddWithValue("@response", response ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@error", error ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@syncedAt", DateTime.Now);
        command.Parameters.AddWithValue("@failedStatus", SyncStatus.Failed);

        await command.ExecuteNonQueryAsync();
    }

    public async Task MarkBatchAsSuccessAsync(string jobName, List<string> hashes, string apiResponse)
    {
        if (_connection == null) throw new InvalidOperationException("Database not initialized");

        using var transaction = _connection.BeginTransaction();
        try
        {
            foreach (var hash in hashes)
            {
                await UpdateRecordStatusAsync(jobName, hash, SyncStatus.Success, apiResponse);
            }
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task MarkBatchAsFailedAsync(string jobName, List<string> hashes, string error)
    {
        if (_connection == null) throw new InvalidOperationException("Database not initialized");

        using var transaction = _connection.BeginTransaction();
        try
        {
            foreach (var hash in hashes)
            {
                await UpdateRecordStatusAsync(jobName, hash, SyncStatus.Failed, null, error);
            }
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task UpdateJobMetadataAsync(string jobName, int successCount, int failureCount)
    {
        if (_connection == null) throw new InvalidOperationException("Database not initialized");

        var command = _connection.CreateCommand();
        command.CommandText = @"
            INSERT OR REPLACE INTO JobMetadata (JobName, LastSyncTime, TotalRecordsSynced, LastSuccessCount, LastFailureCount)
            VALUES (
                @jobName, 
                @syncTime, 
                COALESCE((SELECT TotalRecordsSynced FROM JobMetadata WHERE JobName = @jobName), 0) + @successCount,
                @successCount,
                @failureCount
            )
        ";
        command.Parameters.AddWithValue("@jobName", jobName);
        command.Parameters.AddWithValue("@syncTime", DateTime.Now);
        command.Parameters.AddWithValue("@successCount", successCount);
        command.Parameters.AddWithValue("@failureCount", failureCount);

        await command.ExecuteNonQueryAsync();
    }

    public async Task CleanupOldStatesAsync(int retentionDays)
    {
        if (_connection == null) throw new InvalidOperationException("Database not initialized");

        var cutoffDate = DateTime.Now.AddDays(-retentionDays);
        var command = _connection.CreateCommand();
        command.CommandText = @"
            DELETE FROM SyncStates 
            WHERE SyncedAt < @cutoffDate AND Status = @successStatus
        ";
        command.Parameters.AddWithValue("@cutoffDate", cutoffDate);
        command.Parameters.AddWithValue("@successStatus", SyncStatus.Success);

        var deleted = await command.ExecuteNonQueryAsync();
        if (deleted > 0)
        {
            Log.Information("Cleaned up {Count} old sync state records", deleted);
        }
    }

    public async Task<int> GetPendingCountAsync(string jobName)
    {
        if (_connection == null) throw new InvalidOperationException("Database not initialized");

        var command = _connection.CreateCommand();
        command.CommandText = @"
            SELECT COUNT(*) FROM SyncStates 
            WHERE JobName = @jobName AND Status = @status
        ";
        command.Parameters.AddWithValue("@jobName", jobName);
        command.Parameters.AddWithValue("@status", SyncStatus.Pending);

        var result = await command.ExecuteScalarAsync();
        return Convert.ToInt32(result);
    }

    public async Task<(int success, int failed, int pending)> GetJobStatisticsAsync(string jobName)
    {
        if (_connection == null) throw new InvalidOperationException("Database not initialized");

        var command = _connection.CreateCommand();
        command.CommandText = @"
            SELECT 
                SUM(CASE WHEN Status = 'SUCCESS' THEN 1 ELSE 0 END) as success,
                SUM(CASE WHEN Status = 'FAILED' THEN 1 ELSE 0 END) as failed,
                SUM(CASE WHEN Status = 'PENDING' THEN 1 ELSE 0 END) as pending
            FROM SyncStates 
            WHERE JobName = @jobName
        ";
        command.Parameters.AddWithValue("@jobName", jobName);

        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return (
                reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                reader.IsDBNull(1) ? 0 : reader.GetInt32(1),
                reader.IsDBNull(2) ? 0 : reader.GetInt32(2)
            );
        }

        return (0, 0, 0);
    }

    public void Dispose()
    {
        _connection?.Dispose();
    }
}
