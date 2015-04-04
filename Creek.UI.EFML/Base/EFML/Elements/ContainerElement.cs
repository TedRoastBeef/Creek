using System.Collections.Generic;
using System.Windows.Forms;

namespace Creek.UI.EFML.Base.EFML.Elements
{
    public class ContainerElement : UiElement
    {
        public List<ElementBase> Childs;

        public BorderStyle border;

        public ContainerElement()
        {
            Childs = new List<ElementBase>();
        }
    }
}