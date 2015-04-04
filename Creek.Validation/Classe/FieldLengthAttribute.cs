using System;
using System.Collections.Generic;
using System.Reflection;

namespace Creek.Validation
{ 
    [AttributeUsage(AttributeTargets.Property)]
    public class FieldLengthAttribute : Attribute, IDbValidationAttribute
    {
        private string mMessage = "{0} can only be {1} character(s) long";

        public int MaxLength { get; set; }

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
        public void Validate(object o, MethodInfo info, IList<string> errors) { }
        public void Validate(object o, PropertyInfo propertyInfo,
                             IList<string> errors)
        {
            var value = propertyInfo.GetValue(o, null);
            if (value is string)
            {
                if (MaxLength != 0 && ((string)value).Length >= MaxLength)
                {
                    errors.Add(String.Format
                                   (mMessage, propertyInfo.Name, MaxLength));
                }
            }
        }
    }
}