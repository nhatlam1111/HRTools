using HelpersNetFramework.classes;
using Serilog;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace Helpers.controllers
{
    public static class LogController
    {
        public static Form DisplayForm;
        public static Control DisplayText;
        public static DataGridView DisplayDataGridview;

        public static BindingList<LogMessage> LogMessages = new BindingList<LogMessage>();

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

            if (DisplayDataGridview != null)
            { 
                Helper.BindListToGrid(LogMessages, DisplayDataGridview, true);
            }
        }

        private static void SetControlMessage(string messageTemplate)
        {
            LogMessages.Insert(0, new LogMessage() { Time = DateTime.Now, Message = messageTemplate });

            if(LogMessages.Count > 500)
            {
                LogMessages.RemoveAt(LogMessages.Count - 1); // Keep the last 500 messages
            }

            if (DisplayForm != null && DisplayText != null) ThreadController.SetText(DisplayForm, DisplayText, messageTemplate);
        }

        public static void Information(string messageTemplate)
        { 
            Log.Information(messageTemplate);
            SetControlMessage(messageTemplate);
        }

        public static void Information<T>(string messageTemplate, T propertyValue)
        {
            Log.Information(messageTemplate, propertyValue);
            SetControlMessage(messageTemplate);
        }

        public static void Information(string messageTemplate, bool writeToLogFile)
        {
            if (writeToLogFile) Log.Information(messageTemplate);
            SetControlMessage(messageTemplate);
        }

        public static void Information<T>(string messageTemplate, T propertyValue, bool writeToLogFile)
        {
            if(writeToLogFile) Log.Information(messageTemplate, propertyValue);
            SetControlMessage(messageTemplate);
        }

        public static void Error(string messageTemplate)
        {
            Log.Error(messageTemplate);
            SetControlMessage(messageTemplate);
        }

        public static void Error<T>(string messageTemplate, T propertyValue)
        {
            Log.Error(messageTemplate, propertyValue);
            SetControlMessage(messageTemplate);
        }

        public static void Error(string messageTemplate, bool writeToLogFile)
        {
            if (writeToLogFile) Log.Error(messageTemplate);
            SetControlMessage(messageTemplate);
        }

        public static void Error<T>(string messageTemplate, T propertyValue, bool writeToLogFile)
        {
            if (writeToLogFile) Log.Error(messageTemplate, propertyValue);
            SetControlMessage(messageTemplate);
        }

        public static void Warning(string messageTemplate)
        {
            Log.Warning(messageTemplate);
            SetControlMessage(messageTemplate);
        }

        public static void Warning<T>(string messageTemplate, T propertyValue)
        {
            Log.Warning(messageTemplate, propertyValue);
            SetControlMessage(messageTemplate);
        }

        public static void Warning(string messageTemplate, bool writeToLogFile)
        {
            if (writeToLogFile) Log.Warning(messageTemplate);
            SetControlMessage(messageTemplate);
        }

        public static void Warning<T>(string messageTemplate, T propertyValue, bool writeToLogFile)
        {
            if (writeToLogFile) Log.Warning(messageTemplate, propertyValue);
            SetControlMessage(messageTemplate);
        }
    }
}
