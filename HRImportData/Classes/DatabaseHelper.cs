using HRImportData.Controllers;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Data;
using System.Data.Common;
using System.Text;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace HRImportData.Classes
{
    internal static class DatabaseHelper
    {
        public static string configFile = Directory.GetCurrentDirectory() + "\\db.config";

        private static OracleConnection _con;
        private const string connectionString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={0})(PORT={1}))(CONNECT_DATA=(SERVER=dedicated)(SERVICE_NAME={2})));User ID={3};Password={4}";

        public static string user_name="";
        public static string user_password = "";
        public static string host = "";
        public static int port = 1521;
        public static string service_name = "";

        public static string site_user_name;
        public static string site_user_pass;
        public static SITE_VERSION site_version;

        public static new string ToString()
        {
            return $"user_name|{user_name}" +
                $";user_password|{user_password}" +
                $";host|{host}" +
                $";port|{port}" +
                $";service_name|{service_name}"+
                $";site_user_name|{site_user_name}"+
                $";site_user_pass|{site_user_pass}"+
                $";site_version|{site_version}"
            ;
        }

        public static void SaveConfig()
        {
            try
            {
                string config = ToString();
                string encryptStr = EncryptionHelper.Encrypt(config, true);

                if (File.Exists(configFile))
                {
                    File.Delete(configFile);
                }

                using (FileStream fs = File.Create(configFile))
                {
                    Byte[] savebytes = new UTF8Encoding(true).GetBytes(encryptStr);
                    fs.Write(savebytes, 0, savebytes.Length);
                    fs.Close();
                    fs.Dispose();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static void LoadConfig()
        {
            try
            {
                using (StreamReader sr = File.OpenText(configFile))
                {
                    string encryptStr = sr.ReadToEnd();
                    string decryptStr = EncryptionHelper.Decrypt(encryptStr, true);

                    List<string> infos = decryptStr.Split(';').ToList();
                    foreach (var info in infos)
                    {
                        string[] data = info.Split('|');

                        string value = data.Length == 2 ? data[1] : "";
                        switch (data[0])
                        {
                            case "user_name": user_name = value; break;
                            case "user_password": user_password = value; break;
                            case "port": port = int.Parse(value); break;
                            case "host": host = value; break;
                            case "service_name": service_name = value; break;
                            case "site_user_name": site_user_name = value; break;
                            case "site_user_pass": site_user_pass = value; break;
                            case "site_version": Enum.TryParse(value, out site_version); break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                //return false;
            }

            //return true;
        }


        public static void LoadConfig(string filePath)
        {
            try
            {
                using (StreamReader sr = File.OpenText(filePath))
                {
                    string encryptStr = sr.ReadToEnd();
                    string decryptStr = EncryptionHelper.Decrypt(encryptStr, true);

                    List<string> infos = decryptStr.Split(';').ToList();
                    foreach (var info in infos)
                    {
                        string[] data = info.Split('|');

                        string value = data.Length == 2 ? data[1] : "";
                        switch (data[0])
                        {
                            case "user_name": user_name = value; break;
                            case "user_password": user_password = value; break;
                            case "port": port = int.Parse(value); break;
                            case "host": host = value; break;
                            case "service_name": service_name = value; break;
                            case "site_user_name": site_user_name = value; break;
                            case "site_user_pass": site_user_pass = value; break;
                            case "site_version": Enum.TryParse(value, out site_version); break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                //return false;
            }

            //return true;
        }

        public static async Task<bool> Connect()
        {
            string _connectionString = string.Format(connectionString, host, port, service_name, user_name, user_password);
            return await Connect(_connectionString);
        }

        public static async Task<bool> Connect(string connectionString)
        {
            _con = new OracleConnection();
            _con.ConnectionString = connectionString;

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

        public static async void Close()
        {
            if (_con != null)
            {
                await _con.CloseAsync();
                await _con.DisposeAsync();
                _con = null;
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
                LogController.Error("excuteSQLCommandAsync: " + sql+": "+ err, true);
                return false;
            }

            return true;
        }

        public static async Task<bool> excuteSQLCommandBatchAsync(List<string> listSQL)
        {
            using (OracleTransaction transaction = _con.BeginTransaction())
            {
                LogController.Information("Start transaction: ", true);
                for (int i = 0; i< listSQL.Count; i++)
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
                        await transaction.RollbackAsync();

                        LogController.Error(sqlItem + ": "+err, true);
                        LogController.Information("Rollback transaction", true);
                        LogController.Information("End transaction", true);
                        return false;
                    }
                    
                    string message = string.Format("executed {0}%", ((i + 1)*100/ listSQL.Count));
                    SetControllerMessage(message);
                }

                await transaction.CommitAsync();

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
                    string tableName = col["TABLE_NAME"].ToString()!;

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
                    string columnName = col["COLUMN_NAME"].ToString()!;
                    string dataType = col["DATA_TYPE"].ToString()!;

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

        private static void SetControllerMessage(string message)
        {
            MainController.WriteInformation(message, false);
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
            return oracleDataType.ToUpper() switch
            {
                "VARCHAR2" => OracleDbType.Varchar2,
                "NVARCHAR2" => OracleDbType.NVarchar2,
                "CHAR" => OracleDbType.Char,
                "NCHAR" => OracleDbType.NChar,
                "CLOB" => OracleDbType.Clob,
                "NCLOB" => OracleDbType.NClob,
                "BLOB" => OracleDbType.Blob,
                "NUMBER" => OracleDbType.Decimal, // Oracle NUMBER can be decimal in C#
                "FLOAT" => OracleDbType.Double,
                "BINARY_FLOAT" => OracleDbType.BinaryFloat,
                "BINARY_DOUBLE" => OracleDbType.BinaryDouble,
                "DATE" => OracleDbType.Date,
                "TIMESTAMP" => OracleDbType.TimeStamp,
                "TIMESTAMP WITH TIME ZONE" => OracleDbType.TimeStampTZ,
                "TIMESTAMP WITH LOCAL TIME ZONE" => OracleDbType.TimeStampLTZ,
                "RAW" => OracleDbType.Raw,
                "LONG" => OracleDbType.Long,
                _ => OracleDbType.Varchar2 // Default to VARCHAR2 if unknown
            };
        }

    }
}
