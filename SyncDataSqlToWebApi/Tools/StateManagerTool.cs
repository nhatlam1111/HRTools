using System.CommandLine;
using System.Data;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Serilog;
using SyncDataSqlToWebApi.Services;

namespace SyncDataSqlToWebApi.Tools;

public static class StateManagerTool
{
    public static async Task<int> ExecuteAsync(string[] args)
    {
        // Load configuration to get database path
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var stateDbPath = configuration["SyncStateSettings:DatabasePath"] ?? "sync-state.db";

        // Setup CLI commands
        var rootCommand = new RootCommand("Sync State Manager Tool - Manage sync state database");

        // VIEW command
        var viewCommand = new Command("view", "View sync state records");
        var viewJobOption = new Option<string?>("--job", "Job name to filter");
        var viewStatusOption = new Option<string?>("--status", "Status to filter (SUCCESS, FAILED, PENDING)");
        var viewLimitOption = new Option<int>("--limit", () => 100, "Maximum records to display");
        viewCommand.AddOption(viewJobOption);
        viewCommand.AddOption(viewStatusOption);
        viewCommand.AddOption(viewLimitOption);
        viewCommand.SetHandler(async (job, status, limit) =>
        {
            await ViewStateAsync(stateDbPath, job, status, limit);
        }, viewJobOption, viewStatusOption, viewLimitOption);

        // STATS command
        var statsCommand = new Command("stats", "Show statistics for jobs");
        var statsJobOption = new Option<string?>("--job", "Job name (omit for all jobs)");
        statsCommand.AddOption(statsJobOption);
        statsCommand.SetHandler(async (job) =>
        {
            await ShowStatisticsAsync(stateDbPath, job);
        }, statsJobOption);

        // RESET command
        var resetCommand = new Command("reset", "Reset sync state");
        var resetJobOption = new Option<string>("--job", "Job name to reset") { IsRequired = true };
        var resetBeforeOption = new Option<DateTime?>("--before", "Reset records synced before this date");
        var resetAfterOption = new Option<DateTime?>("--after", "Reset records synced after this date");
        var resetHashOption = new Option<string?>("--hash", "Reset specific record by hash");
        var resetAllOption = new Option<bool>("--all", "Reset all records for the job");
        resetCommand.AddOption(resetJobOption);
        resetCommand.AddOption(resetBeforeOption);
        resetCommand.AddOption(resetAfterOption);
        resetCommand.AddOption(resetHashOption);
        resetCommand.AddOption(resetAllOption);
        resetCommand.SetHandler(async (job, before, after, hash, all) =>
        {
            await ResetStateAsync(stateDbPath, job, before, after, hash, all);
        }, resetJobOption, resetBeforeOption, resetAfterOption, resetHashOption, resetAllOption);

        // CLEANUP command
        var cleanupCommand = new Command("cleanup", "Remove old SUCCESS records");
        var cleanupDaysOption = new Option<int>("--days", () => 30, "Remove records older than this many days");
        cleanupCommand.AddOption(cleanupDaysOption);
        cleanupCommand.SetHandler(async (days) =>
        {
            await CleanupAsync(stateDbPath, days);
        }, cleanupDaysOption);

        // EXPORT command
        var exportCommand = new Command("export", "Export state to CSV");
        var exportJobOption = new Option<string>("--job", "Job name to export") { IsRequired = true };
        var exportOutputOption = new Option<string>("--output", "Output file path") { IsRequired = true };
        exportCommand.AddOption(exportJobOption);
        exportCommand.AddOption(exportOutputOption);
        exportCommand.SetHandler(async (job, output) =>
        {
            await ExportStateAsync(stateDbPath, job, output);
        }, exportJobOption, exportOutputOption);

        rootCommand.AddCommand(viewCommand);
        rootCommand.AddCommand(statsCommand);
        rootCommand.AddCommand(resetCommand);
        rootCommand.AddCommand(cleanupCommand);
        rootCommand.AddCommand(exportCommand);

        return await rootCommand.InvokeAsync(args);
    }

    private static async Task ViewStateAsync(string dbPath, string? jobName, string? status, int limit)
    {
        using var connection = new SqliteConnection($"Data Source={dbPath}");
        await connection.OpenAsync();

        var query = "SELECT * FROM SyncStates WHERE 1=1";
        if (!string.IsNullOrEmpty(jobName))
            query += " AND JobName = @jobName";
        if (!string.IsNullOrEmpty(status))
            query += " AND Status = @status";
        query += " ORDER BY SyncedAt DESC LIMIT @limit";

        using var command = connection.CreateCommand();
        command.CommandText = query;
        if (!string.IsNullOrEmpty(jobName))
            command.Parameters.AddWithValue("@jobName", jobName);
        if (!string.IsNullOrEmpty(status))
            command.Parameters.AddWithValue("@status", status);
        command.Parameters.AddWithValue("@limit", limit);

        using var reader = await command.ExecuteReaderAsync();

        Console.WriteLine("\n{0,-5} {1,-20} {2,-45} {3,-25} {4,-10} {5,-5}", 
            "ID", "JobName", "RecordHash", "SyncedAt", "Status", "Retry");
        Console.WriteLine(new string('-', 120));

        int count = 0;
        while (await reader.ReadAsync())
        {
            Console.WriteLine("{0,-5} {1,-20} {2,-45} {3,-25} {4,-10} {5,-5}",
                reader.GetInt32(0),
                reader.GetString(1),
                reader.GetString(2).Substring(0, Math.Min(40, reader.GetString(2).Length)),
                reader.GetDateTime(4).ToString("yyyy-MM-dd HH:mm:ss"),
                reader.GetString(6),
                reader.GetInt32(7));
            count++;
        }

        Console.WriteLine(new string('-', 120));
        Console.WriteLine($"Total: {count} records\n");
    }

