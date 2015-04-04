using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace Creek.Tools
{
    public struct Date : IComparable, IFormattable, ISerializable, IComparable<Date>, IEquatable<Date>
    {
        public static readonly Date MaxValue = new Date(DateTime.MaxValue);
        public static readonly Date MinValue = new Date(DateTime.MinValue);
        private DateTime _dt;

        public Date(int year, int month, int day)
        {
            _dt = new DateTime(year, month, day);
        }

        public Date(DateTime dateTime)
        {
            _dt =
                dateTime.AddHours(-dateTime.Hour).AddMinutes(-dateTime.Minute).AddSeconds(-dateTime.Second).
                    AddMilliseconds(-dateTime.Millisecond);
        }

        public int Day
        {
            get { return _dt.Day; }
        }

        public DayOfWeek DayOfWeek
        {
            get { return _dt.DayOfWeek; }
        }

        public int DayOfYear
        {
            get { return _dt.DayOfYear; }
        }

        public int Month
        {
            get { return _dt.Month; }
        }

        public static Date Today
        {
            get { return new Date(DateTime.Today); }
        }

        public int Year
        {
            get { return _dt.Year; }
        }

        #region IComparable Members

        public int CompareTo(object value)
        {
            return _dt.CompareTo(value);
        }

        #endregion

        #region IComparable<Date> Members

        public int CompareTo(Date value)
        {
            return _dt.CompareTo(value._dt);
        }

        #endregion

        #region IEquatable<Date> Members

        public bool Equals(Date value)
        {
            return _dt.Equals(value._dt);
        }

        #endregion

        #region IFormattable Members

        public string ToString(string format, IFormatProvider provider)
        {
            return _dt.ToString(format, provider);
        }

        #endregion

        #region ISerializable Members

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ticks", _dt.Ticks);
        }

        #endregion

        public static TimeSpan operator -(Date d1, Date d2)
        {
            return d1._dt - d2._dt;
        }

        public static Date operator -(Date d, TimeSpan t)
        {
            return new Date(d._dt - t);
        }

        public static bool operator !=(Date d1, Date d2)
        {
            return d1._dt != d2._dt;
        }

        public static Date operator +(Date d, TimeSpan t)
        {
            return new Date(d._dt + t);
        }

        public static bool operator <(Date d1, Date d2)
        {
            return d1._dt < d2._dt;
        }

        public static bool operator <=(Date d1, Date d2)
        {
            return d1._dt <= d2._dt;
        }

        public static bool operator ==(Date d1, Date d2)
        {
            return d1._dt == d2._dt;
        }

        public static bool operator >(Date d1, Date d2)
        {
            return d1._dt > d2._dt;
        }

        public static bool operator >=(Date d1, Date d2)
        {
            return d1._dt >= d2._dt;
        }

        public static implicit operator DateTime(Date d)
        {
            return d._dt;
        }

        public static explicit operator Date(DateTime d)
        {
            return new Date(d);
        }

        public Date AddDays(int value)
        {
            return new Date(_dt.AddDays(value));
        }

        public Date AddMonths(int value)
        {
            return new Date(_dt.AddMonths(value));
        }

        public Date AddYears(int value)
        {
            return new Date(_dt.AddYears(value));
        }

        public static int Compare(Date d1, Date d2)
        {
            return d1.CompareTo(d2);
        }

        public static int DaysInMonth(int year, int month)
        {
            return DateTime.DaysInMonth(year, month);
        }

        public override bool Equals(object value)
        {
            return value is Date && _dt.Equals(((Date) value)._dt);
        }

        public override int GetHashCode()
        {
            return _dt.GetHashCode();
        }

        public static bool Equals(Date d1, Date d2)
        {
            return d1._dt.Equals(d2._dt);
        }

        public static bool IsLeapYear(int year)
        {
            return DateTime.IsLeapYear(year);
        }

        public static Date Parse(string s)
        {
            return new Date(DateTime.Parse(s));
        }

        public static Date Parse(string s, IFormatProvider provider)
        {
            return new Date(DateTime.Parse(s, provider));
        }

        public static Date Parse(string s, IFormatProvider provider, DateTimeStyles style)
        {
            return new Date(DateTime.Parse(s, provider, style));
        }

        public static Date ParseExact(string s, string format, IFormatProvider provider)
        {
            return new Date(DateTime.ParseExact(s, format, provider));
        }

        public static Date ParseExact(string s, string format, IFormatProvider provider, DateTimeStyles style)
        {
            return new Date(DateTime.ParseExact(s, format, provider, style));
        }

        public static Date ParseExact(string s, string[] formats, IFormatProvider provider, DateTimeStyles style)
        {
            return new Date(DateTime.ParseExact(s, formats, provider, style));
        }

        public TimeSpan Subtract(Date value)
        {
            return this - value;
        }

        public Date Subtract(TimeSpan value)
        {
            return this - value;
        }

        public string ToLongString()
        {
            return _dt.ToLongDateString();
        }

        public string ToShortString()
        {
            return _dt.ToShortDateString();
        }

        public override string ToString()
        {
            return ToShortString();
        }

        public string ToString(IFormatProvider provider)
        {
            return _dt.ToString(provider);
        }

        public string ToString(string format)
        {
            if (format == "O" || format == "o" || format == "s")
            {
                return ToString("yyyy-MM-dd");
            }

            return _dt.ToString(format);
        }

        public static bool TryParse(string s, out Date result)
        {
            DateTime d;
            bool success = DateTime.TryParse(s, out d);
            result = new Date(d);
            return success;
        }

        public static bool TryParse(string s, IFormatProvider provider, DateTimeStyles style, out Date result)
        {
            DateTime d;
            bool success = DateTime.TryParse(s, provider, style, out d);
            result = new Date(d);
            return success;
        }

        public static bool TryParseExact(string s, string format, IFormatProvider provider, DateTimeStyles style,
                                         out Date result)
        {
            DateTime d;
            bool success = DateTime.TryParseExact(s, format, provider, style, out d);
            result = new Date(d);
            return success;
        }

        public static bool TryParseExact(string s, string[] formats, IFormatProvider provider, DateTimeStyles style,
                                         out Date result)
        {
            DateTime d;
            bool success = DateTime.TryParseExact(s, formats, provider, style, out d);
            result = new Date(d);
            return success;
        }
    }

    public static class DateTimeExtensions
    {
        public static Date ToDate(this DateTime dt)
        {
            return new Date(dt);
        }
    }
}