using System;
using Creek.Behaviors;

namespace BehaviorTest
{
    class ConvertTest : ConvertBehavior<int>
    {

        public double ParseTest(string s)
        {
            return Parse<double>(s);
        }
        public bool TryParseTest(string s)
        {
            return TryParse<DateTime>(s);
        }

        public int[] ArrayTest(object[] o)
        {
            return Array(o);
        }
   

    }
}