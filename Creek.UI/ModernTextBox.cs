using System;
using System.Drawing;
using System.Windows.Forms;

namespace Creek.UI
{
    public sealed class ModernTextBox : TextBox
    {
        private readonly Font EditFont;
        private readonly Font NonEditFont;

        private string EditText;

        public ModernTextBox()
        {
            NonEditFont = new Font("Segoe UI", 9, FontStyle.Italic);
            EditFont = new Font("Segoe UI", 9, FontStyle.Regular);

            Font = NonEditFont;

            GotFocus += ModernTextBox_GotFocus;
            LostFocus += ModernTextBox_LostFocus;
            TextChanged += ModernTextBox_TextChanged;
        }

        public bool isPasswordBox { get; set; }
        public string NonEditText { get; set; }

        private void ModernTextBox_TextChanged(object sender, EventArgs e)
        {
            EditText = Text;
        }


        private void ModernTextBox_LostFocus(object sender, EventArgs e)
        {
            if (Text != NonEditText)
            {
                Font = NonEditFont;
                Text = NonEditText;
            }
        }

        private void ModernTextBox_GotFocus(object sender, EventArgs e)
        {
            if (Text != NonEditText)
            {
                Font = EditFont;
                if (isPasswordBox)
                {
                    PasswordChar = '•';
                }
            }
            else
            {
                Text = "";
            }
        }
    }
}