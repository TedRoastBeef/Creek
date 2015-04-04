using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Creek.UI.FastColoredTextBox
{
    public class DynamicCollection : IEnumerable<AutocompleteItem>
    {
        private readonly AutocompleteMenu menu;
        private FastColoredTextBox tb;

        public DynamicCollection(AutocompleteMenu menu, FastColoredTextBox tb)
        {
            this.menu = menu;
            this.tb = tb;
        }

        #region IEnumerable<AutocompleteItem> Members

        public IEnumerator<AutocompleteItem> GetEnumerator()
        {
            //get current fragment of the text
            string text = menu.Fragment.Text;

            //extract class name (part before dot)
            string[] parts = text.Split('.');
            if (parts.Length < 2)
                yield break;
            string className = parts[parts.Length - 2];

            //find type for given className
            Type type = FindTypeByName(className);

            if (type == null)
                yield break;

            //return static methods of the class
            foreach (string methodName in type.GetMethods().AsEnumerable().Select(mi => mi.Name).Distinct())
                yield return new MethodAutocompleteItem(methodName + "()")
                                 {
                                     ToolTipTitle = methodName,
                                     ToolTipText = "Description of method " + methodName + " goes here.",
                                 };

            //return static properties of the class
            foreach (PropertyInfo pi in type.GetProperties())
                yield return new MethodAutocompleteItem(pi.Name)
                                 {
                                     ToolTipTitle = pi.Name,
                                     ImageIndex = 0,
                                     ToolTipText = "Description of property " + pi.Name + " goes here.",
                                 };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        private Type FindTypeByName(string name)
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            return assemblies.SelectMany(a => a.GetTypes()).FirstOrDefault(t => t.Name == name);
        }
    }
}