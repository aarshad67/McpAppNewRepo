using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Diagnostics;

namespace MCPApp
{
    public partial class UserOverviewForm : Form
    {
        private bool actionListClicked = false;
      //  TreeNode testExcelNode = new TreeNode("Test Excel Run");
        TreeNode jobPlannerParentNode = new TreeNode("Job Planner");
        TreeNode jobPlannerNode = new TreeNode("Job Planner - All Jobs");
        TreeNode jobPlannerBeamNode = new TreeNode("Job Planner - BEAM Jobs");
        TreeNode jobPlannerSlabNode = new TreeNode("Job Planner - SLAB Jobs ( incl STAIRS jobs )");
        TreeNode reportsParentNode = new TreeNode("REPORTS");
        TreeNode supplierRptsNode = new TreeNode("Supplier(s) Reports");
        TreeNode notOnShopRptsNode = new TreeNode("NOT ON SHOP Report");
        TreeNode dummyNode = new TreeNode("Please CLICK on options below");
        TreeNode systemAdminParentNode = new TreeNode("System Administration");
        TreeNode userMaintenanceNode = new TreeNode("User Maintenance");
        TreeNode productsNode = new TreeNode("Products");
        TreeNode customersNode = new TreeNode("Customers");
        TreeNode suppliersNode = new TreeNode("Suppliers");
        TreeNode newParentJobNode = new TreeNode("Create a new job");
        TreeNode searchParentJobNode = new TreeNode("Search for job");

        TreeNode whiteboardParentNode = new TreeNode("Whiteboard");
        TreeNode whiteboardNode = new TreeNode("Go to Whiteboard");
        TreeNode whiteboardJobExtensionsNode = new TreeNode("Whiteboard - Jobs with Extensions Only");
        TreeNode ramsPoNode = new TreeNode("RAM(s) Sent / PO(s)");
        TreeNode errorAuditNode = new TreeNode("Audit of Error(s)");
        TreeNode lockedJobsNode = new TreeNode("View LOCKED Jobs");
        TreeNode cancelledJobsNode = new TreeNode("View CANCELLED Jobs");
        TreeNode deletedJobsNode = new TreeNode("View DELETED Jobs Audit");
        TreeNode quitParentNode = new TreeNode("Quit Program");
        TreeNode jobPlannerRptsNode = new TreeNode("Job Planner Reports");
        TreeNode beamLmRptNode = new TreeNode("Beam LM Per Year Per Supplier");
        TreeNode beamM2RptNode = new TreeNode("Beam M² Per Year Per Supplier");
        TreeNode slabM2RptNode = new TreeNode("Slab M² Per Year Per Supplier");
        TreeNode jobHouseKeepingNode = new TreeNode("Job Planner Housekeeping");
        TreeNode wbHouseKeepingNode = new TreeNode("Whitebioard Housekeeping");
        TreeNode wbEmptyProductsNode = new TreeNode("Missing PRODUCTS Completed Whiteboard jobs");
        TreeNode updateBeamJobsQtyNode = new TreeNode("Update Missing LM/M² On Completed BEAM jobs");
        TreeNode updateSlabJobsQtyNode = new TreeNode("Update Missing M² On Completed SLAB jobs");
        TreeNode updateNonSpecifiedJobsQtyNode = new TreeNode("Update Completed jobs where Beam Lm/M² and SLAB M² are all ZERO  ");
        TreeNode missingSuppliersNode = new TreeNode("Update Missing SUPPLIERS on Completed Jobs");

        string mcpFullName = "";
        string mcpUserID = "";
        MeltonData mcData = new MeltonData();
        string loggedInUser = ConfigurationManager.AppSettings["LoggedInUser"];
        string mcpVersion = ConfigurationManager.AppSettings["AppVersion"];

        public UserOverviewForm()
        {
            InitializeComponent();
            this.Dispose();
        }

        public UserOverviewForm(string userID, string fullName)
        {
            mcpFullName = fullName;
            mcpUserID = userID;

            InitializeComponent();

        }

