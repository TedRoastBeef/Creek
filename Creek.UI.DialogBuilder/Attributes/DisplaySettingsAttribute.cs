using System;

namespace Creek.UI.DialogBuilder.Attributes
{
  [AttributeUsage(AttributeTargets.Property)]
  public class DisplaySettingsAttribute : Attribute
  {
    public string Label { get; set; }
    public bool ReadOnly { get; set; }
    public bool Visible { get; set; }
    public int Width { get; set; }

    public DisplaySettingsAttribute()
    {
      Visible = true;
    }
  }
}
