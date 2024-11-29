using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

namespace MCPApp
{
    public partial class CreateParentJobForm : Form
    {
        MeltonData mcData = new MeltonData();

        DataTable pJobDT = new DataTable();
        Logger logger = new Logger();
        private string loggedInUser = ConfigurationManager.AppSettings["LoggedInUser"];

        private string siteAddress = "";
        public string SiteAddress
        {
            get
            {
                return siteAddress;
            }
            set
            {
                siteAddress = value;
            }
        }

        private string siteContact = "";
        public string SiteContact
        {
            get
            {
                return siteContact;
            }
            set
            {
                siteContact = value;
            }
        }

        private string siteContactTel = "";
        public string SiteContactTel
        {
            get
            {
                return siteContactTel;
            }
            set
            {
                siteContactTel = value;
            }
        }

        private string siteContactEmail = "";

        public string SiteContactEmail
        {
            get
            {
                return siteContactEmail;
            }
            set
            {
                siteContactEmail = value;
            }
        }

        private string custCode = "";
        private string custName = "";
        private string origCustName = "";
        private string origSiteAddress = "";


        private int parentJobNo = 0;

        private bool updateDetailsFlag = false;

        public CreateParentJobForm()
        {
            InitializeComponent();
        }

        public CreateParentJobForm(int parentJob,string siteAddr,string contact,string contactTel,string contactEmail)
        {
            InitializeComponent();
            parentJobNo = parentJob;
            siteAddress = siteAddr;
            siteContact = contact;
            siteContactTel = contactTel;
            siteContactEmail = contactEmail;
        }

        public CreateParentJobForm(int parentJob,bool updateFlag)
        {
            InitializeComponent();
            parentJobNo = parentJob;
            updateDetailsFlag = updateFlag;
        }

        private void CreateParentJobForm_Load(object sender, EventArgs e)
        {
            string loggedInUser = ConfigurationManager.AppSettings["LoggedInUser"];
            label9.Text = "NOTE - Please do not use UPPERCASE for the full site address. Only use it for the postcode";

            if (loggedInUser.ToUpper() == "AA")
            {
                button1.Visible = true;
                button2.Visible = true;
                button3.Visible = true;
                button4.Visible = true;
                button5.Visible = true;
                button6.Visible = true;
                button7.Visible = true;
                button8.Visible = true;
            }
            else
            {
                button1.Visible = false;
                button2.Visible = false;
                button3.Visible = false;
                button4.Visible = false;
                button5.Visible = false;
                button6.Visible = false;
                button7.Visible = false;
                button8.Visible = false;
            }

            if (updateDetailsFlag)
            {
                if (parentJobNo > 0)
                {
                    this.Text = "Parent Job No." + parentJobNo.ToString();
                    FillCustomersCombo();
                    DataTable dt = mcData.GetParentJobsDT(parentJobNo);
                    if (dt.Rows.Count < 1) { return; }

                    foreach (DataRow dr in dt.Rows)
                    {
                        parentJobNoTextBox.Text = parentJobNo.ToString();
                        custCodeTextBox.Text = dr["custCode"] == null ? "" : dr["custCode"].ToString();
                        custCombo.Text = mcData.GetCustName(custCodeTextBox.Text);
                        siteAddressTextBox.Text = dr["siteAddress"] == null ? "" : dr["siteAddress"].ToString();
                        siteContactTextBox.Text = dr["siteContact"] == null ? "" : dr["siteContact"].ToString();
                        contactTelTextBox.Text = dr["siteContactTel"] == null ? "" : dr["siteContactTel"].ToString();
                        contactEmailTextBox.Text = dr["siteContactEmail"] == null ? "" : dr["siteContactEmail"].ToString();
                        origCustName = mcData.GetCustName(custCodeTextBox.Text);
                        origSiteAddress = siteAddressTextBox.Text;

                    }
                }
                return;
            }

            if (parentJobNo > 0)
            {
                this.Text = "Site Details";
                label3.Visible = false;
                custCombo.Visible = false;
                label5.Visible = false;
                custCodeTextBox.Visible = false;

                parentJobNoTextBox.Enabled = true;
                siteAddressTextBox.Enabled = true;
                

                parentJobNoTextBox.Text = parentJobNo.ToString();
                siteAddressTextBox.Text = siteAddress;
                siteContactTextBox.Text = siteContact;
                contactTelTextBox.Text = siteContactTel;
                contactEmailTextBox.Text = siteContactEmail;
                return;

            }
            else
            {
                this.Text = "Create Parent Job";
                label3.Visible = true;
                custCombo.Visible = true;
                label5.Visible = true;
                custCodeTextBox.Visible = true;

                parentJobNoTextBox.Enabled = true;
                siteAddressTextBox.Enabled = true;
                

                uint pJobNo = mcData.GetNextParentJobNo();
                this.Text = "Parent Job No." + pJobNo.ToString();
                parentJobNoTextBox.Text = pJobNo.ToString();
                FillCustomersCombo();
                custCombo.Text = "";
                custCombo.Focus();
            }
            
        }

