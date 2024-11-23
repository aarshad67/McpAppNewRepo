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
    public partial class RecreatePhaseForm : Form
    {
        int parentJobNo = 0;
        bool isPhaseCreated = false;
        public bool IsPhaseCreated
        {
            get
            {
                return isPhaseCreated;
            }
            set
            {
                isPhaseCreated = value;
            }
        }
        MeltonData mcData = new MeltonData();

        public RecreatePhaseForm()
        {
            InitializeComponent();
        }

        public RecreatePhaseForm(int parentJob)
        {
            InitializeComponent();
            parentJobNo = parentJob;
        }

        private void RecreatePhaseForm_Load(object sender, EventArgs e)
        {
            this.Text = $"Re-create a deleted phase for PARENT Job [{parentJobNo}]";
            phaseNoTextBox.Focus();
        }

        private void phaseNoTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        private void recreateButton_Click(object sender, EventArgs e)
        {
            if (!mcData.IsValidPhaseNo(phaseNoTextBox.Text)) { return; }

            string job = parentJobNo.ToString() + "." + phaseNoTextBox.Text;

            if (mcData.IsJobExists(job))
            {
                MessageBox.Show(String.Format("Job No.{0} already exists. Cannot continue", job));
                return;
            }

            string phaseStr = phaseNoTextBox.Text;
            string nextJobNo = job;
            string customerCode = mcData.GetCustomerCodeByJobNo(nextJobNo);
            string custName = mcData.GetCustName(customerCode);
            string siteAddress = mcData.GetSiteAddressFromParentJob(parentJobNo);
            string err = "";
            string wbErr = "";
            string dbErr = "";
            if (!mcData.IsJobExists(nextJobNo))
            {
                err = mcData.CreateJobPlanner(parentJobNo, nextJobNo, phaseNoTextBox.Text, "", DateTime.Now.AddYears(1), DateTime.Now.AddYears(1), siteAddress, "N", "N", "N", 0, 0, 0, "", "", "", "", 0,0,"");
            }
            if (!mcData.IsWhiteboardJobExists(nextJobNo))
            {
                wbErr = mcData.CreateWhiteBoard(nextJobNo, DateTime.Now.AddYears(1), customerCode, siteAddress, "", "", 0, 0, 0, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",
                    "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "","");
            }
            if (!mcData.IsDesignBoardJobExists(nextJobNo))
            {
                //jobNo,designDate, designStatus, requiredDate, floorlevel, suppShortname, supplierRef, stairsIncluded, salesman, supplyType, slabM2, beamM2, beamLM
                dbErr = mcData.CreateDesignBoardJob(nextJobNo, DateTime.Now.AddYears(1), "NOT DRAWN", DateTime.Now.AddYears(1),0, "", "", "", "", "", 0, 0, 0, "");
            }
            if (err == "OK" && wbErr == "OK" && dbErr == "OK")
            {
                isPhaseCreated = true;
                this.Dispose();
                this.Close();
            }

        }
    }
}
