using System;

namespace Creek.UI.DialogBuilder
{
  internal class ControlTag
  {
    public string CustomLabel { get; set; }
    public string ErrorMessage { get; set; }
    public string PropertyName { get; set; }
    public Type PropertyType { get; set; }

    public bool IsRequired
    {
      get { return !string.IsNullOrEmpty(ErrorMessage); }
    }
  }
}
