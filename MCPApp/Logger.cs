using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;
using System.Configuration;

namespace MCPApp
{
    public class Logger
    {
        //MeltonData mcData = new MeltonData();
        public void LogLine(string message)
        {
            try
            {
                string loggedInUser = ConfigurationManager.AppSettings["LoggedInUser"];
                string dd = DateTime.Today.Day < 10 ? "0" + DateTime.Today.Day.ToString() : DateTime.Today.Day.ToString();
                string monthName = DateTime.Today.ToString("MMM", CultureInfo.InvariantCulture);
                string yyyy = DateTime.Today.Year.ToString();
                string logFileName = Path.Combine(Path.GetTempPath(), "MCP_Error_" + loggedInUser.ToUpper() + "_" + dd + monthName.ToUpper() + yyyy + ".txt");
                using (StreamWriter sw = new StreamWriter(logFileName, true))
                {
                    sw.WriteLine(DateTime.Now.ToLongTimeString() + " - " + message);
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message.ToString();
               // string audit = mcData.CreateErrorAudit("Logger.cs", "LogLine(...)", msg);
                return;
            }
        }





    }
}
