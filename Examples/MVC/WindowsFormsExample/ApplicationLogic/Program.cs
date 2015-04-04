using System;
using System.Windows.Forms;
using Creek.MVC;
using Creek.MVC.Configuration;
using Creek.MVC.Tasks;
using MVCSharp.Examples.WindowsFormsExample.ApplicationLogic;

namespace WindowsFormsExample
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            MVCConfiguration config = WinformsViewsManager.GetDefaultConfig();
            (new TasksManager(config)).StartTask(typeof(MainTask));
            
            Application.Run(Application.OpenForms[0]);
        }
    }
}