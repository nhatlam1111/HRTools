namespace AttendanceAccessToOracle
{
    partial class Main
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            groupBox1 = new GroupBox();
            btnSaveFileConfig = new Button();
            xSyncEachMinutes = new TextBox();
            label8 = new Label();
            xSyncDays = new TextBox();
            label7 = new Label();
            label6 = new Label();
            xAccessFilePass = new TextBox();
            label3 = new Label();
            label2 = new Label();
            btnSelectAccessFile = new Button();
            xAccessFilePath = new TextBox();
            label1 = new Label();
            lblServer = new Label();
            xClientList = new ComboBox();
            label4 = new Label();
            label5 = new Label();
            xDbPass = new TextBox();
            xDbUser = new TextBox();
            panel1 = new Panel();
            lblCurrentLog = new Label();
            lblStatus = new Label();
            btnStartStop = new Button();
            gridMessage = new DataGridView();
            TIME = new DataGridViewTextBoxColumn();
            MESSAGE = new DataGridViewTextBoxColumn();
            groupBox1.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridMessage).BeginInit();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(btnSaveFileConfig);
            groupBox1.Controls.Add(xSyncEachMinutes);
            groupBox1.Controls.Add(label8);
            groupBox1.Controls.Add(xSyncDays);
            groupBox1.Controls.Add(label7);
            groupBox1.Controls.Add(label6);
            groupBox1.Controls.Add(xAccessFilePass);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(btnSelectAccessFile);
            groupBox1.Controls.Add(xAccessFilePath);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(lblServer);
            groupBox1.Controls.Add(xClientList);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(label5);
            groupBox1.Controls.Add(xDbPass);
            groupBox1.Controls.Add(xDbUser);
            groupBox1.Location = new Point(12, -4);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(316, 340);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            // 
            // btnSaveFileConfig
            // 
            btnSaveFileConfig.Location = new Point(233, 311);
            btnSaveFileConfig.Name = "btnSaveFileConfig";
            btnSaveFileConfig.Size = new Size(75, 23);
            btnSaveFileConfig.TabIndex = 10;
            btnSaveFileConfig.Text = "Save";
            btnSaveFileConfig.UseVisualStyleBackColor = true;
            btnSaveFileConfig.Click += btnSaveFileConfig_Click;
            // 
            // xSyncEachMinutes
            // 
            xSyncEachMinutes.Location = new Point(169, 281);
            xSyncEachMinutes.Name = "xSyncEachMinutes";
            xSyncEachMinutes.Size = new Size(139, 23);
            xSyncEachMinutes.TabIndex = 8;
            xSyncEachMinutes.KeyPress += xSyncEachMinutes_KeyPress;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(6, 289);
            label8.Name = "label8";
            label8.Size = new Size(158, 15);
            label8.TabIndex = 58;
            label8.Text = "Sync data (each [n] minutes)";
            // 
            // xSyncDays
            // 
            xSyncDays.Location = new Point(169, 252);
            xSyncDays.Name = "xSyncDays";
            xSyncDays.Size = new Size(139, 23);
            xSyncDays.TabIndex = 7;
            xSyncDays.KeyPress += xSyncDays_KeyPress;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(6, 260);
            label7.Name = "label7";
            label7.Size = new Size(135, 15);
            label7.TabIndex = 56;
            label7.Text = "Sync data (Last [n] days)";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label6.Location = new Point(6, 226);
            label6.Name = "label6";
            label6.Size = new Size(70, 21);
            label6.TabIndex = 55;
            label6.Text = "Options";
            // 
            // xAccessFilePass
            // 
            xAccessFilePass.Location = new Point(75, 181);
            xAccessFilePass.Name = "xAccessFilePass";
            xAccessFilePass.PasswordChar = '*';
            xAccessFilePass.Size = new Size(233, 23);
            xAccessFilePass.TabIndex = 6;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(6, 189);
            label3.Name = "label3";
            label3.Size = new Size(57, 15);
            label3.TabIndex = 53;
            label3.Text = "Password";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(5, 161);
            label2.Name = "label2";
            label2.Size = new Size(64, 15);
            label2.TabIndex = 52;
            label2.Text = "File (.mdb)";
            // 
            // btnSelectAccessFile
            // 
            btnSelectAccessFile.Location = new Point(280, 152);
            btnSelectAccessFile.Name = "btnSelectAccessFile";
            btnSelectAccessFile.Size = new Size(30, 24);
            btnSelectAccessFile.TabIndex = 5;
            btnSelectAccessFile.Text = "...";
            btnSelectAccessFile.UseVisualStyleBackColor = true;
            btnSelectAccessFile.Click += btnSelectAccessFile_Click;
            // 
            // xAccessFilePath
            // 
            xAccessFilePath.Location = new Point(75, 152);
            xAccessFilePath.Name = "xAccessFilePath";
            xAccessFilePath.ReadOnly = true;
            xAccessFilePath.Size = new Size(199, 23);
            xAccessFilePath.TabIndex = 4;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(6, 130);
            label1.Name = "label1";
            label1.Size = new Size(91, 21);
            label1.TabIndex = 49;
            label1.Text = "Access File";
            // 
            // lblServer
            // 
            lblServer.AutoSize = true;
            lblServer.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblServer.Location = new Point(6, 17);
            lblServer.Name = "lblServer";
            lblServer.Size = new Size(59, 21);
            lblServer.TabIndex = 48;
            lblServer.Text = "Server";
            // 
            // xClientList
            // 
            xClientList.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            xClientList.AutoCompleteSource = AutoCompleteSource.ListItems;
            xClientList.DropDownStyle = ComboBoxStyle.DropDownList;
            xClientList.FormattingEnabled = true;
            xClientList.Location = new Point(6, 41);
            xClientList.Name = "xClientList";
            xClientList.Size = new Size(302, 23);
            xClientList.TabIndex = 1;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(169, 70);
            label4.Name = "label4";
            label4.Size = new Size(57, 15);
            label4.TabIndex = 47;
            label4.Text = "Password";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(6, 70);
            label5.Name = "label5";
            label5.Size = new Size(65, 15);
            label5.TabIndex = 46;
            label5.Text = "User Name";
            // 
            // xDbPass
            // 
            xDbPass.Location = new Point(169, 88);
            xDbPass.Name = "xDbPass";
            xDbPass.PasswordChar = '*';
            xDbPass.Size = new Size(139, 23);
            xDbPass.TabIndex = 3;
            // 
            // xDbUser
            // 
            xDbUser.Location = new Point(6, 88);
            xDbUser.Name = "xDbUser";
            xDbUser.Size = new Size(148, 23);
            xDbUser.TabIndex = 2;
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.Control;
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(lblCurrentLog);
            panel1.Controls.Add(lblStatus);
            panel1.Controls.Add(btnStartStop);
            panel1.Controls.Add(gridMessage);
            panel1.Location = new Point(334, 5);
            panel1.Name = "panel1";
            panel1.Size = new Size(454, 331);
            panel1.TabIndex = 1;
            // 
            // lblCurrentLog
            // 
            lblCurrentLog.AutoSize = true;
            lblCurrentLog.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblCurrentLog.ForeColor = Color.Tomato;
            lblCurrentLog.Location = new Point(198, 8);
            lblCurrentLog.Name = "lblCurrentLog";
            lblCurrentLog.RightToLeft = RightToLeft.Yes;
            lblCurrentLog.Size = new Size(12, 15);
            lblCurrentLog.TabIndex = 63;
            lblCurrentLog.Text = "-";
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblStatus.ForeColor = Color.RoyalBlue;
            lblStatus.Location = new Point(84, 3);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(16, 21);
            lblStatus.TabIndex = 62;
            lblStatus.Text = "-";
            // 
            // btnStartStop
            // 
            btnStartStop.BackColor = Color.RoyalBlue;
            btnStartStop.FlatStyle = FlatStyle.Flat;
            btnStartStop.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnStartStop.ForeColor = SystemColors.Control;
            btnStartStop.Location = new Point(3, 3);
            btnStartStop.Name = "btnStartStop";
            btnStartStop.Size = new Size(75, 25);
            btnStartStop.TabIndex = 11;
            btnStartStop.Text = "Start";
            btnStartStop.UseVisualStyleBackColor = false;
            // 
            // gridMessage
            // 
            gridMessage.AllowUserToAddRows = false;
            gridMessage.AllowUserToDeleteRows = false;
            gridMessage.AllowUserToResizeRows = false;
            gridMessage.BorderStyle = BorderStyle.Fixed3D;
            gridMessage.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            gridMessage.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            gridMessage.Columns.AddRange(new DataGridViewColumn[] { TIME, MESSAGE });
            gridMessage.Location = new Point(3, 31);
            gridMessage.Name = "gridMessage";
            gridMessage.ReadOnly = true;
            gridMessage.RowHeadersVisible = false;
            gridMessage.ShowCellErrors = false;
            gridMessage.ShowEditingIcon = false;
            gridMessage.ShowRowErrors = false;
            gridMessage.Size = new Size(446, 295);
            gridMessage.TabIndex = 0;
            // 
            // TIME
            // 
            TIME.Frozen = true;
            TIME.HeaderText = "Time";
            TIME.Name = "TIME";
            TIME.ReadOnly = true;
            TIME.Width = 150;
            // 
            // MESSAGE
            // 
            MESSAGE.HeaderText = "Message";
            MESSAGE.Name = "MESSAGE";
            MESSAGE.ReadOnly = true;
            MESSAGE.Width = 280;
            // 
            // Main
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 348);
            Controls.Add(panel1);
            Controls.Add(groupBox1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "Main";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Sync Attendance Data";
            Load += Main_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)gridMessage).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
        private Label lblServer;
        private ComboBox xClientList;
        private Label label4;
        private Label label5;
        private TextBox xDbPass;
        private TextBox xDbUser;
        private Button btnSelectAccessFile;
        private TextBox xAccessFilePath;
        private Label label1;
        private TextBox xAccessFilePass;
        private Label label3;
        private Label label2;
        private TextBox xSyncEachMinutes;
        private Label label8;
        private TextBox xSyncDays;
        private Label label7;
        private Label label6;
        private Button btnSaveFileConfig;
        private Panel panel1;
        private DataGridView gridMessage;
        private DataGridViewTextBoxColumn TIME;
        private DataGridViewTextBoxColumn MESSAGE;
        private Button btnStartStop;
        private Label lblStatus;
        private Label lblCurrentLog;
    }
}
