using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;

namespace MCPApp
{
    public partial class WhiteboardJobExtensionsForm : Form
    {
        MeltonData mcData = new MeltonData();
        Logger logger = new Logger();

        private int rowIndex = 0;
        private int colIndex = 0;

        public WhiteboardJobExtensionsForm()
        {
            InitializeComponent();
        }

        private void WhiteboardJobExtensionsForm_Load(object sender, EventArgs e)
        {
            this.Text = "Whiteboard Jobs with Extensions Only ";
            label1.Text = "NOTE : Original job is highlighted";
            label2.Text = "DOUBLE CLICk on job to take you to it on the Whiteboard";
            this.Cursor = Cursors.WaitCursor;
            BuildDGV();
            PopulateDGV();
            AlternateExtendedJobsRowColor();
            HighlightAnyIssues();
            this.Cursor = Cursors.Default;
        }

        private void copyAlltoClipboard()
        {
            //  return;
            dgv.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            dgv.MultiSelect = true;
            dgv.SelectAll();
            DataObject dataObj = dgv.GetClipboardContent();
            if (dataObj != null)
                Clipboard.SetDataObject(dataObj);
        }

        private void GenerateExcelWorkbook()
        {

            copyAlltoClipboard();
            Microsoft.Office.Interop.Excel.Application xlexcel;
            Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
            Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;
            xlexcel = new Microsoft.Office.Interop.Excel.Application();
            xlexcel.Visible = true;
            xlWorkBook = xlexcel.Workbooks.Add(misValue);
            xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            xlWorkSheet.Rows.AutoFit();
            xlWorkSheet.Columns.AutoFit();
            //Microsoft.Office.Tools.Excel.NamedRange rng = this.Controls.AddNamedRange(this.Range["A1"], "NamedRange1");
            Range range1 = xlWorkSheet.get_Range("B1", "AB1");
            range1.Cells.Interior.Color = Color.Yellow;

            //freeze top row
            xlWorkSheet.Activate();
            xlWorkSheet.Application.ActiveWindow.SplitRow = 1;
            xlWorkSheet.Application.ActiveWindow.FreezePanes = true;

            //apply filters
            Range firstRow = xlWorkSheet.Rows[1];
            firstRow.Activate();
            firstRow.Select();
            firstRow.AutoFilter(1,
                                Type.Missing,
                                Microsoft.Office.Interop.Excel.XlAutoFilterOperator.xlAnd,
                                Type.Missing,
                                true);
            xlWorkSheet.Cells.Font.Size = 8;
            Microsoft.Office.Interop.Excel.Range CR = (Microsoft.Office.Interop.Excel.Range)xlWorkSheet.Cells[1, 1];
            CR.Select();
            xlWorkSheet.PasteSpecial(CR, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, true);

            //delete blank column 1
            Microsoft.Office.Interop.Excel.Range objRange = (Microsoft.Office.Interop.Excel.Range)xlWorkSheet.get_Range("A1", Type.Missing);
            objRange.EntireColumn.Delete(Type.Missing);
            //autosize all the columns
            Range fullRange = (Microsoft.Office.Interop.Excel.Range)xlWorkSheet.get_Range("A1", "IV65536");
            fullRange.Columns.AutoFit();
            string myFullPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + $"extendedWBjobs{DateTime.Now.ToString("ddmmmyyyhhmmss")}.xlsx";
            
            xlWorkBook.SaveAs(@myFullPath, Type.Missing, Type.Missing,
            Type.Missing, Type.Missing, Type.Missing, XlSaveAsAccessMode.xlExclusive,
            Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            xlWorkBook.Close();
            xlexcel.Quit();
            return;
        }

        private void BuildDGV()
        {
            try
            {
                System.Drawing.Font currentFont = dgv.Font;
                DataGridViewCellStyle newStyle = new DataGridViewCellStyle();
                newStyle.Font = new System.Drawing.Font(currentFont, FontStyle.Bold);

                dgv.Columns.Clear();
                //0
                DataGridViewTextBoxColumn dateTimeColumn = new DataGridViewTextBoxColumn();
                dateTimeColumn.HeaderText = "Job No";
                dateTimeColumn.Width = 80;
                dateTimeColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
               // dateTimeColumn.DefaultCellStyle.BackColor = Color.Yellow;
                dateTimeColumn.ReadOnly = true;
                dgv.Columns.Add(dateTimeColumn);

                //1
                DataGridViewTextBoxColumn dateColumn = new DataGridViewTextBoxColumn();
                dateColumn.HeaderText = "Required Date";
                dateColumn.Width = 180;
                dateColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dateColumn.ReadOnly = true;
                dgv.Columns.Add(dateColumn);

                //2
                DataGridViewTextBoxColumn custCodeColumn = new DataGridViewTextBoxColumn();
                custCodeColumn.HeaderText = "Cust Code";
                custCodeColumn.Width = 80;
                custCodeColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                custCodeColumn.ReadOnly = true;
                dgv.Columns.Add(custCodeColumn);

                //3
                DataGridViewTextBoxColumn siteAddrColumn = new DataGridViewTextBoxColumn();
                siteAddrColumn.HeaderText = "Site Address";
                siteAddrColumn.Width = 400;
                siteAddrColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
              //  siteAddrColumn.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                siteAddrColumn.ReadOnly = true;
                dgv.Columns.Add(siteAddrColumn);

                //4
                DataGridViewTextBoxColumn invoicedColumn = new DataGridViewTextBoxColumn();
                invoicedColumn.HeaderText = "WB Invoiced";
                invoicedColumn.Width = 80;
                invoicedColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                invoicedColumn.DefaultCellStyle = newStyle;
                invoicedColumn.ReadOnly = true;
                dgv.Columns.Add(invoicedColumn);

                //5
                DataGridViewTextBoxColumn completedColumn = new DataGridViewTextBoxColumn();
                completedColumn.HeaderText = "WB Completed";
                completedColumn.Width = 80;
                completedColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                completedColumn.DefaultCellStyle = newStyle;
                completedColumn.ReadOnly = true;
                dgv.Columns.Add(completedColumn);

                //6
                DataGridViewTextBoxColumn jpCompletedColumn = new DataGridViewTextBoxColumn();
                jpCompletedColumn.HeaderText = "JP Completed";
                jpCompletedColumn.Width = 80;
                jpCompletedColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                jpCompletedColumn.DefaultCellStyle = newStyle;
                jpCompletedColumn.ReadOnly = true;
                dgv.Columns.Add(jpCompletedColumn);

                dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgv.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                dgv.EnableHeadersVisualStyles = false;
                dgv.AllowUserToAddRows = false;




            }
            catch (Exception ex)
            {
                string msg = "BuildDGV ERROR - " + ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.TargetSite;
                MessageBox.Show(msg);
                string audit = mcData.CreateErrorAudit("WhiteboardJobExtensionsForm.cs", "BuildDGV()", msg);
            }
        }

        private void PopulateDGV()
        {
            int row = 0;
            

            try
            {
                dgv.Rows.Clear();
                System.Data.DataTable dt = mcData.GetWhiteboardJobExtensionsWithDatesAndCompleteFlagsDT();
                if (dt.Rows.Count < 1) { return; }
                string job = "";
                foreach (DataRow dr in dt.Rows)
                {
                    dgv.Rows.Add();
                    job = dr["jobNo"].ToString();
                    dgv[0, row].Value = job;
                    dgv[1, row].Value = Convert.ToDateTime(dr["requiredDate"]).ToString("dddd, dd MMMM yyyy");
                    dgv[2, row].Value = dr["custCode"].ToString();
                    dgv[3, row].Value = dr["siteAddress"].ToString();
                    dgv[4, row].Value = dr["isInvoiced"].ToString();
                    dgv[5, row].Value = dr["completedFlag"].ToString();
                    dgv[6, row++].Value = mcData.IsJobCompleted(job) ? "Y" : "N"; 
                }
                dgv.CurrentCell = dgv.Rows[0].Cells[0];
                return;
            }
            catch (Exception ex)
            {
                string msg = String.Format("PopulateDGV Error : {0}", ex.Message);
                MessageBox.Show(msg);
                // logger.LogLine(msg);
                string audit = mcData.CreateErrorAudit("WhiteboardJobExtensionsForm.cs", "PopulateDGV()", msg);
                return;
            }
        }

        private void AlternateExtendedJobsRowColor()
        {
            var jobRows = dgv.Rows.Cast<DataGridViewRow>()
         .Where(x => x.Cells[0].Value.ToString().Length == 8);

            string lastSample = "";
            Color backColor = Color.LightGray;
            foreach (DataGridViewRow row in jobRows)
            {
                string currentSample = row.Cells[0].Value.ToString().Substring(0, 8);
                if (currentSample != lastSample)
                {
                    //backColor = (backColor == Color.LightGray) ? Color.DarkGray : Color.LightGray;
                    backColor = Color.LightGray;
                    lastSample = currentSample;
                }
                row.Cells.Cast<DataGridViewCell>()
                    .Where(x => x.Style.BackColor == default(Color))
                    .ToList().ForEach(x => x.Style.BackColor = backColor);
            }
        }

        private void HighlightAnyIssues()
        {
            foreach (DataGridViewRow dgvRow in dgv.Rows)
            {
                string wbInvoiced = dgvRow.Cells[4].Value.ToString().Trim();
                string wbCompleted = dgvRow.Cells[5].Value.ToString().Trim();
                string jpCompleted = dgvRow.Cells[6].Value.ToString().Trim();
                if (String.Compare(wbInvoiced, wbCompleted) == 0 &&  String.Compare(wbCompleted, jpCompleted) == 0)
                {
                    dgvRow.DefaultCellStyle.ForeColor = Color.Black;
                }
                else
                {
                    dgvRow.DefaultCellStyle.ForeColor = Color.Red;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            GenerateExcelWorkbook();
        }

        private void dgv_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!dgv.Focused) { return; }

            if (dgv.Rows[e.RowIndex].Cells[0].Value == null)
            {
                return;
            }
            string phaseJob = dgv.Rows[e.RowIndex].Cells[0].Value.ToString();


           

            if (!mcData.IsValidWhiteboardJob(phaseJob))
            {
                MessageBox.Show(String.Format("Job [{0}] is not [ON SHOP] or [APPROVED]. Cannot continue to Whiteboard", phaseJob));
                return;
            }

            if (mcData.IsJobCompleted(phaseJob))
            {
                MessageBox.Show(String.Format("Job [{0}] is flagged as COMPLETED. Cannot continue to Whiteboard", phaseJob));
                return;
            }
            // int numWeeks = 1;
            System.Data.DataTable dt = mcData.WhiteboardDatesDT(phaseJob);
            DateTime jobDate = mcData.GetPlannerDateByJobNo(phaseJob);
            DateTime startDate = mcData.GetMonday(jobDate);
            DateTime lastDate = startDate.AddDays(6);
            TimeSpan ts = lastDate - startDate;
            int dateDiff = ts.Days;
            decimal numWeeks = dateDiff / 7m;
            int roundedNumWeeks = (int)Decimal.Round(numWeeks, 1) + 1;
            WhiteboardForm wbForm = new WhiteboardForm(phaseJob,startDate, lastDate, dt, roundedNumWeeks);
            wbForm.ShowDialog();
            return;
        }
    }
}
