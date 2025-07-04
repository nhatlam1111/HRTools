using HRImportData.Classes;
using HRImportData.Forms;
using Helpers;
using NPOI.XSSF.UserModel;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using static HRImportData.Controllers.MainController;
using Helpers.classes;
using System.ComponentModel;
using Helpers.controllers;

namespace HRImportData.Controllers
{
    public static class ImportController
    {
        public delegate string ValidateFromControllerDelegate(DataRow rowImport, DataRow validateColumn);
        public delegate Task<bool> ValidateImportDelegate(DataTable dtColumnMapping, List<DataRow> importDatas, ValidateFromControllerDelegate validateFromController, ValidateOption options);
        public static ValidateImportDelegate ValidateImport = ValidateImportData;


        public static List<DatabaseColumn> DatabaseColumns = new List<DatabaseColumn>();
        public static ExcelWorkbook ExcelWorkbook = null;
        public static ExcelWorkSheet ExcelWorkSheetProcessing = new ExcelWorkSheet();
        public static List<DataRow> ExcelWorkSheetDataImport = new List<DataRow>();
        public static int ExcelWorkSheetStartRowImport = 0;
        public static string TableImport = "";
        public static IMPORT_TYPE ImportType;

        public static DataTable dtColumnMapping = new DataTable();
        public static ImportMain frmImport;

        public static ValidateOption validateOption = new ValidateOption();

        private static string ImportTypeStr { get{
                string importTypeName = Enum.GetName(typeof(IMPORT_TYPE), ImportController.ImportType) ?? "";
                return importTypeName == "" ? "" : importTypeName.StartsWith("INSERT") ? "INSERT" : "UPDATE";
            } 
        }

        public static readonly string referenceFunctionPath = $"{AppDomain.CurrentDomain.BaseDirectory}\\referencefunction.dat";
        public static BindingList<ReferenceFunction> referenceFunctions = new BindingList<ReferenceFunction>();

        public static void Init()
        {
            DatabaseColumns = new List<DatabaseColumn>();
            ExcelWorkbook = null;
            ExcelWorkSheetProcessing = new ExcelWorkSheet();
            ExcelWorkSheetDataImport = new List<DataRow>();
            ExcelWorkSheetStartRowImport = 0;
            TableImport = "";

            InitReferenceFunction();

            dtColumnMapping = new DataTable();
            dtColumnMapping.Columns.Add("Excel", typeof(string));
            dtColumnMapping.Columns.Add("Database", typeof(string));
            dtColumnMapping.Columns.Add("DBMapping", typeof(bool));
            dtColumnMapping.Columns.Add("Type", typeof(OracleDbType));
            dtColumnMapping.Columns.Add("Reference", typeof(string));
            dtColumnMapping.Columns.Add("Description", typeof(string));
        }

        public static void InitReferenceFunction()
        {
            if (File.Exists(referenceFunctionPath))
            {
                var listTmp = Helper.ReadListObjectFromFile<ReferenceFunction>(referenceFunctionPath);
                referenceFunctions = new BindingList<ReferenceFunction>(listTmp);
            }
            else
            {
                referenceFunctions = new BindingList<ReferenceFunction>();
            }
        }

        public static void Release()
        {
            ExcelWorkbook = null;
            ExcelWorkSheetProcessing = new ExcelWorkSheet();
            ExcelWorkSheetDataImport = new List<DataRow>();
            ExcelWorkSheetStartRowImport = 0;

            MainController.ReleaseMemory();
        }

        public static async Task<List<DatabaseTable>> GetTableImports(IMPORT_TYPE importType)
        {
            List<DatabaseTable> tableImports = new List<DatabaseTable>();
            var tableImport = new DatabaseTable();

            switch (importType)
            {
                case IMPORT_TYPE.SELECT_IMPORT_TYPE:
                    break;
                case IMPORT_TYPE.UPDATE_EMPLOYEE:
                case IMPORT_TYPE.INSERT_EMPLOYEE:
                case IMPORT_TYPE.UPDATE_SALARY:

                    tableImport = new DatabaseTable();
                    tableImport.table_name = "THR_EMPLOYEE";
                    tableImport.columns = new List<DatabaseColumn>();
                    tableImports.Add(tableImport);
                    break;
                case IMPORT_TYPE.INSERT_TIME_TEMP:
                    tableImport = new DatabaseTable();
                    tableImport.table_name = "THR_TIME_TEMP";
                    tableImport.columns = new List<DatabaseColumn>();
                    tableImports.Add(tableImport);
                    break;
                case IMPORT_TYPE.INSERT_TABLE:
                case IMPORT_TYPE.UPDATE_TABLE:
                    tableImports = await OracleDb.GetTables();
                    tableImports.Insert(0, new DatabaseTable() { table_name = "", columns = new List<DatabaseColumn>() });
                    break;
            }

            return tableImports;
        }

