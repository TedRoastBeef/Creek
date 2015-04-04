/*
 * ImageListBox.cs
 * by Michael Damron
 * 
 * Last updated: Thursday, January 19, 2006
 * 
 * Known issues: Changing font size.
 * 
 */

using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Creek.UI
{
    /// <summary>
    /// Two column combobox for image and text
    /// </summary>
    public class ImageListBox : ListBox
    {
        private readonly DataSet m_dsData;
        private readonly ImageList m_lstImages;

        /// <summary>
        /// Construct new ImageListBox
        /// </summary>
        public ImageListBox()
        {
            DataTable dtData;

            m_dsData = new DataSet();
            m_lstImages = new ImageList();

            dtData = new DataTable("ImageListBox");
            dtData.Columns.Add("ImageRef");
            dtData.Columns.Add("Text");

            m_dsData.Tables.Add(dtData);

            // Setup owner draw code

            DrawMode = DrawMode.OwnerDrawFixed;
            DrawItem += OnDrawItem;


            SelectedIndexChanged += OnSelectedIndexChanged;
            //this.DropDown += new System.EventHandler( OnDropDown );						
        }

        /// <summary>
        /// Add Image (using File Path) and text item.
        /// </summary>
        public void Add(string strPath, string strText)
        {
            Image imgTemp;
            DataRow drData;
            string strKey;

            strKey = Path.GetFileName(strPath);
            imgTemp = Image.FromFile(strPath);
            m_lstImages.Images.Add(strKey, imgTemp);

            // add to DataSet
            drData = m_dsData.Tables[0].NewRow();
            drData[0] = strKey;
            drData[1] = strText;
            m_dsData.Tables[0].Rows.Add(drData);

            Items.Add(strText);
        }

        /// <summary>
        /// Add Image to listbox.
        /// </summary>
        /// <param name="objImage"></param>
        /// <param name="strKey"></param>
        /// <param name="strText"></param>
        public void Add(Image objImage, string strKey, string strText)
        {
            DataRow drData;

            m_lstImages.Images.Add(strKey, objImage);

            // add to DataSet
            drData = m_dsData.Tables[0].NewRow();
            drData[0] = strKey;
            drData[1] = strText;
            m_dsData.Tables[0].Rows.Add(drData);

            Items.Add(strText);
        }

        /// <summary>
        /// Remove item at index.
        /// </summary>
        /// <param name="Index"></param>
        public void RemoveAt(int Index)
        {
            if (m_dsData.Tables[0].Rows.Count > 0 && Index >= 0)
            {
                m_dsData.Tables[0].Rows.RemoveAt(Index);
                m_lstImages.Images.RemoveAt(Index);
                Items.RemoveAt(Index);
                //SelectedIndex = 0;
                Refresh();
            }
        }

        /// <summary>
        /// Remove all items.
        /// </summary>
        public void RemoveAll()
        {
            m_dsData.Clear();
            foreach (string strKey in m_lstImages.Images.Keys)
                m_lstImages.Images.RemoveByKey(strKey);
            Items.Clear();
            Refresh();
        }

        /// <summary>
        /// Get Image at index.
        /// </summary>
        /// <param name="Index"></param>
        /// <returns></returns>
        public Image GetImage(int Index)
        {
            if (m_lstImages.Images.Count == 0 || Index == -1)
                return null;

            return m_lstImages.Images[Index];
        }

        public string GetKey(int Index)
        {
            if (m_lstImages.Images.Count == 0 || Index == -1)
                return null;

            return m_dsData.Tables[0].Rows[Index][0].ToString();
        }

        /// <summary>
        /// Marked for deletion.
        /// </summary>
        /// <param name="Index"></param>
        /// <returns></returns>
        public DataTable GetSelectedData(int Index)
        {
            DataTable dtTemp;
            DataRow drTemp;

            if (m_lstImages.Images.Count == 0 || Index == -1)
                return null;

            dtTemp = m_dsData.Tables[0].Clone();
            drTemp = m_dsData.Tables[0].Rows[Index];
            dtTemp.Rows.Add(drTemp.ItemArray);

            return dtTemp;
        }

        /// <summary>
        /// Get selected items in a data table.
        /// </summary>
        /// <returns></returns>
        public DataTable GetSelectedData()
        {
            DataTable dtTemp;
            DataRow drTemp;

            if (m_lstImages.Images.Count == 0 || SelectedIndices.Count == 0)
                return null;

            dtTemp = m_dsData.Tables[0].Clone();
            foreach (int iPos in SelectedIndices)
            {
                drTemp = m_dsData.Tables[0].Rows[iPos];
                dtTemp.Rows.Add(drTemp.ItemArray);
            }

            return dtTemp;
        }

        public ImageList GetSelectedImages(int Index)
        {
            ImageList lstTemp;

            if (m_lstImages.Images.Count == 0 || Index == -1)
                return null;

            lstTemp = new ImageList();

            lstTemp.Images.Add(m_lstImages.Images[Index]);
            return lstTemp;
        }

        public ImageList GetSelectedImages()
        {
            ImageList lstTemp;

            if (m_lstImages.Images.Count == 0 || SelectedIndices.Count == 0)
                return null;

            lstTemp = new ImageList();

            foreach (int iPos in SelectedIndices)
            {
                lstTemp.Images.Add(m_lstImages.Images[iPos]);
            }

            return lstTemp;
        }

        private void OnDrawItem(object sender, DrawItemEventArgs e)
        {
            Graphics grfx = e.Graphics;
            Rectangle rect;
            string strKey;
            int iWidth;
            //Image imgTemp;

            if (m_lstImages.Images.Count > 0 && e.Index < m_lstImages.Images.Count)
            {
                e.DrawBackground();
                e.DrawFocusRectangle();

                rect = e.Bounds;
                iWidth = rect.Width;
                rect.X += 5;
                rect.Y += 1;
                rect.Height -= 1;
                rect.Width = rect.Height;
                strKey = m_dsData.Tables[0].Rows[e.Index][0].ToString();
                grfx.DrawImage(m_lstImages.Images[strKey], rect);
                rect.Offset(15, 0);
                rect.Width = iWidth;
                grfx.DrawString(m_dsData.Tables[0].Rows[e.Index][1].ToString(), e.Font, Brushes.Black, rect);
            }
        }

        private void OnSelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            // 
            // ImageListBox
            // 
            MouseHover += ImageListBox_MouseHover;
            MouseMove += ImageListBox_MouseMove;
            ResumeLayout(false);
        }

        private void ImageListBox_MouseHover(object sender, EventArgs e)
        {
        }

        private void ImageListBox_MouseMove(object sender, MouseEventArgs e)
        {
            //e.Location.ToString();
        }
    }
}