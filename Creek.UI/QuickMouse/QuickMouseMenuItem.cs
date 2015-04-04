using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Creek.UI.QuickMouse
{
    using Creek.UI.Winforms.Properties;

    [Serializable]
    public class QuickMouseMenuItem
    {
        /// <summary>
        /// Parent MenuItem
        /// </summary>
        public QuickMouseMenuItem ParentQuickMouseMenuItem = null;

        public List<QuickMouseMenuItem> lstSubQuickMouseMenuItems = new List<QuickMouseMenuItem>();
        public ToolStripMenuItem tsmenuitem;

        #region Properties

        private string caption = "";
        private string description = "";
        private string key;
        private Point location;

        private Image menuimage;
        public int order;

        [Browsable(false)]
        public Point Location
        {
            get { return location; }
            set { location = value; }
        }

        /// <summary>
        /// menu image
        /// </summary>
        public Image MenuImage
        {
            get { return menuimage; }
            set { menuimage = value; }
        }

        /// <summary>
        /// menudescription to tooltip
        /// </summary>
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        /// <summary>
        /// menucaption to tooltip
        /// </summary>
        public string Caption
        {
            get { return caption; }
            set { caption = value; }
        }

        /// <summary>
        /// the key to identify the menuitem
        /// </summary>
        public string Key
        {
            get { return key; }
            set { key = value; }
        }

        #endregion

        public QuickMouseMenuItem(Image img, string Caption, string Description)
        {
            menuimage = img;
            this.Caption = Caption;
            this.Description = Description;
        }

        public QuickMouseMenuItem()
        {
        }


        public QuickMouseMenuItem(Image img)
        {
            menuimage = img;
        }

        /// <summary>
        /// Add child menuItem
        /// </summary>
        /// <param name="childitem"></param>
        /// <returns></returns>
        public QuickMouseMenuItem AddChildQuickMouseMenuItem(QuickMouseMenuItem childitem)
        {
            lstSubQuickMouseMenuItems.Add(childitem);
            childitem.ParentQuickMouseMenuItem = this;
            return childitem;
        }

        /// <summary>
        /// Add child menu from Menustrip with Click event!!!
        /// </summary>
        /// <param name="menustrip"></param>
        /// <returns></returns>
        public QuickMouseMenuItem CreateChildQuickMouseMenuItemFromMenuStrip(ToolStripMenuItem menustrip)
        {
            var childitem = new QuickMouseMenuItem();
            childitem.caption = menustrip.Text;
            childitem.description = menustrip.ToolTipText;
            if (menustrip.Image == null)
            {
                childitem.menuimage = Resources.AppMenu32;
            }
            else
            {
                childitem.menuimage = menustrip.Image;
            }
            tsmenuitem = menustrip;

            lstSubQuickMouseMenuItems.Add(childitem);
            childitem.ParentQuickMouseMenuItem = this;
            return childitem;
        }
    }
}