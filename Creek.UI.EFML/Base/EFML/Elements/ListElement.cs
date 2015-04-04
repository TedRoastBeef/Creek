using System.Collections.Generic;

namespace Creek.UI.EFML.Base.EFML.Elements
{
    public class ListElement : UiElement
    {
        public List<string> Childs;
        public int SelectedIndex;
        public string SeletedChild;

        public ListElement()
        {
            Childs = new List<string>();

            Events.Add("onselectionchange", null);
        }
    }
}