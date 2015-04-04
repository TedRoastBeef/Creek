using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace Creek.UI
{
    /// <summary>
    /// Summary description for Class1.
    /// </summary>
    [DefaultEvent("Clicked")]
    public class Toolbar : Control
    {
        #region Delegates

        public delegate void EventHandler(int selectedIndex);

        #endregion

        #region Add on - not finished yet

        // I would like that the properties can be adjusted in categories. 
        // So e.g. linecolor category contains normal, over, down.
        private ColorsTemp _lineColor = new ColorsTemp();

        [Browsable(false)]
        public ColorsTemp LineColors
        {
            get { return _lineColor; }
            set { _lineColor = value; }
        }

        #region Nested type: ColorsTemp

        [TypeConverter(typeof (ColorsTempConverter))]
        public class ColorsTemp
        {
            private Color _down = Color.Black;
            private Color _normal = Color.Black;
            private Color _over = Color.Black;

            public Color Normal
            {
                get { return _normal; }
                set { _normal = value; }
            }

            public Color Over
            {
                get { return _over; }
                set { _over = value; }
            }

            public Color Down
            {
                get { return _down; }
                set { _down = value; }
            }
        }

        #endregion

        #region Nested type: ColorsTempConverter

        internal class ColorsTempConverter : ExpandableObjectConverter
        {
            public override bool CanConvertFrom(
                ITypeDescriptorContext context, Type t)
            {
                if (t == typeof (string))
                {
                    return false;
                }
                return base.CanConvertFrom(context, t);
            }

            public override object ConvertFrom(
                ITypeDescriptorContext context,
                CultureInfo info,
                object value)
            {
                if (value is string)
                {
                    try
                    {
                        var s = (string) value;
                        string[] colorsArray = s.Split('|');

                        var colors = new ColorsTemp();
                        colors.Normal = Color.FromName(colorsArray[1]);
                        colors.Over = Color.FromName(colorsArray[2]);
                        colors.Down = Color.FromName(colorsArray[0]);
                    }
                    catch
                    {
                    }
                    // if we got this far, complain that we
                    // couldn't parse the string
                    //
                    throw new ArgumentException(
                        "Can not convert '" + (string) value +
                        "' to type ColorsTemp");
                }
                return base.ConvertFrom(context, info, value);
            }

            public override object ConvertTo(
                ITypeDescriptorContext context,
                CultureInfo culture,
                object value,
                Type destType)
            {
                if (destType == typeof (string) && value is ColorsTemp)
                {
                    var colors = value as ColorsTemp;
                    return string.Join("|", new[] {colors.Normal.Name, colors.Over.Name, colors.Down.Name});
                }

                return base.ConvertTo(context, culture, value, destType);
            }
        }

        #endregion

        #endregion

        #region Events

        public event EventHandler Clicked;
        public event EventHandler ItemChanged;

        #endregion

        #region Enums

        private enum State
        {
            Over,
            Down
        }

        #endregion

        #region Size parameters

        private int _lineSize = 1;
        private int _offset = 1;
        private int _sizeOfButtonBorderAndSpace;

        #endregion

        #region Selected parameters

        private int _selectedIndex = -1;
        private State _state;

        #endregion

        #region Color Parameters

        //Next 3 internal variables only used as cache
        private Brush _brush = Brushes.Transparent;
        private Color _brushColor = Color.Transparent;
        private Color _brushColorDown = Color.Transparent;
        private Color _brushColorOver = Color.Transparent;

        private Brush _brushDown = Brushes.Transparent;
        private Brush _brushOver = Brushes.Transparent;
        private Pen _pen = Pens.Black;
        private Pen _penDown = Pens.Black;
        private Pen _penOver = Pens.Black;

        #endregion

        #region Other Parameters

        //private Alignment _alignment = Alignment.Top;
        private ContentAlignment _alignment = ContentAlignment.TopCenter;
        private ImageList _iList = new ImageList();

        #endregion

        public Toolbar()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer | ControlStyles.UserPaint |
                ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor, true);
            Dock = DockStyle.Bottom;
        }

        #region Invokers

        private void InvokeItemChanged(int selectedIndex)
        {
            if (ItemChanged != null)
                ItemChanged(selectedIndex);
        }

        private void InvokeClicked(int selectedIndex)
        {
            if (Clicked != null)
                Clicked(selectedIndex);
        }

        #endregion

        #region Helpers

        private void CalculateRect(ref Rectangle smallButtonRect, ref Rectangle largeButtonRect)
        {
            if (_iList.Images.Count == 0)
            {
                smallButtonRect = new Rectangle(_offset, _offset, 0, _iList.ImageSize.Height + _lineSize*2);
                largeButtonRect = new Rectangle(_offset, _offset, 0, (_iList.ImageSize.Height + _lineSize)*2);
            }

            int left = 0;
            int smallButtonTop = 0;
            int width = _sizeOfButtonBorderAndSpace*_iList.Images.Count + _iList.ImageSize.Width/2;

            switch (_alignment)
            {
                case ContentAlignment.TopLeft:
                    left = _offset;
                    smallButtonTop = _offset;
                    break;
                case ContentAlignment.TopCenter:
                    left = (Width - width)/2;
                    smallButtonTop = _offset;
                    break;
                case ContentAlignment.TopRight:
                    left = Width - width - _offset;
                    smallButtonTop = _offset;
                    break;
                case ContentAlignment.MiddleLeft:
                    left = _offset;
                    smallButtonTop = _iList.ImageSize.Height/2 + _offset;
                    break;
                case ContentAlignment.MiddleCenter:
                    left = (Width - width)/2;
                    smallButtonTop = _iList.ImageSize.Height/2 + _offset;
                    break;
                case ContentAlignment.MiddleRight:
                    left = Width - width - _offset;
                    smallButtonTop = _iList.ImageSize.Height/2 + _offset;
                    break;
                case ContentAlignment.BottomLeft:
                    left = _offset;
                    smallButtonTop = _iList.ImageSize.Height + _offset;
                    break;
                case ContentAlignment.BottomCenter:
                    left = (Width - width)/2;
                    smallButtonTop = _iList.ImageSize.Height + _offset;
                    break;
                case ContentAlignment.BottomRight:
                    left = Width - width - _offset;
                    smallButtonTop = _iList.ImageSize.Height + _offset;
                    break;
            }

            smallButtonRect = new Rectangle(left, smallButtonTop, width, _iList.ImageSize.Height);
            largeButtonRect = new Rectangle(left, _offset, width, _iList.ImageSize.Height*2);
        }

        #endregion

        #region Drawing Helper Functions

        private void DrawPicturesAndFrames(Graphics g, Rectangle smallButtonRect, Rectangle largeButtonRect)
        {
            for (int i = 0; i < _iList.Images.Count; i++)
            {
                int x = i*_sizeOfButtonBorderAndSpace + smallButtonRect.Left + _iList.ImageSize.Width/2;

                if (_selectedIndex == i) // Draw upfront picture(scaled)
                {
                    var destRectBB = new Rectangle(x - _iList.ImageSize.Width/2, _offset, _iList.ImageSize.Width*2 + 1,
                                                   _iList.ImageSize.Height*2 + 1);
                    var destRect = new Rectangle(x - _iList.ImageSize.Width/2 + 1, _offset + 1, _iList.ImageSize.Width*2,
                                                 _iList.ImageSize.Height*2);
                    var sourceRect = new Rectangle(0, 0, _iList.ImageSize.Width, _iList.ImageSize.Height);

                    // Draw border of button
                    g.DrawRectangle(_state == State.Over ? _penOver : _penDown, destRectBB);

                    // Draw background color
                    Brush brush = null;

                    if (_state == State.Over)
                    {
                        if (_brushColorOver != Color.Transparent)
                            brush = _brushOver;
                    }
                    else
                    {
                        if (_brushColorDown != Color.Transparent)
                            brush = _brushDown;
                    }

                    if (brush != null)
                        g.FillRectangle(brush, destRect);

                    // Draw scaled image
                    g.DrawImage(_iList.Images[i], destRect, sourceRect, GraphicsUnit.Pixel);
                }
                else // Draw normally
                {
                    var destRect = new Rectangle(x, smallButtonRect.Top, _iList.ImageSize.Width + 1,
                                                 _iList.ImageSize.Height + 1);

                    // Draw border of button
                    g.DrawRectangle(_pen, destRect);

                    // Draw background color
                    if (_brushColor != Color.Transparent)
                    {
                        destRect = new Rectangle(x + _lineSize, smallButtonRect.Top + _lineSize, _iList.ImageSize.Width,
                                                 _iList.ImageSize.Height);
                        g.FillRectangle(_brush, destRect);
                    }

                    // Draw image
                    g.DrawImage(_iList.Images[i], x + 1, smallButtonRect.Top + 1);
                }
            }
        }

        private void DrawLine(Graphics g, Rectangle smallButtonRect, Rectangle largeButtonRect)
        {
            int middleLine = smallButtonRect.Top + (smallButtonRect.Bottom - smallButtonRect.Top)/2;

            g.DrawLine(_pen, 0, middleLine, smallButtonRect.Left, middleLine);
            g.DrawLine(_pen, smallButtonRect.Right, middleLine, Width, middleLine);

            // Drawing lines between boxes
            var blackPen = new Pen(_pen.Color, _lineSize);
            blackPen.DashPattern = new float[] {_iList.ImageSize.Width/2, _iList.ImageSize.Width + _lineSize*2};

            if (_selectedIndex != -1)
            {
                if (_selectedIndex != 0) // Draw line until selectedIndex
                    g.DrawLine(blackPen, new Point(smallButtonRect.Left, middleLine),
                               new Point(smallButtonRect.Left + _selectedIndex*_sizeOfButtonBorderAndSpace, middleLine));

                if (_selectedIndex != (_iList.Images.Count - 1)) // Draw line after selectedIndex
                    g.DrawLine(blackPen,
                               new Point(smallButtonRect.Left + (_selectedIndex + 2)*_sizeOfButtonBorderAndSpace,
                                         middleLine), new Point(smallButtonRect.Right, middleLine));
            }
            else
            {
                g.DrawLine(blackPen, new Point(smallButtonRect.Left, middleLine),
                           new Point(smallButtonRect.Right, middleLine));
            }
        }

        #endregion

        #region Properties

        [Category("Behavior")]
        public ImageList ImageList
        {
            get { return _iList; }
            set
            {
                _iList = value;

                if (_iList != null)
                {
                    Height = (_iList.ImageSize.Height + _lineSize + _offset)*2;
                    _sizeOfButtonBorderAndSpace = _iList.ImageSize.Width + _lineSize*2 + _iList.ImageSize.Width/2;
                }

                Invalidate();
            }
        }

        [Category("Appearance"), Description("Where to place the toolbar in the control")]
        public ContentAlignment ContentAlignment
        {
            get { return _alignment; }
            set
            {
                _alignment = value;
                Invalidate();
            }
        }

        [Category("Appearance"), Description("The color of the toolbar items bounding box when it is being pressed")]
        public Color LineColorDown
        {
            get { return _penDown.Color; }
            set
            {
                _penDown = new Pen(value, _lineSize);
                Invalidate();
            }
        }

        [Category("Appearance"), Description("The color of the toolbar items bounding box when the mouse is over")]
        public Color LineColorOver
        {
            get { return _penOver.Color; }
            set
            {
                _penOver = new Pen(value, _lineSize);
                Invalidate();
            }
        }

        [Category("Appearance"), Description("The color of all lines by default")]
        public Color LineColorNormal
        {
            get { return _pen.Color; }
            set
            {
                _pen = new Pen(value, _lineSize);
                Invalidate();
            }
        }

        [Category("Appearance"),
         Description("The toolbar item background by default. This only shows for pictures with transparent background")
        ]
        public Color BackgroundColorNormal
        {
            get { return _brushColor; }
            set
            {
                _brushColor = value;
                _brush = new SolidBrush(_brushColor);
                Invalidate();
            }
        }

        [Category("Appearance"),
         Description(
             "The menu item background color when mouse is over. This only shows for pictures with transparent background"
             )]
        public Color BackgroundColorOver
        {
            get { return _brushColorOver; }
            set
            {
                _brushColorOver = value;
                _brushOver = new SolidBrush(_brushColorOver);
                Invalidate();
            }
        }

        [Category("Appearance"),
         Description(
             "The menu item background color when mouse is being pressed. This only shows for pictures with transparent background"
             )]
        public Color BackgroundColorDown
        {
            get { return _brushColorDown; }
            set
            {
                _brushColorDown = value;
                _brushDown = new SolidBrush(_brushColorDown);
                Invalidate();
            }
        }

        #endregion

        #region Overrides

        protected override void OnSizeChanged(EventArgs e)
        {
            if (_iList != null)
            {
                int correctHeight = (_iList.ImageSize.Height + _lineSize + _offset)*2;

                if (Height != correctHeight)
                    Height = correctHeight;
            }

            base.OnSizeChanged(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var smallButtonRect = new Rectangle(0, 0, 0, 0);
            Rectangle largeButtonRect = smallButtonRect;

            CalculateRect(ref smallButtonRect, ref largeButtonRect);

            DrawLine(e.Graphics, smallButtonRect, largeButtonRect);
            DrawPicturesAndFrames(e.Graphics, smallButtonRect, largeButtonRect);

            base.OnPaint(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            bool invalidate = false;

            var smallButtonRect = new Rectangle(0, 0, 0, 0);
            Rectangle largeButtonRect = smallButtonRect;

            CalculateRect(ref smallButtonRect, ref largeButtonRect);

            if (smallButtonRect.Contains(e.X, e.Y))
            {
                int mouseXWithoutOffset = e.X - smallButtonRect.Left - _iList.ImageSize.Width/2;
                var buttonIndex = (int) Math.Floor((double) mouseXWithoutOffset/_sizeOfButtonBorderAndSpace);

                if ((mouseXWithoutOffset - buttonIndex*_sizeOfButtonBorderAndSpace) <= _iList.ImageSize.Width)
                    // mouse over index = buttonIndex
                {
                    invalidate = buttonIndex != _selectedIndex;

                    _state = e.Button == MouseButtons.Left ? State.Down : State.Over;

                    _selectedIndex = buttonIndex;
                }
                else // mouse over nothing
                {
                    invalidate = _selectedIndex != -1;
                    _selectedIndex = -1;
                }
            }
            else // mouse over nothing
            {
                invalidate = _selectedIndex != -1;
                _selectedIndex = -1;
            }

            base.OnMouseMove(e);

            if (invalidate)
            {
                InvokeItemChanged(_selectedIndex);
                Invalidate();
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            bool invalidate = _selectedIndex != -1;
            _selectedIndex = -1;

            base.OnMouseLeave(e);

            if (invalidate)
                Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            bool invalidate = false;

            if (_selectedIndex != -1)
            {
                invalidate = true;
                _state = State.Down;
            }

            base.OnMouseDown(e);

            if (invalidate)
                Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            bool invalidate = false;

            if (_selectedIndex != -1)
            {
                InvokeClicked(_selectedIndex);
                invalidate = true;
                _state = State.Over;
            }

            base.OnMouseUp(e);

            if (invalidate)
                Invalidate();
        }

        #endregion
    }
}