using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MCPApp
{
    public partial class AddCustomerForm : Form
    {
        MeltonData mcData = new MeltonData();
        Logger logger = new Logger();
        string custName = ""; 
	    string custCode = "";
        string origCustCode = "";
	    string contactEmail = "";
	    string tempCustCode = ""; 
	    string nonExistingCustomer = ""; 
	    string contactName = ""; 
	    string contactTel = "";
        string contactMobile = "";
        private string custCodeUpdated = "";
        public string CustCodeUpdated
        {
            get
            {
                return custCodeUpdated;
            }
            set
            {
                custCodeUpdated = value;
            }
        }
        private bool viewCustDetailFlag = false;



        public AddCustomerForm()
        {
            InitializeComponent();
        }

        public AddCustomerForm(string custCodeKey)
        {
            InitializeComponent();
            custCode = custCodeKey;
            origCustCode = custCodeKey;
        }

        public AddCustomerForm(string custCodeKey, bool viewFlag)
        {
            InitializeComponent();
            custCode = custCodeKey;
            viewCustDetailFlag = viewFlag;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            custCodeUpdated = custCodeTextBox.Text;
            this.Close();
        }

        private void updateCustomerButton_Click(object sender, EventArgs e)
        {
            string custName = custNameTextBox.Text;
            string custCode = custCodeTextBox.Text;
            string contactEmail = contactEmailTextBox.Text;
            string tempCustCode = tempCustCheckBox.Checked ? "YES" : "NO";
            string nonExistingCustomer = nonExistingAccountCheckBox.Checked ? "YES" : "NO";
            string contactName = contactNameTextBox.Text;
            string contactTel = contactTelTextBox.Text;
            string contactMobile = contactMobileTextBox.Text;

            if (updateCustCodecheckBox.Checked)
            {
                int num = mcData.DeleteCustomer(origCustCode);
                if (num > 0)
                {

                    string err = mcData.CreateCustomer(custCode.ToUpper(), custName, contactName, contactEmail, contactTel, contactMobile, tempCustCode, nonExistingCustomer);
                    if (err == "OK")
                    {
                        MessageBox.Show(String.Format("Customer [{0} - {1}] updated successfully", custCode, custName));
                        custCodeUpdated = custCode;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show(String.Format("Error updating customer [{0} - {1}] error : {2}", custCode, custName, err));
                        custCodeUpdated = "ERR";
                        this.Close();
                    }
                }

                return;
            }


            if (mcData.IsCustomerExists(custCodeTextBox.Text.ToUpper()))
            {
                string err = mcData.UpdateCustomer(custCode, custName, contactName, contactEmail, contactTel, contactMobile, tempCustCode, nonExistingCustomer);
                if (err == "OK")
                {
                    MessageBox.Show(String.Format("Customer [{0} - {1}] updated successfully", custCode, custName));
                    custCodeUpdated = custCode;
                    this.Close();
                }
                else
                {
                    MessageBox.Show(String.Format("Error updating  [{0} - {1}] error : {2}", custCode, custName, err));
                    custCodeUpdated = "ERR";
                    this.Close();
                }
            }
            else
            {
                string err = mcData.CreateCustomer(custCode.ToUpper(), custName, contactName, contactEmail, contactTel, contactMobile, tempCustCode, nonExistingCustomer);
                if (err == "OK")
                {
                    MessageBox.Show(String.Format("Customer [{0} - {1}] created successfully", custCode, custName));
                    custCodeUpdated = custCode;
                    this.Close();
                }
                else
                {
                    MessageBox.Show(String.Format("Error created [{0} - {1}] error : {2}", custCode, custName, err));
                    custCodeUpdated = "ERR";
                    this.Close();
                }
            }
        }

        private void AddCustomerForm_Load(object sender, EventArgs e)
        {
            this.Text = "Add Customer";
            if (String.IsNullOrWhiteSpace(custCode))
            {
                EmptyDetails();
                custCodeTextBox.ReadOnly = false;
            }
            else
            {
                FillCustomerDetails(custCode);
                custCodeTextBox.ReadOnly = true;
                if (viewCustDetailFlag)
                {
                    removeCustomerButton.Visible = false;
                }
                else
                {
                    removeCustomerButton.Visible = true;
                }
            }

        }

        private void EmptyDetails()
        {
            custCodeTextBox.Text = String.Empty;
            custNameTextBox.Text = String.Empty;
            contactNameTextBox.Text = String.Empty;
            contactEmailTextBox.Text = String.Empty;
            contactTelTextBox.Text = String.Empty;
            contactMobileTextBox.Text = String.Empty;
            tempCustCheckBox.Checked = false;
            nonExistingAccountCheckBox.Checked = false; 
        }

        private void FillCustomerDetails(string custCode)
        {
            EmptyDetails();
            try
            {
                DataTable dt = mcData.GetCustomerByCode(custCode);
                foreach (DataRow dr in dt.Rows)
                {
                    custCodeTextBox.Text = dr["custCode"].ToString();
                    custNameTextBox.Text = dr["custName"].ToString();
                    contactNameTextBox.Text = dr["contactName"].ToString();
                    contactEmailTextBox.Text = dr["contactEmail"].ToString();
                    contactTelTextBox.Text = dr["contactTel"].ToString();
                    contactMobileTextBox.Text = dr["contactMobile"].ToString();
                    tempCustCheckBox.Checked = dr["tempCustCode"].ToString() == "YES" ? true : false;
                    nonExistingAccountCheckBox.Checked = dr["nonExistingCustomer"].ToString() == "YES" ? true : false;
                }
                return;
            }
            catch (Exception ex)
            {
                string msg = String.Format("FillCustomerDetails() Error : {0}", ex.Message.ToString());
                string audit = mcData.CreateErrorAudit("AddCustomerForm.cs", String.Format("FillCustomerDetails(string {0})",custCode), msg);
                logger.LogLine(msg);
                return;
            }
            
        }

        private void removeCustomerButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(String.Format("Confirm deletion of [{0} - {1}] ?", custCodeTextBox.Text, custNameTextBox.Text), "Confirm Delete", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                int num = mcData.DeleteCustomer(custCodeTextBox.Text.ToUpper());
                if (num > 0)
                {
                    custCodeUpdated = "DEL";
                    this.Close();
                    return;
                }
                else
                {
                    MessageBox.Show(String.Format("[{0} - {1}] was NOT deleted ?", custCodeTextBox.Text, custNameTextBox.Text));
                    custCodeUpdated = custCodeTextBox.Text;
                    this.Close();
                    return;
                }
            }
        }

        private void updateCustCodecheckBox_CheckStateChanged(object sender, EventArgs e)
        {
            if (updateCustCodecheckBox.Checked)
            {
                custCodeTextBox.ReadOnly = false;
                custCodeTextBox.Focus();
                return;
            }
            else
            {
                custCodeTextBox.ReadOnly = true;
                return;
            }
        }
    }
}
