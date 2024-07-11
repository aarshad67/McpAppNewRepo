using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;

namespace MCPApp
{
    public partial class SuppliersOverviewForm : Form
    {
        
        MeltonData mcData = new MeltonData();
        Logger logger = new Logger();

        public SuppliersOverviewForm()
        {
            InitializeComponent();
        }

        private void SuppliersOverviewForm_Load(object sender, EventArgs e)
        {
            this.Text = "Supplier Maintanance Screen ( DOUBLE CLICK on a supplier to edit it )";
            BuildSuppliersDGV();
            PopulateDGV();
        }

        private void BuildSuppliersDGV()
        {
            try
            {


                suppDGV.Columns.Clear();
                //0
                DataGridViewTextBoxColumn suppCodeBoxColumn = new DataGridViewTextBoxColumn();
                suppCodeBoxColumn.HeaderText = "Supp Code";
                suppCodeBoxColumn.Width = 70;
                suppCodeBoxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                suppCodeBoxColumn.ReadOnly = true;
                suppDGV.Columns.Add(suppCodeBoxColumn);

                //1
                DataGridViewTextBoxColumn suppNameTextBoxColumn = new DataGridViewTextBoxColumn();
                suppNameTextBoxColumn.HeaderText = "Supplier Name";
                suppNameTextBoxColumn.Width = 200;
                suppNameTextBoxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                suppNameTextBoxColumn.ReadOnly = true;
                suppDGV.Columns.Add(suppNameTextBoxColumn);

                //2
                DataGridViewTextBoxColumn shortnameTextBoxColumn = new DataGridViewTextBoxColumn();
                shortnameTextBoxColumn.HeaderText = "Shortname";
                shortnameTextBoxColumn.Width = 100;
                shortnameTextBoxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                shortnameTextBoxColumn.ReadOnly = true;
                suppDGV.Columns.Add(shortnameTextBoxColumn);

                //3
                DataGridViewTextBoxColumn suppAddressTextBoxColumn = new DataGridViewTextBoxColumn();
                suppAddressTextBoxColumn.HeaderText = "Address";
                suppAddressTextBoxColumn.Width = 300;
                suppAddressTextBoxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                suppAddressTextBoxColumn.ReadOnly = true;
                suppDGV.Columns.Add(suppAddressTextBoxColumn);

                //4
                DataGridViewTextBoxColumn productTypeTextBoxColumn = new DataGridViewTextBoxColumn();  //0
                productTypeTextBoxColumn.HeaderText = "Product Type";
                productTypeTextBoxColumn.Width = 100;
                productTypeTextBoxColumn.ReadOnly = false;
                suppDGV.Columns.Add(productTypeTextBoxColumn);

                //5
                DataGridViewTextBoxColumn contactNameTextBoxColumn = new DataGridViewTextBoxColumn();
                contactNameTextBoxColumn.HeaderText = "Contact Name";
                contactNameTextBoxColumn.Width = 120;
                contactNameTextBoxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                contactNameTextBoxColumn.ReadOnly = true;
                suppDGV.Columns.Add(contactNameTextBoxColumn);

                //6
                DataGridViewTextBoxColumn contactEmailTextBoxColumn = new DataGridViewTextBoxColumn();
                contactEmailTextBoxColumn.HeaderText = "Contact Email";
                contactEmailTextBoxColumn.Width = 180;
                contactEmailTextBoxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                contactEmailTextBoxColumn.ReadOnly = true;
                suppDGV.Columns.Add(contactEmailTextBoxColumn);

                //7
                DataGridViewTextBoxColumn contactTelTextBoxColumn = new DataGridViewTextBoxColumn();
                contactTelTextBoxColumn.HeaderText = "Contact Tel";
                contactTelTextBoxColumn.Width = 120;
                contactTelTextBoxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                contactTelTextBoxColumn.ReadOnly = true;
                suppDGV.Columns.Add(contactTelTextBoxColumn);

                //8
                DataGridViewTextBoxColumn colourCodeTextBoxColumn = new DataGridViewTextBoxColumn();
                colourCodeTextBoxColumn.HeaderText = "Colour Code";
                colourCodeTextBoxColumn.Width = 120;
                colourCodeTextBoxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                colourCodeTextBoxColumn.ReadOnly = true;
                suppDGV.Columns.Add(colourCodeTextBoxColumn);

                //9
                DataGridViewTextBoxColumn rgb1TextBoxColumn = new DataGridViewTextBoxColumn();
                rgb1TextBoxColumn.HeaderText = "RGB1";
                rgb1TextBoxColumn.Width = 50;
                rgb1TextBoxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                rgb1TextBoxColumn.ReadOnly = true;
                suppDGV.Columns.Add(rgb1TextBoxColumn);

                //10
                DataGridViewTextBoxColumn rgb2TextBoxColumn = new DataGridViewTextBoxColumn();
                rgb2TextBoxColumn.HeaderText = "RGB2";
                rgb2TextBoxColumn.Width = 50;
                rgb2TextBoxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                rgb2TextBoxColumn.ReadOnly = true;
                suppDGV.Columns.Add(rgb2TextBoxColumn);

                //11
                DataGridViewTextBoxColumn rgb3TextBoxColumn = new DataGridViewTextBoxColumn();
                rgb3TextBoxColumn.HeaderText = "RGB3";
                rgb3TextBoxColumn.Width = 50;
                rgb3TextBoxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                rgb3TextBoxColumn.ReadOnly = true;
                suppDGV.Columns.Add(rgb3TextBoxColumn);

                suppDGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                suppDGV.EnableHeadersVisualStyles = false;
                suppDGV.Columns[0].DefaultCellStyle.BackColor = Color.Yellow;
                suppDGV.Columns[1].DefaultCellStyle.BackColor = Color.Yellow;




            }
            catch (Exception ex)
            {

                MessageBox.Show("BuildSuppliersDGV() ERROR - " + ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.TargetSite);
                string audit = mcData.CreateErrorAudit("SuppliersOverviewForm.cs", "BuildSuppliersDGV()", ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.TargetSite);
                return;
            }





        }

