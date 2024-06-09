using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MCPApp
{
    public partial class excelTestForm : Form
    {
        private object missing = Type.Missing;
        private string fileName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
                                    + "\\SoSample.xlsx";
        public excelTestForm()
        {
            InitializeComponent();
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        private void GenerateButton_Click(object sender, EventArgs e)
        {
            RunRepport();
            System.Diagnostics.Process.Start(fileName);
        }

        public void RunRepport()
        {
            Microsoft.Office.Interop.Excel.Application oXL = new Microsoft.Office.Interop.Excel.Application();
            oXL.Visible = false;
            Microsoft.Office.Interop.Excel.Workbook oWB = oXL.Workbooks.Add(missing);
            Microsoft.Office.Interop.Excel.Worksheet oSheet = oWB.ActiveSheet as Microsoft.Office.Interop.Excel.Worksheet;
            oSheet.Name = "The first sheet";
            oSheet.Cells[1, 1] = "Something";
            Microsoft.Office.Interop.Excel.Worksheet oSheet2 = oWB.Sheets.Add(missing, missing, 1, missing)
                            as Microsoft.Office.Interop.Excel.Worksheet;
            oSheet2.Name = "The second sheet";
            oSheet2.Cells[1, 1] = "Something completely different";
            
            oWB.SaveAs(fileName, Microsoft.Office.Interop.Excel.XlFileFormat.xlOpenXMLWorkbook,
                missing, missing, missing, missing,
                Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange,
                missing, missing, missing, missing, missing);
            oWB.Close(missing, missing, missing);
            oXL.UserControl = true;
            oXL.Quit();
        }
    }
}
