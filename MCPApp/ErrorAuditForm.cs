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
    public partial class ErrorAuditForm : Form
    {
        MeltonData mcData = new MeltonData();

        public ErrorAuditForm()
        {
            InitializeComponent();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        private void BuildDGV()
        {
            try
            {
                dgv.Columns.Clear();
                //0
                DataGridViewTextBoxColumn dateTimeColumn = new DataGridViewTextBoxColumn();
                dateTimeColumn.HeaderText = "User";
                dateTimeColumn.Width = 40;
                dateTimeColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dateTimeColumn.ReadOnly = true;
                dgv.Columns.Add(dateTimeColumn);

                //1
                DataGridViewTextBoxColumn userColumn = new DataGridViewTextBoxColumn();
                userColumn.HeaderText = "Audit Date";
                userColumn.Width = 150;
                userColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                userColumn.ReadOnly = true;
                dgv.Columns.Add(userColumn);

                //2
                DataGridViewTextBoxColumn classColumn = new DataGridViewTextBoxColumn();
                classColumn.HeaderText = "Class";
                classColumn.Width = 200;
                classColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                classColumn.ReadOnly = true;
                dgv.Columns.Add(classColumn);

                //3
                DataGridViewTextBoxColumn methodColumn = new DataGridViewTextBoxColumn();
                methodColumn.HeaderText = " Method";
                methodColumn.Width = 200;
                methodColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                methodColumn.ReadOnly = true;
                dgv.Columns.Add(methodColumn);

                //4
                DataGridViewTextBoxColumn errorColumn = new DataGridViewTextBoxColumn();
                errorColumn.HeaderText = " Error Message";
                errorColumn.Width = 1000;
                errorColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                errorColumn.DefaultCellStyle.ForeColor = Color.Red;
                errorColumn.ReadOnly = true;
                dgv.Columns.Add(errorColumn);

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
                string audit = mcData.CreateErrorAudit("JobDateAuditForm.cs", "BuildDGV()", msg);
            }
        }

        private void PopulateDGV()
        {
            int row = 0;

            try
            {
                dgv.Rows.Clear();
                System.Data.DataTable dt = mcData.GetErrorAuditDT();
                if (dt.Rows.Count < 1) { return; }
                foreach (DataRow dr in dt.Rows)
                {
                    dgv.Rows.Add();
                    dgv[0, row].Value = dr["errUser"].ToString();
                    dgv[1, row].Value = Convert.ToDateTime(dr["errDate"].ToString());
                    dgv[2, row].Value = dr["errClassName"].ToString();
                    dgv[3, row].Value = dr["errMethod"].ToString();
                    dgv[4, row++].Value = dr["errMessage"].ToString();
                }
                dgv.CurrentCell = dgv.Rows[0].Cells[0];
                return;
            }
            catch (Exception ex)
            {
                string msg = String.Format("PopulateDGV Error : {0}", ex.Message);
                MessageBox.Show(msg);
               // logger.LogLine(msg);
                string audit = mcData.CreateErrorAudit("JobDateAuditForm.cs", "PopulateDGV()", msg);
                return;
            }
        }

        private void PopulateDGV(int lastNumDays)
        {
            int row = 0;

            try
            {
                dgv.Rows.Clear();
                System.Data.DataTable dt = mcData.GetErrorAuditDT();
                if (dt.Rows.Count < 1) { return; }
                foreach (DataRow dr in dt.Rows)
                {
                    dgv.Rows.Add();
                    dgv[0, row].Value = dr["errUser"].ToString();
                    dgv[1, row].Value = Convert.ToDateTime(dr["errDate"].ToString());
                    dgv[2, row].Value = dr["errClassName"].ToString();
                    dgv[3, row].Value = dr["errMethod"].ToString();
                    dgv[4, row++].Value = dr["errMessage"].ToString();
                }
                dgv.CurrentCell = dgv.Rows[0].Cells[0];
                return;
            }
            catch (Exception ex)
            {
                string msg = String.Format("PopulateDGV Error : {0}", ex.Message);
                MessageBox.Show(msg);
                // logger.LogLine(msg);
                string audit = mcData.CreateErrorAudit("JobDateAuditForm.cs", "PopulateDGV()", msg);
                return;
            }
        }

        private void ErrorAuditForm_Load(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            this.Location = new System.Drawing.Point(0, 0);

            this.Size = Screen.PrimaryScreen.WorkingArea.Size;
            this.Text = "Audit MCP Errors";
            BuildDGV();
            PopulateDGV();
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

            xlWorkBook.SaveAs(@"C:\test1\absBook1.xlsx", Type.Missing, Type.Missing,
            Type.Missing, Type.Missing, Type.Missing, XlSaveAsAccessMode.xlExclusive,
            Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            xlWorkBook.Close();
            xlexcel.Quit();
            return;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.AppStarting;
            GenerateExcelWorkbook();
            this.Cursor = Cursors.Default;
            return;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            PopulateDGV();
        }
    }
}
