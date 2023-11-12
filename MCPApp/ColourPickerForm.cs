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
    public partial class ColourPickerForm : Form
    {
        MeltonData mcData = new MeltonData();

        private string colourCode = "";
        public string ColourCode
        {
            get
            {
                return colourCode;
            }
            set
            {
                colourCode = value;
            }
        }

        private int rgb1 = 0;
        public int RGB1
        {
            get
            {
                return rgb1;
            }
            set
            {
                rgb1 = value;
            }
        }

        private int rgb2 = 0;
        public int RGB2
        {
            get
            {
                return rgb2;
            }
            set
            {
                rgb2 = value;
            }
        }

        private int rgb3 = 0;
        public int RGB3
        {
            get
            {
                return rgb3;
            }
            set
            {
                rgb3 = value;
            }
        }

        

        public ColourPickerForm()
        {
            InitializeComponent();
        }

        private void ColourPickerForm_Load(object sender, EventArgs e)
        {
            this.Text = "Select your colour by double clicking on the row";
            BuildColoursDGV();
            PopulateDGV();
        }

        private void BuildColoursDGV()
        {
            try
            {


                colourDGV.Columns.Clear();

                //0
                DataGridViewTextBoxColumn colourBoxColumn = new DataGridViewTextBoxColumn();
                colourBoxColumn.HeaderText = "Colour";
                colourBoxColumn.Width = 100;
                colourBoxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                colourBoxColumn.ReadOnly = true;
                colourDGV.Columns.Add(colourBoxColumn);

                //1
                DataGridViewTextBoxColumn colourCodeBoxColumn = new DataGridViewTextBoxColumn();
                colourCodeBoxColumn.HeaderText = "Colour Code";
                colourCodeBoxColumn.Width = 100;
                colourCodeBoxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                colourCodeBoxColumn.ReadOnly = true;
                colourDGV.Columns.Add(colourCodeBoxColumn);

                //2
                DataGridViewTextBoxColumn colourDescTextBoxColumn = new DataGridViewTextBoxColumn();
                colourDescTextBoxColumn.HeaderText = "Colour Desc";
                colourDescTextBoxColumn.Width = 150;
                colourDescTextBoxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                colourDescTextBoxColumn.ReadOnly = true;
                colourDGV.Columns.Add(colourDescTextBoxColumn);

                //3
                DataGridViewTextBoxColumn rgb1TextBoxColumn = new DataGridViewTextBoxColumn();
                rgb1TextBoxColumn.HeaderText = "RGB 1";
                rgb1TextBoxColumn.Width = 50;
                rgb1TextBoxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                rgb1TextBoxColumn.ReadOnly = true;
                colourDGV.Columns.Add(rgb1TextBoxColumn);

                //4
                DataGridViewTextBoxColumn rgb2TextBoxColumn = new DataGridViewTextBoxColumn();
                rgb2TextBoxColumn.HeaderText = "RGB 2";
                rgb2TextBoxColumn.Width = 50;
                rgb2TextBoxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                rgb2TextBoxColumn.ReadOnly = true;
                colourDGV.Columns.Add(rgb2TextBoxColumn);

                //5
                DataGridViewTextBoxColumn rgb3TextBoxColumn = new DataGridViewTextBoxColumn();
                rgb3TextBoxColumn.HeaderText = "RGB 3";
                rgb3TextBoxColumn.Width = 50;
                rgb3TextBoxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                rgb3TextBoxColumn.ReadOnly = true;
                colourDGV.Columns.Add(rgb3TextBoxColumn);

                colourDGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                colourDGV.EnableHeadersVisualStyles = false;
                




            }
            catch (Exception ex)
            {
                string msg = "BuildColoursDGV ERROR - " + ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.TargetSite;
                string audit = mcData.CreateErrorAudit("ColourPickerForm.cs", "BuildColoursDGV", msg);
                MessageBox.Show(msg);
            }





        }

        private void PopulateDGV()
        {
            int row = 0;
            int rgb1, rgb2, rgb3 = 0;
            try
            {
                Color colour = new Color();

                colourDGV.Rows.Clear();
                DataTable dt = mcData.GetAllColours();
                foreach (DataRow dr in dt.Rows)
                {
                    rgb1 = Convert.ToInt16(dr["rgb1"].ToString());
                    rgb2 = Convert.ToInt16(dr["rgb2"].ToString());
                    rgb3 = Convert.ToInt16(dr["rgb3"].ToString());
                    colourDGV.Rows.Add();
                    colourDGV[0, row].Style.BackColor = Color.FromArgb(rgb1, rgb2, rgb3);
                    colourDGV[1, row].Value = dr["colourCode"].ToString();
                    colourDGV[2, row].Value = dr["colourDesc"].ToString();
                    colourDGV[3, row].Value = dr["rgb1"].ToString();
                    colourDGV[4, row].Value = dr["rgb2"].ToString();
                    colourDGV[5, row++].Value = dr["rgb3"].ToString();
                    
                }
                colourDGV.CurrentCell = colourDGV.Rows[0].Cells[0];

            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("PopulateDGV Error : {0}", ex.Message));
                string audit = mcData.CreateErrorAudit("ColourPickerForm.cs", "PopulateDGV()", ex.Message);
            }


        }

        private void colourDGV_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!colourDGV.Focused) { return; }

            if (colourDGV.Rows[e.RowIndex].Cells[1].Value == null) { return; }

            colourCode = colourDGV.Rows[e.RowIndex].Cells[1].Value.ToString();
            rgb1 = Convert.ToInt16(colourDGV.Rows[e.RowIndex].Cells[3].Value.ToString());
            rgb2 = Convert.ToInt16(colourDGV.Rows[e.RowIndex].Cells[4].Value.ToString());
            rgb3 = Convert.ToInt16(colourDGV.Rows[e.RowIndex].Cells[5].Value.ToString());
            this.Dispose();
            this.Close();

        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }
    }
}
