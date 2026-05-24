using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using Serilog;

class Program
{
    static async Task Main(string[] args)
    {
        EnsureSingleInstance();

        try
        {
            var configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.File(Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory, "logs", "Log.txt"),
                    rollingInterval: RollingInterval.Day,
                    outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level}] {Message}{NewLine}{Exception}")
                .CreateLogger();

            Log.Information("SyncDataOracle started.");

            
            

            var sourceDb = configuration.GetSection("SourceDatabase");
            var targetDb = configuration.GetSection("TargetDatabase");
            var syncSettings = configuration.GetSection("SyncSettings");

            string sourceConnectionString = BuildConnectionString(sourceDb);
            string targetConnectionString = BuildConnectionString(targetDb);

            int intervalMinutes = syncSettings.GetValue<int>("IntervalMinutes", 0);

            using var cts = new CancellationTokenSource();
            Console.CancelKeyPress += (_, eventArgs) =>
            {
                eventArgs.Cancel = true;
                Log.Information("Cancellation requested. Stopping scheduler...");
                cts.Cancel();
            };

            await RunSchedulerAsync(sourceConnectionString, targetConnectionString, syncSettings, intervalMinutes, cts.Token).ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            Log.Information("Operation cancelled by user.");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Log.Error(ex, "An error occurred during sync.");
            //Environment.ExitCode = -1;
        }
        finally
        {
            Log.Information("SyncDataOracle stopped.");
            Log.CloseAndFlush();
        }
    }

    private static string BuildConnectionString(IConfigurationSection section)
    {
        if (section == null)
        {
            throw new ArgumentNullException(nameof(section));
        }

        string userId = section["UserId"] ?? throw new InvalidOperationException("Database UserId must be configured.");
        string password = section["Password"] ?? throw new InvalidOperationException("Database Password must be configured.");
        string tns = section["TnsName"] ?? throw new InvalidOperationException("Database TnsName must be configured.");

        return $"User Id={userId};Password={password};Data Source={tns};";
    }

    private static async Task RunSchedulerAsync(string sourceConnStr, string targetConnStr, IConfigurationSection syncSettings, int intervalMinutes, CancellationToken cancellationToken)
    {
        await ExecuteSyncCycleAsync(sourceConnStr, targetConnStr, syncSettings, cancellationToken).ConfigureAwait(false);

        if (intervalMinutes <= 0)
        {
            Log.Information("IntervalMinutes not configured or <= 0. Scheduler will not repeat.");
            return;
        }

        var interval = TimeSpan.FromMinutes(intervalMinutes);
        Log.Information("Scheduler will execute every {Interval} minutes.", intervalMinutes);

        using var timer = new PeriodicTimer(interval);

        while (await timer.WaitForNextTickAsync(cancellationToken).ConfigureAwait(false))
        {
            try
            {
                await ExecuteSyncCycleAsync(sourceConnStr, targetConnStr, syncSettings, cancellationToken).ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Synchronization cycle encountered an error.");
            }
        }
    }

    private static async Task ExecuteSyncCycleAsync(string sourceConnStr, string targetConnStr, IConfigurationSection syncSettings, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        string sourceTable = syncSettings.GetValue<string>("SourceTable") ?? throw new InvalidOperationException("SourceTable must be configured.");
        string targetTable = syncSettings.GetValue<string>("TargetTable") ?? sourceTable;
        int daysToSync = Math.Max(1, syncSettings.GetValue<int>("DaysToSync", 1));

        string[] uniqueColumns = syncSettings.GetSection("UniqueColumns").Get<string[]>() ?? Array.Empty<string>();
        uniqueColumns = uniqueColumns
            .Where(col => !string.IsNullOrWhiteSpace(col))
            .Select(col => col.Trim())
            .ToArray();

        if (uniqueColumns.Length == 0)
        {
            throw new InvalidOperationException("UniqueColumns must be configured with at least one column.");
        }

        string? sourceQueryTemplate = syncSettings["SourceQuery"];
        if (string.IsNullOrWhiteSpace(sourceQueryTemplate))
        {
            throw new InvalidOperationException("SourceQuery must be configured.");
        }

        string sourceQuery = sourceQueryTemplate.Replace("{SourceTable}", sourceTable);

        DateTime epoch = new DateTime(1970, 1, 1, 7, 0, 0, DateTimeKind.Utc);
        DateTime endDate = DateTime.Now;
        DateTime startDate = endDate.AddDays(-Math.Abs(daysToSync));

        long startSeconds = (long)(startDate - epoch).TotalSeconds;
        long endSeconds = (long)(endDate - epoch).TotalSeconds;

        Log.Information("Starting synchronization cycle for {SourceTable} -> {TargetTable} covering {Start} to {End}.", sourceTable, targetTable, startDate, endDate);

        await Task.Run(() => SyncData(sourceConnStr, targetConnStr, targetTable, startSeconds, endSeconds, uniqueColumns, sourceQuery, cancellationToken), cancellationToken).ConfigureAwait(false);

        Log.Information("Synchronization cycle completed.");
    }

    private static void SyncData(string sourceConnStr, string targetConnStr, string targetTable, long startDate, long endDate, string[] uniqueColumns, string sourceQuery, CancellationToken cancellationToken)
    {
        using var sourceConn = new OracleConnection(sourceConnStr);
        using var targetConn = new OracleConnection(targetConnStr);

        sourceConn.Open();
        targetConn.Open();

        Log.Information("SyncData: begin copying into {TargetTable}.", targetTable);

        using var cmd = new OracleCommand(sourceQuery, sourceConn)
        {
            BindByName = true
        };

        cmd.Parameters.Add(new OracleParameter("startDate", OracleDbType.Long) { Value = startDate });
        cmd.Parameters.Add(new OracleParameter("endDate", OracleDbType.Long) { Value = endDate });

        using var reader = cmd.ExecuteReader(CommandBehavior.SequentialAccess);

        int insertedCount = 0;
        int duplicateCount = 0;
        int processedCount = 0;

        while (reader.Read())
        {
            cancellationToken.ThrowIfCancellationRequested();
            processedCount++;

            var uniqueValues = uniqueColumns.Select(col => reader[col]).ToArray();
            string whereClause = string.Join(" AND ", uniqueColumns.Select((col, i) => $"{col} = :val{i}"));
            string checkQuery = $"SELECT COUNT(*) FROM {targetTable} WHERE {whereClause}";

            using var checkCmd = new OracleCommand(checkQuery, targetConn)
            {
                BindByName = true
            };

            for (int i = 0; i < uniqueColumns.Length; i++)
            {
                var value = uniqueValues[i] is DBNull ? DBNull.Value : uniqueValues[i];
                checkCmd.Parameters.Add(new OracleParameter($"val{i}", value));
            }

            int count = Convert.ToInt32(checkCmd.ExecuteScalar());

            if (count == 0)
            {
                var columns = new List<string>();
                var values = new List<string>();
                var parameters = new List<OracleParameter>();

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string colName = reader.GetName(i);
                    columns.Add(colName);
                    values.Add($":{colName}");
                    object? rawValue = reader.IsDBNull(i) ? DBNull.Value : reader.GetValue(i);
                    parameters.Add(new OracleParameter(colName, rawValue));
                }

                string insertQuery = $"INSERT INTO {targetTable} ({string.Join(",", columns)}) VALUES ({string.Join(",", values)})";

                using var insertCmd = new OracleCommand(insertQuery, targetConn)
                {
                    BindByName = true
                };

                insertCmd.Parameters.AddRange(parameters.ToArray());
                insertCmd.ExecuteNonQuery();

                insertedCount++;
            }
            else
            {
                duplicateCount++;
            }
        }

        Log.Information(
            "SyncData: finished for {TargetTable}. Processed {Processed} rows, inserted {Inserted}, skipped {Duplicates} existing rows.",
            targetTable,
            processedCount,
            insertedCount,
            duplicateCount);
    }

    private static void EnsureSingleInstance()
    {
        try
        {
            var current = Process.GetCurrentProcess();
            var duplicates = Process.GetProcessesByName(current.ProcessName)
                .Where(p => p.Id != current.Id)
                .ToList();

            Log.Information("Found {ProcessName} other instance(s) of SyncDataOracle running.", current.ProcessName);

            var duplicatePids = duplicates.Select(p => p.Id).ToArray();

            foreach (var process in duplicates)
            {
                process.Dispose();
            }

            if (duplicatePids.Length > 0)
            {
                Log.Warning("Another instance of SyncDataOracle is already running (PID(s): {Pids}). Exiting current instance.", string.Join(",", duplicatePids));
                Environment.Exit(0);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to verify single instance.");
            throw;
        }
    }
}
