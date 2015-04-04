using System;
using System.Reflection;
using System.Windows.Forms;
using EFML_Designer.Properties;

namespace EFML_Designer
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new MainView(args));
        }
    }
}
