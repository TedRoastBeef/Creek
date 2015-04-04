using System;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace Creek.UI.DialogBuilder
{
    internal class ControlFactory
    {
        internal static Control CreateControl(object item, PropertyInfo property)
        {
            Control ctrl = null;
            Type type = property.PropertyType;

            // The control depends on the property type
            if (type == typeof (string))
            {
                ctrl = new TextBox();
                var textbox = ctrl as TextBox;
                textbox.Text = (string) property.GetValue(item, null);
                textbox.Margin = new Padding(3, 3, 16, 0);
            }
            else if (type == typeof (char))
            {
                ctrl = new TextBox();
                var textbox = ctrl as TextBox;
                textbox.MaxLength = 1;
                textbox.Width = 20;
                textbox.Text = Convert.ToString(property.GetValue(item, null));
                textbox.Margin = new Padding(3, 3, 16, 0);
            }
            else if (type == typeof (int))
            {
                ctrl = new NumericUpDown();
                var numeric = ctrl as NumericUpDown;
                numeric.Value = Convert.ToDecimal(property.GetValue(item, null));
            }
            else if (type == typeof (decimal))
            {
                ctrl = new NumericUpDown();
                var numeric = ctrl as NumericUpDown;
                numeric.DecimalPlaces = 2;
                numeric.Value = Convert.ToDecimal(property.GetValue(item, null));
            }
            else if (type == typeof (bool))
            {
                ctrl = new CheckBox();
                var checkbox = ctrl as CheckBox;
                checkbox.Checked = Convert.ToBoolean(property.GetValue(item, null));
            }
            else if (type.BaseType == typeof (Enum))
            {
                ctrl = new ComboBox();
                var dropdown = ctrl as ComboBox;
                dropdown.DropDownStyle = ComboBoxStyle.DropDownList;
                string[] names = Enum.GetNames(type);
                string value = Convert.ToString(property.GetValue(item, null));
                foreach (string name in names)
                {
                    dropdown.Items.Add(name);
                    if (name == value)
                        dropdown.SelectedIndex = dropdown.Items.Count - 1;
                }
            }
            else if (type == typeof (DateTime))
            {
                ctrl = new DateTimePicker();
                var date = ctrl as DateTimePicker;
                DateTime dateValue = Convert.ToDateTime(property.GetValue(item, null));
                if (dateValue < date.MinDate)
                    dateValue = date.MinDate;
                if (dateValue > date.MaxDate)
                    dateValue = date.MaxDate;
                date.Value = dateValue;
            }
            if (ctrl != null)
            {
                var tag = new ControlTag();
                tag.PropertyName = property.Name;
                tag.PropertyType = property.PropertyType;
                ctrl.Tag = tag;
            }
            return ctrl;
        }

        /// <summary>
        /// Creates a new instance of the Label control using the specified text value.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        internal static Label CreateLabel(string text)
        {
            var label = new Label();
            label.Text = GetLabel(text) + ":";
            label.AutoSize = true;
            label.Margin = new Padding(3, 6, 6, 0);
            return label;
        }

        /// <summary>
        /// Returns a friendly label from the supplied name. For example, the
        /// string "firstName" would be returned as "First Name".
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private static string GetLabel(string text)
        {
            bool isFirst = true;
            var sb = new StringBuilder();
            foreach (char c in text)
            {
                if (isFirst)
                {
                    sb.Append(Char.ToUpper(c));
                    isFirst = false;
                }
                else
                {
                    if (Char.IsUpper(c))
                        sb.Append(' ');
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
    }
}