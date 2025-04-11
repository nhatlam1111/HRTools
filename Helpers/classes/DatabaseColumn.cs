using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers.classes
{
    public class DatabaseColumn
    {
        public string column_name { get; set; }
        public OracleDbType column_type { get; set; }
        public string column_type_name { get; set; }
        public int column_type_size { get; set; }
        public string column_description { get; set; }
        public string excel_mapping { get; set; }
        public bool condition_compare { get; set; }

        public static List<DatabaseColumn> ToList(List<DataRow> columns)
        {
            List<DatabaseColumn> databaseColumns = new List<DatabaseColumn>();

            foreach (var v in columns)
            {
                databaseColumns.Add(new DatabaseColumn()
                {
                    column_name = v["Database"] + "",
                    column_type = (OracleDbType)v["Type"],
                    column_type_name = OracleDb.GetOracleDbTypeName((OracleDbType)v["Type"]),
                    column_type_size = OracleDb.GetOracleDbTypeName((OracleDbType)v["Type"]) == "varchar2" ? 4000 : 0,
                    excel_mapping = v["Excel"] + ""
                }); ;
            }

            return databaseColumns;
        }
    }
}
