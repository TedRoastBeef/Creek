using Creek.UI.EFML.Elements;

namespace Creek.UI.EFML.UI_Elements
{
    public class TextArea : InputElement
    {
        public TextArea()
        {
            Events.Add("ontextchanged", null);
        }
    }
}