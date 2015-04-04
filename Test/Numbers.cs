using System;
using Creek.Behaviors;

namespace Test
{
    public sealed class Numbers : EnumBehavior<Numbers>
    {
        public static readonly Numbers Zero = new Numbers("ZERO");
        public static readonly Numbers One = new Numbers("ONE");
        public static readonly Numbers Two = new Numbers("TWO");
        public static readonly Numbers Three = new Numbers("THREE");

        public Numbers(object value) : base(value)
        {
        }

        public new static Numbers Parse(string s)
        {
            switch (s)
            {
                case "0":
                    return Zero;
                case "1":
                    return One;
                case "2":
                    return Two;
                case "3":
                    return Three;
                default:
                    throw new Exception(s + " is not in Enum Numbers");
            }
        }
    }
}