using System;
using Creek.MVC.Configuration.Views;

namespace MVCSharp.Examples.AdvancedCustomization.Presentation
{
    public class ViewInfoEx : ViewInfo
    {
        private string imgName;

        public ViewInfoEx(string viewName, string imgName, Type viewType)
            : base(viewName, viewType)
        {
            this.imgName = imgName;
        }

        public string ImgName
        {
            get { return imgName; }
            set { imgName = value; }
        }
    }
}
