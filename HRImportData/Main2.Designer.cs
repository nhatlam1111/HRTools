namespace HRImportData
{
    partial class Main2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main2));
            ctrGroupClient = new GroupBox();
            ctrRememberLogin = new CheckBox();
            ctrClientSites = new ComboBox();
            label4 = new Label();
            label5 = new Label();
            ctrDbUserPass = new TextBox();
            ctrDbUserName = new TextBox();
            ctrGroupWeb = new GroupBox();
            ctrSiteVersion = new ComboBox();
            label8 = new Label();
            btnLogin = new Button();
            label6 = new Label();
            label7 = new Label();
            ctrSitePass = new TextBox();
            ctrSiteUser = new TextBox();
            ctrGroupSavedLogin = new GroupBox();
            dgvSavedLogin = new DataGridView();
            ctrGroupClient.SuspendLayout();
            ctrGroupWeb.SuspendLayout();
            ctrGroupSavedLogin.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvSavedLogin).BeginInit();
            SuspendLayout();
            // 
            // ctrGroupClient
            // 
            ctrGroupClient.Controls.Add(ctrRememberLogin);
            ctrGroupClient.Controls.Add(ctrClientSites);
            ctrGroupClient.Controls.Add(label4);
            ctrGroupClient.Controls.Add(label5);
            ctrGroupClient.Controls.Add(ctrDbUserPass);
            ctrGroupClient.Controls.Add(ctrDbUserName);
            ctrGroupClient.Location = new Point(413, 12);
            ctrGroupClient.Name = "ctrGroupClient";
            ctrGroupClient.Size = new Size(314, 124);
            ctrGroupClient.TabIndex = 0;
            ctrGroupClient.TabStop = false;
            ctrGroupClient.Text = "Client";
            // 
            // ctrRememberLogin
            // 
            ctrRememberLogin.AutoSize = true;
            ctrRememberLogin.Checked = true;
            ctrRememberLogin.CheckState = CheckState.Checked;
            ctrRememberLogin.Location = new Point(224, 98);
            ctrRememberLogin.Name = "ctrRememberLogin";
            ctrRememberLogin.Size = new Size(84, 19);
            ctrRememberLogin.TabIndex = 8;
            ctrRememberLogin.Text = "Remember";
            ctrRememberLogin.UseVisualStyleBackColor = true;
            // 
            // ctrClientSites
            // 
            ctrClientSites.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            ctrClientSites.AutoCompleteSource = AutoCompleteSource.ListItems;
            ctrClientSites.FormattingEnabled = true;
            ctrClientSites.Location = new Point(6, 22);
            ctrClientSites.Name = "ctrClientSites";
            ctrClientSites.Size = new Size(302, 23);
            ctrClientSites.TabIndex = 1;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(160, 51);
            label4.Name = "label4";
            label4.Size = new Size(57, 15);
            label4.TabIndex = 42;
            label4.Text = "Password";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(4, 51);
            label5.Name = "label5";
            label5.Size = new Size(65, 15);
            label5.TabIndex = 41;
            label5.Text = "User Name";
            // 
            // ctrDbUserPass
            // 
            ctrDbUserPass.Location = new Point(160, 69);
            ctrDbUserPass.Name = "ctrDbUserPass";
            ctrDbUserPass.PasswordChar = '*';
            ctrDbUserPass.Size = new Size(148, 23);
            ctrDbUserPass.TabIndex = 4;
            // 
            // ctrDbUserName
            // 
            ctrDbUserName.Location = new Point(6, 69);
            ctrDbUserName.Name = "ctrDbUserName";
            ctrDbUserName.Size = new Size(148, 23);
            ctrDbUserName.TabIndex = 3;
            // 
            // ctrGroupWeb
            // 
            ctrGroupWeb.Controls.Add(ctrSiteVersion);
            ctrGroupWeb.Controls.Add(label8);
            ctrGroupWeb.Controls.Add(btnLogin);
            ctrGroupWeb.Controls.Add(label6);
            ctrGroupWeb.Controls.Add(label7);
            ctrGroupWeb.Controls.Add(ctrSitePass);
            ctrGroupWeb.Controls.Add(ctrSiteUser);
            ctrGroupWeb.Location = new Point(413, 142);
            ctrGroupWeb.Name = "ctrGroupWeb";
            ctrGroupWeb.Size = new Size(314, 118);
            ctrGroupWeb.TabIndex = 2;
            ctrGroupWeb.TabStop = false;
            ctrGroupWeb.Text = "Web login user";
            // 
            // ctrSiteVersion
            // 
            ctrSiteVersion.DropDownStyle = ComboBoxStyle.DropDownList;
            ctrSiteVersion.FormattingEnabled = true;
            ctrSiteVersion.Items.AddRange(new object[] { "NODEJS", "GASP", "ESYS" });
            ctrSiteVersion.Location = new Point(7, 85);
            ctrSiteVersion.Name = "ctrSiteVersion";
            ctrSiteVersion.Size = new Size(211, 23);
            ctrSiteVersion.TabIndex = 7;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(6, 67);
            label8.Name = "label8";
            label8.Size = new Size(67, 15);
            label8.TabIndex = 14;
            label8.Text = "Site version";
            // 
            // btnLogin
            // 
            btnLogin.Location = new Point(224, 85);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(84, 23);
            btnLogin.TabIndex = 9;
            btnLogin.Text = "Login";
            btnLogin.UseVisualStyleBackColor = true;
            btnLogin.Click += btnLogin_Click;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(160, 19);
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
            ctrSitePass.Location = new Point(160, 37);
            ctrSitePass.Name = "ctrSitePass";
            ctrSitePass.PasswordChar = '*';
            ctrSitePass.Size = new Size(148, 23);
            ctrSitePass.TabIndex = 6;
            // 
            // ctrSiteUser
            // 
            ctrSiteUser.Location = new Point(6, 37);
            ctrSiteUser.Name = "ctrSiteUser";
            ctrSiteUser.Size = new Size(148, 23);
            ctrSiteUser.TabIndex = 5;
            // 
            // ctrGroupSavedLogin
            // 
            ctrGroupSavedLogin.Controls.Add(dgvSavedLogin);
            ctrGroupSavedLogin.Location = new Point(12, 12);
            ctrGroupSavedLogin.Name = "ctrGroupSavedLogin";
            ctrGroupSavedLogin.Size = new Size(395, 248);
            ctrGroupSavedLogin.TabIndex = 3;
            ctrGroupSavedLogin.TabStop = false;
            ctrGroupSavedLogin.Text = "Saved Login";
            // 
            // dgvSavedLogin
            // 
            dgvSavedLogin.AllowUserToAddRows = false;
            dgvSavedLogin.AllowUserToDeleteRows = false;
            dgvSavedLogin.AllowUserToOrderColumns = true;
            dgvSavedLogin.AllowUserToResizeRows = false;
            dgvSavedLogin.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvSavedLogin.Location = new Point(6, 22);
            dgvSavedLogin.MultiSelect = false;
            dgvSavedLogin.Name = "dgvSavedLogin";
            dgvSavedLogin.RowHeadersVisible = false;
            dgvSavedLogin.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dgvSavedLogin.ShowEditingIcon = false;
            dgvSavedLogin.Size = new Size(383, 216);
            dgvSavedLogin.TabIndex = 114;
            dgvSavedLogin.CellClick += dgvSavedLogin_CellClick;
            // 
            // Main2
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(734, 271);
            Controls.Add(ctrGroupSavedLogin);
            Controls.Add(ctrGroupWeb);
            Controls.Add(ctrGroupClient);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "Main2";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "HR Import";
            FormClosing += Main2_FormClosing;
            ctrGroupClient.ResumeLayout(false);
            ctrGroupClient.PerformLayout();
            ctrGroupWeb.ResumeLayout(false);
            ctrGroupWeb.PerformLayout();
            ctrGroupSavedLogin.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvSavedLogin).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox ctrGroupClient;
        private Label label4;
        private Label label5;
        private TextBox ctrDbUserPass;
        private TextBox ctrDbUserName;
        private GroupBox ctrGroupWeb;
        private ComboBox ctrSiteVersion;
        private Label label8;
        private Button btnLogin;
        private Label label6;
        private Label label7;
        private TextBox ctrSitePass;
        private TextBox ctrSiteUser;
        private CheckBox ctrRememberLogin;
        private GroupBox ctrGroupSavedLogin;
        private ComboBox ctrClientSites;
        private DataGridView dgvSavedLogin;
    }
}