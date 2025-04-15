using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceAccessToOracle.classes
{
    public class SqlTemplates
    {
        

        //Attendance sql
        public string ACCESS_SELECT_ATTENDANCE { get; set; } = "";
        public string ORACLE_SELECT_ATTENDANCE { get; set; } = "";
        public string ORACLE_INSERT_ATTENDANCE { get; set; } = "";


        //User sql
        public string ACCESS_SELECT_USER { get; set; } = "";
        public string ACCESS_INSERT_USER { get; set; } = "";
        
        public string ORACLE_SELECT_USER { get; set; } = "";
        public string ORACLE_INSERT_USER { get; set; } = "";
        public string ORACLE_UPDATE_USER { get; set; } = "";
    }
}
