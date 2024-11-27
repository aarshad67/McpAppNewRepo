
namespace MCPApp
{
    partial class DesignBoardForm
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
            this.cancelButton = new System.Windows.Forms.Button();
            this.weeksTabControl = new System.Windows.Forms.TabControl();
            this.supplierContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.selectSupplierToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.productContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.selectProductToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wbDailyContextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addCommenttoolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.jobContextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.GoToJobPlannerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusContextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.selectDesignStatusToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dateChangeContextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.canToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AdditionalCommentContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.AddAdditionalCommentMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToEXCELToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.supplierContextMenuStrip.SuspendLayout();
            this.productContextMenuStrip.SuspendLayout();
            this.wbDailyContextMenuStrip1.SuspendLayout();
            this.jobContextMenuStrip1.SuspendLayout();
            this.statusContextMenuStrip1.SuspendLayout();
            this.dateChangeContextMenuStrip1.SuspendLayout();
            this.AdditionalCommentContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.BackColor = System.Drawing.Color.Red;
            this.cancelButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancelButton.ForeColor = System.Drawing.Color.White;
            this.cancelButton.Location = new System.Drawing.Point(1503, 923);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(153, 69);
            this.cancelButton.TabIndex = 17;
            this.cancelButton.Text = "CLOSE";
            this.cancelButton.UseVisualStyleBackColor = false;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // weeksTabControl
            // 
            this.weeksTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.weeksTabControl.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.weeksTabControl.Location = new System.Drawing.Point(12, 22);
            this.weeksTabControl.Name = "weeksTabControl";
            this.weeksTabControl.SelectedIndex = 0;
            this.weeksTabControl.Size = new System.Drawing.Size(1657, 879);
            this.weeksTabControl.TabIndex = 19;
            // 
            // supplierContextMenuStrip
            // 
            this.supplierContextMenuStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.supplierContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectSupplierToolStripMenuItem});
            this.supplierContextMenuStrip.Name = "wbContextMenuStrip";
            this.supplierContextMenuStrip.Size = new System.Drawing.Size(201, 36);
            this.supplierContextMenuStrip.Text = "JOB Option(s)";
            // 
            // selectSupplierToolStripMenuItem
            // 
            this.selectSupplierToolStripMenuItem.Name = "selectSupplierToolStripMenuItem";
            this.selectSupplierToolStripMenuItem.Size = new System.Drawing.Size(200, 32);
            this.selectSupplierToolStripMenuItem.Text = "Select Supplier";
            this.selectSupplierToolStripMenuItem.Click += new System.EventHandler(this.selectSupplierToolStripMenuItem_Click);
            // 
            // productContextMenuStrip
            // 
            this.productContextMenuStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.productContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectProductToolStripMenuItem});
            this.productContextMenuStrip.Name = "wbContextMenuStrip";
            this.productContextMenuStrip.Size = new System.Drawing.Size(198, 36);
            this.productContextMenuStrip.Text = "Select Product";
            // 
            // selectProductToolStripMenuItem
            // 
            this.selectProductToolStripMenuItem.Name = "selectProductToolStripMenuItem";
            this.selectProductToolStripMenuItem.Size = new System.Drawing.Size(197, 32);
            this.selectProductToolStripMenuItem.Text = "Select Product";
            this.selectProductToolStripMenuItem.Click += new System.EventHandler(this.selectProductToolStripMenuItem_Click);
            // 
            // wbDailyContextMenuStrip1
            // 
            this.wbDailyContextMenuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.wbDailyContextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addCommenttoolStripMenuItem});
            this.wbDailyContextMenuStrip1.Name = "wbDailyContextMenuStrip1";
            this.wbDailyContextMenuStrip1.Size = new System.Drawing.Size(203, 36);
            // 
            // addCommenttoolStripMenuItem
            // 
            this.addCommenttoolStripMenuItem.Name = "addCommenttoolStripMenuItem";
            this.addCommenttoolStripMenuItem.Size = new System.Drawing.Size(202, 32);
            this.addCommenttoolStripMenuItem.Text = "Add Comment";
            this.addCommenttoolStripMenuItem.Click += new System.EventHandler(this.addCommenttoolStripMenuItem_Click);
            // 
            // jobContextMenuStrip1
            // 
            this.jobContextMenuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.jobContextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.GoToJobPlannerToolStripMenuItem,
            this.exportToEXCELToolStripMenuItem});
            this.jobContextMenuStrip1.Name = "jobContextMenuStrip1";
            this.jobContextMenuStrip1.Size = new System.Drawing.Size(300, 101);
            // 
            // GoToJobPlannerToolStripMenuItem
            // 
            this.GoToJobPlannerToolStripMenuItem.Name = "GoToJobPlannerToolStripMenuItem";
            this.GoToJobPlannerToolStripMenuItem.Size = new System.Drawing.Size(299, 32);
            this.GoToJobPlannerToolStripMenuItem.Text = "Go to Job In JOB PLANNER";
            this.GoToJobPlannerToolStripMenuItem.Click += new System.EventHandler(this.GoToJobPlannerToolStripMenuItem_Click);
            // 
            // statusContextMenuStrip1
            // 
            this.statusContextMenuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusContextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectDesignStatusToolStripMenuItem});
            this.statusContextMenuStrip1.Name = "statusContextMenuStrip1";
            this.statusContextMenuStrip1.Size = new System.Drawing.Size(244, 36);
            // 
            // selectDesignStatusToolStripMenuItem
            // 
            this.selectDesignStatusToolStripMenuItem.Name = "selectDesignStatusToolStripMenuItem";
            this.selectDesignStatusToolStripMenuItem.Size = new System.Drawing.Size(243, 32);
            this.selectDesignStatusToolStripMenuItem.Text = "Select Design Status";
            this.selectDesignStatusToolStripMenuItem.Click += new System.EventHandler(this.selectDesignStatusToolStripMenuItem_Click);
            // 
            // dateChangeContextMenuStrip1
            // 
            this.dateChangeContextMenuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.dateChangeContextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.canToolStripMenuItem});
            this.dateChangeContextMenuStrip1.Name = "dateChangeContextMenuStrip1";
            this.dateChangeContextMenuStrip1.Size = new System.Drawing.Size(187, 36);
            // 
            // canToolStripMenuItem
            // 
            this.canToolStripMenuItem.Name = "canToolStripMenuItem";
            this.canToolStripMenuItem.Size = new System.Drawing.Size(186, 32);
            this.canToolStripMenuItem.Text = "Change Date";
            this.canToolStripMenuItem.Click += new System.EventHandler(this.canToolStripMenuItem_Click);
            // 
            // AdditionalCommentContextMenuStrip
            // 
            this.AdditionalCommentContextMenuStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.AdditionalCommentContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddAdditionalCommentMenuItem});
            this.AdditionalCommentContextMenuStrip.Name = "AdditionalCommentContextMenuStrip";
            this.AdditionalCommentContextMenuStrip.Size = new System.Drawing.Size(290, 36);
            this.AdditionalCommentContextMenuStrip.Text = "Add Additional Comment";
            this.AdditionalCommentContextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.AdditionalCommentContextMenuStrip_Opening);
            // 
            // AddAdditionalCommentMenuItem
            // 
            this.AddAdditionalCommentMenuItem.Name = "AddAdditionalCommentMenuItem";
            this.AddAdditionalCommentMenuItem.Size = new System.Drawing.Size(289, 32);
            this.AddAdditionalCommentMenuItem.Text = "Add Additional Comment";
            this.AddAdditionalCommentMenuItem.Click += new System.EventHandler(this.AddAdditionalCommentMenuItem_Click);
            // 
            // exportToEXCELToolStripMenuItem
            // 
            this.exportToEXCELToolStripMenuItem.Name = "exportToEXCELToolStripMenuItem";
            this.exportToEXCELToolStripMenuItem.Size = new System.Drawing.Size(299, 32);
            this.exportToEXCELToolStripMenuItem.Text = "Export To EXCEL";
            this.exportToEXCELToolStripMenuItem.Click += new System.EventHandler(this.exportToEXCELToolStripMenuItem_Click);
            // 
            // DesignBoardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1689, 1015);
            this.Controls.Add(this.weeksTabControl);
            this.Controls.Add(this.cancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "DesignBoardForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DesignBoardForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.DesignBoardForm_Load);
            this.supplierContextMenuStrip.ResumeLayout(false);
            this.productContextMenuStrip.ResumeLayout(false);
            this.wbDailyContextMenuStrip1.ResumeLayout(false);
            this.jobContextMenuStrip1.ResumeLayout(false);
            this.statusContextMenuStrip1.ResumeLayout(false);
            this.dateChangeContextMenuStrip1.ResumeLayout(false);
            this.AdditionalCommentContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.TabControl weeksTabControl;
        private System.Windows.Forms.ContextMenuStrip supplierContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem selectSupplierToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip productContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem selectProductToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip wbDailyContextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem addCommenttoolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip jobContextMenuStrip1;
        private System.Windows.Forms.ContextMenuStrip statusContextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem selectDesignStatusToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip dateChangeContextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem canToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip AdditionalCommentContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem AddAdditionalCommentMenuItem;
        private System.Windows.Forms.ToolStripMenuItem GoToJobPlannerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToEXCELToolStripMenuItem;
    }
}