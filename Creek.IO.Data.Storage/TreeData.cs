using System;
using System.Collections.Generic;
using System.Text;

namespace Creek.Data.Storage
{
    /// <summary>
    /// Implementation of the abstract class Data, which uses a recursive list of sections (rose tree)
    /// </summary>
    public class TreeData : Data
    {
        Dictionary<string, TreeData> sections = new Dictionary<string, TreeData>();
        Dictionary<string, string> values = new Dictionary<string, string>();

        private string lastLookupSectionName = null;
        private TreeData lastLookupSection = null;

        /// <summary>
        /// Factory method to create a new instance
        /// </summary>
        /// <returns>The newly created instance</returns>
        public static TreeData CreateInstance()
        {
            return new TreeData();
        }

        /// <summary>
        /// private constructor
        /// </summary>
        private TreeData() {}

        /// <summary>
        /// Clear all contents of the DataStorage
        /// </summary>
        public override void Clear()
        {
            sections.Clear();
            values.Clear();
        }

        /// <summary>
        /// Add a section to the storage
        /// </summary>
        /// <param name="section">name of the section</param>
        /// <returns></returns>
        public override bool AddSection(string section)
        {
            try
            {
                if (section.IndexOf('\\') >= 0)
                {
                    string[] sectionlist = section.Split(new char[] { '\\' }, 2);
                    if (!sections.ContainsKey(sectionlist[0]))
                    {
                        sections.Add(sectionlist[0], new TreeData());
                    }
                    return sections[sectionlist[0]].AddSection(sectionlist[1]);
                }
                else
                {
                    return true;
                }
            }
            catch
            {
                throw new Exception("Section could not be created: " + section);
            }
        }

        /// <summary>
        /// Remove a whole section with entries and subsections
        /// </summary>
        /// <param name="section">the section to remove</param>
        public override void RemoveSection(string section)
        {
            try
            {
                if (section.IndexOf('\\') >= 0)
                {
                    string sectionName = section.Substring(section.LastIndexOf('\\') + 1);
                    TreeData parentSection = findParentSection(section);
                    parentSection.RemoveSection(sectionName);
                }
                else
                {
                    sections.Remove(section);
                }
            }
            catch { }
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
            TreeData dataSection = findSection(section);
            return getSubSections(dataSection, section).ToArray();
        }

        /// <summary>
        /// Get all subsections of the given section and all subsections that currently exist in the DataStorage
        /// </summary>
        /// <param name="section">name of the parent section</param>
        /// <returns>An array with all subsection names</returns>
        public void getSubSections(string prefix, List<string> sectionList)
        {
            foreach (KeyValuePair<string, TreeData> s in sections)
            {
                string tmpSection = prefix + s.Key + '\\';
                sectionList.Add(tmpSection);
                s.Value.getSubSections(tmpSection, sectionList);
            }
        }

