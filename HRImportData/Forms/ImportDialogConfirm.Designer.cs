namespace HRImportData.Forms
{
    partial class ImportDialogConfirm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            checkboxValidateMappingExists = new CheckBox();
            labelValidateMappingExists = new Label();
            checkboxValidateDuplicate = new CheckBox();
            checkboxValidateNotNull = new CheckBox();
            btnBackupImport = new Button();
            btnImport = new Button();
            btnBackupImport2 = new Button();
            SuspendLayout();
            // 
            // checkboxValidateMappingExists
            // 
            checkboxValidateMappingExists.AutoSize = true;
            checkboxValidateMappingExists.Checked = true;
            checkboxValidateMappingExists.CheckState = CheckState.Checked;
            checkboxValidateMappingExists.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            checkboxValidateMappingExists.Location = new Point(12, 12);
            checkboxValidateMappingExists.Name = "checkboxValidateMappingExists";
            checkboxValidateMappingExists.Size = new Size(343, 24);
            checkboxValidateMappingExists.TabIndex = 0;
            checkboxValidateMappingExists.Text = "Validate Mapping database column is checked";
            checkboxValidateMappingExists.UseVisualStyleBackColor = true;
            // 
            // labelValidateMappingExists
            // 
            labelValidateMappingExists.AutoSize = true;
            labelValidateMappingExists.Font = new Font("Segoe UI", 9F, FontStyle.Italic, GraphicsUnit.Point, 0);
            labelValidateMappingExists.Location = new Point(31, 39);
            labelValidateMappingExists.Name = "labelValidateMappingExists";
            labelValidateMappingExists.Size = new Size(363, 30);
            labelValidateMappingExists.TabIndex = 1;
            labelValidateMappingExists.Text = "- Required for updating data\r\n- Optional for inserting data (checking for existence before insertion)";
            // 
            // checkboxValidateDuplicate
            // 
            checkboxValidateDuplicate.AutoSize = true;
            checkboxValidateDuplicate.Checked = true;
            checkboxValidateDuplicate.CheckState = CheckState.Checked;
            checkboxValidateDuplicate.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            checkboxValidateDuplicate.Location = new Point(12, 92);
            checkboxValidateDuplicate.Name = "checkboxValidateDuplicate";
            checkboxValidateDuplicate.Size = new Size(396, 24);
            checkboxValidateDuplicate.TabIndex = 2;
            checkboxValidateDuplicate.Text = "Validate Duplicate rows on Mapping database column";
            checkboxValidateDuplicate.UseVisualStyleBackColor = true;
            // 
            // checkboxValidateNotNull
            // 
            checkboxValidateNotNull.AutoSize = true;
            checkboxValidateNotNull.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            checkboxValidateNotNull.Location = new Point(12, 136);
            checkboxValidateNotNull.Name = "checkboxValidateNotNull";
            checkboxValidateNotNull.Size = new Size(351, 24);
            checkboxValidateNotNull.TabIndex = 3;
            checkboxValidateNotNull.Text = "Validate Empty cell value on importing column ";
            checkboxValidateNotNull.UseVisualStyleBackColor = true;
            // 
            // btnBackupImport
            // 
            btnBackupImport.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnBackupImport.Location = new Point(230, 181);
            btnBackupImport.Name = "btnBackupImport";
            btnBackupImport.Size = new Size(175, 56);
            btnBackupImport.TabIndex = 5;
            btnBackupImport.Text = "Backup Updating Data and Import";
            btnBackupImport.UseVisualStyleBackColor = true;
            btnBackupImport.Click += btnBackupImport_Click;
            // 
            // btnImport
            // 
            btnImport.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnImport.Location = new Point(411, 181);
            btnImport.Name = "btnImport";
            btnImport.Size = new Size(86, 56);
            btnImport.TabIndex = 6;
            btnImport.Text = "Import";
            btnImport.UseVisualStyleBackColor = true;
            btnImport.Click += btnImport_Click;
            // 
            // btnBackupImport2
            // 
            btnBackupImport2.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnBackupImport2.Location = new Point(12, 181);
            btnBackupImport2.Name = "btnBackupImport2";
            btnBackupImport2.Size = new Size(144, 56);
            btnBackupImport2.TabIndex = 7;
            btnBackupImport2.Text = "Backup Table data and Import";
            btnBackupImport2.UseVisualStyleBackColor = true;
            btnBackupImport2.Click += btnBackupImport2_Click;
            // 
            // ImportDialogConfirm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(509, 249);
            Controls.Add(btnBackupImport2);
            Controls.Add(btnImport);
            Controls.Add(btnBackupImport);
            Controls.Add(checkboxValidateNotNull);
            Controls.Add(checkboxValidateDuplicate);
            Controls.Add(labelValidateMappingExists);
            Controls.Add(checkboxValidateMappingExists);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "ImportDialogConfirm";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private CheckBox checkboxValidateMappingExists;
        private Label labelValidateMappingExists;
        private CheckBox checkboxValidateDuplicate;
        private CheckBox checkboxValidateNotNull;
        private Button btnBackupImport;
        private Button btnImport;
        private Button btnBackupImport2;
    }
}