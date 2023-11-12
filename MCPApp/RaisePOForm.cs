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
    public partial class RaisePOForm : Form
    {
        MeltonData mcData = new MeltonData();
        Logger logger = new Logger();
        private string jobNo = "";
        private string suppShortname = "";

        public RaisePOForm()
        {
            InitializeComponent();
        }

        public RaisePOForm(string jobNum)
        {
            InitializeComponent();
            jobNo = jobNum;
        }

        private void RaisePOForm_Load(object sender, EventArgs e)
        {
            this.Text = String.Format("Raise PO for Job No.{0}", jobNo);
            suppShortname = mcData.GetSuppShortnameFromJob(jobNo);
           // FillSupplierDetails(suppShortname);
            FillJobDetails(jobNo);
            poTextBox.Focus();
        }

        //private void EmptySupplierDetails()
        //{
        //    suppCodeTextBox.Text = String.Empty;
        //    suppNameTextBox.Text = String.Empty;
        //    shortnameTextBox.Text = String.Empty;
        //    suppAddressTextBox.Text = String.Empty;
        //    productTypeCombo.Text = String.Empty;
        //    contactNameTextBox.Text = String.Empty;
        //    contactEmailTextBox.Text = String.Empty;
        //    contactTelTextBox.Text = String.Empty;
        //}

        //private void FillSupplierDetails(string shortname)
        //{
        //    EmptySupplierDetails();
        //    try
        //    {
        //        DataTable dt = mcData.GetSupplierByShortname(shortname);
        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            suppCodeTextBox.Text = dr["suppCode"].ToString();
        //            suppNameTextBox.Text = dr["suppName"].ToString();
        //            shortnameTextBox.Text = dr["shortname"].ToString();
        //            suppAddressTextBox.Text = dr["suppAddress"].ToString();
        //            productTypeCombo.Text = dr["productType"].ToString();
        //            contactNameTextBox.Text = dr["contactName"].ToString();
        //            contactEmailTextBox.Text = dr["contactEmail"].ToString();
        //            contactTelTextBox.Text = dr["contactTel"].ToString();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string msg = String.Format("FillSupplierDetails() Error : {0}", ex.Message.ToString());
        //        logger.LogLine(msg);
        //        return;
        //    }

        //}

        private void EmptyJobDetails()
        {
            jobNoTextBox.Text = "";
            reqDateTextBox.Text = "";
            stairsSuppTextBox.Text = "";
            productTextBox.Text = "";
            totalM2TextBox.Text = "";
            ramDateTextBox.Text = "";
            sentByTextBox.Text = "";
        }

        private void FillJobDetails(string jobNum)
        {
            EmptyJobDetails();
            try
            {
                DataTable dt = mcData.GetJobPurchaseOrder(jobNum);
                foreach (DataRow dr in dt.Rows)
                {
                    jobNoTextBox.Text = jobNo;
                    reqDateTextBox.Text = Convert.ToDateTime(dr["requiredDate"].ToString()).ToShortDateString();
                    stairsSuppTextBox.Text = dr["suppShortname"].ToString();
                    productTextBox.Text = dr["products"].ToString();
                    totalM2TextBox.Text = dr["totalM2"].ToString();
                    ramDateTextBox.Text = Convert.ToDateTime(dr["ramSentDate"].ToString()).ToShortDateString();
                    sentByTextBox.Text = dr["ramSentBy"].ToString();
                }
            }
            catch (Exception ex)
            {
                string msg = String.Format("FillJobDetails() Error : {0}", ex.Message.ToString());
                logger.LogLine(msg);
                string audit = mcData.CreateErrorAudit("RaisePOForm.cs", String.Format("FillJobDetails({0})",jobNum), ex.Message);
                return;
            }

        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show(String.Format("Confirm PO Creation for Job No.{0}",jobNo),"PO Confirmation",MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                if (String.IsNullOrWhiteSpace(poTextBox.Text))
                {
                    MessageBox.Show("Invalid PO number");
                    poTextBox.Focus();
                    return;
                }
                string siteAddress = mcData.GetSiteAddressFromJobNo(jobNo);
                string poDetail = String.IsNullOrWhiteSpace(poDetailsTextBox.Text) ? "N/A" : poDetailsTextBox.Text;
                string err = mcData.UpdateJobPO(jobNo, Convert.ToDateTime(reqDateTextBox.Text), siteAddress, productTextBox.Text, Convert.ToInt16(totalM2TextBox.Text), 
                                                shortnameTextBox.Text, stairsSuppTextBox.Text, Convert.ToDateTime(ramDateTextBox.Text),sentByTextBox.Text, poTextBox.Text, poDetailsTextBox.Text);
                if (err == "OK")
                {
                    MessageBox.Show(String.Format("PO successfully raised for Job No.{0}", jobNo));
                    this.Dispose();
                    this.Close();
                    return;
                }
                else
                {
                    MessageBox.Show(String.Format("Error raising PO for Job No.{0} : {1}", jobNo,err));
                    return;
                }

            }
        }
    }
}