        private void ClearParentJobData()
        {
            parentJobNoTextBox.Text = mcData.GetNextParentJobNo().ToString();
            siteAddressTextBox.Text = String.Empty;
            siteContactTextBox.Text = String.Empty;
            contactTelTextBox.Text = String.Empty;
            contactEmailTextBox.Text = String.Empty;
            custCombo.Text = String.Empty;
            custCodeTextBox.Text = String.Empty;

        }

        private void FillCustomersCombo()
        {
            DataTable dt = mcData.GetAllCustomerForCombo();
            custCombo.DataSource = dt;
            custCombo.ValueMember = dt.Columns[0].ColumnName;
            custCombo.DisplayMember = dt.Columns[1].ColumnName.ToString();
            return;

        }

        private void custCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!custCombo.Focused)
            {
                return;
            }

            custCodeTextBox.Text = custCombo.SelectedValue.ToString();
            siteAddressTextBox.Focus();
            return;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        private void FillPArentJobDetailsDatatable()
        {
            try
            {
                pJobDT.Columns.Clear();
                pJobDT.Columns.Add("parentJobNo", typeof(uint));
                pJobDT.Columns.Add("custCode", typeof(string));
                pJobDT.Columns.Add("custName", typeof(string));
                pJobDT.Columns.Add("siteAddress", typeof(string));
                pJobDT.Columns.Add("siteContact", typeof(string));
                pJobDT.Columns.Add("siteContactTel", typeof(string));
                pJobDT.Columns.Add("siteContactEmail", typeof(string));

                DataRow dr = pJobDT.NewRow();
                dr["parentJobNo"] = Convert.ToUInt32(parentJobNoTextBox.Text);
                dr["custCode"] = custCodeTextBox.Text;
                dr["custName"] = custCombo.Text;
                dr["siteAddress"] = siteAddressTextBox.Text;
                dr["siteContact"] = siteContactTextBox.Text;
                dr["siteContactTel"] = contactTelTextBox.Text;
                dr["siteContactEmail"] = contactEmailTextBox.Text;
                pJobDT.Rows.Add(dr);
                return;
            }
            catch (Exception ex)
            {
                string msg = String.Format("FillPArentJobDetailsDatatable() Error : {0}", ex.Message.ToString());
                string audit = mcData.CreateErrorAudit("CreateParentJobForm.cs", "FillPArentJobDetailsDatatable()", msg);
                logger.LogLine(msg);
                return;
            }
            

        }

