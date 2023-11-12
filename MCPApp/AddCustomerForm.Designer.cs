namespace MCPApp
{
    partial class AddCustomerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddCustomerForm));
            this.custNameTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.nonExistingAccountCheckBox = new System.Windows.Forms.CheckBox();
            this.tempCustCheckBox = new System.Windows.Forms.CheckBox();
            this.removeCustomerButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.updateCustomerButton = new System.Windows.Forms.Button();
            this.contactTelTextBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.contactMobileTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.contactEmailTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.custCodeTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.contactNameTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.updateCustCodecheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // custNameTextBox
            // 
            this.custNameTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.custNameTextBox.Location = new System.Drawing.Point(370, 94);
            this.custNameTextBox.MaxLength = 30;
            this.custNameTextBox.Name = "custNameTextBox";
            this.custNameTextBox.Size = new System.Drawing.Size(728, 35);
            this.custNameTextBox.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(70, 98);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(292, 35);
            this.label3.TabIndex = 83;
            this.label3.Text = "Customer Full Name :";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // nonExistingAccountCheckBox
            // 
            this.nonExistingAccountCheckBox.AutoSize = true;
            this.nonExistingAccountCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nonExistingAccountCheckBox.Location = new System.Drawing.Point(514, 471);
            this.nonExistingAccountCheckBox.Name = "nonExistingAccountCheckBox";
            this.nonExistingAccountCheckBox.Size = new System.Drawing.Size(375, 33);
            this.nonExistingAccountCheckBox.TabIndex = 7;
            this.nonExistingAccountCheckBox.Text = "Non Existing Customer Account";
            this.nonExistingAccountCheckBox.UseVisualStyleBackColor = true;
            // 
            // tempCustCheckBox
            // 
            this.tempCustCheckBox.AutoSize = true;
            this.tempCustCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tempCustCheckBox.Location = new System.Drawing.Point(514, 425);
            this.tempCustCheckBox.Name = "tempCustCheckBox";
            this.tempCustCheckBox.Size = new System.Drawing.Size(423, 33);
            this.tempCustCheckBox.TabIndex = 6;
            this.tempCustCheckBox.Text = "Temporary Customer Account Code";
            this.tempCustCheckBox.UseVisualStyleBackColor = true;
            // 
            // removeCustomerButton
            // 
            this.removeCustomerButton.ForeColor = System.Drawing.Color.Red;
            this.removeCustomerButton.Location = new System.Drawing.Point(514, 555);
            this.removeCustomerButton.Name = "removeCustomerButton";
            this.removeCustomerButton.Size = new System.Drawing.Size(298, 52);
            this.removeCustomerButton.TabIndex = 9;
            this.removeCustomerButton.Text = "REMOVE CUSTOMER";
            this.removeCustomerButton.UseVisualStyleBackColor = true;
            this.removeCustomerButton.Click += new System.EventHandler(this.removeCustomerButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.BackColor = System.Drawing.Color.Red;
            this.cancelButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancelButton.ForeColor = System.Drawing.Color.White;
            this.cancelButton.Location = new System.Drawing.Point(852, 555);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(262, 51);
            this.cancelButton.TabIndex = 10;
            this.cancelButton.Text = "CANCEL";
            this.cancelButton.UseVisualStyleBackColor = false;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // updateCustomerButton
            // 
            this.updateCustomerButton.Location = new System.Drawing.Point(170, 555);
            this.updateCustomerButton.Name = "updateCustomerButton";
            this.updateCustomerButton.Size = new System.Drawing.Size(298, 52);
            this.updateCustomerButton.TabIndex = 8;
            this.updateCustomerButton.Text = "UPDATE CUSTOMER";
            this.updateCustomerButton.UseVisualStyleBackColor = true;
            this.updateCustomerButton.Click += new System.EventHandler(this.updateCustomerButton_Click);
            // 
            // contactTelTextBox
            // 
            this.contactTelTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.contactTelTextBox.Location = new System.Drawing.Point(370, 295);
            this.contactTelTextBox.Name = "contactTelTextBox";
            this.contactTelTextBox.Size = new System.Drawing.Size(728, 35);
            this.contactTelTextBox.TabIndex = 4;
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(164, 294);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(200, 35);
            this.label7.TabIndex = 82;
            this.label7.Text = "Contact Tel :";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // contactMobileTextBox
            // 
            this.contactMobileTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.contactMobileTextBox.Location = new System.Drawing.Point(370, 355);
            this.contactMobileTextBox.Name = "contactMobileTextBox";
            this.contactMobileTextBox.Size = new System.Drawing.Size(492, 35);
            this.contactMobileTextBox.TabIndex = 5;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(164, 354);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(200, 35);
            this.label6.TabIndex = 81;
            this.label6.Text = "Contact Mobile :";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // contactEmailTextBox
            // 
            this.contactEmailTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.contactEmailTextBox.Location = new System.Drawing.Point(370, 234);
            this.contactEmailTextBox.MaxLength = 50;
            this.contactEmailTextBox.Name = "contactEmailTextBox";
            this.contactEmailTextBox.Size = new System.Drawing.Size(728, 35);
            this.contactEmailTextBox.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(164, 234);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(200, 35);
            this.label2.TabIndex = 80;
            this.label2.Text = "Contact Email :";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // custCodeTextBox
            // 
            this.custCodeTextBox.BackColor = System.Drawing.Color.Yellow;
            this.custCodeTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.custCodeTextBox.Location = new System.Drawing.Point(370, 43);
            this.custCodeTextBox.MaxLength = 10;
            this.custCodeTextBox.Name = "custCodeTextBox";
            this.custCodeTextBox.Size = new System.Drawing.Size(298, 35);
            this.custCodeTextBox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(64, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(298, 35);
            this.label1.TabIndex = 79;
            this.label1.Text = "Customer Code :";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // contactNameTextBox
            // 
            this.contactNameTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.contactNameTextBox.Location = new System.Drawing.Point(370, 174);
            this.contactNameTextBox.MaxLength = 30;
            this.contactNameTextBox.Name = "contactNameTextBox";
            this.contactNameTextBox.Size = new System.Drawing.Size(728, 35);
            this.contactNameTextBox.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(164, 174);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(200, 35);
            this.label4.TabIndex = 85;
            this.label4.Text = "Contact Name:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // updateCustCodecheckBox
            // 
            this.updateCustCodecheckBox.AutoSize = true;
            this.updateCustCodecheckBox.Location = new System.Drawing.Point(698, 48);
            this.updateCustCodecheckBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.updateCustCodecheckBox.Name = "updateCustCodecheckBox";
            this.updateCustCodecheckBox.Size = new System.Drawing.Size(242, 24);
            this.updateCustCodecheckBox.TabIndex = 86;
            this.updateCustCodecheckBox.Text = "Update Customer Code Only ";
            this.updateCustCodecheckBox.UseVisualStyleBackColor = true;
            this.updateCustCodecheckBox.CheckStateChanged += new System.EventHandler(this.updateCustCodecheckBox_CheckStateChanged);
            // 
            // AddCustomerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1224, 689);
            this.ControlBox = false;
            this.Controls.Add(this.updateCustCodecheckBox);
            this.Controls.Add(this.contactNameTextBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.custNameTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.nonExistingAccountCheckBox);
            this.Controls.Add(this.tempCustCheckBox);
            this.Controls.Add(this.removeCustomerButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.updateCustomerButton);
            this.Controls.Add(this.contactTelTextBox);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.contactMobileTextBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.contactEmailTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.custCodeTextBox);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "AddCustomerForm";
            this.Text = "AddCustomerForm";
            this.Load += new System.EventHandler(this.AddCustomerForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox custNameTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox nonExistingAccountCheckBox;
        private System.Windows.Forms.CheckBox tempCustCheckBox;
        private System.Windows.Forms.Button removeCustomerButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button updateCustomerButton;
        private System.Windows.Forms.TextBox contactTelTextBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox contactMobileTextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox contactEmailTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox custCodeTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox contactNameTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox updateCustCodecheckBox;
    }
}