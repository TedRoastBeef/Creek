// one line to give the program's name and an idea of what it does.
// Copyright (C) 2004  Sebastian Faltoni creator of dotnetfireball.org
// 
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 2
// of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.

#region Using directives

using System;
using System.Collections;

#endregion

namespace Creek.UI.Options
{

    #region TabItemCollection

    /// <summary>
    /// The collection of TabItem
    /// </summary>
    public sealed class TabItemCollection
    {
        #region Declares

        private readonly ArrayList m_Items;
        private readonly ProjectProperties m_Parent;

        #endregion

        #region TabItemCollection

        /// <summary>
        /// TabItemCollection Constructor
        /// </summary>
        /// <param name="parent"></param>
        public TabItemCollection(ProjectProperties parent)
        {
            m_Items = new ArrayList();
            m_Parent = parent;
        }

        #endregion

        /// <summary>
        /// Get/Set the TabItem with index
        /// </summary>
        /// <param name="index"></param>
        /// <returns>The request TabItem</returns>
        public TabItem this[int index]
        {
            get { return (TabItem) m_Items[index]; }

            set { m_Items[index] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public int Count
        {
            get { return m_Items.Count; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Get the index on the collection the specified TabItem
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int IndexOf(TabItem item)
        {
            return m_Items.IndexOf(item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        public void Insert(int index, TabItem item)
        {
            m_Items.Insert(index, item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index)
        {
            m_Items.RemoveAt(index);
        }

        /// <summary>
        /// Add a TabItem object to the collection
        /// </summary>
        /// <param name="item"></param>
        public void Add(TabItem item)
        {
            item.Click += item_Click;
            m_Items.Add(item);
            m_Parent.TabStrip.ReAlignItems();
        }

        /// <summary>
        /// Remove all Item from the collection
        /// </summary>
        public void Clear()
        {
            m_Items.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(TabItem item)
        {
            return m_Items.Contains(item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(TabItem[] array, int arrayIndex)
        {
            m_Items.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public void Remove(TabItem item)
        {
            m_Items.Remove(item);
            ;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()
        {
            return m_Items.GetEnumerator();
        }

        #region item_Click

        private void item_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < m_Items.Count; i++)
            {
                if (!(m_Items[i].Equals(sender)))
                {
                    ((TabItem) m_Items[i]).UnSelect();
                }
            }

            if (((TabItem) sender).TabPageControl != null)
                m_Parent.AddCurrentTabPage(((TabItem) sender).TabPageControl);
        }

        #endregion
    }

    #endregion
}