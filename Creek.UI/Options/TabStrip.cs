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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

#endregion

namespace Creek.UI.Options
{

    #region TabStrip

    /// <summary>
    /// 
    /// </summary>
    internal sealed class TabStrip : Control
    {
        #region Declares

        private TabItemCollection m_Items;

        #endregion

        #region TabStrip

        public TabStrip(ProjectProperties control)
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint
                     | ControlStyles.DoubleBuffer
                     | ControlStyles.SupportsTransparentBackColor, true);

            m_Items = new TabItemCollection(control);

            BackColor = Color.Transparent;
        }

        #endregion

        #region ReAlignItems

        internal void ReAlignItems()
        {
            for (int i = 0; i < m_Items.Count; i++)
            {
                if (!Controls.Contains(m_Items[i]))
                {
                    Controls.Add(m_Items[i]);
                }

                if (i == 0)
                {
                    m_Items[i].Left = 0;
                    m_Items[i].Top = 6;
                    m_Items[i].SeparatorMode = TabItem.DrawSeparatorMode.DrawSeparatorTopBottom;
                }
                else
                {
                    m_Items[i].Left = 0;
                    m_Items[i].Top = m_Items[i - 1].Top + m_Items[i - 1].Height;
                }
            }
            if (m_Items.Count > 0)
                Size = new Size(m_Items[m_Items.Count - 1].Width, Height);
                    //m_Items[m_Items.Count - 1].Top + m_Items[m_Items.Count - 1].Height + 44);
        }

        #endregion

        #region Items

        /// <summary>
        /// Get/Set the TabItemCollection
        /// </summary>
        public TabItemCollection Items
        {
            get { return m_Items; }
            set
            {
                m_Items = value;
                ReAlignItems();
            }
        }

        #endregion

        #region OnPaint

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
//            e.Graphics.FillRectangle(TabBrushes.GetTabGradient(this.ClientRectangle), this.ClientRectangle);
            DrawBorder(e.Graphics);
        }

        #endregion

        #region Draw Methods

        private void DrawBorder(Graphics g)
        {
            DrawBorderTop(g);
            DrawBorderBottom(g);
        }

        private void DrawBorderTop(Graphics g)
        {
            Color border = Color.FromArgb(127, 157, 185);

            var gp = new GraphicsPath();

            gp.AddArc(new Rectangle(1, 1, 8, 8), 180, 90);

            gp.AddLine(5, 1, ClientRectangle.Width, 1);

            g.DrawPath(new Pen(border), gp);

            gp.Dispose();
        }

        private void DrawBorderBottom(Graphics g)
        {
            Color border = Color.FromArgb(127, 157, 185);

            int top = 10;

            if (m_Items.Count > 0)
                top = m_Items[m_Items.Count - 1].Top + m_Items[m_Items.Count - 1].Height;

            var gp = new GraphicsPath();

            gp.AddArc(new Rectangle(1, top - 4, 8, 8), 100, 80);

            // gp.CloseAllFigures();

            int newTop = top + 44;

            gp.AddLine(5, top + 4, ClientRectangle.Width - 8, newTop - 10);


            var tmp = new Rectangle(ClientRectangle.Width - 8 - 3, newTop - 10, 6, 6);


            gp.AddArc(tmp, 45, 18);

            gp.AddLine(ClientRectangle.Width - 8 + 2,
                       newTop - 10 + 3, ClientRectangle.Width - 8 + 2, ClientRectangle.Height);

            gp.AddLine(ClientRectangle.Width - 8 + 2, ClientRectangle.Height - 1,
                       ClientRectangle.Width, ClientRectangle.Height - 1);


            g.DrawPath(new Pen(border), gp);

            gp.Dispose();

            if (m_Items.Count > 0)
                top = m_Items[m_Items.Count - 1].Top + m_Items[m_Items.Count - 1].Height;

            g.DrawLine(new Pen(border), ClientRectangle.Width - 1,
                       top, ClientRectangle.Width - 1, ClientRectangle.Height - 5);
        }

        #endregion Draw Methods

        #region OnResize

        protected override void OnResize(EventArgs e)
        {
            ReAlignItems();
            base.OnResize(e);
            Invalidate();
        }

        #endregion
    }

    #endregion
}