using Microsoft.Extensions.Configuration;
using Serilog;
using SyncDataSqlToWebApi.Models;
using SyncDataSqlToWebApi.Services;
using System.CommandLine;

namespace SyncDataSqlToWebApi;

class Program
{
    static async Task<int> Main(string[] args)
    {
        // Check if this is a state-tool command
        if (args.Length > 0 && args[0] == "state-tool")
        {
            return await Tools.StateManagerTool.ExecuteAsync(args.Skip(1).ToArray());
        }

        // Normal sync operation
        return await RunSyncAsync();
    }

    static async Task<int> RunSyncAsync()
    {
        try
        {
            // Load configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // Configure Serilog
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.File(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs", "sync-.log"),
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 30,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();

            Log.Information("===========================================");
            Log.Information("SyncDataSqlToWebApi started");
            Log.Information("===========================================");

            // Load settings
            var sqlConnectionString = configuration["SqlServer:ConnectionString"] 
                ?? throw new InvalidOperationException("SQL Server connection string not configured");

            var apiBaseUrl = configuration["ApiSettings:BaseUrl"] 
                ?? throw new InvalidOperationException("API base URL not configured");
            var apiEndpoint = configuration["ApiSettings:Endpoint"] ?? "dso/bulkinsertpro";
            var maxRowPerRequest = configuration.GetValue<int>("ApiSettings:MaxRowPerRequest", 500);
            var timeoutSeconds = configuration.GetValue<int>("ApiSettings:TimeoutSeconds", 120);

            var stateDbPath = configuration["SyncStateSettings:DatabasePath"] ?? "sync-state.db";
            var enableTracking = configuration.GetValue<bool>("SyncStateSettings:EnableTracking", true);
            var retentionDays = configuration.GetValue<int>("SyncStateSettings:RetentionDays", 30);
            var autoCleanup = configuration.GetValue<bool>("SyncStateSettings:AutoCleanup", true);
            var hashAlgorithm = configuration["SyncStateSettings:HashAlgorithm"] ?? "SHA256";

            var intervalMinutes = configuration.GetValue<int>("SchedulerSettings:IntervalMinutes", 0);
            var enableScheduler = configuration.GetValue<bool>("SchedulerSettings:EnableScheduler", false);
            var runOnStartup = configuration.GetValue<bool>("SchedulerSettings:RunOnStartup", true);

            // Load sync jobs
            var jobs = configuration.GetSection("SyncJobs").Get<List<SyncJob>>() 
                ?? new List<SyncJob>();

            if (jobs.Count == 0)
            {
                Log.Warning("No sync jobs configured");
                return 0;
            }

            // Initialize services
            using var sqlService = new SqlDataService(sqlConnectionString);
            var apiService = new ApiService(apiBaseUrl, apiEndpoint, maxRowPerRequest, timeoutSeconds);
            using var stateManager = new SyncStateManager(stateDbPath, hashAlgorithm);

            // Test SQL connection
            Log.Information("Testing SQL Server connection...");
            if (!await sqlService.TestConnectionAsync())
            {
                Log.Error("Failed to connect to SQL Server. Exiting.");
                return -1;
            }

            // Initialize state database
            if (enableTracking)
            {
                Log.Information("Initializing sync state database...");
                await stateManager.InitializeDatabaseAsync();
            }

            var executor = new SyncJobExecutor(sqlService, apiService, stateManager);

            // Setup cancellation token
            using var cts = new CancellationTokenSource();
            Console.CancelKeyPress += (_, eventArgs) =>
            {
                eventArgs.Cancel = true;
                Log.Information("Cancellation requested. Stopping...");
                cts.Cancel();
            };

            // Run sync
            if (runOnStartup)
            {
                await executor.ExecuteAllJobsAsync(jobs);

                // Cleanup old states if enabled
                if (enableTracking && autoCleanup)
                {
                    await stateManager.CleanupOldStatesAsync(retentionDays);
                }
            }

            // Start scheduler if enabled
            if (enableScheduler && intervalMinutes > 0)
            {
                Log.Information("Scheduler enabled. Will run every {Minutes} minutes", intervalMinutes);
                await RunSchedulerAsync(executor, jobs, stateManager, intervalMinutes, retentionDays, autoCleanup, enableTracking, cts.Token);
            }

            Log.Information("SyncDataSqlToWebApi completed");
            return 0;
        }
        catch (OperationCanceledException)
        {
            Log.Information("Operation cancelled by user");
            return 0;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Application error");
            return -1;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    static async Task RunSchedulerAsync(
        SyncJobExecutor executor, 
        List<SyncJob> jobs, 
        SyncStateManager stateManager,
        int intervalMinutes,
        int retentionDays,
        bool autoCleanup,
        bool enableTracking,
        CancellationToken cancellationToken)
    {
        var interval = TimeSpan.FromMinutes(intervalMinutes);

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                await Task.Delay(interval, cancellationToken);

                if (cancellationToken.IsCancellationRequested)
                    break;

                Log.Information("Scheduler triggered");
                await executor.ExecuteAllJobsAsync(jobs);

                // Cleanup old states
                if (enableTracking && autoCleanup)
                {
                    await stateManager.CleanupOldStatesAsync(retentionDays);
                }
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Scheduler cycle error");
            }
        }

        Log.Information("Scheduler stopped");
    }
}
