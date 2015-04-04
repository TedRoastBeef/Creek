using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace Creek.UI.PropertyGridEx
{
    public class BrowsableTypeConverter : ExpandableObjectConverter
    {
        #region LabelStyle enum

        public enum LabelStyle
        {
            lsNormal,
            lsTypeName,
            lsEllipsis
        }

        #endregion

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return true;
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value,
                                         Type destinationType)
        {
            var attribute1 =
                (BrowsableLabelStyleAttribute)
                context.PropertyDescriptor.Attributes[typeof (BrowsableLabelStyleAttribute)];
            if (attribute1 != null)
            {
                switch (attribute1.LabelStyle)
                {
                    case LabelStyle.lsNormal:
                        {
                            return base.ConvertTo(context, culture, RuntimeHelpers.GetObjectValue(value),
                                                  destinationType);
                        }
                    case LabelStyle.lsTypeName:
                        {
                            return ("(" + value.GetType().Name + ")");
                        }
                    case LabelStyle.lsEllipsis:
                        {
                            return "(...)";
                        }
                }
            }
            return base.ConvertTo(context, culture, RuntimeHelpers.GetObjectValue(value), destinationType);
        }

        #region Nested type: BrowsableLabelStyleAttribute

        public class BrowsableLabelStyleAttribute : Attribute
        {
            private LabelStyle eLabelStyle = LabelStyle.lsNormal;

            public BrowsableLabelStyleAttribute(LabelStyle LabelStyle)
            {
                eLabelStyle = LabelStyle;
            }

            public LabelStyle LabelStyle
            {
                get { return eLabelStyle; }
                set { eLabelStyle = value; }
            }
        }

        #endregion
    }
}