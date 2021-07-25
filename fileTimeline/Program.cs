using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace fileTimeline
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string[] commandLineArgs = Environment.GetCommandLineArgs();
            if (commandLineArgs.Length != 2)
            {
                MessageBox.Show("Invalid call!");
                Environment.Exit(1);
            }

            DirectoryInfo di = new DirectoryInfo(commandLineArgs[1]);
            if (!di.Exists)
            {
                MessageBox.Show(String.Format("{0} not found!", di.FullName));
                Environment.Exit(2);
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1(di));
        }
    }
}
