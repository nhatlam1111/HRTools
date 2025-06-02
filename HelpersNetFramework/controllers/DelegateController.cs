using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Helpers.controllers
{
    public static class DelegateController
    {
        public delegate void WriteErrorLogDelegate(string message, bool writeToLogFile);
        public delegate void WriteInformationMessageDelegate(string message, bool writeToLogFile, string tag);
        public delegate string SaveWorkbookDelegate(XSSFWorkbook workbook, string fileName);
        public delegate void WriteUIMessageDelegate(string message, string tag);



        public static WriteErrorLogDelegate WriteError = WriteErrorCallback;
        public static WriteInformationMessageDelegate WriteInformation = WriteMessageCallback;
        public static SaveWorkbookDelegate SaveWorkbook = SaveAsWorkbook;
        public static WriteUIMessageDelegate UpdateUIMessage = null;


        private static async void WriteErrorCallback(string message, bool writeToLogFile)
        {
            LogController.Error(message, writeToLogFile);
        }

        private static async void WriteMessageCallback(string message, bool writeToLogFile, string tag)
        {
            LogController.Information(message, writeToLogFile);
            if (UpdateUIMessage != null)
            {
                UpdateUIMessage(message, tag);
            }
        }

        private static string SaveAsWorkbook(XSSFWorkbook workbook, string fileName)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel Workbook (*.xlsx)|*.xlsx";
            saveFileDialog.Title = "Save as";
            saveFileDialog.FileName = fileName;
            var result = saveFileDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                // Saves the Image via a FileStream created by the OpenFile method.
                using (System.IO.FileStream fs = (System.IO.FileStream)saveFileDialog.OpenFile())
                {
                    workbook.Write(fs, false);
                    fs.Close();
                    fs.Dispose();
                }
                return saveFileDialog.FileName;
            }
            else
            {
                return "";
            }
        }
    }
}
