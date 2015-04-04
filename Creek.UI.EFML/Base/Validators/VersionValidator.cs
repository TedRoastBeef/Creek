namespace Creek.UI.EFML.Base.Validators
{
    public class VersionValidator : IValidator
    {
        #region Overrides of IValidator

        public override string Name
        {
            get { return "version"; }
        }

        public override string Pattern
        {
            get { return @"\d+\.\d+\.\d+\.[0-9F]+"; }
        }

        #endregion
    }
}