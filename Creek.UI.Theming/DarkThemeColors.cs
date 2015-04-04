namespace Creek.UI.Theming
{
    using System.Drawing;

    public class DarkThemeColors : IFormThemeColors, IThumbnailThemeColors, IImageViewerThemeColors,
        IPropertyGridThemeColors, IFolderTreeViewThemeColors, IShellListViewThemeColors
    {
        #region Fields 

        private Color dialogBackColor = Color.FromArgb(102, 110, 138);

        private Color formBackColor = Color.FromArgb(30, 30, 30);

        private Color imageBackColor = Color.FromArgb(20, 20, 30);

        private Color propertyGridBackColor = Color.FromArgb(102, 110, 138);

        private Color propertyGridCategorySplitterColor = Constants.DarkThemeableControlBackColor;

        private Color propertyGridHelpBackColor = Constants.DarkThemeableControlBackColor;

        private Color propertyGridHelpBorderColor = Color.FromArgb(60, 70, 73);

        private Color propertyGridLineColor = Constants.DarkThemeableControlBackColor;

        private Color propertyGridselectedItemWithFocusBackColor = Color.DarkSlateGray;

        private Color propertyGridViewBackColor = Color.FromArgb(226, 232, 235);

        private Color propertyGridViewBorderColor = Color.FromArgb(60, 70, 73);

        private Color shellListViewTextColor = Color.White;

        private Color themeableControlBackColor = Constants.DarkThemeableControlBackColor;

        private Color thumbnailHottrackBorderColor = Color.FromArgb(191, 215, 238);

        private Color thumbnailHottrackGradientBottomColor = Color.FromArgb(80, 80, 90);

        private Color thumbnailHottrackGradientTopColor = Color.FromArgb(30, 40, 50);

        private Color thumbnailPlaceHolderGradientBottomColor = Color.FromArgb(245, 247, 249);

        private Color thumbnailPlaceHolderGradientTopColor = Color.FromArgb(46, 86, 97);

        private Color thumbnailSelectedBorderColor = Color.White;

        private Color thumbnailSelectedGradientBottomColor = Color.FromArgb(214, 225, 236);

        private Color thumbnailSelectedGradientTopColor = Color.FromArgb(27, 60, 117);

        private Color thumbnailTextColor = Color.White;

        private Color treeTextColor = Color.White;

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
            get { return this.propertyGridselectedItemWithFocusBackColor; }
            set { this.propertyGridselectedItemWithFocusBackColor = value; }
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
