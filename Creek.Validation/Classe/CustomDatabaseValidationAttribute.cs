using System;
using System.Collections.Generic;
using System.Reflection;

namespace Creek.Validation
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CustomDatabaseValidationAttribute :
        Attribute, IDbValidationAttribute
    {

        public void Validate(object o, PropertyInfo propertyInfo,
                             IList<string> errors) { }

        public void Validate(object o, MethodInfo info, IList<string> errors)
        {
            var result = (IList<string>)info.Invoke(o, null);
            foreach (string abc in result)
            {
                errors.Add(abc);
            }
        }
    }
}