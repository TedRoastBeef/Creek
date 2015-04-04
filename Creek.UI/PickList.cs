/*
 * PickList.cs
 * by Michael Damron
 * 
 * Last updated: Thursday, January 19, 2006
 * 
 */

using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Creek.UI
{
    public partial class PickList : UserControl
    {
        #region Delegates

        public delegate void ItemsMovedHandler(ImageList lstImages, DataTable dtData);

        #endregion

        public PickList()
        {
            InitializeComponent();
        }

        public string SuggestedCaption
        {
            get { return lblSuggested.Text; }
            set { lblSuggested.Text = value; }
        }

        public string AvailableCaption
        {
            get { return lblAvailable.Text; }
            set { lblAvailable.Text = value; }
        }

        [Category("Action")]
        [Description("Notifies user items moved to the right.")]
        public event ItemsMovedHandler ItemsMovedRight; //(ImageList lstImages, DataTable dtData);

        [Category("Action")]
        [Description("Notifies user items moved to the left.")]
        public event ItemsMovedHandler ItemsMovedLeft; //ImageList lstImages, DataTable dtData);

        public virtual void OnItemsMovedRight(ImageList lstImages, DataTable dtData)
        {
            if (ItemsMovedRight != null)
            {
                ItemsMovedRight(lstImages, dtData);
            }
        }

        public virtual void OnItemsMovedLeft(ImageList lstImages, DataTable dtData)
        {
            if (ItemsMovedLeft != null)
            {
                ItemsMovedLeft(lstImages, dtData);
            }
        }

        private void PickList_Load(object sender, EventArgs e)
        {
        }

        private void cmdMoveRight_Click(object sender, EventArgs e)
        {
            MoveRight(imageListBoxA.SelectedIndex);
        }

        private void cmdMoveLeft_Click(object sender, EventArgs e)
        {
            MoveLeft(imageListBoxS.SelectedIndex);
        }

        private void MoveRight(int Index)
        {
            Image imgTemp;
            string strText, strKey;
            ImageList lstImages;
            DataTable dtTable;
            int iCount;

            if (Index >= 0)
            {
                lstImages = imageListBoxA.GetSelectedImages();
                dtTable = imageListBoxA.GetSelectedData();

                foreach (int iPos in imageListBoxA.SelectedIndices)
                {
                    imgTemp = imageListBoxA.GetImage(iPos);
                    //strText = imageListBoxA.SelectedItem.ToString();
                    strText = imageListBoxA.Items[iPos].ToString();
                    strKey = imageListBoxA.GetKey(iPos);
                    //imageListBoxA.RemoveAt(iPos);
                    imageListBoxS.Add(imgTemp, strKey, strText);
                }

                iCount = imageListBoxA.SelectedIndices.Count;

                for (int iRemovePos = iCount; iRemovePos > 0; iRemovePos--)
                {
                    imageListBoxA.RemoveAt(imageListBoxA.SelectedIndices[iRemovePos - 1]);
                }

                UpdateUI();
                OnItemsMovedRight(lstImages, dtTable);
            }
        }

        private void MoveLeft(int Index)
        {
            Image imgTemp;
            string strText, strKey;
            ImageList lstImages;
            DataTable dtTable;
            int iCount;

            if (Index >= 0)
            {
                lstImages = imageListBoxS.GetSelectedImages();
                dtTable = imageListBoxS.GetSelectedData();

                foreach (int iPos in imageListBoxS.SelectedIndices)
                {
                    imgTemp = imageListBoxS.GetImage(iPos);
                    //strText = imageListBoxS.SelectedItem.ToString();
                    strText = imageListBoxS.Items[iPos].ToString();
                    strKey = imageListBoxS.GetKey(iPos);
                    //imageListBoxS.RemoveAt(Index);
                    imageListBoxA.Add(imgTemp, strKey, strText);
                }

                iCount = imageListBoxS.SelectedIndices.Count;

                for (int iRemovePos = iCount; iRemovePos > 0; iRemovePos--)
                    imageListBoxS.RemoveAt(imageListBoxS.SelectedIndices[iRemovePos - 1]);

                UpdateUI();
                OnItemsMovedLeft(lstImages, dtTable);
            }
        }

        public void AddSuggestedItem(Image imgItem, string strKey, string strItem)
        {
            imageListBoxS.Add(imgItem, strKey, strItem);
            UpdateUI();
        }

        public void AddAvailableItem(Image imgItem, string strKey, string strItem)
        {
            imageListBoxA.Add(imgItem, strKey, strItem);
            UpdateUI();
        }

        private void imageListBoxA_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            MoveRight(imageListBoxA.SelectedIndex);
        }

        private void imageListBoxS_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            MoveLeft(imageListBoxS.SelectedIndex);
        }

        private void imageListBoxA_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateUI();
        }

        private void imageListBoxS_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateUI();
        }

        private void imageListBoxS_MouseClick(object sender, MouseEventArgs e)
        {
            imageListBoxA.ClearSelected();
        }

        private void imageListBoxA_MouseClick(object sender, MouseEventArgs e)
        {
            imageListBoxS.ClearSelected();
        }

        private void cmdMoveAllRight_Click(object sender, EventArgs e)
        {
            for (int iPos = 0; iPos < imageListBoxA.Items.Count; iPos++)
                imageListBoxA.SelectedIndices.Add(iPos);

            MoveRight(1);
        }

        private void cmdMoveAllLeft_Click(object sender, EventArgs e)
        {
            for (int iPos = 0; iPos < imageListBoxS.Items.Count; iPos++)
                imageListBoxS.SelectedIndices.Add(iPos);

            MoveLeft(1);
        }

        private void UpdateUI()
        {
            cmdMoveAllLeft.Enabled = imageListBoxS.Items.Count > 0;
            cmdMoveLeft.Enabled = imageListBoxS.Items.Count > 0 && imageListBoxS.SelectedIndex >= 0;
            cmdMoveRight.Enabled = imageListBoxA.Items.Count > 0 && imageListBoxA.SelectedIndex >= 0;
            cmdMoveAllRight.Enabled = imageListBoxA.Items.Count > 0;
        }
    }
}