using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceAccessToOracle.classes
{
    public class AttendanceModel
    {
        public string USER_ID { get; set; } = "";
        public string USER_NAME { get; set; } = "";
        public string CARD_ID { get; set; } = "";
        public string WORK_DATE_FULL { get; set; } = "";
        public string WORK_DATE { get; set; } = "";
        public string WORK_TIME { get; set; } = "";
        public string EVT { get; set; } = ""; //EVENT --DO LA KEY NEN KO THE DE EVENT DC
        public string MACHINE_ID { get; set; } = "";
        public string MACHINE_NAME { get; set; } = "";
        public string MACHINE_IP { get; set; } = "";
        public string MACHINE_TYPE { get; set; } = "";
        public string REMARK { get; set; } = "";

    }
}
