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
        ExcelUtlity excelUtil = new ExcelUtlity();
        private string filename = "";
        private string fullFilePath = "";
        private object missing = Type.Missing;

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

        private void GenerateRpt(string rptMode)
        {
            filename = $"{rptMode}_by_Supplier_{DateTime.Now.ToString("ddMMMyyyyhhmmss")}.xlsx";
            fullFilePath = System.IO.Path.Combine(pathTextBox.Text, filename);
            groupBox3.Text = "Report Progress - (Step 1 of 10) CREATE EXCEL FILE";
            Microsoft.Office.Interop.Excel.Application oXL = new Microsoft.Office.Interop.Excel.Application();
            oXL.Visible = false;
            this.Cursor = Cursors.WaitCursor;
            string colName = "";
            switch (rptMode)
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
            Microsoft.Office.Interop.Excel.Workbook oWB = oXL.Workbooks.Add(missing);
            DataTable yearsDT = mcData.GetYearsDT(rptMode,_startDate, _endDate);
            List<int> rptYears = new List<int>();
            foreach (DataRow yearDR in yearsDT.Rows)
            {
                rptYears.Add(Convert.ToInt32(yearDR["year"]));
            }
            int year = DateTime.Now.Year;
            groupBox3.Text = "Report Progress - (Step 1 of 10) CREATING DATA";
            for (int j = 0; j < rptYears.Count; j++)
            {
                Microsoft.Office.Interop.Excel.Worksheet sheetCurrent = oXL.Worksheets.Add();
                sheetCurrent.Name = rptYears[j].ToString();
                year = rptYears[j];
                DataTable dt = mcData.GetJobPlannerDT(rptMode, _startDate, _endDate, year);
                DataTable newDT = new DataTable();
                string jobNo = "";
                string custName = "";
                string custCode = "";

                DateTime reqDate;
                Double dateValue;

                newDT.Columns.Clear();
                newDT.Columns.Add("JobNo", typeof(string));
                newDT.Columns.Add("FloorLevel", typeof(string));
                newDT.Columns.Add("RequiredDate", typeof(Double));
                newDT.Columns.Add("Customer", typeof(string));
                newDT.Columns.Add(rptMode, typeof(int));
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
                        var subTotal = mcData.GetTotalBySupplier(rptMode, currSupplier, _startDate, _endDate, year);
                        dr["JobNo"] = "";
                        dr["FloorLevel"] = "";
                        dr["RequiredDate"] = 0;
                        dr["Customer"] = "TOTAL:";
                        dr[rptMode] = subTotal;
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
                    dr[rptMode] = row[colName];
                    dr["SupplyType"] = row["supplyType"].ToString();
                    dr["ProductSupplier"] = row["productSupplier"].ToString();
                    lblYear.Text = year.ToString();
                    lblJobNo.Text = jobNo;
                    lblDate.Text = reqDate.ToString("ddd, dd MMM yyyy");
                    currSupplier = row["productSupplier"].ToString();
                    newDT.Rows.Add(dr);

                    numRows++;
                }
                if (numRows > 0)
                {
                    DataRow dr = newDT.NewRow();
                    var subTotal = mcData.GetTotalBySupplier(rptMode, currSupplier, _startDate, _endDate, year);
                    dr["JobNo"] = "";
                    dr["FloorLevel"] = "";
                    dr["RequiredDate"] = 0;
                    dr["Customer"] = "TOTAL:";
                    dr[rptMode] = subTotal;
                    dr["SupplyType"] = "";
                    dr["ProductSupplier"] = currSupplier;
                    newDT.Rows.Add(dr);
                }

                sheetCurrent.Name = year.ToString();
                sheetCurrent.Cells[1, 1] = $"{rptMode} by Supplier Rpt";
                sheetCurrent.Cells[1, 2] = "Ran : " + DateTime.Now.ToString("ddd,dd MMM yyyy");
                int rowcount = 2;
                groupBox3.Text = "Report Progress - (Step 1 of 10) GENERATING EXCEL DATA";
                foreach (DataRow datarow in newDT.Rows)
                {
                    lblYear.Text = year.ToString();
                    lblJobNo.Text = datarow["JobNo"].ToString();
                    lblDate.Text = DateTime.FromOADate((double)datarow["requiredDate"]).ToString("ddd, dd MMM yyyy");
                    rowcount += 1;

                    for (int i = 1; i <= newDT.Columns.Count; i++)
                    {
                        sheetCurrent.Cells[1, i].EntireRow.Font.Bold = true;
                        sheetCurrent.Cells[2, i].EntireRow.Font.Bold = true;
                        sheetCurrent.Cells[1, i].EntireRow.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);
                        sheetCurrent.Cells[2, i].EntireRow.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);
                        sheetCurrent.Cells[1, i].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Blue);
                        sheetCurrent.Cells[2, i].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Blue);
                        if (rowcount == 3)
                        {
                            sheetCurrent.Cells[2, i] = newDT.Columns[i - 1].ColumnName;
                            sheetCurrent.Cells.Font.Color = System.Drawing.Color.Black;

                        }

                        sheetCurrent.Cells[rowcount, i] = datarow[i - 1].ToString();

                    }
                    if (sheetCurrent.Cells[rowcount, 4].Value == "TOTAL:")
                    {
                        sheetCurrent.Cells[rowcount, 4].EntireRow.Font.Bold = true;
                        sheetCurrent.Cells[rowcount, 3].Value = "";
                        sheetCurrent.Cells[rowcount, 4].Style.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                    }
                    sheetCurrent.Columns[3].NumberFormat = "DD/MM/YYYY";
                    sheetCurrent.Columns[5].NumberFormat = "#,##";
                }
                
                sheetCurrent.Application.ActiveSheet.Rows[3].Select();
                sheetCurrent.Application.ActiveWindow.FreezePanes = true;

                sheetCurrent.Cells[2, 9].Value = "YEAR";
                sheetCurrent.Cells[2, 10].Value = "SUPPLIER";
                sheetCurrent.Cells[2, 11].Value = rptMode;
                sheetCurrent.Cells[2, 9].EntireRow.Font.Bold = true;
                sheetCurrent.Cells[2, 9].EntireRow.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);
                sheetCurrent.Cells[2, 9].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Green);
                sheetCurrent.Cells[2, 10].EntireRow.Font.Bold = true;
                sheetCurrent.Cells[2, 10].EntireRow.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);
                sheetCurrent.Cells[2, 10].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Green);
                sheetCurrent.Cells[2, 11].EntireRow.Font.Bold = true;
                sheetCurrent.Cells[2, 11].EntireRow.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);
                sheetCurrent.Cells[2, 11].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Green);

                groupBox3.Text = "Report Progress - (Step 1 of 10) CREATING SUMMARY TABLE";

                DataTable myDT = mcData.GetSupplierSummaryByYearDT(rptMode, year);
                DataTable newSummaryDT = new DataTable();
                newSummaryDT.Columns.Clear();
                newSummaryDT.Columns.Add("YEAR", typeof(string));
                newSummaryDT.Columns.Add("SUPPLIER", typeof(string));
                newSummaryDT.Columns.Add(rptMode, typeof(int));
                int sumTotal = 0;
                foreach (DataRow datarow in myDT.Rows)
                {
                    lblYear.Text = year.ToString();
                    lblJobNo.Text = "";
                    lblDate.Text = "";
                    sumTotal++;
                    DataRow sdr = newSummaryDT.NewRow();
                    sdr["Year"] = year;
                    sdr["Supplier"] = datarow["productSupplier"];
                    sdr[rptMode] = Convert.ToInt32(datarow[rptMode]);
                    newSummaryDT.Rows.Add(sdr);
                }
                int summaryRowNo = 0;
                
                int total = 0;
                foreach (DataRow row in newSummaryDT.Rows)
                {
                    //total = Convert.ToInt32(row[rptMode]);
                    summaryRowNo++;
                    sheetCurrent.Cells[summaryRowNo + 2, 9] = row["YEAR"].ToString();
                    sheetCurrent.Cells[summaryRowNo + 2, 10] = row["SUPPLIER"].ToString();
                    sheetCurrent.Cells[summaryRowNo + 2, 11] = sumTotal.ToString();
                    //++;
                }
                summaryRowNo++;
                sheetCurrent.Cells[summaryRowNo + 2, 9] = year.ToString();
                sheetCurrent.Cells[summaryRowNo + 2, 10] = "SUPPLIER";
                sheetCurrent.Cells[summaryRowNo + 2, 11] = sumTotal;
                sheetCurrent.Columns[11].NumberFormat = "#,##";
                sheetCurrent.Columns.AutoFit();
            }
            

            groupBox3.Text = "Report Progress - (Step 1 of 10) SAVING EXCEL FILE AND OPENING";
            oWB.SaveAs(fullFilePath, Microsoft.Office.Interop.Excel.XlFileFormat.xlOpenXMLWorkbook,
                missing, missing, missing, missing,
                Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange,
                missing, missing, missing, missing, missing);
            oWB.Close(missing, missing, missing);
            oXL.UserControl = true;
            oXL.Quit();
            this.Cursor = Cursors.Default;
            Process.Start(fullFilePath);
            this.Dispose();
            this.Close();
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
            GenerateRpt(_rptMode);
            
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
                //fullFilePath = System.IO.Path.Combine(pathTextBox.Text, filename);
                Environment.SpecialFolder root = folderDlg.RootFolder;
            }
        }
    }
}
