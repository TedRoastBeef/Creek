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

using System.Drawing;
using System.Windows.Forms;

#endregion

namespace Creek.UI.Options
{

    #region TabPage

    /// <summary>
    /// 
    /// </summary>
    public class TabPage : Panel
    {
        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        public TabPage()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint
                     | ControlStyles.DoubleBuffer
                     | ControlStyles.SupportsTransparentBackColor
                     | ControlStyles.ResizeRedraw
                     | ControlStyles.StandardClick
                     | ControlStyles.ContainerControl, true);

            BackColor = Color.Transparent;
        }

        #endregion
    }

    #endregion
}