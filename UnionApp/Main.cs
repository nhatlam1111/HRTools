using Helpers;
using Helpers.controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UnionApp.classes;
using UnionApp.controllers;

namespace UnionApp
{
    public partial class Main: Form
    {
        string configFile = $"{AppDomain.CurrentDomain.BaseDirectory}\\config.dat";
        string procedureFile = $"{AppDomain.CurrentDomain.BaseDirectory}\\procedure.dat";
        string sqlFile = $"{AppDomain.CurrentDomain.BaseDirectory}\\sql.dat";

        public Main()
        {
            InitializeComponent();

            LogController.DisplayForm = this;
            LogController.DisplayDataGridview = gridLog;

            LogController.Start("");
        }

        private void Main_Load(object sender, EventArgs e)
        {
            Helper.LoadTnsFile();

            xClientList.Items.Clear();
            xClientList.Items.AddRange(new List<string>(Helper.clientList.Keys.OrderBy(q => q)).ToArray());

            LoadConfig();
            LogController.Information("Application started.", true);


            BindingGridDataSource();
        }

        private void LoadConfig()
        {
            try
            {
                if (!File.Exists(configFile))
                {
                    return;
                }

                MainController.config = Helper.ReadObjectFromFile<Config>(configFile, true);
                xClientList.SelectedItem = MainController.config.Client;
                xDbUser.Text = MainController.config.DbUser;
                xDbPass.Text = MainController.config.DbPass;
                xListenPort.Text = MainController.config.listenerPort.ToString();
                
                if (!File.Exists(procedureFile)) return;

                TerminalController.procedure = Helper.ReadObjectFromFile<Procedure>(procedureFile, false);

                if (!File.Exists(sqlFile)) return;

                TerminalController.sqlTemplates = Helper.ReadObjectFromFile<SqlTemplates>(sqlFile, false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void SaveConfig()
        {
            MainController.config = new Config
            {
                Client = xClientList.SelectedItem + "",
                DbUser = xDbUser.Text,
                DbPass = xDbPass.Text,
                listenerPort = int.Parse(xListenPort.Text ?? "9870"),
            };

            Helper.WriteObjectToFile(MainController.config, configFile); 
            //MessageBox.Show("saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

            if (string.IsNullOrEmpty(xListenPort.Text))
            {
                MessageBox.Show("Please enter Listener Port.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void xListenPort_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Check for the flag being set in the KeyDown event.
            if (!Char.IsNumber(e.KeyChar) && e.KeyChar != '\b')
            {
                // Stop the character from being entered into the control since it is non-numerical.
                e.Handled = true;
            }
        }

        private async void btnStartStop_Click(object sender, EventArgs e)
        {
           

            if (btnStartStop.Text == "Start")
            {
                if (!ValidateInput()) return;
                SaveConfig();

                await MainController.Start();

                if(!OracleDb.IsConnected)
                {
                    MessageBox.Show("Failed to connect to the database. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                TerminalController.Start();

                if (TerminalController.running)
                { 
                    xClientList.Enabled = false;
                    xDbUser.Enabled = false;
                    xDbPass.Enabled = false;
                    xListenPort.Enabled = false;
                }

                btnStartStop.Text = "Stop";
                btnStartStop.BackColor = Color.OrangeRed;
            }
            else
            {
                TerminalController.Stop();

                xClientList.Enabled = true;
                xDbUser.Enabled = true;
                xDbPass.Enabled = true;
                xListenPort.Enabled = true;

                btnStartStop.Text = "Start";
                btnStartStop.BackColor = Color.RoyalBlue;
            }

        }

        private void BindingGridDataSource()
        { 
            Helper.BindListToGrid(TerminalController.devices, gridDevice, true);
            Helper.BindListToGrid(TerminalController.monitorings, gridMonitoring, true);

            List<string> HiddenDeviceColumns = new List<string> { "CompanyPk", "CompanyBranch", "AccessGroup" };
            for (int i = 0; i < gridDevice.Columns.Count; i++)
            {
                if (HiddenDeviceColumns.IndexOf(gridDevice.Columns[i].HeaderText) >= 0)
                {
                    gridDevice.Columns[i].Visible = false;
                }

                if (gridDevice.Columns[i].HeaderText == "Id" || gridDevice.Columns[i].HeaderText == "Code")
                {
                    gridDevice.Columns[i].Width = 40;
                    gridDevice.Columns[i].Frozen = true;
                }
            }

            for (int i = 0; i < gridMonitoring.Columns.Count; i++)
            {
                if (gridMonitoring.Columns[i].HeaderText == "Time")
                {
                    gridMonitoring.Columns[i].Width = 125;
                }
            }

            for (int i = 0; i < gridLog.Columns.Count; i++)
            {
                if (gridLog.Columns[i].HeaderText == "Time")
                {
                    gridLog.Columns[i].Width = 130;
                    gridLog.Columns[i].Frozen = true;
                }

                if (gridLog.Columns[i].HeaderText == "Message")
                {
                    gridLog.Columns[i].Width = 250;
                }
            }
        }

        private void gridDevice_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            int rowIndex = e.RowIndex;
            int statusColumnIdx = -1;

            for (int i = 0; i < gridDevice.Columns.Count; i++)
            { 
                if(gridDevice.Columns[i].HeaderText == "Status")
                {
                    statusColumnIdx = i;
                    break;
                }
            }

            if (rowIndex >= 0 && rowIndex < gridDevice.Rows.Count)
            {
                DataGridViewRow row = gridDevice.Rows[rowIndex];
                if (row.Cells[statusColumnIdx].Value != null && (DEVICE_STATUS)row.Cells[statusColumnIdx].Value == DEVICE_STATUS.DISCONNECTED)
                {
                    row.DefaultCellStyle.BackColor = Color.OrangeRed;
                }
                else
                {
                    row.DefaultCellStyle.BackColor = Color.LightGreen;
                }
            }
        }
    }
}