        private void UserOverviewForm_Load(object sender, EventArgs e)
        {
            this.Text = String.Format("{0} logged into Melton Concrete Portal at {1}", mcpFullName, DateTime.Now.ToLongTimeString());
            dummyNode.ForeColor = Color.White;
            dummyNode.BackColor = Color.Black;
            quitParentNode.ForeColor = Color.Red;
            quitParentNode.BackColor = Color.White;
            newParentJobNode.ForeColor = Color.Blue;


            whiteboardParentNode.Nodes.Add(whiteboardNode);
            whiteboardParentNode.Nodes.Add(whiteboardJobExtensionsNode);
            whiteboardParentNode.Nodes.Add(wbHouseKeepingNode);
            menuTreeView.Nodes.Add(dummyNode);
            if (mcData.IsUserManager(loggedInUser))
            {
                menuTreeView.Nodes.Add(userMaintenanceNode);
            }
            //  menuTreeView.Nodes.Add(productsNode);
            menuTreeView.Nodes.Add(customersNode);
            menuTreeView.Nodes.Add(suppliersNode);
            menuTreeView.Nodes.Add(newParentJobNode);
            menuTreeView.Nodes.Add(searchParentJobNode);
            menuTreeView.Nodes.Add(jobPlannerParentNode);
            menuTreeView.Nodes.Add(reportsParentNode);
            jobPlannerParentNode.Nodes.Add(jobPlannerNode);
            jobPlannerParentNode.Nodes.Add(jobPlannerBeamNode);
            jobPlannerParentNode.Nodes.Add(jobPlannerSlabNode);//
            jobPlannerParentNode.Nodes.Add(jobHouseKeepingNode);
            jobHouseKeepingNode.Nodes.Add(updateBeamJobsQtyNode);
          //  jobHouseKeepingNode.Nodes.Add(updateSlabJobsQtyNode);
            jobHouseKeepingNode.Nodes.Add(updateNonSpecifiedJobsQtyNode);//
            jobHouseKeepingNode.Nodes.Add(missingSuppliersNode);
            wbHouseKeepingNode.Nodes.Add(wbEmptyProductsNode);
            reportsParentNode.Nodes.Add(jobPlannerRptsNode);
            reportsParentNode.Nodes.Add(supplierRptsNode);
            reportsParentNode.Nodes.Add(notOnShopRptsNode);
           // jobPlannerRptsNode.Nodes.Add(testExcelNode);
            jobPlannerRptsNode.Nodes.Add(beamLmRptNode); //
            jobPlannerRptsNode.Nodes.Add(beamM2RptNode);
            jobPlannerRptsNode.Nodes.Add(slabM2RptNode);
            menuTreeView.Nodes.Add(whiteboardParentNode);
            if (loggedInUser == "sp" || loggedInUser == "dm" || loggedInUser == "aa" || loggedInUser == "ac" || loggedInUser == "eb")
            {
                menuTreeView.Nodes.Add(lockedJobsNode);
                menuTreeView.Nodes.Add(cancelledJobsNode);
                menuTreeView.Nodes.Add(deletedJobsNode);
            }
                

            if (loggedInUser == "aa")
            {
                menuTreeView.Nodes.Add(errorAuditNode);
            }

            menuTreeView.Nodes.Add(quitParentNode);
            statusStrip1.Items.Add($"SQL Connection Server : {mcData.GetServerName()}");
            statusStrip1.Items.Add($"| Database Name : {mcData.GetDatabaseName()}");
            statusStrip1.Items.Add($"| MCP App Version : {mcpVersion}");

            LoadBulletinBoard();




        }

        private void LoadBulletinBoard()
        {
            this.Cursor = Cursors.WaitCursor;
            bulletinTreeView.Nodes.Clear();
            TreeNode OnStopCustomersNode = new TreeNode("Customers flagged as ON STOP");
            TreeNode HousekeepingNode = new TreeNode("Job Planner / Whiteboard Housekeeping Notifications");
            TreeNode customerNode = new TreeNode("N/A");
            TreeNode alertNode = new TreeNode("N/A");
            bulletinTreeView.Nodes.Add(OnStopCustomersNode);
            bulletinTreeView.Nodes.Add(HousekeepingNode);

            DataTable dt = mcData.GetAllOnStopCustomersDT();
            int missingBeamCount = mcData.GetNumMissingData("BEAMZERO");
            int missingSlabCount = mcData.GetNumMissingData("SLABZERO");
            int missingAllCount = mcData.GetNumMissingData("ALLZERO");
            int missingSuppCount = mcData.GetNumMissingData("MISSINGSUPPLIER");
            int missingProducts = mcData.GetNumMissingData("MISSINGPRODUCTS");

            alertNode = new TreeNode($"Number of completed BEAM jobs with Missing LM or M²: {missingBeamCount}");
            HousekeepingNode.Nodes.Add(alertNode);
            //alertNode = new TreeNode($"Number of completed SLAB jobs with Missing M²: {missingSlabCount}");
            //HousekeepingNode.Nodes.Add(alertNode);
            alertNode = new TreeNode($"Number of completed jobs where Beam LM/M² and Slab M² are all ZERO: {missingAllCount}");
            HousekeepingNode.Nodes.Add(alertNode);
            alertNode = new TreeNode($"Number of completed jobs where SUPPLIERS are Missing: {missingSuppCount}");
            HousekeepingNode.Nodes.Add(alertNode);
            alertNode = new TreeNode($"Number of WHITEBOARD jobs with missing Products: {missingProducts}");
            HousekeepingNode.Nodes.Add(alertNode);

            int onStopCount = 0;
            if(dt == null)
            {
                OnStopCustomersNode.Nodes.Add("N/A");
                bulletinTreeView.ExpandAll();
                this.Cursor = Cursors.Default;
                return;
            }
            else
            {
                if(dt.Rows.Count == 0)
                {
                    OnStopCustomersNode.Nodes.Add("N/A");
                    bulletinTreeView.ExpandAll();
                    this.Cursor = Cursors.Default;
                    return;
                }
            }
            string custLine = "";
            string userName = "";
            foreach (DataRow dr in dt.Rows)
            {
                userName = mcData.GetUserFullNameFromUserID(dr["onStopUser"].ToString());
                custLine = $"[{dr["custCode"].ToString()} - {dr["custName"].ToString()}] was put ON STOP at {dr["onStopDateTime"].ToString()} by {userName} ";
                customerNode = new TreeNode(custLine);
                OnStopCustomersNode.Nodes.Add(customerNode);
                onStopCount++;

            }
            bulletinTreeView.ExpandAll();
            this.Cursor = Cursors.Default;
            return;
        }

