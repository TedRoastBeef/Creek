//*****************************************************************************
//	Done by : Sylvain BLANCHARD
//	Date : 08/01/2006
//*****************************************************************************

using System.ComponentModel;
using System.Windows.Forms;

namespace Creek.Dynamics.Design.Style.Designtime
{
    class VSDesignTimeEnvironment
    {
        Component c;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:VSDesignTimeEnvironment"/> class.
        /// </summary>
        /// <param name="c">The c.</param>
        public VSDesignTimeEnvironment(Component c)
        {
            this.c = c;
        }

        /// <summary>
        /// Gets the app config path.
        /// </summary>
        /// <returns></returns>
        public string GetAppConfigPath()
        {
            var info = new System.IO.FileInfo(Application.StartupPath + "\\app.config");
            return info.Directory.FullName;
        }
    }
}
