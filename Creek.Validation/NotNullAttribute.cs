namespace Creek.Validation
{
    using System.Reflection;

    using EFMLEditor.Internal.Validating.Attributes;

    public class NotNullAttribute : ValidatingAttribute
    {
        #region Overrides of ValidatingAttribute

        public override bool Do(object inpt, ParameterInfo parameter)
        {
            bool con = inpt != null;

            if (!con)
            {
                this.Message = parameter.Name + " cannot be null";
            }

            return con;
        }

        #endregion
    }
}