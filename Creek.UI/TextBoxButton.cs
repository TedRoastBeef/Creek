using System;
using System.Drawing;
using System.Windows.Forms;

namespace Creek.UI
{
    public class TextBoxButton : RichTextBox
    {
        private Button myButton;

        public TextBoxButton()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            myButton = new Button();
            SuspendLayout();
            // 
            // myButton
            // 
            myButton.Dock = DockStyle.Right;
            myButton.Location = new Point(173, 0);
            myButton.MaximumSize = new Size(23, 0);
            myButton.MinimumSize = new Size(23, 0);
            myButton.Name = "myButton";
            myButton.Text = "...";
            myButton.Size = new Size(25, 83);
            myButton.TabIndex = 1;
            myButton.UseVisualStyleBackColor = true;
            myButton.MouseEnter += OnMouseEnter;
            // 
            // TextBoxButton
            // 
            Margin = new Padding(0);
            ScrollBars = RichTextBoxScrollBars.None;
            Size = new Size(196, 83);
            MouseEnter += OnMouseEnter;
            ResumeLayout(false);
        }

        public event EventHandler ClickButton
        {
            add { myButton.Click += value; }
            remove { myButton.Click -= value; }
        }

        protected override void OnCreateControl()
        {
            if (!Controls.Contains(myButton))
            {
                Controls.Add(myButton);
                //size of control - size control button +10
                RightMargin = Size.Width - (myButton.Size.Width + 10);
            }

            base.OnCreateControl();
        }

        private void OnMouseEnter(object sender, EventArgs e)
        {
            if (sender is Button)
            {
                myButton.Cursor = Cursors.Default;
            }
            else
            {
                Cursor = Cursors.IBeam;
            }
        }
    }
}