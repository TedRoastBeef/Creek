//*****************************************************************************
//	Done by : Sylvain BLANCHARD
//	Date : 08/01/2006
//*****************************************************************************

using System;
using System.Xml.Serialization;

namespace Creek.Dynamics.Design.Style.Filemanagement
{
    /// <summary>
    /// Represents a property of a control which is defined in the styles sheet file.
    /// </summary>
    [Serializable, ]
    public class Property
    {
        /// <summary>
        /// A property has a name
        /// </summary>
        private string name;

        /// <summary>
        /// A property has a value stored in the styles sheet file as a string
        /// </summary>
        private string value;

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
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        [XmlAttribute("Value")]
        public string Value
        {
            get { return value; }
            set {this.value = value; }
        }
    }
}
