using AttendanceAccessToOracle.controllers;
using Helpers.controllers;
using System.Data;
using System.Data.OleDb;

namespace AttendanceAccessToOracle.classes
{
    public static class AccessOleDb
    {
        public const string connectionStringAccess = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Mode=Read;Jet OLEDB:Database Password={1}";

        public static async Task<DataTable> ReadData(string sql)
        {
            DataTable dt = new DataTable();

            string connString = string.Format(connectionStringAccess, MainController.config.AccessFilePath, MainController.config.AccessFilePass);

            try
            {
                using (OleDbConnection accessConn = new OleDbConnection(connString))
                {
                    await accessConn.OpenAsync();

                    if (accessConn.State == System.Data.ConnectionState.Open)
                    {
                        using (OleDbCommand accessCmd = new OleDbCommand(sql, accessConn))
                        using (OleDbDataReader rd = accessCmd.ExecuteReader())
                        {
                            dt.Load(rd);
                        }
                    }

                    accessConn.Close();
                    accessConn.Dispose();
                }

            }
            catch (Exception ex)
            {
                LogController.Information(ex.Message);
            }

            return dt;
        }
    }
}
