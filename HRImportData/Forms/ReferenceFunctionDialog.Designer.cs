namespace HRImportData.Forms
{
    partial class ReferenceFunctionDialog
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
            btnDelete = new Button();
            btnAdd = new Button();
            gridReferenceFuntion = new DataGridView();
            btnSave = new Button();
            ((System.ComponentModel.ISupportInitialize)gridReferenceFuntion).BeginInit();
            SuspendLayout();
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(713, 12);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(75, 23);
            btnDelete.TabIndex = 3;
            btnDelete.Text = "Delete";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            // 
            // btnAdd
            // 
            btnAdd.Location = new Point(551, 12);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(75, 23);
            btnAdd.TabIndex = 1;
            btnAdd.Text = "Add";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += btnAdd_Click;
            // 
            // gridReferenceFuntion
            // 
            gridReferenceFuntion.AllowUserToAddRows = false;
            gridReferenceFuntion.AllowUserToResizeRows = false;
            gridReferenceFuntion.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            gridReferenceFuntion.Location = new Point(12, 41);
            gridReferenceFuntion.Name = "gridReferenceFuntion";
            gridReferenceFuntion.RowHeadersVisible = false;
            gridReferenceFuntion.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            gridReferenceFuntion.ShowCellErrors = false;
            gridReferenceFuntion.ShowRowErrors = false;
            gridReferenceFuntion.Size = new Size(776, 397);
            gridReferenceFuntion.TabIndex = 3;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(632, 12);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(75, 23);
            btnSave.TabIndex = 2;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // ReferenceFunctionDialog
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnSave);
            Controls.Add(gridReferenceFuntion);
            Controls.Add(btnAdd);
            Controls.Add(btnDelete);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "ReferenceFunctionDialog";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Reference Functions";
            Load += ReferenceFunctionDialog_Load;
            ((System.ComponentModel.ISupportInitialize)gridReferenceFuntion).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Button btnDelete;
        private Button btnAdd;
        private DataGridView gridReferenceFuntion;
        private Button btnSave;
    }
}