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
using System.IO;


namespace MCPApp
{
    public partial class WhiteboardEmptyProductsForm : Form
    {
        MeltonData mcData = new MeltonData();
        public WhiteboardEmptyProductsForm()
        {
            InitializeComponent();
        }

        private void WhiteboardEmptyProductsForm_Load(object sender, EventArgs e)
        {
           
            this.Text = "All Whiteboard Jobs With Products and Quantities";
            //  BuildDGV();
            //PopulateDGV();
            System.Data.DataTable myDT = mcData.GetAllWhiteboardJobProductsByQtyDT();
            myDGV.DataSource = myDT;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = myDT.Rows.Count;
        }

        private void BuildDGV()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                myDGV.Columns.Clear();
                //0
                DataGridViewTextBoxColumn jobNoBoxColumn = new DataGridViewTextBoxColumn();
                jobNoBoxColumn.HeaderText = "JobNo";
                jobNoBoxColumn.Width = 70;
                jobNoBoxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                jobNoBoxColumn.ReadOnly = true;
                myDGV.Columns.Add(jobNoBoxColumn);

                //1
                DataGridViewTextBoxColumn reqDateColumn = new DataGridViewTextBoxColumn();
                reqDateColumn.HeaderText = "Req. Date";
                reqDateColumn.Width = 100;
                reqDateColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                reqDateColumn.ReadOnly = true;
                myDGV.Columns.Add(reqDateColumn);

                //2
                DataGridViewTextBoxColumn levelColumn = new DataGridViewTextBoxColumn();
                levelColumn.HeaderText = "Floor Level";
                levelColumn.Width = 120;
                levelColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                levelColumn.ReadOnly = true;
                myDGV.Columns.Add(levelColumn);

                //3
                DataGridViewTextBoxColumn suppTypeColumn = new DataGridViewTextBoxColumn();
                suppTypeColumn.HeaderText = "Supp Type";
                suppTypeColumn.Width = 60;
                suppTypeColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                suppTypeColumn.ReadOnly = true;
                myDGV.Columns.Add(suppTypeColumn);

                //4
                DataGridViewTextBoxColumn productColumn = new DataGridViewTextBoxColumn();  //0
                productColumn.HeaderText = "Product";
                productColumn.Width = 100;
                productColumn.DefaultCellStyle.BackColor = Color.LightYellow;
                productColumn.ReadOnly = false;
                myDGV.Columns.Add(productColumn);

                //5
                DataGridViewTextBoxColumn stairsColumn = new DataGridViewTextBoxColumn();
                stairsColumn.HeaderText = "Stairs?";
                stairsColumn.Width = 60;
                stairsColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                stairsColumn.ReadOnly = true;
                myDGV.Columns.Add(stairsColumn);

                //6
                DataGridViewTextBoxColumn totalM2Column = new DataGridViewTextBoxColumn();
                totalM2Column.HeaderText = "Total M²";
                totalM2Column.Width = 60;
                totalM2Column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                totalM2Column.ReadOnly = true;
                myDGV.Columns.Add(totalM2Column);

                //7
                DataGridViewTextBoxColumn beamLMColumn = new DataGridViewTextBoxColumn();
                beamLMColumn.HeaderText = "Beam LM";
                beamLMColumn.Width = 60;
                beamLMColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                beamLMColumn.ReadOnly = true;
                myDGV.Columns.Add(beamLMColumn);

                //8
                DataGridViewTextBoxColumn beamM2Column = new DataGridViewTextBoxColumn();
                beamM2Column.HeaderText = "Beam M²";
                beamM2Column.Width = 60;
                beamM2Column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                beamM2Column.ReadOnly = true;
                myDGV.Columns.Add(beamM2Column);

                //9
                DataGridViewTextBoxColumn slabM2Column = new DataGridViewTextBoxColumn();
                slabM2Column.HeaderText = "Slab M²";
                slabM2Column.Width = 60;
                slabM2Column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                slabM2Column.ReadOnly = true;
                myDGV.Columns.Add(slabM2Column);

                //10
                DataGridViewTextBoxColumn supplierColumn = new DataGridViewTextBoxColumn();
                supplierColumn.HeaderText = "SUPPLIER";
                supplierColumn.Width = 80;
                supplierColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                supplierColumn.DefaultCellStyle.BackColor = Color.LightYellow;
                supplierColumn.ReadOnly = true;
                myDGV.Columns.Add(supplierColumn);

                //11
                DataGridViewTextBoxColumn custCodeColumn = new DataGridViewTextBoxColumn();
                custCodeColumn.HeaderText = "Cust Code";
                custCodeColumn.Width = 60;
                custCodeColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                custCodeColumn.ReadOnly = true;
                myDGV.Columns.Add(custCodeColumn);

                //12
                DataGridViewTextBoxColumn siteColumn = new DataGridViewTextBoxColumn();
                siteColumn.HeaderText = "Site Address";
                siteColumn.Width = 200;
                siteColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                siteColumn.ReadOnly = true;
                myDGV.Columns.Add(siteColumn);

                myDGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                myDGV.EnableHeadersVisualStyles = false;
                //  myDGV.Columns[0].DefaultCellStyle.BackColor = Color.Yellow;
                //   myDGV.Columns[1].DefaultCellStyle.BackColor = Color.Yellow;

