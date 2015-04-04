using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Media;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Creek.UI
{
    public class ValidatingTextBoxEventArgs : EventArgs
    {
        private bool accept = true;
        private string text = "";

        public ValidatingTextBoxEventArgs(string text)
        {
            this.text = text;
        }

        /// <summary>
        /// Gets the text that should be validated or sets a new text to be inserted into the input field.
        /// </summary>
        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the new text should be accepted.
        /// </summary>
        public bool Accept
        {
            get { return accept; }
            set { accept = value; }
        }
    }

    public enum ValidatingTextBoxType
    {
        /// <summary>
        /// No restriction on the text
        /// </summary>
        Default = 0,

        /// <summary>
        /// Only numeric text is allowed (digits from 0 to 9 only)
        /// </summary>
        Numeric,

        /// <summary>
        /// The list of valid characters is regarded, if not empty
        /// </summary>
        ValidCharacters,

        /// <summary>
        /// The regular expression is regarded, if not empty
        /// </summary>
        RegularExpression,

        /// <summary>
        /// The <code>ValidateText</code> event is fired to delegate the text validation
        /// </summary>
        Custom
    }

    public class ValidatingTextBox : TextBox
    {
        private bool autoHeight;
        private bool deferFinalCheck;
        private ToolTip errorNoticeTooltip;
        private string expectedFormatDescription = "";
        private Regex finalRegex;
        private string finalRegexString = "";
        private int leftPadding;
        private int prevSelLen;
        private int prevSelStart;
        private string prevText = "";
        private Regex regex;
        private string regexString = "";
        private bool requireInput;
        private bool trimOnLeaving;
        private ValidatingTextBoxType type = ValidatingTextBoxType.Default;
        private string validCharacters = "";

        /// <summary>
        /// Standard type of text input to be accepted
        /// </summary>
        [Category("Behavior")]
        [Description("Standard type of text input to be accepted")]
        [DefaultValue(ValidatingTextBoxType.Default)]
        public ValidatingTextBoxType Type
        {
            get { return type; }
            set { type = value; }
        }

        /// <summary>
        /// Characters to be accepted
        /// </summary>
        [Category("Behavior")]
        [Description("Characters to be accepted")]
        [DefaultValue("")]
        public string ValidCharacters
        {
            get { return validCharacters; }
            set { validCharacters = value; }
        }

        /// <summary>
        /// Regex pattern to be accepted
        /// </summary>
        [Category("Behavior")]
        [Description("Regex to be accepted")]
        [DefaultValue("")]
        public string RegularExpression
        {
            get { return regexString; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    regex = new Regex(value);
                else
                    regex = null;
                regexString = value;
            }
        }

        /// <summary>
        /// Regex pattern to be matched when leaving the focus
        /// </summary>
        [Category("Behavior")]
        [Description("Regex pattern to be matched when leaving the focus")]
        [DefaultValue("")]
        public string FinalRegularExpression
        {
            get { return finalRegexString; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    finalRegex = new Regex(value);
                else
                    finalRegex = null;
                finalRegexString = value;
            }
        }

        /// <summary>
        /// Require input. Checked as part of the final check.
        /// </summary>
        [Category("Behavior")]
        [Description("Require input. Checked as part of the final check.")]
        [DefaultValue(false)]
        public bool RequireInput
        {
            get { return requireInput; }
            set { requireInput = value; }
        }

        /// <summary>
        /// Trim the text when leaving the focus
        /// </summary>
        [Category("Behavior")]
        [Description("Trim the text when leaving the focus")]
        [DefaultValue(false)]
        public bool TrimOnLeaving
        {
            get { return trimOnLeaving; }
            set { trimOnLeaving = value; }
        }

        /// <summary>
        /// Description of the expected format to be displayed to the user
        /// </summary>
        [Category("Behavior")]
        [Description("Description of the expected format to be displayed to the user")]
        [DefaultValue("")]
        public string ExpectedFormatDescription
        {
            get { return expectedFormatDescription; }
            set { expectedFormatDescription = value; }
        }

        /// <summary>
        /// Defer the final checks. They will only be applied when the FinalCheck method is invoked.
        /// </summary>
        [Category("Behavior")]
        [Description("Defer the final checks. They will only be applied when the FinalCheck method is invoked.")]
        [DefaultValue(false)]
        public bool DeferFinalCheck
        {
            get { return deferFinalCheck; }
            set { deferFinalCheck = value; }
        }

        /// <summary>
        /// Adjust the height of the control depending on its contents
        /// </summary>
        [Category("Layout")]
        [Description("Adjust the height of the control depending on its contents")]
        [DefaultValue(false)]
        public bool AutoHeight
        {
            get { return autoHeight; }
            set
            {
                if (value != autoHeight)
                {
                    autoHeight = value;
                    UpdateAutoHeight();
                }
            }
        }

        /// <summary>
        /// Adjust the left and right padding
        /// </summary>
        [Category("Layout")]
        [Description("Adjust the left and right padding")]
        [DefaultValue(0)]
        public int LeftPadding
        {
            get { return leftPadding; }
            set
            {
                if (value != leftPadding)
                {
                    leftPadding = value;
                    UpdatePadding();
                    UpdateAutoHeight();
                }
            }
        }

        /// <summary>
        /// Validate the new text before it is applied. Only used if Type is <code>Custom</code>.
        /// </summary>
        [Category("Behavior")]
        [Description("Validate the new text before it is applied. Only used if Type is Custom.")]
        public event EventHandler<ValidatingTextBoxEventArgs> ValidateText;

        /// <summary>
        /// Validate the new text before the focus leaves.
        /// </summary>
        [Category("Behavior")]
        [Description("Validate the new text before the focus leaves.")]
        public event EventHandler<ValidatingTextBoxEventArgs> FinalValidateText;

        protected override void Dispose(bool disposing)
        {
            if (errorNoticeTooltip != null) errorNoticeTooltip.Dispose();

            base.Dispose(disposing);
        }

        protected override void OnTextChanged(EventArgs e)
        {
            bool accept = true;

            HideErrorNotice();
            if (type == ValidatingTextBoxType.Numeric)
            {
                string valid = "0123456789";
                foreach (char c in Text)
                {
                    if (valid.IndexOf(c) == -1)
                    {
                        accept = false;
                        break;
                    }
                }
            }
            else if (type == ValidatingTextBoxType.ValidCharacters && !string.IsNullOrEmpty(validCharacters))
            {
                foreach (char c in Text)
                {
                    if (validCharacters.IndexOf(c) == -1)
                    {
                        accept = false;
                        break;
                    }
                }
            }
            else if (type == ValidatingTextBoxType.RegularExpression && regex != null)
            {
                accept = regex.IsMatch(Text);
            }
            else if (type == ValidatingTextBoxType.Custom && ValidateText != null)
            {
                var e2 = new ValidatingTextBoxEventArgs(Text);
                ValidateText(this, e2);
                accept = e2.Accept;
            }

            if (!accept)
            {
                SystemSounds.Beep.Play();
                Console.Beep(50, 10);
                Text = prevText;
                SelectionStart = prevSelStart;
                SelectionLength = prevSelLen;
                ShowErrorNotice();
            }
            else
            {
                prevText = Text;
            }

            base.OnTextChanged(e);

            UpdateAutoHeight();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            // Add some comfort here
            if (e.KeyCode == Keys.Back && !e.Alt && e.Control && !e.Shift ||
                e.KeyCode == Keys.W && !e.Alt && e.Control && !e.Shift)
            {
                // Delete word to the left
                int sel = SelectionStart;
                if (sel > 0)
                {
                    int pos = Text.Substring(0, sel - 1).LastIndexOfAny(new[] {' ', '\t'});
                    //if (pos == -1) pos = 0;
                    pos++;
                    Text = Text.Substring(0, pos) + Text.Substring(sel);
                    SelectionStart = pos;
                }
                e.SuppressKeyPress = e.Handled = true;
            }
            if (e.KeyCode == Keys.U && !e.Alt && e.Control && !e.Shift)
            {
                // Delete all
                Text = "";
                e.SuppressKeyPress = e.Handled = true;
            }
            if (e.KeyCode == Keys.A && !e.Alt && e.Control && !e.Shift)
            {
                // Select all
                SelectionStart = 0;
                SelectionLength = Text.Length;
                e.SuppressKeyPress = e.Handled = true;
            }
            if (e.KeyCode == Keys.Up && !e.Alt && e.Control && !e.Shift && Multiline)
            {
                // Scroll line up
                SendMessage(Handle, EM_SCROLL, new IntPtr(SB_LINEUP), IntPtr.Zero);
                e.SuppressKeyPress = e.Handled = true;
            }
            if (e.KeyCode == Keys.Down && !e.Alt && e.Control && !e.Shift && Multiline)
            {
                // Scroll line down
                SendMessage(Handle, EM_SCROLL, new IntPtr(SB_LINEDOWN), IntPtr.Zero);
                e.SuppressKeyPress = e.Handled = true;
            }

            prevSelStart = SelectionStart;
            prevSelLen = SelectionLength;

            base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);

            prevSelStart = SelectionStart;
            prevSelLen = SelectionLength;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            prevSelStart = SelectionStart;
            prevSelLen = SelectionLength;

            base.OnMouseDown(e);
        }

        protected override void OnLeave(EventArgs e)
        {
            if (trimOnLeaving)
            {
                Text = Text.Trim();
            }

            HideErrorNotice();
            if (!deferFinalCheck)
            {
                FinalCheck();
            }

            base.OnLeave(e);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            UpdateAutoHeight();
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == 0x30 /* WM_SETFONT */)
            {
                // Post-process the message
                UpdatePadding();
            }
        }

        private void UpdatePadding()
        {
            // Set new margins for multi-line text boxes
            SendMessage(
                Handle,
                0xD3 /* EM_SETMARGINS */,
                (IntPtr) 3 /* EC_LEFTMARGIN | EC_RIGHTMARGIN */,
                (IntPtr) (leftPadding & 0xffff) /* left/right margin value */);
        }

        /// <summary>
        /// Performs the deferred final checks. Shows the error message, plays the beep sound and focuses the control if errors were found.
        /// </summary>
        /// <returns>true if the check is successful, false if errors were found</returns>
        public bool FinalCheck()
        {
            bool accept = true;

            HideErrorNotice();
            if (accept && requireInput && Text.Length == 0)
            {
                accept = false;
            }
            if (accept && finalRegex != null)
            {
                accept = finalRegex.IsMatch(Text);
            }
            if (accept && FinalValidateText != null)
            {
                var e2 = new ValidatingTextBoxEventArgs(Text);
                FinalValidateText(this, e2);
                accept = e2.Accept;
                Text = e2.Text;
            }

            if (!accept)
            {
                ShowErrorNotice();
                SystemSounds.Beep.Play();
                Console.Beep(50, 10);
                Focus();
            }
            return accept;
        }

        protected void ShowErrorNotice()
        {
            string msg;
            switch (CultureInfo.CurrentUICulture.TwoLetterISOLanguageName)
            {
                case "de":
                    msg = "Die Eingabe hat ein ungültiges Format.";
                    if (!string.IsNullOrEmpty(expectedFormatDescription))
                        msg += "\r\nErwartet: " + expectedFormatDescription;
                    else if (Type == ValidatingTextBoxType.Numeric)
                        msg += "\r\nEs sind nur nummerische Eingaben (0-9) zulässig.";
                    if (requireInput)
                        msg += "\r\nEine Eingabe ist erforderlich.";
                    break;
                default:
                    msg = "The input format is invalid.";
                    if (!string.IsNullOrEmpty(expectedFormatDescription))
                        msg += "\r\nExpected: " + expectedFormatDescription;
                    else if (Type == ValidatingTextBoxType.Numeric)
                        msg += "\r\nOnly numeric values (0-9) are accepted.";
                    if (requireInput)
                        msg += "\r\nInput is required.";
                    break;
            }

            if (errorNoticeTooltip != null)
            {
                errorNoticeTooltip.Dispose();
            }
            errorNoticeTooltip = new ToolTip();
            errorNoticeTooltip.ForeColor = SystemColors.WindowText;
            errorNoticeTooltip.BackColor = MixedColor(SystemColors.Window, Color.Red, 0.25);
            errorNoticeTooltip.Show(msg, this, 0, Height, 10000);
        }

        protected void HideErrorNotice()
        {
            if (errorNoticeTooltip != null)
            {
                errorNoticeTooltip.Dispose();
            }
        }

        private void UpdateAutoHeight()
        {
            if (autoHeight && Multiline)
            {
                //Size prefSize = GetPreferredSize(new Size(Width, 0));

                // This is based on TextBoxBase.GetPreferredSizeCore():
                Size borderSize = SizeFromClientSize(Size.Empty) + Padding.Size;
                if (BorderStyle != BorderStyle.None)
                    borderSize += new Size(0, 3);
                if (BorderStyle == BorderStyle.FixedSingle)
                    borderSize += new Size(2, 2);
                var proposedSize = new Size(Width - borderSize.Width, 0);
                // WordEllipsis is what makes it better than the default:
                // it prevents the measure rectangle to extend to the right for words longer than
                // a line, which would in turn allow for more words in all lines, resulting in a
                // lower measured height.
                Size prefSize = TextRenderer.MeasureText(Text, Font, proposedSize,
                                                         TextFormatFlags.WordBreak | TextFormatFlags.WordEllipsis);
                Height = Math.Max(prefSize.Height, base.FontHeight) + borderSize.Height;

                // FIXME
                // The height is still too low for lines wrapped in the middle of a word, but at
                // least it won't get smaller the longer word is entered.
            }
        }

        #region Unclassified.WinApi

        private const uint EM_SCROLL = 0xB5;
        private const uint SB_LINEUP = 0;
        private const uint SB_LINEDOWN = 1;

        [DllImport("user32")]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        #endregion

        #region Unclassified.Drawing.ColorMath

        private static Color MixedColor(Color color1, Color color2, double ratio)
        {
            var a = (int) Math.Round(color1.A*(1 - ratio) + color2.A*ratio);
            var r = (int) Math.Round(color1.R*(1 - ratio) + color2.R*ratio);
            var g = (int) Math.Round(color1.G*(1 - ratio) + color2.G*ratio);
            var b = (int) Math.Round(color1.B*(1 - ratio) + color2.B*ratio);
            return Color.FromArgb(a, r, g, b);
        }

        #endregion
    }
}