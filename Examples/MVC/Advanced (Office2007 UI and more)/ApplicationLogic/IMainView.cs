using System;
using System.Collections.Generic;
using System.Text;
using MVCSharp.Examples.AdvancedCustomization.ApplicationLogic;

namespace MVCSharp.Examples.AdvancedCustomization.Presentation
{
    public interface IMainView
    {
        void AddViewToNavPane(InteractionPointInfoEx ip);
    }
}