                this.Cursor = Cursors.Default;


            }
            catch (Exception ex)
            {

                MessageBox.Show("BuildDGV() ERROR - " + ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.TargetSite);
                string audit = mcData.CreateErrorAudit("WhiteboardEmptyProductsForm.cs", "BuildDGV()", ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.TargetSite);
                this.Cursor = Cursors.Default;
                return;
            }





        }

        private void PopulateDGV()
        {
            int row = 0;
            
            try
            {
                this.Cursor = Cursors.WaitCursor;
                myDGV.Rows.Clear();
                System.Data.DataTable dt = mcData.GetAllWhiteboardJobProductsByQtyDT();
                foreach (DataRow dr in dt.Rows)
                {

                    myDGV.Rows.Add();
                    myDGV[0, row].Value = dr["job"].ToString();
                    myDGV[1, row].Value = dr["RequiredDate"].ToString();
                    myDGV[2, row].Value = dr["FloorLevel"].ToString();
                    myDGV[3, row].Value = dr["jobType"].ToString();
                    myDGV[4, row].Value = dr["product"].ToString();
                    myDGV[5, row].Value = dr["stairs"].ToString();
                    myDGV[6, row].Value = dr["TotalM2"].ToString();
                    myDGV[7, row].Value = dr["BeamLm"].ToString();
                    myDGV[8, row].Value = dr["BeamM2"].ToString();
                    myDGV[9, row].Value = dr["SlabM2"].ToString();
                    myDGV[10, row].Value = dr["Supplier"].ToString();
                    myDGV[11, row].Value = dr["CustomerCode"].ToString();
                    myDGV[12, row++].Value = dr["SiteAddress"].ToString();
                }
                myDGV.CurrentCell = myDGV.Rows[0].Cells[0];
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("Populating Customers DGV Error : {0}", ex.Message));
                string audit = mcData.CreateErrorAudit("WhiteboardEmptyProductsForm.cs", "PopulateDGV()", ex.Message);
                this.Cursor = Cursors.Default;
                return;
            }


        }


        private void copyAlltoClipboard()
        {
            //  return;
            myDGV.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            myDGV.MultiSelect = true;
            myDGV.SelectAll();
            DataObject dataObj = myDGV.GetClipboardContent();
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

            Range jobColumnRange = xlWorkSheet.UsedRange[1, Type.Missing];
            jobColumnRange.Cells.NumberFormat = "0.00";


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
            string excelRptPath = Path.Combine(Path.GetTempPath(), $"CompeletedWhiteboardJobsWithMissingPeroducts_{DateTime.Now.ToString("ddmmyyyyhhmmmss")}.xlsx");
            //Path.Combine(Path.GetTempPath(), "MCP_Error_" + loggedInUser.ToUpper() + "_" + dd + monthName.ToUpper() + yyyy + ".txt");
            xlWorkBook.SaveAs(excelRptPath, Type.Missing, Type.Missing,
            Type.Missing, Type.Missing, Type.Missing, XlSaveAsAccessMode.xlExclusive,
            Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            xlWorkBook.Close();
            xlexcel.Quit();
            System.Diagnostics.Process.Start(excelRptPath);
            return;
        }


        private void CalculateRevisedProducts()
        {
            /*
            BB Only
            SL Only
            SLAB
            ST Only

            0 job,
            1 Completed,
            2 RequiredDate,
            3 FloorLevel,
            4 jobType,
            5 CurrentProduct,
            6 RevisedProduct,
            7 stairs,
            8 TotalM2,
            9 BeamLm,
            10 BeamM2,
            11 SlabM2,
            12 Supplier,
            13 CustomerCode,
            14 SiteAddress

             */
            string supplier = "";
            string product = "";
            int beamLm = 0;
            int beamM2 = 0;
            int slabM2 = 0;
            string jobNo = "";
            string revisedProduct = "";
            this.Cursor = Cursors.WaitCursor;
            for (int i = 0; i < myDGV.Rows.Count; i++)
            {
                progressBar1.Value = i;
                if (myDGV.Rows[i].Cells[0].Value == null) { continue; }
                jobNo = myDGV.Rows[i].Cells[0].Value.ToString();
                product = myDGV.Rows[i].Cells[5].Value.ToString();
                beamLm = Convert.ToInt32(myDGV.Rows[i].Cells[9].Value);
                beamM2 = Convert.ToInt32(myDGV.Rows[i].Cells[10].Value);
                slabM2 = Convert.ToInt32(myDGV.Rows[i].Cells[11].Value);
                supplier = myDGV.Rows[i].Cells[12].Value.ToString();
                revisedProduct = mcData.GetRevisedProductFromQtyAndSupplier(product, supplier, beamLm, beamM2, slabM2);
                string result = mcData.UpdateWhiteBoardJobProduct(jobNo, revisedProduct);
                
                myDGV.Rows[i].Cells[6].Value = revisedProduct;
                lblJob.Text = jobNo;
                lblJob.Refresh();
                lblCurrentProduct.Text = product;
                lblCurrentProduct.Refresh();
                lblSupplier.Text = supplier;
                lblSupplier.Refresh();
                lblRevisedProduct.Text = revisedProduct;
                lblRevisedProduct.Refresh();
                
            }
            myDGV.Refresh();
            this.Cursor = Cursors.Default;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            GenerateExcelWorkbook();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Path.GetTempPath());
        }

        private void button6_Click(object sender, EventArgs e)
        {
            CalculateRevisedProducts();
        }
    }
}
