namespace Creek.Validation
{
    using System;
    using System.Diagnostics;
    using System.Linq.Expressions;
    using System.Reflection;

    using EFMLEditor.Internal.Validating.Attributes;

    public partial class Validator
    {
        [DebuggerStepThrough]
        public static void Validate<TClass>(Expression<Action<TClass>> expression, object parameters)
        {
            MethodInfo mi = MethodInfoHelper.GetMethodInfo(expression);
            foreach (var parameterInfo in mi.GetParameters())
            {
                object[] ca = parameterInfo.GetCustomAttributes(typeof(ValidatingAttribute), true);
                foreach (ValidatingAttribute o in ca)
                {
                    object value = null;
                    foreach (var pi in parameters.GetType().GetProperties())
                    {
                        if (parameterInfo.Name == pi.Name) value = pi.GetValue(parameters, null);
                    }
                    if(!o.Do(value, parameterInfo))
                    {
                        throw new ValidatingException(o.Message);
                    }
                }
            }
        }
    }
}