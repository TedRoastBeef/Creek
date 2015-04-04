namespace Creek.UI.EFML.Base.CSS.Converters
{
    public class NullConverter : IConverter<object>
    {
        #region Overrides of IConverter<bool>

        public override object Convert(string s)
        {
            return null;
        }

        public override string Convert(object s)
        {
            return "null";
        }

        #endregion
    }
}