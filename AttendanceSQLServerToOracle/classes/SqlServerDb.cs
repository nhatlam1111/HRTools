using Helpers.controllers;
using Microsoft.Data.SqlClient;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Timers;
namespace AttendanceAccessToOracle.classes
{
    public static class SqlServerDb
    {
        public static string connectionString = "Server=$[SqlHost];Database=$[SqlService];User Id=$[SqlUserName];Password=$[SqlPassword];TrustServerCertificate=True";
        
        private static  SqlConnection _con;
        private static System.Timers.Timer timerKeepConnect;
        public static ConnectionState State
        {
            get { return _con == null ? ConnectionState.Closed : _con.State; }
        }

        public static async Task Connect(string _connectionString, bool keepConnect)
        {
            _con = new SqlConnection();
            _con.ConnectionString = string.IsNullOrEmpty(_connectionString) ? connectionString : _connectionString;

            try
            {
                await _con.OpenAsync();
                if (keepConnect)
                {
                    timerKeepConnect = new System.Timers.Timer();
                    timerKeepConnect.Interval = 1000 * 60 * 5;
                    timerKeepConnect.Elapsed += TimerKeepConnect_Elapsed;
                    timerKeepConnect.Start();
                }
            }
            catch (Exception e)
            {
                Close();
                LogController.Error("Database: Connect; ERROR: " + e.Message);
            }
        }

        private static async void TimerKeepConnect_Elapsed(object? sender, ElapsedEventArgs e)
        {
            if (_con != null && _con.State == ConnectionState.Open)
            {
                try
                {
                    if (State == ConnectionState.Broken || State == ConnectionState.Closed)
                    {
                        await Connect("");
                        LogController.Information("Database: KeepConnectionAlive - Reconnect", true);
                    }
                }
                catch (Exception ee)
                {
                    LogController.Error("Database: KeepConnectionAlive - Reconnect; ERROR: " + ee.Message);
                }
            }

        }

        public static async Task<bool> Connect(string _connectionString)
        {
            _con = new SqlConnection();
            _con.ConnectionString = string.IsNullOrEmpty(_connectionString) ? connectionString : _connectionString;

            try
            {
                await _con.OpenAsync();
                return true;
            }
            catch (Exception e)
            {
                Close();
                return false;
            }

        }

        public static async Task Close()
        {
            if (_con != null)
            {
                await _con.CloseAsync();
                await _con.DisposeAsync();
                _con = null;

                if (timerKeepConnect != null)
                {
                    timerKeepConnect.Stop();
                    timerKeepConnect.Dispose();
                    timerKeepConnect = null;
                }
            }
        }

        public static async Task<DataTable> excuteSQLAsync(string sql)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = _con;
                cmd.CommandText = sql;
                DbDataReader dr = await cmd.ExecuteReaderAsync();
                dt.Load(dr);
            }
            catch (Exception e)
            {
                string err = e.Message;
                LogController.Error("excuteSQLAsync: " + sql + ": " + err, true);
            }

            return dt;
        }

        public static async Task<bool> excuteSQLCommandAsync(string sql)
        {
            SqlCommand command = _con.CreateCommand();
            command.Connection = _con;

            command.CommandText = sql;

            try
            {
                await command.ExecuteNonQueryAsync();
                //LogController.Information("excuteSQLCommandAsync: "+sql, true);
            }
            catch (Exception e)
            {
                string err = e.Message;
                LogController.Error("excuteSQLCommandAsync: " + sql + ": " + err, true);
                return false;
            }

            return true;
        }

        public static async Task<bool> excuteSQLCommandBatchAsync(List<string> listSQL)
        {
            using (SqlTransaction transaction = _con.BeginTransaction())
            {
                LogController.Information("Start transaction: ", true);
                string tag = DateTime.Now.Ticks.ToString();
                for (int i = 0; i < listSQL.Count; i++)
                {
                    string sqlItem = listSQL[i];
                    try
                    {
                        SqlCommand command = _con.CreateCommand();
                        command.Connection = _con;
                        command.CommandText = sqlItem;
                        await command.ExecuteNonQueryAsync();
                        LogController.Information(sqlItem, true);
                    }
                    catch (Exception e)
                    {
                        string err = e.Message;
                        await transaction.RollbackAsync();

                        LogController.Error(sqlItem + ": " + err, true);
                        SetControllerMessage(err, tag);
                        LogController.Information("Rollback transaction", true);
                        LogController.Information("End transaction", true);
                        return false;
                    }

                    string message = $"executed ({(i + 1)}/{listSQL.Count})";  //   string.Format("executed {0}%", ((i + 1) * 100 / listSQL.Count));
                    SetControllerMessage(message, tag);
                }

                await transaction.CommitAsync();

                LogController.Information("End transaction", true);
            }
            return true;
        }

        private static async void SetControllerMessage(string message, string tag)
        {
            DelegateController.WriteInformation(message, false, tag);
        }

    }
}
