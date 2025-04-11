using HRImportData.Classes;
using HRImportData.Controllers;
using HRImportData.Forms;

namespace HRImportData
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();

            var versionBinding = new BindingSource();
            versionBinding.DataSource = MainController.SiteVersions();

            ctrSiteVersion.DataSource = versionBinding;
            ctrSiteVersion.DisplayMember = "display";
            ctrSiteVersion.ValueMember = "value";


            /*string ss = EncryptionHelper.GetMD5Base64Hash("Negima12");

            string a = EncryptionHelper.GetMD5Base64Hash("1055");
            string b =  BCrypt.Net.BCrypt.HashPassword("123456");

            var bb = BCrypt.Net.BCrypt.Verify("1234567", "$2a$10$EpMsRirUFgtE6dYstonfhO/Gsy3XTK78/nxsZdL/SAz55WceWBQg.");*/
        }

        private void ctrPort_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
            (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private bool CheckInputDatabase()
        {
            if (
                    string.IsNullOrEmpty(ctrHost.Text)
                    || string.IsNullOrEmpty(ctrPort.Text)
                    || string.IsNullOrEmpty(ctrSID.Text)
                    || string.IsNullOrEmpty(ctrUserName.Text)
                    || string.IsNullOrEmpty(ctrUserPass.Text)
                )
            {
                MessageBox.Show("Please input database information.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            DatabaseHelper.host = ctrHost.Text;
            DatabaseHelper.port = int.Parse(ctrPort.Text);
            DatabaseHelper.service_name = ctrSID.Text;
            DatabaseHelper.user_name = ctrUserName.Text;
            DatabaseHelper.user_password = ctrUserPass.Text;

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
            DatabaseHelper.site_version = (SITE_VERSION)ctrSiteVersion.SelectedValue;

            return true;
        }

        private void btnCheckConnect_Click(object sender, EventArgs e)
        {
            if (!CheckInputDatabase()) return;

            MainController.CheckConnectDatabase();

            if (MainController.isConnectDatabase)
            {
                MessageBox.Show("Connected.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                MainController.EnableControl(ctrDatabaseInfo, false);
                btnSaveConfig.Enabled = true; ;
                //ctrDatabaseInfo.Enabled = false;
            }
            else
            {
                MessageBox.Show("Connection fail.", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (!MainController.isConnectDatabase)
            {
                if (!CheckInputDatabase()) return;

                MainController.CheckConnectDatabase();
            }

            if (MainController.isConnectDatabase)
            {
                MainController.EnableControl(ctrDatabaseInfo, false);
                

                if (!CheckInputSiteUser()) return;

                MainController.Login();

                if (MainController.isSiteLogin)
                {
                    MessageBox.Show("Connected.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ctrSiteInfo.Enabled = false;
                    ctrFunctions.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Wrong user or password.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                btnSaveConfig.Enabled = true;
            }
        }

        private void btnLoadConfig_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = Directory.GetCurrentDirectory();
            openFileDialog1.Filter = "Config file(*.config)|*.config";
            openFileDialog1.FilterIndex = 0;

            if (openFileDialog1.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            var path = openFileDialog1.FileName;
            try
            {
                DatabaseHelper.LoadConfig(path);
                ctrHost.Text = DatabaseHelper.host;
                ctrPort.Text = DatabaseHelper.port + "";
                ctrSID.Text = DatabaseHelper.service_name;
                ctrUserName.Text = DatabaseHelper.user_name;
                ctrUserPass.Text = DatabaseHelper.user_password;

                ctrSiteUser.Text = DatabaseHelper.site_user_name ;
                ctrSitePass.Text = DatabaseHelper.site_user_pass ;
                ctrSiteVersion.SelectedValue = DatabaseHelper.site_version ;
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnSaveConfig_Click(object sender, EventArgs e)
        {
            if (!CheckInputDatabase()) return;
            if (!CheckInputSiteUser()) return;
            DatabaseHelper.SaveConfig();
        }

        private void btnImportBasicSalary_Click(object sender, EventArgs e)
        {
            frmImportSalary form = new frmImportSalary();
            //form.WindowState = FormWindowState.Maximized;
            form.Owner = this;
            form.ShowDialog();
        }

        private void btnImportTimeTemp_Click(object sender, EventArgs e)
        {
            frmImportTimeTemp form = new frmImportTimeTemp();
            //form.WindowState = FormWindowState.Maximized;
            form.Owner = this;
            form.ShowDialog();
        }

        private void ctrSiteVersion_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                DatabaseHelper.site_version = (SITE_VERSION)ctrSiteVersion.SelectedValue;
            }
            catch
            { 
            
            }
            
        }
    }
}
