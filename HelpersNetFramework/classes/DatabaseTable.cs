using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers.classes
{
    public class DatabaseTable
    {
        public string table_name { get; set; }
        public List<DatabaseColumn> columns { get; set; }

        public DatabaseTable()
        {
            columns = new List<DatabaseColumn>();
        }
    }
}