        private void PopulateDGV()
        {
            int row = 0;
            string suppCode = "";
            int rgb1, rgb2, rgb3 = 255;
            try
            {
                suppDGV.Rows.Clear();
                System.Data.DataTable dt = mcData.GetAllSuppliers();
                foreach (DataRow dr in dt.Rows)
                {
                    rgb1 = dr["rgb1"] == null ? 255 : Convert.ToInt16(dr["rgb1"].ToString());
                    rgb2 = dr["rgb2"] == null ? 255 : Convert.ToInt16(dr["rgb2"].ToString());
                    rgb3 = dr["rgb3"] == null ? 255 : Convert.ToInt16(dr["rgb3"].ToString());
                    suppCode = dr["suppCode"].ToString();

                    suppDGV.Rows.Add();
                    suppDGV[0, row].Style.BackColor = Color.FromArgb(rgb1, rgb2, rgb3);
                    suppDGV[8, row].Style.BackColor = Color.FromArgb(rgb1, rgb2, rgb3);
                    suppDGV[0, row].Value = dr["suppCode"].ToString();
                    suppDGV[1, row].Value = dr["suppName"].ToString();
                    suppDGV[2, row].Value = dr["shortname"].ToString();
                    suppDGV[3, row].Value = dr["suppAddress"].ToString();
                    suppDGV[4, row].Value = dr["productType"].ToString();
                    suppDGV[5, row].Value = dr["contactName"].ToString();
                    suppDGV[6, row].Value = dr["contactEmail"].ToString();
                    suppDGV[7, row].Value = dr["contactTel"].ToString();
                    suppDGV[8, row].Value = dr["colourCode"].ToString();
                    suppDGV[9, row].Value = dr["rgb1"].ToString();
                    suppDGV[10, row].Value = dr["rgb2"].ToString();
                    suppDGV[11, row++].Value = dr["rgb3"].ToString();
                }
                suppDGV.CurrentCell = suppDGV.Rows[0].Cells[0];

            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("Populating Customers DGV Error : {0}", ex.Message));
                string audit = mcData.CreateErrorAudit("SuppliersOverviewForm.cs", "PopulateDGV()", ex.Message);
                return;
            }


        }

