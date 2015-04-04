namespace Creek.UI.Theming
{
    using System;

    /// <summary>My simple custom themes design.</summary>
    /// <remarks>
    /// <para>
    /// This <b>Theme</b> enum defines the names of the supported themes. My themes simply allow assigning all the
    /// user interface item colours that I predecided are "themeable", at once. The <see cref="DarkThemeColors"/>,
    /// <seealso cref="LightThemeColors"/> and <seealso cref="MonoThemeColors"/> types correspond with the themes.
    /// </para><para>
    /// Then, for each control that I decide is themeable (typically a Form, but it doesn't have to be), I define
    /// an interface whose members are all <b>Color</b> properties that represent all the themeable properties of
    /// the type. <b>DarkThemeColors</b>, <b>LightThemeColors</b> and <b>MonoThemeColors</b> must implement all
    /// the interfaces. At runtime, whichever implementation is current (exposed as the dynamic
    /// <see cref="Romy.Controls.BaseForm.ThemeColors"/>) property, can simply be cast to whatever interface is
    /// needed by each control.</para></remarks>
    public enum Theme
    {
        Light,
        Dark,
        Monochrome
    }

    public class ThemeChangedEventArgs : EventArgs
    {
        public ThemeChangedEventArgs(Theme newTheme)
        {
            this.NewTheme = newTheme;
        }

        public Theme NewTheme { get; set; }
    }
}
