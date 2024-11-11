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
    public partial class NotOnShopRptsForm : Form
    {
        MeltonData mcData = new MeltonData();
        ExcelUtlity excel = new ExcelUtlity();

        public NotOnShopRptsForm()
        {
            InitializeComponent();
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        private void NotOnShopRptsForm_Load(object sender, EventArgs e)
        {
            this.Text = "[NOT ON SHOP] reports - " + DateTime.Now.ToShortDateString();
            label2.Text = "";
        }

        private void SelectBFolderButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            folderDlg.ShowNewFolderButton = true;
            // Show the FolderBrowserDialog.  
            DialogResult result = folderDlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                pathTextBox.Text = folderDlg.SelectedPath;
                Environment.SpecialFolder root = folderDlg.RootFolder;
            }  
        }

        private void GenerateReport(string path, string rptName)
        {
            this.Cursor = Cursors.WaitCursor;
            DataTable sourceDT = mcData.GetJobsNotOnShopDT();

            DataTable jobsDT = new DataTable();
            string jobNo = "";
            string custName = "";
            string custCode = "";
            string siteAddr = "";
            string filename = "NOTONSHOP" + DateTime.Now.ToString("ddMMMyyyyhhmmss") + ".xlsx";
            string fullFilePath = System.IO.Path.Combine(pathTextBox.Text, filename);
            DateTime designDate;
            DateTime reqDate;
            Double designDateValue;
            Double requiredDateValue;
            string designStatus = "";

            jobsDT.Columns.Clear();
            jobsDT.Columns.Add("JobNo", typeof(string));
            jobsDT.Columns.Add("Customer", typeof(string));
            jobsDT.Columns.Add("FloorLevel", typeof(string));
            jobsDT.Columns.Add("DesignDate", typeof(Double)); // set to double because of Excel 
            jobsDT.Columns.Add("RequiredDate", typeof(Double)); // set to double because of Excel 
            jobsDT.Columns.Add("DesignStatus", typeof(string));
            jobsDT.Columns.Add("SiteAddress", typeof(string));
            jobsDT.Columns.Add("SupplyType", typeof(string));
            jobsDT.Columns.Add("SupplierRef", typeof(string));

            foreach (DataRow row in sourceDT.Rows)
            {
                DataRow dr = jobsDT.NewRow();
                jobNo = row["jobNo"].ToString();
                custCode = mcData.GetCustomerCodeByJobNo(jobNo);
                custName = mcData.GetCustName(custCode);
                siteAddr = mcData.GetSiteAddressFromJobNo(jobNo);
                designDate = Convert.ToDateTime(row["designDate"]);
                designDateValue = designDate.ToOADate();
                reqDate = Convert.ToDateTime(row["requiredDate"]);
                requiredDateValue = reqDate.ToOADate();
                designStatus = row["designStatus"].ToString();
                dr["JobNo"] = jobNo;
                dr["Customer"] = custName;
                dr["FloorLevel"] = row["floorLevel"].ToString();
                dr["DesignDate"] = designDateValue;
                dr["RequiredDate"] = requiredDateValue;
                dr["DesignStatus"] = designStatus;
                dr["SiteAddress"] = siteAddr;
                dr["SupplyType"] = row["supplyType"].ToString();
                dr["SupplierRef"] = row["supplierRef"].ToString();
                jobsDT.Rows.Add(dr);
                label2.Text = jobNo;
            }
            excel.WriteDataTableToExcel(jobsDT, rptName, fullFilePath, rptName,4);
            this.Cursor = Cursors.Default;
            string message = "Report(s) successfully created in [" + pathTextBox.Text + "] location." + Environment.NewLine + "" + Environment.NewLine + "Do you wish to check the excel file(s) now?";
            if (MessageBox.Show(message, "Open Excel Files ? ", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                System.Diagnostics.Process.Start(fullFilePath);
               // OpenLocationFolder();
            }
        }

        private void OpenLocationFolder()
        {
            try
            {
                System.Diagnostics.Process.Start("explorer.exe", (String)pathTextBox.Text);
            }
            catch (Win32Exception win32Exception)
            {
                //The system cannot find the file specified...
                Console.WriteLine(String.Format("OpenLocationFolder() ERROR - {0}", win32Exception.Message));
            }
        }

        private void GenerateButton_Click(object sender, EventArgs e)
        {
            if (!System.IO.Directory.Exists(pathTextBox.Text))
            {
                MessageBox.Show(String.Format("[{0}] path does NOT exist", pathTextBox.Text));
                return;
            }

            GenerateReport(pathTextBox.Text, "JOBS NOT ON SHOP");

           

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pathTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        
    }
}
