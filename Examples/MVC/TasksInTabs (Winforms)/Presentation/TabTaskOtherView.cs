using System;
using Creek.MVC;
using Creek.MVC.Configuration.Views;
using MVCSharp.Examples.TasksInTabs.ApplicationLogic;

namespace MVCSharp.Examples.TasksInTabs.Presentation
{
    [View(typeof(TabTask1), TabTask1.View1)]
    public partial class TabTaskOtherView : WinUserControlView
    {
        public TabTaskOtherView()
        {
            InitializeComponent();
        }

        public override void Activate(bool activate)
        {
            int taskStartTimes = (Controller.Task as TabTask1).TaskStartTimes;
            label1.Text = "Task was started " + taskStartTimes.ToString() + " times.";        
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Controller.Task.Navigator.Navigate(TabTask1.View2);
        }
    }
}
