using HRImportData.Classes;
using HRImportData.Forms;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using static HRImportData.Controllers.ImportController;

namespace HRImportData.Controllers
{
    internal static class ImportTimeTempController
    {
        public static List<DatabaseColumn> DatabaseColumns = new List<DatabaseColumn>();
        private static List<DatabaseColumn> DatabaseColumnsFromDB = new List<DatabaseColumn>();
        public static ExcelWorkbook ExcelWorkbook = null;
        public static ExcelWorkSheet ExcelWorkSheetProcessing = new ExcelWorkSheet();
        public static List<DataRow> ExcelWorkSheetDataImport = new List<DataRow>();
        public static List<DataRow> ColumnMappingImport = new List<DataRow>();

        public static int ExcelWorkSheetStartRowImport = 0;
        public static string TableImport = "THR_TIME_TEMP";

        public static DataTable dtColumnMapping = new DataTable();
        public static frmImportTimeTemp frmImportTimeTemp;
        

        public static async Task Init(frmImportTimeTemp form)
        {
            frmImportTimeTemp = form;
            ExcelWorkbook = null;
            ExcelWorkSheetProcessing = new ExcelWorkSheet();
            ExcelWorkSheetDataImport = new List<DataRow>();
            ExcelWorkSheetStartRowImport = 0;
            dtColumnMapping = new DataTable();

            DatabaseColumns = new List<DatabaseColumn>() 
            {
                new DatabaseColumn { column_name = "", column_type = OracleDbType.Varchar2, column_description = "" },
                new DatabaseColumn { column_name = "id", column_type = OracleDbType.Varchar2, column_description = "Employee ID Num (terminal user id)" },
                new DatabaseColumn { column_name = "location", column_type = OracleDbType.Double, column_description = "Terminal Machine ID" },
                new DatabaseColumn { column_name = "work_dt", column_type = OracleDbType.Varchar2, column_description = "Working date (yyyymmdd" },
                new DatabaseColumn { column_name = "time", column_type = OracleDbType.Varchar2, column_description = "Working time (hh:mm)" }
                
            };


            dtColumnMapping = new DataTable();

            dtColumnMapping.Columns.Add("Excel", typeof(string));
            dtColumnMapping.Columns.Add("Database", typeof(string));
            dtColumnMapping.Columns.Add("DBMapping", typeof(bool));
            dtColumnMapping.Columns.Add("Type", typeof(OracleDbType));
            dtColumnMapping.Columns.Add("Description", typeof(string));

            DatabaseColumnsFromDB = await DatabaseHelper.GetTableColumns(TableImport);

            DatabaseColumns = DatabaseColumns
                    .Where(q => string.IsNullOrEmpty(q.column_name) || DatabaseColumnsFromDB.Exists(e => e.column_name.ToLower() == q.column_name.ToLower()))
                    .ToList();

        }

        public static void Release()
        {
            frmImportTimeTemp = null;
            ExcelWorkbook = null;
            ExcelWorkSheetProcessing = new ExcelWorkSheet();
            ExcelWorkSheetDataImport = new List<DataRow>();
            ExcelWorkSheetStartRowImport = 0;
            dtColumnMapping = new DataTable();

            MainController.ReleaseMemory();
        }

        public static void ReadWorkbook(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                //log
                return;
            }
            
            try
            {
                ExcelWorkbook = ExcelController.ReadWorkbook(filePath);
            }
            catch (Exception ex)
            {
                //log
                throw;
            }
        }

