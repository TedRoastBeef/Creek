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
using System.Security;
using System.Windows.Forms;
using Creek.UI.Metro.Components;
using Creek.UI.Metro.Controls.Design;
using Creek.UI.Metro.Drawing;
using Creek.UI.Metro.Interfaces;
using Creek.UI.Metro.Native;
using MetroFramework;

namespace Creek.UI.Metro.Controls
{

    #region Enums

    public enum MetroLabelMode
    {
        Default,
        Selectable
    }

    #endregion

    [Designer(typeof (MetroLabelDesigner))]
    [ToolboxBitmap(typeof (Label))]
    public class MetroLabel : Label, IMetroControl
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

        private readonly DoubleBufferedTextBox baseTextBox;
        private MetroLabelMode labelMode = MetroLabelMode.Default;

        private MetroLabelSize metroLabelSize = MetroLabelSize.Medium;

        private MetroLabelWeight metroLabelWeight = MetroLabelWeight.Light;
        private bool wrapToLine;

        [DefaultValue(MetroLabelSize.Medium)]
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public MetroLabelSize FontSize
        {
            get { return metroLabelSize; }
            set
            {
                metroLabelSize = value;
                Refresh();
            }
        }

        [DefaultValue(MetroLabelWeight.Light)]
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public MetroLabelWeight FontWeight
        {
            get { return metroLabelWeight; }
            set
            {
                metroLabelWeight = value;
                Refresh();
            }
        }

        [DefaultValue(MetroLabelMode.Default)]
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public MetroLabelMode LabelMode
        {
            get { return labelMode; }
            set { labelMode = value; }
        }

        [DefaultValue(false)]
        [Category(MetroDefaults.PropertyCategory.Behaviour)]
        public bool WrapToLine
        {
            get { return wrapToLine; }
            set
            {
                wrapToLine = value;
                Refresh();
            }
        }

        #endregion

        #region Constructor

