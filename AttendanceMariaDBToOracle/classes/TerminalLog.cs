using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceMariaDBToOracle.classes
{
    internal class TerminalLog
    {
        public int TerminalId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string CardId { get; set; }
        public DateTime EventTime { get; set; }
        public int AuthType { get; set; }
        public int AuthResult { get; set; }

        public TerminalLog()
        {
            TerminalId = -1;
            UserId = -1;
            UserName = string.Empty;
            CardId = string.Empty;
            EventTime = DateTime.MinValue;
            AuthType = 0;
            AuthResult = 0;
        }

    }
}
