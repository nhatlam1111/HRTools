namespace HRImportData.Forms
{
    partial class frmProcessing
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            ctrProcessing = new ProgressBar();
            panel1 = new Panel();
            lblMessage = new Label();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // ctrProcessing
            // 
            ctrProcessing.Dock = DockStyle.Top;
            ctrProcessing.Location = new Point(0, 0);
            ctrProcessing.MarqueeAnimationSpeed = 50;
            ctrProcessing.Name = "ctrProcessing";
            ctrProcessing.Size = new Size(396, 40);
            ctrProcessing.Step = 25;
            ctrProcessing.Style = ProgressBarStyle.Marquee;
            ctrProcessing.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.Control;
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(lblMessage);
            panel1.Controls.Add(ctrProcessing);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(1, 1);
            panel1.Name = "panel1";
            panel1.Size = new Size(398, 74);
            panel1.TabIndex = 2;
            // 
            // lblMessage
            // 
            lblMessage.Dock = DockStyle.Fill;
            lblMessage.Font = new Font("Segoe UI", 12F);
            lblMessage.ForeColor = Color.FromArgb(192, 0, 0);
            lblMessage.Location = new Point(0, 40);
            lblMessage.Name = "lblMessage";
            lblMessage.Size = new Size(396, 32);
            lblMessage.TabIndex = 1;
            lblMessage.Text = "label1";
            lblMessage.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // frmProcessing
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(400, 76);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "frmProcessing";
            Padding = new Padding(1);
            StartPosition = FormStartPosition.CenterScreen;
            Text = "frmProcessing";
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private ProgressBar ctrProcessing;
        private Panel panel1;
        private Label lblMessage;
    }
}