using Serilog;

namespace HRImportData.Controllers
{
    public static class LogController
    {
        public static Form DisplayForm;
        public static TextBox DisplayText;
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
