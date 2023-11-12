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
    public partial class EditParentJobForm : Form
    {
        int parentJobNo = 0;
        MeltonData mcData = new MeltonData();
        Logger logger = new Logger();

        public EditParentJobForm()
        {
            InitializeComponent();
        }

        public EditParentJobForm(int parent)
        {
            InitializeComponent();
            parentJobNo = parent;
        }

        private void EditParentJobForm_Load(object sender, EventArgs e)
        {
            this.Text = String.Format("Edit PARENT job no.{0}", parentJobNo.ToString());
            FillCustomersCombo();
            FillDetails();
            siteAddressTextBox.Enabled = false;
        }

        private void FillCustomersCombo()
        {
            DataTable dt = mcData.GetAllCustomerForCombo();
            custCombo.DataSource = dt;
            custCombo.ValueMember = dt.Columns[0].ColumnName;
            custCombo.DisplayMember = dt.Columns[1].ColumnName.ToString();
            return;

        }

        private void FillDetails()
        {
            DataTable dt = mcData.GetParentJobsDT(parentJobNo);

            foreach (DataRow dr in dt.Rows)
            {
                parentJobNoTextBox.Text = dr["parentJobNo"].ToString();
                custCombo.Text = mcData.GetCustName(dr["custCode"].ToString());
                custCodeTextBox.Text = dr["custCode"].ToString();
                siteAddressTextBox.Text = dr["siteAddress"].ToString();
                siteContactTextBox.Text = dr["siteContact"].ToString();
                contactTelTextBox.Text = dr["siteContactTel"].ToString();
                contactEmailTextBox.Text = dr["siteContactEmail"].ToString();
            }
        }

        private void continueButton_Click(object sender, EventArgs e)
        {
            string err = mcData.UpdateParentJob(parentJobNo, custCodeTextBox.Text, siteAddressTextBox.Text, siteContactTextBox.Text, contactTelTextBox.Text, contactEmailTextBox.Text);
            if (err != "OK")
            {
                MessageBox.Show(String.Format("Error updating Parent Job No.{0} : {1}", parentJobNo.ToString(), err));
                return;
            }
            else
            {
                MessageBox.Show(String.Format("Parent Job No.{0} updated successfully", parentJobNo.ToString()));
                string updateErr = mcData.UpdateCustCodeInWhiteboard(parentJobNo.ToString(), custCodeTextBox.Text);
                if (updateErr != "OK")
                {
                    MessageBox.Show(String.Format("Error updating customer on Whiteboard for all jobs on Parent Job No.{0} : {1}", parentJobNo.ToString(), err));
                    return;
                }
                this.Dispose();
                this.Close();
                return;
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        private void custCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!custCombo.Focused)
            {
                return;
            }

            custCodeTextBox.Text = custCombo.SelectedValue.ToString();
            siteContactTextBox.Focus();
            return;
        }
    }
}
