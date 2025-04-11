using HRImportData.Classes;
using HRImportData.Forms;

namespace HRImportData
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            SetupLogging.Start(null);
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            //Application.Run(new frmImportSalary());
            //Application.Run(new frmImportTimeTemp());
            Application.Run(new Main2());
        }
    }
}