namespace MCPApp
{
    partial class SuppliersOverviewForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SuppliersOverviewForm));
            this.suppDGV = new System.Windows.Forms.DataGridView();
            this.AddSupplierBtn = new System.Windows.Forms.Button();
            this.CloseButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.suppDGV)).BeginInit();
            this.SuspendLayout();
            // 
            // suppDGV
            // 
            this.suppDGV.AllowUserToAddRows = false;
            this.suppDGV.AllowUserToDeleteRows = false;
            this.suppDGV.AllowUserToOrderColumns = true;
            this.suppDGV.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.suppDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.suppDGV.Location = new System.Drawing.Point(13, 14);
            this.suppDGV.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.suppDGV.Name = "suppDGV";
            this.suppDGV.RowHeadersWidth = 62;
            this.suppDGV.Size = new System.Drawing.Size(1698, 738);
            this.suppDGV.TabIndex = 1;
            this.suppDGV.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.suppDGV_CellDoubleClick);
            this.suppDGV.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.suppDGV_CellFormatting);
            // 
            // AddSupplierBtn
            // 
            this.AddSupplierBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.AddSupplierBtn.Location = new System.Drawing.Point(13, 776);
            this.AddSupplierBtn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.AddSupplierBtn.Name = "AddSupplierBtn";
            this.AddSupplierBtn.Size = new System.Drawing.Size(281, 100);
            this.AddSupplierBtn.TabIndex = 3;
            this.AddSupplierBtn.Text = "ADD SUPPLIER";
            this.AddSupplierBtn.UseVisualStyleBackColor = true;
            this.AddSupplierBtn.Click += new System.EventHandler(this.AddSupplierBtn_Click);
            // 
            // CloseButton
            // 
            this.CloseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CloseButton.BackColor = System.Drawing.Color.Red;
            this.CloseButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CloseButton.ForeColor = System.Drawing.Color.White;
            this.CloseButton.Location = new System.Drawing.Point(1462, 776);
            this.CloseButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(259, 100);
            this.CloseButton.TabIndex = 4;
            this.CloseButton.Text = "CLOSE";
            this.CloseButton.UseVisualStyleBackColor = false;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // SuppliersOverviewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1734, 890);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.AddSupplierBtn);
            this.Controls.Add(this.suppDGV);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "SuppliersOverviewForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SuppliersOverviewForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.SuppliersOverviewForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.suppDGV)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView suppDGV;
        private System.Windows.Forms.Button AddSupplierBtn;
        private System.Windows.Forms.Button CloseButton;
    }
}