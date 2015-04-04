using System;
using System.IO;
using System.Windows.Forms;
using Creek.UI.EFML;
using EFML.Properties;

namespace EFML
{
    internal static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        public static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var f = string.Join(" ", args);
            
            Form b = EfmlForm.Build(File.ReadAllText(f));

            Application.Run(b);
        }
    }
}