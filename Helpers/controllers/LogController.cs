using Serilog;


namespace Helpers.controllers
{
    public static class LogController
    {
        public static Form DisplayForm;
        public static Control DisplayText;

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

        public static async void Information(string messageTemplate)
        { 
            Log.Information(messageTemplate);
            if(DisplayForm != null && DisplayText != null) ThreadController.SetText(DisplayForm, DisplayText, messageTemplate);
        }

        public static async void Information<T>(string messageTemplate, T propertyValue)
        {
            Log.Information(messageTemplate, propertyValue);
            if (DisplayForm != null && DisplayText != null) ThreadController.SetText(DisplayForm, DisplayText, messageTemplate);
        }

        public static async void Information(string messageTemplate, bool writeToLogFile)
        {
            if (writeToLogFile) Log.Information(messageTemplate);
            if (DisplayForm != null && DisplayText != null) ThreadController.SetText(DisplayForm, DisplayText, messageTemplate);
        }

        public static async void Information<T>(string messageTemplate, T propertyValue, bool writeToLogFile)
        {
            if(writeToLogFile) Log.Information(messageTemplate, propertyValue);
            if (DisplayForm != null && DisplayText != null) ThreadController.SetText(DisplayForm, DisplayText, messageTemplate);
        }

        public static async void Error(string messageTemplate)
        {
            Log.Error(messageTemplate);
            if (DisplayForm != null && DisplayText != null) ThreadController.SetText(DisplayForm, DisplayText, messageTemplate);
        }

        public static async void Error<T>(string messageTemplate, T propertyValue)
        {
            Log.Error(messageTemplate, propertyValue);
            if (DisplayForm != null && DisplayText != null) ThreadController.SetText(DisplayForm, DisplayText, messageTemplate);
        }

        public static async void Error(string messageTemplate, bool writeToLogFile)
        {
            if (writeToLogFile) Log.Error(messageTemplate);
            if (DisplayForm != null && DisplayText != null) ThreadController.SetText(DisplayForm, DisplayText, messageTemplate);
        }

        public static async void Error<T>(string messageTemplate, T propertyValue, bool writeToLogFile)
        {
            if (writeToLogFile) Log.Error(messageTemplate, propertyValue);
            if (DisplayForm != null && DisplayText != null) ThreadController.SetText(DisplayForm, DisplayText, messageTemplate);
        }

        public static async void Warning(string messageTemplate)
        {
            Log.Warning(messageTemplate);
            if (DisplayForm != null && DisplayText != null) ThreadController.SetText(DisplayForm, DisplayText, messageTemplate);
        }

        public static async void Warning<T>(string messageTemplate, T propertyValue)
        {
            Log.Warning(messageTemplate, propertyValue);
            if (DisplayForm != null && DisplayText != null) ThreadController.SetText(DisplayForm, DisplayText, messageTemplate);
        }

        public static async void Warning(string messageTemplate, bool writeToLogFile)
        {
            if (writeToLogFile) Log.Warning(messageTemplate);
            if (DisplayForm != null && DisplayText != null) ThreadController.SetText(DisplayForm, DisplayText, messageTemplate);
        }

        public static async void Warning<T>(string messageTemplate, T propertyValue, bool writeToLogFile)
        {
            if (writeToLogFile) Log.Warning(messageTemplate, propertyValue);
            if (DisplayForm != null && DisplayText != null) ThreadController.SetText(DisplayForm, DisplayText, messageTemplate);
        }
    }
}