        private void continueButton_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(custCombo.Text)) { MessageBox.Show("Customer needs adding"); custCombo.Focus(); return; }

            if (String.IsNullOrWhiteSpace(custCodeTextBox.Text)) { MessageBox.Show("Customer account needs adding"); custCodeTextBox.Focus(); return; }

            if (String.IsNullOrWhiteSpace(siteAddressTextBox.Text)) 
            { 
                MessageBox.Show("Site Address needs adding"); 
                siteAddressTextBox.Focus(); 
                return; }

            if(parentJobNoTextBox.Text.Length > 5) 
            { 
                MessageBox.Show("Parent Job Number MUST be 5 digits long");
                parentJobNoTextBox.Text = "";
                parentJobNoTextBox.Focus(); 
                return; 
            }

            if (!updateDetailsFlag)
            {
                if (mcData.IsParentJobExists(Convert.ToInt32(parentJobNoTextBox.Text)))
                {
                    string siteAddress = "";
                    string custCode = "";
                    string dateCreated = "";
                    string createdBy = "";

                    mcData.GetParentJobDetail(Convert.ToInt32(parentJobNoTextBox.Text), out siteAddress, out custCode, out dateCreated, out createdBy);
                    string message = "**** WARNING ****" + Environment.NewLine + Environment.NewLine + Environment.NewLine +
                            String.Format("Parent Job No.{0} already exists", parentJobNoTextBox.Text) + Environment.NewLine + Environment.NewLine +
                            "Site Address : " + siteAddress + Environment.NewLine +
                            "Customer : " + mcData.GetCustName(custCode) + Environment.NewLine +
                            "Job Created : " + dateCreated + " by MCP user [" + createdBy + "]";

                    MessageBox.Show(message);
                    ClearParentJobData();
                    parentJobNoTextBox.Focus();
                    return;
                }
            }
            

            if (updateDetailsFlag)
            {
                string result = mcData.UpdateParentJob(parentJobNo,custCodeTextBox.Text,siteAddressTextBox.Text,siteContactTextBox.Text,contactTelTextBox.Text,contactEmailTextBox.Text);
                if (result == "OK")
                {
                    MessageBox.Show(String.Format("Parent Job No.{0} UPDATED successfully", parentJobNo));
                    if (!String.Equals(origCustName, custCombo.Text))
                    {
                        string err = mcData.UpdateCustCodeInWhiteboard(parentJobNoTextBox.Text, custCodeTextBox.Text);
                        if (err != "OK")
                        {
                            MessageBox.Show(String.Format("Error updating Whiteboard with Cust Name : {0}", err));
                            return;
                        }
                    }
                    if (!String.Equals(origSiteAddress, siteAddressTextBox))
                    {
                        string err = mcData.UpdateJobPlannerSiteAddrByParentJob(parentJobNoTextBox.Text, siteAddressTextBox.Text);
                        if (err != "OK")
                        {
                            MessageBox.Show(String.Format("Error updating JobPlanner with Site Address : {0}", err));
                            return;
                        }
                    }
                    this.Dispose();
                    this.Close();
                    return;
                }
                else
                {
                    MessageBox.Show(String.Format("Error updating Parent Job No.{0} : {1}", parentJobNo, result));
                    this.Dispose();
                    this.Close();
                    return;
                }
                
            }

            if (parentJobNo == 0)
            {
                FillPArentJobDetailsDatatable();
                JobEntryForm jobForm = new JobEntryForm(pJobDT);
                jobForm.ShowDialog();
                if (jobForm.CancelJob)
                {
                    this.Dispose();
                    this.Close();
                }
            }
            else
            {
                string result = mcData.UpdateParentJobWithSiteAddr(parentJobNo, siteContactTextBox.Text, contactTelTextBox.Text, contactEmailTextBox.Text);
                if (result == "OK")
                {
                    MessageBox.Show(String.Format("Parent Job No.{0} updated successfully", parentJobNo));
                    siteAddress = siteAddressTextBox.Text;
                    siteContact = siteContactTextBox.Text;
                    siteContactTel = contactTelTextBox.Text;
                    siteContactEmail = contactEmailTextBox.Text;
                    this.Dispose();
                    this.Close();
                }
                else
                {
                    MessageBox.Show(String.Format("Error updating Parent Job No.{0} : {1}",parentJobNo,result));
                    this.Dispose();
                    this.Close();
                }
            }
            
            return;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            custCodeTextBox.Text = "BOLE001";
            custCombo.Text = "Bole Construction Limited";
            siteAddressTextBox.Text = "123 Acme Lane, Acme Village, Leicester, LE4";
            siteContactTextBox.Text = "Joe Bloggs";
            contactEmailTextBox.Text = custCodeTextBox.Text + "@" + custCodeTextBox.Text + ".com";
            contactTelTextBox.Text = "0123 456 7890";

            for (int i = 1; i < 10; i++)
            {
                string suffix = i < 10 ? "0" + i.ToString() : i.ToString();
                string lockType = i % 2 == 0 ? "WB" : "JP";
                string response = mcData.CreateDummyJobLock("99999." + suffix, lockType, loggedInUser);
            }
            return;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            custCodeTextBox.Text = "DAVI002";
            custCombo.Text = "Anthony A Davies Ltd";
            siteAddressTextBox.Text = "100 Apple Lane, Appleton, Derby, DE4";
            siteContactTextBox.Text = "Dave Steele";
            contactEmailTextBox.Text = custCodeTextBox.Text + "@" + custCodeTextBox.Text + ".com";
            contactTelTextBox.Text = "0123 456 7890";
            return;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            custCodeTextBox.Text = "CONT001";
            custCombo.Text = "Contract Trading Services Ltd";
            siteAddressTextBox.Text = "3 Lothair Road, Aylestone, Leicester, LE2";
            siteContactTextBox.Text = "Jason Smith";
            contactEmailTextBox.Text = custCodeTextBox.Text + "@" + custCodeTextBox.Text + ".com";
            contactTelTextBox.Text = "0123 456 7890";
            return;

        }

        private void button4_Click(object sender, EventArgs e)
        {
            custCodeTextBox.Text = "MEND001";
            custCombo.Text = "Mendip Developments Ltd";
            siteAddressTextBox.Text = "100 New North Road, Islington, London, N3";
            siteContactTextBox.Text = "Bal Duhra";
            contactEmailTextBox.Text = custCodeTextBox.Text + "@" + custCodeTextBox.Text + ".com";
            contactTelTextBox.Text = "0123 456 7890";
            return;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            custCodeTextBox.Text = "PASD001";
            custCombo.Text = "PAS Developments Ltd";
            siteAddressTextBox.Text = "51 Evington Valley Rd";
            siteContactTextBox.Text = "Tariq Ali";
            contactEmailTextBox.Text = custCodeTextBox.Text + "@" + custCodeTextBox.Text + ".com";
            contactTelTextBox.Text = "0123 456 7890";
            return;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            custCodeTextBox.Text = "LAND001";
            custCombo.Text = "T&A Land Associates Ltd";
            siteAddressTextBox.Text = "96 Fox Lane, Palmers Green, London, NW6";
            siteContactTextBox.Text = "Tariq Ali";
            contactEmailTextBox.Text = custCodeTextBox.Text + "@" + custCodeTextBox.Text + ".com";
            contactTelTextBox.Text = "0123 456 7890";
            return;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            custCodeTextBox.Text = "WORK001";
            custCombo.Text = "Workforce Civils Limited";
            siteAddressTextBox.Text = "100 Fonthill Road, Bristol, BS3";
            siteContactTextBox.Text = "Ray Haque";
            contactEmailTextBox.Text = custCodeTextBox.Text + "@" + custCodeTextBox.Text + ".com";
            contactTelTextBox.Text = "0123 456 7890";
            return;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            custCodeTextBox.Text = "RAWL001";
            custCombo.Text = "Rawle Gammon & Baker Holdings Ltd";
            siteAddressTextBox.Text = "101 Bromford Road, Birmingham, B3";
            siteContactTextBox.Text = "Zaki Afzal";
            contactEmailTextBox.Text = custCodeTextBox.Text + "@" + custCodeTextBox.Text + ".com";
            contactTelTextBox.Text = "0123 456 7890";
            return;
        }

        
    }
}
