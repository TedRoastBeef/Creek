using System.Collections.Generic;
using Creek.UI.EFML.Base;
using Creek.UI.EFML.Base.EFML.Elements;

namespace Creek.UI.EFML.UI_Elements
{
    public class Tabcontrol : UiElement
    {
        public List<TabPage> Pages = new List<TabPage>();
    }

    public class TabPage : ContainerElement
    {
        public string Caption;
    }
}