using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceAccessToOracle.classes
{
    public class UserModel
    {
        public string ID { get; set; } = "";
        public string USER_ID { get; set; } = "";
        public string USER_NAME { get; set; } = "";
        public string FULL_NAME { get; set; } = "";
        public string LAST_NAME { get; set; } = "";
        public string CARD_ID { get; set; } = "";
        public string POSITION { get; set; } = "";
        public string PASSWORD { get; set; } = "";
        public string SEX { get; set; } = "";
        public string BIRTH_DATE { get; set; } = "";
        public string BIRTH_PLACE { get; set; } = "";
        public string JOIN_DATE { get; set; } = "";
        public string ADDRESS { get; set; } = "";
        public string PHONE { get; set; } = "";
        public string EMAIL { get; set; } = "";
        public string NATION { get; set; } = "";
        public string CITIZEN_ID { get; set; } = "";
        public string CITIZEN_DATE { get; set; } = "";
        public string CITIZEN_PLACE { get; set; } = "";
        public string REMARK { get; set; } = "";
        public string ACTIVE_YN { get; set; } = "";
    }
}
