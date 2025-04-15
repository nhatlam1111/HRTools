using AttendanceAccessToOracle.classes;
using Helpers;
using Helpers.controllers;
using Serilog.Context;
using System;
using System.Data;
using System.Timers;
using static System.Windows.Forms.AxHost;

namespace AttendanceAccessToOracle.controllers
{
    public static class MainController
    {
        public static Form mainForm;
        public static DataGridView mainGridView;
        public static SqlTemplates sqlTemplates = new SqlTemplates();
        public static Config config = new Config();
        public static bool isRunning = false;

        public static System.Timers.Timer timerSyncAttendance;
        public static System.Timers.Timer timerSyncUser;

        internal static void Start()
        {
            LoadSqlTemplates();
            LogController.Information("Start Sync data", true);
            UpdateUIMessage("Start sync data", null);

            DelegateController.UpdateUIMessage = UpdateUIMessage;
            string accessMode = config.SyncUser ? "ReadWrite" : "Read";

            OracleDb.connectionString = $"Data Source={Helper.clientList[config.Client].Replace("\r\n", "").Replace(" ", "")};User Id={config.DbUser};Password={config.DbPass};";
            SqlServerDb.connectionString = Helper.ReplaceText(SqlServerDb.connectionString, config);

            OracleDb.Connect("", true);
            SqlServerDb.Connect("", true);

            if (config.SyncAttendance)
            {
                timerSyncAttendance = new System.Timers.Timer();
                timerSyncAttendance.Interval = config.SyncMinutes * 60 * 1000;
                timerSyncAttendance.Elapsed += TimerSyncAttendance_tick;
                timerSyncAttendance.Start();

                Task.Run(() => TimerSyncAttendance_tick(null, null));
                
            }

            if (config.SyncUser)
            {
                timerSyncUser = new System.Timers.Timer();
                timerSyncUser.Interval = config.SyncMinutes * 60 * 1000;
                timerSyncUser.Elapsed += TimerSyncUsers_tick;
                timerSyncUser.Start();

                Task.Run(() => TimerSyncUsers_tick(null, null));
            }
        }

        internal static void Stop()
        {
            LogController.Information("Stop Sync data", true);
            UpdateUIMessage("Stop Sync data", null);

            OracleDb.Close();
            SqlServerDb.Close();
            if (timerSyncAttendance != null)
            {
                timerSyncAttendance.Stop();
                timerSyncAttendance.Dispose();
            }
            if (timerSyncUser != null)
            {
                timerSyncUser.Stop();
                timerSyncUser.Dispose();
            }
        }

        private static void LoadSqlTemplates()
        {
            if (!File.Exists(config.SqlPath))
            {
                MessageBox.Show($"Sql template not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            sqlTemplates = Helper.ReadObjectFromFile<SqlTemplates>(config.SqlPath, false);
        }

        public static void UpdateUIMessage(string message, string tag)
        {
            if (mainForm != null && mainGridView != null)
            {
                ThreadController.SetGridView(mainForm, mainGridView, new string[] { DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), message }, string.IsNullOrEmpty(tag) ? DateTime.Now.Ticks.ToString() : tag);
            }
        }

        private static async void TimerSyncAttendance_tick(object sender, ElapsedEventArgs e)
        {
            try
            {
                if(OracleDb.State == ConnectionState.Broken || OracleDb.State == ConnectionState.Closed)
                {
                    LogController.Error("Cannot connect to server.");
                    UpdateUIMessage("Cannot connect to server", null);
                    return;
                }

                List<AttendanceModel> attendanceListAccess = new List<AttendanceModel>();
                List<AttendanceModel> attendanceListOracle = new List<AttendanceModel>();

                string fromdt = DateTime.Now.AddDays(-config.SyncDays).ToString("yyyyMMdd").ToString();
                string todt = DateTime.Now.ToString("yyyyMMdd").ToString();
                string sqlAccess = sqlTemplates.ACCESS_SELECT_ATTENDANCE.Replace("$[from_dt]", fromdt).Replace("$[to_dt]", todt);
                string sqlOracle = sqlTemplates.ORACLE_SELECT_ATTENDANCE.Replace("$[from_dt]", fromdt).Replace("$[to_dt]", todt);

                List<string> sqlList = new List<string>();
                UpdateUIMessage("Start sync Attendance", null);
                

                var dt = await SqlServerDb.excuteSQLAsync(sqlAccess);
                attendanceListAccess = Helper.DatatableToList<AttendanceModel>(dt);

                var dtOracle = await OracleDb.excuteSQLAsync(sqlOracle);
                attendanceListOracle = Helper.DatatableToList<AttendanceModel>(dtOracle);

                attendanceListAccess = attendanceListAccess.Where(q => !attendanceListOracle.Any(p => p.USER_ID == q.USER_ID && p.WORK_DATE == q.WORK_DATE && q.WORK_TIME == p.WORK_TIME)).ToList();

                if (attendanceListAccess != null && attendanceListAccess.Count > 0)
                {
                    string sqlInsert = sqlTemplates.ORACLE_INSERT_ATTENDANCE;
                    foreach (var item in attendanceListAccess)
                    {
                       sqlList.Add(Helper.ReplaceText(sqlInsert, item));
                    }
                    
                    await OracleDb.excuteSQLCommandBatchAsync(sqlList);
                    
                }
                else
                {
                    LogController.Information("No data to sync.");
                    UpdateUIMessage("No data to sync.", null);
                }

            }
            catch (Exception ex)
            {
                LogController.Error(ex.ToString());
                UpdateUIMessage(ex.Message, null);
            }
            finally
            {
                LogController.Information("End sync Attendance.");
                UpdateUIMessage("End sync Attendance", null);
            }
        }

        private static async void TimerSyncUsers_tick(object sender, ElapsedEventArgs e)
        {

        }
    }
}
