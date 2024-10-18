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
    public partial class DatesOutOfSyncForm : Form
    {
        public DatesOutOfSyncForm()
        {
            InitializeComponent();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        private void DatesOutOfSyncForm_Load(object sender, EventArgs e)
        {

        }

        private void BuildDGV()
        {
            try
            {
                myDGV.Columns.Clear();
                //0
                DataGridViewTextBoxColumn jobColumn = new DataGridViewTextBoxColumn();
                jobColumn.HeaderText = "Job No";
                jobColumn.Width = 70;
                jobColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                jobColumn.ReadOnly = true;
                myDGV.Columns.Add(jobColumn);

                //1
                DataGridViewTextBoxColumn jpDateColumn = new DataGridViewTextBoxColumn();
                jpDateColumn.HeaderText = "JP Date";
                jpDateColumn.Width = 70;
                jpDateColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                jpDateColumn.ReadOnly = true;
                myDGV.Columns.Add(jpDateColumn);

                //2
                DataGridViewTextBoxColumn wbDateColumn = new DataGridViewTextBoxColumn();
                wbDateColumn.HeaderText = "WB Date";
                wbDateColumn.Width = 70;
                wbDateColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                wbDateColumn.ReadOnly = true;
                myDGV.Columns.Add(wbDateColumn);

                //3
                DataGridViewTextBoxColumn siteAddressColumn = new DataGridViewTextBoxColumn();
                siteAddressColumn.HeaderText = "Site Address";
                siteAddressColumn.Width = 300;
                siteAddressColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                siteAddressColumn.ReadOnly = true;
                myDGV.Columns.Add(siteAddressColumn);

                //3
                DataGridViewTextBoxColumn customerColumn = new DataGridViewTextBoxColumn();
                customerColumn.HeaderText = "Customer";
                customerColumn.Width = 80;
                customerColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                customerColumn.ReadOnly = true;
                myDGV.Columns.Add(customerColumn);

                myDGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                myDGV.EnableHeadersVisualStyles = false;


            }
            catch (Exception ex)
            {
                MeltonData mcData = new MeltonData();
                MessageBox.Show("BuildDGV ERROR - " + ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.TargetSite);
                string audit = mcData.CreateErrorAudit("DatesOutOfSyncForm.cs", "BuildDGV()", ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.TargetSite);
                return;
            }
        }

        private void PopulateDGV()
        {
            //MeltonData mcData = new MeltonData();
            //int row = 0;
            //int rgb1, rgb2, rgb3 = 255;
            //try
            //{
            //    myDGV.Rows.Clear();
            //    System.Data.DataTable dt = mcData.GetAllSuppliers();
            //    foreach (DataRow dr in dt.Rows)
            //    {
            //        rgb1 = dr["rgb1"] == null ? 255 : Convert.ToInt16(dr["rgb1"].ToString());
            //        rgb2 = dr["rgb2"] == null ? 255 : Convert.ToInt16(dr["rgb2"].ToString());
            //        rgb3 = dr["rgb3"] == null ? 255 : Convert.ToInt16(dr["rgb3"].ToString());
            //        suppDGV.Rows.Add();
            //        suppDGV[0, row].Style.BackColor = Color.FromArgb(rgb1, rgb2, rgb3);
            //        suppDGV[0, row].Value = dr["suppCode"].ToString();
            //        suppDGV[1, row].Value = dr["suppName"].ToString();
            //        suppDGV[2, row].Value = dr["shortname"].ToString();
            //        suppDGV[3, row++].Value = dr["productType"].ToString();

            //    }
            //    suppDGV.CurrentCell = suppDGV.Rows[0].Cells[0];
            //    return;
            //}
            //catch (Exception ex)
            //{
            //    string msg = String.Format("PopulateDGV Error : {0}", ex.Message);
            //    MessageBox.Show(msg);
            //    logger.LogLine(msg);
            //    string audit = mcData.CreateErrorAudit("SuppliersListForm.cs", "PopulateDGV()", ex.Message);
            //    return;

            //}


        }
    }
}
