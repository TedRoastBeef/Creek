using System;
using System.Collections.Generic;
using System.Globalization;

namespace Creek.Data.Registry
{
    internal class ValueEntry : IEntry
    {
        private string m_Key;
        private object m_Value;
        private ValueFormat m_ValueFormat;
        private readonly CultureInfo m_NeutralCulture;

        public ValueEntry(string key)
        {
            m_Key = key;

            m_ValueFormat = ValueFormat.Unknown;
            m_Value = null;

            m_NeutralCulture = CultureInfo.CreateSpecificCulture("en-US");
        }

        #region IEntry Members

        public string Key
        {
            get { return m_Key; }
            set { m_Key = value; }
        }

        public bool IsFolder
        {
            get { return false; }
        }

        public List<IEntry> Children
        {
            get { throw new RegistryException("A value entry does not support children."); }
        }

        public IEntry AddFolder(string key)
        {
            throw new RegistryException("The Value Entry does not support AddFolder");
        }

        public IEntry AddValue(string key)
        {
            throw new RegistryException("The Value Entry does not support AddFolder");
        }

        public void Remove(string key)
        {
            throw new RegistryException("The Value Entry does not support Remove");
        }

        public bool Contains(string key)
        {
            return false;
        }

        public void SetValue(object value, ValueFormat format)
        {
            if (format == ValueFormat.Double)
            {
                //
                //  Convert the double value to "neutral" culture.
                //
                var d = Convert.ToDouble(value);
                var formattedValue = d.ToString(m_NeutralCulture.NumberFormat);

                m_Value = formattedValue;
                m_ValueFormat = format;
            }
            else
            {
                m_Value = value;
                m_ValueFormat = format;
            }
        }

        public object GetValue()
        {
            var format = GetValueFormat();
            if (format == ValueFormat.Double)
            {
                //
                //  Convert the double value to "neutral" culture.
                //
                var d = Convert.ToDouble(m_Value, m_NeutralCulture.NumberFormat);

                return d;
            }

            return m_Value;
        }

        public ValueFormat GetValueFormat()
        {
            return m_ValueFormat;
        }

        public IEntry this[string key]
        {
            get
            {
                throw new RegistryException("Value does not support");
            }
        }

        #endregion
    }
}