namespace Creek.Text
{
    using System;
    using System.Collections.Generic;

    public class StringGenerator
    {
        private const string data = "gugsohTDFSUIDSOT$WEQ)$%HFbdduir8954pjrh455689789465132utiruoepwösdn";

        private static Random rndm = new Random();
        private static string Gen(int length)
        {
            var ret = new List<char>();

            for (int i = 0; i < length*rndm.Next(1, 25); i++)
            {
                ret.Add(data[rndm.Next(0, data.Length)]);
            }
            ret.Add('\r');

            return new string(ret.ToArray());
        }

        public static string Generate(int times = 100)
        {
            var ret = new List<char>();
            for (var i = 0; i < times; i++)
            {
                ret.AddRange(Gen(100).ToCharArray());
            }

            return new string(ret.ToArray());
        }

    }
}
