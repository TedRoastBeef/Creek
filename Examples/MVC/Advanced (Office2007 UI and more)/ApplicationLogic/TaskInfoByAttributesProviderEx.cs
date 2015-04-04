using Creek.MVC.Configuration.Tasks;
using MVCSharp.Examples.AdvancedCustomization.Presentation;

namespace MVCSharp.Examples.AdvancedCustomization.ApplicationLogic
{
    public class TaskInfoByAttributesProviderEx : TaskInfoByAttributesProvider
    {
        protected override InteractionPointInfo CreateInteractionPointInfo(
                           string viewName, InteractionPointAttribute ipAttribute)
        {
            InteractionPointInfoEx result = new InteractionPointInfoEx();
            result.ViewName = viewName;
            result.ControllerType = ipAttribute.ControllerType;
            result.IsCommonTarget = ipAttribute.IsCommonTarget;
            result.ViewCategory = (ipAttribute as IPointExAttribute).ViewCategory;
            return result;
        }
    }
}
