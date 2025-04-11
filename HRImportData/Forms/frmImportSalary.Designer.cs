namespace HRImportData.Forms
{
    partial class frmImportSalary
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
            groupBox1 = new GroupBox();
            btnExcelResize = new Button();
            label4 = new Label();
            ctrExcelFreezeTo = new TextBox();
            btnExcelSelectFile = new Button();
            lblExcelRecord = new Label();
            gridExcelData = new DataGridView();
            btnExcelProcess = new Button();
            ctrExcelStartRow = new TextBox();
            label3 = new Label();
            ctrExcelSheet = new ComboBox();
            label2 = new Label();
            ctrExcelSelectFile = new TextBox();
            label1 = new Label();
            groupBox2 = new GroupBox();
            btnMappingResize = new Button();
            lblMappingRecord = new Label();
            gridColumnMapping = new DataGridView();
            groupBox3 = new GroupBox();
            chkBackupData = new CheckBox();
            chkDisableTrigger = new CheckBox();
            btnFuncImport = new Button();
            txtLastLog = new TextBox();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridExcelData).BeginInit();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridColumnMapping).BeginInit();
            groupBox3.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(btnExcelResize);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(ctrExcelFreezeTo);
            groupBox1.Controls.Add(btnExcelSelectFile);
            groupBox1.Controls.Add(lblExcelRecord);
            groupBox1.Controls.Add(gridExcelData);
            groupBox1.Controls.Add(btnExcelProcess);
            groupBox1.Controls.Add(ctrExcelStartRow);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(ctrExcelSheet);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(ctrExcelSelectFile);
            groupBox1.Controls.Add(label1);
            groupBox1.Location = new Point(0, 29);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(850, 761);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Data Import";
            // 
            // btnExcelResize
            // 
            btnExcelResize.Location = new Point(741, 63);
            btnExcelResize.Name = "btnExcelResize";
            btnExcelResize.Size = new Size(103, 23);
            btnExcelResize.TabIndex = 13;
            btnExcelResize.Text = "Resize Column";
            btnExcelResize.UseVisualStyleBackColor = true;
            btnExcelResize.Click += btnExcelResize_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(529, 19);
            label4.Name = "label4";
            label4.Size = new Size(100, 15);
            label4.TabIndex = 12;
            label4.Text = "Freeze to Column";
            // 
            // ctrExcelFreezeTo
            // 
            ctrExcelFreezeTo.BorderStyle = BorderStyle.FixedSingle;
            ctrExcelFreezeTo.Location = new Point(529, 37);
            ctrExcelFreezeTo.Name = "ctrExcelFreezeTo";
            ctrExcelFreezeTo.Size = new Size(100, 23);
            ctrExcelFreezeTo.TabIndex = 11;
            // 
            // btnExcelSelectFile
            // 
            btnExcelSelectFile.Location = new Point(365, 37);
            btnExcelSelectFile.Name = "btnExcelSelectFile";
            btnExcelSelectFile.Size = new Size(31, 23);
            btnExcelSelectFile.TabIndex = 9;
            btnExcelSelectFile.Text = "...";
            btnExcelSelectFile.UseVisualStyleBackColor = true;
            btnExcelSelectFile.Click += btnExcelSelectFile_Click;
            // 
            // lblExcelRecord
            // 
            lblExcelRecord.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            lblExcelRecord.ForeColor = SystemColors.ControlText;
            lblExcelRecord.Location = new Point(12, 63);
            lblExcelRecord.Name = "lblExcelRecord";
            lblExcelRecord.Size = new Size(151, 23);
            lblExcelRecord.TabIndex = 8;
            lblExcelRecord.Text = "Record(s): ";
            lblExcelRecord.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // gridExcelData
            // 
            gridExcelData.AllowUserToAddRows = false;
            gridExcelData.AllowUserToDeleteRows = false;
            gridExcelData.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            gridExcelData.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            gridExcelData.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            gridExcelData.Location = new Point(12, 89);
            gridExcelData.Name = "gridExcelData";
            gridExcelData.ReadOnly = true;
            gridExcelData.RowHeadersVisible = false;
            gridExcelData.RowTemplate.Height = 25;
            gridExcelData.ShowEditingIcon = false;
            gridExcelData.Size = new Size(832, 660);
            gridExcelData.TabIndex = 7;
            // 
            // btnExcelProcess
            // 
            btnExcelProcess.Location = new Point(741, 37);
            btnExcelProcess.Name = "btnExcelProcess";
            btnExcelProcess.Size = new Size(103, 23);
            btnExcelProcess.TabIndex = 6;
            btnExcelProcess.Text = "Process";
            btnExcelProcess.UseVisualStyleBackColor = true;
            btnExcelProcess.Click += btnExcelProcess_Click;
            // 
            // ctrExcelStartRow
            // 
            ctrExcelStartRow.BorderStyle = BorderStyle.FixedSingle;
            ctrExcelStartRow.Location = new Point(635, 37);
            ctrExcelStartRow.Name = "ctrExcelStartRow";
            ctrExcelStartRow.Size = new Size(100, 23);
            ctrExcelStartRow.TabIndex = 5;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(635, 19);
            label3.Name = "label3";
            label3.Size = new Size(96, 15);
            label3.TabIndex = 4;
            label3.Text = "Start Row Import";
            // 
            // ctrExcelSheet
            // 
            ctrExcelSheet.FormattingEnabled = true;
            ctrExcelSheet.Location = new Point(402, 37);
            ctrExcelSheet.Name = "ctrExcelSheet";
            ctrExcelSheet.Size = new Size(121, 23);
            ctrExcelSheet.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(402, 19);
            label2.Name = "label2";
            label2.Size = new Size(70, 15);
            label2.TabIndex = 2;
            label2.Text = "Select Sheet";
            // 
            // ctrExcelSelectFile
            // 
            ctrExcelSelectFile.BorderStyle = BorderStyle.FixedSingle;
            ctrExcelSelectFile.Enabled = false;
            ctrExcelSelectFile.Location = new Point(12, 37);
            ctrExcelSelectFile.Name = "ctrExcelSelectFile";
            ctrExcelSelectFile.Size = new Size(347, 23);
            ctrExcelSelectFile.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 19);
            label1.Name = "label1";
            label1.Size = new Size(59, 15);
            label1.TabIndex = 0;
            label1.Text = "Select File";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(btnMappingResize);
            groupBox2.Controls.Add(lblMappingRecord);
            groupBox2.Controls.Add(gridColumnMapping);
            groupBox2.Location = new Point(850, 29);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(515, 761);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "Column Mapping";
            // 
            // btnMappingResize
            // 
            btnMappingResize.Location = new Point(406, 63);
            btnMappingResize.Name = "btnMappingResize";
            btnMappingResize.Size = new Size(103, 23);
            btnMappingResize.TabIndex = 14;
            btnMappingResize.Text = "Resize Column";
            btnMappingResize.UseVisualStyleBackColor = true;
            btnMappingResize.Click += btnMappingResize_Click;
            // 
            // lblMappingRecord
            // 
            lblMappingRecord.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            lblMappingRecord.ForeColor = SystemColors.ControlText;
            lblMappingRecord.Location = new Point(6, 63);
            lblMappingRecord.Name = "lblMappingRecord";
            lblMappingRecord.Size = new Size(156, 23);
            lblMappingRecord.TabIndex = 9;
            lblMappingRecord.Text = "Record(s): ";
            lblMappingRecord.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // gridColumnMapping
            // 
            gridColumnMapping.AllowUserToAddRows = false;
            gridColumnMapping.AllowUserToDeleteRows = false;
            gridColumnMapping.AllowUserToResizeRows = false;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = SystemColors.Control;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle2.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            gridColumnMapping.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            gridColumnMapping.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            gridColumnMapping.Location = new Point(6, 89);
            gridColumnMapping.Name = "gridColumnMapping";
            gridColumnMapping.RowHeadersVisible = false;
            gridColumnMapping.RowTemplate.Height = 25;
            gridColumnMapping.ShowEditingIcon = false;
            gridColumnMapping.Size = new Size(503, 660);
            gridColumnMapping.TabIndex = 2;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(chkBackupData);
            groupBox3.Controls.Add(chkDisableTrigger);
            groupBox3.Controls.Add(btnFuncImport);
            groupBox3.Location = new Point(1365, 29);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(214, 761);
            groupBox3.TabIndex = 2;
            groupBox3.TabStop = false;
            // 
            // chkBackupData
            // 
            chkBackupData.AutoSize = true;
            chkBackupData.Checked = true;
            chkBackupData.CheckState = CheckState.Checked;
            chkBackupData.Location = new Point(143, 41);
            chkBackupData.Name = "chkBackupData";
            chkBackupData.Size = new Size(65, 19);
            chkBackupData.TabIndex = 5;
            chkBackupData.Text = "Backup";
            chkBackupData.UseVisualStyleBackColor = true;
            // 
            // chkDisableTrigger
            // 
            chkDisableTrigger.AutoSize = true;
            chkDisableTrigger.Location = new Point(6, 41);
            chkDisableTrigger.Name = "chkDisableTrigger";
            chkDisableTrigger.Size = new Size(104, 19);
            chkDisableTrigger.TabIndex = 4;
            chkDisableTrigger.Text = "Disable Trigger";
            chkDisableTrigger.UseVisualStyleBackColor = true;
            // 
            // btnFuncImport
            // 
            btnFuncImport.Location = new Point(6, 63);
            btnFuncImport.Name = "btnFuncImport";
            btnFuncImport.Size = new Size(202, 23);
            btnFuncImport.TabIndex = 0;
            btnFuncImport.Text = "Import";
            btnFuncImport.UseVisualStyleBackColor = true;
            btnFuncImport.Click += btnFuncImport_Click;
            // 
            // txtLastLog
            // 
            txtLastLog.BackColor = SystemColors.ButtonFace;
            txtLastLog.BorderStyle = BorderStyle.FixedSingle;
            txtLastLog.Dock = DockStyle.Top;
            txtLastLog.ForeColor = Color.FromArgb(192, 0, 0);
            txtLastLog.Location = new Point(5, 5);
            txtLastLog.Name = "txtLastLog";
            txtLastLog.ReadOnly = true;
            txtLastLog.Size = new Size(1574, 23);
            txtLastLog.TabIndex = 3;
            txtLastLog.Text = "-------------------";
            txtLastLog.TextAlign = HorizontalAlignment.Center;
            // 
            // frmImportSalary
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1584, 791);
            Controls.Add(txtLastLog);
            Controls.Add(groupBox3);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "frmImportSalary";
            Padding = new Padding(5);
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Import Basic Salary";
            FormClosed += frmImportSalary_FormClosed;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)gridExcelData).EndInit();
            groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)gridColumnMapping).EndInit();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private GroupBox groupBox1;
        private TextBox ctrExcelSelectFile;
        private Label label1;
        private Button btnExcelProcess;
        private TextBox ctrExcelStartRow;
        private Label label3;
        private ComboBox ctrExcelSheet;
        private Label label2;
        private DataGridView gridExcelData;
        private Label lblExcelRecord;
        private GroupBox groupBox2;
        private Label lblMappingRecord;
        private DataGridView gridColumnMapping;
        private GroupBox groupBox3;
        private Button btnFuncImport;
        private Button btnExcelSelectFile;
        private TextBox txtLastLog;
        private Label label4;
        private TextBox ctrExcelFreezeTo;
        private Button btnExcelResize;
        private Button btnMappingResize;
        public CheckBox chkBackupData;
        public CheckBox chkDisableTrigger;
    }
}