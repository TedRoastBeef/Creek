using System;

namespace Creek.UI.EFML.Base.CSS.Converters
{
    public class EnumConverter
    {
        public static T Convert<T>(string s)
        {
            return (T) Enum.Parse(typeof (T), s);
        }

        public static object Convert(Type t, string s)
        {
            return Enum.Parse(t, s);
        }
    }
}