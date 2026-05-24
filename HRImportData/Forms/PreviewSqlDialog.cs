using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HRImportData.Forms
{
    public partial class PreviewSqlDialog : Form
    {
        public PreviewSqlDialog()
        {
            InitializeComponent();
        }

        public PreviewSqlDialog(List<string> sqls)
        {
            InitializeComponent();

            if (sqls != null && sqls.Count > 0)
            {
                foreach (var sql in sqls)
                {
                    txtSql.AppendText(sql + Environment.NewLine );
                }
            }
            else
            {
                txtSql.AppendText("No SQL statements to preview.");
            }
        }

        private void PreviewSql_Load(object sender, EventArgs e)
        {

        }
    }
}
