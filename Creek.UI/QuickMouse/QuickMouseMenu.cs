using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Creek.UI.QuickMouse
{
    using Creek.UI.Winforms.Properties;

    public partial class QuickMouseMenu : Form
    {
        /// <summary>
        /// list  items
        /// </summary>        
        [Browsable(true)] [Category("QuickMouseMenu")] public List<QuickMouseMenuItem> lstRootItems;

        public QuickMouseMenu()
        {
            InitializeComponent();

            lstRootItems = new List<QuickMouseMenuItem>();

            SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            Visible = false;

            //if class is not a form, but a usercontrol
            //this.ParentChanged += new EventHandler(QuickMouseMenuUC_ParentChanged);
        }

        #region mouse down

        /// <summary>
        /// Set Mouse Down event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QuickMouseMenuUC_ParentChanged(object sender, EventArgs e)
        {
            //if class is not a form, but a usercontrol
            if (Parent != null)
            {
                //if (Parent is Form && (Parent as Form).IsMdiContainer)
                //{
                //    foreach (Control act in (Parent as Form).Controls)
                //    {
                //        if (act is MdiClient)
                //        {
                //            act.MouseDown += new MouseEventHandler(Parent_MouseDown);
                //            break;
                //        }
                //    }
                //}
                //else
                //{
                //   Parent.MouseDown += new MouseEventHandler(Parent_MouseDown);
                //}
            }
        }

        public void Parent_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ShowMenu(e.Location);
            }
        }

        #endregion

        #region Settings

        private bool hideonmouseleave = true;
        private bool navigateonhover;


        private bool savelastlocation = true;

        [Browsable(true)]
        [Category("QuickMouseMenu")]
        public bool NavigateOnHover
        {
            get { return navigateonhover; }
            set { navigateonhover = value; }
        }

        [Browsable(true)]
        [Category("QuickMouseMenu")]
        public bool SaveLastLocation
        {
            get { return savelastlocation; }
            set { savelastlocation = value; }
        }


        [Browsable(true)]
        [Category("QuickMouseMenu")]
        public bool HideOnMouseLeave
        {
            get { return hideonmouseleave; }
            set { hideonmouseleave = value; }
        }

        #endregion

        public void ShowMenu(Point loc)
        {
            Point mouse = Cursor.Position;
            //Point mouse = loc;            
            var new_loc = new Point(mouse.X - Width/2, mouse.Y - Height/2);

            Location = new_loc;
            if (!savelastlocation || navigated_parentQuickMouseMenuItem == null)
            {
                CreatePicBoxes(lstRootItems);
            }
            else
            {
                CreatePicBoxes(navigated_parentQuickMouseMenuItem.lstSubQuickMouseMenuItems);
            }

            Visible = true;
        }

        private void CreatePicBoxes(List<QuickMouseMenuItem> lstItems)
        {
            Controls.Clear();

            if (lstItems == lstRootItems)
            {
                root_level_active = true;
            }
            else
            {
                root_level_active = false;
            }

            foreach (QuickMouseMenuItem act in lstItems)
            {
                act.order = lstItems.IndexOf(act);
                act.Location = CalcNewItemLocation(act.order, lstItems.Count);
            }
            navigated_parentQuickMouseMenuItem = lstItems[0].ParentQuickMouseMenuItem;

            foreach (QuickMouseMenuItem act in lstItems)
            {
                var picbox = new PictureBox();

                picbox.Size = new Size(MenuItemSize.X, MenuItemSize.Y);

                picbox.MouseEnter += picbox_MouseEnter;
                picbox.MouseLeave += picbox_MouseLeave;
                picbox.MouseClick += picbox_MouseClick;
                picbox.MouseHover += picbox_MouseHover;


                picbox.Tag = act;

                picbox.Image = act.MenuImage;
                picbox.SizeMode = PictureBoxSizeMode.Zoom;
                picbox.Location = act.Location;

                if (act.Caption != "")
                {
                    toolTip1.SetToolTip(picbox, act.Caption + Environment.NewLine + act.Description);
                }

                Controls.Add(picbox);
            }

            var picBack = new PictureBox();

            picBack.Click += picBack_Click;
            picBack.MouseHover += picBack_MouseHover;
            picBack.DoubleClick += picBack_DoubleClick;

            picBack.Size = new Size(32, 32);
            picBack.Image = Resources.back_button;
            picBack.SizeMode = PictureBoxSizeMode.Zoom;

            picBack.Location = new Point(MenuCentral.X - 16, MenuCentral.Y - 16);

            Controls.Add(picBack);
        }

        //[Browsable(false)]
        //[Category("QuickMouseMenu")]
        //public List<QuickMouseMenuItem> QuickMenuItems
        //{
        //    get
        //    {
        //        return lstItems;
        //    }
        //    set
        //    {
        //        lstItems = value;
        //    }
        //}


        private void QuickMouseMenuUC_MouseLeave(object sender, EventArgs e)
        {
            if (!hideonmouseleave)
            {
                return;
            }

            bool hide = false;
            Point mouse = Cursor.Position;


            //Point uc_screenpoint =PointToScreen(this.Location);

            Point uc_screenpoint = Location;

            if (mouse.X <= uc_screenpoint.X || mouse.Y <= uc_screenpoint.Y)
            {
                hide = true;
            }
            if (mouse.X >= uc_screenpoint.X + Width || mouse.Y >= uc_screenpoint.Y + Height)
            {
                hide = true;
            }
            if (hide)
            {
                Visible = false;
            }
        }

        #region Mouse Events on Items

        private void picbox_MouseClick(object sender, MouseEventArgs e)
        {
            if (((sender as PictureBox).Tag as QuickMouseMenuItem).lstSubQuickMouseMenuItems.Count > 0)
            {
                NavigateToChild((sender as PictureBox).Tag as QuickMouseMenuItem);
                return;
            }

            if (QuickMenuItemClicked != null)
            {
                Visible = false;
                if (((sender as PictureBox).Tag as QuickMouseMenuItem).tsmenuitem != null)
                {
                    ((sender as PictureBox).Tag as QuickMouseMenuItem).tsmenuitem.PerformClick();
                }
                else
                {
                    QuickMenuItemClicked((sender as PictureBox).Tag, e);
                }
            }
        }

        private void picbox_MouseHover(object sender, EventArgs e)
        {
            if (navigateonhover)
            {
                NavigateToChild((sender as PictureBox).Tag as QuickMouseMenuItem);
            }
        }

        private void NavigateToChild(QuickMouseMenuItem qmmi)
        {
            if (qmmi.lstSubQuickMouseMenuItems.Count > 0)
            {
                //Point cur_loc = System.Windows.Forms.Cursor.Position;
                //this.Location = Parent.PointToClient(new Point(cur_loc.X - MenuCentral.X, cur_loc.Y - MenuCentral.Y));
                navigated_parentQuickMouseMenuItem = qmmi;
                CreatePicBoxes(qmmi.lstSubQuickMouseMenuItems);
            }
        }

        [Browsable(true)]
        [Category("QuickMouseMenu")]
        public event MouseEventHandler QuickMenuItemClicked;

        #endregion

        #region Back level

        private QuickMouseMenuItem navigated_parentQuickMouseMenuItem;

        private bool root_level_active = true;

        private void picBack_Click(object sender, EventArgs e)
        {
            if (root_level_active)
            {
                Visible = false;
                return;
            }
            if (navigated_parentQuickMouseMenuItem != null &&
                navigated_parentQuickMouseMenuItem.ParentQuickMouseMenuItem != null)
            {
                CreatePicBoxes(navigated_parentQuickMouseMenuItem.ParentQuickMouseMenuItem.lstSubQuickMouseMenuItems);
            }
            else
            {
                CreatePicBoxes(lstRootItems);
            }
        }

        private void picBack_DoubleClick(object sender, EventArgs e)
        {
            if (!root_level_active)
            {
                CreatePicBoxes(lstRootItems);
            }
        }

        private void picBack_MouseHover(object sender, EventArgs e)
        {
            //picBack_Click(null, null);            
        }

        #endregion

        #region Adding Item

        private Point menuItemSize = new Point(32, 32);

        [Browsable(false)]
        public Point MenuCentral
        {
            get { return new Point(Width/2, Height/2); }
        }

        [Category("QuickMouseMenu")]
        [Browsable(true)]
        public Point MenuItemSize
        {
            get { return menuItemSize; }
            set { menuItemSize = value; }
        }

        public QuickMouseMenuItem AddQuickMouseMenuItem(Image img, string Caption)
        {
            return AddQuickMouseMenuItem(null, img, Caption);
        }

        public QuickMouseMenuItem AddQuickMouseMenuItem(string key, Image img, string Caption)
        {
            return AddQuickMouseMenuItem(key, img, Caption, "");
        }

        public QuickMouseMenuItem AddQuickMouseMenuItem(Image img, string Caption, string Description)
        {
            return AddQuickMouseMenuItem(null, img, Caption, Description);
        }

        public QuickMouseMenuItem AddQuickMouseMenuItem(string key, Image img, string Caption, string Description)
        {
            var newitem = new QuickMouseMenuItem(img, Caption, Description);
            newitem.Key = key;
            lstRootItems.Add(newitem);

            return newitem;
        }


        private Point CalcNewItemLocation(int order, int count)
        {
            int X;
            int Y;

            var sin = (int) (Math.Sin(Math.PI*order*360/count/180)*(MenuCentral.X - menuItemSize.X));
            int cos = -1*(int) (Math.Cos(Math.PI*order*360/count/180)*(MenuCentral.Y - menuItemSize.Y));

            X = sin + MenuCentral.X - menuItemSize.X/2;
            Y = cos + MenuCentral.Y - menuItemSize.Y/2;
            return new Point(X, Y);
        }

        #endregion

        #region Removing item

        public void ClearItems()
        {
            lstRootItems.Clear();
        }

        #endregion

        #region Selecting Item

        private void picbox_MouseLeave(object sender, EventArgs e)
        {
            var p = sender as PictureBox;
            p.Size = new Size(p.Width - 5, p.Height - 5);
        }

        private void picbox_MouseEnter(object sender, EventArgs e)
        {
            var p = sender as PictureBox;
            p.Size = new Size(p.Width + 5, p.Height + 5);
        }

        #endregion
    }
}