using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers.classes
{
    public class ExcelWorkSheet
    {
        public int index { set; get; }
        public string name { set; get; }
        public ISheet sheet { get; set; }
        public DataTable datas { get; set; }
    }
}
