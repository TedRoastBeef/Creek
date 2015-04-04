using System.Windows.Forms;
using Creek.UI.EFML.Base.Controls;
using Creek.UI.EFML.Base.Controls.Navigator;

namespace Creek.UI.EFML.Base
{
    internal class WinformsControlProvider : ControlProvider
    {
        public WinformsControlProvider()
        {
            Add<Label>(Tag.Label);
            Add<Button>(Tag.Button);
            Add<CheckBox>(Tag.Checkbox);
            Add<Dropdown>(Tag.Dropdown);
            Add<LinkLabel>(Tag.Link);
            Add<RadioButton>(Tag.Radiobutton);
            Add<PlaceholderTextBox>(Tag.Textbox);
            Add<DivPanel>(Tag.Div);
            Add<GroupBox>(Tag.Group);
            Add<PageNavigator>(Tag.Navigator);
            Add<Line>(Tag.Line);
        }
    }
}