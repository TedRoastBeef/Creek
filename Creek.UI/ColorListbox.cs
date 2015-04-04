using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Creek.UI
{
    public sealed class ColorListBox : ListBox
    {
        private readonly ColorListBoxItemCollection _Items;
        private bool _ShowImages;
        private ContentAlignment _TextAlign;

        public ColorListBox()
        {
            DrawMode = DrawMode.OwnerDrawFixed;
            _Items = new ColorListBoxItemCollection(this);

            _ShowImages = true;
            _TextAlign = ContentAlignment.MiddleLeft;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public new ColorListBoxItemCollection Items
        {
            get { return _Items; }
        }

        //The original items that the user will never see.
        private ObjectCollection baseItems
        {
            get { return base.Items; }
        }

        public new ColorListBoxItem SelectedItem
        {
            get { return (ColorListBoxItem) base.SelectedItem; }
            set { base.SelectedItem = value; }
        }

        public new ColorListBoxSelectedItemCollection SelectedItems
        {
            get
            {
                var items = new ColorListBoxSelectedItemCollection();
                foreach (object item in base.SelectedItems)
                {
                    items.Add((ColorListBoxItem) item);
                }
                return items;
            }
        }

        [DefaultValue(true)]
        public bool ShowImages
        {
            get { return _ShowImages; }
            set
            {
                if (_ShowImages != value)
                {
                    _ShowImages = value;
                    Invalidate();
                }
            }
        }

        [DefaultValue(ContentAlignment.MiddleLeft)]
        public ContentAlignment TextAlign
        {
            get { return _TextAlign; }
            set
            {
                if (_TextAlign != value)
                {
                    _TextAlign = value;
                    Invalidate();
                }
            }
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            base.OnDrawItem(e);

            //Draw original background and selection.
            //You can remove this and draw your own background if you want.
            e.DrawBackground();
            e.DrawFocusRectangle();

            if (e.Index >= 0 && e.Index < Items.Count)
            {
                ColorListBoxItem item = Items[e.Index];
                if (item != null)
                {
                    e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
                    if (ShowImages && item.Image != null)
                    {
                        //Draw the image
                        e.Graphics.DrawImage(item.Image, e.Bounds.X, e.Bounds.Y, ItemHeight, ItemHeight);
                    }
                }

                //Draw the item text
                DrawItemText(e, item);
            }
        }

        private void DrawItemText(DrawItemEventArgs e, ColorListBoxItem item)
        {
            float x = 0;
            float y = 0;
            SizeF textSize = e.Graphics.MeasureString(item.Text, Font);
            float w = textSize.Width;
            float h = textSize.Height;
            Rectangle bounds = e.Bounds;

            //If we are showing images, make some room for them and adjust the bounds width.
            if (ShowImages)
            {
                bounds.X += ItemHeight;
                bounds.Width -= ItemHeight;
            }

            //Depending on which TextAlign is chosen, determine the x and y position of the text.
            switch (TextAlign)
            {
                case ContentAlignment.BottomCenter:
                    x = bounds.X + (bounds.Width - w)/2;
                    y = bounds.Y + bounds.Height - h;
                    break;
                case ContentAlignment.BottomLeft:
                    x = bounds.X;
                    y = bounds.Y + bounds.Height - h;
                    break;
                case ContentAlignment.BottomRight:
                    x = bounds.X + bounds.Width - w;
                    y = bounds.Y + bounds.Height - h;
                    break;
                case ContentAlignment.MiddleCenter:
                    x = bounds.X + (bounds.Width - w)/2;
                    y = bounds.Y + (bounds.Height - h)/2;
                    break;
                case ContentAlignment.MiddleLeft:
                    x = bounds.X;
                    y = bounds.Y + (bounds.Height - h)/2;
                    break;
                case ContentAlignment.MiddleRight:
                    x = bounds.X + bounds.Width - w;
                    y = bounds.Y + (bounds.Height - h)/2;
                    break;
                case ContentAlignment.TopCenter:
                    x = bounds.X + (bounds.Width - w)/2;
                    y = bounds.Y;
                    break;
                case ContentAlignment.TopLeft:
                    x = bounds.X;
                    y = bounds.Y;
                    break;
                case ContentAlignment.TopRight:
                    x = bounds.X + bounds.Width - w;
                    y = bounds.Y;
                    break;
            }

            //Finally draw the text.
            e.Graphics.DrawString(item.Text, Font, new SolidBrush(item.Color), x, y);
        }


        //A collection of ColorListBoxItems

        #region Nested type: ColorListBoxItemCollection

        public class ColorListBoxItemCollection : Collection<ColorListBoxItem>
        {
            //Keep a reference to the ColorListBox so we can update its baseItems list

            private readonly ColorListBox _listBox;


            public ColorListBoxItemCollection(ColorListBox listBox)
            {
                _listBox = listBox;
            }


            public ColorListBoxItem Add(string text)
            {
                return Add(text, Color.Black, null);
            }

            public ColorListBoxItem Add(string text, Color color)
            {
                return Add(text, color, null);
            }

            public ColorListBoxItem Add(string text, Color color, Image img)
            {
                var item = new ColorListBoxItem(text, color, img);
                InsertItem(Items.Count, item);
                return item;
            }

            protected override void ClearItems()
            {
                base.ClearItems();
                _listBox.baseItems.Clear();
            }

            protected override void InsertItem(int index, ColorListBoxItem item)
            {
                base.InsertItem(index, item);
                _listBox.baseItems.Insert(index, item);
            }

            protected override void RemoveItem(int index)
            {
                base.RemoveItem(index);
                _listBox.baseItems.RemoveAt(index);
            }

            protected override void SetItem(int index, ColorListBoxItem item)
            {
                base.SetItem(index, item);
                _listBox.baseItems[index] = item;
            }

            public void AddRange(IEnumerable<ColorListBoxItem> items)
            {
                foreach (ColorListBoxItem item in items)
                {
                    InsertItem(Items.Count, item);
                }
            }
        }

        #endregion

        //A collection containing the selected items

        #region Nested type: ColorListBoxSelectedItemCollection

        public class ColorListBoxSelectedItemCollection : Collection<ColorListBoxItem>
        {
        }

        #endregion
    }

    //An item that is added to the ColorListBox
    public class ColorListBoxItem
    {
        public ColorListBoxItem(string text, Color color)
        {
            Text = text;
            Color = color;
        }

        public ColorListBoxItem(string text, Color color, Image img)
        {
            Text = text;
            Color = color;
            Image = img;
        }

        public string Text { get; set; }

        public Color Color { get; set; }

        public Image Image { get; set; }
    }
}