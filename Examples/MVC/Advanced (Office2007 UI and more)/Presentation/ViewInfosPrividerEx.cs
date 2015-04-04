using System;
using Creek.MVC.Configuration.Views;

namespace MVCSharp.Examples.AdvancedCustomization.Presentation
{
    public class ViewInfosPrividerEx : DefaultViewInfosProvider
    {
        protected override ViewInfo newViewInfo(Type viewType, ViewAttribute vAtr)
        {
            return new ViewInfoEx(vAtr.ViewName,
                  (vAtr as ViewExAttribute).ImgName, viewType);
        }
    }
}
