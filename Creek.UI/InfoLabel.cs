using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Creek.UI
{
    /// <seealso cref="http://beta.unclassified.de/code/dotnet/infolabel/"/>
    [Designer(typeof (InfoLabelDesigner))]
    [DefaultEvent("Click")]
    public class InfoLabel : UserControl
    {
        #region Private fields

        private Brush backColorBrush;
        private Color borderColor;
        private Pen borderColorPen;
        private Color hoverBackColor;
        private Brush hoverBackColorBrush;
        private Color hoverBorderColor;
        private Pen hoverBorderColorPen;
        private Color hoverForeColor;
        private Brush hoverForeColorBrush;
        private bool hovering;
        private bool supportHovering;
        private bool useBorder;

        #endregion Private fields

        #region Constructors

        public InfoLabel()
        {
            InitializeComponent();

            BackColor = SystemColors.Info;
            ForeColor = SystemColors.InfoText;
            BorderColor = SystemColors.ControlDark;

            HoverBackColor = SystemColors.Highlight;
            HoverForeColor = SystemColors.HighlightText;
            HoverBorderColor = SystemColors.ControlDark;
        }

        #endregion Constructors

        #region Designer stuff

        private readonly IContainer components = null;
        private FlowLayoutPanel flowLayoutPanel1;
        private Label label1;
        private PictureBox pictureBox1;

        protected override void Dispose(bool disposing)
        {
            if (borderColorPen != null) borderColorPen.Dispose();
            if (backColorBrush != null) backColorBrush.Dispose();
            if (hoverBorderColorPen != null) hoverBorderColorPen.Dispose();
            if (hoverBackColorBrush != null) hoverBackColorBrush.Dispose();
            if (hoverForeColorBrush != null) hoverForeColorBrush.Dispose();

            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            flowLayoutPanel1 = new FlowLayoutPanel();
            pictureBox1 = new PictureBox();
            label1 = new Label();
            flowLayoutPanel1.SuspendLayout();
            ((ISupportInitialize) (pictureBox1)).BeginInit();
            SuspendLayout();
            //
            // flowLayoutPanel1
            //
            flowLayoutPanel1.AutoSize = true;
            flowLayoutPanel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flowLayoutPanel1.BackColor = Color.Transparent;
            flowLayoutPanel1.Controls.Add(pictureBox1);
            flowLayoutPanel1.Controls.Add(label1);
            flowLayoutPanel1.Dock = DockStyle.Top;
            flowLayoutPanel1.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanel1.Location = new Point(0, 0);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Padding = new Padding(3);
            flowLayoutPanel1.Size = new Size(279, 34);
            flowLayoutPanel1.TabIndex = 0;
            flowLayoutPanel1.MouseLeave += flowLayoutPanel1_MouseLeave;
            flowLayoutPanel1.Click += flowLayoutPanel1_Click;
            flowLayoutPanel1.Resize += flowLayoutPanel1_Resize;
            flowLayoutPanel1.MouseEnter += flowLayoutPanel1_MouseEnter;
            //
            // pictureBox1
            //
            pictureBox1.BackColor = Color.Transparent;
            flowLayoutPanel1.SetFlowBreak(pictureBox1, true);
            pictureBox1.Location = new Point(3, 3);
            pictureBox1.Margin = new Padding(1, 0, 2, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(16, 16);
            pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            pictureBox1.Visible = false;
            pictureBox1.MouseLeave += flowLayoutPanel1_MouseLeave;
            pictureBox1.Click += flowLayoutPanel1_Click;
            pictureBox1.MouseEnter += flowLayoutPanel1_MouseEnter;
            //
            // label1
            //
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Location = new Point(23, 4);
            label1.Margin = new Padding(0, 1, 0, 1);
            label1.Name = "label1";
            label1.Size = new Size(247, 26);
            label1.TabIndex = 1;
            label1.Text = "label1";
            label1.UseMnemonic = false;
            label1.MouseLeave += flowLayoutPanel1_MouseLeave;
            label1.Click += flowLayoutPanel1_Click;
            label1.MouseEnter += flowLayoutPanel1_MouseEnter;
            //
            // InfoLabel
            //
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(flowLayoutPanel1);
            DoubleBuffered = true;
            Name = "InfoLabel";
            Size = new Size(279, 80);
            Enter += InfoLabel_Enter;
            Leave += InfoLabel_Leave;
            KeyDown += InfoLabel_KeyDown;
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            ((ISupportInitialize) (pictureBox1)).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion Designer stuff

        #region Old properties

        [DefaultValue(typeof (SystemColors), "Info")]
        public override Color BackColor
        {
            get { return base.BackColor; }
            set
            {
                base.BackColor = value;
                backColorBrush = new SolidBrush(value);
            }
        }

        [DefaultValue(typeof (SystemColors), "InfoText")]
        public override Color ForeColor
        {
            get { return base.ForeColor; }
            set { base.ForeColor = value; }
        }

        [Browsable(false)]
        public override Image BackgroundImage
        {
            get { return base.BackgroundImage; }
            set { base.BackgroundImage = null; }
        }

        [Browsable(false)]
        public override ImageLayout BackgroundImageLayout
        {
            get { return base.BackgroundImageLayout; }
            set { base.BackgroundImageLayout = ImageLayout.None; }
        }

        [Browsable(false)]
        public new BorderStyle BorderStyle
        {
            get { return BorderStyle.None; }
        }

        public override AnchorStyles Anchor
        {
            get { return base.Anchor; }
            set
            {
                if ((value & AnchorStyles.Top) != 0 && (value & AnchorStyles.Bottom) != 0)
                    throw new ArgumentException();

                base.Anchor = value;
            }
        }

        [Browsable(false)]
        public override bool AutoScroll
        {
            get { return base.AutoScroll; }
            set { base.AutoScroll = false; }
        }

        [Browsable(false)]
        public override bool AutoSize
        {
            get { return base.AutoSize; }
            set { base.AutoSize = false; }
        }

        public override DockStyle Dock
        {
            get { return base.Dock; }
            set
            {
                if (value == DockStyle.Fill || value == DockStyle.Left || value == DockStyle.Right)
                    throw new ArgumentException();

                base.Dock = value;
            }
        }

        // This must be named different from "Text" because the designer won't
        // store that property persistently and reset it every time the project
        // is rebuilt.
        //[Category("Appearance")]
        //[Browsable(false)]
        //public string Text2
        //{
        //    get
        //    {
        //        return label1.Text;
        //    }
        //    set
        //    {
        //        label1.Text = value;
        //    }
        //}

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override string Text
        {
            get { return label1.Text; }
            set { label1.Text = value; }
        }

        public override ContextMenu ContextMenu { get; set; }

        public override ContextMenuStrip ContextMenuStrip { get; set; }

        #endregion Old properties

        #region New properties

        [Category("Appearance")]
        [DefaultValue(typeof (SystemColors), "ControlDark")]
        public Color BorderColor
        {
            get { return borderColor; }
            set
            {
                borderColor = value;
                borderColorPen = new Pen(borderColor);
                Invalidate();
            }
        }

        [Category("Appearance")]
        [DefaultValue(typeof (SystemColors), "ControlDark")]
        public Color HoverBorderColor
        {
            get { return hoverBorderColor; }
            set
            {
                hoverBorderColor = value;
                hoverBorderColorPen = new Pen(hoverBorderColor);
                Invalidate();
            }
        }

        [Category("Appearance")]
        [DefaultValue(false)]
        public bool UseBorder
        {
            get { return useBorder; }
            set
            {
                useBorder = value;
                flowLayoutPanel1.Padding = new Padding(useBorder ? 3 : 2);
                Invalidate();
            }
        }

        [Category("Appearance")]
        [DefaultValue(typeof (SystemColors), "Highlight")]
        public Color HoverBackColor
        {
            get { return hoverBackColor; }
            set
            {
                hoverBackColor = value;
                hoverBackColorBrush = new SolidBrush(value);
                Invalidate();
            }
        }

        [Category("Appearance")]
        [DefaultValue(typeof (SystemColors), "HighlightText")]
        public Color HoverForeColor
        {
            get { return hoverForeColor; }
            set
            {
                hoverForeColor = value;
                hoverForeColorBrush = new SolidBrush(value);
                Invalidate();
            }
        }

        [Category("Appearance")]
        [DefaultValue(null)]
        public Image IconImage
        {
            get { return pictureBox1.Image; }
            set
            {
                pictureBox1.Image = value;
                pictureBox1.Visible = value != null;
            }
        }

        [Category("Behavior")]
        [DefaultValue(false)]
        public bool SupportHovering
        {
            get { return supportHovering; }
            set
            {
                supportHovering = value;
                UpdateHovering();
                Invalidate();
            }
        }

        #endregion New properties

        #region New event handlers

        protected override void OnSizeChanged(EventArgs e)
        {
            Invalidate();
            base.OnSizeChanged(e);
        }

        private void flowLayoutPanel1_Resize(object sender, EventArgs e)
        {
            UpdateSize();
        }

        private void flowLayoutPanel1_MouseEnter(object sender, EventArgs e)
        {
            hovering = true;
            UpdateHovering();
            Invalidate();
        }

        private void flowLayoutPanel1_MouseLeave(object sender, EventArgs e)
        {
            hovering = false;
            UpdateHovering();
            Invalidate();
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);

            if (useBorder)
            {
                if (supportHovering && hovering)
                {
                    e.Graphics.FillRectangle(hoverBackColorBrush, 1, 1, Width - 2, Height - 2);
                    e.Graphics.DrawRectangle(hoverBorderColorPen, 0, 0, Width - 1, Height - 1);
                }
                else
                {
                    //e.Graphics.FillRectangle(backColorBrush, 1, 1, Width - 2, Height - 2);
                    e.Graphics.DrawRectangle(borderColorPen, 0, 0, Width - 1, Height - 1);
                }
            }
            else
            {
                if (supportHovering && hovering)
                {
                    e.Graphics.FillRectangle(hoverBackColorBrush, 0, 0, Width, Height);
                }
                else
                {
                    //e.Graphics.FillRectangle(backColorBrush, 0, 0, Width, Height);
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (Focused || FindForm() != null && FindForm().ActiveControl == this)
                // Focused alone doesn't work when re-activating the window
            {
                e.Graphics.DrawRectangle(new Pen(new HatchBrush(HatchStyle.Percent50, label1.ForeColor, Color.Empty)), 0,
                                         0, Width - 1, Height - 1);
            }

            // Immediately paint sub controls to avoid a delay of screen garbage
            pictureBox1.Update();
            label1.Update();
        }

        #endregion New event handlers

        #region Other private methods

        private void UpdateSize()
        {
            int oldHeight = Height;
            Height = flowLayoutPanel1.Height;
            if (Height != oldHeight)
                Invalidate();
        }

        private void UpdateHovering()
        {
            if (supportHovering && hovering)
            {
                label1.ForeColor = HoverForeColor;
                Cursor = HandCursor;
            }
            else
            {
                label1.ForeColor = Color.Empty;
                Cursor = Cursors.Default;
            }
        }

        #endregion Other private methods

        #region Public events

        public new event EventHandler Click;

        private void flowLayoutPanel1_Click(object sender, EventArgs e)
        {
            if (ContextMenu != null)
            {
                ContextMenu.Show(this, new Point(0, Height - (useBorder ? 1 : 0)));
            }
            else if (ContextMenuStrip != null)
            {
                ContextMenuStrip.Show(this, new Point(0, Height - (useBorder ? 1 : 0)));
            }
            else
            {
                if (Click != null) Click(this, EventArgs.Empty);
            }
        }

        private void InfoLabel_Enter(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void InfoLabel_Leave(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void InfoLabel_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Enter || e.KeyCode == Keys.Space) && !e.Alt && !e.Control && !e.Shift)
            {
                flowLayoutPanel1_Click(this, EventArgs.Empty);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        #endregion Public events

        #region WinAPI stuff

        // This comes from class Unclassified.WinApi

        /// <summary>
        /// Gets the system's hand mouse cursor, used for hyperlinks.
        /// The .NET framework only gives its internal cursor but not the one that the user has set in their profile.
        /// </summary>
        private Cursor HandCursor
        {
            get
            {
                RegistryKey cursorsKey = Registry.CurrentUser.OpenSubKey(@"Control Panel\Cursors");
                if (cursorsKey != null)
                {
                    object o = cursorsKey.GetValue("Hand");
                    if (o is string)
                    {
                        IntPtr cursorHandle = LoadCursorFromFile((string) o);
                        return new Cursor(cursorHandle);
                    }
                }
                return Cursors.Hand;
            }
        }

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr LoadCursorFromFile(string path);
    }

    #endregion WinAPI stuff
}