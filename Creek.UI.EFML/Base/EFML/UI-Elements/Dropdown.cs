using Creek.UI.EFML.Base.EFML.Elements;

namespace Creek.UI.EFML.UI_Elements
{
    public class Dropdown : ListElement
    {
        public DropdownStyle style;
    }

    public enum DropdownStyle
    {
        list,
        combo
    }
}