        public static async Task GetTableColumns()
        {
            DatabaseColumns = await OracleDb.GetTableColumns(TableImport);
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
                LogController.Error(ex.Message);
                throw;
            }
        }

        public static async Task<bool> ExportTableData()
        {
            await WriteInformation($"Begin Export Data: {ImportController.TableImport}", true);
            string sqlBackup = string.Format(SQLTemplates.ORACLE_SELECT_BACKUP_DB_TABLE, TableImport);

            DataTable dtBackup = await OracleDb.excuteSQLAsync(sqlBackup);
            string saveBackup = "";

            using (XSSFWorkbook wb = ExcelController.DataTableToWorkbook(dtBackup))
            {
                saveBackup = MainController.SaveWorkbook(wb, $"export_{TableImport.ToLower()}_{DateTime.Now:yyyyMMdd_HHmmss}");
                
                MessageBox.Show($"Exported data to file: {saveBackup}", "Export Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
            } // Ensures wb.Dispose() is called

            // Release DataTable memory
            dtBackup.Clear();
            dtBackup.Dispose();

            await WriteInformation($"End Export Data: {ImportController.TableImport}", true);

            // Force memory release
            MainController.ReleaseMemory();
            return !string.IsNullOrEmpty(saveBackup);
        }

        public static bool ValidateBeforeImport()
        { 
            if(ExcelWorkSheetStartRowImport <= 0)
            {
                MessageBox.Show("Start row for import must be greater than 0", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (ExcelWorkSheetProcessing == null || ExcelWorkSheetProcessing.datas == null || ExcelWorkSheetProcessing?.datas?.Rows?.Count == 0)
            {
                MessageBox.Show("No data found in the worksheet", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            var dtColumnMappingLinq = dtColumnMapping.AsEnumerable();
            var importColumns = dtColumnMappingLinq
                .Where(q => !string.IsNullOrEmpty(q["Database"] + ""))
                .ToList();

            if (importColumns.Count == 0)
            {
                MessageBox.Show("No columns mapped to database found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if(ImportTypeStr == "UPDATE" && !importColumns.Any(q => !string.IsNullOrEmpty(q["DBMapping"] + "") && (bool)q["DBMapping"]))
            {
                MessageBox.Show("No [DBMapping] column found for UPDATE operation", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if(ImportTypeStr == "INSERT" && importColumns.Any(q => !string.IsNullOrEmpty(q["DBMapping"] + "") && (bool)q["DBMapping"]))
            {
                MessageBox.Show("INSERT operation doesn't need [DBMapping] column", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private static async Task<bool> ValidateImportData(DataTable dtColumnMapping, List<DataRow> importDatas, ValidateFromControllerDelegate validateFromController, ValidateOption options)
        {
            bool valid = true;
            await WriteInformation("Validating Mapping Column...", true);

            var dtColumnMappingLinq = dtColumnMapping.AsEnumerable();
            var importColumns = dtColumnMappingLinq.Where(q => !string.IsNullOrEmpty(q["Database"] + "")).ToList();
            bool isHaveDBMapping = dtColumnMappingLinq.Any(q => !string.IsNullOrEmpty(q["DBMapping"] + "") && (bool)q["DBMapping"]);
            string ErrorCol = "";

            if (options.validate_dbmapping && !isHaveDBMapping)
            {
                await WriteInformation("Validating [DBMapping] column...", true);
                valid = WriteError("Cannot find any [DBMapping] column", true);
                goto END_VALIDATE;
            }

            if (importDatas != null && importDatas.Count > 0)
            {
                if (options.validate_dbmapping_data_duplicate)
                {
                    await WriteInformation("Validating Duplicate value(s) on [DBMapping] column(s)...", true);

                    var DBMappingColumns = dtColumnMappingLinq
                        .Where(q => !string.IsNullOrEmpty(q["DBMapping"] + "") && (bool)q["DBMapping"])
                        .Select(s => s["Excel"] + "")
                        .ToList();

                    var groupDuplicated = importDatas
                        .GroupBy(g => g, new Classes.DataRowComparer(DBMappingColumns))
                        .Where(q => q.Count() > 1);

                    var aaa = groupDuplicated.ToList();

                    if (groupDuplicated.Count() > 0)
                    {
                        valid = WriteError("Have Duplicate value(s) on [DBMapping] column(s)", true);
                        goto END_VALIDATE;
                    }
                }

                if (options.validate_not_null || options.validate_value_data_type || options.validate_from_controller)
                {
                    await WriteInformation($"Validating data...", false);
                    for (int i = 0; i < importDatas.Count; i++)
                    {
                        var rowData = importDatas[i];
                        bool invalid = false;
                        string errorMessageFromController = "";

                        foreach (var col in importColumns)
                        {
                            var value = rowData[col["Excel"] + ""];

                            if (!options.validate_not_null && string.IsNullOrEmpty(value + "")) continue;

                            if (options.validate_value_data_type)
                            {
                                switch ((OracleDbType)col["Type"])
                                {
                                    case OracleDbType.Int16:
                                        {
                                            short temp = 0;
                                            invalid = !short.TryParse(value + "", out temp);
                                            break;
                                        }
                                    case OracleDbType.Int32:
                                        {
                                            int temp = 0;
                                            invalid = !int.TryParse(value + "", out temp);
                                            break;
                                        }
                                    case OracleDbType.Int64:
                                        {
                                            long temp = 0;
                                            invalid = !long.TryParse(value + "", out temp);
                                            break;
                                        }
                                    case OracleDbType.Decimal:
                                        {
                                            decimal temp = 0;
                                            invalid = !decimal.TryParse(value + "", out temp);
                                            break;
                                        }
                                    case OracleDbType.Double:
                                        {
                                            Double temp = 0;
                                            invalid = !double.TryParse(value + "", out temp);
                                            break;
                                        }
                                    default:
                                        {
                                            break;
                                        }
                                }
                            }

                            if (options.validate_from_controller && validateFromController != null)
                            {
                                //validate theo dieu kien tung loai import
                                errorMessageFromController = validateFromController(rowData, col);
                                invalid = !string.IsNullOrEmpty(errorMessageFromController);
                            }


                            if (invalid)
                            {
                                ErrorCol = col["Excel"] + "";
                                break;
                            }
                        }

                        if (!string.IsNullOrEmpty(errorMessageFromController))
                        {
                            valid = WriteError(errorMessageFromController, true);
                            goto END_VALIDATE;
                        }

                        if (invalid)
                        {
                            valid = WriteError($"Data type not matched at column [{ErrorCol}], row index [{rowData["index"] + ""}]", true);
                            goto END_VALIDATE;
                        }

                    }
                }
            }
            else
            {
                valid = WriteError("No data found", true);
            }

        END_VALIDATE:
            if (valid) await WriteInformation("Validating completed", true);
            else await WriteInformation("Validating got error", true);
            return valid;
        }

        internal static async Task ImportData()
        {
            frmImport.Enabled = false;
            frmImport.frmProcessing.Show();
            await MainController.WriteInformation("Importing...", true);

            var excelData = ExcelWorkSheetProcessing.datas;
            ExcelWorkSheetDataImport = excelData.AsEnumerable().ToList();
            ExcelWorkSheetDataImport.RemoveRange(0, ExcelWorkSheetStartRowImport - 1);
            var triggers = await OracleDb.GetTableTriggers(TableImport);

            ValidateFromControllerDelegate validateTemplate = null;
            switch (ImportType)
            {
                case IMPORT_TYPE.INSERT_TIME_TEMP:
                    validateTemplate = ValidateTemplates.ValidateImportTimeTemp;
                    break;
            }

            bool valid = await ValidateImport(dtColumnMapping, ExcelWorkSheetDataImport, validateTemplate, validateOption);

            if (valid)
            {
                //backup db data to execl
                if (validateOption.backup_data)
                {
                    await MainController.WriteInformation("Backup data...", true);
                    bool exported = false;
                    if (validateOption.backup_table_data)
                    {
                        exported = await ExportTableData();
                    }
                    else
                    {
                        exported = await BackupData();
                    }

                    if (!exported)
                    {
                        goto END_IMPORT;
                    }
                    
                }

                //disable trigger 
                if (validateOption.trigger_disabled)
                {
                    await MainController.WriteInformation($"Disable triggers [{string.Join(", ", triggers)}]...", true);

                    if (triggers.Count > 0)
                    {
                        await OracleDb.DisableTrigger(triggers);
                    }
                }

                //import
                await MainController.WriteInformation($"Import data to database...", true);
                if(ImportTypeStr == "INSERT")
                {
                    await InsertToDb();
                }
                else
                {
                    await UpdateToDb();
                }
                

                //enable trigger 
                if (validateOption.trigger_disabled)
                {
                    await MainController.WriteInformation($"Enable triggers [{string.Join(", ", triggers)}]...", true);

                    if (triggers.Count > 0)
                    {
                        await OracleDb.EnableTrigger(triggers);
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
            frmImport.frmProcessing.Hide();
            frmImport.Enabled = true;

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
            await OracleDb.excuteSQLCommandAsync(sqlDBMappingTableCreate);

            /*==============================================
             //insert from excel to DBMapping table
             ==============================================*/

            foreach (var data in ExcelWorkSheetDataImport)
            {
                string columns = string.Join(",", tempDBMappingColumn.Select(s => s.column_name).ToList());
                string values = "";
                List<string> tempValues = new List<string>();
                foreach (var col in tempDBMappingColumn)
                {
                    var excelValue = data[col.excel_mapping];
                    if (!string.IsNullOrEmpty(col.sql_reference))
                    {
                        string _sql = col.sql_reference.Replace("$[value]", excelValue + "");
                        var _dt = await OracleDb.excuteSQLAsync(_sql);
                        if (_dt != null && _dt.Rows.Count > 0)
                        {
                            excelValue = _dt.Rows[0][0] + "";
                        }
                        else
                        {
                            excelValue = ""; //default value if sql reference not return any value
                        }

                        tempValues.Add($"'{excelValue}'");
                    }
                    else
                    {
                        tempValues.Add($"'{data[col.excel_mapping]}'");
                    }
                }
                values = string.Join(",", tempValues);

                listSQL.Add(string.Format(SQLTemplates.ORACLE_TABLE_DBMAPPING_INSERT, columns, values));
            }

            Task insertTask = new Task(async () =>
            {
                valid = await OracleDb.excuteSQLCommandBatchAsync(listSQL);
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

                DataTable dtBackup = await OracleDb.excuteSQLAsync(sqlBackup);

                XSSFWorkbook wb = ExcelController.DataTableToWorkbook(dtBackup);
                string saveBackup = MainController.SaveWorkbook(wb, $"backup_salary_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}");

                if (!string.IsNullOrEmpty(saveBackup))
                {
                    await MainController.WriteInformation("Backup data: Cancelled", true);
                    //DialogResult dialogResult = MessageBox.Show("Backup data has been cancelled, do you want to continue?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    //if (dialogResult != DialogResult.Yes) valid = false;
                }

            }
            /*==============================================
             //END - Get data backup from db
             ==============================================*/

            await OracleDb.excuteSQLCommandAsync(sqlDBMappingTableDrop);
            return valid;
        }


        private static async Task UpdateToDb()
        {
            //prepare
            List<string> listSQL = new List<string>();
            var dtColumnMappingLinq = dtColumnMapping.AsEnumerable();
            var DBMappingColumns = dtColumnMappingLinq
                .Where(q => !string.IsNullOrEmpty(q["DBMapping"] + "") && (bool)q["DBMapping"])
                .ToList();

            var importColumns = dtColumnMappingLinq
                .Where(q => !string.IsNullOrEmpty(q["Database"] + "") && !(!string.IsNullOrEmpty(q["DBMapping"] + "") && (bool)q["DBMapping"]))
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
                    var excelValue = data[col.excel_mapping];
                    if (!string.IsNullOrEmpty(col.sql_reference))
                    {
                        string _sql = col.sql_reference.Replace("$[value]", excelValue + "");
                        var _dt = await OracleDb.excuteSQLAsync(_sql);
                        if (_dt != null && _dt.Rows.Count > 0)
                        {
                            excelValue = _dt.Rows[0][0] + "";
                        }
                        else
                        {
                            excelValue = ""; //default value if sql reference not return any value
                        }

                        tempValuesDBMapping.Add($"'{excelValue}'");
                    }
                    else
                    {
                        tempValuesDBMapping.Add($"'{data[col.excel_mapping]}'");
                    }
                }


                foreach (var col in tempImportColumn)
                {
                    var excelValue = data[col.excel_mapping];
                    if (!string.IsNullOrEmpty(col.sql_reference))
                    {
                        string _sql = $"({col.sql_reference.Replace("$[value]", excelValue + "")})";
                        tempColumnValue.Add($"{col.column_name} = {_sql}");
                    }
                    else
                    {
                        if (col.column_type_name != "number")
                        {
                            tempColumnValue.Add($"{col.column_name} = '{excelValue}'");
                        }
                        else
                        {
                            tempColumnValue.Add($"{col.column_name} = {((excelValue == null || excelValue == DBNull.Value || excelValue + "" == "") ? "''" : excelValue)}");
                        }
                    }
                }

                tempColumnValue.Add($"mod_by = '{currentLogin.SiteUserName}'");
                tempColumnValue.Add($"mod_dt = to_date('{dtImport.ToString("yyyyMMddHHmmss")}', 'yyyymmddhh24miss')");

                string valueDBMapping = string.Join(",", tempValuesDBMapping);
                string updateColumn = string.Join(",", tempColumnValue);
                string updateWhere = $"del_if = 0 and ({vDbColumns}) = ({valueDBMapping})";

                string sqlImport = string.Format(SQLTemplates.ORACLE_UPDATE_IMPORT_DATA, TableImport, updateColumn, updateWhere);
                listSQL.Add(sqlImport);
            }

            Task ImportTask = new Task(async () =>
            {
                await OracleDb.excuteSQLCommandBatchAsync(listSQL);
            });
            ImportTask.Start();
            await ImportTask;
        }


        private static async Task<bool> InsertToDb()
        {
            bool valid = true;
            DateTime dtImport = DateTime.Now;
            List<string> listSQL = new List<string>();
            var dtColumnMappingLinq = dtColumnMapping.AsEnumerable();
            var importColumns = dtColumnMappingLinq
                .Where(q => !string.IsNullOrEmpty(q["Database"] + ""))
                .ToList();

            var tempImportColumn = DatabaseColumn.ToList(importColumns);

            //default columns
            if (DatabaseColumns.Any(q => q.column_name == "PK"))
            {
                tempImportColumn.Add(new DatabaseColumn() { column_name = "PK", column_type = OracleDbType.Double });
            }

            if (DatabaseColumns.Any(q => q.column_name == "CRT_DT"))
            {
                tempImportColumn.Add(new DatabaseColumn() { column_name = "CRT_DT", column_type = OracleDbType.Date });
            }

            if (DatabaseColumns.Any(q => q.column_name == "CRT_DT"))
            {
                tempImportColumn.Add(new DatabaseColumn() { column_name = "CRT_BY", column_type = OracleDbType.Varchar2 });
            }

            //end default columns

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
                        tempColumnValue.Add($"'{currentLogin.SiteUserName}'");
                        continue;
                    }

                    var excelValue = data[col.excel_mapping];
                    if (!string.IsNullOrEmpty(col.sql_reference))
                    {
                        string _sql = col.sql_reference.Replace("$[value]", excelValue + "");
                        var _dt = await OracleDb.excuteSQLAsync(_sql);
                        if (_dt != null && _dt.Rows.Count > 0)
                        {
                            excelValue = _dt.Rows[0][0] + "";
                        }
                        else
                        {
                            excelValue = ""; //default value if sql reference not return any value
                        }

                        tempColumnValue.Add($"'{excelValue}'");
                    }
                    else
                    {
                        if (col.column_type_name != "number")
                        {
                            tempColumnValue.Add($"'{data[col.excel_mapping]}'");
                        }
                        else
                        {
                            tempColumnValue.Add($"{data[col.excel_mapping]}");
                        }
                    }
                }


                string insertColumns = string.Join(",", tempImportColumn.Select(s => s.column_name).ToList());
                string insertValues = string.Join(",", tempColumnValue);

                string sqlImport = string.Format(SQLTemplates.ORACLE_INSERT_IMPORT_DATA, TableImport, insertColumns, insertValues);
                listSQL.Add(sqlImport);
            }


            Task importTask = new Task(async () =>
            {
                valid = await OracleDb.excuteSQLCommandBatchAsync(listSQL);
            });

            importTask.Start();

            await importTask;

            return valid;
        }
    }
}