        private void menuTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //   bool nodeClicked = false;
            //testExcelNode

            //if (menuTreeView.SelectedNode == testExcelNode)
            //{
            //    excelTestForm frm = new excelTestForm();
            //    frm.ShowDialog();
            //    this.menuTreeView.SelectedNode = null;
            //    return;

            //}

            //WhiteboardEmptyProductsForm
            if (menuTreeView.SelectedNode == wbEmptyProductsNode)
            {
                WhiteboardEmptyProductsForm wbForm = new WhiteboardEmptyProductsForm();
                wbForm.ShowDialog();
                //    nodeClicked = true;
                this.menuTreeView.SelectedNode = null;
                return;
            }

            if (menuTreeView.SelectedNode == updateBeamJobsQtyNode)
            {
                JobsMissingDataForm jobForm = new JobsMissingDataForm("BEAMZERO");
                jobForm.ShowDialog();
                //    nodeClicked = true;
                this.menuTreeView.SelectedNode = null;
                return;
            }

            if (menuTreeView.SelectedNode == updateSlabJobsQtyNode)
            {
                JobsMissingDataForm jobForm = new JobsMissingDataForm("SLABZERO");
                jobForm.ShowDialog();
                //    nodeClicked = true;
                this.menuTreeView.SelectedNode = null;
                return;
            }

            if (menuTreeView.SelectedNode == updateNonSpecifiedJobsQtyNode)
            {
                JobsMissingDataForm jobForm = new JobsMissingDataForm("ALLZERO");
                jobForm.ShowDialog();
                //    nodeClicked = true;
                this.menuTreeView.SelectedNode = null;
                return;
            }

            if (menuTreeView.SelectedNode == missingSuppliersNode)
            {
                JobsMissingDataForm jobForm = new JobsMissingDataForm("MISSINGSUPPLIER");
                jobForm.ShowDialog();
                //    nodeClicked = true;
                this.menuTreeView.SelectedNode = null;
                return;
            }

            if (menuTreeView.SelectedNode == supplierRptsNode)
            {
                SuppliersReportForm suppRptForm = new SuppliersReportForm();
                suppRptForm.ShowDialog();
                this.menuTreeView.SelectedNode = null;
                return;

            }

            if (menuTreeView.SelectedNode == beamLmRptNode)
            {
                JobPlannerRptNewForm jpRptForm = new JobPlannerRptNewForm(beamLmRptNode.Text,"BeamLM");
                jpRptForm.ShowDialog();
                this.menuTreeView.SelectedNode = null;
                return;
            }

            if (menuTreeView.SelectedNode == beamM2RptNode)
            {
                JobPlannerRptNewForm jpRptForm = new JobPlannerRptNewForm(beamM2RptNode.Text, "BeamM2");
                jpRptForm.ShowDialog();
                this.menuTreeView.SelectedNode = null;
                return;
            }

            if (menuTreeView.SelectedNode == slabM2RptNode)
            {
                JobPlannerRptNewForm jpRptForm = new JobPlannerRptNewForm(slabM2RptNode.Text, "SlabM2");
                jpRptForm.ShowDialog();
                this.menuTreeView.SelectedNode = null;
                return;
            }

            //lockedJobsNode
            if (menuTreeView.SelectedNode == lockedJobsNode)
            {
                if (!mcData.IsJobLockExist())
                {
                    MessageBox.Show("No LOCKED jobs currently exist");
                    return;
                }
                LockedJobsForm ljForm = new LockedJobsForm();
                ljForm.ShowDialog();
                this.menuTreeView.SelectedNode = null;
                return;

            }

