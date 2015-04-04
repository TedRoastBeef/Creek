namespace Creek.UI.Theming
{
    using System;
    using System.Drawing;

    /// <summary>A control that changes colours according to the active theme, implementing one or more of the
    /// other interfaces defined below.</summary>
    /// <remarks><para>
    /// The <b>ThemeChanged</b> event is implemented by the base form class. Derived forms need not implement this
    /// event. Instead, they override <see cref="BaseForm.LoadTheme()"/> in response to the theme being changed.</para>
    /// <para>
    /// Besides the properties defined here, forms also change their ToolStrip (and related) background images and their
    /// ToolStrip item images. See the <seealso cref="BaseForm.OnThemeChanged(ThemeChangedEventArgs)"/> handler for details.</para>
    /// <para>
    /// The <b>BaseForm</b> implements the themes by association of instances of three types, each of which implements all of the theme
    /// interfaces: <see cref="DarkThemeColors"/>, <see cref="LightThemeColors"/> and <see cref="MonoThemeColors"/>/</para></remarks>
    public interface IThemeableControl
    {
        /// <summary>The themes are <see cref="System.Drawing.Color"/> properties
        /// defined by several interfaces for different types.</summary>
        Theme Theme { get; }

        /// <summary>Indicates that the theme has changed.</summary>
        event EventHandler<ThemeChangedEventArgs> ThemeChanged;
    }

    /// <summary>Themeable properties for forms and dialogs.</summary>
    public interface IFormThemeColors
    {
        /// <summary>Background colour for dialogs.</summary>
        Color DialogBackColor { get; set; }

        /// <summary>Background colour for forms.</summary>
        Color FormBackColor { get; set; }

        /// <summary>Background colour for Panels and any controls where a single custom colour sufficiently themes the control.</summary>
        /// <remarks>For any control that needs more complex theming, a control-specific interface is added.</remarks>
        Color ThemeableControlBackColor { get; set; }
    }

    /// <summary>Themeable properties for the image viewer.</summary>
    public interface IImageViewerThemeColors
    {
        /// <summary>Background colour of the image viewer.</summary>
        Color ImageBackColor { get; set; }
    }

    /// <summary>Themeable properties for property grids.</summary>
    public interface IPropertyGridThemeColors
    {
        Color PropertyGridBackColor { get; set; }

        Color PropertyGridCategorySplitterColor { get; set; }

        Color PropertyGridHelpBackColor { get; set; }

        Color PropertyGridHelpBorderColor { get; set; }

        Color PropertyGridLineColor { get; set; }

        Color PropertyGridSelectedItemWithFocusBackColor { get; set; }

        Color PropertyGridViewBackColor { get; set; }

        Color PropertyGridViewBorderColor { get; set; }
    }

    /// <summary>Themeable properties for the FolderTreeView. (This TreeView is <b>not owner-drawn</b>.
    /// It uses styles introduced in Windows Vista, hence we don't really customize it much.) We only
    /// change its text colour (according to its background, since it is drawn transparently).</summary>
    public interface IFolderTreeViewThemeColors
    {
        Color TreeTextColor { get; set; }
    }

    /// <summary>Themeable properties for the Shell ListView. We only need set its text colour, depending
    /// on its background, like the TreeView.</summary>
    public interface IShellListViewThemeColors
    {
        Color ShellListViewTextColor { get; set; }
    }

    /// <summary>Themeable properties for thumbnails.</summary>
    /// <remarks><para>
    /// Thumbnails are instances of my own control where <b>everything</b> is drawn in custom code, where
    /// the normal (Light) theme is designed to closely resemble thumbnails as drawn by Windows explorer.
    /// </para><para>
    /// Since the thumbnails use gradients for selection and hot-tracking, different colours are required
    /// for various properties, to match the surrounding ambient colours when changing the theme.</para>
    /// <para>
    /// Since there are several colours that change on Thumbnails, and the Thumbnail code is already complex
    /// enough without adding the theme code, the theme-related properties are implemented as a static instance
    /// of the <see cref="ThumbnailColorTable"/> helper type. This interface is implemented by another helper,
    /// the <see cref="ThumbnailThemer"/>.</para></remarks>
    public interface IThumbnailThemeColors
    {
        Color ThumbnailHottrackBorderColor { get; set; }

        Color ThumbnailHottrackGradientBottomColor { get; set; }

        Color ThumbnailHottrackGradientTopColor { get; set; }

        Color ThumbnailPlaceHolderGradientBottomColor { get; set; }

        Color ThumbnailPlaceHolderGradientTopColor { get; set; }

        Color ThumbnailSelectedBorderColor { get; set; }

        Color ThumbnailSelectedGradientBottomColor { get; set; }

        Color ThumbnailSelectedGradientTopColor { get; set; }

        Color ThumbnailTextColor { get; set; }
    }
}
