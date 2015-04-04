using System;
using System.ComponentModel;
using System.Globalization;

namespace Creek.UI.EnumButtonGroup
{
    /// <summary>
    /// Used in by PropertyGrid control to convert string values to types
    /// </summary>
    public class StringToTypeConverter : ExpandableObjectConverter
    {
        /// <summary>
        /// Determines whether the specified type can be converted to a type
        /// </summary>
        /// <param name="context">Type descriptor context</param>
        /// <param name="sourceType">Type to be converted to a type</param>
        /// <returns>True if the source type can be converted to a type</returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            bool canConvert;

            // String is the only type we allow to be converted to a type
            if (sourceType == typeof (string))
            {
                canConvert = true;
            }
            else
            {
                canConvert = base.CanConvertFrom(context, sourceType);
            }

            return (canConvert);
        }

        /// <summary>
        /// Performs string to type conversion
        /// </summary>
        /// <param name="context">Type descriptor context</param>
        /// <param name="culture">Current culture</param>
        /// <param name="value">String to convert from</param>
        /// <returns>A type</returns>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            object convertedValue;

            try
            {
                // NOTE: if the type is either not loaded under the current assembly or in mscorlib, then the
                // return value will be null
                convertedValue = Type.GetType(value as string);
            }
            catch (FormatException)
            {
                convertedValue = base.ConvertFrom(context, culture, value);
            }

            return (convertedValue);
        }
    }
}