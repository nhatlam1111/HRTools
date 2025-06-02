namespace UnionApp
{
    partial class Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnStartStop = new System.Windows.Forms.Button();
            this.xListenPort = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.xDbPass = new System.Windows.Forms.TextBox();
            this.xDbUser = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.xClientList = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.gridDevice = new System.Windows.Forms.DataGridView();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.gridMonitoring = new System.Windows.Forms.DataGridView();
            this.label5 = new System.Windows.Forms.Label();
            this.btnStopCommand = new System.Windows.Forms.Button();
            this.gridLog = new System.Windows.Forms.DataGridView();
            this.TIME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Message = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridCommand = new System.Windows.Forms.DataGridView();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridDevice)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridMonitoring)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridLog)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridCommand)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btnStartStop);
            this.groupBox1.Controls.Add(this.xListenPort);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.xDbPass);
            this.groupBox1.Controls.Add(this.xDbUser);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.xClientList);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox1.Location = new System.Drawing.Point(12, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(138, 543);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.RoyalBlue;
            this.label1.Location = new System.Drawing.Point(6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 20);
            this.label1.TabIndex = 9;
            this.label1.Text = "Server";
            // 
            // btnStartStop
            // 
            this.btnStartStop.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnStartStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStartStop.ForeColor = System.Drawing.Color.White;
            this.btnStartStop.Location = new System.Drawing.Point(5, 20);
            this.btnStartStop.Name = "btnStartStop";
            this.btnStartStop.Size = new System.Drawing.Size(127, 34);
            this.btnStartStop.TabIndex = 8;
            this.btnStartStop.Text = "Start";
            this.btnStartStop.UseVisualStyleBackColor = false;
            this.btnStartStop.Click += new System.EventHandler(this.btnStartStop_Click);
            // 
            // xListenPort
            // 
            this.xListenPort.Location = new System.Drawing.Point(6, 208);
            this.xListenPort.Name = "xListenPort";
            this.xListenPort.Size = new System.Drawing.Size(126, 20);
            this.xListenPort.TabIndex = 7;
            this.xListenPort.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.xListenPort_KeyPress);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 192);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Listener Port";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 142);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Password";
            // 
            // xDbPass
            // 
            this.xDbPass.Location = new System.Drawing.Point(6, 158);
            this.xDbPass.Name = "xDbPass";
            this.xDbPass.PasswordChar = '*';
            this.xDbPass.Size = new System.Drawing.Size(126, 20);
            this.xDbPass.TabIndex = 4;
            // 
            // xDbUser
            // 
            this.xDbUser.Location = new System.Drawing.Point(5, 109);
            this.xDbUser.Name = "xDbUser";
            this.xDbUser.Size = new System.Drawing.Size(127, 20);
            this.xDbUser.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 93);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "User Name";
            // 
            // xClientList
            // 
            this.xClientList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.xClientList.FormattingEnabled = true;
            this.xClientList.Location = new System.Drawing.Point(5, 60);
            this.xClientList.Name = "xClientList";
            this.xClientList.Size = new System.Drawing.Size(127, 21);
            this.xClientList.TabIndex = 1;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.gridDevice);
            this.groupBox2.Location = new System.Drawing.Point(156, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(321, 546);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.RoyalBlue;
            this.label8.Location = new System.Drawing.Point(6, -3);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(63, 20);
            this.label8.TabIndex = 7;
            this.label8.Text = "Device";
            // 
            // gridDevice
            // 
            this.gridDevice.AllowUserToAddRows = false;
            this.gridDevice.AllowUserToDeleteRows = false;
            this.gridDevice.AllowUserToResizeRows = false;
            this.gridDevice.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridDevice.EnableHeadersVisualStyles = false;
            this.gridDevice.Location = new System.Drawing.Point(6, 20);
            this.gridDevice.MultiSelect = false;
            this.gridDevice.Name = "gridDevice";
            this.gridDevice.RowHeadersVisible = false;
            this.gridDevice.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.gridDevice.Size = new System.Drawing.Size(309, 520);
            this.gridDevice.TabIndex = 6;
            this.gridDevice.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.gridDevice_RowPrePaint);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.gridMonitoring);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox3.Location = new System.Drawing.Point(483, 6);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(484, 546);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            // 
            // gridMonitoring
            // 
            this.gridMonitoring.AllowUserToAddRows = false;
            this.gridMonitoring.AllowUserToDeleteRows = false;
            this.gridMonitoring.AllowUserToResizeRows = false;
            this.gridMonitoring.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridMonitoring.Location = new System.Drawing.Point(6, 20);
            this.gridMonitoring.Name = "gridMonitoring";
            this.gridMonitoring.ReadOnly = true;
            this.gridMonitoring.RowHeadersVisible = false;
            this.gridMonitoring.Size = new System.Drawing.Size(472, 520);
            this.gridMonitoring.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.RoyalBlue;
            this.label5.Location = new System.Drawing.Point(6, -3);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(93, 20);
            this.label5.TabIndex = 1;
            this.label5.Text = "Monitoring";
            // 
            // btnStopCommand
            // 
            this.btnStopCommand.Location = new System.Drawing.Point(318, -1);
            this.btnStopCommand.Name = "btnStopCommand";
            this.btnStopCommand.Size = new System.Drawing.Size(75, 20);
            this.btnStopCommand.TabIndex = 9;
            this.btnStopCommand.Text = "Stop";
            this.btnStopCommand.UseVisualStyleBackColor = true;
            // 
            // gridLog
            // 
            this.gridLog.AllowUserToAddRows = false;
            this.gridLog.AllowUserToDeleteRows = false;
            this.gridLog.AllowUserToResizeRows = false;
            this.gridLog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridLog.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.TIME,
            this.Message});
            this.gridLog.Location = new System.Drawing.Point(6, 334);
            this.gridLog.Name = "gridLog";
            this.gridLog.RowHeadersVisible = false;
            this.gridLog.Size = new System.Drawing.Size(387, 206);
            this.gridLog.TabIndex = 10;
            // 
            // TIME
            // 
            this.TIME.Frozen = true;
            this.TIME.HeaderText = "Time";
            this.TIME.Name = "TIME";
            this.TIME.ReadOnly = true;
            this.TIME.Width = 120;
            // 
            // Message
            // 
            this.Message.HeaderText = "Message";
            this.Message.Name = "Message";
            this.Message.ReadOnly = true;
            this.Message.Width = 240;
            // 
            // gridCommand
            // 
            this.gridCommand.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridCommand.Location = new System.Drawing.Point(6, 19);
            this.gridCommand.Name = "gridCommand";
            this.gridCommand.Size = new System.Drawing.Size(387, 294);
            this.gridCommand.TabIndex = 8;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label7.Location = new System.Drawing.Point(6, 316);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(96, 15);
            this.label7.TabIndex = 4;
            this.label7.Text = "Log Message(s)";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label9);
            this.groupBox4.Controls.Add(this.btnStopCommand);
            this.groupBox4.Controls.Add(this.gridLog);
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Controls.Add(this.gridCommand);
            this.groupBox4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox4.Location = new System.Drawing.Point(973, 6);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(399, 546);
            this.groupBox4.TabIndex = 11;
            this.groupBox4.TabStop = false;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.RoyalBlue;
            this.label9.Location = new System.Drawing.Point(6, -3);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(97, 20);
            this.label9.TabIndex = 1;
            this.label9.Text = "Processing";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1384, 561);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Union App";
            this.Load += new System.EventHandler(this.Main_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridDevice)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridMonitoring)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridLog)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridCommand)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox xDbPass;
        private System.Windows.Forms.TextBox xDbUser;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox xClientList;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox xListenPort;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnStartStop;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridView gridMonitoring;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DataGridView gridLog;
        private System.Windows.Forms.DataGridView gridCommand;
        private System.Windows.Forms.DataGridView gridDevice;
        private System.Windows.Forms.Button btnStopCommand;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.DataGridViewTextBoxColumn TIME;
        private System.Windows.Forms.DataGridViewTextBoxColumn Message;
    }
}

