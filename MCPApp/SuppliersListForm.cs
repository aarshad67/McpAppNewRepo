using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using Microsoft.Office.Interop.Excel;


namespace MCPApp
{
    public partial class SuppliersListForm : Form
    {
        private string supplierCode = "";
        public string SupplierCode
        {
            get
            {
                return supplierCode;
            }
            set
            {
                supplierCode = value;
            }
        }

        private string shortname = "";
        public string Shortname
        {
            get
            {
                return shortname;
            }
            set
            {
                shortname = value;
            }
        }
        Logger logger = new Logger();

        public SuppliersListForm()
        {
            InitializeComponent();
        }

        public SuppliersListForm(string suppCode)
        {
            InitializeComponent();
            supplierCode = suppCode;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        private void SuppliersListForm_Load(object sender, EventArgs e)
        {
            this.Text = "DOUBLE CLICK to select supplier from list";
            BuildSuppliersDGV();
            PopulateDGV();
            shortname = supplierCode;
            return;
        }

        private void BuildSuppliersDGV()
        {
            try
            {
                suppDGV.Columns.Clear();
                //0
                DataGridViewTextBoxColumn suppCodeBoxColumn = new DataGridViewTextBoxColumn();
                suppCodeBoxColumn.HeaderText = "Supp Code";
                suppCodeBoxColumn.Width = 100;
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
                DataGridViewTextBoxColumn productTypeTextBoxColumn = new DataGridViewTextBoxColumn();
                productTypeTextBoxColumn.HeaderText = "Prod Type";
                productTypeTextBoxColumn.Width = 80;
                productTypeTextBoxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                productTypeTextBoxColumn.ReadOnly = true;
                suppDGV.Columns.Add(productTypeTextBoxColumn);

                suppDGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                suppDGV.EnableHeadersVisualStyles = false;
                

            }
            catch (Exception ex)
            {
                MeltonData mcData = new MeltonData();
                MessageBox.Show("BuildSuppliersDGV ERROR - " + ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.TargetSite);
                string audit = mcData.CreateErrorAudit("SuppliersListForm.cs", "BuildSuppliersDGV()", ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.TargetSite);
                return;
            }
        }

        private void PopulateDGV()
        {
            MeltonData mcData = new MeltonData();
            int row = 0;
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
                    suppDGV.Rows.Add();
                    suppDGV[0, row].Style.BackColor = Color.FromArgb(rgb1, rgb2, rgb3);
                    suppDGV[0, row].Value = dr["suppCode"].ToString();
                    suppDGV[1, row].Value = dr["suppName"].ToString();
                    suppDGV[2, row].Value = dr["shortname"].ToString();
                    suppDGV[3, row++].Value = dr["productType"].ToString();

                }
                suppDGV.CurrentCell = suppDGV.Rows[0].Cells[0];
                return;
            }
            catch (Exception ex)
            {
                string msg = String.Format("PopulateDGV Error : {0}", ex.Message);
                MessageBox.Show(msg);
                logger.LogLine(msg);
                string audit = mcData.CreateErrorAudit("SuppliersListForm.cs", "PopulateDGV()", ex.Message);
                return;
                
            }


        }

        private void suppDGV_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (!suppDGV.Focused) { return; }

                //if (e.ColumnIndex == 0)
                //{
                    if (suppDGV.Rows[e.RowIndex].Cells[0].Value != null)
                    {
                        supplierCode = suppDGV.Rows[e.RowIndex].Cells[0].Value.ToString();
                        shortname = suppDGV.Rows[e.RowIndex].Cells[2].Value.ToString();
                        this.Close();
                        return;
                    }
                //}
                return;
            }
            catch (Exception ex)
            {
                MeltonData mcData = new MeltonData();
                string msg = String.Format("suppDGV_CellDoubleClick() Error : {0}", ex.Message.ToString());
                logger.LogLine(msg);
                string audit = mcData.CreateErrorAudit("SuppliersListForm.cs", "suppDGV_CellDoubleClick()", ex.Message);
                return;
            }
            
        }

        private void selectButton_Click(object sender, EventArgs e)
        {

        }

        private void suppDGV_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {

        }

        

        
    }

}
