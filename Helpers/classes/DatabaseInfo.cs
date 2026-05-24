using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers.classes
{
    public class DatabaseInfo
    {
        public string Server { get; set; }
        public string Database { get; set; }
        public int Port { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
        public bool UseIntegratedSecurity { get; set; }

        public DatabaseInfo()
        {
            Server = string.Empty;
            Database = string.Empty;
            UserId = string.Empty;
            Password = string.Empty;
            UseIntegratedSecurity = false;
        }
    }
}
