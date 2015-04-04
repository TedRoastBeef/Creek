namespace Creek.Validation
{
    using System.Reflection;

    using EFMLEditor.Internal.Validating.Attributes;

    public class NotEmptyAttribute : ValidatingAttribute
    {
        #region Overrides of ValidatingAttribute

        public override bool Do(object inpt, ParameterInfo parameter)
        {
            bool con = inpt.ToString() != "";

            if (!con)
            {
                this.Message = parameter.Name + " cannot be empty";
            }

            return con;
        }

        #endregion
    }
}