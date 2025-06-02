using System;
using System.Windows.Forms;

namespace Helpers.controllers
{
    public static class ThreadController
    {
        private delegate void SetTextBox(Form form, Control ctrl, string text);
        private delegate void SetComboboxDataSource(Form form, Control ctrl, BindingSource source, string displayMember, string displayValue);
        private delegate void SetGridViewItem(Form f, DataGridView ctrl, string[] values, string tag);
        private delegate void RefreshGridViewItem(Form form, DataGridView ctrl);
        private delegate void ClearGridViewItem(Form f, DataGridView ctrl, string tag);


        public static void SetText(Form form, Control ctrl, string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (ctrl.InvokeRequired)
            {
                SetTextBox d = new SetTextBox(SetText);
                form.Invoke(d, new object[] { form, ctrl, text });
            }
            else
            {
                ctrl.Text = text;
            }
        }

        public static void SetComboBoxDataSource(Form form, Control ctrl, BindingSource source, string displayMember, string displayValue)
        {
            if (ctrl.InvokeRequired)
            {
                SetComboboxDataSource d = new SetComboboxDataSource(SetComboBoxDataSource);
                form.Invoke(d, new object[] { form, ctrl, source, displayMember, displayValue });
            }
            else
            {
                ((ComboBox)ctrl).DataSource = source;
                ((ComboBox)ctrl).DisplayMember = "name";
                ((ComboBox)ctrl).ValueMember = "index";
            }
        }

        public static void SetGridView(Form form, DataGridView ctrl, string[] values, string tag)
        {
            if (ctrl.InvokeRequired)
            {
                SetGridViewItem d = new SetGridViewItem(SetGridView);
                form.Invoke(d, new object[] { form, ctrl, values, tag });
            }
            else
            {
                bool isExists = false;

                foreach (DataGridViewRow dr in ctrl.Rows)
                {
                    if (dr.Tag + "" == tag)
                    {
                        for (int i = 0; i < dr.Cells.Count; i++)
                        {
                            try { dr.Cells[i].Value = values[i]; } catch { }
                        }

                        isExists = true;
                        break;
                    }
                }
                if (!isExists)
                {
                    ctrl.Rows.Insert(0, values);
                    ctrl.Rows[0].Tag = tag;

                    try
                    {
                        if (ctrl.Rows.Count > 5000)
                        {
                            ctrl.Rows.RemoveAt(ctrl.Rows.Count - 1);
                        }
                    }
                    catch (Exception e)
                    {
                    }
                }
            }
        }

        public static void RefreshGridView(Form form, DataGridView ctrl)
        {
            if (ctrl.InvokeRequired)
            {
                RefreshGridViewItem d = new RefreshGridViewItem(RefreshGridView);
                form.Invoke(d, new object[] { form, ctrl });
            }
            else
            {
                ctrl.Refresh();
            }
        }

        public static void ClearGridView(Form form, DataGridView ctrl, string tag)
        {
            if (ctrl.InvokeRequired)
            {
                ClearGridViewItem d = new ClearGridViewItem(ClearGridView);
                form.Invoke(d, new object[] { form, ctrl, tag });
            }
            else
            {
                if (string.IsNullOrEmpty(tag))
                {
                    ctrl.Rows.Clear();
                }
                else
                {
                    for (int i = 0; i < ctrl.Rows.Count; i++)
                    {
                        DataGridViewRow dr = ctrl.Rows[i];
                        if (dr.Tag + "" == tag)
                        {
                            ctrl.Rows.Remove(dr);
                            break;
                        }
                    }

                }
            }
        }
    }
}
