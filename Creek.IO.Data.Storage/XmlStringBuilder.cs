using System.Collections.Generic;
using System.Text;

namespace Creek.Data.Storage
{
    /// <summary>
    /// A builder class to create nicely formatted XML strings.
    /// 
    /// Current limitations:
    /// It does not support attributes.
    /// It requires you to enforce valid xml (i.e. names, only one root etc.)
    /// </summary>
    class XmlStringBuilder
    {
        private Stack<string> sections = new Stack<string>();
        private StringBuilder xml = new StringBuilder();
        private string indent = "";

        public XmlStringBuilder()
        {
            appendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
        }

        /// <summary>
        /// Open a (sub) section to which values or subsections can be assigned later on.
        /// </summary>
        /// <param name="section">Name of the new subsection</param>
        public void openSubSection(string section)
        {
            appendLine("<" + section + " type=\"section\">");
            sections.Push(section);
            makeIndentString();
        }

        /// <summary>
        /// Closes a subsection
        /// </summary>
        public void closeSubSection()
        {
            if (sections.Count <= 0)
                return;

            string section = sections.Pop();
            makeIndentString();
            appendLine("</" + section + ">");
        }

        /// <summary>
        /// Adds a value to the current section
        /// </summary>
        /// <param name="name">name of the value</param>
        /// <param name="value">the actual value</param>
        public void addValue(string name, string value)
        {
            appendLine("<" + name + ">" + value + "</" + name + ">");
        }

        /// <summary>
        /// recalculates the indent string
        /// </summary>
        private void makeIndentString()
        {
            indent = "";
            for (int i = 0; i < sections.Count; i++)
            {
                indent += "\t";
            }
        }

        /// <summary>
        /// appends a line to the StringBuilder
        /// </summary>
        /// <param name="text"></param>
        private void appendLine(string text)
        {
            xml.Append(indent);
            xml.AppendLine(text);
        }

        public override string ToString()
        {
            return xml.ToString();
        }
    }
}
