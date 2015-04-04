using System.Collections.Generic;

namespace Creek.Validation
{
    public partial class Validator
    {
        /**
     * <summary>
     * This method will validate the given method
     * </summary>
     * <remarks>
     * Validation will only work if the object
     * contains specific validation attributes
     * </remarks>
     * <returns>
     * A list of string values representing the errors
     * </returns>
     */

        public static IList<string> Validate(object o)
        {
            return Validate(o, false, "Object cannot be null");
        }

//end Validate
        /**
     * <summary>
     * This method will validate the given method
     * </summary>
     * <remarks>
     * Validation will only work if the object
     * contains specific validation attributes
     * </remarks>
     * <returns>
     * A list of string values representing the errors
     * </returns>
     */

        public static IList<string> Validate
            (object o, bool allowNullObject, string nullMessage)
        {
            var errors = new List<string>();
            if (o != null)
            {
                foreach (var info in o.GetType().GetProperties())
                {
                    foreach (var customAttribute in info.GetCustomAttributes
                        (typeof (IDbValidationAttribute), true))
                    {
                        ((IDbValidationAttribute) customAttribute).Validate
                            (o, info, errors);
                        if (info.PropertyType.IsClass ||
                            info.PropertyType.IsInterface)
                        {
                            errors.AddRange(Validate
                                                (info.GetValue(o, null), true, null));
                        }
                    }
                } //end foreach
                foreach (var method in o.GetType().GetMethods())
                {
                    foreach (var customAttribute in method.GetCustomAttributes
                        (typeof (IDbValidationAttribute), true))
                    {
                        ((IDbValidationAttribute) customAttribute).Validate
                            (o, method, errors);
                    }
                }
            }
            else if (!allowNullObject)
            {
                errors.Add(nullMessage);
            }
            return errors;
        }
    }
}