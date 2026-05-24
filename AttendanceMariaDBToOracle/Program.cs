using AttendanceMariaDBToOracle;
using AttendanceMariaDBToOracle.classes;
using Helpers;
using Helpers.classes;
using Helpers.controllers;
using System.Windows.Forms;


LogController.Start("");

DateTime fromDate = DateTime.Now.AddDays(-15);
DateTime toDate = DateTime.Now;

MariaDbProcessing mariaDbProcessing = new MariaDbProcessing();
OracleDbProcessing oracleDbProcessing = new OracleDbProcessing();


var mariaLogs = new List<TerminalLog>();

try 
{
    mariaLogs = mariaDbProcessing.GetTerminalLogs(fromDate, toDate);
}
catch (Exception ex)
{
    Console.WriteLine($"Error retrieving logs from MariaDB: {ex.Message}");
    LogController.Information($"Error retrieving logs from MariaDB: {ex.Message}");
    return;
}


if (mariaLogs.Count > 0)
{
    bool completed = oracleDbProcessing.InsertTerminalLogs(mariaLogs);

    if (completed)
    {
        Console.WriteLine("Successfully transferred logs from MariaDB to Oracle.");
        LogController.Information("Successfully transferred logs from MariaDB to Oracle.");
    }
    else
    {
        Console.WriteLine("Failed to transfer logs from MariaDB to Oracle.");
        LogController.Information("Failed to transfer logs from MariaDB to Oracle.");
    }
}
else
{
    Console.WriteLine("No logs found in MariaDB for the specified date range.");
     LogController.Information("No logs found in MariaDB for the specified date range.");
}

Application.Exit();