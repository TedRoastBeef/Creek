using System;
using System.Collections.Generic;
using System.Reflection;

namespace Creek.Validation
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FieldNullableAttribute : Attribute, IDbValidationAttribute
    {
        private string mMessage = "{0} cannot be null";

        public FieldNullableAttribute()
        {
            IsNullable = false;
        }

        public bool IsNullable { get; set; }

        public string Message
        {
            get
            {
                return mMessage;
            }
            set {
                mMessage = value ?? String.Empty;
            }
        }
        public void Validate(object o, MethodInfo info,
                             IList<string> errors) { }
        public void Validate(object o, PropertyInfo propertyInfo,
                             IList<string> errors)
        {
            var value = propertyInfo.GetValue(o, null);
            if (value == null && !IsNullable)
            {
                errors.Add(String.Format(mMessage, propertyInfo.Name));
            }
        }
    }
}