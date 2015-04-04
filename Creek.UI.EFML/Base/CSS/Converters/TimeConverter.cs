using System;

namespace Creek.UI.EFML.Base.CSS.Converters
{
    public class TimeConverter : IConverter<int>
    {
        #region Overrides of IConverter<int>

        public override int Convert(string s)
        {
            if (s.EndsWith("ms"))
                return int.Parse(s.Remove(s.Length - 2, 2));
            if(s.EndsWith("s"))
                return int.Parse(s.Remove(s.Length - 1, 1))*1000;
            return int.Parse(s);
        }

        public override string Convert(int s)
        {
            return s + "ms";
        }

        #endregion
    }
}