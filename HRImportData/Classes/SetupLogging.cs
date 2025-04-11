using Serilog;

namespace HRImportData.Classes
{
    public static class SetupLogging
    {
        public static void Development()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.File(Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory,
                        "LogFiles", "Log.txt"),
                    rollingInterval: RollingInterval.Day,
                    outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level}] {Message}{NewLine}{Exception}")
                .CreateLogger();

        }

        public static void Start(string _client)
        {
            string client = _client ?? "General"; // Default to "General" if not set

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.File(Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory,
                        "Logs", (string.IsNullOrEmpty(client) ? "general" : client.ToLower()), ".txt"),
                    rollingInterval: RollingInterval.Day,
                    outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level}] {Message}{NewLine}{Exception}")
                .CreateLogger();
        }

    }
}
