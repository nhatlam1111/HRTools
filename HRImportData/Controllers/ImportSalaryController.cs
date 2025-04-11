using HRImportData.Classes;
using HRImportData.Forms;
using NPOI.XSSF.UserModel;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace HRImportData.Controllers
{
    internal static class ImportSalaryController
    {
        public static List<DatabaseColumn> DatabaseColumns = new List<DatabaseColumn>();
        public static ExcelWorkbook ExcelWorkbook = null;
        public static ExcelWorkSheet ExcelWorkSheetProcessing = new ExcelWorkSheet();
        public static List<DataRow> ExcelWorkSheetDataImport = new List<DataRow>();
        public static int ExcelWorkSheetStartRowImport = 0;
        public static string TableImport = "THR_EMPLOYEE";

        public static DataTable dtColumnMapping = new DataTable();
        public static frmImportSalary frmImportSalary;
        

        public static async Task Init(frmImportSalary form)
        {
            frmImportSalary = form;

            DatabaseColumns = new List<DatabaseColumn>();
            /*{
                new DatabaseColumn { column_name = "", column_type = OracleDbType.Varchar2, column_description = "" },
                new DatabaseColumn { column_name = "emp_id", column_type = OracleDbType.Varchar2, column_description = "Employee ID", condition_compare = true },
                new DatabaseColumn { column_name = "pro_sal", column_type = OracleDbType.Double, column_description = "Probation Salary" },
                new DatabaseColumn { column_name = "level1_sal", column_type = OracleDbType.Double, column_description = "Salary Level 1" },
                new DatabaseColumn { column_name = "basic_sal", column_type = OracleDbType.Double, column_description = "Salary Level 2" },
                new DatabaseColumn { column_name = "insurance_sal_l1", column_type = OracleDbType.Double, column_description = "Insurance Salary level 1" },
                new DatabaseColumn { column_name = "insurance_sal", column_type = OracleDbType.Double, column_description = "Insurance Salary level 2" },
                new DatabaseColumn { column_name = "confirm_dt", column_type = OracleDbType.Varchar2, column_description = "Confirm date"},

                new DatabaseColumn { column_name = "allow_amt1_l1", column_type = OracleDbType.Double, column_description = "Allowance A1 level 1 (HR0019)" },
                new DatabaseColumn { column_name = "allow_amt2_l1", column_type = OracleDbType.Double, column_description = "Allowance A2 level 1 (HR0019)" },
                new DatabaseColumn { column_name = "allow_amt3_l1", column_type = OracleDbType.Double, column_description = "Allowance A3 level 1 (HR0019)" },
                new DatabaseColumn { column_name = "allow_amt4_l1", column_type = OracleDbType.Double, column_description = "Allowance A4 level 1 (HR0019)" },
                new DatabaseColumn { column_name = "allow_amt5_l1", column_type = OracleDbType.Double, column_description = "Allowance A5 level 1 (HR0019)" },
                new DatabaseColumn { column_name = "allow_amt6_l1", column_type = OracleDbType.Double, column_description = "Allowance A6 level 1 (HR0019)" },
                new DatabaseColumn { column_name = "allow_amt7_l1", column_type = OracleDbType.Double, column_description = "Allowance A7 level 1 (HR0019)" },
                new DatabaseColumn { column_name = "allow_amt8_l1", column_type = OracleDbType.Double, column_description = "Allowance A8 level 1 (HR0019)" },
                new DatabaseColumn { column_name = "allow_amt9_l1", column_type = OracleDbType.Double, column_description = "Allowance A9 level 1 (HR0019)" },
                new DatabaseColumn { column_name = "allow_amt10_l1", column_type = OracleDbType.Double, column_description = "Allowance A10 level 1 (HR0019)" },
                new DatabaseColumn { column_name = "allow_amt11_l1", column_type = OracleDbType.Double, column_description = "Allowance A11 level 1 (HR0019)" },
                new DatabaseColumn { column_name = "allow_amt12_l1", column_type = OracleDbType.Double, column_description = "Allowance A12 level 1 (HR0019)" },
                new DatabaseColumn { column_name = "allow_amt13_l1", column_type = OracleDbType.Double, column_description = "Allowance A13 level 1 (HR0019)" },
                new DatabaseColumn { column_name = "allow_amt14_l1", column_type = OracleDbType.Double, column_description = "Allowance A14 level 1 (HR0019)" },
                new DatabaseColumn { column_name = "allow_amt15_l1", column_type = OracleDbType.Double, column_description = "Allowance A15 level 1 (HR0019)" },
                new DatabaseColumn { column_name = "allow_amt16_l1", column_type = OracleDbType.Double, column_description = "Allowance A16 level 1 (HR0019)" },
                new DatabaseColumn { column_name = "allow_amt17_l1", column_type = OracleDbType.Double, column_description = "Allowance A17 level 1 (HR0019)" },
                new DatabaseColumn { column_name = "allow_amt18_l1", column_type = OracleDbType.Double, column_description = "Allowance A18 level 1 (HR0019)" },
                new DatabaseColumn { column_name = "allow_amt19_l1", column_type = OracleDbType.Double, column_description = "Allowance A19 level 1 (HR0019)" },
                new DatabaseColumn { column_name = "allow_amt20_l1", column_type = OracleDbType.Double, column_description = "Allowance A20 level 1 (HR0019)" },

                new DatabaseColumn { column_name = "allow_amt1", column_type = OracleDbType.Double, column_description = "Allowance A1 level 2 (HR0019)" },
                new DatabaseColumn { column_name = "allow_amt2", column_type = OracleDbType.Double, column_description = "Allowance A2 level 2 (HR0019)" },
                new DatabaseColumn { column_name = "allow_amt3", column_type = OracleDbType.Double, column_description = "Allowance A3 level 2 (HR0019)" },
                new DatabaseColumn { column_name = "allow_amt4", column_type = OracleDbType.Double, column_description = "Allowance A4 level 2 (HR0019)" },
                new DatabaseColumn { column_name = "allow_amt5", column_type = OracleDbType.Double, column_description = "Allowance A5 level 2 (HR0019)" },
                new DatabaseColumn { column_name = "allow_amt6", column_type = OracleDbType.Double, column_description = "Allowance A6 level 2 (HR0019)" },
                new DatabaseColumn { column_name = "allow_amt7", column_type = OracleDbType.Double, column_description = "Allowance A7 level 2 (HR0019)" },
                new DatabaseColumn { column_name = "allow_amt8", column_type = OracleDbType.Double, column_description = "Allowance A8 level 2 (HR0019)" },
                new DatabaseColumn { column_name = "allow_amt9", column_type = OracleDbType.Double, column_description = "Allowance A9 level 2 (HR0019)" },
                new DatabaseColumn { column_name = "allow_amt10", column_type = OracleDbType.Double, column_description = "Allowance A10 level 2 (HR0019)" },
                new DatabaseColumn { column_name = "allow_amt11", column_type = OracleDbType.Double, column_description = "Allowance A11 level 2 (HR0019)" },
                new DatabaseColumn { column_name = "allow_amt12", column_type = OracleDbType.Double, column_description = "Allowance A12 level 2 (HR0019)" },
                new DatabaseColumn { column_name = "allow_amt13", column_type = OracleDbType.Double, column_description = "Allowance A13 level 2 (HR0019)" },
                new DatabaseColumn { column_name = "allow_amt14", column_type = OracleDbType.Double, column_description = "Allowance A14 level 2 (HR0019)" },
                new DatabaseColumn { column_name = "allow_amt15", column_type = OracleDbType.Double, column_description = "Allowance A15 level 2 (HR0019)" },
                new DatabaseColumn { column_name = "allow_amt16", column_type = OracleDbType.Double, column_description = "Allowance A16 level 2 (HR0019)" },
                new DatabaseColumn { column_name = "allow_amt17", column_type = OracleDbType.Double, column_description = "Allowance A17 level 2 (HR0019)" },
                new DatabaseColumn { column_name = "allow_amt18", column_type = OracleDbType.Double, column_description = "Allowance A18 level 2 (HR0019)" },
                new DatabaseColumn { column_name = "allow_amt19", column_type = OracleDbType.Double, column_description = "Allowance A19 level 2 (HR0019)" },
                new DatabaseColumn { column_name = "allow_amt20", column_type = OracleDbType.Double, column_description = "Allowance A20 level 2 (HR0019)" }
            };*/


            dtColumnMapping = new DataTable();

            dtColumnMapping.Columns.Add("Excel", typeof(string));
            dtColumnMapping.Columns.Add("Database", typeof(string));
            dtColumnMapping.Columns.Add("DBMapping", typeof(bool));
            dtColumnMapping.Columns.Add("Type", typeof(OracleDbType));
            dtColumnMapping.Columns.Add("Description", typeof(string));


            DatabaseColumns = await DatabaseHelper.GetTableColumns(TableImport);
        }

        public static void Release()
        {
            frmImportSalary = null;
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
            frmImportSalary.Enabled = false;
            frmImportSalary.frmProcessing.Show();
            await MainController.WriteInformation( "Importing...", true);

            var excelData = ExcelWorkSheetProcessing.datas;
            ExcelWorkSheetDataImport = excelData.AsEnumerable().ToList();
            ExcelWorkSheetDataImport.RemoveRange(0, ExcelWorkSheetStartRowImport - 1);
            var triggers = await DatabaseHelper.GetTableTriggers(TableImport);

            ValidateOption options = new ValidateOption()
            {
                validate_dbmapping = true,
                validate_dbmapping_data_duplicate = true,
                validate_value_data_type = true,
                validate_not_null = false,
                validate_from_controller = false
            };


            bool valid = await ImportController.ValidateImport(dtColumnMapping, ExcelWorkSheetDataImport, null, options);

            if (valid)
            {


                //backup db data to execl
                if (frmImportSalary.chkBackupData.Checked)
                {
                    await MainController.WriteInformation("Backup data...", true);
                    if (!(await BackupData()))
                    {
                        goto END_IMPORT;
                    }
                }

                //disable trigger thr_employee
                if (frmImportSalary.chkDisableTrigger.Checked)
                {
                    await MainController.WriteInformation($"Disable triggers [{string.Join(", ", triggers)}]...", true);

                    if (triggers.Count > 0)
                    {
                        await DatabaseHelper.DisableTrigger(triggers);
                    }
                }

                //import
                await MainController.WriteInformation($"Import data to database...", true);
                await ImportToDb();

                //enable trigger thr_employee
                if (frmImportSalary.chkDisableTrigger.Checked)
                {
                    await MainController.WriteInformation($"Enable triggers [{string.Join(", ", triggers)}]...", true);

                    if (triggers.Count > 0)
                    {
                        await DatabaseHelper.EnableTrigger(triggers);
                    }
                }
            }
            else
            {
                goto END_IMPORT;
            }
            await MainController.WriteInformation($"Import data to database completed", true);
            MessageBox.Show("Completed", "", MessageBoxButtons.OK, MessageBoxIcon.Information);

            END_IMPORT:
            frmImportSalary.frmProcessing.Hide();
            frmImportSalary.Enabled = true;

            MainController.ReleaseMemory();
        }

        private static async Task<bool> BackupData()
        {
            bool valid = true;
            //prepare
            var dtColumnMappingLinq = dtColumnMapping.AsEnumerable();
            var DBMappingColumns = dtColumnMappingLinq
                .Where(q => !string.IsNullOrEmpty(q["DBMapping"] + "") && (bool)q["DBMapping"])
                .ToList();

            var importColumns = dtColumnMappingLinq
                .Where(q => !string.IsNullOrEmpty(q["Database"] + ""))
                .ToList();

            var tempDBMappingColumn = DatabaseColumn.ToList(DBMappingColumns);
            var tempImportColumn = DatabaseColumn.ToList(importColumns);

            string sqlColumnDBMapping = string.Join(",", 
                tempDBMappingColumn
                .Select(s => s.column_type_name == "varchar2" 
                            ? $"{s.column_name} {s.column_type_name}({s.column_type_size}) " 
                            : $"{s.column_name} ")
                .ToList());

            string sqlDBMappingTableCreate = string.Format(SQLTemplates.ORACLE_TABLE_DBMAPPING_CREATE, sqlColumnDBMapping);
            string sqlDBMappingTableDrop = SQLTemplates.ORACLE_TABLE_DBMAPPING_DROP;
            List<string> listSQL = new List<string>();

            //generate table vloopkup
            await DatabaseHelper.excuteSQLCommandAsync(sqlDBMappingTableCreate);

            /*==============================================
             //insert from excel to DBMapping table
             ==============================================*/

            foreach (var data in ExcelWorkSheetDataImport)
            { 
                string columns = string.Join(",",  tempDBMappingColumn.Select(s => s.column_name).ToList());
                string values = "";
                List<string> tempValues = new List<string>();
                foreach (var col in tempDBMappingColumn)
                {
                    tempValues.Add($"'{data[col.excel_mapping]}'");
                }
                values = string.Join(",", tempValues);

                listSQL.Add(string.Format(SQLTemplates.ORACLE_TABLE_DBMAPPING_INSERT, columns, values));
            }

            Task insertTask = new Task(async () =>
            {
                valid = await DatabaseHelper.excuteSQLCommandBatchAsync(listSQL);
            });
            insertTask.Start();
            await insertTask;


            /*==============================================
             //END - insert from excel to DBMapping table
             ==============================================*/

            /*==============================================
             //Get data backup from db
             ==============================================*/
            if (valid)
            {
                List<DatabaseColumn> backupColumns = DatabaseColumns
                    .Where(q => tempImportColumn.Exists(e => e.column_name.ToLower() == q.column_name.ToLower()))
                    .ToList();

                string vColumns = string.Join(",", tempDBMappingColumn.Select(s => s.column_name).ToList());
                string vDbColumns = string.Join(",", tempDBMappingColumn.Select(s => $"e.{s.column_name}").ToList());

                string selectColumn = string.Join(",", backupColumns.Select(s => s.column_name).ToList());
                string selectFrom = $"{TableImport} e";
                string selectWhere = $"e.del_if = 0 and exists (select 1 from {SQLTemplates.ORACLE_TABLE_DBMAPPING} q where ({vColumns}) = ({vDbColumns}) ) ";

                string sqlBackup = string.Format(SQLTemplates.ORACLE_SELECT_BACKUP_DB_DATA, selectColumn, selectFrom, selectWhere);

                DataTable dtBackup = await DatabaseHelper.excuteSQLAsync(sqlBackup);

                XSSFWorkbook wb = ExcelController.DataTableToWorkbook(dtBackup);
                string saveBackup = MainController.SaveWorkbook(wb, $"backup_salary_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}");

                if (!string.IsNullOrEmpty(saveBackup))
                {
                    await MainController.WriteInformation("Backup data: Cancelled", true);
                    DialogResult dialogResult = MessageBox.Show("Backup data has been cancelled, do you want to continue?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dialogResult != DialogResult.Yes) valid = false;
                }

            }
            /*==============================================
             //END - Get data backup from db
             ==============================================*/

            await DatabaseHelper.excuteSQLCommandAsync(sqlDBMappingTableDrop);
            return valid;
        }

        private static async Task ImportToDb()
        {
            //prepare
            List<string> listSQL = new List<string>();
            var dtColumnMappingLinq = dtColumnMapping.AsEnumerable();
            var DBMappingColumns = dtColumnMappingLinq
                .Where(q => !string.IsNullOrEmpty(q["DBMapping"] + "") && (bool)q["DBMapping"])
                .ToList();

            var importColumns = dtColumnMappingLinq
                .Where(q => !string.IsNullOrEmpty(q["Database"] + "") && !(!string.IsNullOrEmpty(q["DBMapping"] + "") && (bool)q["DBMapping"]) )
                .ToList();

            var tempDBMappingColumn = DatabaseColumn.ToList(DBMappingColumns);
            var tempImportColumn = DatabaseColumn.ToList(importColumns);

            string vDbColumns = string.Join(",", tempDBMappingColumn.Select(s => $"{s.column_name}").ToList());
            DateTime dtImport = DateTime.Now;
            foreach (var data in ExcelWorkSheetDataImport)
            {
                List<string> tempValuesDBMapping = new List<string>();
                List<string> tempColumnValue = new List<string>();

                foreach (var col in tempDBMappingColumn)
                {
                    tempValuesDBMapping.Add($"'{data[col.excel_mapping]}'");
                }


                foreach (var col in tempImportColumn)
                {
                    if (col.column_type_name != "number")
                    {
                        tempColumnValue.Add($"{col.column_name} = '{data[col.excel_mapping]}'");
                    }
                    else
                    {
                        tempColumnValue.Add($"{col.column_name} = {( (data[col.excel_mapping] == null || data[col.excel_mapping] == DBNull.Value || data[col.excel_mapping]+"" ==  "") 
                            ? "''" 
                            : data[col.excel_mapping]) }"
                        );
                    }
                    
                }

                tempColumnValue.Add($"mod_by = '{DatabaseHelper.site_user_name}'");
                tempColumnValue.Add($"mod_dt = to_date('{dtImport.ToString("yyyyMMddHHmmss")}', 'yyyymmddhh24miss')");

                string valueDBMapping = string.Join(",", tempValuesDBMapping);
                string updateColumn = string.Join(",", tempColumnValue);
                string updateWhere = $"del_if = 0 and ({vDbColumns}) = ({valueDBMapping})";

                string sqlImport = string.Format(SQLTemplates.ORACLE_UPDATE_IMPORT_DATA, TableImport, updateColumn, updateWhere);
                listSQL.Add(sqlImport);
            }

            Task ImportTask = new Task(async () =>
            {
                await DatabaseHelper.excuteSQLCommandBatchAsync(listSQL);
            });
            ImportTask.Start();
            await ImportTask;

        }

    }
}
