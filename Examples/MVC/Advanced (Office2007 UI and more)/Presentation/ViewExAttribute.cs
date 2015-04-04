using System;
using Creek.MVC.Configuration.Views;

namespace MVCSharp.Examples.AdvancedCustomization.Presentation
{
    public class ViewExAttribute : ViewAttribute
    {
        private string imgName;

        public ViewExAttribute(Type taskType, string viewName, string imgName)
            : base(taskType, viewName)
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
