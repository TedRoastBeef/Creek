using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Creek.UI.Unity3.Controls
{
    public partial class DropDownControl : UserControl
    {
        #region eDockSide enum

        public enum eDockSide
        {
            Left,
            Right
        }

        #endregion

        #region eDropState enum

        public enum eDropState
        {
            Closed,
            Closing,
            Dropping,
            Dropped
        }

        #endregion

        private Rectangle _AnchorClientBounds;
        private Size _AnchorSize = new Size(121, 21);
        private bool _DesignView = true;
        private eDockSide _DockSide;
        private string _Text;

        private Control _dropDownItem;
        private eDropState _dropState;
        private bool closedWhileInControl;
        private DropDownContainer dropContainer;
        protected bool mousePressed;
        private Size storedSize;

        public DropDownControl()
        {
            InitializeComponent();
            storedSize = Size;
            BackColor = Color.White;
            Text = Name;
        }

        protected eDropState DropState
        {
            get { return _dropState; }
        }

        public new string Text
        {
            get { return _Text; }
            set
            {
                _Text = value;
                Invalidate();
            }
        }

        public Size AnchorSize
        {
            get { return _AnchorSize; }
            set
            {
                _AnchorSize = value;
                Invalidate();
            }
        }

        public eDockSide DockSide
        {
            get { return _DockSide; }
            set { _DockSide = value; }
        }


        [DefaultValue(false)]
        protected bool DesignView
        {
            get { return _DesignView; }
            set
            {
                if (_DesignView == value) return;

                _DesignView = value;
                if (_DesignView)
                {
                    Size = storedSize;
                }
                else
                {
                    storedSize = Size;
                    Size = _AnchorSize;
                }
            }
        }

        public Rectangle AnchorClientBounds
        {
            get { return _AnchorClientBounds; }
        }

        protected virtual bool CanDrop
        {
            get
            {
                if (dropContainer != null)
                    return false;

                if (dropContainer == null && closedWhileInControl)
                {
                    closedWhileInControl = false;
                    return false;
                }

                return !closedWhileInControl;
            }
        }

        public void InitializeDropDown(Control dropDownItem)
        {
            if (_dropDownItem != null)
                throw new Exception("The drop down item has already been implemented!");
            _DesignView = false;
            _dropState = eDropState.Closed;
            Size = _AnchorSize;
            _AnchorClientBounds = new Rectangle(2, 2, _AnchorSize.Width - 21, _AnchorSize.Height - 4);
            //removes the dropDown item from the controls list so it 
            //won't be seen until the drop-down window is active
            if (Controls.Contains(dropDownItem))
                Controls.Remove(dropDownItem);
            _dropDownItem = dropDownItem;
        }

        public event EventHandler PropertyChanged;

        protected void OnPropertyChanged()
        {
            if (PropertyChanged != null)
                PropertyChanged(null, null);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (_DesignView)
                storedSize = Size;
            _AnchorSize.Width = Width;
            if (!_DesignView)
            {
                _AnchorSize.Height = Height;
                _AnchorClientBounds = new Rectangle(2, 2, _AnchorSize.Width - 21, _AnchorSize.Height - 4);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            mousePressed = true;
            OpenDropDown();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            mousePressed = false;
            Invalidate();
        }

        protected void OpenDropDown()
        {
            if (_dropDownItem == null)
                throw new NotImplementedException(
                    "The drop down item has not been initialized!  Use the InitializeDropDown() method to do so.");

            if (!CanDrop) return;

            dropContainer = new DropDownContainer(_dropDownItem);
            dropContainer.Bounds = GetDropDownBounds();
            dropContainer.DropStateChange += dropContainer_DropStateChange;
            dropContainer.FormClosed += dropContainer_Closed;
            ParentForm.Move += ParentForm_Move;
            _dropState = eDropState.Dropping;
            dropContainer.Show(this);
            _dropState = eDropState.Dropped;
            Invalidate();
        }

        private void ParentForm_Move(object sender, EventArgs e)
        {
            dropContainer.Bounds = GetDropDownBounds();
        }


        public void CloseDropDown()
        {
            if (dropContainer != null)
            {
                _dropState = eDropState.Closing;
                dropContainer.Freeze = false;
                dropContainer.Close();
            }
        }

        private void dropContainer_DropStateChange(eDropState state)
        {
            _dropState = state;
        }

        private void dropContainer_Closed(object sender, FormClosedEventArgs e)
        {
            if (!dropContainer.IsDisposed)
            {
                dropContainer.DropStateChange -= dropContainer_DropStateChange;
                dropContainer.FormClosed -= dropContainer_Closed;
                ParentForm.Move -= ParentForm_Move;
                dropContainer.Dispose();
            }
            dropContainer = null;
            closedWhileInControl = (RectangleToScreen(ClientRectangle).Contains(Cursor.Position));
            _dropState = eDropState.Closed;
            Invalidate();
        }

        protected virtual Rectangle GetDropDownBounds()
        {
            var inflatedDropSize = new Size(_dropDownItem.Width + 2, _dropDownItem.Height + 2);
            Rectangle screenBounds = _DockSide == eDockSide.Left
                                         ? new Rectangle(Parent.PointToScreen(new Point(Bounds.X, Bounds.Bottom)),
                                                         inflatedDropSize)
                                         : new Rectangle(
                                               Parent.PointToScreen(new Point(Bounds.Right - _dropDownItem.Width,
                                                                              Bounds.Bottom)), inflatedDropSize);
            Rectangle workingArea = Screen.GetWorkingArea(screenBounds);
            //make sure we're completely in the top-left working area
            if (screenBounds.X < workingArea.X) screenBounds.X = workingArea.X;
            if (screenBounds.Y < workingArea.Y) screenBounds.Y = workingArea.Y;

            //make sure we're not extended past the working area's right /bottom edge
            if (screenBounds.Right > workingArea.Right && workingArea.Width > screenBounds.Width)
                screenBounds.X = workingArea.Right - screenBounds.Width;
            if (screenBounds.Bottom > workingArea.Bottom && workingArea.Height > screenBounds.Height)
                screenBounds.Y = workingArea.Bottom - screenBounds.Height;

            return screenBounds;
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            //Check if VisualStyles are supported...
            //Thanks to codeproject member: Mathiyazhagan for catching :)
            if (ComboBoxRenderer.IsSupported)
            {
                ComboBoxRenderer.DrawTextBox(e.Graphics, new Rectangle(new Point(0, 0), _AnchorSize), getState());
                ComboBoxRenderer.DrawDropDownButton(e.Graphics,
                                                    new Rectangle(_AnchorSize.Width - 19, 2, 18, _AnchorSize.Height - 4),
                                                    getState());
            }
            else
            {
                ControlPaint.DrawComboButton(e.Graphics, new Rectangle(
                                                             _AnchorSize.Width - 19, 2, 18, _AnchorSize.Height - 4),
                                             (Enabled) ? ButtonState.Normal : ButtonState.Inactive);
            }

            using (Brush b = new SolidBrush(BackColor))
            {
                e.Graphics.FillRectangle(b, AnchorClientBounds);
            }

            TextRenderer.DrawText(e.Graphics, _Text, Font, AnchorClientBounds, ForeColor, TextFormatFlags.WordEllipsis);
        }

        private ComboBoxState getState()
        {
            if (mousePressed || dropContainer != null)
                return ComboBoxState.Pressed;
            else
                return ComboBoxState.Normal;
        }

        public void FreezeDropDown(bool remainVisible)
        {
            if (dropContainer != null)
            {
                dropContainer.Freeze = true;
                if (!remainVisible)
                    dropContainer.Visible = false;
            }
        }

        public void UnFreezeDropDown()
        {
            if (dropContainer != null)
            {
                dropContainer.Freeze = false;
                if (!dropContainer.Visible)
                    dropContainer.Visible = true;
            }
        }

        #region Nested type: DropDownContainer

        internal sealed class DropDownContainer : Form, IMessageFilter
        {
            #region Delegates

            public delegate void DropWindowArgs(eDropState state);

            #endregion

            public bool Freeze;


            public DropDownContainer(Control dropDownItem)
            {
                FormBorderStyle = FormBorderStyle.None;
                dropDownItem.Location = new Point(1, 1);
                Controls.Add(dropDownItem);
                StartPosition = FormStartPosition.Manual;
                ShowInTaskbar = false;
                Application.AddMessageFilter(this);
            }

            #region IMessageFilter Members

            public bool PreFilterMessage(ref Message m)
            {
                if (!Freeze && Visible && (ActiveForm == null || !ActiveForm.Equals(this)))
                {
                    OnDropStateChange(eDropState.Closing);
                    Close();
                }


                return false;
            }

            #endregion

            public event DropWindowArgs DropStateChange;

            public void OnDropStateChange(eDropState state)
            {
                if (DropStateChange != null)
                    DropStateChange(state);
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);
                e.Graphics.DrawRectangle(Pens.Gray, new Rectangle(0, 0, ClientSize.Width - 1, ClientSize.Height - 1));
            }

            protected override void OnClosing(CancelEventArgs e)
            {
                Application.RemoveMessageFilter(this);
                Controls.RemoveAt(0); //prevent the control from being disposed
                base.OnClosing(e);
            }
        }

        #endregion
    }
}