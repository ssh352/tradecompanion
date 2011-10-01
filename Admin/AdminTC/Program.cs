using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AdminTC
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new frmAdmin());
                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Admin TC");
            }
        }
    }
}