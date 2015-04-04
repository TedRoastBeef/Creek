using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace Creek.UI.Navigation
{
    using Creek.UI.Winforms.Properties;

    #region TravelButton

    [DefaultEvent("ItemClicked")]
    public partial class TravelButton : Control
    {
        #region Fields

        private readonly Rectangle BACKGROUND_RECT = new Rectangle(0, 0, 74, 29);
        private readonly Rectangle BACK_BUTTON_RECT = new Rectangle(2, 2, 25, 25);
        private readonly Rectangle DROPDOWN_ARROW_RECT = new Rectangle(57, 4, 17, 20);
        private readonly Rectangle FORWARD_BUTTON_RECT = new Rectangle(30, 2, 25, 25);
        private bool _bButtonEnabled = true;
        private bool _bButtonOnly;
        private ControlState _bButtonState = ControlState.Normal;
        private string _bToolTip;

        private Image _backButton;
        private Image _backGround;

        private ControlState _ddArrowState = ControlState.Normal;
        private ContextMenuStrip _dropDownMenu;
        private bool _fButtonEnabled = true;
        private ControlState _fButtonState = ControlState.Normal;

        private string _fToolTip;
        private Image _forwardButton;
        private bool _showMenu = true;

        public Image ForwardButton
        {
            get { return _forwardButton; }
            set { _forwardButton = value; }
        }

        public Image BackGround
        {
            get { return _backGround; }
            set { _backGround = value; }
        }

        public Image BackwardButton
        {
            get { return _backButton; }
            set { _backButton = value; }
        }

        [Browsable(true)]
        public event TravelButtonItemClickedEventHandler ItemClicked;

        [Browsable(false)]
        public event EventHandler DropDownMenuChanged;

        [Browsable(false)]
        public event PaintEventHandler PaintBackground;

        private enum ControlState
        {
            Normal,
            Hover,
            Pressed,
            Disabled
        }

        #endregion

        #region Constructors

        public TravelButton()
        {
            Assembly assembly = Assembly.GetAssembly(typeof (TravelButton));

            _backGround = Resources.BACKGROUND;
            _backButton = Resources.LEFTBUTTON;
            _forwardButton = Resources.RIGHTBUTTON;

            DoubleBuffered = true;

            InitializeComponent();
        }

        #endregion

        #region Properties

        [Category("Behavior"), Browsable(true), DefaultValue(false)]
        public bool BackButtonOnly
        {
            get { return _bButtonOnly; }
            set
            {
                _bButtonOnly = value;
                if (_bButtonOnly)
                    _fButtonEnabled = false;

                Invalidate();
            }
        }

        [Category("Behavior"), Browsable(true), DefaultValue(true)]
        public bool BackEnabled
        {
            get { return _bButtonEnabled; }
            set
            {
                _bButtonEnabled = value;
                Invalidate();
            }
        }

        [Category("Behavior"), Browsable(true), DefaultValue(true)]
        public bool ForwardEnabled
        {
            get { return (!_bButtonOnly && _fButtonEnabled); }
            set
            {
                _fButtonEnabled = (!_bButtonOnly && value);
                Invalidate();
            }
        }

        private bool DDArrowEnabled
        {
            get
            {
                return (!_bButtonOnly && (BackEnabled || ForwardEnabled) && _dropDownMenu != null &&
                        _dropDownMenu.Items.Count != 0);
            }
        }

        [Browsable(true)]
        public string BackToolTip
        {
            get { return _bToolTip; }
            set { _bToolTip = value; }
        }

        [Browsable(true)]
        public string ForwardToolTip
        {
            get { return _fToolTip; }
            set { _fToolTip = value; }
        }

        [Category("Behavior"), Browsable(true), DefaultValue(null)]
        public ContextMenuStrip DropDownMenu
        {
            get { return _dropDownMenu; }
            set
            {
                if (!DesignMode && _dropDownMenu != null)
                    _dropDownMenu.VisibleChanged -= DropDownMenu_VisibleChanged;

                _dropDownMenu = value;

                if (!DesignMode && _dropDownMenu != null)
                    _dropDownMenu.VisibleChanged += DropDownMenu_VisibleChanged;

                if (DropDownMenuChanged != null)
                    DropDownMenuChanged(this, EventArgs.Empty);
            }
        }

        #endregion

        #region Override

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            Invalidate();
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            if (PaintBackground != null)
                PaintBackground(this, pevent);
            else
                base.OnPaintBackground(pevent);
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            if (Enabled)
            {
                if (!BackButtonOnly)
                {
                    if (DDArrowEnabled)
                        DrawItem(pe.Graphics, TravelButtonItem.BackGround, _ddArrowState);
                    else
                        DrawItem(pe.Graphics, TravelButtonItem.BackGround, ControlState.Disabled);
                }
                else
                {
                    DrawItem(pe.Graphics, TravelButtonItem.BackGround, ControlState.Disabled);
                }

                if (BackEnabled)
                    DrawItem(pe.Graphics, TravelButtonItem.BackButton, _bButtonState);
                else
                    DrawItem(pe.Graphics, TravelButtonItem.BackButton, ControlState.Disabled);

                if (!BackButtonOnly)
                {
                    if (ForwardEnabled)
                        DrawItem(pe.Graphics, TravelButtonItem.ForwardButton, _fButtonState);
                    else
                        DrawItem(pe.Graphics, TravelButtonItem.ForwardButton, ControlState.Disabled);
                }
            }
            else
            {
                DrawItem(pe.Graphics, TravelButtonItem.BackGround, ControlState.Disabled);
                DrawItem(pe.Graphics, TravelButtonItem.BackButton, ControlState.Disabled);

                if (!BackButtonOnly)
                {
                    DrawItem(pe.Graphics, TravelButtonItem.ForwardButton, ControlState.Disabled);
                }
            }

            base.OnPaint(pe);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            _bButtonState = ControlState.Normal;
            _fButtonState = ControlState.Normal;

            if (_dropDownMenu != null && _dropDownMenu.Visible)
                _ddArrowState = ControlState.Pressed;
            else
                _ddArrowState = ControlState.Normal;

            Invalidate();

            _showMenu = true;
        }

        protected override void OnMouseDown(MouseEventArgs me)
        {
            base.OnMouseDown(me);

            if (Enabled)
            {
                if (me.Button == MouseButtons.Left && me.Clicks == 1)
                {
                    if (BackEnabled && BACK_BUTTON_RECT.Contains(me.X, me.Y))
                    {
                        _bButtonState = ControlState.Pressed;
                        _fButtonState = ControlState.Normal;
                        _ddArrowState = ControlState.Normal;
                        _showMenu = true;
                    }
                    else if (!BackButtonOnly && ForwardEnabled && FORWARD_BUTTON_RECT.Contains(me.X, me.Y))
                    {
                        _bButtonState = ControlState.Normal;
                        _fButtonState = ControlState.Pressed;
                        _ddArrowState = ControlState.Normal;
                        _showMenu = true;
                    }
                    else if (!BackButtonOnly && DDArrowEnabled && DROPDOWN_ARROW_RECT.Contains(me.X, me.Y))
                    {
                        _bButtonState = ControlState.Normal;
                        _fButtonState = ControlState.Normal;

                        if (_dropDownMenu != null)
                        {
                            if (_showMenu)
                            {
                                _dropDownMenu.Show(this, new Point(BACKGROUND_RECT.X, BACKGROUND_RECT.Height),
                                                   ToolStripDropDownDirection.Default);
                                _ddArrowState = ControlState.Pressed;
                            }
                            else
                            {
                                _ddArrowState = ControlState.Hover;
                            }
                            _showMenu = !_showMenu;
                        }
                        else
                        {
                            _ddArrowState = ControlState.Hover;
                            _showMenu = true;
                        }
                    }
                    else
                    {
                        _bButtonState = ControlState.Normal;
                        _fButtonState = ControlState.Normal;
                        _ddArrowState = ControlState.Normal;
                        _showMenu = true;
                    }

                    Invalidate();
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs me)
        {
            base.OnMouseMove(me);

            if (Enabled && !Capture)
            {
                if (BackEnabled && BACK_BUTTON_RECT.Contains(me.X, me.Y))
                {
                    _bButtonState = ControlState.Hover;
                    _fButtonState = ControlState.Normal;
                    _ddArrowState = ControlState.Normal;

                    SetToolTip(TravelButtonItem.BackButton);
                }
                else if (!BackButtonOnly && ForwardEnabled && FORWARD_BUTTON_RECT.Contains(me.X, me.Y))
                {
                    _bButtonState = ControlState.Normal;
                    _fButtonState = ControlState.Hover;
                    _ddArrowState = ControlState.Normal;

                    SetToolTip(TravelButtonItem.ForwardButton);
                }
                else if (!BackButtonOnly && DDArrowEnabled && DROPDOWN_ARROW_RECT.Contains(me.X, me.Y))
                {
                    _bButtonState = ControlState.Normal;
                    _fButtonState = ControlState.Normal;
                    _ddArrowState = ControlState.Hover;

                    SetToolTip(TravelButtonItem.DropDownArrow);
                }
                else
                {
                    _bButtonState = ControlState.Normal;
                    _fButtonState = ControlState.Normal;
                    _ddArrowState = ControlState.Normal;

                    SetToolTip(TravelButtonItem.BackGround);
                }

                Invalidate();
            }
        }

        protected override void OnMouseDoubleClick(MouseEventArgs me)
        {
            if (DROPDOWN_ARROW_RECT.Contains(me.X, me.Y))
                return;
            else
                base.OnMouseDoubleClick(me);
        }

        protected override void OnClick(EventArgs e)
        {
            if (Enabled)
            {
                Point p = PointToClient(MousePosition);
                var item = TravelButtonItem.BackGround;

                if (BackEnabled && BACK_BUTTON_RECT.Contains(p.X, p.Y))
                {
                    item = TravelButtonItem.BackButton;
                    _bButtonState = ControlState.Hover;
                }
                else if (!BackButtonOnly && ForwardEnabled && FORWARD_BUTTON_RECT.Contains(p.X, p.Y))
                {
                    item = TravelButtonItem.ForwardButton;
                    _fButtonState = ControlState.Hover;
                }
                else if (!BackButtonOnly && DDArrowEnabled && DROPDOWN_ARROW_RECT.Contains(p.X, p.Y))
                {
                    item = TravelButtonItem.DropDownArrow;
                    //_dropDownArrowState = ControlState.Hover;
                }

                Invalidate();

                if (item != TravelButtonItem.BackGround && ItemClicked != null)
                    ItemClicked(this, new TravelButtonItemClickedEventArgs(item));
            }

            base.OnClick(e);
        }

        #endregion

        #region Pulbic

        public void SetButtonEnabled(TravelButtonItem item, bool enable)
        {
            if (item == TravelButtonItem.BackButton)
                BackEnabled = enable;
            else if (item == TravelButtonItem.ForwardButton)
                ForwardEnabled = enable;
        }

        public void SetButtonToolTip(TravelButtonItem item, string tip)
        {
            if (item == TravelButtonItem.BackButton)
                BackToolTip = tip;
            else if (item == TravelButtonItem.ForwardButton)
                ForwardToolTip = tip;
        }

        #endregion

        #region Private

        private void SetToolTip(TravelButtonItem item)
        {
            string curToolTip = toolTip.GetToolTip(this);

            if (item == TravelButtonItem.BackButton)
            {
                if (curToolTip != _bToolTip)
                {
                    toolTip.SetToolTip(this, _bToolTip);
                }
            }
            else if (item == TravelButtonItem.ForwardButton)
            {
                if (curToolTip != _fToolTip)
                {
                    toolTip.SetToolTip(this, _fToolTip);
                }
            }
            else
            {
                toolTip.Hide(this);
            }
        }

        private void DropDownMenu_VisibleChanged(object sender, EventArgs e)
        {
            var menu = sender as ContextMenuStrip;
            if (menu != null)
            {
                if (!menu.Visible)
                {
                    _ddArrowState = ControlState.Normal;
                }

                Invalidate();
            }
        }

        private void DrawItem(Graphics g, TravelButtonItem item, ControlState state)
        {
            Rectangle srcRect;
            switch (item)
            {
                case TravelButtonItem.BackButton:
                    srcRect = new Rectangle(new Point(0, 0), BACK_BUTTON_RECT.Size);
                    break;
                case TravelButtonItem.ForwardButton:
                    srcRect = new Rectangle(new Point(0, 0), FORWARD_BUTTON_RECT.Size);
                    break;
                default:
                    srcRect = BACKGROUND_RECT;
                    break;
            }

            int xOffset = 0;
            switch (state)
            {
                case ControlState.Normal:
                    xOffset = 0;
                    break;
                case ControlState.Pressed:
                    xOffset = srcRect.Width;
                    break;
                case ControlState.Hover:
                    xOffset = srcRect.Width*2;
                    break;
                case ControlState.Disabled:
                    xOffset = srcRect.Width*3;
                    break;
            }

            srcRect.X = xOffset;

            if (item == TravelButtonItem.BackButton)
            {
                g.DrawImage(_backButton, BACK_BUTTON_RECT, srcRect, GraphicsUnit.Pixel);
            }
            else if (item == TravelButtonItem.ForwardButton)
            {
                g.DrawImage(_forwardButton, FORWARD_BUTTON_RECT, srcRect, GraphicsUnit.Pixel);
            }
            else if (item == TravelButtonItem.BackGround)
            {
                if (!BackButtonOnly)
                {
                    g.DrawImage(_backGround, BACKGROUND_RECT, srcRect, GraphicsUnit.Pixel);
                }
                else
                {
                    var rect1 = new Rectangle(BACKGROUND_RECT.X, BACKGROUND_RECT.Y,
                                              BACKGROUND_RECT.Width - DROPDOWN_ARROW_RECT.Width, BACKGROUND_RECT.Height);
                    var rect2 = new Rectangle(srcRect.X, srcRect.Y, srcRect.Width - DROPDOWN_ARROW_RECT.Width,
                                              srcRect.Height);
                    g.DrawImage(_backGround, rect1, rect2, GraphicsUnit.Pixel);
                }
            }
        }

        #endregion
    }

    #endregion

    #region Other

    public delegate void TravelButtonItemClickedEventHandler(object sender, TravelButtonItemClickedEventArgs e);

    public enum TravelButtonItem
    {
        BackGround,
        BackButton,
        ForwardButton,
        DropDownArrow
    }

    public class TravelButtonItemClickedEventArgs : EventArgs
    {
        private readonly TravelButtonItem _item = TravelButtonItem.BackGround;

        public TravelButtonItemClickedEventArgs(TravelButtonItem clickedItem)
        {
            _item = clickedItem;
        }

        public TravelButtonItem ClickedItem
        {
            get { return _item; }
        }
    }

    #endregion
}