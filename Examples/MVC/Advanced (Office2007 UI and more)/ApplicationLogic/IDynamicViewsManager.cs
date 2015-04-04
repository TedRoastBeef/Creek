using System;
using System.Collections.Generic;
using System.Text;

namespace MVCSharp.Examples.AdvancedCustomization.ApplicationLogic
{
    public interface IDynamicViewsManager
    {
        InteractionPointInfoEx CreateView(ViewCategory vc);
    }
}
