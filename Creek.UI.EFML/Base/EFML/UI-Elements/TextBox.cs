using System.Drawing;
using Creek.UI.EFML.Elements;

namespace Creek.UI.EFML.UI_Elements
{
    public class TextBox : InputElement
    {
        public Color forecolor = Color.Black;
        public string placeholder;

        public TextBox()
        {
            Events.Add("ontextchanged", null);
        }
    }
}