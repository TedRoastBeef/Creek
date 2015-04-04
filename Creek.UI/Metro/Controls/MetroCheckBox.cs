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
    [Designer(typeof (MetroCheckBoxDesigner))]
    [ToolboxBitmap(typeof (CheckBox))]
    public class MetroCheckBox : CheckBox, IMetroControl
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

        private MetroCheckBoxSize metroCheckBoxSize = MetroCheckBoxSize.Small;

        private MetroCheckBoxWeight metroCheckBoxWeight = MetroCheckBoxWeight.Regular;

        [DefaultValue(false)]
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public bool DisplayFocus
        {
            get { return displayFocusRectangle; }
            set { displayFocusRectangle = value; }
        }

        [DefaultValue(MetroCheckBoxSize.Small)]
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public MetroCheckBoxSize FontSize
        {
            get { return metroCheckBoxSize; }
            set { metroCheckBoxSize = value; }
        }

        [DefaultValue(MetroCheckBoxWeight.Regular)]
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public MetroCheckBoxWeight FontWeight
        {
            get { return metroCheckBoxWeight; }
            set { metroCheckBoxWeight = value; }
        }

        [Browsable(false)]
        public override Font Font
        {
            get { return base.Font; }
            set { base.Font = value; }
        }

        #endregion

        #region Constructor

        public MetroCheckBox()
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

                if (!useCustomBackColor)
                {
                    backColor = MetroPaint.BackColor.Form(Theme);
                    if (Parent is MetroTile)
                    {
                        backColor = MetroPaint.GetStyleColor(Style);
                    }
                }

                if (backColor.A == 255)
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

            if (useCustomForeColor)
            {
                foreColor = ForeColor;

                if (isHovered && !isPressed && Enabled)
                {
                    borderColor = MetroPaint.BorderColor.CheckBox.Hover(Theme);
                }
                else if (isHovered && isPressed && Enabled)
                {
                    borderColor = MetroPaint.BorderColor.CheckBox.Press(Theme);
                }
                else if (!Enabled)
                {
                    borderColor = MetroPaint.BorderColor.CheckBox.Disabled(Theme);
                }
                else
                {
                    borderColor = MetroPaint.BorderColor.CheckBox.Normal(Theme);
                }
            }
            else
            {
                if (isHovered && !isPressed && Enabled)
                {
                    foreColor = MetroPaint.ForeColor.CheckBox.Hover(Theme);
                    borderColor = MetroPaint.BorderColor.CheckBox.Hover(Theme);
                }
                else if (isHovered && isPressed && Enabled)
                {
                    foreColor = MetroPaint.ForeColor.CheckBox.Press(Theme);
                    borderColor = MetroPaint.BorderColor.CheckBox.Press(Theme);
                }
                else if (!Enabled)
                {
                    foreColor = MetroPaint.ForeColor.CheckBox.Disabled(Theme);
                    borderColor = MetroPaint.BorderColor.CheckBox.Disabled(Theme);
                }
                else
                {
                    foreColor = !useStyleColors
                                    ? MetroPaint.ForeColor.CheckBox.Normal(Theme)
                                    : MetroPaint.GetStyleColor(Style);
                    borderColor = MetroPaint.BorderColor.CheckBox.Normal(Theme);
                }
            }

            using (var p = new Pen(borderColor))
            {
                var boxRect = new Rectangle(0, Height/2 - 6, 12, 12);
                e.Graphics.DrawRectangle(p, boxRect);
            }

            if (Checked)
            {
                Color fillColor = CheckState == CheckState.Indeterminate ? borderColor : MetroPaint.GetStyleColor(Style);

                using (var b = new SolidBrush(fillColor))
                {
                    var boxRect = new Rectangle(2, Height/2 - 4, 9, 9);
                    e.Graphics.FillRectangle(b, boxRect);
                }
            }

            var textRect = new Rectangle(16, 0, Width - 16, Height);
            TextRenderer.DrawText(e.Graphics, Text, MetroFonts.CheckBox(metroCheckBoxSize, metroCheckBoxWeight),
                                  textRect, foreColor, MetroPaint.GetTextFormatFlags(TextAlign));

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

        protected override void OnCheckedChanged(EventArgs e)
        {
            base.OnCheckedChanged(e);
            Invalidate();
        }

        public override Size GetPreferredSize(Size proposedSize)
        {
            Size preferredSize;
            base.GetPreferredSize(proposedSize);

            using (Graphics g = CreateGraphics())
            {
                proposedSize = new Size(int.MaxValue, int.MaxValue);
                preferredSize = TextRenderer.MeasureText(g, Text,
                                                         MetroFonts.CheckBox(metroCheckBoxSize, metroCheckBoxWeight),
                                                         proposedSize, MetroPaint.GetTextFormatFlags(TextAlign));
                preferredSize.Width += 16;
            }

            return preferredSize;
        }

        #endregion
    }
}