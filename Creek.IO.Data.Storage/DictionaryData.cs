using System.Collections.Generic;
using System.Text;

namespace Creek.Data.Storage
{
    /// <summary>
    /// Implementation of the abstract class Data, which uses one big Dictionary to store the values
    /// </summary>
    public class DictionaryData : Data
    {
        private SortedDictionary<string, string> data = new SortedDictionary<string, string>();


        /// <summary>
        /// Factory method to create a new instance
        /// </summary>
        /// <returns>The newly created instance</returns>
        public static DictionaryData createInstance()
        {
            return new DictionaryData();
        }

        /// <summary>
        /// private contructor
        /// </summary>
        private DictionaryData() { }

        /// <summary>
        /// Clear all contents of the DataStorage
        /// </summary>
        public override void Clear()
        {
            data.Clear();
        }

        /// <summary>
        /// Add a section to the storage
        /// </summary>
        /// <param name="section">name of the section</param>
        /// <returns></returns>
        public override bool AddSection(string section)
        {
            if (section[section.Length - 1] != '\\')
            {
                section = section + '\\';
            }

            // sections end with a backslash and have an empty value
            try
            {
                data.Add(section, "");
            }
            catch { }

            return true;
        }

        /// <summary>
        /// Remove a whole section with entries and subsections
        /// </summary>
        /// <param name="section">the section to remove</param>
        public override void RemoveSection(string section)
        {
            
            SortedDictionary<string, string> data1 = new SortedDictionary<string,string>();
            foreach (KeyValuePair<string, string> entry in data)
                if (!((entry.Key.StartsWith(section)) && (entry.Key.EndsWith(@"\"))))
                    data1.Add(entry.Key, entry.Value);
            data = data1;
        }

        /// <summary>
        /// Get all sections and their subsections that currently exist in the DataStorage
        /// </summary>
        /// <returns>An array with all section names</returns>
        public override string[] GetSections()
        {
            return GetSubSections("");
        }

        /// <summary>
        /// Get all subsections of the given section and all subsections that currently exist in the DataStorage
        /// </summary>
        /// <param name="section">name of the parent section</param>
        /// <returns>An array with all subsection names</returns>
        public override string[] GetSubSections(string section)
        {
            List<string> sections = new List<string>();

            foreach (KeyValuePair<string, string> entry in data)
            {
                if ((entry.Key.StartsWith(section)) && (entry.Key.EndsWith(@"\")))
                    sections.Add(entry.Key);
            }

            return sections.ToArray();
        }

        /// <summary>
        /// Get a value from the SettingStorage
        /// </summary>
        /// <param name="name">name of the value</param>
        /// <returns>the requested value. If the value does not exist, an empty string will be returned</returns>
        public override string GetValue(string name)
        {
            if (!IsValidEntryName(name))
                return "";

            string value;

            try
            {
                value = data[name];
            }
            catch { return ""; }

            if (value != null)
                return value;
            else
                return "";
        }

        /// <summary>
        /// Get all values from the given section in the SettingStorage
        /// </summary>
        /// <param name="section">name of the section</param>
        /// <returns>A List with all entries of the given section</returns>
        public override List<KeyValuePair<string, string>> GetValues(string section)
        {
            List<KeyValuePair<string, string>> values = new List<KeyValuePair<string, string>>();
            if (!section.EndsWith("\\"))
                section += "\\";

            foreach (KeyValuePair<string, string> entry in data)
            {
                if ((entry.Key.StartsWith(section)) && ((entry.Key.Length <= section.Length)  || (entry.Key.IndexOf('\\', section.Length + 1) < 0)))
                {
                    if (!entry.Key.EndsWith("\\"))
                        values.Add(new KeyValuePair<string, string>(entry.Key.Substring(section.Length), entry.Value));
                }
            }

            return values;
        }

        /// <summary>
        /// Set a value in the DataStorage
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public override void SetValue(string name, string value)
        {
            if (!IsValidEntryName(name))
                return;

            string[] sectionlist = name.Split('\\');
            string tmpsection = "";

            for (int i = 0; i<sectionlist.Length - 1; i++)
            {
                tmpsection += sectionlist[i] + "\\";
                if (!tmpsection.Equals(name))
                    AddSection(tmpsection);
            }

            data[name] = value;
        }

        /// <summary>
        /// Removes the value with the given name
        /// </summary>
        /// <param name="name">name of the value to remove</param>
        public override void RemoveValue(string name)
        {

        }

        /// <summary>
        /// Create a string representation of the whole DataStorage
        /// </summary>
        /// <returns>string with all sections and entries</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (KeyValuePair<string, string> entry in data)
            {
                if (entry.Key[entry.Key.Length - 1] == '\\')
                    sb.AppendLine(entry.Key);
                else
                    sb.AppendLine(entry.Key + '=' + entry.Value);
            }

            return sb.ToString();
        }
    }
}
