
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
            this.exportToEXCELToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusContextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.selectDesignStatusToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dateChangeContextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.canToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AdditionalCommentContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.AddAdditionalCommentMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.RefreshBtn = new System.Windows.Forms.Button();
            this.designStatusAuditForJobToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.cancelButton.Location = new System.Drawing.Point(1002, 600);
            this.cancelButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(102, 45);
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
            this.weeksTabControl.Location = new System.Drawing.Point(19, 8);
            this.weeksTabControl.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.weeksTabControl.Name = "weeksTabControl";
            this.weeksTabControl.SelectedIndex = 0;
            this.weeksTabControl.Size = new System.Drawing.Size(1092, 583);
            this.weeksTabControl.TabIndex = 19;
            // 
            // supplierContextMenuStrip
            // 
            this.supplierContextMenuStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.supplierContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectSupplierToolStripMenuItem});
            this.supplierContextMenuStrip.Name = "wbContextMenuStrip";
            this.supplierContextMenuStrip.Size = new System.Drawing.Size(152, 26);
            this.supplierContextMenuStrip.Text = "JOB Option(s)";
            // 
            // selectSupplierToolStripMenuItem
            // 
            this.selectSupplierToolStripMenuItem.Name = "selectSupplierToolStripMenuItem";
            this.selectSupplierToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.selectSupplierToolStripMenuItem.Text = "Select Supplier";
            this.selectSupplierToolStripMenuItem.Click += new System.EventHandler(this.selectSupplierToolStripMenuItem_Click);
            // 
            // productContextMenuStrip
            // 
            this.productContextMenuStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.productContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectProductToolStripMenuItem});
            this.productContextMenuStrip.Name = "wbContextMenuStrip";
            this.productContextMenuStrip.Size = new System.Drawing.Size(151, 26);
            this.productContextMenuStrip.Text = "Select Product";
            // 
            // selectProductToolStripMenuItem
            // 
            this.selectProductToolStripMenuItem.Name = "selectProductToolStripMenuItem";
            this.selectProductToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.selectProductToolStripMenuItem.Text = "Select Product";
            this.selectProductToolStripMenuItem.Click += new System.EventHandler(this.selectProductToolStripMenuItem_Click);
            // 
            // wbDailyContextMenuStrip1
            // 
            this.wbDailyContextMenuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.wbDailyContextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addCommenttoolStripMenuItem});
            this.wbDailyContextMenuStrip1.Name = "wbDailyContextMenuStrip1";
            this.wbDailyContextMenuStrip1.Size = new System.Drawing.Size(154, 26);
            // 
            // addCommenttoolStripMenuItem
            // 
            this.addCommenttoolStripMenuItem.Name = "addCommenttoolStripMenuItem";
            this.addCommenttoolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.addCommenttoolStripMenuItem.Text = "Add Comment";
            this.addCommenttoolStripMenuItem.Click += new System.EventHandler(this.addCommenttoolStripMenuItem_Click);
            // 
            // jobContextMenuStrip1
            // 
            this.jobContextMenuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.jobContextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.GoToJobPlannerToolStripMenuItem,
            this.designStatusAuditForJobToolStripMenuItem,
            this.exportToEXCELToolStripMenuItem});
            this.jobContextMenuStrip1.Name = "jobContextMenuStrip1";
            this.jobContextMenuStrip1.Size = new System.Drawing.Size(217, 92);
            // 
            // GoToJobPlannerToolStripMenuItem
            // 
            this.GoToJobPlannerToolStripMenuItem.Name = "GoToJobPlannerToolStripMenuItem";
            this.GoToJobPlannerToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
            this.GoToJobPlannerToolStripMenuItem.Text = "Go to Job In JOB PLANNER";
            this.GoToJobPlannerToolStripMenuItem.Click += new System.EventHandler(this.GoToJobPlannerToolStripMenuItem_Click);
            // 
            // exportToEXCELToolStripMenuItem
            // 
            this.exportToEXCELToolStripMenuItem.Name = "exportToEXCELToolStripMenuItem";
            this.exportToEXCELToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
            this.exportToEXCELToolStripMenuItem.Text = "Export To EXCEL";
            this.exportToEXCELToolStripMenuItem.Click += new System.EventHandler(this.exportToEXCELToolStripMenuItem_Click);
            // 
            // statusContextMenuStrip1
            // 
            this.statusContextMenuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusContextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectDesignStatusToolStripMenuItem});
            this.statusContextMenuStrip1.Name = "statusContextMenuStrip1";
            this.statusContextMenuStrip1.Size = new System.Drawing.Size(180, 26);
            // 
            // selectDesignStatusToolStripMenuItem
            // 
            this.selectDesignStatusToolStripMenuItem.Name = "selectDesignStatusToolStripMenuItem";
            this.selectDesignStatusToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.selectDesignStatusToolStripMenuItem.Text = "Select Design Status";
            this.selectDesignStatusToolStripMenuItem.Click += new System.EventHandler(this.selectDesignStatusToolStripMenuItem_Click);
            // 
            // dateChangeContextMenuStrip1
            // 
            this.dateChangeContextMenuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.dateChangeContextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.canToolStripMenuItem});
            this.dateChangeContextMenuStrip1.Name = "dateChangeContextMenuStrip1";
            this.dateChangeContextMenuStrip1.Size = new System.Drawing.Size(143, 26);
            // 
            // canToolStripMenuItem
            // 
            this.canToolStripMenuItem.Name = "canToolStripMenuItem";
            this.canToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.canToolStripMenuItem.Text = "Change Date";
            this.canToolStripMenuItem.Click += new System.EventHandler(this.canToolStripMenuItem_Click);
            // 
            // AdditionalCommentContextMenuStrip
            // 
            this.AdditionalCommentContextMenuStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.AdditionalCommentContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddAdditionalCommentMenuItem});
            this.AdditionalCommentContextMenuStrip.Name = "AdditionalCommentContextMenuStrip";
            this.AdditionalCommentContextMenuStrip.Size = new System.Drawing.Size(212, 26);
            this.AdditionalCommentContextMenuStrip.Text = "Add Additional Comment";
            this.AdditionalCommentContextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.AdditionalCommentContextMenuStrip_Opening);
            // 
            // AddAdditionalCommentMenuItem
            // 
            this.AddAdditionalCommentMenuItem.Name = "AddAdditionalCommentMenuItem";
            this.AddAdditionalCommentMenuItem.Size = new System.Drawing.Size(211, 22);
            this.AddAdditionalCommentMenuItem.Text = "Add Additional Comment";
            this.AddAdditionalCommentMenuItem.Click += new System.EventHandler(this.AddAdditionalCommentMenuItem_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(15, 632);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 20);
            this.label1.TabIndex = 20;
            this.label1.Text = "label1";
            // 
            // RefreshBtn
            // 
            this.RefreshBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.RefreshBtn.BackColor = System.Drawing.Color.Green;
            this.RefreshBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RefreshBtn.ForeColor = System.Drawing.Color.White;
            this.RefreshBtn.Location = new System.Drawing.Point(818, 600);
            this.RefreshBtn.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.RefreshBtn.Name = "RefreshBtn";
            this.RefreshBtn.Size = new System.Drawing.Size(155, 45);
            this.RefreshBtn.TabIndex = 21;
            this.RefreshBtn.Text = "Refresh the Design Board ";
            this.RefreshBtn.UseVisualStyleBackColor = false;
            this.RefreshBtn.Click += new System.EventHandler(this.RefreshBtn_Click);
            // 
            // designStatusAuditForJobToolStripMenuItem
            // 
            this.designStatusAuditForJobToolStripMenuItem.Name = "designStatusAuditForJobToolStripMenuItem";
            this.designStatusAuditForJobToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
            this.designStatusAuditForJobToolStripMenuItem.Text = "Design Status Audit for Job";
            this.designStatusAuditForJobToolStripMenuItem.Click += new System.EventHandler(this.designStatusAuditForJobToolStripMenuItem_Click);
            // 
            // DesignBoardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1126, 660);
            this.Controls.Add(this.RefreshBtn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.weeksTabControl);
            this.Controls.Add(this.cancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
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
            this.PerformLayout();

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
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button RefreshBtn;
        private System.Windows.Forms.ToolStripMenuItem designStatusAuditForJobToolStripMenuItem;
    }
}