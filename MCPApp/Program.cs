using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

namespace MCPApp
{
    static class Program 
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            MeltonData mcData = new MeltonData();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            LoginForm loginForm = new LoginForm();
            Application.Run(new LoginForm());
            string mcpUserID = ConfigurationManager.AppSettings["LoggedInUser"];
            string mcpUserFullName = ConfigurationManager.AppSettings["LoggedInUserFullName"];
            if (mcpUserID.Length > 0 && mcpUserFullName.Length > 0)
            {
                try
                {
                    Application.Run(new UserOverviewForm(mcpUserID, mcpUserFullName));
                    return;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    string audit = mcData.CreateErrorAudit("Program.cs", "Main()", ex.Message);
                    throw;
                }


            }
            else
            {
                return;
            }
        }

        
    }
}
