using Helpers.controllers;
using HRImportData.Classes;
using HRImportData.Controllers;
using NPOI.Util;
using Oracle.ManagedDataAccess.Client;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Security.Policy;
using static HRImportData.Controllers.MainController;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace HRImportData.Forms
{
    public partial class ImportMain : Form
    {
        public frmProcessing frmProcessing;
        public ImportMain()
        {
            InitializeComponent();

            //set lại log theo client
            LogController.Start($"{MainController.currentLogin.Site}-{MainController.currentLogin.DbUserName}");
            LogController.Information("Login: {0}", $"{MainController.currentLogin.SiteUserName}");

            this.Text = $"HR Import - [{MainController.currentLogin.Site}-{MainController.currentLogin.DbUserName}-{MainController.currentLogin.SiteUserName}]";

            frmProcessing = new frmProcessing();
            frmProcessing.Owner = this;
            MainController.CurrentFormProcessing = frmProcessing;

            LogController.DisplayForm = this;
            LogController.DisplayText = txtLastLog;

            var bindingSourceFunctions = new BindingSource();
            bindingSourceFunctions.DataSource = MainController.importFunctions;
            ctrImportFunction.DataSource = bindingSourceFunctions;
            ctrImportFunction.DisplayMember = "Value";
            ctrImportFunction.ValueMember = "Key";

            gridColumnMapping.CellValueChanged += new DataGridViewCellEventHandler(gridColumnMapping_CellValueChanged);
            gridColumnMapping.CurrentCellDirtyStateChanged += new EventHandler(gridColumnMapping_CurrentCellDirtyStateChanged);

            groupBoxData.Enabled = false;
            groupBoxMapping.Enabled = false;
            panelControls.Enabled = false;
            ctrTableImport.Enabled = false;

            btnReferenceFunction.Enabled = true;

            ImportController.frmImport = this;
            ImportController.Init();
        }

        #region Events
        // This event handler manually raises the CellValueChanged event 
        // by calling the CommitEdit method. 
        void gridColumnMapping_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (gridColumnMapping.IsCurrentCellDirty)
            {
                // This fires the cell value changed handler below
                gridColumnMapping.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void gridColumnMapping_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                DataGridViewComboBoxCell cb = (DataGridViewComboBoxCell)gridColumnMapping.Rows[e.RowIndex].Cells[e.ColumnIndex];
                if (cb.Value != null)
                {
                    // do stuff
                    gridColumnMapping.Invalidate();

                    var selectedItem = ImportController.DatabaseColumns.Find(q => q.column_name.Equals(cb.Value));
                    gridColumnMapping.Rows[e.RowIndex].Cells[2].Value = selectedItem == null ? false : selectedItem.condition_compare;
                    gridColumnMapping.Rows[e.RowIndex].Cells[3].Value = selectedItem == null ? DBNull.Value : selectedItem.column_type;
                    gridColumnMapping.Rows[e.RowIndex].Cells[4].Value = selectedItem == null ? null : selectedItem.column_description;
                }
            }

        }

        #endregion



        private void LoadWorkBook()
        {
            LogController.Information("Begin load Workbook");
            gridColumnMapping.DataSource = null;
            gridExcelData.DataSource = null;

            var sheets = new BindingSource();
            sheets.DataSource = ImportController.ExcelWorkbook.workSheets;

            LogController.Information("Sheets {name}", ImportController.ExcelWorkbook.workSheets.Select(s => new { index = s.index, name = s.name, record = s.datas.Rows.Count }));

            ctrExcelSheet.DataSource = sheets;
            ctrExcelSheet.DisplayMember = "name";
            ctrExcelSheet.ValueMember = "index";

            ctrExcelSelectFile.Text = ImportController.ExcelWorkbook.workBookPath;
            LogController.Information("End load Workbook");
        }

        private void btnExcelSelectFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = Directory.GetCurrentDirectory();
            openFileDialog1.Filter = "Spreadsheet(*.xlsx)|*.xlsx";
            openFileDialog1.FilterIndex = 0;

            if (openFileDialog1.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            var path = openFileDialog1.FileName;

            LogController.Information("Begin load file " + path);

            ImportController.ReadWorkbook(path);
            LoadWorkBook();

            LogController.Information("End load file " + path);
        }

        private void btnExcelProcess_Click(object sender, EventArgs e)
        {
            if (ImportController.ExcelWorkbook == null)
            {
                MessageBox.Show("Please select excel file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var selectedSheet = (int)ctrExcelSheet.SelectedValue;

            LogController.Information("Begin Process Sheet " + ctrExcelSheet.SelectedText);

            var excelDataSource = new BindingSource();
            ImportController.ExcelWorkSheetProcessing = ImportController.ExcelWorkbook.workSheets[selectedSheet];
            var datas = ImportController.ExcelWorkSheetProcessing.datas;
            excelDataSource.DataSource = datas;
            gridExcelData.DataSource = excelDataSource;

            try
            {
                ImportController.ExcelWorkSheetStartRowImport = int.Parse(ctrExcelStartRow.Text);
            }
            catch
            {
                ImportController.ExcelWorkSheetStartRowImport = 1;
            }

            SetGridImport();
            SetGridMapping();

            LogController.Information("End Process Sheet");
        }

        private void SetGridImport()
        {
            lblExcelRecord.Text = "Record(s): " + gridExcelData.Rows.Count;
            try
            {
                LogController.Information("Data Rows:  " + gridExcelData.Rows.Count);

                gridExcelData.Columns[0].Width = 40;

                if (string.IsNullOrEmpty(ctrExcelStartRow.Text)) return;
                int startRow = int.Parse(ctrExcelStartRow.Text);

                if (startRow > gridExcelData.Rows.Count)
                {
                    MessageBox.Show("Start row out of range", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int rowIdx = 0;
                foreach (DataGridViewRow dr in gridExcelData.Rows)
                {
                    if (rowIdx + 1 >= startRow)
                    {
                        dr.DefaultCellStyle.BackColor = APP_COLOR.GRID_BACK_COLOR_IMPORT;
                        dr.DefaultCellStyle.ForeColor = APP_COLOR.GRID_FORE_COLOR_IMPORT;
                        dr.Frozen = false;
                    }
                    else
                    {
                        dr.Frozen = true;
                    }

                    rowIdx++;
                }

                if (string.IsNullOrEmpty(ctrExcelFreezeTo.Text)) return;
                int freezeCol = ExcelController.ColumnInExcel.IndexOf(ctrExcelFreezeTo.Text.Trim().ToUpper());

                if (freezeCol > 0)
                {
                    for (int colIdx = 0; colIdx < gridExcelData.Columns.Count; colIdx++)
                    {
                        gridExcelData.Columns[colIdx].Frozen = false;

                        if (colIdx <= freezeCol) gridExcelData.Columns[colIdx].Frozen = true;
                    }
                }

            }
            catch (Exception e)
            {
                LogController.Error("SetGridImport" + e.Message);
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void SetGridMapping()
        {
            ImportController.GetTableColumns().Wait();

            ImportController.dtColumnMapping.Clear();

            BindingSource bindingMapping = new BindingSource();
            bindingMapping.DataSource = ImportController.dtColumnMapping;
            gridColumnMapping.DataSource = bindingMapping;


            //column mapping database
            DataGridViewComboBoxColumn comboBoxDbColumn = new DataGridViewComboBoxColumn();
            comboBoxDbColumn.DataSource = ImportController.DatabaseColumns;
            comboBoxDbColumn.DisplayMember = "column_name";
            comboBoxDbColumn.ValueMember = "column_name";
            comboBoxDbColumn.DataPropertyName = "Database";
            comboBoxDbColumn.HeaderText = "Database";

            gridColumnMapping.Columns.RemoveAt(1);
            gridColumnMapping.Columns.Insert(1, comboBoxDbColumn);


            //column referencefunction
            var bindingListRef = ImportController.referenceFunctions.Copy();
            bindingListRef.Insert(0, new Classes.ReferenceFunction() { Name = " ", Sql = "" }); // Add a default option for no reference function
            DataGridViewComboBoxColumn comboBoxRefFunction = new DataGridViewComboBoxColumn();
            comboBoxRefFunction.DataSource = bindingListRef;
            comboBoxRefFunction.DisplayMember = "Name";
            comboBoxRefFunction.ValueMember = "Sql";
            comboBoxRefFunction.DataPropertyName = "Reference";
            comboBoxRefFunction.HeaderText = "Reference Function";

            gridColumnMapping.Columns.RemoveAt(4);
            gridColumnMapping.Columns.Insert(4, comboBoxRefFunction);




            foreach (DataGridViewColumn col in gridColumnMapping.Columns)
            {
                if (col.DataPropertyName == "Database" || col.DataPropertyName == "DBMapping" || col.DataPropertyName == "Reference") col.ReadOnly = false;
                else col.ReadOnly = true;
            }


            try
            {
                var selectedSheet = (int)ctrExcelSheet.SelectedValue;
                var datas = ImportController.ExcelWorkbook.workSheets[selectedSheet].datas;

                if (datas.Columns.Count > 0)
                {
                    foreach (DataColumn col in datas.Columns)
                    {
                        var mappingRow = ImportController.dtColumnMapping.NewRow();
                        mappingRow[0] = col.ColumnName;

                        ImportController.dtColumnMapping.Rows.Add(mappingRow);
                    }
                }

                LogController.Information("Column mapping Rows:  " + ImportController.dtColumnMapping.Rows.Count);

            }
            catch (Exception e)
            {
                LogController.Error("SetGridMapping" + e.Message);
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            lblMappingRecord.Text = "Record(s): " + gridColumnMapping.Rows.Count;
        }

        private void btnExcelResize_Click(object sender, EventArgs e)
        {
            try
            {
                gridExcelData.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            }
            catch (Exception ee)
            {
                LogController.Error("btnExcelResize_Click" + ee.Message);
                MessageBox.Show(ee.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnMappingResize_Click(object sender, EventArgs e)
        {
            try
            {
                gridColumnMapping.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            }
            catch (Exception ee)
            {
                LogController.Error("btnMappingResize_Click" + ee.Message);
                MessageBox.Show(ee.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnFuncImport_Click(object sender, EventArgs e)
        {
            if (ImportController.ExcelWorkbook == null)
            {
                MessageBox.Show("Please select excel file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!ImportController.ValidateBeforeImport())
            {
                return;
            }

            string importTypeName = Enum.GetName(typeof(IMPORT_TYPE), ImportController.ImportType);

            if (!string.IsNullOrEmpty(importTypeName))
            {
                ImportController.validateOption = new ValidateOption()
                {
                    validate_not_null = false,
                    validate_value_data_type = true,
                    validate_from_controller = false,
                };

                if (importTypeName.StartsWith("INSERT"))
                {
                    ImportController.validateOption.validate_dbmapping = false;
                    ImportController.validateOption.validate_dbmapping_data_duplicate = false;

                }

                if (importTypeName.StartsWith("UPDATE"))
                {
                    ImportController.validateOption.validate_dbmapping = true;
                    ImportController.validateOption.validate_dbmapping_data_duplicate = true;
                    ImportController.validateOption.backup_data = true;
                }

                ImportDialogConfirm importDialogConfirm = new ImportDialogConfirm();

                DialogResult dialogResult = importDialogConfirm.ShowDialog();

                if (dialogResult == DialogResult.OK)
                {
                    await ImportController.ImportData();


                }

            }





            //await ImportController.ImportData();

        }

        private void ImportMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            //ImportController.Release();
        }

        private void ctrImportFunction_SelectedValueChanged(object sender, EventArgs e)
        {
            if (ctrImportFunction.SelectedValue is IMPORT_TYPE selectedImport)
            {
                ImportController.ImportType = selectedImport;
                bindingTables(selectedImport);

                if (selectedImport == IMPORT_TYPE.INSERT_TABLE || selectedImport == IMPORT_TYPE.UPDATE_TABLE)
                {
                    ctrTableImport.Enabled = true;
                }
                else
                {
                    ctrTableImport.Enabled = false;
                }
            }
        }

        private async Task bindingTables(IMPORT_TYPE selectedImport)
        {

            var bindingTables = new BindingSource();
            bindingTables.DataSource = await ImportController.GetTableImports(selectedImport);
            ctrTableImport.DataSource = bindingTables;
            ctrTableImport.DisplayMember = "table_name";
            ctrTableImport.ValueMember = "table_name";
            if (bindingTables.Count == 0)
            {
                groupBoxData.Enabled = false;
                groupBoxMapping.Enabled = false;
                panelControls.Enabled = false;
            }

            ctrTableImport.SelectedIndex = 0;
        }

        private void ctrTableImport_SelectedValueChanged(object sender, EventArgs e)
        {
            btnExport.Enabled = false;
            if (ctrTableImport.SelectedValue is string selectedTable)
            {
                if (!string.IsNullOrEmpty(selectedTable))
                {
                    groupBoxData.Enabled = true;
                    groupBoxMapping.Enabled = true;
                    panelControls.Enabled = true;
                    btnExport.Enabled = true;
                    ImportController.TableImport = selectedTable;
                }
                else
                {
                    groupBoxData.Enabled = false;
                    groupBoxMapping.Enabled = false;
                    panelControls.Enabled = false;
                    btnExport.Enabled = false;
                }

                ImportController.Release();
                gridColumnMapping.DataSource = null;
                gridExcelData.DataSource = null;
                ctrExcelSelectFile.Text = "";
                ctrExcelSheet.DataSource = null;
            }

        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            ExportTable();
        }

        private void ExportTable()
        {
            ImportController.ExportTableData();
        }

        private void btnReferenceFunction_Click(object sender, EventArgs e)
        {
            ReferenceFunctionDialog dialog = new ReferenceFunctionDialog();

            DialogResult dialogResult = dialog.ShowDialog();

            ImportController.InitReferenceFunction();
        }
    }
}
