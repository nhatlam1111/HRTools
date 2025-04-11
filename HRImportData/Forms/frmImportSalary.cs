using HRImportData.Classes;
using HRImportData.Controllers;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Data.Common;
using System.IO;

namespace HRImportData.Forms
{
    public partial class frmImportSalary : Form
    {
        public frmProcessing frmProcessing;
        public frmImportSalary()
        {
            InitializeComponent();
            frmProcessing = new frmProcessing();
            frmProcessing.Owner = this;
            MainController.CurrentFormProcessing = frmProcessing;
            ImportSalaryController.Init(this);

            LogController.DisplayForm = this;
            LogController.DisplayText = txtLastLog;

            gridColumnMapping.CellValueChanged += new DataGridViewCellEventHandler(gridColumnMapping_CellValueChanged);
            gridColumnMapping.CurrentCellDirtyStateChanged += new EventHandler(gridColumnMapping_CurrentCellDirtyStateChanged);

            LogController.Information("Open Import Salary");
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

                    var selectedItem = ImportSalaryController.DatabaseColumns.Find(q => q.column_name.Equals(cb.Value));
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
            sheets.DataSource = ImportSalaryController.ExcelWorkbook.workSheets;

            LogController.Information("Sheets {name}", ImportSalaryController.ExcelWorkbook.workSheets.Select(s => new { index = s.index, name = s.name, record = s.datas.Rows.Count }));

            ctrExcelSheet.DataSource = sheets;
            ctrExcelSheet.DisplayMember = "name";
            ctrExcelSheet.ValueMember = "index";

            ctrExcelSelectFile.Text = ImportSalaryController.ExcelWorkbook.workBookPath;
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

            ImportSalaryController.ReadWorkbook(path);
            LoadWorkBook();

            LogController.Information("End load file " + path);
        }

        private void btnExcelProcess_Click(object sender, EventArgs e)
        {
            if (ImportSalaryController.ExcelWorkbook == null)
            {
                MessageBox.Show("Please select excel file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var selectedSheet = (int)ctrExcelSheet.SelectedValue;

            LogController.Information("Begin Process Sheet " + ctrExcelSheet.SelectedText);

            var excelDataSource = new BindingSource();
            ImportSalaryController.ExcelWorkSheetProcessing = ImportSalaryController.ExcelWorkbook.workSheets[selectedSheet];
            var datas = ImportSalaryController.ExcelWorkSheetProcessing.datas;
            excelDataSource.DataSource = datas;
            gridExcelData.DataSource = excelDataSource;

            SetGridImport();
            SetGridMapping();

            try
            {
                ImportSalaryController.ExcelWorkSheetStartRowImport = int.Parse(ctrExcelStartRow.Text);
            }
            catch
            {
                ImportSalaryController.ExcelWorkSheetStartRowImport = 1;
            }

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
            ImportSalaryController.dtColumnMapping.Clear();

            BindingSource bindingMapping = new BindingSource();
            bindingMapping.DataSource = ImportSalaryController.dtColumnMapping;
            gridColumnMapping.DataSource = bindingMapping;

            DataGridViewComboBoxColumn comboBoxDbColumn = new DataGridViewComboBoxColumn();
            comboBoxDbColumn.DataSource = ImportSalaryController.DatabaseColumns;
            comboBoxDbColumn.DisplayMember = "column_name";
            comboBoxDbColumn.ValueMember = "column_name";
            comboBoxDbColumn.DataPropertyName = "Database";
            comboBoxDbColumn.HeaderText = "Database";

            gridColumnMapping.Columns.RemoveAt(1);
            gridColumnMapping.Columns.Insert(1, comboBoxDbColumn);

            foreach (DataGridViewColumn col in gridColumnMapping.Columns)
            {
                if (col.DataPropertyName == "Database" || col.DataPropertyName == "DBMapping") col.ReadOnly = false;
                else col.ReadOnly = true;
            }


            try
            {
                var selectedSheet = (int)ctrExcelSheet.SelectedValue;
                var datas = ImportSalaryController.ExcelWorkbook.workSheets[selectedSheet].datas;

                if (datas.Columns.Count > 0)
                {
                    foreach (DataColumn col in datas.Columns)
                    {
                        var mappingRow = ImportSalaryController.dtColumnMapping.NewRow();
                        mappingRow[0] = col.ColumnName;

                        ImportSalaryController.dtColumnMapping.Rows.Add(mappingRow);
                    }
                }

                LogController.Information("Column mapping Rows:  " + ImportSalaryController.dtColumnMapping.Rows.Count);

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
            if (ImportSalaryController.ExcelWorkbook == null)
            {
                MessageBox.Show("Please select excel file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            await ImportSalaryController.ImportData();

        }

        private void frmImportSalary_FormClosed(object sender, FormClosedEventArgs e)
        {
            ImportSalaryController.Release();
        }
    }
}
