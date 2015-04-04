using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Creek.Tools
{
    [Flags]
    public enum CharacterType
    {
        Space = 1,
        Digit = 2,
        UpperCase = 4,
        LowerCase = 8,
        Symbol = 16
    }

    public class Rndm
    {
        public static int GetInt(int min, int max)
        {
            var rr = new IntegerRandomGenerator(min, max);

            return rr.GetRandom();
        }

        public static double GetDouble(double min, double max)
        {
            var rr = new DoubleRandomGenerator(min, max);

            return rr.GetRandom();
        }

        public static bool GetBoolean()
        {
            var rr = new BooleanRandomGen();

            return rr.GetRandom();
        }

        public static DateTime GetDate(DateTime min, DateTime max)
        {
            var rr = new DateRandomGenerator(min, max);

            return rr.GetRandom();
        }

        public static t GetEnum<t>()
        {
            var rr = new EnumRandomGenerator<t>();

            return rr.GetRandom();
        }

        public static t GetList<t>(IList<t> list)
        {
            var rr = new ListRandomGenerator<t>(list);

            return rr.GetRandom();
        }

        public static string GetString(int maxlength, CharacterType ct)
        {
            var rr = new StringRandomGenerator(maxlength, ct);

            return rr.GetRandom();
        }
    }

    public abstract class RandomGeneratorBase<T>
    {
        protected Random _random = new Random(int.Parse(
            Guid.NewGuid().ToString().Substring(0, 8),
            NumberStyles.HexNumber));

        public abstract T GetRandom();
    }

    public class BooleanRandomGen : RandomGeneratorBase<bool>
    {
        public override bool GetRandom()
        {
            return (_random.NextDouble() >= 0.5);
        }
    }

    public class IntegerRandomGenerator : RandomGeneratorBase<int>
    {
        private readonly int _max;
        private readonly int _min;

        public IntegerRandomGenerator(int min, int max)
        {
            _min = min;
            _max = max;
        }

        public override int GetRandom()
        {
            return _random.Next(_min, _max);
        }
    }

    public class DoubleRandomGenerator : RandomGeneratorBase<double>
    {
        private readonly double _max;
        private readonly double _min;

        public DoubleRandomGenerator(double min, double max)
        {
            _min = min;
            _max = max;
        }

        public override double GetRandom()
        {
            return ((_max - _min)*_random.NextDouble()) + _min;
        }
    }

    public class StringRandomGenerator : RandomGeneratorBase<string>
    {
        private const bool DEFAULT_PAD_RAIGHT = false;
        private readonly char[] _chars;
        private readonly int _max;
        private readonly int _min;
        private readonly bool _padRight = DEFAULT_PAD_RAIGHT;

        public StringRandomGenerator(int maxLength, CharacterType charType)
            : this(0, maxLength, charType, DEFAULT_PAD_RAIGHT)
        {
        }

        public StringRandomGenerator(int maxLength, CharacterType charType, bool padRight)
            : this(0, maxLength, charType, padRight)
        {
        }

        public StringRandomGenerator(int minLength, int maxLength, CharacterType charType)
            : this(minLength, maxLength, charType, DEFAULT_PAD_RAIGHT)
        {
        }

        public StringRandomGenerator(int minLength, int maxLength, CharacterType charType, bool padRight)
        {
            _min = minLength;
            _max = maxLength;
            _chars = RandomGeneratorHelper.GetChars(charType);
            _padRight = padRight;
        }

        public override string GetRandom()
        {
            int n = 0;
            if (_min == _max)
            {
                n = _max;
            }
            else
            {
                n = GetRandomLength();
            }
            var sb = new StringBuilder();
            for (int i = 0; i <= n - 1; i++)
            {
                sb.Append(GetRandomChar());
            }
            if (_padRight)
            {
                return sb.ToString().PadRight(_max);
            }
            else
            {
                return sb.ToString();
            }
        }

        private int GetRandomLength()
        {
            return RandomGeneratorHelper.GetRandomInteger(_min, _max + 1);
        }

        private char GetRandomChar()
        {
            int rnd = RandomGeneratorHelper.GetRandomInteger(0, _chars.GetUpperBound(0) + 1);
            return _chars[rnd];
        }
    }

    public class RandomGeneratorHelper
    {
        private static readonly Random _random = new Random(DateTime.Now.Millisecond*DateTime.Now.Second);

        public static int GetRandomInteger()
        {
            return GetRandomInteger(true);
        }

        public static int GetRandomInteger(bool onlyPositive)
        {
            if (onlyPositive)
            {
                return GetRandomInteger(0, int.MaxValue);
            }
            else
            {
                return GetRandomInteger(int.MinValue, int.MaxValue);
            }
        }

        public static int GetRandomInteger(int min, int max)
        {
            return _random.Next(min, max);
        }

        public static char[] GetChars(CharacterType cht)
        {
            char[] ch = null;
            if ((cht & CharacterType.Digit) > 0)
            {
                ch = ReziseAndAppendCharArray(ch, GetDigits());
            }
            if ((cht & CharacterType.UpperCase) > 0)
            {
                ch = ReziseAndAppendCharArray(ch, GetUpperCase());
            }
            if ((cht & CharacterType.LowerCase) > 0)
            {
                ch = ReziseAndAppendCharArray(ch, GetLowerCase());
            }
            if ((cht & CharacterType.Space) > 0)
            {
                char[] spc = {' '};
                ch = ReziseAndAppendCharArray(ch, spc);
            }
            if ((cht & CharacterType.Symbol) > 0)
            {
                char[] spc = GetChars("!-/*");
                ch = ReziseAndAppendCharArray(ch, spc);
            }
            return ch;
        }

        private static char[] ReziseAndAppendCharArray(char[] mainArray, char[] toBeCopiedArray)
        {
            if (mainArray == null)
            {
                mainArray = toBeCopiedArray;
            }
            else
            {
                int oldLength = mainArray.Length;
                var newArray = new char[oldLength + toBeCopiedArray.Length];
                Array.Copy(mainArray, 0, newArray, 0, oldLength);
                Array.Copy(toBeCopiedArray, 0, newArray, oldLength, toBeCopiedArray.Length);
                mainArray = newArray;
            }
            return mainArray;
        }

        private static char[] GetChars(string charRange)
        {
            string[] ss = charRange.Split('-');
            char ch1 = ss[0][0];
            char ch2 = ss[1][0];
            var ch = new char[Convert.ToInt32(ch2) - Convert.ToInt32(ch1) + 1];
            for (int i = Convert.ToInt32(ch1); i <= Convert.ToInt32(ch2); i++)
            {
                ch[i - Convert.ToInt32(ch1)] = Convert.ToChar(i);
            }
            return ch;
        }

        private static char[] GetDigits()
        {
            char[] ch = {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9'};
            return ch;
        }

        private static char[] GetUpperCase()
        {
            var ch = new char[26];
            int A_ASC_VALUE = Convert.ToInt32('A');
            for (int i = A_ASC_VALUE; i < A_ASC_VALUE + 26; i++)
            {
                ch[i - A_ASC_VALUE] = Convert.ToChar(i);
            }
            return ch;
        }

        private static char[] GetLowerCase()
        {
            var ch = new char[26];
            int A_ASC_VALUE = Convert.ToInt32('a');
            for (int i = A_ASC_VALUE; i < A_ASC_VALUE + 26; i++)
            {
                ch[i - A_ASC_VALUE] = Convert.ToChar(i);
            }
            return ch;
        }
    }


    public class ListRandomGenerator<T> : RandomGeneratorBase<T>
    {
        private readonly IList<T> _list = new List<T>();
        private readonly bool _unique;

        public ListRandomGenerator(IList<T> list) : this(list, false, false)
        {
        }

        public ListRandomGenerator(IList<T> list, bool unique, bool makeLocalCopyOfList)
        {
            _unique = unique;

            if (unique && makeLocalCopyOfList)
            {
                // make a copy 
                _list = new List<T>();
                foreach (T o in list)
                    _list.Add(o);
            }
            else
                _list = list;
        }

        public override T GetRandom()
        {
            if (_unique)
            {
                if (_list.Count == 0)
                    throw new InvalidOperationException("The list is exhausted. No more unique items could be returned.");
                int index = base._random.Next(0, _list.Count);
                T o = _list[index];
                _list.RemoveAt(index);
                return o;
            }
            else
                return _list[base._random.Next(0, _list.Count)];
        }
    }

    public class PrioritisedListRandomGenerator<T> : RandomGeneratorBase<T>
    {
        private readonly List<T> _list = new List<T>();

        public PrioritisedListRandomGenerator(IList<T> items, int maxScore)
        {
            foreach (T item in items)
            {
                for (int i = 0; i < _random.Next(1, maxScore); i++)
                    _list.Add(item);
            }
        }

        public PrioritisedListRandomGenerator(Dictionary<T, int> itemAsKeyIntegerScoreAsValue)
        {
            foreach (T key in itemAsKeyIntegerScoreAsValue.Keys)
            {
                int count = itemAsKeyIntegerScoreAsValue[key];
                for (int i = 0; i < count; i++)
                    _list.Add(key);
            }
        }

        public override T GetRandom()
        {
            return _list[base._random.Next(0, _list.Count)];
        }
    }

    public class DateRandomGenerator : RandomGeneratorBase<DateTime>
    {
        private readonly long _maxDate = DateTime.Now.Ticks;
        private readonly long _minDate = DateTime.MinValue.Ticks;

        public DateRandomGenerator() : this(DateTime.MinValue, DateTime.Now)
        {
        }

        public DateRandomGenerator(DateTime minDate) : this(minDate, DateTime.Now)
        {
        }

        public DateRandomGenerator(DateTime minDate, DateTime maxDate)
        {
            _minDate = minDate.Ticks;
            _maxDate = maxDate.Ticks;
        }

        public override DateTime GetRandom()
        {
            return new DateTime((long) ((_random.NextDouble()*(_maxDate - _minDate)) + _minDate));
        }
    }

    public class EnumRandomGenerator<T> : ListRandomGenerator<T>
    {
        public EnumRandomGenerator()
            : base(new List<T>((IList<T>) Enum.GetValues(typeof (T))))
        {
        }
    }
}