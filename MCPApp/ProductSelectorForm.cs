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
    public partial class ProductSelectorForm : Form
    {
        MeltonData mcData = new MeltonData();

        private string product = "";
        public string Product
        {
            get
            {
                return product;
            }
            set
            {
                product = value;
            }
        }

        public ProductSelectorForm()
        {
            InitializeComponent();
        }

        private void ProductSelectorForm_Load(object sender, EventArgs e)
        {
            this.Text = "Products";
            BuildList();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        private void BuildList()
        {
            try
            {
                listBox1.Items.Clear();
                DataTable dt = mcData.GetAllProducts();
                if (dt.Rows.Count < 1) { return; }

                foreach (DataRow dr in dt.Rows)
                {
                    listBox1.Items.Add(dr["product"].ToString());
                }
               


            }
            catch (Exception ex)
            {

                MessageBox.Show("BuildList() ERROR - " + ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.TargetSite);
                string audit = mcData.CreateErrorAudit("ProductSelectorForm.cs", "BuildList()", ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.TargetSite);
                return;
            }
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            if (!listBox1.Focused) { return; }
            product = listBox1.SelectedItem.ToString();
            this.Dispose();
            this.Close();

        }
    }
}