        public MetroLabel()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.UserPaint, true);

            baseTextBox = new DoubleBufferedTextBox();
            baseTextBox.Visible = false;
            Controls.Add(baseTextBox);
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
            Color foreColor;

            if (useCustomForeColor)
            {
                foreColor = ForeColor;
            }
            else
            {
                if (!Enabled)
                {
                    if (Parent != null)
                    {
                        if (Parent is MetroTile)
                        {
                            foreColor = MetroPaint.ForeColor.Tile.Disabled(Theme);
                        }
                        else
                        {
                            foreColor = MetroPaint.ForeColor.Label.Normal(Theme);
                        }
                    }
                    else
                    {
                        foreColor = MetroPaint.ForeColor.Label.Disabled(Theme);
                    }
                }
                else
                {
                    if (Parent != null)
                    {
                        if (Parent is MetroTile)
                        {
                            foreColor = MetroPaint.ForeColor.Tile.Normal(Theme);
                        }
                        else
                        {
                            if (useStyleColors)
                            {
                                foreColor = MetroPaint.GetStyleColor(Style);
                            }
                            else
                            {
                                foreColor = MetroPaint.ForeColor.Label.Normal(Theme);
                            }
                        }
                    }
                    else
                    {
                        if (useStyleColors)
                        {
                            foreColor = MetroPaint.GetStyleColor(Style);
                        }
                        else
                        {
                            foreColor = MetroPaint.ForeColor.Label.Normal(Theme);
                        }
                    }
                }
            }

            if (LabelMode == MetroLabelMode.Selectable)
            {
                CreateBaseTextBox();
                UpdateBaseTextBox();

                if (!baseTextBox.Visible)
                {
                    TextRenderer.DrawText(e.Graphics, Text, MetroFonts.Label(metroLabelSize, metroLabelWeight),
                                          ClientRectangle, foreColor, MetroPaint.GetTextFormatFlags(TextAlign));
                }
            }
            else
            {
                DestroyBaseTextbox();
                TextRenderer.DrawText(e.Graphics, Text, MetroFonts.Label(metroLabelSize, metroLabelWeight),
                                      ClientRectangle, foreColor, MetroPaint.GetTextFormatFlags(TextAlign, wrapToLine));
                OnCustomPaintForeground(new MetroPaintEventArgs(Color.Empty, foreColor, e.Graphics));
            }
        }

        #endregion

        #region Overridden Methods

        public override void Refresh()
        {
            if (LabelMode == MetroLabelMode.Selectable)
            {
                UpdateBaseTextBox();
            }

            base.Refresh();
        }

        public override Size GetPreferredSize(Size proposedSize)
        {
            Size preferredSize;
            base.GetPreferredSize(proposedSize);

            using (Graphics g = CreateGraphics())
            {
                proposedSize = new Size(int.MaxValue, int.MaxValue);
                preferredSize = TextRenderer.MeasureText(g, Text, MetroFonts.Label(metroLabelSize, metroLabelWeight),
                                                         proposedSize, MetroPaint.GetTextFormatFlags(TextAlign));
            }

            return preferredSize;
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            Invalidate();
        }

        protected override void OnResize(EventArgs e)
        {
            if (LabelMode == MetroLabelMode.Selectable)
            {
                HideBaseTextBox();
            }

            base.OnResize(e);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            if (LabelMode == MetroLabelMode.Selectable)
            {
                ShowBaseTextBox();
            }
        }

        #endregion

        #region Label Selection Mode

        private bool firstInitialization = true;

        private void CreateBaseTextBox()
        {
            if (baseTextBox.Visible && !firstInitialization) return;
            if (!firstInitialization) return;

            firstInitialization = false;

            if (!DesignMode)
            {
                Form parentForm = FindForm();
                if (parentForm != null)
                {
                    parentForm.ResizeBegin += parentForm_ResizeBegin;
                    parentForm.ResizeEnd += parentForm_ResizeEnd;
                }
            }

            baseTextBox.BackColor = Color.Transparent;
            baseTextBox.Visible = true;
            baseTextBox.BorderStyle = BorderStyle.None;
            baseTextBox.Font = MetroFonts.Label(metroLabelSize, metroLabelWeight);
            baseTextBox.Location = new Point(1, 0);
            baseTextBox.Text = Text;
            baseTextBox.ReadOnly = true;

            baseTextBox.Size = GetPreferredSize(Size.Empty);
            baseTextBox.Multiline = true;

            baseTextBox.DoubleClick += BaseTextBoxOnDoubleClick;
            baseTextBox.Click += BaseTextBoxOnClick;

            Controls.Add(baseTextBox);
        }

        private void parentForm_ResizeEnd(object sender, EventArgs e)
        {
            if (LabelMode == MetroLabelMode.Selectable)
            {
                ShowBaseTextBox();
            }
        }

        private void parentForm_ResizeBegin(object sender, EventArgs e)
        {
            if (LabelMode == MetroLabelMode.Selectable)
            {
                HideBaseTextBox();
            }
        }

        private void DestroyBaseTextbox()
        {
            if (!baseTextBox.Visible) return;

            baseTextBox.DoubleClick -= BaseTextBoxOnDoubleClick;
            baseTextBox.Click -= BaseTextBoxOnClick;
            baseTextBox.Visible = false;
        }

        private void UpdateBaseTextBox()
        {
            if (!baseTextBox.Visible) return;

            SuspendLayout();
            baseTextBox.SuspendLayout();

            if (useCustomBackColor)
                baseTextBox.BackColor = BackColor;
            else
                baseTextBox.BackColor = MetroPaint.BackColor.Form(Theme);

            if (!Enabled)
            {
                if (Parent != null)
                {
                    if (Parent is MetroTile)
                    {
                        baseTextBox.ForeColor = MetroPaint.ForeColor.Tile.Disabled(Theme);
                    }
                    else
                    {
                        if (useStyleColors)
                        {
                            baseTextBox.ForeColor = MetroPaint.GetStyleColor(Style);
                        }
                        else
                        {
                            baseTextBox.ForeColor = MetroPaint.ForeColor.Label.Disabled(Theme);
                        }
                    }
                }
                else
                {
                    if (useStyleColors)
                    {
                        baseTextBox.ForeColor = MetroPaint.GetStyleColor(Style);
                    }
                    else
                    {
                        baseTextBox.ForeColor = MetroPaint.ForeColor.Label.Disabled(Theme);
                    }
                }
            }
            else
            {
                if (Parent != null)
                {
                    if (Parent is MetroTile)
                    {
                        baseTextBox.ForeColor = MetroPaint.ForeColor.Tile.Normal(Theme);
                    }
                    else
                    {
                        if (useStyleColors)
                        {
                            baseTextBox.ForeColor = MetroPaint.GetStyleColor(Style);
                        }
                        else
                        {
                            baseTextBox.ForeColor = MetroPaint.ForeColor.Label.Normal(Theme);
                        }
                    }
                }
                else
                {
                    if (useStyleColors)
                    {
                        baseTextBox.ForeColor = MetroPaint.GetStyleColor(Style);
                    }
                    else
                    {
                        baseTextBox.ForeColor = MetroPaint.ForeColor.Label.Normal(Theme);
                    }
                }
            }

            baseTextBox.Font = MetroFonts.Label(metroLabelSize, metroLabelWeight);
            baseTextBox.Text = Text;
            baseTextBox.BorderStyle = BorderStyle.None;

            Size = GetPreferredSize(Size.Empty);

            baseTextBox.ResumeLayout();
            ResumeLayout();
        }

        private void HideBaseTextBox()
        {
            baseTextBox.Visible = false;
        }

        private void ShowBaseTextBox()
        {
            baseTextBox.Visible = true;
        }

        [SecuritySafeCritical]
        private void BaseTextBoxOnClick(object sender, EventArgs eventArgs)
        {
            WinCaret.HideCaret(baseTextBox.Handle);
        }

        [SecuritySafeCritical]
        private void BaseTextBoxOnDoubleClick(object sender, EventArgs eventArgs)
        {
            baseTextBox.SelectAll();
            WinCaret.HideCaret(baseTextBox.Handle);
        }

        private class DoubleBufferedTextBox : TextBox
        {
            public DoubleBufferedTextBox()
            {
                SetStyle(ControlStyles.SupportsTransparentBackColor | ControlStyles.OptimizedDoubleBuffer, true);
            }
        }

        #endregion
    }
}