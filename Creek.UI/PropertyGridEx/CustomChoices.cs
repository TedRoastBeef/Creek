using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Creek.UI.PropertyGridEx
{
    [Serializable]
    public class CustomChoices : ArrayList
    {
        public CustomChoices(ArrayList array, bool IsSorted)
        {
            AddRange(array);
            if (IsSorted)
            {
                Sort();
            }
        }

        public CustomChoices(ArrayList array)
        {
            AddRange(array);
        }

        public CustomChoices(string[] array, bool IsSorted)
        {
            AddRange(array);
            if (IsSorted)
            {
                Sort();
            }
        }

        public CustomChoices(string[] array)
        {
            AddRange(array);
        }

        public CustomChoices(int[] array, bool IsSorted)
        {
            AddRange(array);
            if (IsSorted)
            {
                Sort();
            }
        }

        public CustomChoices(int[] array)
        {
            AddRange(array);
        }

        public CustomChoices(double[] array, bool IsSorted)
        {
            AddRange(array);
            if (IsSorted)
            {
                Sort();
            }
        }

        public CustomChoices(double[] array)
        {
            AddRange(array);
        }

        public CustomChoices(object[] array, bool IsSorted)
        {
            AddRange(array);
            if (IsSorted)
            {
                Sort();
            }
        }

        public CustomChoices(object[] array)
        {
            AddRange(array);
        }

        public ArrayList Items
        {
            get { return this; }
        }

        #region Nested type: CustomChoicesAttributeList

        public class CustomChoicesAttributeList : Attribute
        {
            private readonly ArrayList oList = new ArrayList();

            public CustomChoicesAttributeList(string[] List)
            {
                oList.AddRange(List);
            }

            public CustomChoicesAttributeList(ArrayList List)
            {
                oList.AddRange(List);
            }

            public CustomChoicesAttributeList(ListBox.ObjectCollection List)
            {
                oList.AddRange(List);
            }

            public ArrayList Item
            {
                get { return oList; }
            }

            public TypeConverter.StandardValuesCollection Values
            {
                get { return new TypeConverter.StandardValuesCollection(oList); }
            }
        }

        #endregion

        #region Nested type: CustomChoicesTypeConverter

        public class CustomChoicesTypeConverter : TypeConverter
        {
            private CustomChoicesAttributeList oChoices;

            public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
            {
                bool returnValue;
                var Choices =
                    (CustomChoicesAttributeList)
                    context.PropertyDescriptor.Attributes[typeof (CustomChoicesAttributeList)];
                if (oChoices != null)
                {
                    return true;
                }
                if (Choices != null)
                {
                    oChoices = Choices;
                    returnValue = true;
                }
                else
                {
                    returnValue = false;
                }
                return returnValue;
            }

            public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
            {
                bool returnValue;
                var Choices =
                    (CustomChoicesAttributeList)
                    context.PropertyDescriptor.Attributes[typeof (CustomChoicesAttributeList)];
                if (oChoices != null)
                {
                    return true;
                }
                if (Choices != null)
                {
                    oChoices = Choices;
                    returnValue = true;
                }
                else
                {
                    returnValue = false;
                }
                return returnValue;
            }

            public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
            {
                var Choices =
                    (CustomChoicesAttributeList)
                    context.PropertyDescriptor.Attributes[typeof (CustomChoicesAttributeList)];
                if (oChoices != null)
                {
                    return oChoices.Values;
                }
                return base.GetStandardValues(context);
            }
        }

        #endregion
    }
}