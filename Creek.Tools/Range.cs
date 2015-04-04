using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Creek.Tools
{
    public class Range<T> : IEnumerable<T> where T : IComparable
    {
        public T End;
        public T Start;

        public Range(T start, T end)
        {
            Start = start;
            End = end;
        }

        public static explicit operator Range<T>(string s)
        {
            s = s.Remove(0, 1);
            s = s.Remove(s.Length - 1, 1);
            string[] sp = s.Split('-');

            var r = new Range<T>
                (
                (T) Convert.ChangeType(sp[0], typeof (T)),
                (T) Convert.ChangeType(sp[0], typeof (T))
                );
            return r;
        }

        public static explicit operator string(Range<T> s)
        {
            return s.ToString();
        }

        public override string ToString()
        {
            return String.Format("[{0}-{1}]", Start, End);
        }

        /// <summary>
        /// Determines if the Range is valid
        /// </summary>
        /// <returns>True if range is valid, else false</returns>
        public bool IsValid()
        {
            return Start.CompareTo(End) <= 0;
        }

        /// <summary>
        /// Determines if the provided value is inside the range
        /// </summary>
        /// <param name="value">The value to test</param>
        /// <returns>True if the value is inside Range, else false</returns>
        public bool ContainsValue(T value)
        {
            return (Start.CompareTo(value) <= 0) && (value.CompareTo(End) <= 0);
        }

        public bool InRange(T value)
        {
            return (Start.CompareTo(value) <= 0) && (value.CompareTo(End) <= 0);
        }

        /// <summary>
        /// Determines if this Range is inside the bounds of another range
        /// </summary>
        /// <param name="range">The parent range to test on</param>
        /// <returns>True if range is inclusive, else false</returns>
        public bool IsInRange(Range<T> range)
        {
            return IsValid() && range.IsValid() && range.ContainsValue(Start) && range.ContainsValue(End);
        }

        /// <summary>
        /// Determines if another range is inside the bounds of this range
        /// </summary>
        /// <param name="range">The child range to test</param>
        /// <returns>True if range is inside, else false</returns>
        public bool ContainsRange(Range<T> range)
        {
            return IsValid() && range.IsValid() && ContainsValue(range.Start) && ContainsValue(range.End);
        }

        #region Implementation of IEnumerable

        public IEnumerator<T> GetEnumerator()
        {
            return GetArray();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        protected virtual IEnumerator<T> GetArray()
        {
            return null;
        }

        #endregion
    }

    public class Range : Range<int>
    {
        public Range(int start, int end)
            : base(start, end)
        {
        }

        protected override IEnumerator<int> GetArray()
        {
            var r = new List<int>();

            for (int i = Start; i <= End; i++)
            {
                r.Add(i);
            }

            return r.GetEnumerator();
        }
    }

    public class ByteRange : Range<byte>
    {
        public ByteRange(byte start, byte end)
            : base(start, end)
        {
        }

        protected override IEnumerator<byte> GetArray()
        {
            var r = new List<byte>();

            for (int i = Start; i <= End; i++)
            {
                r.Add(byte.Parse(true.ToString()));
            }

            return r.GetEnumerator();
        }
    }

    public class CharRange : Range<char>
    {
        public CharRange(char start, char end)
            : base(start, end)
        {
        }

        protected override IEnumerator<char> GetArray()
        {
            var r = new List<char>();

            for (int i = Start; i <= End; i++)
            {
                r.Add(char.Parse(i.ToString()));
            }

            return r.GetEnumerator();
        }
    }

    public class AlphaRange : Range<char>
    {
        private readonly int _end;
        private readonly int _start;
        private string _alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZäöüabcdefghijklmnopqrstuvwxyzäöü";

        public AlphaRange(char start, char end)
            : base(start, end)
        {
            _start = _alpha.IndexOf(start);
            _end = _alpha.LastIndexOf(end);
        }

        protected override IEnumerator<char> GetArray()
        {
            string r = _alpha.Substring(_start, _end - _start);
            List<char> rc = r.ToCharArray().ToList();

            return rc.GetEnumerator();
        }
    }
}