        private void GetSuppCurrentRow(string suppCode)
        {
            if (!this.Text.Contains("Supplier")) { return; }
            int index = -1;
            try
            {
                for (int i = 0; i < suppDGV.Rows.Count; i++)
                {
                    if (suppDGV.Rows[i].Cells[0].Value.ToString() == suppCode)
                    {
                        index = i;
                        break;
                    }
                }
                if (index > -1)
                {
                    suppDGV.CurrentCell = suppDGV.Rows[index].Cells[0];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("GetSuppCurrentRow()  ERROR : " + ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.TargetSite);
                string audit = mcData.CreateErrorAudit("SuppliersOverviewForm.cs", String.Format("GetSuppCurrentRow({0})",suppCode), ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.TargetSite);
                return;
            }
        }

        private void AddSupplierBtn_Click(object sender, EventArgs e)
        {
            AddSupplierForm addForm = new AddSupplierForm();
            addForm.ShowDialog();
            PopulateDGV();
            if (addForm.SuppCodeUpdated != "ERR" && !String.IsNullOrWhiteSpace(addForm.SuppCodeUpdated)) { GetSuppCurrentRow(addForm.SuppCodeUpdated); }
            return;
        }

        private void suppDGV_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string suppCode = suppDGV[0, e.RowIndex].Value.ToString();
                AddSupplierForm addForm = new AddSupplierForm(suppCode);
                addForm.ShowDialog();
                PopulateDGV();
                if (addForm.SuppCodeUpdated != "ERR" && addForm.SuppCodeUpdated != "DEL" && !String.IsNullOrWhiteSpace(addForm.SuppCodeUpdated)) { GetSuppCurrentRow(addForm.SuppCodeUpdated); } else { PopulateDGV(); }
                return;
            }
            catch (Exception ex)
            {
                string msg = String.Format("suppDGV_CellDoubleClick() Error : {0}", ex.Message.ToString());
                logger.LogLine(msg);
                string audit = mcData.CreateErrorAudit("SuppliersOverviewForm.cs", "suppDGV_CellDoubleClick()", ex.Message);
                return;
            }
            
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        private void suppDGV_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //if (!suppDGV.Focused) { return; }

            //if (suppDGV.Rows[e.RowIndex].Cells[0].Value == null)
            //{
            //    return;
            //}

            //string suppCode = suppDGV.Rows[e.RowIndex].Cells[0].Value.ToString();
            //int rgb1, rgb2,rgb3 = 255;
            //mcData.GetSupplierColour(suppCode, out rgb1, out rgb2, out rgb3);
            //suppDGV.Rows[e.RowIndex].Cells[0].Style.BackColor = Color.FromArgb(rgb1, rgb2, rgb3);
            //return;
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GenerateExcelWorkbook();
        }

        private void copyAlltoClipboard()
        {
            //  return;
            suppDGV.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            suppDGV.MultiSelect = true;
            suppDGV.SelectAll();
            DataObject dataObj = suppDGV.GetClipboardContent();
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
            string excelRptPath = Path.Combine(Path.GetTempPath(), $"SupplierList_{DateTime.Now.ToString("ddmmyyyyhhmmmss")}.xlsx");
            //Path.Combine(Path.GetTempPath(), "MCP_Error_" + loggedInUser.ToUpper() + "_" + dd + monthName.ToUpper() + yyyy + ".txt");
            xlWorkBook.SaveAs(excelRptPath, Type.Missing, Type.Missing,
            Type.Missing, Type.Missing, Type.Missing, XlSaveAsAccessMode.xlExclusive,
            Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            xlWorkBook.Close();
            xlexcel.Quit();
            return;
        }
    }
}
