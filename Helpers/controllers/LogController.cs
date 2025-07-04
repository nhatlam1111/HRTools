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

        public static void Information(string messageTemplate)
        { 
            Log.Information(messageTemplate);
            if(DisplayForm != null && DisplayText != null) ThreadController.SetText(DisplayForm, DisplayText, messageTemplate);
        }

        public static void Information<T>(string messageTemplate, T propertyValue)
        {
            Log.Information(messageTemplate, propertyValue);
            if (DisplayForm != null && DisplayText != null) ThreadController.SetText(DisplayForm, DisplayText, messageTemplate);
        }

        public static void Information(string messageTemplate, bool writeToLogFile)
        {
            if (writeToLogFile) Log.Information(messageTemplate);
            if (DisplayForm != null && DisplayText != null) ThreadController.SetText(DisplayForm, DisplayText, messageTemplate);
        }

        public static void Information<T>(string messageTemplate, T propertyValue, bool writeToLogFile)
        {
            if(writeToLogFile) Log.Information(messageTemplate, propertyValue);
            if (DisplayForm != null && DisplayText != null) ThreadController.SetText(DisplayForm, DisplayText, messageTemplate);
        }

        public static void InformationBackground(string messageTemplate)
        {
            Log.Information(messageTemplate);
        }

        public static void InformationBackground<T>(string messageTemplate, T propertyValue)
        {
            Log.Information(messageTemplate, propertyValue);
        }

        public static void InformationBackground(string messageTemplate, bool writeToLogFile)
        {
            if (writeToLogFile) Log.Information(messageTemplate);
        }

        public static void InformationBackground<T>(string messageTemplate, T propertyValue, bool writeToLogFile)
        {
            if (writeToLogFile) Log.Information(messageTemplate, propertyValue);
        }

        public static void Error(string messageTemplate)
        {
            Log.Error(messageTemplate);
            if (DisplayForm != null && DisplayText != null) ThreadController.SetText(DisplayForm, DisplayText, messageTemplate);
        }

        public static void Error<T>(string messageTemplate, T propertyValue)
        {
            Log.Error(messageTemplate, propertyValue);
            if (DisplayForm != null && DisplayText != null) ThreadController.SetText(DisplayForm, DisplayText, messageTemplate);
        }

        public static void Error(string messageTemplate, bool writeToLogFile)
        {
            if (writeToLogFile) Log.Error(messageTemplate);
            if (DisplayForm != null && DisplayText != null) ThreadController.SetText(DisplayForm, DisplayText, messageTemplate);
        }

        public static void Error<T>(string messageTemplate, T propertyValue, bool writeToLogFile)
        {
            if (writeToLogFile) Log.Error(messageTemplate, propertyValue);
            if (DisplayForm != null && DisplayText != null) ThreadController.SetText(DisplayForm, DisplayText, messageTemplate);
        }

        public static void ErrorBackground(string messageTemplate)
        {
            Log.Error(messageTemplate);
        }

        public static void ErrorBackground<T>(string messageTemplate, T propertyValue)
        {
            Log.Error(messageTemplate, propertyValue);
        }

        public static void ErrorBackground(string messageTemplate, bool writeToLogFile)
        {
            if (writeToLogFile) Log.Error(messageTemplate);
        }

        public static void ErrorBackground<T>(string messageTemplate, T propertyValue, bool writeToLogFile)
        {
            if (writeToLogFile) Log.Error(messageTemplate, propertyValue);
        }

        public static void Warning(string messageTemplate)
        {
            Log.Warning(messageTemplate);
            if (DisplayForm != null && DisplayText != null) ThreadController.SetText(DisplayForm, DisplayText, messageTemplate);
        }

        public static void Warning<T>(string messageTemplate, T propertyValue)
        {
            Log.Warning(messageTemplate, propertyValue);
            if (DisplayForm != null && DisplayText != null) ThreadController.SetText(DisplayForm, DisplayText, messageTemplate);
        }

        public static void Warning(string messageTemplate, bool writeToLogFile)
        {
            if (writeToLogFile) Log.Warning(messageTemplate);
            if (DisplayForm != null && DisplayText != null) ThreadController.SetText(DisplayForm, DisplayText, messageTemplate);
        }

        public static void Warning<T>(string messageTemplate, T propertyValue, bool writeToLogFile)
        {
            if (writeToLogFile) Log.Warning(messageTemplate, propertyValue);
            if (DisplayForm != null && DisplayText != null) ThreadController.SetText(DisplayForm, DisplayText, messageTemplate);
        }

        public static void WarningBackground(string messageTemplate)
        {
            Log.Warning(messageTemplate);
        }

        public static void WarningBackground<T>(string messageTemplate, T propertyValue)
        {
            Log.Warning(messageTemplate, propertyValue);
        }

        public static void WarningBackground(string messageTemplate, bool writeToLogFile)
        {
            if (writeToLogFile) Log.Warning(messageTemplate);
        }

        public static void WarningBackground<T>(string messageTemplate, T propertyValue, bool writeToLogFile)
        {
            if (writeToLogFile) Log.Warning(messageTemplate, propertyValue);
        }
    }
}
