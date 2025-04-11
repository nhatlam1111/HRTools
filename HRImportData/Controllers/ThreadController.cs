namespace HRImportData.Controllers
{
    public static class ThreadController
    {
        private delegate void SetTextBox(Form form, Control ctrl, string text);
        private delegate void SetComboboxDataSource(Form form, Control ctrl, BindingSource source, string displayMember, string displayValue);


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
    }
}
