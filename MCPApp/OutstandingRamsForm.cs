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
    public partial class OutstandingRamsForm : Form
    {
        MeltonData mcData = new MeltonData();
        Logger logger = new Logger();
        int rowIndex, colIndex = 0;

        public OutstandingRamsForm()
        {
            InitializeComponent();
        }

        private void OutstandingRamsForm_Load(object sender, EventArgs e)
        {
            this.Text = "Jobs with Outstanding PO(s) / RAM(s) sent";
            BuildDGV();
            PopulateDGV();

        }

        private void BuildDGV()
        {
            try
            {
               

                DataGridViewTextBoxColumn quoteNoColumn = new DataGridViewTextBoxColumn();//0
                quoteNoColumn.DataPropertyName = "JobNo";
                quoteNoColumn.HeaderText = "Job No.";
                quoteNoColumn.Width = 90;
                quoteNoColumn.ReadOnly = true;
                quoteNoColumn.Frozen = true;
                quoteNoColumn.DefaultCellStyle.ForeColor = Color.Yellow;
                quoteNoColumn.DefaultCellStyle.BackColor = Color.Black;
                jobDGV.Columns.Add(quoteNoColumn);

                DataGridViewTextBoxColumn dateColumn = new DataGridViewTextBoxColumn();//1
                dateColumn.DataPropertyName = "date";
                dateColumn.HeaderText = "Required Date";
                dateColumn.Width = 120;
                dateColumn.ReadOnly = true;
                dateColumn.Frozen = true;
                dateColumn.DefaultCellStyle.BackColor = Color.Yellow;
                dateColumn.DefaultCellStyle.ForeColor = Color.Blue;
                jobDGV.Columns.Add(dateColumn);

                DataGridViewTextBoxColumn siteColumn = new DataGridViewTextBoxColumn();//2
                siteColumn.DataPropertyName = "site";
                siteColumn.HeaderText = "Site";
                siteColumn.Width = 250;
                siteColumn.ReadOnly = true;
                siteColumn.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                jobDGV.Columns.Add(siteColumn);
               
                DataGridViewTextBoxColumn productsColumn = new DataGridViewTextBoxColumn();//3
                productsColumn.DataPropertyName = "products";
                productsColumn.HeaderText = "Products";
                productsColumn.Width = 80;
                productsColumn.ReadOnly = true;
                jobDGV.Columns.Add(productsColumn);

                DataGridViewTextBoxColumn totalM2Column = new DataGridViewTextBoxColumn();//4
                totalM2Column.DataPropertyName = "totalM2";
                totalM2Column.HeaderText = "Total M²";
                totalM2Column.Width = 80;
                totalM2Column.ReadOnly = true;
                jobDGV.Columns.Add(totalM2Column);

                DataGridViewTextBoxColumn productSupplierColumn = new DataGridViewTextBoxColumn();//5
                productSupplierColumn.DataPropertyName = "productSupplier";
                productSupplierColumn.HeaderText = "Product Supplier";
                productSupplierColumn.Width = 70;
                productSupplierColumn.ReadOnly = true;
                jobDGV.Columns.Add(productSupplierColumn);

                DataGridViewTextBoxColumn stairsSupplierColumn = new DataGridViewTextBoxColumn();//6
                stairsSupplierColumn.DataPropertyName = "stairsSupplier";
                stairsSupplierColumn.HeaderText = "Stairs Supplier";
                stairsSupplierColumn.Width = 70;
                stairsSupplierColumn.ReadOnly = true;
                jobDGV.Columns.Add(stairsSupplierColumn);

                DataGridViewTextBoxColumn ramSentColumn = new DataGridViewTextBoxColumn();//7
                ramSentColumn.DataPropertyName = "ramSentDate";
                ramSentColumn.HeaderText = "Date RAM Sent";
                ramSentColumn.Width = 150;
                ramSentColumn.ReadOnly = true;
                jobDGV.Columns.Add(ramSentColumn);

                DataGridViewTextBoxColumn ramSentByColumn = new DataGridViewTextBoxColumn();//8
                ramSentByColumn.DataPropertyName = "ramSentBy";
                ramSentByColumn.HeaderText = "RAM sent by";
                ramSentByColumn.Width = 50;
                ramSentByColumn.ReadOnly = true;
                jobDGV.Columns.Add(ramSentByColumn);

                DataGridViewTextBoxColumn poNoColumn = new DataGridViewTextBoxColumn();//9
                poNoColumn.DataPropertyName = "poNo";
                poNoColumn.HeaderText = "PO Number";
                poNoColumn.Width = 70;
                poNoColumn.ReadOnly = false;
                jobDGV.Columns.Add(poNoColumn);

                DataGridViewTextBoxColumn poDetailsColumn = new DataGridViewTextBoxColumn();//10
                poDetailsColumn.DataPropertyName = "poDetails";
                poDetailsColumn.HeaderText = "PO Details";
                poDetailsColumn.Width = 150;
                poDetailsColumn.DefaultCellStyle.ForeColor = Color.Blue;
                poDetailsColumn.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                poDetailsColumn.ReadOnly = false;
                jobDGV.Columns.Add(poDetailsColumn);

                DataGridViewTextBoxColumn poDateColumn = new DataGridViewTextBoxColumn();//11
                poDateColumn.DataPropertyName = "poDate";
                poDateColumn.HeaderText = "PO Date";
                poDateColumn.Width = 70;
                poDateColumn.ReadOnly = true;
                jobDGV.Columns.Add(poDateColumn);

                DataGridViewTextBoxColumn poByColumn = new DataGridViewTextBoxColumn();//12
                poByColumn.DataPropertyName = "poBy";
                poByColumn.HeaderText = "PO Raised By";
                poByColumn.Width = 70;
                poByColumn.ReadOnly = true;
                jobDGV.Columns.Add(poByColumn);



                jobDGV.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                jobDGV.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                jobDGV.SelectionMode = DataGridViewSelectionMode.CellSelect;
                jobDGV.ScrollBars = System.Windows.Forms.ScrollBars.Both;
                
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show("BuildDGV() ERROR - " + ex.Message.ToString());
                string audit = mcData.CreateErrorAudit("OutstandingRamsForm.cs", "BuildDGV()", ex.Message.ToString());
                return;
            }

        }

        private void PopulateDGV()
        {
            int row = 0;

            int rgb1 = 255;
            int rgb2 = 255;
            int rgb3 = 255;
            int srgb1 = 255;
            int srgb2 = 255;
            int srgb3 = 255;
            string suppShortname = "";
            string stairsSupplier = "";

            jobDGV.Rows.Clear();

            DataTable dt = mcData.GetAllJobPurchaseOrders();
            if (dt == null)
            {
                return;
            }
            if (dt.Rows.Count < 1)
            {
                return;
            }

            foreach (DataRow dr in dt.Rows)
            {
                suppShortname = dr["suppShortname"].ToString();
                stairsSupplier = dr["stairsSupplier"].ToString();
                string po = dr["poNo"] == null ? "" : dr["poNo"].ToString();
                mcData.GetSupplierColourByShortname(suppShortname, out rgb1, out rgb2, out rgb3);
                mcData.GetSupplierColourByShortname(stairsSupplier, out srgb1, out srgb2, out srgb3);
                jobDGV.Rows.Add();
                jobDGV[0, row].Value = dr["jobNo"].ToString();
                jobDGV.Rows[row].ReadOnly = String.IsNullOrWhiteSpace(po) ? false : true;
                jobDGV[1, row].Value = Convert.ToDateTime(dr["requiredDate"].ToString()).ToShortDateString();
                jobDGV[2, row].Value = dr["siteAddress"].ToString();
                jobDGV[3, row].Value = dr["products"].ToString();
                jobDGV[4, row].Value = Convert.ToInt32(dr["totalM2"].ToString());
                jobDGV[5, row].Value = dr["suppShortname"].ToString();
                jobDGV[5, row].Style.BackColor = Color.FromArgb(rgb1, rgb2, rgb3);
                jobDGV[6, row].Value = dr["stairsSupplier"].ToString();
                jobDGV[6, row].Style.BackColor = Color.FromArgb(srgb1, srgb2, srgb3);
                jobDGV[7, row].Value = Convert.ToDateTime(dr["ramSentDate"].ToString()).ToString();
                jobDGV[8, row].Value = dr["ramSentBy"].ToString();
                jobDGV[9, row].Value = dr["poNo"] == null ? "" : dr["poNo"].ToString();
                jobDGV[10, row].Value = dr["poDetails"] == null ? "" : dr["poDetails"].ToString();
                jobDGV[11, row].Value = dr["poRaisedDate"] == DBNull.Value ? "" : Convert.ToDateTime(dr["poRaisedDate"].ToString()).ToShortDateString();
                jobDGV[12, row++].Value = dr["poRaisedBy"] == null ? "" : dr["poRaisedBy"].ToString();
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        private void raisePOToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!jobDGV.Focused) { return; }
            if (jobDGV[0, rowIndex].Value == null) { return; }
            string jobNo = jobDGV[0, this.rowIndex].Value.ToString();
            RaisePOForm poForm = new RaisePOForm(jobNo);
            poForm.ShowDialog();
            PopulateDGV();
            return;
            
        }

        private void jobDGV_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left && e.RowIndex != -1) // added this and it now works
            {
                this.rowIndex = e.RowIndex;
                this.colIndex = e.ColumnIndex;
                this.jobDGV = (DataGridView)sender;
                return;
            }

            if (e.Button == System.Windows.Forms.MouseButtons.Right && e.RowIndex != -1)
            {
                this.rowIndex = e.RowIndex;
                this.colIndex = e.ColumnIndex;
                this.jobDGV = (DataGridView)sender;
                this.contextMenuStrip1.Show(this.jobDGV, e.Location);
                contextMenuStrip1.Show(Cursor.Position);
                return; ;
            }
        }
    }
}
