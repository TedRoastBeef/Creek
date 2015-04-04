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
using System.Windows.Forms;

#endregion

namespace Creek.UI.Options
{

    #region PropertyControl

    /// <summary>
    /// 
    /// </summary>
    public sealed class ProjectProperties : ScrollableControl
    {
        #region Declares

        private readonly TabStrip m_Strip;
        private readonly TabPageContainer m_TabPageContainer;

        #endregion

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        public ProjectProperties()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint
                     | ControlStyles.DoubleBuffer
                     | ControlStyles.Selectable | ControlStyles.UserMouse, true);

            m_Strip = new TabStrip(this);

            Controls.Add(m_Strip);

            m_TabPageContainer = new TabPageContainer();

            Controls.Add(m_TabPageContainer);


            AutoScroll = true;
        }

        #endregion Constructo

        #region OnPaint

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.FillRectangle(TabBrushes.GetPropertyControlGradient(Screen.PrimaryScreen.Bounds), ClientRectangle);
        }

        #endregion

        #region OnResize

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            m_Strip.Left = 20;
            m_Strip.Top = 10;

            m_Strip.Height = ClientRectangle.Height - 20;

            if (m_Strip.Height <= 300)
                m_Strip.Height = 300;


            m_TabPageContainer.Left = m_Strip.Left + m_Strip.Width;
            m_TabPageContainer.Height = m_Strip.Height;
            m_TabPageContainer.Top = m_Strip.Top;
            m_TabPageContainer.Width = ClientRectangle.Width - 5 - m_Strip.Left - m_Strip.Width;

            Invalidate();
        }

        #endregion

        #region AddCurrentTabPage

        internal void AddCurrentTabPage(Control control)
        {
            m_TabPageContainer.AddCurrentControl(control);
            base.OnResize(new EventArgs());
        }

        #endregion

        #region TabStrip

        internal TabStrip TabStrip
        {
            get { return m_Strip; }
        }

        #endregion

        #region TabItems

        /// <summary>
        /// Return The collection of TabItem for this PropertyControl
        /// </summary>
        /// <value>Return a TabItemCollection</value>
        public TabItemCollection TabItems
        {
            get { return m_Strip.Items; }
        }

        #endregion
    }

    #endregion
}