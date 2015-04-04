using System;
using System.Drawing;
using System.Windows.Forms;

namespace Creek.UI
{
    [ToolboxBitmap(typeof (Label))]
    public class EditableLabel : Label
    {
        private readonly TextBox TextBox1;

        public EditableLabel()
        {
            TextBox1 = new TextBox {BorderStyle = BorderStyle.None, Dock = DockStyle.Fill};

            TextBox1.KeyDown += TextBox1_KeyDown;
            MouseDoubleClick += Label1_MouseDoubleClick;

            Controls.Add(TextBox1);
            TextBox1.Hide();
        }

        private void TextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;

                Text = TextBox1.Text;
                Show();

                TextBox1.Hide();
                OnTextChanged(new EventArgs());
            }
        }


        private void Label1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            TextBox1.Text = Text;
            TextBox1.Show();
        }

        public override string ToString()
        {
            return Text;
        }
    }
}