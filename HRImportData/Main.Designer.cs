namespace HRImportData
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
            ctrDatabaseInfo = new GroupBox();
            btnSaveConfig = new Button();
            btnLoadConfig = new Button();
            btnCheckConnect = new Button();
            label4 = new Label();
            label5 = new Label();
            ctrUserPass = new TextBox();
            ctrUserName = new TextBox();
            label3 = new Label();
            ctrSID = new TextBox();
            label2 = new Label();
            label1 = new Label();
            ctrPort = new TextBox();
            ctrHost = new TextBox();
            ctrSiteInfo = new GroupBox();
            ctrSiteVersion = new ComboBox();
            label8 = new Label();
            btnLogin = new Button();
            label6 = new Label();
            label7 = new Label();
            ctrSitePass = new TextBox();
            ctrSiteUser = new TextBox();
            ctrFunctions = new GroupBox();
            btnImportEmployeePhoto = new Button();
            btnImportTimeTemp = new Button();
            btnImportBasicSalary = new Button();
            ctrDatabaseInfo.SuspendLayout();
            ctrSiteInfo.SuspendLayout();
            ctrFunctions.SuspendLayout();
            SuspendLayout();
            // 
            // ctrDatabaseInfo
            // 
            ctrDatabaseInfo.Controls.Add(btnSaveConfig);
            ctrDatabaseInfo.Controls.Add(btnLoadConfig);
            ctrDatabaseInfo.Controls.Add(btnCheckConnect);
            ctrDatabaseInfo.Controls.Add(label4);
            ctrDatabaseInfo.Controls.Add(label5);
            ctrDatabaseInfo.Controls.Add(ctrUserPass);
            ctrDatabaseInfo.Controls.Add(ctrUserName);
            ctrDatabaseInfo.Controls.Add(label3);
            ctrDatabaseInfo.Controls.Add(ctrSID);
            ctrDatabaseInfo.Controls.Add(label2);
            ctrDatabaseInfo.Controls.Add(label1);
            ctrDatabaseInfo.Controls.Add(ctrPort);
            ctrDatabaseInfo.Controls.Add(ctrHost);
            ctrDatabaseInfo.Location = new Point(12, 12);
            ctrDatabaseInfo.Name = "ctrDatabaseInfo";
            ctrDatabaseInfo.Size = new Size(319, 184);
            ctrDatabaseInfo.TabIndex = 0;
            ctrDatabaseInfo.TabStop = false;
            ctrDatabaseInfo.Text = "Database User";
            // 
            // btnSaveConfig
            // 
            btnSaveConfig.Location = new Point(87, 154);
            btnSaveConfig.Name = "btnSaveConfig";
            btnSaveConfig.Size = new Size(69, 23);
            btnSaveConfig.TabIndex = 60;
            btnSaveConfig.Text = "Save";
            btnSaveConfig.UseVisualStyleBackColor = true;
            btnSaveConfig.Click += btnSaveConfig_Click;
            // 
            // btnLoadConfig
            // 
            btnLoadConfig.Location = new Point(6, 154);
            btnLoadConfig.Name = "btnLoadConfig";
            btnLoadConfig.Size = new Size(75, 23);
            btnLoadConfig.TabIndex = 50;
            btnLoadConfig.Text = "Load";
            btnLoadConfig.UseVisualStyleBackColor = true;
            btnLoadConfig.Click += btnLoadConfig_Click;
            // 
            // btnCheckConnect
            // 
            btnCheckConnect.Location = new Point(162, 154);
            btnCheckConnect.Name = "btnCheckConnect";
            btnCheckConnect.Size = new Size(150, 23);
            btnCheckConnect.TabIndex = 70;
            btnCheckConnect.Text = "Check Connect";
            btnCheckConnect.UseVisualStyleBackColor = true;
            btnCheckConnect.Visible = false;
            btnCheckConnect.Click += btnCheckConnect_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(162, 107);
            label4.Name = "label4";
            label4.Size = new Size(57, 15);
            label4.TabIndex = 9;
            label4.Text = "Password";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(6, 107);
            label5.Name = "label5";
            label5.Size = new Size(65, 15);
            label5.TabIndex = 8;
            label5.Text = "User Name";
            // 
            // ctrUserPass
            // 
            ctrUserPass.Location = new Point(162, 125);
            ctrUserPass.Name = "ctrUserPass";
            ctrUserPass.PasswordChar = '*';
            ctrUserPass.Size = new Size(150, 23);
            ctrUserPass.TabIndex = 40;
            // 
            // ctrUserName
            // 
            ctrUserName.Location = new Point(6, 125);
            ctrUserName.Name = "ctrUserName";
            ctrUserName.Size = new Size(150, 23);
            ctrUserName.TabIndex = 30;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(6, 63);
            label3.Name = "label3";
            label3.Size = new Size(24, 15);
            label3.TabIndex = 5;
            label3.Text = "SID";
            // 
            // ctrSID
            // 
            ctrSID.Location = new Point(6, 81);
            ctrSID.Name = "ctrSID";
            ctrSID.Size = new Size(306, 23);
            ctrSID.TabIndex = 20;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(162, 19);
            label2.Name = "label2";
            label2.Size = new Size(29, 15);
            label2.TabIndex = 3;
            label2.Text = "Port";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(6, 19);
            label1.Name = "label1";
            label1.Size = new Size(32, 15);
            label1.TabIndex = 2;
            label1.Text = "Host";
            // 
            // ctrPort
            // 
            ctrPort.Location = new Point(162, 37);
            ctrPort.Name = "ctrPort";
            ctrPort.Size = new Size(150, 23);
            ctrPort.TabIndex = 10;
            ctrPort.KeyPress += ctrPort_KeyPress;
            // 
            // ctrHost
            // 
            ctrHost.Location = new Point(6, 37);
            ctrHost.Name = "ctrHost";
            ctrHost.Size = new Size(150, 23);
            ctrHost.TabIndex = 0;
            // 
            // ctrSiteInfo
            // 
            ctrSiteInfo.Controls.Add(ctrSiteVersion);
            ctrSiteInfo.Controls.Add(label8);
            ctrSiteInfo.Controls.Add(btnLogin);
            ctrSiteInfo.Controls.Add(label6);
            ctrSiteInfo.Controls.Add(label7);
            ctrSiteInfo.Controls.Add(ctrSitePass);
            ctrSiteInfo.Controls.Add(ctrSiteUser);
            ctrSiteInfo.Location = new Point(337, 12);
            ctrSiteInfo.Name = "ctrSiteInfo";
            ctrSiteInfo.Size = new Size(162, 184);
            ctrSiteInfo.TabIndex = 1;
            ctrSiteInfo.TabStop = false;
            ctrSiteInfo.Text = "Web login user";
            // 
            // ctrSiteVersion
            // 
            ctrSiteVersion.DropDownStyle = ComboBoxStyle.DropDownList;
            ctrSiteVersion.FormattingEnabled = true;
            ctrSiteVersion.Items.AddRange(new object[] { "NODEJS", "GASP", "ESYS" });
            ctrSiteVersion.Location = new Point(6, 125);
            ctrSiteVersion.Name = "ctrSiteVersion";
            ctrSiteVersion.Size = new Size(150, 23);
            ctrSiteVersion.TabIndex = 100;
            ctrSiteVersion.SelectedValueChanged += ctrSiteVersion_SelectedValueChanged;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(6, 107);
            label8.Name = "label8";
            label8.Size = new Size(67, 15);
            label8.TabIndex = 14;
            label8.Text = "Site version";
            // 
            // btnLogin
            // 
            btnLogin.Location = new Point(6, 154);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(150, 23);
            btnLogin.TabIndex = 110;
            btnLogin.Text = "Login";
            btnLogin.UseVisualStyleBackColor = true;
            btnLogin.Click += btnLogin_Click;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(6, 63);
            label6.Name = "label6";
            label6.Size = new Size(57, 15);
            label6.TabIndex = 13;
            label6.Text = "Password";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(6, 19);
            label7.Name = "label7";
            label7.Size = new Size(65, 15);
            label7.TabIndex = 12;
            label7.Text = "User Name";
            // 
            // ctrSitePass
            // 
            ctrSitePass.Location = new Point(6, 81);
            ctrSitePass.Name = "ctrSitePass";
            ctrSitePass.PasswordChar = '*';
            ctrSitePass.Size = new Size(150, 23);
            ctrSitePass.TabIndex = 90;
            // 
            // ctrSiteUser
            // 
            ctrSiteUser.Location = new Point(6, 37);
            ctrSiteUser.Name = "ctrSiteUser";
            ctrSiteUser.Size = new Size(150, 23);
            ctrSiteUser.TabIndex = 80;
            // 
            // ctrFunctions
            // 
            ctrFunctions.Controls.Add(btnImportEmployeePhoto);
            ctrFunctions.Controls.Add(btnImportTimeTemp);
            ctrFunctions.Controls.Add(btnImportBasicSalary);
            ctrFunctions.Enabled = false;
            ctrFunctions.Location = new Point(505, 12);
            ctrFunctions.Name = "ctrFunctions";
            ctrFunctions.Size = new Size(162, 184);
            ctrFunctions.TabIndex = 2;
            ctrFunctions.TabStop = false;
            ctrFunctions.Text = "Import Functions";
            // 
            // btnImportEmployeePhoto
            // 
            btnImportEmployeePhoto.Location = new Point(6, 80);
            btnImportEmployeePhoto.Name = "btnImportEmployeePhoto";
            btnImportEmployeePhoto.Size = new Size(150, 23);
            btnImportEmployeePhoto.TabIndex = 140;
            btnImportEmployeePhoto.Text = "Employee Photo";
            btnImportEmployeePhoto.UseVisualStyleBackColor = true;
            btnImportEmployeePhoto.Visible = false;
            // 
            // btnImportTimeTemp
            // 
            btnImportTimeTemp.Location = new Point(6, 51);
            btnImportTimeTemp.Name = "btnImportTimeTemp";
            btnImportTimeTemp.Size = new Size(150, 23);
            btnImportTimeTemp.TabIndex = 130;
            btnImportTimeTemp.Text = "Time Temp";
            btnImportTimeTemp.UseVisualStyleBackColor = true;
            btnImportTimeTemp.Click += btnImportTimeTemp_Click;
            // 
            // btnImportBasicSalary
            // 
            btnImportBasicSalary.Location = new Point(6, 22);
            btnImportBasicSalary.Name = "btnImportBasicSalary";
            btnImportBasicSalary.Size = new Size(150, 23);
            btnImportBasicSalary.TabIndex = 120;
            btnImportBasicSalary.Text = "Basic Salary";
            btnImportBasicSalary.UseVisualStyleBackColor = true;
            btnImportBasicSalary.Click += btnImportBasicSalary_Click;
            // 
            // Main
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(674, 203);
            Controls.Add(ctrFunctions);
            Controls.Add(ctrSiteInfo);
            Controls.Add(ctrDatabaseInfo);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "Main";
            StartPosition = FormStartPosition.CenterScreen;
            ctrDatabaseInfo.ResumeLayout(false);
            ctrDatabaseInfo.PerformLayout();
            ctrSiteInfo.ResumeLayout(false);
            ctrSiteInfo.PerformLayout();
            ctrFunctions.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private GroupBox ctrDatabaseInfo;
        private TextBox ctrHost;
        private Label label4;
        private Label label5;
        private TextBox ctrUserPass;
        private TextBox ctrUserName;
        private Label label3;
        private TextBox ctrSID;
        private Label label2;
        private Label label1;
        private TextBox ctrPort;
        private Button btnCheckConnect;
        private GroupBox ctrSiteInfo;
        private Button btnLogin;
        private Label label6;
        private Label label7;
        private TextBox ctrSitePass;
        private TextBox ctrSiteUser;
        private GroupBox ctrFunctions;
        private Button btnImportTimeTemp;
        private Button btnImportBasicSalary;
        private Button btnImportEmployeePhoto;
        private ComboBox ctrSiteVersion;
        private Label label8;
        private Button btnSaveConfig;
        private Button btnLoadConfig;
    }
}
