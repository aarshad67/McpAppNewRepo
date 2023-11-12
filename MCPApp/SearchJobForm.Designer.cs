namespace MCPApp
{
    partial class SearchJobForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SearchJobForm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.searchBySiteButton = new System.Windows.Forms.Button();
            this.siteKeyTextBox = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.searchByCustButton = new System.Windows.Forms.Button();
            this.custKeyTextBox = new System.Windows.Forms.TextBox();
            this.jobDGV = new System.Windows.Forms.DataGridView();
            this.cancelButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.allButton = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.editParentMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addPhaseJobToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.searchByJobNoButton = new System.Windows.Forms.Button();
            this.jobNoKeyTextBox = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.jobDGV)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.searchBySiteButton);
            this.groupBox1.Controls.Add(this.siteKeyTextBox);
            this.groupBox1.Location = new System.Drawing.Point(417, 18);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Size = new System.Drawing.Size(488, 166);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Search by SITE ADDRESS";
            // 
            // searchBySiteButton
            // 
            this.searchBySiteButton.Location = new System.Drawing.Point(276, 92);
            this.searchBySiteButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.searchBySiteButton.Name = "searchBySiteButton";
            this.searchBySiteButton.Size = new System.Drawing.Size(176, 51);
            this.searchBySiteButton.TabIndex = 3;
            this.searchBySiteButton.Text = "SEARCH";
            this.searchBySiteButton.UseVisualStyleBackColor = true;
            this.searchBySiteButton.Click += new System.EventHandler(this.searchBySiteButton_Click);
            // 
            // siteKeyTextBox
            // 
            this.siteKeyTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.siteKeyTextBox.Location = new System.Drawing.Point(28, 43);
            this.siteKeyTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.siteKeyTextBox.Name = "siteKeyTextBox";
            this.siteKeyTextBox.Size = new System.Drawing.Size(421, 35);
            this.siteKeyTextBox.TabIndex = 2;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.searchByCustButton);
            this.groupBox2.Controls.Add(this.custKeyTextBox);
            this.groupBox2.Location = new System.Drawing.Point(927, 18);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox2.Size = new System.Drawing.Size(474, 166);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Search by CUSTOMER";
            // 
            // searchByCustButton
            // 
            this.searchByCustButton.Location = new System.Drawing.Point(260, 92);
            this.searchByCustButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.searchByCustButton.Name = "searchByCustButton";
            this.searchByCustButton.Size = new System.Drawing.Size(176, 51);
            this.searchByCustButton.TabIndex = 5;
            this.searchByCustButton.Text = "SEARCH";
            this.searchByCustButton.UseVisualStyleBackColor = true;
            this.searchByCustButton.Click += new System.EventHandler(this.searchByCustButton_Click);
            // 
            // custKeyTextBox
            // 
            this.custKeyTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.custKeyTextBox.Location = new System.Drawing.Point(28, 43);
            this.custKeyTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.custKeyTextBox.Name = "custKeyTextBox";
            this.custKeyTextBox.Size = new System.Drawing.Size(404, 35);
            this.custKeyTextBox.TabIndex = 4;
            // 
            // jobDGV
            // 
            this.jobDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.jobDGV.Location = new System.Drawing.Point(18, 235);
            this.jobDGV.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.jobDGV.Name = "jobDGV";
            this.jobDGV.RowHeadersWidth = 62;
            this.jobDGV.Size = new System.Drawing.Size(1797, 565);
            this.jobDGV.TabIndex = 7;
            this.jobDGV.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.jobDGV_CellDoubleClick);
            this.jobDGV.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.jobDGV_CellMouseUp);
            // 
            // cancelButton
            // 
            this.cancelButton.BackColor = System.Drawing.Color.Red;
            this.cancelButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancelButton.ForeColor = System.Drawing.Color.White;
            this.cancelButton.Location = new System.Drawing.Point(1644, 811);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(180, 58);
            this.cancelButton.TabIndex = 8;
            this.cancelButton.Text = "CLOSE";
            this.cancelButton.UseVisualStyleBackColor = false;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Blue;
            this.label1.Location = new System.Drawing.Point(14, 211);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(350, 20);
            this.label1.TabIndex = 125;
            this.label1.Text = "Double click on Parent Job to get to phased jobs";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.allButton);
            this.groupBox3.Location = new System.Drawing.Point(1424, 20);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox3.Size = new System.Drawing.Size(392, 165);
            this.groupBox3.TabIndex = 126;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "All Parent Jobs";
            // 
            // allButton
            // 
            this.allButton.Location = new System.Drawing.Point(88, 42);
            this.allButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.allButton.Name = "allButton";
            this.allButton.Size = new System.Drawing.Size(208, 74);
            this.allButton.TabIndex = 6;
            this.allButton.Text = "Get All Parent Jobs";
            this.allButton.UseVisualStyleBackColor = true;
            this.allButton.Click += new System.EventHandler(this.allButton_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editParentMenuItem,
            this.addPhaseJobToolStripMenuItem1});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(214, 68);
            // 
            // editParentMenuItem
            // 
            this.editParentMenuItem.Name = "editParentMenuItem";
            this.editParentMenuItem.Size = new System.Drawing.Size(213, 32);
            this.editParentMenuItem.Text = "Edit Parent Job";
            this.editParentMenuItem.Click += new System.EventHandler(this.editParentMenuItem_Click);
            // 
            // addPhaseJobToolStripMenuItem1
            // 
            this.addPhaseJobToolStripMenuItem1.Name = "addPhaseJobToolStripMenuItem1";
            this.addPhaseJobToolStripMenuItem1.Size = new System.Drawing.Size(213, 32);
            this.addPhaseJobToolStripMenuItem1.Text = "Add Phased Job";
            this.addPhaseJobToolStripMenuItem1.Click += new System.EventHandler(this.addPhaseJobToolStripMenuItem1_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.searchByJobNoButton);
            this.groupBox4.Controls.Add(this.jobNoKeyTextBox);
            this.groupBox4.Location = new System.Drawing.Point(18, 18);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox4.Size = new System.Drawing.Size(390, 166);
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Search By Job No";
            // 
            // searchByJobNoButton
            // 
            this.searchByJobNoButton.Location = new System.Drawing.Point(152, 92);
            this.searchByJobNoButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.searchByJobNoButton.Name = "searchByJobNoButton";
            this.searchByJobNoButton.Size = new System.Drawing.Size(176, 51);
            this.searchByJobNoButton.TabIndex = 1;
            this.searchByJobNoButton.Text = "SEARCH";
            this.searchByJobNoButton.UseVisualStyleBackColor = true;
            this.searchByJobNoButton.Click += new System.EventHandler(this.searchByJobNoButton_Click);
            // 
            // jobNoKeyTextBox
            // 
            this.jobNoKeyTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.jobNoKeyTextBox.Location = new System.Drawing.Point(28, 43);
            this.jobNoKeyTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.jobNoKeyTextBox.Name = "jobNoKeyTextBox";
            this.jobNoKeyTextBox.Size = new System.Drawing.Size(296, 35);
            this.jobNoKeyTextBox.TabIndex = 0;
            // 
            // SearchJobForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1840, 886);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.jobDGV);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "SearchJobForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SearchJobForm";
            this.Load += new System.EventHandler(this.SearchJobForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.jobDGV)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView jobDGV;
        private System.Windows.Forms.Button searchBySiteButton;
        private System.Windows.Forms.TextBox siteKeyTextBox;
        private System.Windows.Forms.Button searchByCustButton;
        private System.Windows.Forms.TextBox custKeyTextBox;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button allButton;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem editParentMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addPhaseJobToolStripMenuItem1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button searchByJobNoButton;
        private System.Windows.Forms.TextBox jobNoKeyTextBox;
    }
}