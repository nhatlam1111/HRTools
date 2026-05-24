using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SyncTableOracle.Configuration;
using SyncTableOracle.Services;

var configuration = BuildConfiguration();
using var loggerFactory = CreateLoggerFactory(configuration);
var bootstrapLogger = loggerFactory.CreateLogger("Bootstrap");

var syncSettings = configuration.GetSection("SyncSettings").Get<SyncSettings>() ?? new SyncSettings();
bootstrapLogger.LogInformation("Loaded synchronization settings for table {Table}.", syncSettings.Table.Name);

var cts = CreateCancellationTokenSource();

var dataSyncLogger = loggerFactory.CreateLogger<OracleDataSyncService>();
var schedulerLogger = loggerFactory.CreateLogger<SyncScheduler>();

var syncService = new OracleDataSyncService(syncSettings, dataSyncLogger);
await using var scheduler = new SyncScheduler(syncService, schedulerLogger, syncSettings);

try
{
	await scheduler.RunAsync(cts.Token).ConfigureAwait(false);
}
catch (OperationCanceledException)
{
	bootstrapLogger.LogInformation("Application cancellation requested. Shutting down...");
}
catch (Exception ex)
{
	bootstrapLogger.LogCritical(ex, "Application terminated due to an unhandled exception.");
	Environment.ExitCode = -1;
}
finally
{
	bootstrapLogger.LogInformation("Application stopped.");
}

static IConfigurationRoot BuildConfiguration()
{
	var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production";
	var builder = new ConfigurationBuilder()
		.SetBasePath(AppContext.BaseDirectory)
		.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
		.AddJsonFile($"appsettings.{environment}.json", optional: true)
		.AddEnvironmentVariables();

	return builder.Build();
}

static ILoggerFactory CreateLoggerFactory(IConfiguration configuration)
{
	return LoggerFactory.Create(builder =>
	{
		builder.AddConfiguration(configuration.GetSection("Logging"));
		builder.AddConsole();
	});
}

static CancellationTokenSource CreateCancellationTokenSource()
{
	var cts = new CancellationTokenSource();
	Console.CancelKeyPress += (_, eventArgs) =>
	{
		eventArgs.Cancel = true;
		cts.Cancel();
	};

	return cts;
}