    private static async Task ShowStatisticsAsync(string dbPath, string? jobName)
    {
        using var connection = new SqliteConnection($"Data Source={dbPath}");
        await connection.OpenAsync();

        var query = @"
            SELECT 
                JobName,
                COUNT(*) as Total,
                SUM(CASE WHEN Status = 'SUCCESS' THEN 1 ELSE 0 END) as Success,
                SUM(CASE WHEN Status = 'FAILED' THEN 1 ELSE 0 END) as Failed,
                SUM(CASE WHEN Status = 'PENDING' THEN 1 ELSE 0 END) as Pending,
                MIN(SyncedAt) as FirstSync,
                MAX(SyncedAt) as LastSync
            FROM SyncStates
        ";

        if (!string.IsNullOrEmpty(jobName))
            query += " WHERE JobName = @jobName";

        query += " GROUP BY JobName";

        using var command = connection.CreateCommand();
        command.CommandText = query;
        if (!string.IsNullOrEmpty(jobName))
            command.Parameters.AddWithValue("@jobName", jobName);

        using var reader = await command.ExecuteReaderAsync();

        Console.WriteLine("\n{0,-20} {1,-10} {2,-10} {3,-10} {4,-10} {5,-20} {6,-20}", 
            "JobName", "Total", "Success", "Failed", "Pending", "FirstSync", "LastSync");
        Console.WriteLine(new string('-', 120));

        while (await reader.ReadAsync())
        {
            Console.WriteLine("{0,-20} {1,-10} {2,-10} {3,-10} {4,-10} {5,-20} {6,-20}",
                reader.GetString(0),
                reader.GetInt32(1),
                reader.GetInt32(2),
                reader.GetInt32(3),
                reader.GetInt32(4),
                reader.IsDBNull(5) ? "N/A" : reader.GetDateTime(5).ToString("yyyy-MM-dd HH:mm:ss"),
                reader.IsDBNull(6) ? "N/A" : reader.GetDateTime(6).ToString("yyyy-MM-dd HH:mm:ss"));
        }

        Console.WriteLine();
    }

    private static async Task ResetStateAsync(string dbPath, string jobName, DateTime? before, DateTime? after, string? hash, bool all)
    {
        using var connection = new SqliteConnection($"Data Source={dbPath}");
        await connection.OpenAsync();

        var query = "DELETE FROM SyncStates WHERE JobName = @jobName";

        if (!string.IsNullOrEmpty(hash))
        {
            query += " AND RecordHash = @hash";
        }
        else if (!all)
        {
            if (before.HasValue)
                query += " AND SyncedAt < @before";
            if (after.HasValue)
                query += " AND SyncedAt > @after";
        }

        using var command = connection.CreateCommand();
        command.CommandText = query;
        command.Parameters.AddWithValue("@jobName", jobName);
        if (!string.IsNullOrEmpty(hash))
            command.Parameters.AddWithValue("@hash", hash);
        if (before.HasValue)
            command.Parameters.AddWithValue("@before", before.Value);
        if (after.HasValue)
            command.Parameters.AddWithValue("@after", after.Value);

        var deleted = await command.ExecuteNonQueryAsync();
        Console.WriteLine($"Reset complete. {deleted} records deleted for job '{jobName}'");
    }

    private static async Task CleanupAsync(string dbPath, int days)
    {
        using var connection = new SqliteConnection($"Data Source={dbPath}");
        await connection.OpenAsync();

        var cutoffDate = DateTime.Now.AddDays(-days);
        using var command = connection.CreateCommand();
        command.CommandText = @"
            DELETE FROM SyncStates 
            WHERE SyncedAt < @cutoffDate AND Status = 'SUCCESS'
        ";
        command.Parameters.AddWithValue("@cutoffDate", cutoffDate);

        var deleted = await command.ExecuteNonQueryAsync();
        Console.WriteLine($"Cleanup complete. {deleted} old SUCCESS records deleted (older than {days} days)");
    }

    private static async Task ExportStateAsync(string dbPath, string jobName, string outputPath)
    {
        using var connection = new SqliteConnection($"Data Source={dbPath}");
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT JobName, RecordHash, RecordKeys, SyncedAt, Status, RetryCount, LastError
            FROM SyncStates 
            WHERE JobName = @jobName
            ORDER BY SyncedAt DESC
        ";
        command.Parameters.AddWithValue("@jobName", jobName);

        using var reader = await command.ExecuteReaderAsync();
        using var writer = new StreamWriter(outputPath);

        // Write header
        await writer.WriteLineAsync("JobName,RecordHash,RecordKeys,SyncedAt,Status,RetryCount,LastError");

        int count = 0;
        while (await reader.ReadAsync())
        {
            var line = string.Join(",",
                reader.GetString(0),
                reader.GetString(1),
                $"\"{reader.GetString(2).Replace("\"", "\"\"")}\"",
                reader.GetDateTime(3).ToString("yyyy-MM-dd HH:mm:ss"),
                reader.GetString(4),
                reader.GetInt32(5),
                reader.IsDBNull(6) ? "" : $"\"{reader.GetString(6).Replace("\"", "\"\"")}\"");

            await writer.WriteLineAsync(line);
            count++;
        }

        Console.WriteLine($"Exported {count} records to {outputPath}");
    }
}
