using System.Windows.Forms;

namespace Creek.UI.EFML.Base.JS.ScriptingTypes
{
    public class Screen
    {
        public readonly int height;
        public readonly int width;

        public Screen()
        {
            width = SystemInformation.VirtualScreen.Width;
            height = SystemInformation.VirtualScreen.Height;
        }
    }
}