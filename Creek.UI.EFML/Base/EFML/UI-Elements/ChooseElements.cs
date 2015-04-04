using Creek.UI.EFML.Base.EFML.Elements;

namespace Creek.UI.EFML.UI_Elements
{
    public class Checkbox : ChooseElement
    {
        public Checkbox()
        {
            Events.Add("oncheckedchanged", null);
        }
    }

    public class Radio : ChooseElement
    {
    }
}