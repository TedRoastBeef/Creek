using System;

namespace Creek.UI.DialogBuilder.Attributes
{
  [AttributeUsage(AttributeTargets.Property)]
  public class RequiredFieldAttribute : Attribute
  {
    public string Message { get; set; }
  }
}
