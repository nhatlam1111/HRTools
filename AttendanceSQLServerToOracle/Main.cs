using AttendanceAccessToOracle.classes;
using AttendanceAccessToOracle.controllers;
using Helpers;
using Helpers.controllers;

namespace AttendanceAccessToOracle
{
    public partial class Main : Form
    {
        string configFile = $"{AppDomain.CurrentDomain.BaseDirectory}\\config.dat";

        public Main()
        {
            InitializeComponent();
            LogController.Start("");
            LogController.DisplayForm = this;
            LogController.DisplayText = lblCurrentLog;
            MainController.mainForm = this;
            MainController.mainGridView = gridMessage;
        }

        private void Main_Load(object sender, EventArgs e)
        {
            Helper.LoadTnsFile();

            xClientList.Items.Clear();
            xClientList.Items.AddRange(new List<string>(Helper.clientList.Keys.OrderBy(q => q)).ToArray());

            LoadConfig();
            LogController.Information("Application started.", true);

            if (MainController.config != null)
            {
                btnStartStop_Click(null, null);
            }
        }

        private void btnSaveFileConfig_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                Config config = new Config
                {
                    Client = xClientList.SelectedItem + "",
                    DbUser = xDbUser.Text,
                    DbPass = xDbPass.Text,
                    SqlHost = xSqlHost.Text,
                    SqlPort = xSqlPort.Text,
                    SqlService = xSqlDatabase.Text,
                    SqlUserName = xSqlUserName.Text,
                    SqlPassword = xSqlPassword.Text,
                    SyncDays = int.Parse(xSyncDays.Text ?? "0"),
                    SyncMinutes = int.Parse(xSyncEachMinutes.Text ?? "0"),
                    SqlPath = xSqlTemplatePath.Text,
                    SyncAttendance = xSyncAttendance.Checked,
                    SyncUser = xSyncUsers.Checked
                };

                Helper.WriteObjectToFile(config, configFile);

                MessageBox.Show("saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void LoadConfig()
        {
            try
            {
                if (!File.Exists(configFile))
                {
                    return;
                }

                Config config = Helper.ReadObjectFromFile<Config>(configFile, true);
                xClientList.SelectedItem = config.Client;
                xDbUser.Text = config.DbUser;
                xDbPass.Text = config.DbPass;
                xSqlHost.Text = config.SqlHost;
                xSqlPort.Text = config.SqlPort;
                xSqlDatabase.Text = config.SqlService;
                xSqlUserName.Text = config.SqlUserName;
                xSqlPassword.Text = config.SqlPassword;
                xSyncDays.Text = config.SyncDays.ToString();
                xSyncEachMinutes.Text = config.SyncMinutes.ToString();
                xSqlTemplatePath.Text = config.SqlPath;
                xSyncUsers.Checked = config.SyncUser;
                xSyncAttendance.Checked = config.SyncAttendance;

                MainController.config = config;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private bool ValidateInput()
        {
            if (string.IsNullOrEmpty(xClientList.SelectedItem + ""))
            {
                MessageBox.Show("Please select Server.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrEmpty(xDbUser.Text))
            {
                MessageBox.Show("Please enter User Name.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrEmpty(xDbPass.Text))
            {
                MessageBox.Show("Please enter Password.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrEmpty(xSqlHost.Text))
            {
                MessageBox.Show("Please enter Sql Host.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrEmpty(xSqlPort.Text))
            {
                MessageBox.Show("Please enter Sql Port.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrEmpty(xSqlDatabase.Text))
            {
                MessageBox.Show("Please enter Sql Database.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrEmpty(xSqlUserName.Text))
            {
                MessageBox.Show("Please enter Sql User Name.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrEmpty(xSqlPassword.Text))
            {
                MessageBox.Show("Please enter Sql Password.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrEmpty(xSqlTemplatePath.Text))
            {
                MessageBox.Show("Please select Sql Template.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!xSyncUsers.Checked && !xSyncAttendance.Checked)
            {
                MessageBox.Show("Please select Sync type.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void xSyncDays_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Check for the flag being set in the KeyDown event.
            if (!Char.IsNumber(e.KeyChar) && e.KeyChar != '\b')
            {
                // Stop the character from being entered into the control since it is non-numerical.
                e.Handled = true;
            }
        }

        private void xSyncEachMinutes_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Check for the flag being set in the KeyDown event.
            if (!Char.IsNumber(e.KeyChar) && e.KeyChar != '\b')
            {
                // Stop the character from being entered into the control since it is non-numerical.
                e.Handled = true;
            }
        }

        private void btnSelectSqlTemplate_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = Directory.GetCurrentDirectory();
            openFileDialog1.Filter = "Sql(*.sql)|*.sql";
            openFileDialog1.FilterIndex = 0;

            if (openFileDialog1.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            var path = openFileDialog1.FileName;

            xSqlTemplatePath.Text = path;
        }

        private void btnStartStop_Click(object sender, EventArgs e)
        {
            if (btnStartStop.Text == "Start")
            {
                if (!ValidateInput())
                {
                    return;
                }

                Config config = new Config
                {
                    Client = xClientList.SelectedItem + "",
                    DbUser = xDbUser.Text,
                    DbPass = xDbPass.Text,
                    SqlHost = xSqlHost.Text,
                    SqlPort = xSqlPort.Text,
                    SqlService = xSqlDatabase.Text,
                    SqlUserName = xSqlUserName.Text,
                    SqlPassword = xSqlPassword.Text,
                    SyncDays = int.Parse(xSyncDays.Text ?? "0"),
                    SyncMinutes = int.Parse(xSyncEachMinutes.Text ?? "0"),
                    SqlPath = xSqlTemplatePath.Text,
                    SyncAttendance = xSyncAttendance.Checked,
                    SyncUser = xSyncUsers.Checked
                };

                MainController.config = config;

                btnStartStop.Text = "Stop";
                lblStatus.Text = "Running...";
                lblStatus.ForeColor = Color.RoyalBlue;
                btnStartStop.BackColor = Color.OrangeRed;
                groupConfig.Enabled = false;
                MainController.Start();
            }
            else
            {
                btnStartStop.Text = "Start";
                lblStatus.Text = "Stopped";
                lblStatus.ForeColor = Color.OrangeRed;
                btnStartStop.BackColor = Color.RoyalBlue;
                groupConfig.Enabled = true;
                MainController.Stop();
            }

        }

        private void Main_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
            {
                notifyIcon.Visible = true;
                this.Hide();
            }
            else if (FormWindowState.Normal == this.WindowState)
            {
                notifyIcon.Visible = false;
            }
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            WindowState = FormWindowState.Normal;

            if (FormWindowState.Minimized == this.WindowState)
            {
                notifyIcon.Visible = true;
                //mynotifyicon.ShowBalloonTip(500);
                this.Hide();
            }

            else if (FormWindowState.Normal == this.WindowState)
            {
                notifyIcon.Visible = false;
            }
        }
    }
}
