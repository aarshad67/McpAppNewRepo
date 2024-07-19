using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MCPApp
{
    public partial class EditCompletedJobDetailForm : Form
    {
        private static string _jobNo = "";
        private static DateTime reqDate = DateTime.MinValue;
        private static string site = "";
        private static string suppType = "";
        private static string product = "";
        private static string supplier = "";
        private static string _suppCode = "";
        private static int beamLm = 0;
        private static int beamM2 = 0;
        private static int slabM2 = 0;
        private static int totalM2 = 0;

        private string _supplierShortname = "";
        public string SupplierShortname
        {
            get
            {
                return _supplierShortname;
            }
            set
            {
                _supplierShortname = value;
            }
        }

        private int _wbTotalM2 = 0;
        public int WbTotalM2
        {
            get
            {
                return _wbTotalM2;
            }
            set
            {
                _wbTotalM2 = value;
            }
        }

        private bool _isUpdated = false;
        public bool IsUpdated
        {
            get
            {
                return _isUpdated;
            }
            set
            {
                _isUpdated = value;
            }
        }

        MeltonData mcData = new MeltonData();

        public EditCompletedJobDetailForm()
        {
            InitializeComponent();
        }

        public EditCompletedJobDetailForm(string job)
        {
            InitializeComponent();
            _jobNo = job;
            mcData.GetKeyWhiteboardDetails(_jobNo, out reqDate, out site, out suppType, out product, out supplier,out beamLm, out beamM2, out slabM2, out totalM2);
            _suppCode = mcData.GetSupplierCodeFromShortname(supplier);
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        private int ReCalculateTotalM2()
        {
            int beamM2 = Convert.ToInt32(txtBeamM2.Text);
            int slabM2 = Convert.ToInt32(txtSlabM2.Text);

            if(beamM2 > 0) { return beamM2; }
            if(slabM2 > 0) { return slabM2; }
            return 0;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int totalM2 = ReCalculateTotalM2();
            int rgb1, rgb2, rgb3 = 0;

            mcData.GetSupplierColourByShortname(lblSupplier.Text, out rgb1, out rgb2, out rgb3);
            string err1 = mcData.UpdateWhiteBoardSupplierShortName(_jobNo, lblSupplier.Text, rgb1, rgb2, rgb3);
            string err2 = mcData.UpdateJobPlannerSupplierShortName(_jobNo, lblSupplier.Text);
            string err3 = mcData.UpdateWhiteBoardJobQty(_jobNo, totalM2);
            string err4 = mcData.UpdateJobPlannerQty(_jobNo, Convert.ToInt32(txtBeamLM.Text), Convert.ToInt32(txtBeamM2.Text), Convert.ToInt32(txtSlabM2.Text));
            string message = "";
            StringBuilder sb = new StringBuilder();
            if (err1 =="OK" && err2 =="OK" && err3 =="OK" && err4 == "OK")
            {
                message = $"Completed job [{_jobNo}] has been updated as follows : {Environment.NewLine}{Environment.NewLine}" +
                    $"Supplier : {lblSupplier.Text}{Environment.NewLine}" +
                    $"Beam LM : {txtBeamLM.Text}{Environment.NewLine}" +
                    $"Beam M² : {txtBeamM2.Text}{Environment.NewLine}" +
                    $"Slab M² : {txtSlabM2.Text}{Environment.NewLine}" + 
                    $"Total M² : {totalM2.ToString()}{Environment.NewLine}";
                _supplierShortname = lblSupplier.Text;
                _wbTotalM2 = totalM2;
                _isUpdated = true;
            }
            else
            {
                if(err1 != "OK")
                {
                    sb.Append($"UpdateWhiteBoardSupplierShortName({_jobNo}, {lblSupplier.Text}) ERROR : {err1} " + Environment.NewLine);
                }
                if (err2 != "OK")
                {
                    sb.Append($"UpdateJobPlannerSupplierShortName({_jobNo}, {lblSupplier.Text}) ERROR : {err2} " + Environment.NewLine);
                }
                if (err3 != "OK")
                {
                    sb.Append($"UpdateWhiteBoardJobQty({_jobNo}, {lblTotalM2.Text}) ERROR : {err3} " + Environment.NewLine);
                }
                if (err4 != "OK")
                {
                    sb.Append($"UpdateJobPlannerQty({_jobNo}, {txtBeamLM.Text},{txtBeamM2.Text},{txtSlabM2.Text}) ERROR : {err4} " + Environment.NewLine);
                }
                message = $"Error(s) in updating Completed job [{_jobNo}] : {Environment.NewLine}{Environment.NewLine}" + sb.ToString();
            }
            MessageBox.Show(message);
            this.Dispose();
            this.Close();
            return;
            
        }

        private void EditCompletedJobDetailForm_Load(object sender, EventArgs e)
        {
            this.Text = $"Completed Job No {_jobNo} - Edit QUANTITIES or SUPPLIER only";
            lblRequiredDate.Text = reqDate.ToString("ddd,dd MMM yyyy");
            lblSiteAddress.Text = Regex.Replace(site, @"\r\n?|\n", ""); 
            lblProduct.Text = product;
            lblSupplier.Text = supplier;
            lblSupplyType.Text = suppType;
            lblTotalM2.Text = totalM2.ToString();
            txtBeamLM.Text = beamLm.ToString();
            txtBeamM2.Text = beamM2.ToString();
            txtSlabM2.Text = slabM2.ToString();
            
        }

        private void lblSupplier_Click(object sender, EventArgs e)
        {
            SuppliersListForm suppForm = new SuppliersListForm(_suppCode);
            suppForm.ShowDialog();
            lblSupplier.Text = suppForm.Shortname;
            suppForm.Dispose();
            suppForm.Close();
            return;
        }
    }
}
