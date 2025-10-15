namespace MCPApp
{
    partial class SuppliersReportForm
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
            this.suppDGV = new System.Windows.Forms.DataGridView();
            this.allButton = new System.Windows.Forms.Button();
            this.noneButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.SelectBFolderButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.pathTextBox = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.GenerateButton = new System.Windows.Forms.Button();
            this.closeButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.suppDGV)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // suppDGV
            // 
            this.suppDGV.AllowUserToAddRows = false;
            this.suppDGV.AllowUserToDeleteRows = false;
            this.suppDGV.AllowUserToOrderColumns = true;
            this.suppDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.suppDGV.Location = new System.Drawing.Point(12, 58);
            this.suppDGV.Name = "suppDGV";
            this.suppDGV.RowHeadersWidth = 62;
            this.suppDGV.Size = new System.Drawing.Size(773, 580);
            this.suppDGV.TabIndex = 0;
            // 
            // allButton
            // 
            this.allButton.Location = new System.Drawing.Point(12, 29);
            this.allButton.Name = "allButton";
            this.allButton.Size = new System.Drawing.Size(122, 23);
            this.allButton.TabIndex = 1;
            this.allButton.Text = "Select ALL Suppliers";
            this.allButton.UseVisualStyleBackColor = true;
            this.allButton.Click += new System.EventHandler(this.allButton_Click);
            // 
            // noneButton
            // 
            this.noneButton.Location = new System.Drawing.Point(140, 29);
            this.noneButton.Name = "noneButton";
            this.noneButton.Size = new System.Drawing.Size(121, 23);
            this.noneButton.TabIndex = 2;
            this.noneButton.Text = "Select NO Suppliers";
            this.noneButton.UseVisualStyleBackColor = true;
            this.noneButton.Click += new System.EventHandler(this.noneButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.SelectBFolderButton);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.pathTextBox);
            this.groupBox1.Location = new System.Drawing.Point(791, 58);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(537, 137);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Supplier Rpts Locations";
            // 
            // SelectBFolderButton
            // 
            this.SelectBFolderButton.Location = new System.Drawing.Point(247, 77);
            this.SelectBFolderButton.Name = "SelectBFolderButton";
            this.SelectBFolderButton.Size = new System.Drawing.Size(160, 50);
            this.SelectBFolderButton.TabIndex = 2;
            this.SelectBFolderButton.Text = "Select Folder";
            this.SelectBFolderButton.UseVisualStyleBackColor = true;
            this.SelectBFolderButton.Click += new System.EventHandler(this.SelectBFolderButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Rpts Location :";
            // 
            // pathTextBox
            // 
            this.pathTextBox.Location = new System.Drawing.Point(15, 53);
            this.pathTextBox.Name = "pathTextBox";
            this.pathTextBox.Size = new System.Drawing.Size(394, 20);
            this.pathTextBox.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.GenerateButton);
            this.groupBox2.Location = new System.Drawing.Point(805, 215);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(419, 179);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Generate Supplier Reports";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Blue;
            this.label3.Location = new System.Drawing.Point(31, 114);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(377, 65);
            this.label3.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(30, 89);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(209, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "Generating report for supplier : ";
            // 
            // GenerateButton
            // 
            this.GenerateButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GenerateButton.Location = new System.Drawing.Point(5, 19);
            this.GenerateButton.Name = "GenerateButton";
            this.GenerateButton.Size = new System.Drawing.Size(402, 46);
            this.GenerateButton.TabIndex = 0;
            this.GenerateButton.Text = "All LIVE Jobs By  Selected Supplier(s)";
            this.GenerateButton.UseVisualStyleBackColor = true;
            this.GenerateButton.Click += new System.EventHandler(this.GenerateButton_Click);
            // 
            // closeButton
            // 
            this.closeButton.BackColor = System.Drawing.Color.Red;
            this.closeButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.closeButton.ForeColor = System.Drawing.Color.White;
            this.closeButton.Location = new System.Drawing.Point(1031, 556);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(181, 57);
            this.closeButton.TabIndex = 5;
            this.closeButton.Text = "CLOSE";
            this.closeButton.UseVisualStyleBackColor = false;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // SuppliersReportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1283, 650);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.noneButton);
            this.Controls.Add(this.allButton);
            this.Controls.Add(this.suppDGV);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "SuppliersReportForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SuppliersReportForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.SuppliersReportForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.suppDGV)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView suppDGV;
        private System.Windows.Forms.Button allButton;
        private System.Windows.Forms.Button noneButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button SelectBFolderButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox pathTextBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button GenerateButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Label label3;
    }
}