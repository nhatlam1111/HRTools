using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnionApp.classes
{
    public class Device
    {
        public string Code { get; set; }
        public decimal Id { get; set; }
        public string Name { get; set; }
        public DEVICE_STATUS Status { get; set; } = DEVICE_STATUS.DISCONNECTED;
        public string IP { get; set; }
        public string Version { get; set; }

        public decimal CompanyPk { get; set; }
        public string CompanyBranch { get; set; }
        public string AccessGroup { get; set; }
    }
}
