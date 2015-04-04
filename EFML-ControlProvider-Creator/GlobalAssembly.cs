using System.IO;
using System.Reflection;

namespace EFML_ControlProvider_Creator
{
    internal class GlobalAssembly
    {
        public Assembly a;

        public void Load(Stream s)
        {
            var cp = new Creek.UI.EFML.ControlProvider();
            cp.Load(s);

            a = Assembly.Load(cp.Assembly);
        }

    }
}
