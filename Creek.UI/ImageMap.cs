using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Xml;

namespace Creek.UI
{
    /// <summary>
    /// Summary description for ImageMap.
    /// </summary>
    public partial class ImageMap
    {
        #region Delegates

        public delegate void RegionClickDelegate(int index, string key);

        public delegate void RegionHoverDelegate(int index, string key);

        #endregion

        public ImageMap()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            // TODO: Add any initialization after the InitForm call
            _pathsArray = new ArrayList();
            _pathData = new GraphicsPath();
            _pathData.FillMode = FillMode.Winding;

            components = new Container();
            _toolTip = new ToolTip(components);
            _toolTip.AutoPopDelay = 5000;
            _toolTip.InitialDelay = 1000;
            _toolTip.ReshowDelay = 500;

            _graphics = Graphics.FromHwnd(pictureBox.Handle);
        }

        public XmlDocument Doc
        {
            get { return new XmlDocument(); }
        }

        [Category("Action")]
        public event RegionClickDelegate RegionClick;

        [Category("Action")]
        public event RegionHoverDelegate RegionHover;

        public int AddElipse(string key, Point center, int radius)
        {
            return AddElipse(key, center.X, center.Y, radius);
        }

        public int AddElipse(string key, int x, int y, int radius)
        {
            if (_pathsArray.Count > 0)
                _pathData.SetMarkers();
            _pathData.AddEllipse(x - radius, y - radius, radius*2, radius*2);
            return _pathsArray.Add(key);
        }

        public int AddRectangle(string key, int x1, int y1, int x2, int y2)
        {
            return AddRectangle(key, new Rectangle(x1, y1, (x2 - x1), (y2 - y1)));
        }

        public int AddRectangle(string key, Rectangle rectangle)
        {
            if (_pathsArray.Count > 0)
                _pathData.SetMarkers();
            _pathData.AddRectangle(rectangle);
            return _pathsArray.Add(key);
        }

        public int AddPolygon(string key, Point[] points)
        {
            if (_pathsArray.Count > 0)
                _pathData.SetMarkers();
            _pathData.AddPolygon(points);
            return _pathsArray.Add(key);
        }

        public void FromXml(string s)
        {
            Doc.LoadXml(s);

            foreach (XmlNode cn in Doc.FirstChild.ChildNodes)
            {
                if (cn.Name == "polygon")
                {
                    string key = "";
                    var points = new List<Point>();
                    foreach (XmlNode p in cn.ChildNodes)
                    {
                        key = p.Attributes[0].Value;
                        if (p.Name == "point")
                        {
                            string x = p.Attributes["x"].Value;
                            string y = p.Attributes["y"].Value;

                            points.Add(new Point(Convert.ToInt32(x), Convert.ToInt32(y)));
                        }
                    }
                    AddPolygon(key, points.ToArray());
                }
                else if (cn.Name == "rectangle")
                {
                    AddRectangle(cn.Attributes["key"].Value,
                                 new Rectangle(Convert.ToInt32(cn.Attributes["x"].Value),
                                               Convert.ToInt32(cn.Attributes["y"].Value),
                                               Convert.ToInt32(cn.Attributes["w"].Value),
                                               Convert.ToInt32(cn.Attributes["h"].Value)));
                }
            }
        }
    }
}