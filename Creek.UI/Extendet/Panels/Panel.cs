/////////////////////////////////////////////////////////////////////////////////
// Copyright (C) 2005 - Hooyberghs Johnny
//
// Distribute and change freely, but please don't remove my name from the source 
//
// No warrantee of any kind, express or implied, is included with this
// software; use at your own risk, responsibility for damages (if any) to
// anyone resulting from the use of this software rests entirely with the
// user. 
//
// questions?
// Feel free to contact me: johnny.hooyberghs@gmail.com
//

// dependencies

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace Creek.UI.Extendet.Panels
{
    [ToolboxBitmap(typeof (Panel), "ExDotNet.ico")]
    public class Panel : System.Windows.Forms.Panel
    {
        private readonly Container components = null;

        #region Gradient

        private LinearGradientMode m_GradientMode = LinearGradientMode.Vertical;
        private Color m_color_EndColor = Color.FromKnownColor(KnownColor.InactiveCaption);
        private Color m_color_StartColor = Color.FromKnownColor(KnownColor.InactiveCaptionText);

        [Description("The starting color of the gradient background"), Category("_Gradient"), Browsable(true)]
        public Color GradientStart
        {
            get { return m_color_StartColor; }
            set
            {
                m_color_StartColor = value;
                Invalidate();
            }
        }

        [Description("The end color of the gradient background"), Category("_Gradient"), Browsable(true)]
        public Color GradientEnd
        {
            get { return m_color_EndColor; }
            set
            {
                m_color_EndColor = value;
                Invalidate();
            }
        }

        [Description("The gradient direction"), Category("_Gradient"), Browsable(true)]
        public LinearGradientMode GradientDirection
        {
            get { return m_GradientMode; }
            set
            {
                m_GradientMode = value;
                Invalidate();
            }
        }

        #endregion

        #region Border

        private BorderStyle m_BorderStyle = Extendet.BorderStyle.Single;
        private bool m_bool_Border = true;
        private Color m_color_BorderColor = Color.FromKnownColor(KnownColor.ActiveCaption);
        private int m_int_BorderWidth = 1;

        [Description("The style of the border"), Category("_Border"), Browsable(true)]
        public BorderStyle Style
        {
            get { return m_BorderStyle; }
            set
            {
                m_BorderStyle = value;
                Invalidate();
            }
        }

        [Description("The width in pixels of the border"), Category("_Border"), Browsable(true)]
        public int BorderWidth
        {
            get { return m_int_BorderWidth; }
            set
            {
                m_int_BorderWidth = value;
                Invalidate();
            }
        }

        [Description("Enable/Disable border"), Category("_Border"), Browsable(true)]
        public bool Border
        {
            get { return m_bool_Border; }
            set
            {
                m_bool_Border = value;
                Invalidate();
            }
        }

        [Description("The color of the border"), Category("_Border"), Browsable(true)]
        public Color BorderColor
        {
            get { return m_color_BorderColor; }
            set
            {
                m_color_BorderColor = value;
                Invalidate();
            }
        }

        #endregion

        #region Caption

        private LinearGradientMode m_CaptionGradientMode = LinearGradientMode.Vertical;
        private StringAlignment m_StringAlignment = StringAlignment.Near;
        private bool m_bool_Antialias = true;
        private bool m_bool_Caption = true;
        private Color m_color_CaptionBeginColor = Color.FromArgb(255, 225, 155);
        private Color m_color_CaptionEndColor = Color.FromArgb(255, 165, 78);
        private Color m_color_CaptionTextColor = Color.FromArgb(0, 0, 0);
        private int m_int_CaptionHeight = 24;
        private String m_str_Caption = "Panel";

        [Description("The gradient direction"), Category("_Caption"), Browsable(true)]
        public StringAlignment CaptionTextAlignment
        {
            get { return m_StringAlignment; }
            set
            {
                m_StringAlignment = value;
                Invalidate();
            }
        }

        [Description("The gradient direction"), Category("_Caption"), Browsable(true)]
        public LinearGradientMode CaptionGradientDirection
        {
            get { return m_CaptionGradientMode; }
            set
            {
                m_CaptionGradientMode = value;
                Invalidate();
            }
        }

        [Description("Enable/Disable antialiasing"), Category("_Caption"), Browsable(true)]
        public bool TextAntialias
        {
            get { return m_bool_Antialias; }
            set
            {
                m_bool_Antialias = value;
                Invalidate();
            }
        }

        [Description("The caption"), Category("_Caption"), Browsable(true)]
        public String CaptionText
        {
            get { return m_str_Caption; }
            set
            {
                m_str_Caption = value;
                Invalidate();
            }
        }

        [Description("Enable/Disable the caption"), Category("_Caption"), Browsable(true)]
        public bool Caption
        {
            get { return m_bool_Caption; }
            set
            {
                m_bool_Caption = value;
                Invalidate();
            }
        }

        [Description("Change the caption height"), Category("_Caption"), Browsable(true)]
        public int CaptionHeight
        {
            get { return m_int_CaptionHeight; }
            set
            {
                m_int_CaptionHeight = value;
                Invalidate();
            }
        }

        [Description("Change the caption begincolor"), Category("_Caption"), Browsable(true)]
        public Color CaptionBeginColor
        {
            get { return m_color_CaptionBeginColor; }
            set
            {
                m_color_CaptionBeginColor = value;
                Invalidate();
            }
        }

        [Description("Change the caption endcolor"), Category("_Caption"), Browsable(true)]
        public Color CaptionEndColor
        {
            get { return m_color_CaptionEndColor; }
            set
            {
                m_color_CaptionEndColor = value;
                Invalidate();
            }
        }


        [Description("Change the caption textcolor"), Category("_Caption"), Browsable(true)]
        public Color CaptionTextColor
        {
            get { return m_color_CaptionTextColor; }
            set
            {
                m_color_CaptionTextColor = value;
                Invalidate();
            }
        }

        [Description("Change the caption textcolor"), Category("_Caption"), Browsable(true)]
        public override Font Font
        {
            get { return base.Font; }
            set
            {
                base.Font = value;
                Invalidate();
            }
        }

        #endregion

        #region Icon

        private Icon m_Icon;
        private bool m_bool_Icon;

        [Description("The icon to display in the title"), Category("_Icon"), Browsable(true)]
        public Icon PanelIcon
        {
            get { return m_Icon; }
            set
            {
                m_Icon = value;
                Invalidate();
            }
        }

        [Description("Enable/Disable the icon"), Category("_Icon"), Browsable(true)]
        public bool IconVisible
        {
            get { return m_bool_Icon; }
            set
            {
                m_bool_Icon = value;
                Invalidate();
            }
        }

        #endregion

        #region Constructor

        public Panel()
        {
            InitializeComponent();

            Font = new Font("Arial", 14);

            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
        }

        #endregion

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
        }

        #endregion

        #region Painting

        protected override void OnPaint(PaintEventArgs e)
        {
            // The painting with shadow is slightly different...
            if (m_BorderStyle == Extendet.BorderStyle.Shadow)
            {
                // fill the background
                var brsh = new LinearGradientBrush(new Rectangle(0, 0, Width - 5, Height - 5), m_color_StartColor,
                                                   m_color_EndColor, m_GradientMode);
                e.Graphics.FillRectangle(brsh, 0, 0, Width - 5, Height - 5);

                // draw the border
                var pen = new Pen(m_color_BorderColor);
                for (int i = 0; i < m_int_BorderWidth; i++)
                {
                    e.Graphics.DrawRectangle(pen, i, i, Width - 6 - (i*2), Height - 6 - (i*2));
                }

                // draw the caption bar
                if (m_bool_Caption)
                {
                    if (m_bool_Antialias) e.Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
                    var brsh_Caption =
                        new LinearGradientBrush(
                            new Rectangle(m_int_BorderWidth, m_int_BorderWidth, Width - 5 - (m_int_BorderWidth*2),
                                          m_int_CaptionHeight), m_color_CaptionBeginColor, m_color_CaptionEndColor,
                            m_CaptionGradientMode);
                    e.Graphics.FillRectangle(brsh_Caption, m_int_BorderWidth, m_int_BorderWidth,
                                             Width - 5 - (m_int_BorderWidth*2), m_int_CaptionHeight);
                    var format = new StringFormat();
                    format.FormatFlags = StringFormatFlags.NoWrap;
                    format.LineAlignment = StringAlignment.Center;
                    format.Alignment = m_StringAlignment;
                    format.Trimming = StringTrimming.EllipsisCharacter;
                    e.Graphics.DrawString(
                        m_str_Caption,
                        Font,
                        new SolidBrush(m_color_CaptionTextColor),
                        new Rectangle(
                            // LEFT
                            (m_BorderStyle != Extendet.BorderStyle.None)
                                ? (m_bool_Icon
                                       ? m_int_BorderWidth + m_Icon.Width +
                                         ((m_int_CaptionHeight/2) - (m_Icon.Height/2))
                                       : m_int_BorderWidth)
                                : (m_bool_Icon
                                       ? m_Icon.Width + ((m_int_CaptionHeight/2) - (m_Icon.Height/2))
                                       : 0),
                            // TOP
                            (m_BorderStyle != Extendet.BorderStyle.None)
                                ? m_int_BorderWidth
                                : 0,
                            // WIDTH
                            (m_BorderStyle != Extendet.BorderStyle.None)
                                ? (m_bool_Icon
                                       ? Width - (m_int_BorderWidth*2) -
                                         ((m_int_CaptionHeight/2) - (m_Icon.Height/2)) - m_Icon.Width - 5
                                       : Width - (m_int_BorderWidth*2)) - 5
                                : (m_bool_Icon
                                       ? Width - ((m_int_CaptionHeight/2) - (m_Icon.Height/2)) - m_Icon.Width - 5
                                       : Width) - 5,
                            // HEIGHT
                            m_int_CaptionHeight)
                        , format);
                }

                // draw the shadow
                var pen1 = new Pen(Color.FromArgb(142, 142, 142), 1.0f);
                var pen2 = new Pen(Color.FromArgb(171, 171, 171), 1.0f);
                var pen3 = new Pen(Color.FromArgb(212, 212, 212), 1.0f);
                var pen4 = new Pen(Color.FromArgb(241, 241, 241), 1.0f);

                e.Graphics.DrawLine(pen1, Width - 5, 2, Width - 5, Height - 5);
                e.Graphics.DrawLine(pen2, Width - 4, 4, Width - 4, Height - 4);
                e.Graphics.DrawLine(pen3, Width - 3, 6, Width - 3, Height - 3);
                e.Graphics.DrawLine(pen4, Width - 2, 8, Width - 2, Height - 2);

                e.Graphics.DrawLine(pen1, 2, Height - 5, Width - 5, Height - 5);
                e.Graphics.DrawLine(pen2, 4, Height - 4, Width - 4, Height - 4);
                e.Graphics.DrawLine(pen3, 6, Height - 3, Width - 3, Height - 3);
                e.Graphics.DrawLine(pen4, 8, Height - 2, Width - 2, Height - 2);
            }
            else
            {
                // fill the background
                var brsh = new LinearGradientBrush(new Rectangle(0, 0, Width, Height), m_color_StartColor,
                                                   m_color_EndColor, m_GradientMode);
                e.Graphics.FillRectangle(brsh, 0, 0, Width, Height);

                // draw the border
                switch (m_BorderStyle)
                {
                    case Extendet.BorderStyle.Single:
                        {
                            var pen = new Pen(m_color_BorderColor);
                            for (int i = 0; i < m_int_BorderWidth; i++)
                            {
                                e.Graphics.DrawRectangle(pen, i, i, Width - 1 - (i*2), Height - 1 - (i*2));
                            }
                            break;
                        }
                    case Extendet.BorderStyle.Raised3D:
                        {
                            break;
                        }
                }

                // draw caption bar
                if (m_bool_Caption)
                {
                    if (m_bool_Antialias) e.Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
                    var brsh_Caption =
                        new LinearGradientBrush(
                            new Rectangle((m_BorderStyle != Extendet.BorderStyle.None) ? m_int_BorderWidth : 0,
                                          (m_BorderStyle != Extendet.BorderStyle.None) ? m_int_BorderWidth : 0,
                                          (m_BorderStyle != Extendet.BorderStyle.None)
                                              ? Width - (m_int_BorderWidth*2)
                                              : Width, m_int_CaptionHeight), m_color_CaptionBeginColor,
                            m_color_CaptionEndColor, m_CaptionGradientMode);
                    e.Graphics.FillRectangle(brsh_Caption,
                                             (m_BorderStyle != Extendet.BorderStyle.None) ? m_int_BorderWidth : 0,
                                             (m_BorderStyle != Extendet.BorderStyle.None) ? m_int_BorderWidth : 0,
                                             (m_BorderStyle != Extendet.BorderStyle.None)
                                                 ? Width - (m_int_BorderWidth*2)
                                                 : Width, m_int_CaptionHeight);
                    var format = new StringFormat();
                    format.FormatFlags = StringFormatFlags.NoWrap;
                    format.LineAlignment = StringAlignment.Center;
                    format.Alignment = m_StringAlignment;
                    format.Trimming = StringTrimming.EllipsisCharacter;
                    e.Graphics.DrawString(
                        m_str_Caption,
                        Font,
                        new SolidBrush(m_color_CaptionTextColor),
                        new Rectangle(
                            // LEFT
                            (m_BorderStyle != Extendet.BorderStyle.None)
                                ? (m_bool_Icon
                                       ? m_int_BorderWidth + m_Icon.Width +
                                         ((m_int_CaptionHeight/2) - (m_Icon.Height/2))
                                       : m_int_BorderWidth)
                                : (m_bool_Icon
                                       ? m_Icon.Width + ((m_int_CaptionHeight/2) - (m_Icon.Height/2))
                                       : 0),
                            // TOP
                            (m_BorderStyle != Extendet.BorderStyle.None)
                                ? m_int_BorderWidth
                                : 0,
                            // WIDTH
                            (m_BorderStyle != Extendet.BorderStyle.None)
                                ? (m_bool_Icon
                                       ? Width - (m_int_BorderWidth*2) -
                                         ((m_int_CaptionHeight/2) - (m_Icon.Height/2)) - m_Icon.Width
                                       : Width - (m_int_BorderWidth*2))
                                : (m_bool_Icon
                                       ? Width - ((m_int_CaptionHeight/2) - (m_Icon.Height/2)) - m_Icon.Width
                                       : Width),
                            // HEIGHT
                            m_int_CaptionHeight)
                        , format);
                }
            }

            // draw the icon
            if (m_bool_Icon && m_bool_Caption)
            {
                e.Graphics.DrawIcon(m_Icon,
                                    ((m_BorderStyle != Extendet.BorderStyle.None) ? m_int_BorderWidth : 0) +
                                    ((m_int_CaptionHeight/2) - (m_Icon.Height/2)),
                                    ((m_BorderStyle != Extendet.BorderStyle.None) ? m_int_BorderWidth : 0) +
                                    ((m_int_CaptionHeight/2) - (m_Icon.Height/2)));
            }

            base.OnPaint(e);
        }

        #endregion

        #region Overrides

        protected override void OnResize(EventArgs e)
        {
            Invalidate();
            base.OnResize(e);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}