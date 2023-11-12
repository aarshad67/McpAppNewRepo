namespace MCPApp
{
    partial class WhiteboardDayCommentAuditForm
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
            this.commentDGV = new System.Windows.Forms.DataGridView();
            this.cancelButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.commentDGV)).BeginInit();
            this.SuspendLayout();
            // 
            // commentDGV
            // 
            this.commentDGV.AllowUserToAddRows = false;
            this.commentDGV.AllowUserToDeleteRows = false;
            this.commentDGV.AllowUserToResizeColumns = false;
            this.commentDGV.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.commentDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.commentDGV.Location = new System.Drawing.Point(18, 18);
            this.commentDGV.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.commentDGV.Name = "commentDGV";
            this.commentDGV.RowHeadersWidth = 62;
            this.commentDGV.Size = new System.Drawing.Size(1310, 777);
            this.commentDGV.TabIndex = 3;
            // 
            // cancelButton
            // 
            this.cancelButton.BackColor = System.Drawing.Color.Red;
            this.cancelButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancelButton.ForeColor = System.Drawing.Color.White;
            this.cancelButton.Location = new System.Drawing.Point(1065, 803);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(262, 58);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "CLOSE";
            this.cancelButton.UseVisualStyleBackColor = false;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // WhiteboardDayCommentAuditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1346, 874);
            this.ControlBox = false;
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.commentDGV);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "WhiteboardDayCommentAuditForm";
            this.Text = "WhiteboardDayCommentAuditForm";
            this.Load += new System.EventHandler(this.WhiteboardDayCommentAuditForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.commentDGV)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView commentDGV;
        private System.Windows.Forms.Button cancelButton;
    }
}