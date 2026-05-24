namespace HRImportData.Forms
{
    partial class BackupManagementDialog
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            lbl1 = new Label();
            ctrTableImport = new ComboBox();
            label1 = new Label();
            gridBackup = new DataGridView();
            gridData = new DataGridView();
            btnDelete = new Button();
            btnBackup = new Button();
            btnRestoreSelected = new Button();
            btnRestoreAll = new Button();
            btnExportExcel = new Button();
            lblDataRecord = new Label();
            ((System.ComponentModel.ISupportInitialize)gridBackup).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridData).BeginInit();
            SuspendLayout();
            // 
            // lbl1
            // 
            lbl1.AutoSize = true;
            lbl1.Location = new Point(12, 9);
            lbl1.Name = "lbl1";
            lbl1.Size = new Size(35, 15);
            lbl1.TabIndex = 2;
            lbl1.Text = "Table";
            // 
            // ctrTableImport
            // 
            ctrTableImport.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            ctrTableImport.AutoCompleteSource = AutoCompleteSource.ListItems;
            ctrTableImport.FormattingEnabled = true;
            ctrTableImport.Location = new Point(12, 27);
            ctrTableImport.Name = "ctrTableImport";
            ctrTableImport.Size = new Size(352, 23);
            ctrTableImport.TabIndex = 3;
            ctrTableImport.SelectedValueChanged += ctrTableImport_SelectedValueChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 66);
            label1.Name = "label1";
            label1.Size = new Size(59, 15);
            label1.TabIndex = 4;
            label1.Text = "Backup(s)";
            // 
            // gridBackup
            // 
            gridBackup.AllowUserToAddRows = false;
            gridBackup.AllowUserToDeleteRows = false;
            gridBackup.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            gridBackup.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            gridBackup.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            gridBackup.Location = new Point(12, 91);
            gridBackup.Name = "gridBackup";
            gridBackup.ReadOnly = true;
            gridBackup.RowHeadersVisible = false;
            gridBackup.ShowEditingIcon = false;
            gridBackup.Size = new Size(352, 458);
            gridBackup.TabIndex = 8;
            gridBackup.CellClick += gridBackup_CellClick;
            // 
            // gridData
            // 
            gridData.AllowUserToAddRows = false;
            gridData.AllowUserToDeleteRows = false;
            gridData.AllowUserToResizeRows = false;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = SystemColors.Control;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            gridData.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            gridData.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            gridData.Location = new Point(407, 91);
            gridData.Name = "gridData";
            gridData.ReadOnly = true;
            gridData.RowHeadersVisible = false;
            gridData.ShowEditingIcon = false;
            gridData.Size = new Size(765, 458);
            gridData.TabIndex = 10;
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(289, 62);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(75, 23);
            btnDelete.TabIndex = 11;
            btnDelete.Text = "Delete";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            // 
            // btnBackup
            // 
            btnBackup.Location = new Point(407, 5);
            btnBackup.Name = "btnBackup";
            btnBackup.Size = new Size(138, 58);
            btnBackup.TabIndex = 12;
            btnBackup.Text = "Backup";
            btnBackup.UseVisualStyleBackColor = true;
            btnBackup.Click += btnBackup_Click;
            // 
            // btnRestoreSelected
            // 
            btnRestoreSelected.Location = new Point(673, 5);
            btnRestoreSelected.Name = "btnRestoreSelected";
            btnRestoreSelected.Size = new Size(138, 58);
            btnRestoreSelected.TabIndex = 14;
            btnRestoreSelected.Text = "Restore Selected Row";
            btnRestoreSelected.UseVisualStyleBackColor = true;
            btnRestoreSelected.Click += btnRestoreSelected_Click;
            // 
            // btnRestoreAll
            // 
            btnRestoreAll.Location = new Point(817, 5);
            btnRestoreAll.Name = "btnRestoreAll";
            btnRestoreAll.Size = new Size(138, 58);
            btnRestoreAll.TabIndex = 15;
            btnRestoreAll.Text = "Restore All";
            btnRestoreAll.UseVisualStyleBackColor = true;
            btnRestoreAll.Click += btnRestoreAll_Click;
            // 
            // btnExportExcel
            // 
            btnExportExcel.Location = new Point(1064, 27);
            btnExportExcel.Name = "btnExportExcel";
            btnExportExcel.Size = new Size(108, 23);
            btnExportExcel.TabIndex = 16;
            btnExportExcel.Text = "Export to Excel";
            btnExportExcel.UseVisualStyleBackColor = true;
            btnExportExcel.Click += btnExportExcel_Click;
            // 
            // lblDataRecord
            // 
            lblDataRecord.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblDataRecord.ForeColor = SystemColors.ControlText;
            lblDataRecord.Location = new Point(407, 65);
            lblDataRecord.Name = "lblDataRecord";
            lblDataRecord.Size = new Size(156, 23);
            lblDataRecord.TabIndex = 17;
            lblDataRecord.Text = "Record(s): ";
            lblDataRecord.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // BackupManagementDialog
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1184, 561);
            Controls.Add(lblDataRecord);
            Controls.Add(btnExportExcel);
            Controls.Add(btnRestoreAll);
            Controls.Add(btnRestoreSelected);
            Controls.Add(btnBackup);
            Controls.Add(btnDelete);
            Controls.Add(gridData);
            Controls.Add(gridBackup);
            Controls.Add(label1);
            Controls.Add(ctrTableImport);
            Controls.Add(lbl1);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "BackupManagementDialog";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Backup Management";
            ((System.ComponentModel.ISupportInitialize)gridBackup).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridData).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lbl1;
        private ComboBox ctrTableImport;
        private Label label1;
        private DataGridView gridBackup;
        private DataGridView gridData;
        private Button btnDelete;
        private Button btnBackup;
        private Button btnRestoreSelected;
        private Button btnRestoreAll;
        private Button btnExportExcel;
        private Label lblDataRecord;
    }
}