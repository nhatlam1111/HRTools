using System.Diagnostics;

namespace AttendanceAccessToOracle
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (IsProcessOpen("AttendanceSQLServerToOracle"))
            {
                //MessageBox.Show("This application is currently running", "GW Union", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Exit();
                return;
            }

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Main());
        }

        static bool IsProcessOpen(string name)
        {
            //int count = 0;
            Process currentProcess = Process.GetCurrentProcess();
            foreach (Process clsProcess in Process.GetProcessesByName(name))
            {
                if (currentProcess.Id != clsProcess.Id)
                {

                    return true;
                }
            }

            return false;
        }
    }
}