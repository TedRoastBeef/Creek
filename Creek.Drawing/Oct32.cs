namespace Creek.Drawing
{
    using System;
    using System.Text.RegularExpressions;

    public struct Oct32 : IComparable, IComparable<Oct32>, IEquatable<Oct32>
    {
        public const int MaxValue = 2147483647;
        public const int MinValue = -2147483648;
        private readonly int m_Value;

        private Oct32(int value)
        {
            this.m_Value = value;
        }

        private Oct32(string value)
        {
            this.m_Value = Parse(value).m_Value;
        }

        #region IComparable Members

        public int CompareTo(object value)
        {
            if (value is Oct32)
            {
                var tmp = (Oct32) value;
                return this.m_Value.CompareTo(tmp.m_Value);
            }
            else
                return this.m_Value.CompareTo(value);
        }

        #endregion

        #region IComparable<Oct32> Members

        public int CompareTo(Oct32 value)
        {
            return this.m_Value.CompareTo(value.m_Value);
        }

        #endregion

        #region IEquatable<Oct32> Members

        public bool Equals(Oct32 obj)
        {
            return this.m_Value.Equals(obj.m_Value);
        }

        #endregion

        public static implicit operator Oct32(int value)
        {
            return new Oct32(value);
        }

        public static implicit operator Oct32(string value)
        {
            return new Oct32(value);
        }

        public static implicit operator Oct32(Bin32 value)
        {
            return new Oct32(value);
        }

        public static implicit operator Oct32(Hex32 value)
        {
            return new Oct32(value);
        }

        public static implicit operator int(Oct32 value)
        {
            return value.m_Value;
        }

        public static explicit operator string(Oct32 value)
        {
            return value.ToString();
        }

        public override bool Equals(object obj)
        {
            if (obj is Oct32)
            {
                var tmp = (Oct32) obj;
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

        public static Oct32 Parse(string s)
        {
            return new Oct32(Convert.ToInt32(s, 8));
        }

        public override string ToString()
        {
            return Convert.ToString(this.m_Value, 8);
        }

        public static bool TryParse(string s, out Oct32 result)
        {
            result = 0;
            var pattern = new Regex("[0-7]+$");
            if (s.Length > 0 && s.Length <= 11 && s == s.Trim() && pattern.IsMatch(s))
            {
                if (s.Length == 11)
                {
                    char[] chars = s.ToCharArray();
                    switch (chars[0])
                    {
                        case '4':
                        case '5':
                        case '6':
                        case '7':
                            return false;
                    }
                }
                result = Convert.ToInt32(s, 8);
                return true;
            }
            return false;
        }
    }
}