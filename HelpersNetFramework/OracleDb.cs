using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Data.Common;
using System.Data;
using Helpers.classes;
using Helpers.controllers;
using System.Timers;
using Serilog.Core;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace Helpers
{
    public static class OracleDb
    {
        private static OracleConnection _con;
        public static string connectionString = "";
        private static System.Timers.Timer timerKeepConnect;
        public static ConnectionState State
        {
            get { return _con == null ? ConnectionState.Closed : _con.State; }
        }

        public static bool IsConnected
        {
            get { return _con != null && State != ConnectionState.Broken && State != ConnectionState.Closed; }
        }

        public static async Task Connect(string _connectionString, bool keepConnect)
        {
            _con = new OracleConnection();
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
                LogController.Information("Database: Connect", true);
            }
            catch (Exception e)
            {
                Close();
                LogController.Error("Database: Connect; ERROR: " + e.Message);
            }
        }

        private static async void TimerKeepConnect_Elapsed(object sender, ElapsedEventArgs e)
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
            _con = new OracleConnection();
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
                _con.Close();
                _con.Dispose();
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
                OracleCommand cmd = new OracleCommand();
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
            OracleCommand command = _con.CreateCommand();
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
            using (OracleTransaction transaction = _con.BeginTransaction())
            {
                LogController.Information("Start transaction: ", true);
                string tag = DateTime.Now.Ticks.ToString();
                for (int i = 0; i < listSQL.Count; i++)
                {
                    string sqlItem = listSQL[i];
                    try
                    {
                        OracleCommand command = _con.CreateCommand();
                        command.Connection = _con;
                        command.CommandText = sqlItem;
                        await command.ExecuteNonQueryAsync();
                        LogController.Information(sqlItem, true);
                    }
                    catch (Exception e)
                    {
                        string err = e.Message;
                        transaction.Rollback();

                        LogController.Error(sqlItem + ": " + err, true);
                        SetControllerMessage(err, tag);
                        LogController.Information("Rollback transaction", true);
                        LogController.Information("End transaction", true);
                        return false;
                    }

                    string message = $"executed ({(i + 1)}/{listSQL.Count})";  //   string.Format("executed {0}%", ((i + 1) * 100 / listSQL.Count));
                    SetControllerMessage(message, tag);
                }

                transaction.Commit();

                LogController.Information("End transaction", true);
            }
            return true;
        }

        public static async Task<DataTable> ExecuteProcedureCursor(string _proc, List<string> _params)
        {
            int idxPara = 0;
            DataTable dt = new DataTable();

            try
            {
                OracleCommand cmd = new OracleCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = _con;
                cmd.CommandText = _proc;

                for (int i = 0; i < _params.Count; i++)
                {
                    cmd.Parameters.Add(":p_" + i, _params[i]);
                    idxPara = i;
                }

                //add return cursor
                cmd.Parameters.Add(":refcursor1", OracleDbType.RefCursor, ParameterDirection.Output);
                await cmd.ExecuteNonQueryAsync();
                OracleRefCursor curr = (OracleRefCursor)cmd.Parameters[_params.Count == 0 ? 0 : idxPara + 1].Value;

                using (OracleDataReader dr = curr.GetDataReader())
                {
                    dt.Load(dr);
                }

                LogController.Information("ExecuteProcedureCursor: " + _proc, true);
            }
            catch (Exception e)
            {
                string err = "exec " + _proc + "('" + string.Join("','", _params) + "'); ERROR: " + e.Message;
                LogController.Error(err, true);
            }

            return dt;
        }

        public static async Task<List<DatabaseTable>> GetTables()
        {
            List<DatabaseTable> tables = new List<DatabaseTable>();

            string sql = $"select TABLE_NAME" +
                " from USER_TABLES" +
                " where status = 'VALID' " +
                " and substr(table_name,1,3) not in ('DIM', 'DIR', 'POP', 'TB_', 'TCM', 'TC_', 'TEB', 'TED', 'TFN', 'TGM', 'TGW', 'THT', 'TIC', 'TIE', 'TIN', 'TLG', 'TOA', 'TPR', 'TPS', 'TSA', 'TSH', 'TSI', 'TST', 'VB_') " +
                " order by table_name";

            var dtTables = await excuteSQLAsync(sql);

            if (dtTables != null && dtTables.Rows.Count > 0)
            {
                foreach (DataRow col in dtTables.Rows)
                {
                    string tableName = col["TABLE_NAME"].ToString();

                    tables.Add(new DatabaseTable()
                    {
                        table_name = tableName,
                        //columns = await GetTableColumns(tableName)
                    });
                }
            }

            return tables;
        }

        public static async Task<List<DatabaseColumn>> GetTableColumns(string tableName)
        {
            List<DatabaseColumn> columns = new List<DatabaseColumn>() {
                new DatabaseColumn {  column_name = "", column_type = OracleDbType.Varchar2 },
            };

            string sql = $"select q.column_name, q.data_type " +
                $" from USER_TAB_COLUMNS q " +
                $" where lower(q.TABLE_NAME) = lower('{tableName}')  " +
                $" and lower(q.column_name) not in ('mod_dt', 'mod_by', 'crt_dt', 'crt_by') " +
                $" order by column_name";
            var dtColumns = await excuteSQLAsync(sql);

            if (dtColumns != null && dtColumns.Rows.Count > 0)
            {
                foreach (DataRow col in dtColumns.Rows)
                {
                    string columnName = col["COLUMN_NAME"].ToString();
                    string dataType = col["DATA_TYPE"].ToString();

                    columns.Add(new DatabaseColumn()
                    {
                        column_name = columnName,
                        column_type = MapOracleDataType(dataType)
                    });
                }
            }

            return columns;
        }

        public static async Task<List<string>> GetTableTriggers(string tableName)
        {
            List<string> triggers = new List<string>();

            string sql = $"select q.TRIGGER_NAME from USER_TRIGGERS q where lower(q.TABLE_NAME) = lower('{tableName}' and and q.STATUS = 'ENABLED')";
            var dtColumns = await excuteSQLAsync(sql);

            if (dtColumns != null && dtColumns.Rows.Count > 0)
            {
                foreach (DataRow col in dtColumns.Rows)
                {
                    triggers.Add(col[0] + "");
                }
            }

            return triggers;
        }

        public static async Task<bool> EnableTrigger(List<string> triggers)
        {
            int idx = 0;
            List<string> sqls = new List<string>();
            foreach (var trigger in triggers)
            {
                sqls.Add($"ALTER TRIGGER {trigger} ENABLE");
            }

            return await excuteSQLCommandBatchAsync(sqls);
        }

        public static async Task<bool> DisableTrigger(List<string> triggers)
        {
            int idx = 0;
            List<string> sqls = new List<string>();
            foreach (var trigger in triggers)
            {
                sqls.Add($"ALTER TRIGGER {trigger} DISABLE");
            }

            return await excuteSQLCommandBatchAsync(sqls);
        }

        public static string GetOracleDbTypeName(OracleDbType dbType)
        {
            switch (dbType)
            {
                case OracleDbType.Double:
                case OracleDbType.Decimal:
                case OracleDbType.Int16:
                case OracleDbType.Int32:
                case OracleDbType.Int64:
                    return "number";
                case OracleDbType.Char:
                case OracleDbType.NChar:
                case OracleDbType.Varchar2:
                case OracleDbType.NVarchar2:
                    return "varchar2";
                default:
                    {
                        return Enum.GetName(typeof(OracleDbType), dbType);
                    }
            }
        }


        public static OracleDbType MapOracleDataType(string oracleDataType)
        {
            switch (oracleDataType.ToUpper())
            {
                case "VARCHAR2":
                    return OracleDbType.Varchar2;
                case "NVARCHAR2":
                    return OracleDbType.NVarchar2;
                case "CHAR":
                    return OracleDbType.Char;
                case "NCHAR":
                    return OracleDbType.NChar;
                case "CLOB":
                    return OracleDbType.Clob;
                case "NCLOB":
                    return OracleDbType.NClob;
                case "BLOB":
                    return OracleDbType.Blob;
                case "NUMBER":
                    return OracleDbType.Decimal;
                case "FLOAT":
                    return OracleDbType.Double;
                case "BINARY_FLOAT":
                    return OracleDbType.BinaryFloat;
                case "BINARY_DOUBLE":
                    return OracleDbType.BinaryDouble;
                case "DATE":
                    return OracleDbType.Date;
                case "TIMESTAMP":
                    return OracleDbType.TimeStamp;
                case "TIMESTAMP WITH TIME ZONE":
                    return OracleDbType.TimeStampTZ;
                case "TIMESTAMP WITH LOCAL TIME ZONE":
                    return OracleDbType.TimeStampLTZ;
                case "RAW":
                    return OracleDbType.Raw;
                case "LONG":
                    return OracleDbType.Long;
                default:
                    return OracleDbType.Varchar2;
            }
        }

        private static async void SetControllerMessage(string message, string tag)
        {
            DelegateController.WriteInformation(message, false, tag);
        }
    }
}
