using Helpers;
using Helpers.classes;
using HRImportData.Classes;
using HRImportData.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HRImportData.Forms
{
    public partial class BackupManagementDialog : Form
    {
        private List<DatabaseTable> BackupTables = new List<DatabaseTable>();
        private List<DatabaseTable> tables = new List<DatabaseTable>();

        private string selectedBackupTable = string.Empty;

        private frmProcessing frmProcessing;

        public BackupManagementDialog()
        {
            InitializeComponent();

            frmProcessing = new frmProcessing();
            frmProcessing.Owner = this;

            bindingTables();
        }


        private async Task bindingTables()
        {
            tables = await ImportController.GetTableImports(IMPORT_TYPE.UPDATE_TABLE);


            var bindingTables = new BindingSource();
            bindingTables.DataSource = tables;
            ctrTableImport.DataSource = bindingTables;
            ctrTableImport.DisplayMember = "table_name";
            ctrTableImport.ValueMember = "table_name";

            //ctrTableImport.SelectedIndex = 0;

            ctrTableImport.SelectedValue = ImportController.TableImport;

            BindingBackupTables(ctrTableImport.SelectedValue.ToString());
        }


        private void BindingBackupTables(string table)
        {
            string pattern = $"^{table}_(\\d+(_\\d+)+|\\d+|BK\\d+|BACKUP\\d+|TMP|TEMP)$";

            var backupTables = tables
            .Where(t => Regex.IsMatch(t.table_name, pattern, RegexOptions.IgnoreCase))
            .OrderByDescending(t => t.create_time)
            .Select(q => q)
            .ToList();

            var bindingBackups = new BindingSource();
            bindingBackups.DataSource = backupTables;
            gridBackup.DataSource = bindingBackups;

            try
            {
                gridBackup.Columns[0].Width = 200;
                gridBackup.Columns[1].Width = 150;
            }
            catch
            {
            }
        }

        private async Task BindingBackupData(string table)
        {
            if (!string.IsNullOrEmpty(table))
            {
                frmProcessing.Show();
                frmProcessing.SetMessage("Loading data ...");
                this.Enabled = false;


                var datas = await Task.Run(async () => {
                    return await OracleDb.excuteSQLAsync($"SELECT * FROM {table}");
                });


                //await OracleDb.excuteSQLAsync($"SELECT * FROM {table}");

                lblDataRecord.Text = $"Data records: {datas.Rows.Count}";

                var bindingData = new BindingSource();
                bindingData.DataSource = datas;
                gridData.DataSource = bindingData;

                frmProcessing.Hide();
                this.Enabled = true;
            }
        }

        private void btnBackup_Click(object sender, EventArgs e)
        {

        }

        private void btnRestoreSelected_Click(object sender, EventArgs e)
        {

        }

        private void btnRestoreAll_Click(object sender, EventArgs e)
        {

        }

        private void btnExportExcel_Click(object sender, EventArgs e)
        {

        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedBackupTable))
            { 
                MessageBox.Show("Please select backup table to delete.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }


            var confirm = MessageBox.Show($"Are you sure to delete backup table '{selectedBackupTable}'?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                var result = await OracleDb.excuteSQLCommandAsync($"DROP TABLE {selectedBackupTable} PURGE");
                if (result)
                {
                    MessageBox.Show($"Delete backup table '{selectedBackupTable}' successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    BindingBackupTables(ctrTableImport.SelectedValue.ToString());
                    gridData.DataSource = null;
                    lblDataRecord.Text = "Data records: 0";
                }
                else
                {
                    MessageBox.Show($"Cannot delete backup table '{selectedBackupTable}'.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ctrTableImport_SelectedValueChanged(object sender, EventArgs e)
        {
            if (ctrTableImport.SelectedValue is string)
            {
                BindingBackupTables(ctrTableImport.SelectedValue.ToString());
            }
        }

        private async void gridBackup_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            { 
                var tableName = gridBackup.Rows[e.RowIndex].Cells[0].Value.ToString();
                selectedBackupTable = tableName;
                await BindingBackupData(tableName);
            }
        }
    }
}
