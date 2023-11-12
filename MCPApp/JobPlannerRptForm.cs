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
    public partial class JobPlannerRptForm : Form
    {
        MeltonData mcData = new MeltonData();
        ExcelUtlity excel = new ExcelUtlity();


        private DataTable jobDT;
        public DataTable JobDT
        {
            get
            {
                return jobDT;
            }
            set
            {
                jobDT = value;
            }
        }


        public JobPlannerRptForm()
        {
            InitializeComponent();
        }

        public JobPlannerRptForm(DataTable jobPlannerDT)
        {
            InitializeComponent();
            jobDT = jobPlannerDT;
        }

        private void SelectBFolderButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            folderDlg.ShowNewFolderButton = true;
            // Show the FolderBrowserDialog.  
            DialogResult result = folderDlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                pathTextBox.Text = folderDlg.SelectedPath;
                Environment.SpecialFolder root = folderDlg.RootFolder;
            }  
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        private void JobPlannerRptForm_Load(object sender, EventArgs e)
        {
            this.Text = "JOBPLANNER Report";
        }

        private void OpenLocationFolder()
        {
            try
            {
                System.Diagnostics.Process.Start("explorer.exe", (String)pathTextBox.Text);
            }
            catch (Win32Exception win32Exception)
            {
                //The system cannot find the file specified...
                Console.WriteLine(String.Format("OpenLocationFolder() ERROR - {0}", win32Exception.Message));
            }
        }

        private void GenerateButton_Click(object sender, EventArgs e)
        {
            string rptName = "JOBPLANNER";
            string fullRptName = rptName + "_" + DateTime.Now.ToString("ddMMMyyyyhhmmss") + ".xlsx";
            string fullFilePath = System.IO.Path.Combine(pathTextBox.Text, fullRptName);
            this.Cursor = Cursors.WaitCursor;
            excel.WriteDataTableToExcel(jobDT, rptName, fullFilePath, rptName, 5);
            this.Cursor = Cursors.Default;

            string message = "Report(s) successfully created in [" + pathTextBox.Text + "] location." + Environment.NewLine + "" + Environment.NewLine + "Do you wish to check the excel file(s) now?";
            if (MessageBox.Show(message, "Open Excel Files ? ", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                OpenLocationFolder();
            }
        }
    }
}
