using System;
using System.Collections.Generic;
using System.Linq;

namespace Creek.Data.Storage
{
    /// <summary>
    /// The abstract DataStorage class, that covers all implementation-independent
    /// aspects of the DataStorage
    /// </summary>
    public abstract class Data
    {
        /// <summary>
        /// Factory method to create a new instance of the preferred implementation
        /// </summary>
        /// <returns>The newly created instance</returns>
        public static Data CreateDataStorage()
        {
            return TreeData.CreateInstance();
        }
        
        /// <summary>
        /// Check whether the given name is valid for sections in the DataStorage
        /// </summary>
        /// <param name="name">The name to check</param>
        /// <returns>true, if the name may be used for section names</returns>
        public bool IsValidSectionName(string name)
        {
            if (name[name.Length - 1] != '\\')
                return false;

            return IsValidName(name);
        }

        /// <summary>
        /// Check whether the given name is valid for entries in the DataStorage
        /// </summary>
        /// <param name="name">The name to check</param>
        /// <returns>true, if the name may be used for entry names</returns>
        public bool IsValidEntryName(string name)
        {
            if (name[name.Length - 1] == '\\')
                return false;

            return IsValidName(name);
        }

        /// <summary>
        /// Check whether the given name is valid for the DataStorage
        /// </summary>
        /// <param name="name">The name to check</param>
        /// <returns>true, if the name may be used for section or value names</returns>
        public bool IsValidName(string name)
        {
            if (name.Length == 0)
                return false;

            if (name[0] == '\\')
                return false;
            
            if (name.IndexOf(@"\\", StringComparison.Ordinal) >= 0)
                return false;

            name = name.ToLower();

            return name.All(c => Char.IsLetterOrDigit(c) || c == '\\' || c == '.');
        }

        /// <summary>
        /// Extract the subsection name of the given section name (i.e. from "settings\data\temp\" return "temp")
        /// </summary>
        /// <param name="section">The name of the xection</param>
        /// <returns>The subsection name without trailing backslash</returns>
        public static string GetSubsectionName(string section)
        {
            string name = section.Substring(0, section.Length -1);
            name = name.Substring(name.LastIndexOf("\\", StringComparison.Ordinal) + 1);

            return name;
        }

        /// <summary>
        /// Clear all contents of the DataStorage
        /// </summary>
        public abstract void Clear();

        /// <summary>
        /// Add a section to the storage
        /// </summary>
        /// <param name="section">name of the section</param>
        /// <returns></returns>
        public abstract bool AddSection(string section);

        /// <summary>
        /// Remove a whole section with entries and subsections
        /// </summary>
        /// <param name="section">the section to remove</param>
        public abstract void RemoveSection(string section);

        /// <summary>
        /// Get all sections and their subsections that currently exist in the DataStorage
        /// </summary>
        /// <returns>An array with all section names</returns>
        public abstract string[] GetSections();

        /// <summary>
        /// Get all subsections of the given section and all subsections that currently exist in the DataStorage
        /// </summary>
        /// <param name="section">name of the parent section</param>
        /// <returns>An array with all subsection names</returns>
        public abstract string[] GetSubSections(string section);

        /// <summary>
        /// Get a value from the SettingStorage
        /// </summary>
        /// <param name="name">name of the value</param>
        /// <returns>the requested value. If the value does not exist, an empty string will be returned</returns>
        public abstract string GetValue(string name);

        /// <summary>
        /// Get all values from the given section in the SettingStorage
        /// </summary>
        /// <param name="section">name of the section</param>
        /// <returns>A List with all entries of the given section</returns>
        public abstract List<KeyValuePair<string,string>> GetValues(string section);

        /// <summary>
        /// Set a value in the DataStorage
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public abstract void SetValue(string name, string value);

        /// <summary>
        /// Removes the value with the given name
        /// </summary>
        /// <param name="name">name of the value to remove</param>
        public abstract void RemoveValue(string name);
    }
}
