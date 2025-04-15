using AttendanceAccessToOracle.controllers;
using Helpers.controllers;
using System.Data;
using System.Data.OleDb;

namespace AttendanceAccessToOracle.classes
{
    public static class AccessOleDb
    {
        public static string connectionStringAccess = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Jet OLEDB:Database Password={2};Persist Security Info=False";

        public static async Task<DataTable> ReadData(string sql)
        {
            DataTable dt = new DataTable();

            try
            {
                using (OleDbConnection accessConn = new OleDbConnection(connectionStringAccess))
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
                LogController.Error(ex.Message);
                MainController.UpdateUIMessage(ex.Message, null);
            }

            return dt;
        }
    }
}
