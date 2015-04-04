//*****************************************************************************
//	Done by : Sylvain BLANCHARD
//	Date : 08/01/2006
//*****************************************************************************

using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Creek.Dynamics.Design.Style.Filemanagement
{
    /// <summary>
    /// Represents a style which is defined in the styles sheet file.
    /// </summary>
    [Serializable]
    public class Style
    {
        /// <summary>
        /// A style has a name
        /// </summary>
        private string name;

        /// <summary>
        /// A style has a list of properties
        /// </summary>
        private List<Property> properties = new List<Property>();


        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [XmlAttribute("Name")]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Gets or sets the properties.
        /// </summary>
        /// <value>The properties.</value>
        public List<Property> Properties
        {
            get { return properties; }
            set { properties = value; }
        }
    }
}
