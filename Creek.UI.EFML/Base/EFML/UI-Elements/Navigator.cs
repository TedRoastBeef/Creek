using System.Collections.Generic;
using Creek.UI.EFML.Base;

namespace Creek.UI.EFML.UI_Elements
{
    public class Navigator : UiElement
    {
        public List<Page> Pages = new List<Page>();

        public System.Drawing.Image backgroundimage;
        public System.Drawing.Image backbuttonimage;
        public System.Drawing.Image forwardbuttonimage;

    }

    public class Page
    {
        public string Caption;
        public List<ElementBase> Childs = new List<ElementBase>();
    }
}