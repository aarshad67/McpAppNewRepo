namespace MCPApp
{
    partial class CustomersOverviewForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomersOverviewForm));
            this.custDGV = new System.Windows.Forms.DataGridView();
            this.CloseButton = new System.Windows.Forms.Button();
            this.AddCustomerBtn = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemOn = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemOff = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.custDGV)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // custDGV
            // 
            this.custDGV.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.custDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.custDGV.Location = new System.Drawing.Point(13, 14);
            this.custDGV.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.custDGV.Name = "custDGV";
            this.custDGV.RowHeadersWidth = 62;
            this.custDGV.Size = new System.Drawing.Size(2245, 877);
            this.custDGV.TabIndex = 0;
            this.custDGV.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.custDGV_CellDoubleClick);
            this.custDGV.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.custDGV_CellFormatting);
            this.custDGV.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.custDGV_CellMouseUp);
            // 
            // CloseButton
            // 
            this.CloseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CloseButton.BackColor = System.Drawing.Color.Red;
            this.CloseButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CloseButton.ForeColor = System.Drawing.Color.White;
            this.CloseButton.Location = new System.Drawing.Point(1986, 907);
            this.CloseButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(272, 95);
            this.CloseButton.TabIndex = 1;
            this.CloseButton.Text = "CLOSE";
            this.CloseButton.UseVisualStyleBackColor = false;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // AddCustomerBtn
            // 
            this.AddCustomerBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.AddCustomerBtn.Location = new System.Drawing.Point(13, 907);
            this.AddCustomerBtn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.AddCustomerBtn.Name = "AddCustomerBtn";
            this.AddCustomerBtn.Size = new System.Drawing.Size(335, 82);
            this.AddCustomerBtn.TabIndex = 2;
            this.AddCustomerBtn.Text = "ADD CUSTOMER";
            this.AddCustomerBtn.UseVisualStyleBackColor = true;
            this.AddCustomerBtn.Click += new System.EventHandler(this.AddCustomerBtn_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemOn,
            this.toolStripMenuItemOff});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(291, 68);
            // 
            // toolStripMenuItemOn
            // 
            this.toolStripMenuItemOn.Name = "toolStripMenuItemOn";
            this.toolStripMenuItemOn.Size = new System.Drawing.Size(290, 32);
            this.toolStripMenuItemOn.Text = "Put Customer ON STOP";
            this.toolStripMenuItemOn.Click += new System.EventHandler(this.toolStripMenuItemOn_Click);
            // 
            // toolStripMenuItemOff
            // 
            this.toolStripMenuItemOff.Name = "toolStripMenuItemOff";
            this.toolStripMenuItemOff.Size = new System.Drawing.Size(290, 32);
            this.toolStripMenuItemOff.Text = "Take Custromer OFF STOP";
            this.toolStripMenuItemOff.Click += new System.EventHandler(this.toolStripMenuItemOff_Click);
            // 
            // CustomersOverviewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2271, 1015);
            this.Controls.Add(this.AddCustomerBtn);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.custDGV);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "CustomersOverviewForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CustomersOverviewForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.CustomersOverviewForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.custDGV)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView custDGV;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.Button AddCustomerBtn;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemOn;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemOff;
    }
}