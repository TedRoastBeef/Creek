using System.Windows.Forms;

namespace Creek.UI.EFML.Base.JS.ScriptingTypes
{
    public static class Extensions
    {
        public static string toString(this object obj)
        {
            return obj.ToString();
        }
        public static Control cast(this object target)
        {
            return (Control) target;
        }
    }
}