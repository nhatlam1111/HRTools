using HRImportData.Classes;
using HRImportData.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HRImportData.Forms
{
    public partial class ImportDialogConfirm : Form
    {
        public ImportDialogConfirm()
        {
            InitializeComponent();

            checkboxValidateDuplicate.Checked = ImportController.validateOption.validate_dbmapping_data_duplicate;
            checkboxValidateMappingExists.Checked = ImportController.validateOption.validate_dbmapping;
            checkboxValidateNotNull.Checked = ImportController.validateOption.validate_not_null;
        }

        private void btnBackupImport_Click(object sender, EventArgs e)
        {
            Return();
            ImportController.validateOption.backup_data = true;
            this.DialogResult = DialogResult.OK;
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            Return();
            ImportController.validateOption.backup_data = false;
            this.DialogResult = DialogResult.OK;
        }

        private void Return()
        {
            ImportController.validateOption.validate_dbmapping_data_duplicate = checkboxValidateDuplicate.Checked;
            ImportController.validateOption.validate_dbmapping = checkboxValidateMappingExists.Checked;
            ImportController.validateOption.validate_not_null = checkboxValidateNotNull.Checked;
        }

        private void btnBackupImport2_Click(object sender, EventArgs e)
        {
            Return();
            ImportController.validateOption.backup_data = true;
            ImportController.validateOption.backup_table_data = true;
            this.DialogResult = DialogResult.OK;
        }
    }
}
