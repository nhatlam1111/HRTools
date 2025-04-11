using HRImportData.Classes;
using HRImportData.Controllers;
using HRImportData.Forms;
using System.Data;
using System.Windows.Forms;

namespace HRImportData
{
    public partial class Main2 : Form
    {
        public Main2()
        {
            InitializeComponent();

            ctrSiteVersion.SelectedItem = "GASP";
            Helper.LoadLoginInfo();
            Helper.LoadTnsFile();

            ctrClientSites.Items.Clear();
            ctrClientSites.Items.AddRange(new List<string>(Helper.clientList.Keys.OrderBy(q => q)).ToArray());

            dgvSavedLogin.DataSource = null;

            var bindingSource = new BindingSource();
            bindingSource.DataSource = Helper.loginInfos;
            dgvSavedLogin.DataSource = bindingSource;

            SetGridLogin();

        }

        private void SetGridLogin()
        {
            foreach (DataGridViewColumn col in dgvSavedLogin.Columns)
            {
                if (col.DataPropertyName == "DbUserPass" || col.DataPropertyName == "SiteUserPass" || col.DataPropertyName == "SiteUserName")
                {
                    col.Visible = false;
                }

                if (col.DataPropertyName == "DbUserName")
                {
                    col.HeaderText = "Database User";
                    col.Width = 150;
                }

                //if (col.DataPropertyName == "SiteUserName")
                //{
                //    col.HeaderText = "Web User";
                //}

                if (col.DataPropertyName == "SiteVersion")
                {
                    col.HeaderText = "Version";
                }

                col.ReadOnly = true;
            }

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string site = ctrClientSites.SelectedItem + "";
            string connectionString = !string.IsNullOrEmpty(site)
                ? string.Format("Data Source={0};User Id={1};Password={2};", Helper.clientList[site].Replace("\r\n", "").Replace(" ", ""), ctrDbUserName.Text, ctrDbUserPass.Text)
                : "";
            if (string.IsNullOrEmpty(site))
            {
                MessageBox.Show("Please select client site.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!CheckInputDatabase()) return;
            if (!CheckInputSiteUser()) return;

            MainController.CheckConnectDatabase(connectionString);
            if (!MainController.isConnectDatabase)
            {
                MessageBox.Show("Connect to client fail.", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                LogController.Error($"Connect to client fail. [{site}-{ctrDbUserName.Text}]");
                return;
            }

            var loginInfo = new LoginInfo()
            {
                Site = site,
                DbUserName = ctrDbUserName.Text,
                DbUserPass = ctrDbUserPass.Text,
                SiteUserName = ctrSiteUser.Text,
                SiteUserPass = ctrSitePass.Text,
                SiteVersion = (SITE_VERSION)ctrSiteVersion.SelectedIndex
            };


            MainController.Login(loginInfo);

            if (MainController.isSiteLogin)
            {
                //Login success
                //MessageBox.Show("Connected.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ctrGroupSavedLogin.Enabled = false;
                ctrGroupClient.Enabled = false;
                ctrGroupWeb.Enabled = false;

                if (ctrRememberLogin.Checked)
                {
                    Helper.SaveLoginInfo(loginInfo);
                }


                //login success
                var importForm = new ImportMain();
                this.Hide();
                importForm.ShowDialog();
                this.Close();
            }
            else
            {
                MessageBox.Show("Wrong user or password.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogController.Error($"Wrong user or password. [{DatabaseHelper.site_user_name}]");
            }
        }

        private bool CheckInputDatabase()
        {
            if (string.IsNullOrEmpty(ctrDbUserName.Text)
                    || string.IsNullOrEmpty(ctrDbUserPass.Text)
                )
            {
                MessageBox.Show("Please input database information.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            return true;
        }

        private bool CheckInputSiteUser()
        {
            if (
                    string.IsNullOrEmpty(ctrSiteUser.Text)
                    || string.IsNullOrEmpty(ctrSitePass.Text)
                )
            {
                MessageBox.Show("Please input site user information.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            DatabaseHelper.site_user_name = ctrSiteUser.Text;
            DatabaseHelper.site_user_pass = ctrSitePass.Text;
            DatabaseHelper.site_version = (SITE_VERSION)ctrSiteVersion.SelectedIndex;

            return true;
        }

        private void Main2_FormClosing(object sender, FormClosingEventArgs e)
        {
            LogController.Information("Logout: {0}", $"{DatabaseHelper.site_user_name}");
        }

        private void dgvSavedLogin_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                LoginInfo loginInfo = (LoginInfo)dgvSavedLogin.Rows[e.RowIndex].DataBoundItem;
                ctrClientSites.SelectedItem = loginInfo.Site;
                ctrDbUserName.Text = loginInfo.DbUserName;
                ctrDbUserPass.Text = loginInfo.DbUserPass;
                ctrSiteUser.Text = loginInfo.SiteUserName;
                ctrSitePass.Text = loginInfo.SiteUserPass;
                ctrSiteVersion.SelectedIndex = (int)loginInfo.SiteVersion;
            }
            catch (Exception ex) //do click vo header
            {
                //LogController.Error(ex.Message);
            }

        }
    }
}
