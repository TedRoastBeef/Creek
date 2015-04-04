using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Creek.UI.Metro.Components;
using Creek.UI.Metro.Drawing;
using Creek.UI.Metro.Interfaces;

namespace Creek.UI.Metro.Controls
{
    public class MetroContextMenu : ContextMenuStrip, IMetroControl
    {
        #region Interface

        private MetroColorStyle metroStyle = MetroColorStyle.Default;
        private MetroStyleManager metroStyleManager;
        private MetroThemeStyle metroTheme = MetroThemeStyle.Default;

        [Category("Metro Appearance")]
        public event EventHandler<MetroPaintEventArgs> CustomPaintBackground;

        [Category("Metro Appearance")]
        public event EventHandler<MetroPaintEventArgs> CustomPaint;

        [Category("Metro Appearance")]
        public event EventHandler<MetroPaintEventArgs> CustomPaintForeground;

        [Category("Metro Appearance")]
        [DefaultValue(MetroColorStyle.Default)]
        public MetroColorStyle Style
        {
            get
            {
                if (DesignMode || metroStyle != MetroColorStyle.Default)
                {
                    return metroStyle;
                }

                if (StyleManager != null && metroStyle == MetroColorStyle.Default)
                {
                    return StyleManager.Style;
                }
                if (StyleManager == null && metroStyle == MetroColorStyle.Default)
                {
                    return MetroColorStyle.Blue;
                }

                return metroStyle;
            }
            set { metroStyle = value; }
        }

        [Category("Metro Appearance")]
        [DefaultValue(MetroThemeStyle.Default)]
        public MetroThemeStyle Theme
        {
            get
            {
                if (DesignMode || metroTheme != MetroThemeStyle.Default)
                {
                    return metroTheme;
                }

                if (StyleManager != null && metroTheme == MetroThemeStyle.Default)
                {
                    return StyleManager.Theme;
                }
                if (StyleManager == null && metroTheme == MetroThemeStyle.Default)
                {
                    return MetroThemeStyle.Light;
                }

                return metroTheme;
            }
            set { metroTheme = value; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MetroStyleManager StyleManager
        {
            get { return metroStyleManager; }
            set
            {
                metroStyleManager = value;
                settheme();
            }
        }

        [DefaultValue(false)]
        [Category("Metro Appearance")]
        public bool UseCustomBackColor { get; set; }

        [DefaultValue(false)]
        [Category("Metro Appearance")]
        public bool UseCustomForeColor { get; set; }

        [DefaultValue(false)]
        [Category("Metro Appearance")]
        public bool UseStyleColors { get; set; }

        [Browsable(false)]
        [Category("Metro Behaviour")]
        [DefaultValue(false)]
        public bool UseSelectable
        {
            get { return GetStyle(ControlStyles.Selectable); }
            set { SetStyle(ControlStyles.Selectable, value); }
        }

        protected virtual void OnCustomPaintBackground(MetroPaintEventArgs e)
        {
            if (GetStyle(ControlStyles.UserPaint) && CustomPaintBackground != null)
            {
                CustomPaintBackground(this, e);
            }
        }

        protected virtual void OnCustomPaint(MetroPaintEventArgs e)
        {
            if (GetStyle(ControlStyles.UserPaint) && CustomPaint != null)
            {
                CustomPaint(this, e);
            }
        }

        protected virtual void OnCustomPaintForeground(MetroPaintEventArgs e)
        {
            if (GetStyle(ControlStyles.UserPaint) && CustomPaintForeground != null)
            {
                CustomPaintForeground(this, e);
            }
        }

        #endregion

        public MetroContextMenu(IContainer Container)
        {
            if (Container != null)
            {
                Container.Add(this);
            }
        }

        private void settheme()
        {
            BackColor = MetroPaint.BackColor.Form(Theme);
            ForeColor = MetroPaint.ForeColor.Button.Normal(Theme);
            Renderer = new MetroCTXRenderer(Theme, Style);
        }

        #region Nested type: MetroCTXRenderer

        private class MetroCTXRenderer : ToolStripProfessionalRenderer
        {
            public MetroCTXRenderer(MetroThemeStyle Theme, MetroColorStyle Style)
                : base(new contextcolors(Theme, Style))
            {
            }
        }

        #endregion

        #region Nested type: contextcolors

        private class contextcolors : ProfessionalColorTable
        {
            private readonly MetroColorStyle _style = MetroColorStyle.Blue;
            private readonly MetroThemeStyle _theme = MetroThemeStyle.Light;

            public contextcolors(MetroThemeStyle Theme, MetroColorStyle Style)
            {
                _theme = Theme;
                _style = Style;
            }

            public override Color MenuItemSelected
            {
                get { return MetroPaint.GetStyleColor(_style); }
            }

            public override Color MenuBorder
            {
                get { return MetroPaint.BackColor.Form(_theme); }
            }

            public override Color MenuItemBorder
            {
                get { return MetroPaint.GetStyleColor(_style); }
            }

            public override Color ImageMarginGradientBegin
            {
                get { return MetroPaint.BackColor.Form(_theme); }
            }

            public override Color ImageMarginGradientMiddle
            {
                get { return MetroPaint.BackColor.Form(_theme); }
            }

            public override Color ImageMarginGradientEnd
            {
                get { return MetroPaint.BackColor.Form(_theme); }
            }
        }

        #endregion
    }
}