//*****************************************************************************
//	Done by : Sylvain BLANCHARD
//	Date : 08/01/2006
//*****************************************************************************

using System;
using System.Collections.Generic;

namespace Creek.Dynamics.Design.Style.Filemanagement
{
    /// <summary>
    /// Represents the styles sheet file.
    /// </summary>
    [Serializable]
    public class StylesSheetFile
    {
        /// <summary>
        /// A styles sheet file is composed of a list of styles
        /// </summary>
        private List<Creek.Dynamics.Design.Style.Filemanagement.Style> styles = new List<Creek.Dynamics.Design.Style.Filemanagement.Style>();

        /// <summary>
        /// Gets or sets the styles.
        /// </summary>
        /// <value>The styles.</value>
        public List<Creek.Dynamics.Design.Style.Filemanagement.Style> Styles
        {
            get { return styles; }
            set { styles = value; }
        }
    }
}
