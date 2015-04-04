using System.Drawing;
using System.Windows.Forms;
using Creek.UI.Effects;

namespace Creek.UI.EFML.Base
{
    public interface IUIElement
    {
        string ID { get; set; }
        
        string Content { get; set; }
        IValidator Validator { get; set; }
        IStyle style { get; }
        
    }
    public interface IStyle
    {
        bool autosize { get; set; }
        Font font { get; set; }
        Size Size { get; set; }
        Padding margin { get; set; }
        Padding padding { get; }
        Transition transition { get; set; }
    }
}