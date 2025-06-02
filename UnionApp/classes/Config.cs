using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnionApp.classes
{
    public class Config
    {
        public string Client { get; set; }
        public string DbUser { get; set; }
        public string DbPass { get; set; }
        public int listenerPort { get; set; } = 9870;


        public string SqlPath { get; set; } = $"{AppDomain.CurrentDomain.BaseDirectory}\\sql.dat";

    }
}
