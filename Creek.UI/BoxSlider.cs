using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Creek.UI
{
    public partial class BoxSlider : UserControl
    {
        private readonly List<BoxSliderItem> Items;
        private int _boxes;
        private Point oldpos;

        public BoxSlider()
        {
            Items = new List<BoxSliderItem>();
        }

        public Color BorderColor { get; set; }
        public Color FillColor { get; set; }
        public int Value { get; set; }

        public int Boxes
        {
            get { return _boxes; }
            set
            {
                _boxes = value;
                for (int i = 0; i < value; i++)
                {
                    Items.Add(new BoxSliderItem {rec = new Rectangle(10*(i*3), 10, 20, 20)});
                }

                Invalidate();
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            foreach (BoxSliderItem boxSliderItem in Items)
            {
                if (boxSliderItem.rec.Contains(e.Location))
                {
                    boxSliderItem.isActive = true;
                    oldpos = e.Location;
                }
                else
                {
                    // boxSliderItem.isActive = false;
                }
            }
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            foreach (BoxSliderItem boxSliderItem in Items)
            {
                if (!boxSliderItem.isActive)
                    e.Graphics.DrawRectangle(new Pen(BorderColor), boxSliderItem.rec);
                else
                {
                    e.Graphics.DrawRectangle(new Pen(BorderColor), boxSliderItem.rec);
                    e.Graphics.FillRectangle(new SolidBrush(FillColor),
                                             new Rectangle(new Point(boxSliderItem.rec.X + 1, boxSliderItem.rec.Y + 1),
                                                           new Size(boxSliderItem.rec.Width - 1,
                                                                    boxSliderItem.rec.Height - 1)));
                }
            }
        }
    }

    internal class BoxSliderItem
    {
        public bool isActive;
        public Rectangle rec;
    }
}