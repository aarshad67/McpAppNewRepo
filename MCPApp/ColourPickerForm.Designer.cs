namespace MCPApp
{
    partial class ColourPickerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ColourPickerForm));
            this.colourDGV = new System.Windows.Forms.DataGridView();
            this.cancelButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.colourDGV)).BeginInit();
            this.SuspendLayout();
            // 
            // colourDGV
            // 
            this.colourDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.colourDGV.Location = new System.Drawing.Point(18, 18);
            this.colourDGV.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.colourDGV.Name = "colourDGV";
            this.colourDGV.RowHeadersWidth = 62;
            this.colourDGV.Size = new System.Drawing.Size(969, 954);
            this.colourDGV.TabIndex = 0;
            this.colourDGV.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.colourDGV_CellDoubleClick);
            // 
            // cancelButton
            // 
            this.cancelButton.BackColor = System.Drawing.Color.Red;
            this.cancelButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancelButton.ForeColor = System.Drawing.Color.White;
            this.cancelButton.Location = new System.Drawing.Point(726, 980);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(262, 66);
            this.cancelButton.TabIndex = 97;
            this.cancelButton.Text = "CANCEL";
            this.cancelButton.UseVisualStyleBackColor = false;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // ColourPickerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1005, 1060);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.colourDGV);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "ColourPickerForm";
            this.Text = "ColourPickerForm";
            this.Load += new System.EventHandler(this.ColourPickerForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.colourDGV)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView colourDGV;
        private System.Windows.Forms.Button cancelButton;
    }
}