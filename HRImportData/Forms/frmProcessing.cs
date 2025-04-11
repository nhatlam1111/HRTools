using HRImportData.Controllers;
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
    public partial class frmProcessing : Form
    {
        public frmProcessing()
        {
            InitializeComponent();
        }

        public void SetMessage(string message)
        {
            ThreadController.SetText(this, lblMessage, message);
        }
    }
}
