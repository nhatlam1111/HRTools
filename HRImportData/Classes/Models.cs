using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace HRImportData.Classes
{
    internal class Models
    {
    }

    public class SiteInfo
    {
        public SITE_VERSION value { get; set; }
        public string display { get; set; }
    }

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
                    column_type_name = DatabaseHelper.GetOracleDbTypeName((OracleDbType)v["Type"]),
                    column_type_size = DatabaseHelper.GetOracleDbTypeName((OracleDbType)v["Type"]) == "varchar2" ? 4000 : 0,
                    excel_mapping = v["Excel"] + ""
                }); ;
            }

            return databaseColumns;
        }
    }

    public class DatabaseTable
    {
        public string table_name { get; set; }
        public List<DatabaseColumn> columns { get; set; }

        public DatabaseTable()
        {
            columns = new List<DatabaseColumn>();
        }
    }

    public class ExcelWorkbook
    {
        public XSSFWorkbook workBook { get; set; }
        public string workBookPath { get; set; }  //path of workBook
        public List<ExcelWorkSheet> workSheets { get; set; }
    }

    public class ExcelWorkSheet
    {
        public int index { set; get; }
        public string name { set; get; }
        public ISheet sheet { get; set; }
        public DataTable datas { get; set; }
    }

    public class ValidateOption
    {
        public bool validate_dbmapping { get; set; } = true;
        public bool validate_dbmapping_data_duplicate { get; set; } = true;
        public bool validate_not_null { get; set; } = true;
        public bool validate_value_data_type { get; set; } = true;
        public bool validate_from_controller { get; set; } = true;
        public bool backup_data {  get; set; } = true;
        public bool backup_table_data { get; set; } = true;
        public bool trigger_disabled { get; set; } = false;
    }

    public class DataRowComparer : IEqualityComparer<DataRow>
    {
        private List<string> columnNames;

        public DataRowComparer(List<string> columnNames)
        {
            this.columnNames = columnNames;
        }

        public bool Equals(DataRow x, DataRow y)
        {
            foreach (string columnName in columnNames)
            {
                // Compare values in specified columns
                if (!x[columnName].Equals(y[columnName]))
                    return false;
            }
            return true;
        }

        public int GetHashCode(DataRow obj)
        {
            // Combine hash codes of values in specified columns
            unchecked
            {
                int hash = 17;
                foreach (string columnName in columnNames)
                {
                    hash = hash * 23 + obj[columnName].GetHashCode();
                }
                return hash;
            }
        }
    }
}
