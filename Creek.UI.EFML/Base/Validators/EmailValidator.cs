namespace Creek.UI.EFML.Base.Validators
{
    public class EmailValidator : IValidator
    {
        #region Overrides of IValidator

        public override string Name
        {
            get { return "email"; }
        }

        public override string Pattern
        {
            get
            {
                return @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
                       + "@"
                       + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$";
            }
        }

        #endregion
    }
}