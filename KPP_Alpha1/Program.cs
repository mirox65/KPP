﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new form_Main());
            /*if (LogForm.LogSuccess())
            {
                Application.Run(new form_Main());
            }
            else
            {
                Application.Exit();
            }*/
        }
    }
}
