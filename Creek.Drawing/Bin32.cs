namespace Creek.Drawing
{
    using System;
    using System.Text.RegularExpressions;

    public struct Bin32 : IComparable, IComparable<Bin32>, IEquatable<Bin32>
    {
        public const int MaxValue = 2147483647;
        public const int MinValue = -2147483648;
        private readonly int m_Value;

        private Bin32(int value)
        {
            this.m_Value = value;
        }

        private Bin32(string value)
        {
            this.m_Value = Parse(value).m_Value;
        }

        #region IComparable Members

        public int CompareTo(object value)
        {
            if (value is Bin32)
            {
                var tmp = (Bin32) value;
                return this.m_Value.CompareTo(tmp.m_Value);
            }
            else
                return this.m_Value.CompareTo(value);
        }

        #endregion

        #region IComparable<Bin32> Members

        public int CompareTo(Bin32 value)
        {
            return this.m_Value.CompareTo(value.m_Value);
        }

        #endregion

        #region IEquatable<Bin32> Members

        public bool Equals(Bin32 obj)
        {
            return this.m_Value.Equals(obj.m_Value);
        }

        #endregion

        public static implicit operator Bin32(int value)
        {
            return new Bin32(value);
        }

        public static implicit operator Bin32(string value)
        {
            return new Bin32(value);
        }

        public static implicit operator Bin32(Oct32 value)
        {
            return new Bin32(value);
        }

        public static implicit operator Bin32(Hex32 value)
        {
            return new Bin32(value);
        }

        public static implicit operator int(Bin32 value)
        {
            return value.m_Value;
        }

        public static explicit operator string(Bin32 value)
        {
            return value.ToString();
        }

        public override bool Equals(object obj)
        {
            if (obj is Bin32)
            {
                var tmp = (Bin32) obj;
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

        public static Bin32 Parse(string s)
        {
            return new Bin32(Convert.ToInt32(s, 2));
        }

        public override string ToString()
        {
            return Convert.ToString(this.m_Value, 2);
        }

        public string ToString(int minimumBytes)
        {
            int bits = minimumBytes*8;
            string rtn = this.ToString();
            if (rtn.Length < bits)
                return rtn.PadLeft(bits, '0');
            else
                return rtn;
        }

        public static bool TryParse(string s, out Bin32 result)
        {
            result = 0;
            var pattern = new Regex("[0-1]+$");
            if (s.Length > 0 && s.Length <= 32 && s == s.Trim() && pattern.IsMatch(s))
            {
                result = Convert.ToInt32(s, 2);
                return true;
            }
            return false;
        }
    }
}