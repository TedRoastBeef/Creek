/*--------------------------------------------------------------------------------------
 * Author: Rafey
 * 
 * Comments: Firefox Option Dialog User Control for .NET Win Apps
 * 
 * Email: syedrafey@gmail.com
 * 
 -------------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Creek.UI.MozOptions
{
    public partial class FirefoxDialog : UserControl
    {
        private readonly Dictionary<string, PageProp> pages = new Dictionary<string, PageProp>();
        private PropertyPage activePage;

        public FirefoxDialog()
        {
            InitializeComponent();
        }

        #region Private

        private void AddPage(MozItem item, PropertyPage page)
        {
            var pageProp = new PageProp();

            pageProp.Page = page;
            pageProp.MozItem = item;

            mozPane1.Items.Add(item);

            pages.Add(item.Name, pageProp);
        }

        private MozItem GetMozItem(string text)
        {
            return GetMozItem(text, ImageList == null ? 0 : pages.Count);
        }

        private MozItem GetMozItem(string text, int imageIndex)
        {
            var item = new MozItem();

            item.Name = "mozItem" + pages.Count + 1;

            item.Text = text;

            if (imageIndex < ImageList.Images.Count)
            {
                item.Images.NormalImage = ImageList.Images[imageIndex];
            }

            return item;
        }

        #region Activate Page

        private void mozPane1_ItemClick(object sender, MozItemClickEventArgs e)
        {
            ActivatePage(e.MozItem);
        }

        private bool ActivatePage(MozItem item)
        {
            if (!pages.ContainsKey(item.Name))
            {
                return false;
            }

            PageProp pageProp = pages[item.Name];

            PropertyPage page = pageProp.Page;

            if (activePage != null)
            {
                activePage.Visible = false;
            }

            activePage = page;

            if (activePage != null)
            {
                mozPane1.SelectByName(item.Name);

                activePage.Visible = true;

                if (!page.IsInit)
                {
                    page.OnInit();

                    page.IsInit = true;
                }

                activePage.OnSetActive();
            }

            return true;
        }

        #endregion

        #endregion

        #region Public Interface

        #region Properties

        public Dictionary<string, PageProp> Pages
        {
            get { return pages; }
        }

        public ImageList ImageList
        {
            get { return mozPane1.ImageList; }
            set { mozPane1.ImageList = value; }
        }

        #endregion

        #region Methods

        public void AddPage(string text, PropertyPage page)
        {
            AddPage(GetMozItem(text), page);
        }

        public void AddPage(string text, int imageIndex, PropertyPage page)
        {
            AddPage(GetMozItem(text, imageIndex), page);
        }

        public void Init()
        {
            foreach (PageProp pageProp in pages.Values)
            {
                PropertyPage page = pageProp.Page;

                pagePanel.Controls.Add(page);
                page.Dock = DockStyle.Fill;
                page.Visible = false;
            }

            if (pages.Count != 0)
            {
                ActivatePage(mozPane1.Items[0]);
            }
        }

        #endregion

        #endregion

        #region Dialog Buttons

        private void btnOK_Click(object sender, EventArgs e)
        {
            Apply();

            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            Apply();
        }

        private void Apply()
        {
            foreach (PageProp pageProp in pages.Values)
            {
                if (pageProp.Page.IsInit)
                {
                    pageProp.Page.OnApply();
                }
            }
        }

        private void Close()
        {
            if (ParentForm != null)
            {
                ParentForm.Close();
            }
        }

        #endregion

        public string ButtonOkText
        {
            get { return btnOK.Text; }
            set { btnOK.Text = value; }
        }

        public string ButtonApplyText
        {
            get { return btnApply.Text; }
            set { btnApply.Text = value; }
        }

        public string ButtonCancelText
        {
            get { return btnCancel.Text; }
            set { btnCancel.Text = value; }
        }
    }
}