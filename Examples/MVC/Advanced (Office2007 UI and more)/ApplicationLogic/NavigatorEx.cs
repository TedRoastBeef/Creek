using System;
using Creek.MVC;

namespace MVCSharp.Examples.AdvancedCustomization.ApplicationLogic
{
    public class NavigatorEx : Navigator
    {
        public override IController GetController(string viewName)
        {
            IController result = base.GetController(viewName);
            if (result != null) return result;

            InteractionPointInfoEx ip = TaskInfo.InteractionPoints[viewName]
                                                  as InteractionPointInfoEx;
            Type cType = null;
            switch (ip.ViewCategory)
            {
                case ViewCategory.Mail: cType = typeof(MailController); break;
                case ViewCategory.Notes: cType = typeof(NoteController); break;
                case ViewCategory.Tasks: cType = typeof(TaskController); break;
            }
            ip.ControllerType = cType;
            return base.GetController(viewName);            
        }
    }
}
