using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace MCPApp
{
    public class ExcelUtlity
    {
        /// <summary>
        /// FUNCTION FOR EXPORT TO EXCEL
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="worksheetName"></param>
        /// <param name="saveAsLocation"></param>
        /// <returns></returns>
        /// 


        public bool WriteDataTableToExcelQuick(DataTable dataTable, string worksheetName, string saveAsLocation, string reportType, int dateColumnIndex)
        {
            Microsoft.Office.Interop.Excel.Application excel = null;
            Microsoft.Office.Interop.Excel.Workbook workbook = null;
            Microsoft.Office.Interop.Excel.Worksheet sheet = null;
            Microsoft.Office.Interop.Excel.Range range = null;

            try
            {
                excel = new Microsoft.Office.Interop.Excel.Application
                {
                    Visible = false,
                    DisplayAlerts = false
                };

                workbook = excel.Workbooks.Add(Type.Missing);
                sheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.ActiveSheet;
                sheet.Name = worksheetName;

                //var allColsRange = sheet.UsedRange;
                //allColsRange.WrapText = false;


                // Title - top row
                var titleRange = sheet.Range["A1", "I1"];
                titleRange.Merge();
                titleRange.Value = $"{reportType} report ran on {DateTime.Now:G}";
                titleRange.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.DarkBlue);
                titleRange.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);
                titleRange.Font.Bold = true;

                int rows = dataTable.Rows.Count;
                int cols = dataTable.Columns.Count;

                // ✅ Create a 2D array for bulk write
                object[,] data = new object[rows + 1, cols];

                // Headers
                for (int c = 0; c < cols; c++)
                    data[0, c] = dataTable.Columns[c].ColumnName;

                // Data
                for (int r = 0; r < rows; r++)
                    for (int c = 0; c < cols; c++)
                        data[r + 1, c] = dataTable.Rows[r][c];

                // ✅ Write all at once
                var startCell = (Microsoft.Office.Interop.Excel.Range)sheet.Cells[2, 1];
                var endCell = (Microsoft.Office.Interop.Excel.Range)sheet.Cells[rows + 2, cols];
                range = sheet.Range[startCell, endCell];
                range.Value2 = data;
                range.WrapText = false;

                // Format header
                var headerRange = sheet.Range[sheet.Cells[2, 1], sheet.Cells[2, cols]];
                headerRange.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.DarkBlue);
                headerRange.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);
                headerRange.Font.Bold = true;

                Microsoft.Office.Interop.Excel.Range freezePoint = (Microsoft.Office.Interop.Excel.Range)sheet.Cells[3, 1]; // Cell A3 → everything above this row will freeze
                freezePoint.Activate();
                excel.ActiveWindow.FreezePanes = true;

                // Format columns
                range.EntireColumn.AutoFit();

                // Format date columns
                var dateRange1 = sheet.Range[sheet.Cells[3, dateColumnIndex], sheet.Cells[rows + 2, dateColumnIndex]];
                dateRange1.NumberFormat = "DD/MM/YYYY";
                var dateRange2 = sheet.Range[sheet.Cells[3, dateColumnIndex + 1], sheet.Cells[rows + 2, dateColumnIndex + 1]];
                dateRange2.NumberFormat = "DD/MM/YYYY";

                workbook.SaveAs(saveAsLocation);
                workbook.Close();
                excel.Quit();

                return true;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                return false;
            }
            finally
            {
                sheet = null;
                range = null;
                workbook = null;
                excel = null;
                GC.Collect();
            }
        }

        public bool WriteDataTableToExcel(System.Data.DataTable dataTable, string worksheetName, string saveAsLocation, string ReporType,int dateColumnIndex)
        {
            Microsoft.Office.Interop.Excel.Application excel;
            Microsoft.Office.Interop.Excel.Workbook excelworkBook;
            Microsoft.Office.Interop.Excel.Worksheet excelSheet;
            Microsoft.Office.Interop.Excel.Range excelCellrange;
            Microsoft.Office.Interop.Excel.Range excelDesignDateCellrange;
            Microsoft.Office.Interop.Excel.Range excelReqDateCellrange;

            try
            {
                // Start Excel and get Application object.
                excel = new Microsoft.Office.Interop.Excel.Application();

                // for making Excel visible
                excel.Visible = false;
                excel.DisplayAlerts = false;

                // Creation a new Workbook
                excelworkBook = excel.Workbooks.Add(Type.Missing);

                // Workk sheet
                excelSheet = (Microsoft.Office.Interop.Excel.Worksheet)excelworkBook.ActiveSheet;
                excelSheet.Name = worksheetName;


                excelSheet.Cells[1, 1] = ReporType;
                excelSheet.Cells[1, 2] = "Date : " + DateTime.Now.ToShortDateString();

                // loop through each row and add values to our sheet
                int rowcount = 2;

                foreach (DataRow datarow in dataTable.Rows)
                {
                    rowcount += 1;
                    for (int i = 1; i <= dataTable.Columns.Count; i++)
                    {
                        // on the first iteration we add the column headers
                        if (rowcount == 3)
                        {
                            excelSheet.Cells[2, i] = dataTable.Columns[i - 1].ColumnName;
                            excelSheet.Cells.Font.Color = System.Drawing.Color.Black;

                        }

                        excelSheet.Cells[rowcount, i] = datarow[i - 1].ToString();

                        //for alternate rows
                        if (rowcount > 3)
                        {
                            if (i == dataTable.Columns.Count)
                            {
                                if (rowcount % 2 == 0)
                                {
                                    excelCellrange = excelSheet.Range[excelSheet.Cells[rowcount, 1], excelSheet.Cells[rowcount, dataTable.Columns.Count]];
                                    FormattingExcelCells(excelCellrange, "#CCCCFF", System.Drawing.Color.Black, false);
                                }

                            }
                        }

                    }

                }

                // now we resize the columns
                excelCellrange = excelSheet.Range[excelSheet.Cells[1, 1], excelSheet.Cells[rowcount, dataTable.Columns.Count]];
                excelCellrange.EntireColumn.WrapText = false;
                excelCellrange.EntireColumn.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                excelCellrange.EntireColumn.AutoFit();
                excelDesignDateCellrange = excelSheet.Range[excelSheet.Cells[1, dateColumnIndex], excelSheet.Cells[rowcount, dateColumnIndex]];
                excelDesignDateCellrange.EntireColumn.NumberFormat = "DD/MM/YYYY";
                excelReqDateCellrange = excelSheet.Range[excelSheet.Cells[1, dateColumnIndex + 1], excelSheet.Cells[rowcount, dateColumnIndex + 1]];
                excelReqDateCellrange.EntireColumn.NumberFormat = "DD/MM/YYYY";
                Microsoft.Office.Interop.Excel.Borders border = excelCellrange.Borders;
                border.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                border.Weight = 2d;


                excelCellrange = excelSheet.Range[excelSheet.Cells[1, 1], excelSheet.Cells[2, dataTable.Columns.Count]];
                FormattingExcelCells(excelCellrange, "#000099", System.Drawing.Color.White, true);


                //now save the workbook and exit Excel


                excelworkBook.SaveAs(saveAsLocation); ;
                excelworkBook.Close();
                excel.Quit();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            finally
            {
                excelSheet = null;
                excelCellrange = null;
                excelworkBook = null;
            }

        }

        public bool WriteDataTableToExcel(System.Data.DataTable dataTable, string worksheetName, string saveAsLocation, string ReporType, int dateColumnIndex1, int dateColumnIndex2)
        {
            Microsoft.Office.Interop.Excel.Application excel;
            Microsoft.Office.Interop.Excel.Workbook excelworkBook;
            Microsoft.Office.Interop.Excel.Worksheet excelSheet;
            Microsoft.Office.Interop.Excel.Range excelCellrange;
            Microsoft.Office.Interop.Excel.Range excel1stDateCellrange;
            Microsoft.Office.Interop.Excel.Range excel2ndDateCellrange;

            try
            {
                // Start Excel and get Application object.
                excel = new Microsoft.Office.Interop.Excel.Application();

                // for making Excel visible
                excel.Visible = false;
                excel.DisplayAlerts = false;

                // Creation a new Workbook
                excelworkBook = excel.Workbooks.Add(Type.Missing);

                // Workk sheet
                excelSheet = (Microsoft.Office.Interop.Excel.Worksheet)excelworkBook.ActiveSheet;
                excelSheet.Name = worksheetName;


                excelSheet.Cells[1, 1] = ReporType;
                excelSheet.Cells[1, 2] = "Date : " + DateTime.Now.ToShortDateString();

                // loop through each row and add values to our sheet
                int rowcount = 2;

                foreach (DataRow datarow in dataTable.Rows)
                {
                    rowcount += 1;
                    for (int i = 1; i <= dataTable.Columns.Count; i++)
                    {
                        // on the first iteration we add the column headers
                        if (rowcount == 3)
                        {
                            excelSheet.Cells[2, i] = dataTable.Columns[i - 1].ColumnName;
                            excelSheet.Cells.Font.Color = System.Drawing.Color.Black;

                        }

                        excelSheet.Cells[rowcount, i] = datarow[i - 1].ToString();

                        //for alternate rows
                        if (rowcount > 3)
                        {
                            if (i == dataTable.Columns.Count)
                            {
                                if (rowcount % 2 == 0)
                                {
                                    excelCellrange = excelSheet.Range[excelSheet.Cells[rowcount, 1], excelSheet.Cells[rowcount, dataTable.Columns.Count]];
                                    FormattingExcelCells(excelCellrange, "#CCCCFF", System.Drawing.Color.Black, false);
                                }

                            }
                        }

                    }

                }

                // now we resize the columns
                excelCellrange = excelSheet.Range[excelSheet.Cells[1, 1], excelSheet.Cells[rowcount, dataTable.Columns.Count]];
                excelCellrange.EntireColumn.WrapText = false;
                excelCellrange.EntireColumn.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                excelCellrange.EntireColumn.AutoFit();
                excel1stDateCellrange = excelSheet.Range[excelSheet.Cells[1, dateColumnIndex1], excelSheet.Cells[rowcount, dateColumnIndex1]];
                excel1stDateCellrange.EntireColumn.NumberFormat = "DD/MM/YYYY";
                excel2ndDateCellrange = excelSheet.Range[excelSheet.Cells[1, dateColumnIndex2], excelSheet.Cells[rowcount, dateColumnIndex2]];
                excel2ndDateCellrange.EntireColumn.NumberFormat = "DD/MM/YYYY";
                Microsoft.Office.Interop.Excel.Borders border = excelCellrange.Borders;
                border.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                border.Weight = 2d;


                excelCellrange = excelSheet.Range[excelSheet.Cells[1, 1], excelSheet.Cells[2, dataTable.Columns.Count]];
                FormattingExcelCells(excelCellrange, "#000099", System.Drawing.Color.White, true);


                //now save the workbook and exit Excel


                excelworkBook.SaveAs(saveAsLocation); ;
                excelworkBook.Close();
                excel.Quit();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            finally
            {
                excelSheet = null;
                excelCellrange = null;
                excelworkBook = null;
            }

        }



        /// <summary>
        /// FUNCTION FOR FORMATTING EXCEL CELLS
        /// </summary>
        /// <param name="range"></param>
        /// <param name="HTMLcolorCode"></param>
        /// <param name="fontColor"></param>
        /// <param name="IsFontbool"></param>
        public void FormattingExcelCells(Microsoft.Office.Interop.Excel.Range range, string HTMLcolorCode, System.Drawing.Color fontColor, bool IsFontbool)
        {
            range.Interior.Color = System.Drawing.ColorTranslator.FromHtml(HTMLcolorCode);
            range.Font.Color = System.Drawing.ColorTranslator.ToOle(fontColor);
            if (IsFontbool == true)
            {
                range.Font.Bold = IsFontbool;
            }
        }
    }
}
