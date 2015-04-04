namespace Creek.UI.Theming
{
    using System.Drawing;

    public class LightThemeColors : IFormThemeColors, IThumbnailThemeColors, IImageViewerThemeColors,
        IPropertyGridThemeColors, IFolderTreeViewThemeColors, IShellListViewThemeColors
    {
        #region Fields

        private Color dialogBackColor = Color.FromArgb(184, 196, 208);

        private Color formBackColor = Color.White;

        private Color imageBackColor = Constants.LightBlueGray;

        private Color propertyGridBackColor = Color.FromArgb(184, 196, 208);

        private Color propertyGridCategorySplitterColor = Constants.LightBlueGray;

        private Color propertyGridHelpBackColor = Constants.LightBlueGray;

        private Color propertyGridHelpBorderColor = SystemColors.ControlDark;

        private Color propertyGridLineColor = Constants.LightBlueGray;

        private Color propertyGridSelectedItemWithFocusBackColor = Color.SlateGray;

        private Color propertyGridViewBackColor = Color.White;

        private Color propertyGridViewBorderColor = SystemColors.ControlDark;

        private Color shellListViewTextColor = Color.Black;

        private Color themeableControlBackColor = Constants.LightThemeableControlBackColor;

        private Color thumbnailHottrackBorderColor = Color.FromArgb(158, 168, 182);

        private Color thumbnailHottrackGradientBottomColor = Color.FromArgb(223, 238, 255);

        private Color thumbnailHottrackGradientTopColor = Color.White;

        private Color thumbnailPlaceHolderGradientBottomColor = Color.FromArgb(245, 247, 249);

        private Color thumbnailPlaceHolderGradientTopColor = Color.Silver;

        private Color thumbnailSelectedBorderColor = Color.FromArgb(81, 87, 95);

        private Color thumbnailSelectedGradientBottomColor = Color.FromArgb(245, 247, 249);

        private Color thumbnailSelectedGradientTopColor = Color.FromArgb(145, 179, 239);

        private Color thumbnailTextColor = Color.Black;

        private Color treeTextColor = Color.Black;

        #endregion Fields

        #region Properties

        public Color DialogBackColor
        {
            get { return this.dialogBackColor; }
            set { this.dialogBackColor = value; }
        }

        public Color FormBackColor
        {
            get { return this.formBackColor; }
            set { this.formBackColor = value; }
        }

        public Color ImageBackColor
        {
            get { return this.imageBackColor; }
            set { this.imageBackColor = value; }
        }

        public Color PropertyGridBackColor
        {
            get { return this.propertyGridBackColor; }
            set { this.propertyGridBackColor = value; }
        }

        public Color PropertyGridCategorySplitterColor
        {
            get { return this.propertyGridCategorySplitterColor; }
            set { this.propertyGridCategorySplitterColor = value; }
        }

        public Color PropertyGridHelpBackColor
        {
            get { return this.propertyGridHelpBackColor; }
            set { this.propertyGridHelpBackColor = value; }
        }

        public Color PropertyGridHelpBorderColor
        {
            get { return this.propertyGridHelpBorderColor; }
            set { this.propertyGridHelpBorderColor = value; }
        }

        public Color PropertyGridLineColor
        {
            get { return this.propertyGridLineColor; }
            set { this.propertyGridLineColor = value; }
        }

        public Color PropertyGridSelectedItemWithFocusBackColor
        {
            get { return this.propertyGridSelectedItemWithFocusBackColor; }
            set { this.propertyGridSelectedItemWithFocusBackColor = value; }
        }

        public Color PropertyGridViewBackColor
        {
            get { return this.propertyGridViewBackColor; }
            set { this.propertyGridViewBackColor = value; }
        }

        public Color PropertyGridViewBorderColor
        {
            get { return this.propertyGridViewBorderColor; }
            set { this.propertyGridViewBorderColor = value; }
        }

        public Color ShellListViewTextColor
        {
            get { return this.shellListViewTextColor; }
            set { this.shellListViewTextColor = value; }
        }

        public Color ThemeableControlBackColor
        {
            get { return this.themeableControlBackColor; }
            set { this.themeableControlBackColor = value; }
        }

        public Color ThumbnailHottrackBorderColor
        {
            get { return this.thumbnailHottrackBorderColor; }
            set { this.thumbnailHottrackBorderColor = value; }
        }

        public Color ThumbnailHottrackGradientBottomColor
        {
            get { return this.thumbnailHottrackGradientBottomColor; }
            set { this.thumbnailHottrackGradientBottomColor = value; }
        }

        public Color ThumbnailHottrackGradientTopColor
        {
            get { return this.thumbnailHottrackGradientTopColor; }
            set { this.thumbnailHottrackGradientTopColor = value; }
        }

        public Color ThumbnailPlaceHolderGradientBottomColor
        {
            get { return this.thumbnailPlaceHolderGradientBottomColor; }
            set { this.thumbnailPlaceHolderGradientBottomColor = value; }
        }

        public Color ThumbnailPlaceHolderGradientTopColor
        {
            get { return this.thumbnailPlaceHolderGradientTopColor; }
            set { this.thumbnailPlaceHolderGradientTopColor = value; }
        }

        public Color ThumbnailSelectedBorderColor
        {
            get { return this.thumbnailSelectedBorderColor; }
            set { this.thumbnailSelectedBorderColor = value; }
        }

        public Color ThumbnailSelectedGradientBottomColor
        {
            get { return this.thumbnailSelectedGradientBottomColor; }
            set { this.thumbnailSelectedGradientBottomColor = value; }
        }

        public Color ThumbnailSelectedGradientTopColor
        {
            get { return this.thumbnailSelectedGradientTopColor; }
            set { this.thumbnailSelectedGradientTopColor = value; }
        }

        public Color ThumbnailTextColor
        {
            get { return this.thumbnailTextColor; }
            set { this.thumbnailTextColor = value; }
        }

        public Color TreeTextColor
        {
            get { return this.treeTextColor; }
            set { this.treeTextColor = value; }
        }

        #endregion Properties
    }
}
