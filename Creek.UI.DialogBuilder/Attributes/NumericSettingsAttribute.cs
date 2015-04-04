using System;

namespace Creek.UI.DialogBuilder.Attributes
{
  [AttributeUsage(AttributeTargets.Property)]
  public class NumericSettingsAttribute : Attribute
  {
    public float MinValue { get; set; }
    public float MaxValue { get; set; }
    public int DecimalPlaces { get; set; }
  }
}
