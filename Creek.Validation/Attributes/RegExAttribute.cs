namespace Creek.Validation.Attributes
{
    using System.Reflection;
    using System.Text.RegularExpressions;

    using EFMLEditor.Internal.Validating.Attributes;

    public class RegExAttribute : ValidatingAttribute
    {
        private readonly string pattern;
        private readonly string message;

        public RegExAttribute(string pattern, string message)
        {
            this.pattern = pattern;
            this.message = message;
        }

        #region Overrides of ValidatingAttribute

        public override bool Do(object inpt, ParameterInfo parameter)
        {
            var con = Regex.IsMatch(inpt.ToString(), this.pattern);

            if (!con)
            {
                this.Message = string.Format(this.message, parameter.Name);
            }

            return con;
        }

        #endregion
    }
}