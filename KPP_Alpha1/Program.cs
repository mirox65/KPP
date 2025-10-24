using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using KPP_Alpha1.HelperClasses;

namespace KPP_Alpha1
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Clean logs older than 365 days on startup (T021)
            LogHelper.CleanOldLogs(365);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            // TEMPORARY: Run helper tests for T016 and T020
            // TODO: Replace with normal login form after Phase 1 validation
            Application.Run(new HelperTests());
            
            // Original login flow (restore after tests complete):
            // Application.Run(new Form1());
        }
    }
}
