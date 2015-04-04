namespace Creek.Drawing
{
    using System;
    using System.Text.RegularExpressions;

    public struct Hex32 : IComparable, IComparable<Hex32>, IEquatable<Hex32>
    {
        public const int MaxValue = 2147483647;
        public const int MinValue = -2147483648;
        private readonly int m_Value;

        private Hex32(int value)
        {
            this.m_Value = value;
        }

        private Hex32(string value)
        {
            this.m_Value = Parse(value).m_Value;
        }

        #region IComparable Members

        public int CompareTo(object value)
        {
            if (value is Hex32)
            {
                var tmp = (Hex32) value;
                return this.m_Value.CompareTo(tmp.m_Value);
            }
            else
                return this.m_Value.CompareTo(value);
        }

        #endregion

        #region IComparable<Hex32> Members

        public int CompareTo(Hex32 value)
        {
            return this.m_Value.CompareTo(value.m_Value);
        }

        #endregion

        #region IEquatable<Hex32> Members

        public bool Equals(Hex32 obj)
        {
            return this.m_Value.Equals(obj.m_Value);
        }

        #endregion

        public static implicit operator Hex32(int value)
        {
            return new Hex32(value);
        }

        public static implicit operator Hex32(string value)
        {
            return new Hex32(value);
        }

        public static implicit operator Hex32(Bin32 value)
        {
            return new Hex32(value);
        }

        public static implicit operator Hex32(Oct32 value)
        {
            return new Hex32(value);
        }

        public static implicit operator int(Hex32 value)
        {
            return value.m_Value;
        }

        public static explicit operator string(Hex32 value)
        {
            return value.ToString();
        }

        public override bool Equals(object obj)
        {
            if (obj is Hex32)
            {
                var tmp = (Hex32) obj;
                return this.Equals(tmp);
            }
            else
                return this.m_Value.Equals(obj);
        }

        public override int GetHashCode()
        {
            return this.m_Value.GetHashCode();
        }

        public TypeCode GetTypeCode()
        {
            return this.m_Value.GetTypeCode();
        }

        public static Hex32 Parse(string s)
        {
            return new Hex32(Convert.ToInt32(s, 16));
        }

        public override string ToString()
        {
            return Convert.ToString(this.m_Value, 16).ToUpper();
        }

        public string ToString(int minimumBytes)
        {
            int bits = minimumBytes*2;
            string rtn = this.ToString();
            if (rtn.Length < bits)
                return rtn.PadLeft(bits, '0');
            else
                return rtn;
        }

        public static bool TryParse(string s, out Hex32 result)
        {
            result = 0;
            var pattern = new Regex("[0-9A-Fa-f]+$");
            if (s.Length > 0 && s.Length <= 8 && s == s.Trim() && pattern.IsMatch(s))
            {
                result = Convert.ToInt32(s, 16);
                return true;
            }
            return false;
        }
    }
}