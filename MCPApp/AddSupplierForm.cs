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
    public partial class AddSupplierForm : Form
    {
        MeltonData mcData = new MeltonData();
        Logger logger = new Logger();

        private static bool _isNewSupplier = false;
        string suppName = "";
        string suppCode = "";
        string origSuppCode = "";
        string contactEmail = "";
        string tempSuppCode = "";
        string nonExistingSupplier = "";
        string contactName = "";
        string contactTel = "";
        string contactMobile = "";
        private string suppCodeUpdated = "";
        public string SuppCodeUpdated
        {
            get
            {
                return suppCodeUpdated;
            }
            set
            {
                suppCodeUpdated = value;
            }
        }

        public AddSupplierForm()
        {
            InitializeComponent();
        }

        public AddSupplierForm(string suppCodeKey)
        {
            InitializeComponent();
            suppCode = suppCodeKey;
            origSuppCode = suppCodeKey;
        }

        public void FillProductTypeCombo()
        {
            productTypeCombo.Items.Add("BEAM");
            productTypeCombo.Items.Add("BEAM & STAIRS");
            productTypeCombo.Items.Add("SLAB");
            productTypeCombo.Items.Add("SLAB & STAIRS");
            productTypeCombo.Items.Add("STAIRS");
        }

        private void AddSupplierForm_Load(object sender, EventArgs e)
        {
            this.Text = "Add Supplier";
            if (String.IsNullOrWhiteSpace(suppCode))
            {
                EmptyDetails();
                suppCodeTextBox.ReadOnly = false;
                _isNewSupplier = true;
            }
            else
            {
                FillSupplierDetails(suppCode);
                suppCodeTextBox.ReadOnly = true;
                _isNewSupplier = false;
            }
            FillProductTypeCombo();
            
            
        }

        
        private void cancelButton_Click(object sender, EventArgs e)
        {
            suppCodeUpdated = suppCodeTextBox.Text;
            this.Close();
        }

        private void updateSupplierButton_Click(object sender, EventArgs e)
        {
            string suppName = suppNameTextBox.Text;
            string suppCode = suppCodeTextBox.Text;
            string shortname = shortnameTextBox.Text;
            string suppAddress = suppAddressTextBox.Text;
            string productType = productTypeCombo.Text;
            string contactEmail = contactEmailTextBox.Text;
            string contactName = contactNameTextBox.Text;
            string contactTel = contactTelTextBox.Text;
            string colourCode = colourCodeTextBox.Text;
            int rgb1 = Convert.ToInt16(rgb1TextBox.Text);
            int rgb2 = Convert.ToInt16(rgb2TextBox.Text);
            int rgb3 = Convert.ToInt16(rgb3TextBox.Text);

            if (String.IsNullOrWhiteSpace(shortnameTextBox.Text))
            {
                MessageBox.Show("[Shortname] is a MANDATORY field when creating a supplier account");
                shortnameTextBox.Focus();
                return;
            }

            if(_isNewSupplier)
            {
                if(mcData.IsSupplierShortNameExists(shortnameTextBox.Text.Trim()))
                {
                    string existingSuppCode = "";
                    string existingSuppName = "";
                    mcData.GetSupplierDetailsByShortname(shortnameTextBox.Text.Trim(), out existingSuppCode,out existingSuppName);
                    string warning = $"Shortname[{shortnameTextBox.Text.Trim()}] already exists for : {Environment.NewLine} Supplier Name : {existingSuppName} {Environment.NewLine} Account :  {existingSuppCode}";
                    MessageBox.Show(warning, "WARNING");
                    shortnameTextBox.Text = "";
                    shortnameTextBox.Focus();
                    return;
                }
            }

            if (updateSuppCodeCheckBox.Checked)
            {
                int num = mcData.DeleteSupplier(origSuppCode);
                if (num > 0)
                {
                    string err = mcData.CreateSupplier(suppCode.ToUpper(), suppName,shortname, suppAddress, productType, contactName, contactEmail, contactTel,colourCode,rgb1,rgb2,rgb3);
                    if (err == "OK")
                    {
                        MessageBox.Show(String.Format("Supplier [{0} - {1}] updated successfully", suppCode, suppName));
                        suppCodeUpdated = suppCode;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show(String.Format("Error updating Supplier [{0} - {1}] error : {2}", suppCode, suppName, err));
                        suppCodeUpdated = "ERR";
                        this.Close();
                    }
                }
                return;
            }


            if (mcData.IsSupplierExists(suppCodeTextBox.Text.ToUpper()))
            {
                string err = mcData.UpdateSupplier(suppCode.ToUpper(), suppName,shortname, suppAddress, productType, contactName, contactEmail, contactTel,colourCode,rgb1,rgb2,rgb3);
                if (err == "OK")
                {
                    MessageBox.Show(String.Format("Supplier [{0} - {1}] updated successfully", suppCode, suppName));
                    suppCodeUpdated = suppCode;
                    this.Close();
                }
                else
                {
                    MessageBox.Show(String.Format("Error updating  [{0} - {1}] error : {2}", suppCode, suppName, err));
                    suppCodeUpdated = "ERR";
                    this.Close();
                }
            }
            else
            {
                string err = mcData.CreateSupplier(suppCode.ToUpper(), suppName,shortname, suppAddress, productType, contactName, contactEmail, contactTel, colourCode, rgb1, rgb2, rgb3);
                if (err == "OK")
                {
                    MessageBox.Show(String.Format("Supplier [{0} - {1}] created successfully", suppCode, suppName));
                    suppCodeUpdated = suppCode;
                    this.Close();
                }
                else
                {
                    MessageBox.Show(String.Format("Error created [{0} - {1}] error : {2}", suppCode, suppName, err));
                    suppCodeUpdated = "ERR";
                    this.Close();
                }
            }
        }

        private void EmptyDetails()
        {
            suppCodeTextBox.Text = String.Empty;
            suppNameTextBox.Text = String.Empty;
            shortnameTextBox.Text = String.Empty;
            suppAddressTextBox.Text = String.Empty;
            productTypeCombo.Text = String.Empty;
            contactNameTextBox.Text = String.Empty;
            contactEmailTextBox.Text = String.Empty;
            contactTelTextBox.Text = String.Empty;
        }

        private void FillSupplierDetails(string suppCode)
        {
            EmptyDetails();
            try
            {
                int rgb1, rgb2, rgb3 = 255;
                DataTable dt = mcData.GetSupplierByCode(suppCode);
                foreach (DataRow dr in dt.Rows)
                {
                    rgb1 = dr["rgb1"] == null ? 255 : Convert.ToInt16(dr["rgb1"].ToString());
                    rgb2 = dr["rgb2"] == null ? 255 : Convert.ToInt16(dr["rgb2"].ToString());
                    rgb3 = dr["rgb3"] == null ? 255 : Convert.ToInt16(dr["rgb3"].ToString());
                    suppCodeTextBox.Text = dr["suppCode"].ToString();
                    suppNameTextBox.Text = dr["suppName"].ToString();
                    shortnameTextBox.Text = dr["shortname"].ToString();
                    suppAddressTextBox.Text = dr["suppAddress"].ToString();
                    productTypeCombo.Text = dr["productType"].ToString();
                    contactNameTextBox.Text = dr["contactName"].ToString();
                    contactEmailTextBox.Text = dr["contactEmail"].ToString();
                    contactTelTextBox.Text = dr["contactTel"].ToString();
                    colourCodeTextBox.Text = dr["colourCode"].ToString();
                    rgb1TextBox.Text = dr["rgb1"].ToString();
                    rgb2TextBox.Text = dr["rgb2"].ToString();
                    rgb3TextBox.Text = dr["rgb3"].ToString();
                    panel1.BackColor = Color.FromArgb(rgb1, rgb2, rgb3);

                }
            }
            catch (Exception ex)
            {
                string msg = String.Format("FillSupplierDetails() Error : {0}", ex.Message.ToString());
                string audit = mcData.CreateErrorAudit("AddSupplierForm.cs", String.Format("FillSupplierDetails(string {0})", suppCode), msg);
                logger.LogLine(msg);
                return;
            }
            
        }

        private void removeSupplierButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(String.Format("Confirm deletion of [{0} - {1}] ?", suppCodeTextBox.Text, suppNameTextBox.Text), "Confirm Delete", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                if (mcData.IsSuppShortnameUsedInJobPlanner(shortnameTextBox.Text))
                {
                    MessageBox.Show(String.Format("Cannot delete Supplier [{0}]. It is being used in job(s)", shortnameTextBox.Text));
                    return;
                }

                if (mcData.IsSuppShortnameUsedInWhiteboard(shortnameTextBox.Text))
                {
                    MessageBox.Show(String.Format("Cannot delete Supplier [{0}]. It is being used in job(s)", shortnameTextBox.Text));
                    return;
                }


                int num = mcData.DeleteSupplier(suppCodeTextBox.Text.ToUpper());
                if (num > 0)
                {
                    suppCodeUpdated = "DEL";
                    this.Close();
                    return;
                }
                else
                {
                    MessageBox.Show(String.Format("[{0} - {1}] was NOT deleted ?", suppCodeTextBox.Text, suppNameTextBox.Text));
                    suppCodeUpdated = suppCodeTextBox.Text;
                    this.Close();
                    return;
                }
            }
        }

        private void updateSuppCodeCheckBox_CheckStateChanged(object sender, EventArgs e)
        {
            if (updateSuppCodeCheckBox.Checked)
            {
                suppCodeTextBox.ReadOnly = false;
                suppCodeTextBox.Focus();
                return;
            }
            else
            {
                suppCodeTextBox.ReadOnly = true;
                return;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ColourPickerForm colorForm = new ColourPickerForm();
            colorForm.ShowDialog();
            colourCodeTextBox.Text = colorForm.ColourCode;
            rgb1TextBox.Text = colorForm.RGB1.ToString();
            rgb2TextBox.Text = colorForm.RGB2.ToString();
            rgb3TextBox.Text = colorForm.RGB3.ToString();
            panel1.BackColor = Color.FromArgb(colorForm.RGB1, colorForm.RGB2, colorForm.RGB3);
            return;
        }
    }
}
