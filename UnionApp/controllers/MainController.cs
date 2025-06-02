using Helpers;
using Helpers.controllers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using UnionApp.classes;
using static System.Windows.Forms.AxHost;

namespace UnionApp.controllers
{
    public static class MainController
    {
        public static Config config;
        


        public static async Task Start()
        {
            OracleDb.connectionString = $"Data Source={Helper.clientList[config.Client].Replace("\r\n", "").Replace(" ", "")};User Id={config.DbUser};Password={config.DbPass};";
            await OracleDb.Connect("", true);
        }

        public static async Task Stop()
        {
            await OracleDb.Close();
            LogController.Information("Server stopped.", true);
        }
    }
}
