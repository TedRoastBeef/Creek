using System.Collections.Generic;
using Creek.UI.EFML.Base;

namespace Creek.UI.EFML
{
    public class Document
    {
        public List<ElementBase> Body;
        public Header Header;

        public Document()
        {
            Header = new Header();
            Body = new List<ElementBase>();
        }
    }

    public class Header
    {
        public List<ElementBase> Meta = new List<ElementBase>();
        public List<ElementBase> Scripts = new List<ElementBase>();
        public List<ElementBase> Styles = new List<ElementBase>();
        public List<ElementBase> Validators = new List<ElementBase>();
    }
}