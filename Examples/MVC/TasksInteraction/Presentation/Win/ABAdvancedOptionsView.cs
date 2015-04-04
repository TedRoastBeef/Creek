using System;
using Creek.MVC;
using Creek.MVC.Configuration;
using MVCSharp.Examples.TasksInteraction.ApplicationLogic;

namespace MVCSharp.Examples.TasksInteraction.Presentation.Win
{
    using Creek.MVP.Configuration;

    [WinformsView(typeof(AwardBonusTask), AwardBonusTask.AdvancedOptionsView, ShowModal = true)]
    public partial class ABAdvancedOptionsView : WinFormView, IABAdvancedOptionsView
    {
        public ABAdvancedOptionsView()
        {
            InitializeComponent();
        }

        public bool IsWorkerOfTheMonth
        {
            get { return workerOfTheMonthCB.Checked; }
            set { workerOfTheMonthCB.Checked = value; }
        }

        public bool SpecialServices
        {
            get { return specialServicesCB.Checked; }
            set { specialServicesCB.Checked = value; }
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            (Controller as AbAdvancedOptionsController).DoEnterOptions();
        }
    }
}