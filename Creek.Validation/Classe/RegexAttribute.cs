using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Creek.Validation
{

    public struct Patterns
    {
        public const string URL = "(http\\:\\/\\/|www.).*\\.[a-z]{1-4}";
        public const string EMail = "[a-zA-Z0-9|a-zA-Z0-9\\.]+\\@[a-zA-Z0-9|a-zA-Z0-9\\-]+\\.[com|de|net|mu]+";
        public const string Date = "^\\d{1,2}\\/\\d{1,2}\\/\\d{2,4}$";
        public const string Time = "^\\d{1,2}:\\d{2}\\s?([ap]m)?$";
        public const string Phone = "(\\(?[0-9]{3}\\)?)?\\-?[0-9]{3}\\-?[0-9]{4}(\\s*ext(ension)?[0-9]{5})?$";
        public const string KeyEqualsValue = "(\\w+)\\s*=\\s*(.*)\\s*$";
        public const string String = "\"[^\"\r\n]*\"";
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class RegexAttribute : Attribute, IDbValidationAttribute
    {
        private string mMessage = "{0} is not valid";

        public string Pattern { get; set; }

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
            if (!new Regex(Pattern).IsMatch(value.ToString()))
            {
                errors.Add(String.Format(mMessage, propertyInfo.Name));
            }
        }
    }
}