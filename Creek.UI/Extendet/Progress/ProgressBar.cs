using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace Creek.UI.Extendet.Progress
{

    #region Enum

    //#####
    public enum ProgressCaptionMode
    {
        None,
        Percent,
        Value,
        Custom
    }

    //#####
    public enum ProgressFloodStyle
    {
        Standard,
        Horizontal
    }

    //#####
    public enum ProgressBarEdge
    {
        None,
        Rectangle,
        Rounded
    }

    //#####
    public enum ProgressBarDirection
    {
        Horizontal,
        Vertical
    }

    //#####
    public enum ProgressStyle
    {
        Dashed,
        Solid
    }

    //#####

    #endregion

    [ToolboxBitmap(typeof (ProgressBar), "ExDotNet.ico")]
    public class ProgressBar : UserControl
    {
        private readonly Container components = null;

        #region Direction

        //#####
        private ProgressBarDirection m_Direction = ProgressBarDirection.Horizontal;
        private bool m_bool_Invert;

        [Description("Invert the progress direction")]
        [Category("_Orientation")]
        [Browsable(true)]
        public bool Invert
        {
            get { return m_bool_Invert; }
            set
            {
                m_bool_Invert = value;
                Invalidate();
            }
        }

        //#####

        [Description("Set the progress control horizontal or vertical")]
        [Category("_Orientation")]
        [Browsable(true)]
        public ProgressBarDirection Orientation
        {
            get { return m_Direction; }
            set
            {
                m_Direction = value;
                Invalidate();
            }
        }

        #endregion

        #region Edge

        //#####
        private ProgressBarEdge m_Edge = ProgressBarEdge.Rounded;

        //#####
        private Color m_EdgeColor = Color.FromKnownColor(KnownColor.Gray);

        //#####
        private Color m_EdgeLightColor = Color.FromKnownColor(KnownColor.LightGray);

        //#####
        private int m_EdgeWidth = 1;

        [Description("Set the edge of the control")]
        [Category("_Edge")]
        [Browsable(true)]
        public ProgressBarEdge Edge
        {
            get { return m_Edge; }
            set
            {
                m_Edge = value;
                Invalidate();
            }
        }

        [Description("Set the edge color")]
        [Category("_Edge")]
        [Browsable(true)]
        public Color EdgeColor
        {
            get { return m_EdgeColor; }
            set
            {
                m_EdgeColor = value;
                Invalidate();
            }
        }

        [Description("Set the edge light color")]
        [Category("_Edge")]
        [Browsable(true)]
        public Color EdgeLightColor
        {
            get { return m_EdgeLightColor; }
            set
            {
                m_EdgeLightColor = value;
                Invalidate();
            }
        }

        [Description("Set the edge width")]
        [Category("_Edge")]
        [Browsable(true)]
        public int EdgeWidth
        {
            get { return m_EdgeWidth; }
            set
            {
                m_EdgeWidth = value;
                if (m_EdgeWidth < 0) m_EdgeWidth = 0;
                if (m_EdgeWidth > Int16.MaxValue) m_EdgeWidth = Int16.MaxValue;
                Invalidate();
            }
        }

        //#####

        #endregion

        #region Progress

        //#####
        private Color m_Color1 = Color.FromArgb(0, 255, 0);
        private Color m_Color2 = Color.FromKnownColor(KnownColor.White);
        private ProgressFloodStyle m_FloodStyle = ProgressFloodStyle.Standard;
        private ProgressStyle m_Style = ProgressStyle.Dashed;
        private Color m_color_Back = Color.FromKnownColor(KnownColor.White);

        //#####
        private float m_float_BarFlood = 0.20f;

        //#####
        private int m_int_BarOffset = 1;
        private int m_int_DashSpace = 2;

        //#####
        private int m_int_DashWidth = 5;

        [Description(
            "Set the floodstyle. Standard draws a standard xp-themed progressbar, and with Horizontal you can create a horizontal flood bar (for the best effect, set FloodPercentage to 1.0."
            )]
        [Category("_Progress")]
        [Browsable(true)]
        public ProgressFloodStyle FloodStyle
        {
            get { return m_FloodStyle; }
            set
            {
                m_FloodStyle = value;
                Invalidate();
            }
        }

        [Description("Set the percentage of the flood color, a value between 0.0 and 1.0.")]
        [Category("_Progress")]
        [Browsable(true)]
        public float FloodPercentage
        {
            get { return m_float_BarFlood; }
            set
            {
                m_float_BarFlood = value;
                if (m_float_BarFlood < 0.0f) m_float_BarFlood = 0.0f;
                if (m_float_BarFlood > 1.0f) m_float_BarFlood = 1.0f;
                Invalidate();
            }
        }

        [Description("Set the offset for the left, top, right and bottom")]
        [Category("_Progress")]
        [Browsable(true)]
        public int BarOffset
        {
            get { return m_int_BarOffset; }
            set
            {
                m_int_BarOffset = value;
                if (m_int_BarOffset < 0) m_int_BarOffset = 0;
                if (m_int_BarOffset > Int16.MaxValue) m_int_BarOffset = Int16.MaxValue;
                Invalidate();
            }
        }

        [Description("Set the width of a dash if Dashed mode")]
        [Category("_Progress")]
        [Browsable(true)]
        public int DashWidth
        {
            get { return m_int_DashWidth; }
            set
            {
                m_int_DashWidth = value;
                if (m_int_DashWidth < 0) m_int_DashWidth = 0;
                if (m_int_DashWidth > Int16.MaxValue) m_int_DashWidth = Int16.MaxValue;
                Invalidate();
            }
        }

        //#####

        [Description("Set the space between every dash if Dashed mode")]
        [Category("_Progress")]
        [Browsable(true)]
        public int DashSpace
        {
            get { return m_int_DashSpace; }
            set
            {
                m_int_DashSpace = value;
                if (m_int_DashSpace < 0) m_int_DashSpace = 0;
                if (m_int_DashSpace > Int16.MaxValue) m_int_DashSpace = Int16.MaxValue;
                Invalidate();
            }
        }

        //#####

        [Description("Set progressbar style")]
        [Category("_Progress")]
        [Browsable(true)]
        public ProgressStyle ProgressBarStyle
        {
            get { return m_Style; }
            set
            {
                m_Style = value;
                Invalidate();
            }
        }

        //#####

        [Description("Set the main color")]
        [Category("_Progress")]
        [Browsable(true)]
        public Color MainColor
        {
            get { return m_Color1; }
            set
            {
                m_Color1 = value;
                Invalidate();
            }
        }

        //#####

        [Description("Set the second color")]
        [Category("_Progress")]
        [Browsable(true)]
        public Color SecondColor
        {
            get { return m_Color2; }
            set
            {
                m_Color2 = value;
                Invalidate();
            }
        }

        //#####

        [Description("Set the background color")]
        [Category("_Progress")]
        [Browsable(true)]
        public Color ProgressBackColor
        {
            get { return m_color_Back; }
            set
            {
                m_color_Back = value;
                Invalidate();
            }
        }

        //#####

        #endregion

        #region Properties

        private int m_int_Maximum = 100;
        private int m_int_Minimum;
        private int m_int_Step = 1;
        private int m_int_Value = 33;

        [Description("Set the minimum value of this progress control")]
        [Category("Properties")]
        [Browsable(true)]
        public int Minimum
        {
            get { return m_int_Minimum; }
            set
            {
                if (value < m_int_Maximum) m_int_Minimum = value;
                Invalidate();
            }
        }

        [Description("Set the maximum value of this progress control")]
        [Category("Properties")]
        [Browsable(true)]
        public int Maximum
        {
            get { return m_int_Maximum; }
            set
            {
                if (value > m_int_Minimum) m_int_Maximum = value;
                Invalidate();
            }
        }

        [Description("Set the current value of this progress control")]
        [Category("Properties")]
        [Browsable(true)]
        public int Value
        {
            get { return m_int_Value; }
            set
            {
                m_int_Value = value;
                if (m_int_Value < m_int_Minimum) m_int_Value = m_int_Minimum;
                if (m_int_Value > m_int_Maximum) m_int_Value = m_int_Maximum;
                Invalidate();
            }
        }

        [Description("Set the step value")]
        [Category("Properties")]
        [Browsable(true)]
        public int Step
        {
            get { return m_int_Step; }
            set
            {
                m_int_Step = value;
                Invalidate();
            }
        }

        #endregion

        #region Constructor

        public ProgressBar()
        {
            InitializeComponent();

            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
        }

        #endregion

        #region Caption

        //#####
        private ProgressCaptionMode m_CaptionMode = ProgressCaptionMode.Percent;
        private bool m_bool_Antialias = true;
        private bool m_bool_Shadow = true;
        private Color m_color_Caption = Color.FromKnownColor(KnownColor.Black);
        private Color m_color_Shadow = Color.FromKnownColor(KnownColor.White);

        //#####
        private int m_int_ShadowOffset = 1;
        private String m_str_Caption = "Progress";

        [Description("Change the font")]
        [Category("_Caption")]
        [Browsable(true)]
        public override Font Font
        {
            get { return base.Font; }
            set
            {
                base.Font = value;
                Invalidate();
            }
        }

        [Description("Enable/Disable shadow")]
        [Category("_Caption")]
        [Browsable(true)]
        public bool Shadow
        {
            get { return m_bool_Shadow; }
            set
            {
                m_bool_Shadow = value;
                Invalidate();
            }
        }

        [Description("Set shadow offset")]
        [Category("_Caption")]
        [Browsable(true)]
        public int ShadowOffset
        {
            get { return m_int_ShadowOffset; }
            set
            {
                m_int_ShadowOffset = value;
                Invalidate();
            }
        }

        //#####

        [Description("Enable/Disable antialiasing")]
        [Category("_Caption")]
        [Browsable(true)]
        public bool TextAntialias
        {
            get { return m_bool_Antialias; }
            set
            {
                m_bool_Antialias = value;
                Invalidate();
            }
        }

        //#####

        [Description("Set the caption shadow color.")]
        [Category("_Caption")]
        [Browsable(true)]
        public Color CaptionShadowColor
        {
            get { return m_color_Shadow; }
            set
            {
                m_color_Shadow = value;
                Invalidate();
            }
        }

        //#####

        [Description("Set the caption color.")]
        [Category("_Caption")]
        [Browsable(true)]
        public Color CaptionColor
        {
            get { return m_color_Caption; }
            set
            {
                m_color_Caption = value;
                Invalidate();
            }
        }

        //#####

        [Description("Set the caption mode.")]
        [Category("_Caption")]
        [Browsable(true)]
        public ProgressCaptionMode CaptionMode
        {
            get { return m_CaptionMode; }
            set
            {
                m_CaptionMode = value;
                Invalidate();
            }
        }

        //#####

        [Description("Set the caption.")]
        [Category("_Caption")]
        [Browsable(true)]
        public String Caption
        {
            get { return m_str_Caption; }
            set
            {
                m_str_Caption = value;
                Invalidate();
            }
        }

        //#####

        #endregion

        #region Custom

        //#####
        private bool m_bool_ChangeByMouse;

        [Description("Allows the user to change the value by clicking the mouse")]
        [Category("_Custom")]
        [Browsable(true)]
        public bool ChangeByMouse
        {
            get { return m_bool_ChangeByMouse; }
            set
            {
                m_bool_ChangeByMouse = value;
                Invalidate();
            }
        }

        #endregion

        #region GetCustomCaption

        private String GetCustomCaption(String caption)
        {
            float float_Percentage = ((m_int_Value - m_int_Minimum)/(float) (m_int_Maximum - m_int_Minimum))*100.0f;

            String toReturn = caption.Replace("<VALUE>", m_int_Value.ToString());
            toReturn = caption.Replace("<PERCENTAGE>", float_Percentage.ToString());

            return toReturn;
        }

        #endregion

        #region User Methods

        public void PerformStep()
        {
            m_int_Value += m_int_Step;
            if (m_int_Value < m_int_Minimum) m_int_Value = m_int_Minimum;
            if (m_int_Value > m_int_Maximum) m_int_Value = m_int_Maximum;
        }

        public void Increment(int val)
        {
            m_int_Value += val;
            if (m_int_Value < m_int_Minimum) m_int_Value = m_int_Minimum;
            if (m_int_Value > m_int_Maximum) m_int_Value = m_int_Maximum;
        }

        #endregion

        #region Overrides

        protected override void OnPaint(PaintEventArgs e)
        {
            #region OnPaint - Draw Background

            var brsh = new SolidBrush(m_color_Back);
            e.Graphics.FillRectangle(brsh, 0, 0, Width, Height);

            #endregion

            #region OnPaint - Draw ProgressBar

            switch (m_Direction)
            {
                    #region Horizontal

                case ProgressBarDirection.Horizontal:
                    {
                        float float_ProgressHeight = (Height - m_EdgeWidth*2 - m_int_BarOffset*2);
                        float float_ProgressTotalWidth = Width - m_EdgeWidth*2 - m_int_BarOffset*2;
                        float float_ProgressDrawWidth = float_ProgressTotalWidth/(m_int_Maximum - m_int_Minimum)*
                                                        (m_int_Value - m_int_Minimum);

                        var int_NumberOfDashes = (int) (float_ProgressDrawWidth/(m_int_DashWidth + m_int_DashSpace));
                        var int_TotalDashes = (int) (float_ProgressTotalWidth/(m_int_DashWidth + m_int_DashSpace));

                        var rect_Bar2 = new Rectangle(m_EdgeWidth + m_int_BarOffset, m_EdgeWidth + m_int_BarOffset,
                                                      (int) float_ProgressTotalWidth, (int) float_ProgressHeight);
                        Rectangle rect_Bar;
                        if (m_bool_Invert)
                        {
                            rect_Bar = new Rectangle(
                                m_EdgeWidth + m_int_BarOffset +
                                (int) (float_ProgressTotalWidth - float_ProgressDrawWidth)
                                , m_EdgeWidth + m_int_BarOffset
                                , (int) float_ProgressDrawWidth
                                , (int) float_ProgressHeight);
                        }
                        else
                        {
                            rect_Bar = new Rectangle(
                                m_EdgeWidth + m_int_BarOffset
                                , m_EdgeWidth + m_int_BarOffset
                                , (int) float_ProgressDrawWidth
                                , (int) float_ProgressHeight);
                        }

                        var brsh_Bar = new LinearGradientBrush(rect_Bar2, m_Color2, m_Color1,
                                                               (m_FloodStyle == ProgressFloodStyle.Standard)
                                                                   ? 90.0f
                                                                   : 0.0f);
                        float[] factors = {0.0f, 1.0f, 1.0f, 0.0f};
                        float[] positions = {0.0f, m_float_BarFlood, 1.0f - m_float_BarFlood, 1.0f};

                        var blend = new Blend();
                        blend.Factors = factors;
                        blend.Positions = positions;
                        brsh_Bar.Blend = blend;

                        switch (m_Style)
                        {
                            case ProgressStyle.Solid:
                                {
                                    e.Graphics.FillRectangle(brsh_Bar, rect_Bar);
                                    break;
                                }
                            case ProgressStyle.Dashed:
                                {
                                    if (m_bool_Invert)
                                    {
                                        if (int_NumberOfDashes == 0) int_NumberOfDashes = -1;
                                        for (int i = 0; i < int_NumberOfDashes + 1; i++)
                                        {
                                            int j = i + (int_TotalDashes - int_NumberOfDashes);
                                            e.Graphics.FillRectangle(brsh_Bar,
                                                                     new Rectangle(
                                                                         m_EdgeWidth + m_int_BarOffset +
                                                                         (j*(m_int_DashWidth + m_int_DashSpace)),
                                                                         m_EdgeWidth + m_int_BarOffset, m_int_DashWidth,
                                                                         (int) float_ProgressHeight));
                                        }
                                    }
                                    else
                                    {
                                        if (int_NumberOfDashes == 0) int_NumberOfDashes = -1;
                                        for (int i = 0; i < int_NumberOfDashes + 1; i++)
                                        {
                                            e.Graphics.FillRectangle(brsh_Bar,
                                                                     new Rectangle(
                                                                         m_EdgeWidth + m_int_BarOffset +
                                                                         (i*(m_int_DashWidth + m_int_DashSpace)),
                                                                         m_EdgeWidth + m_int_BarOffset, m_int_DashWidth,
                                                                         (int) float_ProgressHeight));
                                        }
                                    }
                                    break;
                                }
                        }
                        brsh_Bar.Dispose();
                        break;
                    }

                    #endregion

                    #region Vertical

                case ProgressBarDirection.Vertical:
                    {
                        float float_ProgressWidth = (Width - m_EdgeWidth*2 - m_int_BarOffset*2);
                        float float_ProgressTotalHeight = Height - m_EdgeWidth*2 - m_int_BarOffset*2;
                        float float_ProgressDrawHeight = float_ProgressTotalHeight/(m_int_Maximum - m_int_Minimum)*
                                                         (m_int_Value - m_int_Minimum);

                        var int_NumberOfDashes = (int) (float_ProgressDrawHeight/(m_int_DashWidth + m_int_DashSpace));
                        var int_TotalDashes = (int) (float_ProgressTotalHeight/(m_int_DashWidth + m_int_DashSpace));

                        var rect_Bar2 = new Rectangle(m_EdgeWidth + m_int_BarOffset, m_EdgeWidth + m_int_BarOffset,
                                                      (int) float_ProgressWidth, (int) float_ProgressTotalHeight);
                        Rectangle rect_Bar;
                        if (m_bool_Invert)
                        {
                            rect_Bar = new Rectangle(
                                m_EdgeWidth + m_int_BarOffset
                                ,
                                m_EdgeWidth + m_int_BarOffset +
                                (int) (float_ProgressTotalHeight - float_ProgressDrawHeight)
                                , (int) float_ProgressWidth
                                , (int) float_ProgressDrawHeight);
                        }
                        else
                        {
                            rect_Bar = new Rectangle(
                                m_EdgeWidth + m_int_BarOffset
                                , m_EdgeWidth + m_int_BarOffset
                                , (int) float_ProgressWidth
                                , (int) float_ProgressDrawHeight);
                        }

                        var brsh_Bar = new LinearGradientBrush(rect_Bar2, m_Color2, m_Color1,
                                                               (m_FloodStyle == ProgressFloodStyle.Standard)
                                                                   ? 0.0f
                                                                   : 90.0f);
                        float[] factors = {0.0f, 1.0f, 1.0f, 0.0f};
                        float[] positions = {0.0f, m_float_BarFlood, 1.0f - m_float_BarFlood, 1.0f};

                        var blend = new Blend();
                        blend.Factors = factors;
                        blend.Positions = positions;
                        brsh_Bar.Blend = blend;

                        switch (m_Style)
                        {
                            case ProgressStyle.Solid:
                                {
                                    e.Graphics.FillRectangle(brsh_Bar, rect_Bar);
                                    break;
                                }
                            case ProgressStyle.Dashed:
                                {
                                    if (m_bool_Invert)
                                    {
                                        if (int_NumberOfDashes == 0) int_NumberOfDashes = -1;
                                        for (int i = 0; i < int_NumberOfDashes + 1; i++)
                                        {
                                            int j = i + (int_TotalDashes - int_NumberOfDashes);
                                            e.Graphics.FillRectangle(brsh_Bar,
                                                                     new Rectangle(m_EdgeWidth + m_int_BarOffset,
                                                                                   m_EdgeWidth + m_int_BarOffset +
                                                                                   (j*
                                                                                    (m_int_DashWidth + m_int_DashSpace)),
                                                                                   (int) float_ProgressWidth,
                                                                                   m_int_DashWidth));
                                        }
                                    }
                                    else
                                    {
                                        if (int_NumberOfDashes == 0) int_NumberOfDashes = -1;
                                        for (int i = 0; i < int_NumberOfDashes + 1; i++)
                                        {
                                            e.Graphics.FillRectangle(brsh_Bar,
                                                                     new Rectangle(m_EdgeWidth + m_int_BarOffset,
                                                                                   m_EdgeWidth + m_int_BarOffset +
                                                                                   (i*
                                                                                    (m_int_DashWidth + m_int_DashSpace)),
                                                                                   (int) float_ProgressWidth,
                                                                                   m_int_DashWidth));
                                        }
                                    }
                                    break;
                                }
                        }
                        brsh_Bar.Dispose();
                        break;
                    }

                    #endregion
            }

            #endregion

            #region OnPaint - Draw Edge

            switch (m_Edge)
            {
                case ProgressBarEdge.Rectangle:
                    {
                        var pen = new Pen(m_EdgeColor);
                        var pen3 = new Pen(m_EdgeLightColor);
                        for (int i = 0; i < m_EdgeWidth; i++)
                        {
                            e.Graphics.DrawRectangle(pen, 0 + i, 0 + i, Width - 1 - i*2, Height - 1 - i*2);
                        }
                        e.Graphics.DrawLine(pen3, m_EdgeWidth, m_EdgeWidth, Width - 1 - m_EdgeWidth, m_EdgeWidth);
                        e.Graphics.DrawLine(pen3, m_EdgeWidth, m_EdgeWidth, m_EdgeWidth, Height - 1 - m_EdgeWidth);
                        break;
                    }
                case ProgressBarEdge.Rounded:
                    {
                        var pen = new Pen(m_EdgeColor);
                        var pen2 = new Pen(BackColor);
                        var pen3 = new Pen(m_EdgeLightColor);
                        for (int i = 0; i < m_EdgeWidth; i++)
                        {
                            e.Graphics.DrawRectangle(pen, 0 + i, 0 + i, Width - 1 - i*2, Height - 1 - i*2);
                        }
                        e.Graphics.DrawLine(pen2, 0, 0, 1, 0);
                        e.Graphics.DrawLine(pen2, 0, 0, 0, 1);
                        e.Graphics.DrawLine(pen2, 0, Height - 1, 1, Height - 1);
                        e.Graphics.DrawLine(pen2, 0, Height - 1, 0, Height - 2);
                        e.Graphics.DrawLine(pen2, Width - 2, 0, Width - 1, 0);
                        e.Graphics.DrawLine(pen2, Width - 1, 0, Width - 1, 1);
                        e.Graphics.DrawLine(pen2, Width - 2, Height - 1, Width - 1, Height - 1);
                        e.Graphics.DrawLine(pen2, Width - 1, Height - 2, Width - 1, Height - 1);

                        e.Graphics.FillRectangle(new SolidBrush(m_EdgeColor), m_EdgeWidth, m_EdgeWidth, 1, 1);
                        e.Graphics.FillRectangle(new SolidBrush(m_EdgeColor), m_EdgeWidth, Height - 1 - m_EdgeWidth, 1,
                                                 1);
                        e.Graphics.FillRectangle(new SolidBrush(m_EdgeColor), Width - 1 - m_EdgeWidth, m_EdgeWidth, 1, 1);
                        e.Graphics.FillRectangle(new SolidBrush(m_EdgeColor), Width - 1 - m_EdgeWidth,
                                                 Height - 1 - m_EdgeWidth, 1, 1);

                        e.Graphics.DrawLine(pen3, m_EdgeWidth + 1, m_EdgeWidth, Width - 2 - m_EdgeWidth, m_EdgeWidth);
                        e.Graphics.DrawLine(pen3, m_EdgeWidth, m_EdgeWidth + 1, m_EdgeWidth, Height - 2 - m_EdgeWidth);

                        break;
                    }
            }

            #endregion

            #region OnPaint - Draw Caption

            if (m_bool_Antialias) e.Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            var format = new StringFormat();
            format.LineAlignment = StringAlignment.Center;
            format.Alignment = StringAlignment.Center;
            format.Trimming = StringTrimming.EllipsisCharacter;
            switch (m_CaptionMode)
            {
                case ProgressCaptionMode.Value:
                    {
                        if (m_bool_Shadow)
                        {
                            e.Graphics.DrawString(m_int_Value.ToString(), Font, new SolidBrush(m_color_Shadow),
                                                  new Rectangle(m_int_ShadowOffset, m_int_ShadowOffset, Width, Height),
                                                  format);
                        }
                        e.Graphics.DrawString(m_int_Value.ToString(), Font, new SolidBrush(m_color_Caption),
                                              new Rectangle(0, 0, Width, Height), format);
                        break;
                    }
                case ProgressCaptionMode.Percent:
                    {
                        float float_Percentage = ((m_int_Value - m_int_Minimum)/(float) (m_int_Maximum - m_int_Minimum))*
                                                 100.0f;
                        if (m_bool_Shadow)
                        {
                            e.Graphics.DrawString(float_Percentage.ToString() + "%", Font,
                                                  new SolidBrush(m_color_Shadow),
                                                  new Rectangle(m_int_ShadowOffset, m_int_ShadowOffset, Width, Height),
                                                  format);
                        }
                        e.Graphics.DrawString(float_Percentage.ToString() + "%", Font, new SolidBrush(m_color_Caption),
                                              new Rectangle(0, 0, Width, Height), format);
                        break;
                    }
                case ProgressCaptionMode.Custom:
                    {
                        if (m_bool_Shadow)
                        {
                            e.Graphics.DrawString(GetCustomCaption(m_str_Caption), Font, new SolidBrush(m_color_Shadow),
                                                  new Rectangle(m_int_ShadowOffset, m_int_ShadowOffset, Width, Height),
                                                  format);
                        }
                        e.Graphics.DrawString(GetCustomCaption(m_str_Caption), Font, new SolidBrush(m_color_Caption),
                                              new Rectangle(0, 0, Width, Height), format);
                        break;
                    }
            }

            #endregion

            base.OnPaint(e);
        }

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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            // 
            // ProgressBar
            // 
            this.Name = "ProgressBar";
            this.Size = new System.Drawing.Size(256, 24);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ProgressBar_MouseUp);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ProgressBar_MouseMove);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ProgressBar_MouseDown);
        }

        #endregion

        #region ChangeByMouse

        private void ProgressBar_MouseDown(object sender, MouseEventArgs e)
        {
            /**/
        }

        private void ProgressBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (m_bool_ChangeByMouse && e.Button == MouseButtons.Left)
            {
                if (m_Direction == ProgressBarDirection.Horizontal)
                {
                    int int_ProgressWidth = Width - m_int_BarOffset*2 - m_EdgeWidth*2;
                    int int_MousePos = e.X - m_int_BarOffset - m_EdgeWidth;

                    float percentageClick = int_MousePos/(float) int_ProgressWidth;

                    int int_Range = m_int_Maximum - m_int_Minimum;
                    var int_NewValue = (int) (int_Range*percentageClick);
                    if (m_bool_Invert) int_NewValue = int_Range - int_NewValue;
                    int_NewValue += m_int_Minimum;
                    if (int_NewValue < m_int_Minimum) int_NewValue = m_int_Minimum;
                    if (int_NewValue > m_int_Maximum) int_NewValue = m_int_Maximum;
                    m_int_Value = int_NewValue;
                }
                else
                {
                    int int_ProgressWidth = Height - m_int_BarOffset*2 - m_EdgeWidth*2;
                    int int_MousePos = e.Y - m_int_BarOffset - m_EdgeWidth;

                    float percentageClick = int_MousePos/(float) int_ProgressWidth;

                    int int_Range = m_int_Maximum - m_int_Minimum;
                    var int_NewValue = (int) (int_Range*percentageClick);
                    if (m_bool_Invert) int_NewValue = int_Range - int_NewValue;
                    int_NewValue += m_int_Minimum;
                    if (int_NewValue < m_int_Minimum) int_NewValue = m_int_Minimum;
                    if (int_NewValue > m_int_Maximum) int_NewValue = m_int_Maximum;
                    m_int_Value = int_NewValue;
                }
                Invalidate();
            }
        }

        private void ProgressBar_MouseUp(object sender, MouseEventArgs e)
        {
            if (m_bool_ChangeByMouse)
            {
                if (m_Direction == ProgressBarDirection.Horizontal)
                {
                    int int_ProgressWidth = Width - m_int_BarOffset*2 - m_EdgeWidth*2;
                    int int_MousePos = e.X - m_int_BarOffset - m_EdgeWidth;

                    float percentageClick = int_MousePos/(float) int_ProgressWidth;

                    int int_Range = m_int_Maximum - m_int_Minimum;
                    var int_NewValue = (int) (int_Range*percentageClick);
                    if (m_bool_Invert) int_NewValue = int_Range - int_NewValue;
                    int_NewValue += m_int_Minimum;
                    if (int_NewValue < m_int_Minimum) int_NewValue = m_int_Minimum;
                    if (int_NewValue > m_int_Maximum) int_NewValue = m_int_Maximum;
                    m_int_Value = int_NewValue;
                }
                else
                {
                    int int_ProgressWidth = Height - m_int_BarOffset*2 - m_EdgeWidth*2;
                    int int_MousePos = e.Y - m_int_BarOffset - m_EdgeWidth;

                    float percentageClick = int_MousePos/(float) int_ProgressWidth;

                    int int_Range = m_int_Maximum - m_int_Minimum;
                    var int_NewValue = (int) (int_Range*percentageClick);
                    if (m_bool_Invert) int_NewValue = int_Range - int_NewValue;
                    int_NewValue += m_int_Minimum;
                    if (int_NewValue < m_int_Minimum) int_NewValue = m_int_Minimum;
                    if (int_NewValue > m_int_Maximum) int_NewValue = m_int_Maximum;
                    m_int_Value = int_NewValue;
                }
                Invalidate();
            }
        }

        #endregion
    }
}