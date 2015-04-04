using System.Windows.Forms;

namespace Creek.UI.EFML.Base.JS.ScriptingTypes
{
    public class Document
    {
        public Document(UI.EFML.Document d, FlowLayoutPanel layout)
        {
            D = d;
            Layout = layout;
            foreach (UiElement elementBase in d.Body)
            {
                all[elementBase.ID] = elementBase;
            }
        }

        public UI.EFML.Document D { get; set; }
        public FlowLayoutPanel Layout { get; set; }

        public DocumentAll all
        {
            get { return new DocumentAll(Layout); }
        }

        public IUIElement GetElementById(string id)
        {
            foreach (Control control in Layout.Controls)
            {
                if (control.Name == id)
                {
                    return (IUIElement) control;
                }
            }
            return null;
        }
    }
}