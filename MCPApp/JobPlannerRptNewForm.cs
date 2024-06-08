using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace MCPApp
{
    public partial class JobPlannerRptNewForm : Form
    {
        private string _rptTitle = "";
        private string _rptMode = "";
        private DateTime _startDate = DateTime.MinValue;
        private DateTime _endDate = DateTime.MaxValue;
        private DataTable _rptDT = new DataTable();
        MeltonData mcData = new MeltonData();
        ExcelUtlity excel = new ExcelUtlity();

        public JobPlannerRptNewForm()
        {
            InitializeComponent();
        }

        public JobPlannerRptNewForm(string rptTitle, string rptMode)
        {
            _rptTitle = rptTitle;
            _rptMode = rptMode;
            InitializeComponent();
        }

        private void JobPlannerRptNewForm_Load(object sender, EventArgs e)
        {
            this.Text = $"Generate Report - {_rptTitle}";
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        private void BuildDataTable()
        {
            string colName = "";
            switch (_rptMode)
            {
                case "BeamLM":
                    colName = "beamLM";
                    break;
                case "BeamM2":
                    colName = "beamM2";
                    break;
                case "SlabM2":
                    colName = "slabM2";
                    break;
                default:
                    break;
            }

            _rptDT = new DataTable();

            _rptDT.Columns.Add("jobNo", typeof(string));
            _rptDT.Columns.Add("floorLevel", typeof(string));
            _rptDT.Columns.Add("requiredDate", typeof(DateTime));
            _rptDT.Columns.Add("custName", typeof(string));
            _rptDT.Columns.Add(colName, typeof(int));
            _rptDT.Columns.Add("supplyType", typeof(string));
            _rptDT.Columns.Add("productSupplier", typeof(string));
        }
        
        private void FillBeamLMRpt()
        {
            this.Cursor = Cursors.WaitCursor;
            DataTable dt = mcData.GetBeamLmJobPlannerDT(_startDate, _endDate);

            DataTable newDT = new DataTable();
            string jobNo = "";
            string custName = "";
            string custCode = "";
            string filename = "BeamLM_" + DateTime.Now.ToString("ddMMMyyyyhhmmss") + ".xlsx";
            string fullFilePath = System.IO.Path.Combine(pathTextBox.Text, filename);
            DateTime reqDate;
            Double dateValue;

            newDT.Columns.Clear();
            newDT.Columns.Add("JobNo", typeof(string));
            newDT.Columns.Add("FloorLevel", typeof(string));
            newDT.Columns.Add("RequiredDate", typeof(Double));
            newDT.Columns.Add("Customer", typeof(string));
            newDT.Columns.Add("BeamLM", typeof(int));
            newDT.Columns.Add("SupplyType", typeof(string));
            newDT.Columns.Add("ProductSupplier", typeof(string));

            string currSupplier = "";
            string nextSupplier = "";
            int numRows = 0;
            
            foreach (DataRow row in dt.Rows)
            {
                nextSupplier = row["productSupplier"].ToString();
                DataRow dr = newDT.NewRow();
                if ((!String.IsNullOrWhiteSpace(currSupplier) || String.IsNullOrWhiteSpace(nextSupplier)) && nextSupplier != currSupplier)
                {
                    var subTotal = mcData.GetTotalLMBySupplier(currSupplier, _startDate, _endDate);
                    dr["JobNo"] = "";
                    dr["FloorLevel"] = "";
                    dr["RequiredDate"] = 0;
                    dr["Customer"] = "TOTAL:";
                    dr["BeamLM"] = subTotal;
                    dr["SupplyType"] = "";
                    dr["ProductSupplier"] = currSupplier;
                    newDT.Rows.Add(dr);
                    dr = newDT.NewRow();
                }
               // DataRow dr = newDT.NewRow();
                jobNo = row["jobNo"].ToString();
                custCode = mcData.GetCustomerCodeByJobNo(jobNo);
                custName = mcData.GetCustName(custCode);
                reqDate = Convert.ToDateTime(row["requiredDate"]);
                dateValue = reqDate.ToOADate();
                dr["JobNo"] = jobNo;
                dr["FloorLevel"] = row["floorLevel"].ToString();
                dr["RequiredDate"] = dateValue;
                dr["Customer"] = custName;
                dr["BeamLM"] = row["beamLM"];
                dr["SupplyType"] = row["supplyType"].ToString();
                dr["ProductSupplier"] = row["productSupplier"].ToString();
                
                currSupplier = row["productSupplier"].ToString();
                newDT.Rows.Add(dr);
                numRows++;
            }
            if(numRows > 0)
            {
                DataRow dr = newDT.NewRow();
                var subTotal = mcData.GetTotalLMBySupplier(currSupplier, _startDate, _endDate);
                dr["JobNo"] = "";
                dr["FloorLevel"] = "";
                dr["RequiredDate"] = 0;
                dr["Customer"] = "TOTAL:";
                dr["BeamLM"] = subTotal;
                dr["SupplyType"] = "";
                dr["ProductSupplier"] = currSupplier;
                newDT.Rows.Add(dr);
            }
          //  dataGridView1.DataSource = newDT;
            excel.WriteJobPlannerRptDataTableToExcel(newDT, "Beam LM", fullFilePath, "Beam LM", 3);
            this.Cursor = Cursors.Default;
            Process.Start(fullFilePath);
        }

        private void FillBeamM2Rpt()
        {

        }

        private void FillSlabM2Rpt()
        {

        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        private void GenerateButton_Click(object sender, EventArgs e)
        {
            BuildDataTable();
            _startDate = dateTimePicker1.Value.Date;
            _endDate = dateTimePicker2.Value.Date.AddDays(1).AddSeconds(-1);
            switch (_rptMode)
            {
                case "BeamLM":
                    FillBeamLMRpt();
                    break;
                case "BeamM2":
                    FillBeamM2Rpt();
                    break;
                case "SlabM2":
                    FillSlabM2Rpt();
                    break;
                default:
                    break;
            }
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
    }
}