        public static async Task ImportData()
        {
            frmImportTimeTemp.Enabled = false;
            frmImportTimeTemp.frmProcessing.Show();
            await MainController.WriteInformation("Importing...", true);

            var excelData = ExcelWorkSheetProcessing.datas;
            ExcelWorkSheetDataImport = excelData.AsEnumerable().ToList();
            ExcelWorkSheetDataImport.RemoveRange(0, ExcelWorkSheetStartRowImport - 1);

            ValidateFromControllerDelegate validate = ValidateDateTimeFormat;

            ValidateOption options = new ValidateOption()
            {
                validate_dbmapping = false,
                validate_dbmapping_data_duplicate = false,
                validate_value_data_type = true,
                validate_not_null = true,
                validate_from_controller = true
            };

            bool valid = await ValidateImport(dtColumnMapping, ExcelWorkSheetDataImport, validate, options);

            if (valid)
            {
                await MainController.WriteInformation($"Import data to database...", true);
                //import
                await ImportExcelToDbTableImport();
            }
            else
            {
                goto END_IMPORT;
            }

            await MainController.WriteInformation($"Import data to database completed", true);
            MessageBox.Show("Completed", "", MessageBoxButtons.OK, MessageBoxIcon.Information);


        END_IMPORT:
            frmImportTimeTemp.frmProcessing.Hide();
            frmImportTimeTemp.Enabled = true;

            MainController.ReleaseMemory();
        }
        private static string ValidateDateTimeFormat(DataRow rowImport, DataRow validateColumn)
        {
            string errorMsg = "";
            var value = rowImport[validateColumn["Excel"] + ""];

            if (validateColumn["Database"] + "" == "work_dt")
            {
                DateTime d = new DateTime();
                try
                {
                    d = DateTime.ParseExact(value + "", "yyyyMMdd", null);
                }
                catch
                {
                    errorMsg = $"Column [{validateColumn["Database"] + ""}] format must be 'YYYYMMDD'";
                }
            }

            if (validateColumn["Database"] + "" == "time")
            {
                DateTime d = new DateTime();
                try
                {
                    d = DateTime.ParseExact(value + "", "HH:mm", null);
                }
                catch
                {
                    errorMsg = $"Column [{validateColumn["Database"] + ""}] format must be 'hh:mm'";
                }
            }

            return errorMsg;
        }


        private static async Task<bool> ImportExcelToDbTableImport()
        {
            bool valid = true;
            DateTime dtImport = DateTime.Now;
            List<string> listSQL = new List<string>();
            var dtColumnMappingLinq = dtColumnMapping.AsEnumerable();
            var importColumns = dtColumnMappingLinq
                .Where(q => !string.IsNullOrEmpty(q["Database"] + ""))
                .ToList();

            var tempImportColumn = DatabaseColumn.ToList(importColumns);
            tempImportColumn.InsertRange(0, new List<DatabaseColumn>() {
                new DatabaseColumn() { column_name = "PK", column_type = OracleDbType.Double },
                new DatabaseColumn() { column_name = "CRT_DT", column_type = OracleDbType.Date },
                new DatabaseColumn() { column_name = "CRT_BY", column_type = OracleDbType.Varchar2 }
            });


            foreach (var data in ExcelWorkSheetDataImport)
            {
                List<string> tempColumnValue = new List<string>();


                foreach (var col in tempImportColumn)
                {
                    if (col.column_name == "PK")
                    {
                        tempColumnValue.Add($"{TableImport}_SEQ.nextval");
                        continue;
                    }

                    if (col.column_name == "CRT_DT")
                    {
                        tempColumnValue.Add($"to_date('{dtImport.ToString("yyyyMMddHHmmss")}', 'yyyymmddhh24miss')");
                        continue;
                    }

                    if (col.column_name == "CRT_BY")
                    {
                        tempColumnValue.Add($"'{DatabaseHelper.site_user_name}'");
                        continue;
                    }

                    if (col.column_type_name != "number")
                    {
                        tempColumnValue.Add($"'{data[col.excel_mapping]}'");
                    }
                    else
                    {
                        tempColumnValue.Add($"{data[col.excel_mapping]}");
                    }

                }


                string insertColumns = string.Join(",", tempImportColumn.Select(s => s.column_name).ToList());
                string insertValues = string.Join(",", tempColumnValue);

                string sqlImport = string.Format(SQLTemplates.ORACLE_INSERT_IMPORT_DATA, TableImport, insertColumns, insertValues);
                listSQL.Add(sqlImport);
            }


            Task importTask = new Task(async () =>
            {
                valid = await DatabaseHelper.excuteSQLCommandBatchAsync(listSQL);
            });

            importTask.Start();

            await importTask;

            return valid;
        }
    }
}
