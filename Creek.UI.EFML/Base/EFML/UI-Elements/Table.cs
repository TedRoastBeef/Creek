using System.Collections.Generic;
using Creek.UI.EFML.Base;
using Creek.UI.EFML.Base.EFML.Elements;

namespace Creek.UI.EFML.UI_Elements
{
    public class Table : ContainerElement
    {
        public List<ElementBase> Columns = new List<ElementBase>();
        public List<ElementBase> Rows = new List<ElementBase>();
    }
}