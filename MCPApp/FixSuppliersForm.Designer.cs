
namespace MCPApp
{
    partial class FixSuppliersForm
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
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cancelButton = new System.Windows.Forms.Button();
            this.suppDGV = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.summaryDGV = new System.Windows.Forms.DataGridView();
            this.btnUpdateInJP = new System.Windows.Forms.Button();
            this.rbWB = new System.Windows.Forms.RadioButton();
            this.rbJP = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.suppDGV)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.summaryDGV)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(263, 21);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(587, 28);
            this.comboBox1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(221, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Select SUPPLIER to Update :";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cancelButton
            // 
            this.cancelButton.BackColor = System.Drawing.Color.Red;
            this.cancelButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancelButton.ForeColor = System.Drawing.Color.White;
            this.cancelButton.Location = new System.Drawing.Point(263, 66);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(182, 89);
            this.cancelButton.TabIndex = 156;
            this.cancelButton.Text = "CLOSE";
            this.cancelButton.UseVisualStyleBackColor = false;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // suppDGV
            // 
            this.suppDGV.AllowUserToAddRows = false;
            this.suppDGV.AllowUserToDeleteRows = false;
            this.suppDGV.AllowUserToResizeColumns = false;
            this.suppDGV.AllowUserToResizeRows = false;
            this.suppDGV.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.suppDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.suppDGV.Location = new System.Drawing.Point(888, 29);
            this.suppDGV.Name = "suppDGV";
            this.suppDGV.RowHeadersWidth = 62;
            this.suppDGV.RowTemplate.Height = 28;
            this.suppDGV.Size = new System.Drawing.Size(920, 917);
            this.suppDGV.TabIndex = 157;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(884, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(172, 20);
            this.label2.TabIndex = 158;
            this.label2.Text = "Current SUPPLIER(s) :";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(461, 66);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(188, 89);
            this.btnUpdate.TabIndex = 159;
            this.btnUpdate.Text = "Update in WB";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(16, 184);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(850, 36);
            this.progressBar1.TabIndex = 160;
            // 
            // summaryDGV
            // 
            this.summaryDGV.AllowUserToAddRows = false;
            this.summaryDGV.AllowUserToOrderColumns = true;
            this.summaryDGV.AllowUserToResizeColumns = false;
            this.summaryDGV.AllowUserToResizeRows = false;
            this.summaryDGV.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.summaryDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.summaryDGV.Location = new System.Drawing.Point(16, 281);
            this.summaryDGV.Name = "summaryDGV";
            this.summaryDGV.RowHeadersWidth = 62;
            this.summaryDGV.RowTemplate.Height = 28;
            this.summaryDGV.Size = new System.Drawing.Size(856, 665);
            this.summaryDGV.TabIndex = 161;
            // 
            // btnUpdateInJP
            // 
            this.btnUpdateInJP.Location = new System.Drawing.Point(663, 66);
            this.btnUpdateInJP.Name = "btnUpdateInJP";
            this.btnUpdateInJP.Size = new System.Drawing.Size(187, 89);
            this.btnUpdateInJP.TabIndex = 162;
            this.btnUpdateInJP.Text = "Update in JP";
            this.btnUpdateInJP.UseVisualStyleBackColor = true;
            this.btnUpdateInJP.Click += new System.EventHandler(this.btnUpdateInJP_Click);
            // 
            // rbWB
            // 
            this.rbWB.AutoSize = true;
            this.rbWB.Location = new System.Drawing.Point(16, 239);
            this.rbWB.Name = "rbWB";
            this.rbWB.Size = new System.Drawing.Size(320, 24);
            this.rbWB.TabIndex = 163;
            this.rbWB.TabStop = true;
            this.rbWB.Text = "Check Supplier Jobs ON WHITEBOARD";
            this.rbWB.UseVisualStyleBackColor = true;
            this.rbWB.CheckedChanged += new System.EventHandler(this.rbWB_CheckedChanged);
            // 
            // rbJP
            // 
            this.rbJP.AutoSize = true;
            this.rbJP.Location = new System.Drawing.Point(388, 239);
            this.rbJP.Name = "rbJP";
            this.rbJP.Size = new System.Drawing.Size(318, 24);
            this.rbJP.TabIndex = 164;
            this.rbJP.TabStop = true;
            this.rbJP.Text = "Check Supplier Jobs On JOB PLANNER";
            this.rbJP.UseVisualStyleBackColor = true;
            this.rbJP.CheckedChanged += new System.EventHandler(this.rbJP_CheckedChanged);
            // 
            // FixSuppliersForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1828, 964);
            this.Controls.Add(this.rbJP);
            this.Controls.Add(this.rbWB);
            this.Controls.Add(this.btnUpdateInJP);
            this.Controls.Add(this.summaryDGV);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.suppDGV);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "FixSuppliersForm";
            this.Text = "FixSuppliersForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FixSuppliersForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.suppDGV)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.summaryDGV)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.DataGridView suppDGV;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.DataGridView summaryDGV;
        private System.Windows.Forms.Button btnUpdateInJP;
        private System.Windows.Forms.RadioButton rbWB;
        private System.Windows.Forms.RadioButton rbJP;
    }
}