            if (menuTreeView.SelectedNode == cancelledJobsNode)
            {
                if (!mcData.AnyCancelledJobsExist())
                {
                    MessageBox.Show("No CANCELLED jobs currently exist");
                    return;
                }

                CancelledJobsForm cancelForm = new CancelledJobsForm();
                cancelForm.ShowDialog();
                this.menuTreeView.SelectedNode = null;
                return;

            }

            if (menuTreeView.SelectedNode == deletedJobsNode)
            {
                if (!mcData.AnyJobsDeleted())
                {
                    MessageBox.Show("Currently there No DELETED jobs");
                    return;
                }

                DeletedJobsForm deleteForm = new DeletedJobsForm();
                deleteForm.ShowDialog();
                this.menuTreeView.SelectedNode = null;
                return;

            }


            if (menuTreeView.SelectedNode == notOnShopRptsNode)
            {
                NotOnShopRptsForm nosRptForm = new NotOnShopRptsForm();
                nosRptForm.ShowDialog();
                this.menuTreeView.SelectedNode = null;
                return;

            }


            if (menuTreeView.SelectedNode == quitParentNode)
            {
                // this.Close();
                DialogResult result = MessageBox.Show("Exit the Melton Concrete Portal ?", "Leave Melton Concrete Portal", MessageBoxButtons.YesNo);
                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    //this.Dispose();
                    Application.Exit();
                }
                else
                {
                    return;
                }
                //nodeClicked = true;
                //return;
            }

            if (menuTreeView.SelectedNode == userMaintenanceNode)
            {
                UserMaintenanceForm userForm = new UserMaintenanceForm();
                userForm.ShowDialog();
                //  nodeClicked = true;
                this.menuTreeView.SelectedNode = null;
                return;
            }

            if (menuTreeView.SelectedNode == customersNode)
            {
                CustomersOverviewForm custForm = new CustomersOverviewForm();
                custForm.ShowDialog();
                //   nodeClicked = true;
                this.menuTreeView.SelectedNode = null;
                return;
            }

            if (menuTreeView.SelectedNode == suppliersNode)
            {
                SuppliersOverviewForm suppForm = new SuppliersOverviewForm();
                suppForm.ShowDialog();
                //    nodeClicked = true;
                this.menuTreeView.SelectedNode = null;
                return;
            }

            if (menuTreeView.SelectedNode == newParentJobNode)
            {
                CreateParentJobForm jobForm = new CreateParentJobForm();
                jobForm.ShowDialog();
                //    nodeClicked = true;
                this.menuTreeView.SelectedNode = null;
                return;
            }

            if (menuTreeView.SelectedNode == searchParentJobNode)
            {
                SearchJobForm jobForm = new SearchJobForm();
                jobForm.ShowDialog();
                //    nodeClicked = true;
                this.menuTreeView.SelectedNode = null;
                return;
            }

            if (menuTreeView.SelectedNode == jobPlannerNode)
            {
                JobPlannerForm jobForm = new JobPlannerForm("ALL");

                jobForm.ShowDialog();
                //    nodeClicked = true;
                this.menuTreeView.SelectedNode = null;
                return;
            }

            if (menuTreeView.SelectedNode == jobPlannerBeamNode)
            {
                JobPlannerForm jobForm = new JobPlannerForm("BEAM");

                jobForm.ShowDialog();
                //    nodeClicked = true;
                this.menuTreeView.SelectedNode = null;
                return;
            }

            if (menuTreeView.SelectedNode == jobPlannerSlabNode)
            {
                JobPlannerForm jobForm = new JobPlannerForm("SLAB");

                jobForm.ShowDialog();
                //    nodeClicked = true;
                this.menuTreeView.SelectedNode = null;
                return;
            }

            if (menuTreeView.SelectedNode == whiteboardNode)
            {
                WhiteboardParametersForm wbForm = new WhiteboardParametersForm();
                wbForm.ShowDialog();
                //    nodeClicked = true;
                this.menuTreeView.SelectedNode = null;
                return;
            }

            if (menuTreeView.SelectedNode == whiteboardJobExtensionsNode)
            {
                WhiteboardJobExtensionsForm wbExtForm = new WhiteboardJobExtensionsForm();
                wbExtForm.ShowDialog();
                this.menuTreeView.SelectedNode = null;
                return;
            }

            if (menuTreeView.SelectedNode == errorAuditNode)
            {
                ErrorAuditForm errAuditForm = new ErrorAuditForm();
                errAuditForm.ShowDialog();
                //    nodeClicked = true;
                this.menuTreeView.SelectedNode = null;
                return;
            }
        }

        private void UserOverviewForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //mcpUserID
            mcData.UpdateUserLastLoggedOn(mcpUserID);

        }

        private void menuTreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            actionListClicked = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadBulletinBoard();
        }

        
    }
}
