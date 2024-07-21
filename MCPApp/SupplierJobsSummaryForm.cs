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
    public partial class SupplierJobsSummaryForm : Form
    {
        MeltonData mcData = new MeltonData();

        public SupplierJobsSummaryForm()
        {
            InitializeComponent();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        private void BuildSuppliersDGV()
        {
            try
            {
                suppDGV.Columns.Clear();
                //0
                DataGridViewTextBoxColumn compFlagColumn = new DataGridViewTextBoxColumn();
                compFlagColumn.HeaderText = "Completed";
                compFlagColumn.Width = 50;
                compFlagColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                compFlagColumn.ReadOnly = true;
                suppDGV.Columns.Add(compFlagColumn);

                //1
                DataGridViewTextBoxColumn suppCodeBoxColumn = new DataGridViewTextBoxColumn();
                suppCodeBoxColumn.HeaderText = "Supp Code";
                suppCodeBoxColumn.Width = 70;
                suppCodeBoxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                suppCodeBoxColumn.ReadOnly = true;
                suppDGV.Columns.Add(suppCodeBoxColumn);

                //2
                DataGridViewTextBoxColumn suppNameTextBoxColumn = new DataGridViewTextBoxColumn();
                suppNameTextBoxColumn.HeaderText = "Supplier Name";
                suppNameTextBoxColumn.Width = 150;
                suppNameTextBoxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                suppNameTextBoxColumn.ReadOnly = true;
                suppDGV.Columns.Add(suppNameTextBoxColumn);

                //3
                DataGridViewTextBoxColumn shortnameTextBoxColumn = new DataGridViewTextBoxColumn();
                shortnameTextBoxColumn.HeaderText = "Shortname";
                shortnameTextBoxColumn.Width = 100;
                shortnameTextBoxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                shortnameTextBoxColumn.ReadOnly = true;
                suppDGV.Columns.Add(shortnameTextBoxColumn);

                //4
                DataGridViewTextBoxColumn jobCountColumn = new DataGridViewTextBoxColumn();  //0
                jobCountColumn.HeaderText = "Job Count";
                jobCountColumn.Width = 60;
                jobCountColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                jobCountColumn.ReadOnly = true;
                suppDGV.Columns.Add(jobCountColumn);

                //5
                DataGridViewTextBoxColumn beamLMColumn = new DataGridViewTextBoxColumn();  //0
                beamLMColumn.HeaderText = "Beam LM";
                beamLMColumn.Width = 60;
                beamLMColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                beamLMColumn.ReadOnly = true;
                suppDGV.Columns.Add(beamLMColumn);

                //6
                DataGridViewTextBoxColumn beamM2Column = new DataGridViewTextBoxColumn();  //0
                beamM2Column.HeaderText = "Beam M²";
                beamM2Column.Width = 60;
                beamM2Column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                beamM2Column.ReadOnly = true;
                suppDGV.Columns.Add(beamM2Column);

                //7
                DataGridViewTextBoxColumn slabM2Column = new DataGridViewTextBoxColumn();  //0
                slabM2Column.HeaderText = "Slab M²";
                slabM2Column.Width = 60;
                slabM2Column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                slabM2Column.ReadOnly = false;
                suppDGV.Columns.Add(slabM2Column);

                ////8
                //DataGridViewTextBoxColumn totalM2Column = new DataGridViewTextBoxColumn();  //0
                //totalM2Column.HeaderText = "Total M² ";
                //totalM2Column.Width = 70;
                //totalM2Column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                //totalM2Column.ReadOnly = false;
                //suppDGV.Columns.Add(totalM2Column);



                suppDGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                suppDGV.EnableHeadersVisualStyles = false;
                suppDGV.Columns[0].DefaultCellStyle.BackColor = Color.Yellow;
                suppDGV.Columns[0].DefaultCellStyle.Font = new Font(suppDGV.DefaultCellStyle.Font, FontStyle.Bold);
                suppDGV.Columns[3].DefaultCellStyle.Font = new Font(suppDGV.DefaultCellStyle.Font, FontStyle.Bold);
                // suppDGV.Columns[1].DefaultCellStyle.BackColor = Color.Yellow;




            }
            catch (Exception ex)
            {

                MessageBox.Show("BuildSuppliersDGV() ERROR - " + ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.TargetSite);
                string audit = mcData.CreateErrorAudit("SupplierJobsSummaryForm.cs", "BuildSuppliersDGV()", ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.TargetSite);
                return;
            }
        }

        private void PopulateDGV()
        {
            int row = 0;
            string shortname, suppCode, suppName = "";
            int rgb1, rgb2, rgb3 = 255;
            try
            {
                suppDGV.Rows.Clear();
                System.Data.DataTable dt = mcData.GetWhiteboardSupplierSummary();
                progressBar1.Maximum = dt.Rows.Count;
                foreach (DataRow dr in dt.Rows)
                {
                    shortname = dr["Supplier"].ToString();
                    mcData.GetSupplierColourByShortname(shortname, out rgb1, out rgb2, out rgb3);
                    mcData.GetSupplierDetailsByShortname(shortname, out suppCode, out suppName);
                    suppDGV.Rows.Add();
                    suppDGV[1, row].Style.BackColor = Color.FromArgb(rgb1, rgb2, rgb3);
                    suppDGV[3, row].Style.BackColor = Color.FromArgb(rgb1, rgb2, rgb3);
                    suppDGV[0, row].Value = dr.IsNull("Completed") ? "" : dr["Completed"].ToString();
                    suppDGV[1, row].Value = suppCode;
                    suppDGV[2, row].Value = suppName;
                    suppDGV[3, row].Value = shortname;
                    suppDGV[4, row].Value = dr.IsNull("NumJobs") ? "" : Convert.ToDouble(dr["NumJobs"]).ToString("N0");
                    suppDGV[5, row].Value = dr.IsNull("BeamLM") ? "" : Convert.ToDouble(dr["BeamLM"]).ToString("N0");
                    suppDGV[6, row].Value = dr.IsNull("BeamM2") ? "" : Convert.ToDouble(dr["BeamM2"]).ToString("N0");
                    suppDGV[7, row++].Value = dr.IsNull("SlabM2") ? "" : Convert.ToDouble(dr["SlabM2"]).ToString("N0");
                    progressBar1.Value = row;
                    //suppDGV[8, row++].Value = dr.IsNull("TotalM2") ? "" : Convert.ToDouble(dr["TotalM2"]).ToString("N0");

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

        private void SupplierJobsSummaryForm_Load(object sender, EventArgs e)
        {
            this.Text = "Summary of Suppliers and their Jobs";
            progressBar1.Minimum = 0;
            BuildSuppliersDGV();
            PopulateDGV();
            return;
        }
    }
}
