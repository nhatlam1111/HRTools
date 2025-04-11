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

        public string AccessFilePath { get; set; }
        public string AccessFilePass { get; set; }

        public int SyncDays { get; set; } = 0;
        public int SyncMinutes { get; set; } = 5;

    }
}
