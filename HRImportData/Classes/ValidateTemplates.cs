using System.Data;

namespace HRImportData.Classes
{
    public static class ValidateTemplates
    {
        public static string ValidateImportTimeTemp(DataRow rowImport, DataRow validateColumn)
        {
            string errorMsg = "";
            var value = rowImport[validateColumn["Excel"] + ""];

            if ((validateColumn["Database"] + "").ToLower() == "work_dt")
            {
                DateTime d = new DateTime();
                try
                {
                    d = DateTime.ParseExact(value + "", "yyyyMMdd", null);
                }
                catch
                {
                    errorMsg = $"Column [{validateColumn["Database"] + ""}] format must be 'YYYYMMDD'";
                }
            }

            if ((validateColumn["Database"] + "").ToLower() == "time")
            {
                DateTime d = new DateTime();
                try
                {
                    d = DateTime.ParseExact(value + "", "HH:mm", null);
                }
                catch
                {
                    errorMsg = $"Column [{validateColumn["Database"] + ""}] format must be 'hh:mm'";
                }
            }

            return errorMsg;
        }
    }
}
