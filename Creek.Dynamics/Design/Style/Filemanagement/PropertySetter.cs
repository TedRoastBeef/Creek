//*****************************************************************************
//	Done by : Sylvain BLANCHARD
//	Date : 08/01/2006
//*****************************************************************************

using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Creek.Dynamics.Design.Style.Filemanagement
{
    /// <summary>
    /// This class is used to set the value of a property of a control.
    /// This property can be defined  as the following : 
    ///     control.property1.property2.property3 = 10
    /// 
    /// Ex :
    ///     dgDataGrid.AlternatingRowsDefaultCellStyle.BackColor = Red
    ///     txtTextBox.Text = "OK"
    ///     txtTextBox.BackColor = Red
    /// </summary>
    class PropertySetter
    {
        private string styleName;
        private Control control;
        private string path;
        private string value;

        /// <summary>
        /// Set the value of the property of an object.
        /// If you want to set a 'Red' color to all the rows of DataGridView,
        /// use the following  :
        /// control = dgDataGridView
        /// path = "RowsDefaultCellStyle.BackColor"
        /// value = "Red"
        /// </summary>
        /// <param name="control">the control</param>
        /// <param name="path">the path to reach the right</param>
        /// <param name="value">the value to set to the property</param>
        public void Set(string styleName, Control control, string path, string value)
        {
           this.styleName = styleName;
           this.control = control;
           this.path = path;
           this.value = value;

            object lastObject = GetLastObject();
            string lastPropertyName = GetLastPropertyName();

            PropertyInfo prop = lastObject.GetType().GetProperty(lastPropertyName);
            if (prop == null)
                throw new StylesSheetException(StylesSheetException.ExceptionType.PropertyNotFound, String.Empty, control.Name, path, String.Empty);

            prop.SetValue(lastObject, GetValueFromString(prop.PropertyType, value), null);  	
        }

        /// <summary>
        /// Gets the name of the last property.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        private string GetLastPropertyName()
        {
            string[] propertyNames = path.Split('.');
            return propertyNames[propertyNames.Length - 1];
        }

        /// <summary>
        /// Gets the last object.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        private object GetLastObject()
        {
            // Nothing to do, the path isn't a complex path
            if (GetLastPropertyName().Equals(path))
                return control;

            string[] propertyNames = path.Split('.');
            object o = control;
            for (int i = 0; i <= propertyNames.Length - 2; i++)
            {
                o = GetObject(o, propertyNames[i]);
            }
            return o;
        }

        /// <summary>
        /// Gets the object.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        private object GetObject(object o, string propertyName)
        {
            int index = -1;
            if (propertyName.Contains("["))
            {
                int startIndex = propertyName.IndexOf("[");
                int endIndex = propertyName.IndexOf("]");

                if ((endIndex == -1) || (startIndex > endIndex) || (endIndex - startIndex) < 1)
                    throw new StylesSheetException(StylesSheetException.ExceptionType.IndexedPropertyNotCorrectlyDefined, styleName, control.Name, path, String.Empty);

                try
                {
                    index = int.Parse(propertyName.Substring(startIndex + 1, propertyName.Length - endIndex));
                    propertyName = propertyName.Substring(0, startIndex);
                }
                catch(Exception)
                {
                    throw new StylesSheetException(StylesSheetException.ExceptionType.IndexedPropertyNotCorrectlyDefined, styleName, control.Name, path, String.Empty);
                }
            }

            PropertyInfo prop = o.GetType().GetProperty(propertyName);
            if (prop == null)
                throw new StylesSheetException(StylesSheetException.ExceptionType.PropertyNotFound, String.Empty, control.Name, path, String.Empty);
            o = prop.GetValue(o, null);

            // If the property is a indexed property, we get the object
            // which is at the 'index' position in the list
            if (index != -1)
            {
                if (ImplementsIList(o))
                    o = ((IList)o)[index];
                else
                    throw new StylesSheetException(StylesSheetException.ExceptionType.IndexedPropertyNotCorrectlyDefined, styleName, control.Name, path, String.Empty);
            }

            return o;
        }

        /// <summary>
        /// Gets the value from string.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        private object GetValueFromString(Type type, string s)
        {
            return RuntimeHelpers.GetObjectValue(TypeDescriptor.GetConverter(type).ConvertFromString(s));
        }

        /// <summary>
        /// Implementses the I list.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <returns></returns>
        private bool ImplementsIList(object o)
        {
            bool implementsIList = false;

            Type[] interfaces = o.GetType().GetInterfaces();
            foreach (Type type in interfaces)
            {
                if (type.Name == "IList")
                {
                    implementsIList = true;
                    break;
                }
            }

            return implementsIList;
        }
    }
}
