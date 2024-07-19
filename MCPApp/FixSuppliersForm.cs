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
    public partial class FixSuppliersForm : Form
    {
        MeltonData mcData = new MeltonData();
        public FixSuppliersForm()
        {
            InitializeComponent();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        private void FixSuppliersForm_Load(object sender, EventArgs e)
        {
            progressBar1.Minimum = 0;

            this.Text = "Fix Suppliers";
            DataTable dt = mcData.GetAllWhiteboardSupplierShortnamesForCombo();
            comboBox1.DataSource = dt;
            comboBox1.ValueMember = dt.Columns[0].ColumnName;
            comboBox1.DisplayMember = dt.Columns[0].ColumnName.ToString();
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
                DataGridViewTextBoxColumn productTypeTextBoxColumn = new DataGridViewTextBoxColumn();  //0
                productTypeTextBoxColumn.HeaderText = "Product Type";
                productTypeTextBoxColumn.Width = 100;
                productTypeTextBoxColumn.ReadOnly = false;
                suppDGV.Columns.Add(productTypeTextBoxColumn);

                

                suppDGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                suppDGV.EnableHeadersVisualStyles = false;
                suppDGV.Columns[0].DefaultCellStyle.BackColor = Color.Yellow;
                suppDGV.Columns[1].DefaultCellStyle.BackColor = Color.Yellow;




            }
            catch (Exception ex)
            {

                MessageBox.Show("BuildSuppliersDGV() ERROR - " + ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.TargetSite);
                string audit = mcData.CreateErrorAudit("FixSuppliersForm.cs", "BuildSuppliersDGV()", ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.TargetSite);
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
                    suppDGV[2, row].Style.BackColor = Color.FromArgb(rgb1, rgb2, rgb3);
                    suppDGV[0, row].Value = dr["suppCode"].ToString();
                    suppDGV[1, row].Value = dr["suppName"].ToString();
                    suppDGV[2, row].Value = dr["shortname"].ToString();
                    suppDGV[3, row++].Value = dr["productType"].ToString();
                    
                }
                suppDGV.CurrentCell = suppDGV.Rows[0].Cells[0];

            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("Populating Customers DGV Error : {0}", ex.Message));
                string audit = mcData.CreateErrorAudit("FixSuppliersForm.cs", "PopulateDGV()", ex.Message);
                return;
            }


        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdateSupplier(comboBox1.Text);
            summaryDGV.DataSource = null;
            summaryDGV.DataSource = mcData.GetWhiteboardSupplierSummary();
            return;
        }

        private void UpdateSupplier(string shortname)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                DataTable dt = mcData.GetKeyWhiteboardDetailsBySupplier(shortname);
                progressBar1.Maximum = dt.Rows.Count;
                string jobNo, stairsIncl = "";
                int beamLm, beamM2, slabM2 = 0;
                int counter = 0;

                foreach (DataRow dr in dt.Rows)
                {
                    jobNo = dr["jobNo"].ToString();
                    stairsIncl = dr["stairsIncl"].ToString().ToUpper();
                    beamLm = Convert.ToInt32(dr["beamLm"].ToString());
                    beamM2 = Convert.ToInt32(dr["beamM2"].ToString());
                    slabM2 = Convert.ToInt32(dr["slabM2"].ToString());
                    counter++;
                    progressBar1.Value = counter;
                    if (slabM2 == 0  && (beamLm > 0 || beamM2 > 0 )) { mcData.UpdateWhiteBoardSupplier(jobNo, shortname.ToUpper().Trim() + " BEAMS"); }
                    if (slabM2 > 0 && (beamLm == 0 && beamM2 == 0)) { mcData.UpdateWhiteBoardSupplier(jobNo, shortname.ToUpper().Trim() + " SLAB"); }
                    if (stairsIncl == "Y") { mcData.UpdateWhiteBoardSupplier(jobNo, shortname.ToUpper().Trim() + " STAIRS"); }
                }
                MessageBox.Show($"{counter} jobs updated their current [{shortname}] supplier");
                progressBar1.Value = 0;
                this.Cursor = Cursors.Default;
                counter = 0;
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Updating Supplier('{shortname}')  Error : {ex.Message}");
                string audit = mcData.CreateErrorAudit("FixSuppliersForm.cs", "UpdateSupplier()", ex.Message);
                this.Cursor = Cursors.Default;
                return;
            }
            
        }
    }
}
