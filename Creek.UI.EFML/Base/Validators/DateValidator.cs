namespace Creek.UI.EFML.Base.Validators
{
    public class DateValidator : IValidator
    {
        #region Overrides of IValidator

        public override string Name
        {
            get { return "date"; }
        }

        public override string Pattern
        {
            get
            {
                return
                    @"(^((((0[1-9])|([1-2][0-9])|(3[0-1]))|([1-9]))\x2F(((0[1-9])|(1[0-2]))|([1-9]))\x2F(([0-9]{2})|(((19)|([2]([0]{1})))([0-9]{2}))))$)";
            }
        }

        #endregion
    }
}