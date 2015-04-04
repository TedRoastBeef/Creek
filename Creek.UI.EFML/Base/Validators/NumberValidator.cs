namespace Creek.UI.EFML.Base.Validators
{
    public class NumberValidator : IValidator
    {
        #region Overrides of IValidator

        public override string Name
        {
            get { return "number"; }
        }

        public override string Pattern
        {
            get { return @"^\d$"; }
        }

        #endregion
    }
}