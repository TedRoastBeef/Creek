using System.Collections.Generic;
using System.Reflection;

namespace Creek.Validation
{
    public interface IDbValidationAttribute
    {
        void Validate(object o, PropertyInfo propertyInfo, IList<string> errors);
        void Validate(object o, MethodInfo methodInfo, IList<string> errors);
    }
}