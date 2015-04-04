using System;

namespace EFMLEditor.Internal.Validating.Attributes
{
    using System.Reflection;

    public abstract class ValidatingAttribute : Attribute
    {
        public string Message { get; set; }
        public abstract bool Do(object inpt, ParameterInfo parameter);
    }
}
