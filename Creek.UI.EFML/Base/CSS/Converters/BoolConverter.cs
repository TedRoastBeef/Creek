namespace Creek.UI.EFML.Base.CSS.Converters
{
    public class BoolConverter : IConverter<bool>
    {
        #region Overrides of IConverter<bool>

        public override bool Convert(string s)
        {
            return bool.Parse(s);
        }

        public override string Convert(bool s)
        {
            return s.ToString();
        }

        #endregion
    }
}