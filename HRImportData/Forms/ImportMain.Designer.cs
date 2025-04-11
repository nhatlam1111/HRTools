namespace HRImportData.Forms
{
    partial class ImportMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImportMain));
            groupBoxData = new GroupBox();
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
            groupBoxMapping = new GroupBox();
            btnMappingResize = new Button();
            lblMappingRecord = new Label();
            gridColumnMapping = new DataGridView();
            btnFuncImport = new Button();
            txtLastLog = new TextBox();
            groupBox3 = new GroupBox();
            btnExport = new Button();
            ctrTableImport = new ComboBox();
            label5 = new Label();
            ctrImportFunction = new ComboBox();
            panelControls = new Panel();
            groupBoxData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridExcelData).BeginInit();
            groupBoxMapping.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridColumnMapping).BeginInit();
            groupBox3.SuspendLayout();
            panelControls.SuspendLayout();
            SuspendLayout();
            // 
            // groupBoxData
            // 
            groupBoxData.Controls.Add(btnExcelResize);
            groupBoxData.Controls.Add(label4);
            groupBoxData.Controls.Add(ctrExcelFreezeTo);
            groupBoxData.Controls.Add(btnExcelSelectFile);
            groupBoxData.Controls.Add(lblExcelRecord);
            groupBoxData.Controls.Add(gridExcelData);
            groupBoxData.Controls.Add(btnExcelProcess);
            groupBoxData.Controls.Add(ctrExcelStartRow);
            groupBoxData.Controls.Add(label3);
            groupBoxData.Controls.Add(ctrExcelSheet);
            groupBoxData.Controls.Add(label2);
            groupBoxData.Controls.Add(ctrExcelSelectFile);
            groupBoxData.Controls.Add(label1);
            groupBoxData.Location = new Point(0, 76);
            groupBoxData.Name = "groupBoxData";
            groupBoxData.Size = new Size(850, 714);
            groupBoxData.TabIndex = 0;
            groupBoxData.TabStop = false;
            groupBoxData.Text = "Data Import";
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
            lblExcelRecord.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
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
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F);
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
            gridExcelData.ShowEditingIcon = false;
            gridExcelData.Size = new Size(832, 618);
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
            // groupBoxMapping
            // 
            groupBoxMapping.Controls.Add(btnMappingResize);
            groupBoxMapping.Controls.Add(lblMappingRecord);
            groupBoxMapping.Controls.Add(gridColumnMapping);
            groupBoxMapping.Location = new Point(850, 76);
            groupBoxMapping.Name = "groupBoxMapping";
            groupBoxMapping.Size = new Size(515, 714);
            groupBoxMapping.TabIndex = 1;
            groupBoxMapping.TabStop = false;
            groupBoxMapping.Text = "Column Mapping";
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
            lblMappingRecord.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
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
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            gridColumnMapping.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            gridColumnMapping.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            gridColumnMapping.Location = new Point(6, 89);
            gridColumnMapping.Name = "gridColumnMapping";
            gridColumnMapping.RowHeadersVisible = false;
            gridColumnMapping.ShowEditingIcon = false;
            gridColumnMapping.Size = new Size(503, 618);
            gridColumnMapping.TabIndex = 2;
            // 
            // btnFuncImport
            // 
            btnFuncImport.Location = new Point(803, 4);
            btnFuncImport.Name = "btnFuncImport";
            btnFuncImport.Size = new Size(93, 51);
            btnFuncImport.TabIndex = 0;
            btnFuncImport.Text = "Import";
            btnFuncImport.UseVisualStyleBackColor = true;
            btnFuncImport.Click += btnFuncImport_Click;
            // 
            // txtLastLog
            // 
            txtLastLog.BackColor = SystemColors.ButtonFace;
            txtLastLog.BorderStyle = BorderStyle.FixedSingle;
            txtLastLog.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtLastLog.ForeColor = Color.FromArgb(192, 0, 0);
            txtLastLog.Location = new Point(3, 4);
            txtLastLog.Multiline = true;
            txtLastLog.Name = "txtLastLog";
            txtLastLog.ReadOnly = true;
            txtLastLog.Size = new Size(794, 23);
            txtLastLog.TabIndex = 3;
            txtLastLog.Text = "-------------------";
            txtLastLog.TextAlign = HorizontalAlignment.Center;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(btnExport);
            groupBox3.Controls.Add(ctrTableImport);
            groupBox3.Controls.Add(label5);
            groupBox3.Controls.Add(ctrImportFunction);
            groupBox3.Location = new Point(0, 5);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(454, 65);
            groupBox3.TabIndex = 4;
            groupBox3.TabStop = false;
            groupBox3.Text = "Import Function";
            // 
            // btnExport
            // 
            btnExport.Location = new Point(365, 35);
            btnExport.Name = "btnExport";
            btnExport.Size = new Size(83, 23);
            btnExport.TabIndex = 4;
            btnExport.Text = "Export Excel";
            btnExport.UseVisualStyleBackColor = true;
            btnExport.Click += btnExport_Click;
            // 
            // ctrTableImport
            // 
            ctrTableImport.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            ctrTableImport.AutoCompleteSource = AutoCompleteSource.ListItems;
            ctrTableImport.FormattingEnabled = true;
            ctrTableImport.Location = new Point(195, 36);
            ctrTableImport.Name = "ctrTableImport";
            ctrTableImport.Size = new Size(164, 23);
            ctrTableImport.TabIndex = 2;
            ctrTableImport.SelectedValueChanged += ctrTableImport_SelectedValueChanged;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(195, 17);
            label5.Name = "label5";
            label5.Size = new Size(74, 15);
            label5.TabIndex = 1;
            label5.Text = "Table Import";
            // 
            // ctrImportFunction
            // 
            ctrImportFunction.DropDownStyle = ComboBoxStyle.DropDownList;
            ctrImportFunction.FormattingEnabled = true;
            ctrImportFunction.Location = new Point(6, 36);
            ctrImportFunction.Name = "ctrImportFunction";
            ctrImportFunction.Size = new Size(183, 23);
            ctrImportFunction.TabIndex = 0;
            ctrImportFunction.SelectedValueChanged += ctrImportFunction_SelectedValueChanged;
            // 
            // panelControls
            // 
            panelControls.BackColor = Color.LightGray;
            panelControls.Controls.Add(txtLastLog);
            panelControls.Controls.Add(btnFuncImport);
            panelControls.Location = new Point(460, 10);
            panelControls.Name = "panelControls";
            panelControls.Size = new Size(899, 60);
            panelControls.TabIndex = 5;
            // 
            // ImportMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1371, 791);
            Controls.Add(panelControls);
            Controls.Add(groupBox3);
            Controls.Add(groupBoxMapping);
            Controls.Add(groupBoxData);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "ImportMain";
            Padding = new Padding(5);
            StartPosition = FormStartPosition.CenterScreen;
            Text = "HR Import";
            FormClosed += ImportMain_FormClosed;
            groupBoxData.ResumeLayout(false);
            groupBoxData.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)gridExcelData).EndInit();
            groupBoxMapping.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)gridColumnMapping).EndInit();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            panelControls.ResumeLayout(false);
            panelControls.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBoxData;
        private TextBox ctrExcelSelectFile;
        private Label label1;
        private Button btnExcelProcess;
        private TextBox ctrExcelStartRow;
        private Label label3;
        private ComboBox ctrExcelSheet;
        private Label label2;
        private DataGridView gridExcelData;
        private Label lblExcelRecord;
        private GroupBox groupBoxMapping;
        private Label lblMappingRecord;
        private DataGridView gridColumnMapping;
        private Button btnFuncImport;
        private Button btnExcelSelectFile;
        private TextBox txtLastLog;
        private Label label4;
        private TextBox ctrExcelFreezeTo;
        private Button btnExcelResize;
        private Button btnMappingResize;
        private GroupBox groupBox3;
        private Label label5;
        private ComboBox ctrImportFunction;
        private ComboBox ctrTableImport;
        private Panel panelControls;
        private Button btnExport;
    }
}