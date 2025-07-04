using Helpers;
using Helpers.controllers;
using HRImportData.Classes;
using HRImportData.Controllers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HRImportData.Forms
{
    public partial class ReferenceFunctionDialog : Form
    {
        public ReferenceFunctionDialog()
        {
            InitializeComponent();
        }

        private void ReferenceFunctionDialog_Load(object sender, EventArgs e)
        {
            Helper.BindListToGrid(ImportController.referenceFunctions, gridReferenceFuntion, false);

            gridReferenceFuntion.AutoResizeColumns();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ImportController.referenceFunctions.Add(new Classes.ReferenceFunction());
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Save();
            MessageBox.Show("Reference Functions saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var selectedRows = gridReferenceFuntion.SelectedRows;

            if (selectedRows.Count == 0)
            {
                MessageBox.Show("Please select a row to delete.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Do you want to delete the selected rows?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                foreach (DataGridViewRow row in selectedRows)
                {
                    if (!row.IsNewRow)
                    {
                        ImportController.referenceFunctions.RemoveAt(row.Index);
                    }
                }
                gridReferenceFuntion.DataSource = null;
                gridReferenceFuntion.DataSource = ImportController.referenceFunctions;
                gridReferenceFuntion.AutoResizeColumns();

                Save();
            }
        }

        private void Save()
        {
            var referenceFunctions = ImportController.referenceFunctions.ToList();
            Helper.WriteListObjectToFile(referenceFunctions, ImportController.referenceFunctionPath);

            LogController.InformationBackground( JsonConvert.SerializeObject(referenceFunctions)  );
        }
    }
}
