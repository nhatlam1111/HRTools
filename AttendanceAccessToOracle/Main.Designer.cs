﻿namespace AttendanceAccessToOracle
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            groupConfig = new GroupBox();
            xSyncUsers = new CheckBox();
            xSyncAttendance = new CheckBox();
            label9 = new Label();
            btnSelectSqlTemplate = new Button();
            xSqlTemplatePath = new TextBox();
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
            notifyIcon = new NotifyIcon(components);
            groupConfig.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridMessage).BeginInit();
            SuspendLayout();
            // 
            // groupConfig
            // 
            groupConfig.Controls.Add(xSyncUsers);
            groupConfig.Controls.Add(xSyncAttendance);
            groupConfig.Controls.Add(label9);
            groupConfig.Controls.Add(btnSelectSqlTemplate);
            groupConfig.Controls.Add(xSqlTemplatePath);
            groupConfig.Controls.Add(btnSaveFileConfig);
            groupConfig.Controls.Add(xSyncEachMinutes);
            groupConfig.Controls.Add(label8);
            groupConfig.Controls.Add(xSyncDays);
            groupConfig.Controls.Add(label7);
            groupConfig.Controls.Add(label6);
            groupConfig.Controls.Add(xAccessFilePass);
            groupConfig.Controls.Add(label3);
            groupConfig.Controls.Add(label2);
            groupConfig.Controls.Add(btnSelectAccessFile);
            groupConfig.Controls.Add(xAccessFilePath);
            groupConfig.Controls.Add(label1);
            groupConfig.Controls.Add(lblServer);
            groupConfig.Controls.Add(xClientList);
            groupConfig.Controls.Add(label4);
            groupConfig.Controls.Add(label5);
            groupConfig.Controls.Add(xDbPass);
            groupConfig.Controls.Add(xDbUser);
            groupConfig.Location = new Point(12, -4);
            groupConfig.Name = "groupConfig";
            groupConfig.Size = new Size(316, 398);
            groupConfig.TabIndex = 0;
            groupConfig.TabStop = false;
            // 
            // xSyncUsers
            // 
            xSyncUsers.AutoSize = true;
            xSyncUsers.Location = new Point(169, 344);
            xSyncUsers.Name = "xSyncUsers";
            xSyncUsers.Size = new Size(82, 19);
            xSyncUsers.TabIndex = 12;
            xSyncUsers.Text = "Sync Users";
            xSyncUsers.UseVisualStyleBackColor = true;
            xSyncUsers.Visible = false;
            // 
            // xSyncAttendance
            // 
            xSyncAttendance.AutoSize = true;
            xSyncAttendance.Checked = true;
            xSyncAttendance.CheckState = CheckState.Checked;
            xSyncAttendance.Location = new Point(6, 344);
            xSyncAttendance.Name = "xSyncAttendance";
            xSyncAttendance.Size = new Size(115, 19);
            xSyncAttendance.TabIndex = 11;
            xSyncAttendance.Text = "Sync Attendance";
            xSyncAttendance.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(5, 316);
            label9.Name = "label9";
            label9.Size = new Size(75, 15);
            label9.TabIndex = 61;
            label9.Text = "Sql Template";
            // 
            // btnSelectSqlTemplate
            // 
            btnSelectSqlTemplate.Location = new Point(280, 307);
            btnSelectSqlTemplate.Name = "btnSelectSqlTemplate";
            btnSelectSqlTemplate.Size = new Size(30, 24);
            btnSelectSqlTemplate.TabIndex = 10;
            btnSelectSqlTemplate.Text = "...";
            btnSelectSqlTemplate.UseVisualStyleBackColor = true;
            btnSelectSqlTemplate.Click += btnSelectSqlTemplate_Click;
            // 
            // xSqlTemplatePath
            // 
            xSqlTemplatePath.Location = new Point(90, 307);
            xSqlTemplatePath.Name = "xSqlTemplatePath";
            xSqlTemplatePath.ReadOnly = true;
            xSqlTemplatePath.Size = new Size(184, 23);
            xSqlTemplatePath.TabIndex = 9;
            // 
            // btnSaveFileConfig
            // 
            btnSaveFileConfig.Location = new Point(235, 369);
            btnSaveFileConfig.Name = "btnSaveFileConfig";
            btnSaveFileConfig.Size = new Size(75, 23);
            btnSaveFileConfig.TabIndex = 13;
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
            panel1.Size = new Size(454, 389);
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
            btnStartStop.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnStartStop.ForeColor = SystemColors.Control;
            btnStartStop.Location = new Point(3, 3);
            btnStartStop.Name = "btnStartStop";
            btnStartStop.Size = new Size(75, 25);
            btnStartStop.TabIndex = 14;
            btnStartStop.Text = "Start";
            btnStartStop.UseVisualStyleBackColor = false;
            btnStartStop.Click += btnStartStop_Click;
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
            gridMessage.Size = new Size(446, 353);
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
            // notifyIcon
            // 
            notifyIcon.Icon = (Icon)resources.GetObject("notifyIcon.Icon");
            notifyIcon.Text = "WVAttendance";
            notifyIcon.Visible = true;
            notifyIcon.MouseDoubleClick += notifyIcon_MouseDoubleClick;
            // 
            // Main
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 399);
            Controls.Add(panel1);
            Controls.Add(groupConfig);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "Main";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Sync Attendance Data";
            Load += Main_Load;
            Resize += Main_Resize;
            groupConfig.ResumeLayout(false);
            groupConfig.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)gridMessage).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupConfig;
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
        private Label label9;
        private Button btnSelectSqlTemplate;
        private TextBox xSqlTemplatePath;
        private CheckBox xSyncUsers;
        private CheckBox xSyncAttendance;
        private NotifyIcon notifyIcon;
    }
}
