using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Creek.UI.Unity3.Controls
{
    public class ColorGrid : UserControl
    {
        private readonly ToolTip ttp;
        public List<NamedColor> Items;
        private Color _Color;
        private byte _GridPadding = 4;

        private Size _GridSize = new Size(40, 20);
        private int _SelectedIndex = -1;
        private IContainer components;
        private int hoverIndex = -1;
        private ToolTip toolTip1;

        public ColorGrid()
        {
            InitializeComponent();
            SetStyle(
                ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
            Items = new List<NamedColor>(36);
            ttp = new ToolTip();
            ttp.InitialDelay = 5;
            ttp.ReshowDelay = 5;
            LoadDefaultColors(true);
        }

        public Size GridSize
        {
            get { return _GridSize; }
            set
            {
                _GridSize = value;
                Invalidate();
            }
        }

        public byte GridPadding
        {
            get { return _GridPadding; }
            set
            {
                _GridPadding = value;
                Invalidate();
            }
        }


        public int SelectedIndex
        {
            get { return _SelectedIndex; }
            set
            {
                if (_SelectedIndex != value)
                {
                    _SelectedIndex = value;
                    if (_SelectedIndex != -1)
                        _Color = Items[_SelectedIndex].Color;
                    Invalidate();
                    OnSelectedIndexChange();
                }
            }
        }

        public Color Color
        {
            get { return _Color; }
            set
            {
                _Color = value;
                if (value == Color.Empty)
                    SelectedIndex = -1;
                else
                    SelectedIndex = IndexOf(value, true);
            }
        }


        public bool ClipColors { get; set; }
        protected int YOffset { get; set; }

        protected int XOffset { get; set; }


        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            Point pos = PointToClient(MousePosition);
            int index = IndexOf(pos);
            if (index != -1 && index != hoverIndex)
            {
                hoverIndex = index;
                toolTip1.SetToolTip(this, Items[index].ToString());
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            ttp.Active = false;
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            int index = IndexOf(e.Location);
            if (index != -1)
            {
                SelectedIndex = index;
            }
        }


        protected void LoadDefaultColors(bool clearExistingColors)
        {
            if (clearExistingColors)
                Items.Clear();

            Items.Capacity = 36;
            var arrStrHues = new[] {"Red", "Yellow", "Green", "Cyan", "Blue", "Magenta"};

            var c = new ColorManager.HSL();
            float increment = 1.0F/6;
            float value = 0;
            //do the black to white colors first
            c.S = 0.0F;

            for (value = 0; value <= 1.0F; value += increment)
            {
                c.L = value;
                if (value == 0 || value == 1.0F)
                    Items.Add(new NamedColor(ColorManager.HSL_to_RGB(c), value == 0 ? "Black" : "White"));
                else
                    Items.Add(new NamedColor(ColorManager.HSL_to_RGB(c),
                                             "Black (" + Math.Round(value*100.0F).ToString() + "% Light)"));
            }
            increment = (1.0F - .30F)/3.0F;
            float sat_increment = (.70F - .10F)/2.0F;


            //now do the default colors
            c.H = .0F;
            c.S = 1.0F;
            for (int i = 0; i < arrStrHues.Length; i++, c.H += .16F, c.S = 1.0F)
            {
                for (value = .30F; value <= 1.0F; value += increment)
                {
                    c.L = value;
                    Items.Add(new NamedColor(ColorManager.HSL_to_RGB(c),
                                             arrStrHues[i] +
                                             (value < 1.0F
                                                  ? " (" + Math.Round((1.0F - value)*100.0F).ToString() + "% Dark)"
                                                  : "")));
                }

                c.L = 1.0F;
                for (value = .70F; value >= .10F; value -= sat_increment)
                {
                    c.S = value;
                    Items.Add(new NamedColor(ColorManager.HSL_to_RGB(c),
                                             arrStrHues[i] + " (" + Math.Round((1.0F - value)*100.0F).ToString() +
                                             "% Light)"));
                }
            }
        }

        public event EventHandler SelectedIndexChange;

        protected void OnSelectedIndexChange()
        {
            if (SelectedIndexChange != null)
                SelectedIndexChange(null, null);
        }

        public int IndexOf(Point point)
        {
            point.Offset(XOffset, YOffset);
            int colorsPerLine = (Width/(_GridSize.Width + _GridPadding));
            var column = (int) Math.Round((float) (point.X/(_GridSize.Width + _GridPadding)));
            int line = (int) Math.Round((float) (point.Y/(_GridSize.Height + _GridPadding))) + 1;

            int index = colorsPerLine*(line - 1) + column;
            return index < Items.Count ? index : -1;
        }

        public int IndexOf(Color color, bool ignoreAlpha)
        {
            int intCount = Items.Count;
            for (int i = 0; i < intCount; i++)
            {
                if ((ignoreAlpha ? true : Items[i].Color.A == color.A) &&
                    Items[i].Color.R == color.R &&
                    Items[i].Color.G == color.G &&
                    Items[i].Color.B == color.B)
                    return i;
            }
            return -1;
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            Size minArea = Size;
            int intCount = Items.Count;

            e.Graphics.TranslateTransform(-XOffset, -YOffset);

            int colorsPerLine = (minArea.Width/(_GridSize.Width + _GridPadding));
            int startingLine = YOffset == 0 ? 0 : ((_GridSize.Height + _GridPadding)/YOffset);
            int index = startingLine*colorsPerLine;

            var gridBounds = new Rectangle(_GridPadding, _GridPadding,
                                           _GridSize.Width, _GridSize.Height);


            var brush = new SolidBrush(Color.Black);
            for (; index < intCount; index++)
            {
                if (Items[index].Color.A != 255)
                    drawTransparencyGrid(e.Graphics, gridBounds);
                brush.Color = Items[index].Color;
                //paint the color
                e.Graphics.FillRectangle(brush, gridBounds);


                e.Graphics.DrawRectangle(Pens.Gray, gridBounds);
                if (index == _SelectedIndex)
                {
                    Rectangle r = gridBounds;
                    r.Inflate(2, 2);
                    e.Graphics.DrawRectangle(Pens.Blue, r);
                }
                //update the gridBounds
                gridBounds.X += (_GridSize.Width + _GridPadding);
                if (gridBounds.X + _GridSize.Width > minArea.Width)
                    gridBounds.X = XOffset == 0 ? _GridPadding : ((_GridSize.Width + _GridPadding)/XOffset);

                if (gridBounds.X == _GridPadding)
                    gridBounds.Y += (_GridSize.Height + _GridPadding);
                //if (gridBounds.Y + _GridSize.Width > minArea.Width)
                //    gridBounds.X = (int)((_GridSize.Width + _GridPadding) / Math.Abs(XOffset));
            }
        }

        private void drawTransparencyGrid(Graphics g, Rectangle area)
        {
            bool b = false;
            var r = new Rectangle(0, 0, 8, 8);
            g.FillRectangle(Brushes.White, area);
            for (r.Y = area.Y; r.Y < area.Bottom; r.Y += 8)
                for (r.X = ((b = !b) ? area.X : 8); r.X < area.Right; r.X += 16)
                    g.FillRectangle(Brushes.LightGray, r);
        }

        private void InitializeComponent()
        {
            components = new Container();
            toolTip1 = new ToolTip(components);
            SuspendLayout();
            // 
            // ColorGrid
            // 
            Name = "ColorGrid";
            ResumeLayout(false);
        }
    }

    public struct NamedColor
    {
        private Color _Color;

        private string _Name;

        public NamedColor(Color color, string name)
        {
            _Color = color;
            _Name = name;
        }

        public Color Color
        {
            get { return _Color; }
            set { _Color = value; }
        }

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        public override string ToString()
        {
            if (!string.IsNullOrEmpty(_Name))
                return _Name;
            else if (_Color.IsKnownColor)
                return _Color.ToKnownColor().ToString();
            else
                return GetFormattedColorString(_Color);
        }

        public static string GetColorName(Color color)
        {
            if (color.A != 255)
                return GetFormattedColorString(color);

            if (color.R == 0 && color.G == 0 && color.B == 0)
                return "Black";
            if (color.R == 255 && color.G == 0 && color.B == 0)
                return "Red";
            if (color.R == 0 && color.G == 255 && color.B == 0)
                return "Green";
            if (color.R == 0 && color.G == 0 && color.B == 255)
                return "Blue";
            if (color.R == 255 && color.G == 102 && color.B == 0)
                return "Orange";


            return GetFormattedColorString(color);
        }

        public static string GetFormattedColorString(Color color)
        {
            return color.R + ", " + color.G + ", " + color.B;
        }
    }
}