﻿/**
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
using Creek.UI.Metro.Drawing;
using Creek.UI.Metro.Interfaces;
using MetroFramework;

namespace Creek.UI.Metro.Controls
{
    [ToolboxBitmap(typeof (ComboBox))]
    public class MetroComboBox : ComboBox, IMetroControl
    {
        #region Interface

        private MetroColorStyle metroStyle = MetroColorStyle.Default;
        private MetroThemeStyle metroTheme = MetroThemeStyle.Default;
        private bool useCustomBackColor;

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
        public bool UseCustomForeColor { get; set; }

        [DefaultValue(false)]
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public bool UseStyleColors { get; set; }

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
        private bool drawPrompt;
        private bool isFocused;
        private bool isHovered;
        private bool isPressed;
        private MetroComboBoxSize metroComboBoxSize = MetroComboBoxSize.Medium;
        private MetroComboBoxWeight metroComboBoxWeight = MetroComboBoxWeight.Regular;
        private string promptText = "";

        [DefaultValue(false)]
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public bool DisplayFocus
        {
            get { return displayFocusRectangle; }
            set { displayFocusRectangle = value; }
        }

        [DefaultValue(DrawMode.OwnerDrawFixed)]
        [Browsable(false)]
        public new DrawMode DrawMode
        {
            get { return DrawMode.OwnerDrawFixed; }
            set { base.DrawMode = DrawMode.OwnerDrawFixed; }
        }

        [DefaultValue(ComboBoxStyle.DropDownList)]
        [Browsable(false)]
        public new ComboBoxStyle DropDownStyle
        {
            get { return ComboBoxStyle.DropDownList; }
            set { base.DropDownStyle = ComboBoxStyle.DropDownList; }
        }

        [DefaultValue(MetroComboBoxSize.Medium)]
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public MetroComboBoxSize FontSize
        {
            get { return metroComboBoxSize; }
            set { metroComboBoxSize = value; }
        }

        [DefaultValue(MetroComboBoxWeight.Regular)]
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public MetroComboBoxWeight FontWeight
        {
            get { return metroComboBoxWeight; }
            set { metroComboBoxWeight = value; }
        }

        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DefaultValue("")]
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public string PromptText
        {
            get { return promptText; }
            set
            {
                promptText = value.Trim();
                Invalidate();
            }
        }

        [Browsable(false)]
        public override Font Font
        {
            get { return base.Font; }
            set { base.Font = value; }
        }

        #endregion

        #region Constructor

        public MetroComboBox()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.UserPaint, true);

            base.DrawMode = DrawMode.OwnerDrawFixed;
            base.DropDownStyle = ComboBoxStyle.DropDownList;

            drawPrompt = (SelectedIndex == -1);
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
            base.ItemHeight = GetPreferredSize(Size.Empty).Height;

            Color borderColor, foreColor;

            if (isHovered && !isPressed && Enabled)
            {
                foreColor = MetroPaint.ForeColor.ComboBox.Hover(Theme);
                borderColor = MetroPaint.BorderColor.ComboBox.Hover(Theme);
            }
            else if (isHovered && isPressed && Enabled)
            {
                foreColor = MetroPaint.ForeColor.ComboBox.Press(Theme);
                borderColor = MetroPaint.BorderColor.ComboBox.Press(Theme);
            }
            else if (!Enabled)
            {
                foreColor = MetroPaint.ForeColor.ComboBox.Disabled(Theme);
                borderColor = MetroPaint.BorderColor.ComboBox.Disabled(Theme);
            }
            else
            {
                foreColor = MetroPaint.ForeColor.ComboBox.Normal(Theme);
                borderColor = MetroPaint.BorderColor.ComboBox.Normal(Theme);
            }

            using (var p = new Pen(borderColor))
            {
                var boxRect = new Rectangle(0, 0, Width - 1, Height - 1);
                e.Graphics.DrawRectangle(p, boxRect);
            }

            using (var b = new SolidBrush(foreColor))
            {
                e.Graphics.FillPolygon(b,
                                       new[]
                                           {
                                               new Point(Width - 20, (Height/2) - 2), new Point(Width - 9, (Height/2) - 2),
                                               new Point(Width - 15, (Height/2) + 4)
                                           });
            }

            var textRect = new Rectangle(2, 2, Width - 20, Height - 4);

            TextRenderer.DrawText(e.Graphics, Text, MetroFonts.ComboBox(metroComboBoxSize, metroComboBoxWeight),
                                  textRect, foreColor, TextFormatFlags.Left | TextFormatFlags.VerticalCenter);

            OnCustomPaintForeground(new MetroPaintEventArgs(Color.Empty, foreColor, e.Graphics));

            if (displayFocusRectangle && isFocused)
                ControlPaint.DrawFocusRectangle(e.Graphics, ClientRectangle);

            if (drawPrompt)
            {
                DrawTextPrompt(e.Graphics);
            }
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (e.Index >= 0)
            {
                Color foreColor;
                Color backColor = BackColor;

                if (!useCustomBackColor)
                {
                    backColor = MetroPaint.BackColor.Form(Theme);
                }

                if (e.State == (DrawItemState.NoAccelerator | DrawItemState.NoFocusRect) ||
                    e.State == DrawItemState.None)
                {
                    using (var b = new SolidBrush(backColor))
                    {
                        e.Graphics.FillRectangle(b,
                                                 new Rectangle(e.Bounds.Left, e.Bounds.Top, e.Bounds.Width,
                                                               e.Bounds.Height));
                    }

                    foreColor = MetroPaint.ForeColor.Link.Normal(Theme);
                }
                else
                {
                    using (var b = new SolidBrush(MetroPaint.GetStyleColor(Style)))
                    {
                        e.Graphics.FillRectangle(b,
                                                 new Rectangle(e.Bounds.Left, e.Bounds.Top, e.Bounds.Width,
                                                               e.Bounds.Height));
                    }

                    foreColor = MetroPaint.ForeColor.Tile.Normal(Theme);
                }

                var textRect = new Rectangle(0, e.Bounds.Top, e.Bounds.Width, e.Bounds.Height);
                TextRenderer.DrawText(e.Graphics, GetItemText(Items[e.Index]),
                                      MetroFonts.ComboBox(metroComboBoxSize, metroComboBoxWeight), textRect, foreColor,
                                      TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
            }
            else
            {
                base.OnDrawItem(e);
            }
        }

        private void DrawTextPrompt()
        {
            using (Graphics graphics = CreateGraphics())
            {
                DrawTextPrompt(graphics);
            }
        }

        private void DrawTextPrompt(Graphics g)
        {
            Color backColor = BackColor;

            if (!useCustomBackColor)
            {
                backColor = MetroPaint.BackColor.Form(Theme);
            }

            var textRect = new Rectangle(2, 2, Width - 20, Height - 4);
            TextRenderer.DrawText(g, promptText, MetroFonts.ComboBox(metroComboBoxSize, metroComboBoxWeight), textRect,
                                  SystemColors.GrayText, backColor,
                                  TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
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

        private const int OCM_COMMAND = 0x2111;
        private const int WM_PAINT = 15;

        public override Size GetPreferredSize(Size proposedSize)
        {
            Size preferredSize;
            base.GetPreferredSize(proposedSize);

            using (Graphics g = CreateGraphics())
            {
                string measureText = Text.Length > 0 ? Text : "MeasureText";
                proposedSize = new Size(int.MaxValue, int.MaxValue);
                preferredSize = TextRenderer.MeasureText(g, measureText,
                                                         MetroFonts.ComboBox(metroComboBoxSize, metroComboBoxWeight),
                                                         proposedSize,
                                                         TextFormatFlags.Left | TextFormatFlags.LeftAndRightPadding |
                                                         TextFormatFlags.VerticalCenter);
                preferredSize.Height += 4;
            }

            return preferredSize;
        }

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            base.OnSelectedIndexChanged(e);
            drawPrompt = (SelectedIndex == -1);
            Invalidate();
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (((m.Msg == WM_PAINT) || (m.Msg == OCM_COMMAND)) && (drawPrompt))
            {
                DrawTextPrompt();
            }
        }

        #endregion
    }
}