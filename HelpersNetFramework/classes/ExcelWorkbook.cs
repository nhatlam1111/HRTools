using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers.classes
{
    public class ExcelWorkbook
    {
        public XSSFWorkbook workBook { get; set; }
        public string workBookPath { get; set; }  //path of workBook
        public List<ExcelWorkSheet> workSheets { get; set; }
    }
}
