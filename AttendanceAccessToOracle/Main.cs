using AttendanceAccessToOracle.classes;
using Helpers;

namespace AttendanceAccessToOracle
{
    public partial class Main : Form
    {
        string configFile = $"{AppDomain.CurrentDomain.BaseDirectory}\\config.dat";

        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            Helper.LoadTnsFile();

            xClientList.Items.Clear();
            xClientList.Items.AddRange(new List<string>(Helper.clientList.Keys.OrderBy(q => q)).ToArray());

            LoadConfig();
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
                    AccessFilePath = xAccessFilePath.Text,
                    AccessFilePass = xAccessFilePass.Text,
                    SyncDays = int.Parse(xSyncDays.Text),
                    SyncMinutes = int.Parse(xSyncEachMinutes.Text)
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

                Config config = Helper.ReadObjectFromFile<Config>(configFile);
                xClientList.SelectedItem = config.Client;
                xDbUser.Text = config.DbUser;
                xDbPass.Text = config.DbPass;
                xAccessFilePath.Text = config.AccessFilePath;
                xAccessFilePass.Text = config.AccessFilePass;
                xSyncDays.Text = config.SyncDays.ToString();
                xSyncEachMinutes.Text = config.SyncMinutes.ToString();
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

            if (string.IsNullOrEmpty(xAccessFilePath.Text))
            {
                MessageBox.Show("Please select Access File.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private void btnSelectAccessFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = Directory.GetCurrentDirectory();
            openFileDialog1.Filter = "Access(*.mdb)|*.mdb";
            openFileDialog1.FilterIndex = 0;

            if (openFileDialog1.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            var path = openFileDialog1.FileName;

            xAccessFilePath.Text = path;
        }
    }
}