        /// <summary>
        /// Get all subsections of the given section and all subsections that currently exist in the DataStorage
        /// </summary>
        /// <param name="section">name of the parent section</param>
        /// <returns>An array with all subsection names</returns>
        public List<string> getSubSections(TreeData section, string prefix)
        {
            List<string> sectionList = new List<string>();
            section.getSubSections(prefix, sectionList);
            return sectionList;
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

            string value = null;

            try
            {
                if (name.IndexOf('\\') >= 0)
                {
                    string valueName = name.Substring(name.LastIndexOf('\\') + 1);
                    TreeData section = findSection(name);
                    value = section.GetValue(valueName);
                }
                else
                {
                    value = values[name];
                }
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
            if (!section.EndsWith("\\"))
                section += "\\";

            try
            {
                TreeData dataSection = findSection(section);
                return dataSection.getValues();
            }
            catch { return new List<KeyValuePair<string, string>>();}
        }

        /// <summary>
        /// Get all values from the section in the SettingStorage
        /// </summary>
        /// <returns>A List with all entries of the given section</returns>
        protected List<KeyValuePair<string, string>> getValues()
        {
            List<KeyValuePair<string, string>> values = new List<KeyValuePair<string, string>>();

            foreach (KeyValuePair<string, string> value in this.values)
            {
                values.Add(value);
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

            try
            {
                if (name.IndexOf('\\') >= 0)
                {
                    string valueName = name.Substring(name.LastIndexOf('\\') + 1);
                    
                    TreeData section = findSection(name, true);
                    section.SetValue(valueName, value);
                }
                else
                {
                    values[name] = value;
                }

            }
            catch { }
        }

        /// <summary>
        /// Removes the value with the given name
        /// </summary>
        /// <param name="name">name of the value to remove</param>
        public override void RemoveValue(string name)
        {
            if (!IsValidEntryName(name))
                return;

            try
            {
                if (name.IndexOf('\\') >= 0) {
                    string valueName = name.Substring(name.LastIndexOf('\\') + 1);
                    TreeData section = findSection(name);
                    section.RemoveValue(valueName);
                }
                else 
                {
                    values.Remove(name);
                }
                
            }
            catch { }
        }

        /// <summary>
        /// Finds a section identified by a value or section name
        /// </summary>
        /// <param name="name">name of the section</param>
        /// <param name="create">Create subsection that do not exist</param>
        protected TreeData findSection(string name, bool create)
        {
            string sectionName;

            if (name.IndexOf('\\') >= 0)
                sectionName = name.Substring(0, name.LastIndexOf('\\'));
            else
                sectionName = name;

            if (sectionName.Equals(lastLookupSectionName))
                return lastLookupSection;
            else
                return findSectionRecursing(name, create);
        }

        /// <summary>
        /// Finds a section identified by a value or section name
        /// </summary>
        /// <param name="name">name of the section</param>
        /// <param name="create">Create subsection that do not exist</param>
        protected TreeData findSectionRecursing(string name, bool create)
        {
            try
            {
                if (name.IndexOf('\\') >= 0)
                {
                    string[] sectionlist = name.Split(new char[] { '\\' }, 2);
                    if (!sections.ContainsKey(sectionlist[0]) && create)
                        sections.Add(sectionlist[0], new TreeData());
                    return sections[sectionlist[0]].findSectionRecursing(sectionlist[1], create);
                }
                else
                {
                    return this;
                }
            }
            catch
            {
                throw new Exception("Section not found for " + name);
            }
        }

        /// <summary>
        /// Finds a section identified by a value or section name
        /// </summary>
        /// <param name="name">name of the section</param>
        protected TreeData findSection(string name)
        {
            return findSection(name, false);
        }

        /// <summary>
        /// Finds a parentsection identified by a value or section name
        /// </summary>
        /// <param name="name">name of the section</param>
        protected TreeData findParentSection(string name)
        {
            if (name.EndsWith("\\"))
            {
                name = name.Substring(0, name.Length - 1);
            }

            return findSection(name);
        }

        /// <summary>
        /// Create a string representation of the whole DataStorage
        /// </summary>
        /// <returns>string with all sections and entries</returns>
        public override string ToString()
        {
            return ToString("");
        }

        /// <summary>
        /// Create a string representation of the whole DataStorage and add the prefex path to it
        /// </summary>
        /// <returns>string with all sections and entries</returns>
        protected string ToString(string sectionPath)
        {
            StringBuilder sb = new StringBuilder();

            foreach (KeyValuePair<string, TreeData> section in sections)
            {
                sb.Append(sectionPath + "\\" + section.Key);
                sb.Append(section.Value.ToString(sectionPath + "\\" + section.Key));
            }

            foreach (KeyValuePair<string, string> value in values)
            {
                sb.Append(sectionPath + "\\" + value.Key + '=' + value.Value);
            }

            return sb.ToString();
        }
    }
}
