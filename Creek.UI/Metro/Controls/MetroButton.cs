/**
 * MetroFramework - Modern UI for WinForms
 * 
 * The MIT License (MIT)
 * Copyright (c) 2011 Sven Walter, http://github.com/viperneo
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy of 
 * this software and associated documentation files (the "Software"), to deal in the 
 * Software without restriction, including without limitation the rights to use, copy, 
 * modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, 
 * and to permit persons to whom the Software is furnished to do so, subject to the 
 * following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in 
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
 * INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A 
 * PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
 * HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
 * CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE 
 * OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Creek.UI.Metro.Components;
using Creek.UI.Metro.Controls.Design;
using Creek.UI.Metro.Drawing;
using Creek.UI.Metro.Interfaces;
using MetroFramework;

namespace Creek.UI.Metro.Controls
{
    [Designer(typeof (MetroButtonDesigner))]
    [ToolboxBitmap(typeof (Button))]
    [DefaultEvent("Click")]
    public class MetroButton : Button, IMetroControl
    {
        #region Interface

        private MetroColorStyle metroStyle = MetroColorStyle.Default;
        private MetroThemeStyle metroTheme = MetroThemeStyle.Default;
        private bool useCustomBackColor;
        private bool useCustomForeColor;
        private bool useStyleColors;

        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public event EventHandler<MetroPaintEventArgs> CustomPaintBackground;

        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public event EventHandler<MetroPaintEventArgs> CustomPaint;

        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public event EventHandler<MetroPaintEventArgs> CustomPaintForeground;

        [Category(MetroDefaults.PropertyCategory.Appearance)]
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
                    return MetroDefaults.Style;
                }

                return metroStyle;
            }
            set { metroStyle = value; }
        }

        [Category(MetroDefaults.PropertyCategory.Appearance)]
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
                    return MetroDefaults.Theme;
                }

                return metroTheme;
            }
            set { metroTheme = value; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MetroStyleManager StyleManager { get; set; }

        [DefaultValue(false)]
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public bool UseCustomBackColor
        {
            get { return useCustomBackColor; }
            set { useCustomBackColor = value; }
        }

        [DefaultValue(false)]
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public bool UseCustomForeColor
        {
            get { return useCustomForeColor; }
            set { useCustomForeColor = value; }
        }

        [DefaultValue(false)]
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public bool UseStyleColors
        {
            get { return useStyleColors; }
            set { useStyleColors = value; }
        }

        [Browsable(false)]
        [Category(MetroDefaults.PropertyCategory.Behaviour)]
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

        #region Fields

        private bool displayFocusRectangle;
        private bool isFocused;
        private bool isHovered;
        private bool isPressed;
        private MetroButtonSize metroButtonSize = MetroButtonSize.Small;
        private MetroButtonWeight metroButtonWeight = MetroButtonWeight.Bold;

        [DefaultValue(false)]
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public bool DisplayFocus
        {
            get { return displayFocusRectangle; }
            set { displayFocusRectangle = value; }
        }

        [DefaultValue(false)]
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public bool Highlight { get; set; }

        [DefaultValue(MetroButtonSize.Small)]
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public MetroButtonSize FontSize
        {
            get { return metroButtonSize; }
            set { metroButtonSize = value; }
        }

        [DefaultValue(MetroButtonWeight.Bold)]
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public MetroButtonWeight FontWeight
        {
            get { return metroButtonWeight; }
            set { metroButtonWeight = value; }
        }

        #endregion

        #region Constructor

        public MetroButton()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.UserPaint, true);
        }

        #endregion

        #region Paint Methods

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            try
            {
                Color backColor = BackColor;

                if (isHovered && !isPressed && Enabled)
                {
                    backColor = MetroPaint.BackColor.Button.Hover(Theme);
                }
                else if (isHovered && isPressed && Enabled)
                {
                    backColor = MetroPaint.BackColor.Button.Press(Theme);
                }
                else if (!Enabled)
                {
                    backColor = MetroPaint.BackColor.Button.Disabled(Theme);
                }
                else
                {
                    if (!useCustomBackColor)
                    {
                        backColor = MetroPaint.BackColor.Button.Normal(Theme);
                    }
                }

                if (backColor.A == 255 && BackgroundImage == null)
                {
                    e.Graphics.Clear(backColor);
                    return;
                }

                base.OnPaintBackground(e);

                OnCustomPaintBackground(new MetroPaintEventArgs(backColor, Color.Empty, e.Graphics));
            }
            catch
            {
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            try
            {
                if (GetStyle(ControlStyles.AllPaintingInWmPaint))
                {
                    OnPaintBackground(e);
                }

                OnCustomPaint(new MetroPaintEventArgs(Color.Empty, Color.Empty, e.Graphics));
                OnPaintForeground(e);
            }
            catch
            {
                Invalidate();
            }
        }

        protected virtual void OnPaintForeground(PaintEventArgs e)
        {
            Color borderColor, foreColor;

            if (isHovered && !isPressed && Enabled)
            {
                borderColor = MetroPaint.BorderColor.Button.Hover(Theme);
                foreColor = MetroPaint.ForeColor.Button.Hover(Theme);
            }
            else if (isHovered && isPressed && Enabled)
            {
                borderColor = MetroPaint.BorderColor.Button.Press(Theme);
                foreColor = MetroPaint.ForeColor.Button.Press(Theme);
            }
            else if (!Enabled)
            {
                borderColor = MetroPaint.BorderColor.Button.Disabled(Theme);
                foreColor = MetroPaint.ForeColor.Button.Disabled(Theme);
            }
            else
            {
                borderColor = MetroPaint.BorderColor.Button.Normal(Theme);
                if (useCustomForeColor)
                {
                    foreColor = ForeColor;
                }
                else if (useStyleColors)
                {
                    foreColor = MetroPaint.GetStyleColor(Style);
                }
                else
                {
                    foreColor = MetroPaint.ForeColor.Button.Normal(Theme);
                }
            }

            using (var p = new Pen(borderColor))
            {
                var borderRect = new Rectangle(0, 0, Width - 1, Height - 1);
                e.Graphics.DrawRectangle(p, borderRect);
            }

            if (Highlight && !isHovered && !isPressed && Enabled)
            {
                using (Pen p = MetroPaint.GetStylePen(Style))
                {
                    var borderRect = new Rectangle(0, 0, Width - 1, Height - 1);
                    e.Graphics.DrawRectangle(p, borderRect);
                    borderRect = new Rectangle(1, 1, Width - 3, Height - 3);
                    e.Graphics.DrawRectangle(p, borderRect);
                }
            }

            TextRenderer.DrawText(e.Graphics, Text, MetroFonts.Button(metroButtonSize, metroButtonWeight),
                                  ClientRectangle, foreColor, MetroPaint.GetTextFormatFlags(TextAlign));

            OnCustomPaintForeground(new MetroPaintEventArgs(Color.Empty, foreColor, e.Graphics));

            if (displayFocusRectangle && isFocused)
                ControlPaint.DrawFocusRectangle(e.Graphics, ClientRectangle);
        }

        #endregion

        #region Focus Methods

        protected override void OnGotFocus(EventArgs e)
        {
            isFocused = true;
            isHovered = true;
            Invalidate();

            base.OnGotFocus(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            isFocused = false;
            isHovered = false;
            isPressed = false;
            Invalidate();

            base.OnLostFocus(e);
        }

        protected override void OnEnter(EventArgs e)
        {
            isFocused = true;
            isHovered = true;
            Invalidate();

            base.OnEnter(e);
        }

        protected override void OnLeave(EventArgs e)
        {
            isFocused = false;
            isHovered = false;
            isPressed = false;
            Invalidate();

            base.OnLeave(e);
        }

        #endregion

        #region Keyboard Methods

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                isHovered = true;
                isPressed = true;
                Invalidate();
            }

            base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            //Remove this code cause this prevents the focus color
            //isHovered = false;
            //isPressed = false;
            Invalidate();

            base.OnKeyUp(e);
        }

        #endregion

        #region Mouse Methods

        protected override void OnMouseEnter(EventArgs e)
        {
            isHovered = true;
            Invalidate();

            base.OnMouseEnter(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isPressed = true;
                Invalidate();
            }

            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            isPressed = false;
            Invalidate();

            base.OnMouseUp(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            //This will check if control got the focus
            //If not thats the only it will remove the focus color
            if (!isFocused)
            {
                isHovered = false;
            }

            Invalidate();

            base.OnMouseLeave(e);
        }

        #endregion

        #region Overridden Methods

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            Invalidate();
        }

        #endregion
    }
}