using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceAccessToOracle.classes
{
    public class Config
    {
        public string Client { get; set; }
        public string DbUser { get; set; }
        public string DbPass { get; set; }

        //public string AccessFilePath { get; set; }
        // public string AccessFilePass { get; set; }

        public string SqlHost { get; set; } = "localhost";
        public string SqlPort { get; set; }
        public string SqlService { get; set; } = "orcl";
        public string SqlUserName { get; set; }
        public string SqlPassword { get; set; }

        public string SqlPath { get; set; } = $"{AppDomain.CurrentDomain.BaseDirectory}\\sql.dat";

        public int SyncDays { get; set; } = 0;
        public int SyncMinutes { get; set; } = 5;
        public bool SyncAttendance { get; set; } = true;
        public bool SyncUser { get; set; } = false;
    }
}
