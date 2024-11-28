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
    public partial class MyBoardReportForm : Form
    {
        MeltonData mcData = new MeltonData();
        ExcelUtlity excel = new ExcelUtlity();
        

        private DateTime wcDate;
        public DateTime WcDate
        {
            get
            {
                return wcDate;
            }
            set
            {
                wcDate = value;
            }
        }

        private static string _source = "";


        public MyBoardReportForm()
        {
            InitializeComponent();
        }

        public MyBoardReportForm(string source,DateTime weekCommenceDate)
        {
            InitializeComponent();
            wcDate = weekCommenceDate;
            _source = source;
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

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        private void WhiteboardRptForm_Load(object sender, EventArgs e)
        {
            string prefix = _source == "WB" ? "WHITEBOARD" : "DESIGNBOARD";
            this.Text = $"{prefix}  for week commencing " + wcDate.ToString("ddMMMyyyy"); 
            GenerateButton.Text = _source == "WB" ? "Generate WHITEBOARD Rpt" : "Generate DESIGNBOARD Rpt";
            label2.Text = "";
        }

        private void GenerateWhiteboardExcelReport(DataTable sourceDT, DateTime wcDate)
        {

            DataTable wbDT = new DataTable();
            wbDT.Columns.Clear();
            wbDT.Columns.Add("jobNo", typeof(string));
            wbDT.Columns.Add("requiredDate", typeof(Double));
            wbDT.Columns.Add("custName", typeof(string));
            wbDT.Columns.Add("siteAddress", typeof(string));
            // wbDT.Columns.Add("customerCode", typeof(string));
            wbDT.Columns.Add("supplyType", typeof(string));
            wbDT.Columns.Add("products", typeof(string));
            wbDT.Columns.Add("totalM2", typeof(int));
            wbDT.Columns.Add("fixingDaysAllowance", typeof(int));
            wbDT.Columns.Add("salesPrice", typeof(decimal));
            wbDT.Columns.Add("isInvoiced", typeof(string));
            wbDT.Columns.Add("suppShortname", typeof(string));
            wbDT.Columns.Add("stairsIncl", typeof(string));
            wbDT.Columns.Add("stairsSupplier", typeof(string));
            wbDT.Columns.Add("floorlevel", typeof(string));
            wbDT.Columns.Add("continuedFlag", typeof(string));
            wbDT.Columns.Add("cowSentFlag", typeof(string));
            wbDT.Columns.Add("cowReceived", typeof(string));
            wbDT.Columns.Add("wcMonday", typeof(string));
            wbDT.Columns.Add("wcTuesday", typeof(string));
            wbDT.Columns.Add("wcWednesday", typeof(string));
            wbDT.Columns.Add("wcThursday", typeof(string));
            wbDT.Columns.Add("wcFriday", typeof(string));
            wbDT.Columns.Add("wcSaturday", typeof(string));
            wbDT.Columns.Add("wcSunday", typeof(string));
            wbDT.Columns.Add("contracts", typeof(string));
            wbDT.Columns.Add("ramsFlag", typeof(string));
            wbDT.Columns.Add("ramsSignedReturnedFlag", typeof(string));
            wbDT.Columns.Add("ramsCompleteReturnedFlag", typeof(string));
            wbDT.Columns.Add("lorry", typeof(string));
            wbDT.Columns.Add("craneSize", typeof(string));
            wbDT.Columns.Add("craneSupplier", typeof(string));
            wbDT.Columns.Add("spreaderMatFlag", typeof(string));
            wbDT.Columns.Add("hireContractRcvd", typeof(string));
            wbDT.Columns.Add("fallArrest", typeof(string));
            wbDT.Columns.Add("fixingGang", typeof(string));
            wbDT.Columns.Add("onHireFlag", typeof(string));
            wbDT.Columns.Add("extrasFlag", typeof(string));
            wbDT.Columns.Add("concrete", typeof(string));
            wbDT.Columns.Add("blocks", typeof(string));
            wbDT.Columns.Add("drawingsEmailedFlag", typeof(string));
            wbDT.Columns.Add("draughtsman", typeof(string));
            wbDT.Columns.Add("salesman", typeof(string));
            wbDT.Columns.Add("lastComment", typeof(string));
            wbDT.Columns.Add("dateCreated", typeof(string));
            wbDT.Columns.Add("jobCreatedBy", typeof(string));

            string dateCreated = "";
            string custName = "";
            DateTime reqDate;
            Double dateValue;
            string jobNo = "";
            string rptName = "WB_wc " + wcDate.ToString("ddMMMyyyy");
            string fullRptName = rptName + "_" + DateTime.Now.ToString("ddMMMyyyyhhmmss") + ".xlsx";
            string fullFilePath = System.IO.Path.Combine(pathTextBox.Text, fullRptName);
            foreach (DataRow row in sourceDT.Rows)
            {
                DataRow dr = wbDT.NewRow();
                jobNo = row["jobNo"].ToString();
                if (!mcData.IsValidWhiteboardJob(jobNo)) { continue; }
                dr["jobNo"] = jobNo;
                dateCreated = mcData.GetJobCreatedDate(jobNo).ToString("dd/MMM/yyyy hh:mm tt");
                reqDate = Convert.ToDateTime(row["requiredDate"]);
                dateValue = reqDate.ToOADate();
                dr["requiredDate"] = dateValue;
                custName = mcData.GetCustName(row["custCode"].ToString());
                dr["custName"] = custName;

                dr["siteAddress"] = row["siteAddress"].ToString();
                //   dr["customerCode"] = row["customerCode"].ToString();
                dr["supplyType"] = row["supplyType"].ToString();
                dr["products"] = row["products"].ToString();
                dr["totalM2"] = Convert.ToInt16(row["totalM2"].ToString());
                dr["fixingDaysAllowance"] = Convert.ToInt16(row["fixingDaysAllowance"].ToString());
                dr["salesPrice"] = Convert.ToDecimal(row["salesPrice"].ToString());
                dr["isInvoiced"] = row["isInvoiced"].ToString();
                dr["suppShortname"] = row["suppShortname"].ToString();
                dr["stairsIncl"] = row["stairsIncl"].ToString();
                dr["stairsSupplier"] = row["stairsSupplier"].ToString();
                dr["floorlevel"] = row["floorlevel"].ToString();
                dr["continuedFlag"] = row["continuedFlag"].ToString();
                dr["cowSentFlag"] = row["cowSentFlag"].ToString();
                dr["cowReceived"] = row["cowReceived"].ToString();
                dr["wcMonday"] = row["wcMonday"].ToString();
                dr["wcTuesday"] = row["wcTuesday"].ToString();
                dr["wcWednesday"] = row["wcWednesday"].ToString();
                dr["wcThursday"] = row["wcThursday"].ToString();
                dr["wcFriday"] = row["wcFriday"].ToString();
                dr["wcSaturday"] = row["wcSaturday"].ToString();
                dr["wcSunday"] = row["wcSunday"].ToString();
                dr["contracts"] = row["contracts"].ToString();
                dr["ramsFlag"] = row["ramsFlag"].ToString();
                dr["ramsSignedReturnedFlag"] = row["ramsSignedReturnedFlag"].ToString();
                dr["ramsCompleteReturnedFlag"] = row["ramsCompleteReturnedFlag"].ToString();
                dr["craneSize"] = row["craneSize"].ToString();
                dr["craneSupplier"] = row["craneSupplier"].ToString();
                dr["spreaderMatFlag"] = row["spreaderMatFlag"].ToString();
                dr["hireContractRcvd"] = row["hireContractRcvd"].ToString();
                dr["fallArrest"] = row["fallArrest"].ToString();
                dr["fixingGang"] = row["fixingGang"].ToString();
                dr["onHireFlag"] = row["onHireFlag"].ToString();
                dr["extrasFlag"] = row["extrasFlag"].ToString();
                dr["concrete"] = row["concrete"].ToString();
                dr["blocks"] = row["blocks"].ToString();
                dr["drawingsEmailedFlag"] = row["drawingsEmailedFlag"].ToString();
                dr["draughtsman"] = row["draughtsman"].ToString();
                dr["salesman"] = row["salesman"].ToString();
                dr["lastComment"] = row["lastComment"].ToString();
                dr["dateCreated"] = dateCreated;
                dr["jobCreatedBy"] = row["jobCreatedBy"].ToString();
                wbDT.Rows.Add(dr);
                label2.Text = jobNo;
            }
            excel.WriteDataTableToExcel(wbDT, rptName, fullFilePath, rptName,2);

        }

        private void GenerateDesignBoardExcelReport(DataTable sourceDT, DateTime wcDate)
        {

            DataTable dbDT = new DataTable();
            dbDT.Columns.Clear();
            dbDT.Columns.Add("jobNo", typeof(string));
            dbDT.Columns.Add("designDate", typeof(Double));
            dbDT.Columns.Add("detailingDays", typeof(int));
            dbDT.Columns.Add("designStatus", typeof(string));
            dbDT.Columns.Add("daysUnapproved", typeof(int));
            dbDT.Columns.Add("dman", typeof(string));
            dbDT.Columns.Add("salesman", typeof(string));
            dbDT.Columns.Add("requiredDate", typeof(Double));
            dbDT.Columns.Add("customer", typeof(string));
            dbDT.Columns.Add("siteAddress", typeof(string));
            dbDT.Columns.Add("floorlevel", typeof(string));
            dbDT.Columns.Add("supplyType", typeof(string));
            dbDT.Columns.Add("productSupplier", typeof(string));
            dbDT.Columns.Add("slabM2", typeof(int));
            dbDT.Columns.Add("beamM2", typeof(int));
            dbDT.Columns.Add("beamLM", typeof(int));
            dbDT.Columns.Add("wcMonday", typeof(string));
            dbDT.Columns.Add("wcTuesday", typeof(string));
            dbDT.Columns.Add("wcWednesday", typeof(string));
            dbDT.Columns.Add("wcThursday", typeof(string));
            dbDT.Columns.Add("wcFriday", typeof(string));
            dbDT.Columns.Add("additionalNotes", typeof(string));
            


            string dateCreated = "";
            string custName = "";
            string siteAddress = "";
            DateTime designDate;
            Double designDateValue;
            DateTime reqDate;
            Double reqDateValue;
            string jobNo = "";
            string rptName = "DB_wc " + wcDate.ToString("ddMMMyyyy");
            string fullRptName = rptName + "_" + DateTime.Now.ToString("ddMMMyyyyhhmmss") + ".xlsx";
            string fullFilePath = System.IO.Path.Combine(pathTextBox.Text, fullRptName);
            int daysDiff = 0;
            DateTime dateJobCreated;
            foreach (DataRow row in sourceDT.Rows)
            {
                DataRow dr = dbDT.NewRow();
                jobNo = row["jobNo"].ToString();
                if (!mcData.IsValidDesignBoardJob(jobNo)) { continue; }
                dr["jobNo"] = jobNo;
                dateCreated = mcData.GetJobCreatedDate(jobNo).ToString("dd/MMM/yyyy hh:mm tt");
                designDate = Convert.ToDateTime(row["designDate"]);
                designDateValue = designDate.ToOADate();
                reqDate = Convert.ToDateTime(row["requiredDate"]);
                reqDateValue = reqDate.ToOADate();
                dr["designDate"] = designDateValue;
                dr["requiredDate"] = reqDateValue;
                dr["detailingDays"] = row["detailingDays"];//== null ? 0 : Convert.ToInt16(row["detailingDays"]);
                dr["designStatus"] = row["designStatus"].ToString();
                if (row["designStatus"].ToString().ToUpper().Contains("APPROVED") || row["designStatus"].ToString().ToUpper() == "ON SHOP")
                {
                    daysDiff = 0;
                }
                else
                {
                    dateJobCreated = mcData.GetJobCreatedDateByJobNo(jobNo);
                    daysDiff = mcData.GetDaysDiffBetweenTwDates(dateJobCreated.Date);
                }
                dr["daysUnapproved"] = daysDiff;
                dr["dman"] = row["dman"].ToString();
                dr["salesman"] = row["salesman"].ToString();
                
                custName = mcData.GetCustomerNameByJobNo(jobNo);
                dr["customer"] = custName;
                siteAddress = mcData.GetSiteAddressFromJobNo(jobNo);
                dr["siteAddress"] = siteAddress;
                dr["floorlevel"] = row["floorlevel"].ToString();
                dr["supplyType"] = row["supplyType"].ToString();
                dr["productSupplier"] = row["productSupplier"].ToString();
                dr["slabM2"] = Convert.ToInt32(row["slabM2"].ToString());
                dr["beamM2"] = Convert.ToInt32(row["beamM2"].ToString());
                dr["beamLM"] = Convert.ToInt32(row["beamLM"].ToString());
                dr["wcMonday"] = row["wcMonday"].ToString();
                dr["wcTuesday"] = row["wcTuesday"].ToString();
                dr["wcWednesday"] = row["wcWednesday"].ToString();
                dr["wcThursday"] = row["wcThursday"].ToString();
                dr["wcFriday"] = row["wcFriday"].ToString();
                dr["additionalNotes"] = row["additionalNotes"].ToString();


                dbDT.Rows.Add(dr);
                label2.Text = jobNo;
            }
            excel.WriteDataTableToExcel(dbDT, rptName, fullFilePath, rptName, 2,8);

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
            this.Cursor = Cursors.WaitCursor;
            DataTable rptDT = new DataTable();
            if(_source == "WB")
            {
                rptDT = mcData.GetWhiteboardByDateRangeDT(wcDate, wcDate.AddDays(6));
                GenerateWhiteboardExcelReport(rptDT, wcDate);
            }
            else if( _source == "DB")
            {
                rptDT = mcData.GetDesignboardByDateRangeDT(wcDate, wcDate.AddDays(6));
                GenerateDesignBoardExcelReport(rptDT, WcDate);
            }
            else
            {
               //do nothing
            }
            this.Cursor = Cursors.Default;

            string message = "Report(s) successfully created in [" + pathTextBox.Text + "] location." + Environment.NewLine + "" + Environment.NewLine + "Do you wish to check the excel file(s) now?";
            if (MessageBox.Show(message, "Open Excel Files ? ", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                OpenLocationFolder();
            }
        }
    }
}
