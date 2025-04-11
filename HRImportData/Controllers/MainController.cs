using HRImportData.Classes;
using HRImportData.Forms;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using NPOI.XSSF.UserModel;

namespace HRImportData.Controllers
{
    internal static class MainController
    {
        public delegate bool WriteErrorLogDelegate(string message, bool writeToLogFile);
        public delegate Task WriteInformationMessageDelegate(string message, bool writeToLogFile);
        public delegate string SaveWorkbookDelegate(XSSFWorkbook workbook, string fileName);



        public static WriteErrorLogDelegate WriteError = WriteErrorCallback;
        public static WriteInformationMessageDelegate WriteInformation = WriteMessageCallback;
        public static SaveWorkbookDelegate SaveWorkbook = SaveAsWorkbook;


        public static bool isConnectDatabase = false;
        public static bool isSiteLogin = false;
        public static frmProcessing CurrentFormProcessing;
        public static LoginInfo currentLogin;

        public static Dictionary<IMPORT_TYPE, string> importFunctions = new Dictionary<IMPORT_TYPE, string>()
        {
            {IMPORT_TYPE.SELECT_IMPORT_TYPE, "Select function ..."},
            //{IMPORT_TYPE.UPDATE_EMPLOYEE, "Update Employee"},
            {IMPORT_TYPE.UPDATE_SALARY, "Update Salary"},
           // {IMPORT_TYPE.INSERT_EMPLOYEE, "Insert Employee"},
            {IMPORT_TYPE.INSERT_TIME_TEMP, "Insert Time Temp"},

            {IMPORT_TYPE.INSERT_TABLE, "Insert Table"},
            {IMPORT_TYPE.UPDATE_TABLE, "Update Table"}
        };

        public static void ReleaseMemory()
        {
            try
            {
                //long before = GC.GetTotalMemory(false);

                GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true, true);
                GC.WaitForPendingFinalizers();
                GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true, true);
                

                //long after = GC.GetTotalMemory(false);
                //MessageBox.Show($"Memory before GC: {before / 1024} KB\nMemory after GC: {after / 1024} KB", "Memory Usage", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Error releasing memory: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void EnableControl(Control con, bool enable)
        {
            foreach (Control c in con.Controls)
            {
                EnableControl(c, enable);
            }

            con.Enabled = enable;
        }

        public static async void CheckConnectDatabase()
        {
            isConnectDatabase = await DatabaseHelper.Connect();            
        }

        public static async void CheckConnectDatabase(string connectionString)
        {
            isConnectDatabase = await DatabaseHelper.Connect(connectionString);
        }


        public static async void Login(LoginInfo loginInfo)
        {
            string sql = "";
            switch (loginInfo.SiteVersion)
            {
                case SITE_VERSION.NODEJS: sql = SQLTemplates.ORACLE_USER_LOGIN_NODEJS; break;
                case SITE_VERSION.GASP: sql = SQLTemplates.ORACLE_USER_LOGIN_GASP; break;
                case SITE_VERSION.ESYS: sql = SQLTemplates.ORACLE_USER_LOGIN_ESYS; break;
            }

            sql = string.Format(sql, loginInfo.SiteUserName, loginInfo.SiteUserPass);

            var dt = await DatabaseHelper.excuteSQLAsync(sql);

            if (dt != null && dt.Rows.Count > 0)
            {
                var pass = dt.Rows[0][0].ToString();

                if (!pass.Equals(EncryptionHelper.GetMD5Base64Hash(loginInfo.SiteUserPass)))
                {
                    var verified = BCrypt.Net.BCrypt.Verify(loginInfo.SiteUserPass, pass);

                    if (verified) isSiteLogin = true;
                    else isSiteLogin = false;
                }
                else
                {
                    isSiteLogin = true;
                    currentLogin = loginInfo;
                }
            }
            else
            {
                isSiteLogin = false;
            }

        }

        public static List<SiteInfo> SiteVersions()
        {
            List<SiteInfo> list = new List<SiteInfo>() {
                new SiteInfo{ value = SITE_VERSION.NODEJS, display = "NODEJS" },
                new SiteInfo{ value = SITE_VERSION.GASP, display = "GASP" },
                new SiteInfo{ value = SITE_VERSION.ESYS, display = "ESYS" },
            };

            return list;
        }

        private static bool WriteErrorCallback(string message, bool writeToLogFile)
        {
            LogController.Error(message, writeToLogFile);
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }

        private static async Task WriteMessageCallback(string message, bool writeToLogFile)
        {
            if (CurrentFormProcessing == null) return;

            CurrentFormProcessing.SetMessage(message);
            LogController.Information(message, writeToLogFile);

            await Task.Delay(100);
        }

        private static string SaveAsWorkbook(XSSFWorkbook workbook, string fileName)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel Workbook (*.xlsx)|*.xlsx";
            saveFileDialog.Title = "Save as";
            saveFileDialog.FileName = fileName;
            var result = saveFileDialog.ShowDialog();

            if(result == DialogResult.OK)
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
