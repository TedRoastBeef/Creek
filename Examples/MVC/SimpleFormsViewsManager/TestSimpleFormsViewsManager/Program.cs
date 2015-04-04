using System;
using System.Windows.Forms;
using Creek.MVC.Configuration;
using Creek.MVC.Tasks;
using MVCSharp.Examples.SimpleFormsViewsManagerExample.TestGUI.ApplicationLogic;

namespace MVCSharp.Examples.SimpleFormsViewsManagerExample.TestGUI.Presentation
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            var tm = new TasksManager(MVCConfiguration.GetDefault());
            tm.Config.ViewsManagerType = typeof(SimpleFormsViewsManager);

            tm.StartTask(typeof(MainTask));

            Application.Run(Application.OpenForms[0]);
        }
    }
}