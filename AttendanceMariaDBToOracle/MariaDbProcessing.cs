using AttendanceMariaDBToOracle.classes;
using Helpers;
using Helpers.classes;
using Helpers.controllers;
using MySqlConnector;
using NPOI.SS.Formula.Functions;
using System.Data;

namespace AttendanceMariaDBToOracle
{
    internal class MariaDbProcessing
    {
        // This class will handle the processing of data from MariaDB
        // It will include methods to connect to the database, retrieve data,
        // and process it as needed for the application.
        DatabaseInfo db;
        string configPath = AppDomain.CurrentDomain.BaseDirectory + "maria.config";
        string connectionString = string.Empty;
        MySqlConnection connection;
        
        public ConnectionState ConnectionState
        {
            get
            {
                if (connection == null)
                    return ConnectionState.Closed;
                return connection.State;
            }
        }

        public MariaDbProcessing() 
        {
            db = new DatabaseInfo();
            db = Helper.ReadObjectFromFile<DatabaseInfo>(configPath, true);

            //db.Server = "localhost";
            //db.Database = "ucdb";
            //db.Port = 3306;
            //db.UserId = "root";
            //db.Password = "root@123";
            connectionString = "Server=$[Server];Database=$[Database];Port=$[Port];User Id=$[UserId];Password=$[Password];";
            connectionString = Helper.ReplaceText(connectionString, db);
        }

        public List<TerminalLog> GetTerminalLogs(DateTime fromDate, DateTime toDate)
        {
            // Code to retrieve terminal logs from the database            

            string table1 = $"auth_logs_{toDate.ToString("yyyyMM")}";
            string table2 = $"auth_logs_{fromDate.ToString("yyyyMM")}";
            string sql = "";
            string sqlTemplate = @"
                select 
                    terminal_id as TerminalId, 
                    user_id as UserId, 
                    user_name as UserName, 
                    card as CardId, 
                    event_time as EventTime, 
                    auth_type as AuthType, 
                    auth_result as AuthResult
                from $[table]
                where event_time >= '$[fromDate]' and event_time <= '$[toDate]'
            ";

            sql = Helper.ReplaceText(sqlTemplate, new
            {
                table = table1,
                fromDate = fromDate.ToString("yyyy-MM-dd HH:mm:ss"),
                toDate = toDate.ToString("yyyy-MM-dd HH:mm:ss")
            });

            if (table1 != table2)
            {
                sql += $" union all {Helper.ReplaceText(sqlTemplate, new { table = table2, fromDate = fromDate.ToString("yyyy-MM-dd HH:mm:ss"), toDate = toDate.ToString("yyyy-MM-dd HH:mm:ss") })}";
            }

            List<TerminalLog> logs = new List<TerminalLog>();

            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new MySqlCommand(sql, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                TerminalLog log = new TerminalLog
                                {
                                    TerminalId = Convert.ToInt32(reader["TerminalId"]),
                                    UserId = Convert.ToInt32(reader["UserId"]),
                                    UserName = reader["UserName"]+"",
                                    CardId = reader["CardId"] + "",
                                    EventTime = Convert.ToDateTime(reader["EventTime"]),
                                    AuthType = Convert.ToInt32(reader["AuthType"]),
                                    AuthResult = Convert.ToInt32(reader["AuthResult"])
                                };
                                logs.Add(log);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogController.Error($"Error retrieving terminal logs: {ex.Message}");
                throw;
            }
            Console.WriteLine($"Retrieved {logs.Count} terminal logs from MariaDB.");
            LogController.Information($"Retrieved {logs.Count} terminal logs from MariaDB.");
            return logs;
        }

    }
}
