using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Creek.UI
{
    [DefaultEvent("Command")]
    public partial class CommandPrompt : UserControl
    {
        private readonly ArrayList prevMessages = new ArrayList();
        private BorderStyle borderInput = BorderStyle.None;
        private bool cancelOnEsc = true;
        private int currentLine;
        private char[] delimiters = new[] {' '};
        private bool ignoreBlankCommands = true;
        private Color messageColor = SystemColors.ControlText;
        private string promptString = ">";
        private bool showMessages = true;

        public CommandPrompt()
        {
            BackColor = SystemColors.Window;
            InitializeComponent();
            rtbMessages.Height = 0;
        }

        [Description("Show the previous commands")]
        [DefaultValue(true)]
        public bool ShowMessages
        {
            get { return showMessages; }
            set
            {
                if (showMessages != value)
                {
                    if (value)
                    {
                        Height = panelBottom.Height + txtInput.PreferredHeight*5;
                        rtbMessages.Visible = true;
                    }
                    else
                    {
                        rtbMessages.Visible = false;
                        Height = txtInput.Height;
                    }
                }
                showMessages = value;
            }
        }

        [Description("Determines whether event will fire for blank commands")]
        [DefaultValue(true)]
        public bool IgnoreBlankCommands
        {
            get { return ignoreBlankCommands; }
            set { ignoreBlankCommands = value; }
        }

        [Description("String showed as the prompt")]
        [DefaultValue(">")]
        public string PromptString
        {
            get { return promptString; }
            set
            {
                promptString = value;
                lblPrompt.Text = promptString;
            }
        }

        [Description("Forecolor of the prompt")]
        public Color PromptColor
        {
            get { return lblPrompt.ForeColor; }
            set { lblPrompt.ForeColor = value; }
        }


        [Description("Border around the input box")]
        [DefaultValue(BorderStyle.None)]
        public BorderStyle BorderInput
        {
            get { return borderInput; }
            set
            {
                borderInput = value;
                panelBottom.BorderStyle = borderInput;
            }
        }

        [Description("String of characters which act as delimiters between parameters")]
        [DefaultValue(new[] {' '})]
        public char[] Delimiters
        {
            get { return delimiters; }
            set { delimiters = value; }
        }

        [Description("Indicates whether input area is cleared on pressing Escape")]
        [DefaultValue(true)]
        public bool CancelOnEsc
        {
            get { return cancelOnEsc; }
            set { cancelOnEsc = value; }
        }

        [Description("Color of the messages added using AddMessage method")]
        public Color MessageColor
        {
            get { return messageColor; }
            set { messageColor = value; }
        }

        #region "AutoComplete"

        private bool autoCompleteStore;

        [Description("Indicates text completion behaviour at prompt")]
        [DefaultValue(AutoCompleteMode.None)]
        public AutoCompleteMode AutoComplete
        {
            get { return txtInput.AutoCompleteMode; }
            set
            {
                txtInput.AutoCompleteMode = value;
                txtInput.AutoCompleteSource = AutoCompleteSource.CustomSource;
            }
        }

        [Description("Store messages for AutoComplete feature")]
        [DefaultValue(false)]
        public bool AutoCompleteStore
        {
            get { return autoCompleteStore; }
            set { autoCompleteStore = value; }
        }

        [Description("Add AutoComplete data")]
        public void AutoCompleteAdd(string str)
        {
            txtInput.AutoCompleteCustomSource.Add(str);
        }

        [Description("Clear all AutoComplete data")]
        public void AutoCompleteClear()
        {
            txtInput.AutoCompleteCustomSource.Clear();
        }

        #endregion

        #region "File IO"

        [Description("Save the messages to a file")]
        public bool SaveMessages(string FileName)
        {
            try
            {
                rtbMessages.SaveFile(FileName);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        [Description("Load messages from a file")]
        public bool LoadMessages(string FileName)
        {
            try
            {
                rtbMessages.LoadFile(FileName);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion

        [Description("Adds a message to the message RichTextBox")]
        public void AddMessage(string msg)
        {
            int prevLength = rtbMessages.Text.Length;

            if (rtbMessages.Lines.Length > 0)
                rtbMessages.AppendText("\n" + msg);
            else
                rtbMessages.AppendText(msg);

            rtbMessages.SelectionStart = prevLength;
            rtbMessages.SelectionLength = rtbMessages.Text.Length - prevLength;
            rtbMessages.SelectionColor = messageColor;

            rtbMessages.SelectionStart = rtbMessages.Text.Length;
            rtbMessages.SelectionLength = 0;
            rtbMessages.SelectionColor = rtbMessages.ForeColor;
            rtbMessages.ScrollToCaret();

            txtInput.Focus();
        }

        /// <summary>
        /// Adds a command to the messages list
        /// </summary>
        /// <param name="prompt">The prompt string</param>
        /// <param name="command">The command entered</param>
        private void AddCommand(string prompt, Color color, string command)
        {
            int prevLength = rtbMessages.Text.Length;

            rtbMessages.AppendText(prompt + " " + command);

            rtbMessages.SelectionStart = prevLength;
            rtbMessages.SelectionLength = prompt.Length;
            rtbMessages.SelectionColor = color;

            rtbMessages.SelectionLength = 0;
            rtbMessages.SelectionStart = rtbMessages.Text.Length;
            rtbMessages.ScrollToCaret();

            txtInput.Focus();
        }

        [Description("Clear all messages from RichTextBox")]
        public void ClearMessages()
        {
            rtbMessages.Clear();
            rtbMessages.Height = 0;
            rtbMessages.ScrollBars = RichTextBoxScrollBars.None;
        }

        // Stores the current line
        // Used while navigating the previous commands list

        private void txtInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                if (txtInput.Text != "" && IgnoreBlankCommands)
                {
                    SuspendLayout();

                    string prevPrompt = lblPrompt.Text;
                    Color prevPromptColor = PromptColor;
                    // Raise the parameter entered event
                    var parameterEnteredArgs = new ParameterEnteredEventArgs(txtInput.Text, ParseInput(), true);
                    OnParameterEntered(parameterEnteredArgs);

                    // Raise the command event
                    var args = new CommandEventArgs(txtInput.Text, ParseInput(), this);
                    OnCommand(args);

                    if (args.Cancel == false)
                    {
                        if (rtbMessages.Lines.Length > 0)
                            rtbMessages.AppendText("\r\n");

                        AddCommand(prevPrompt, prevPromptColor, txtInput.Text);

                        if (args.Message != "")
                            AddMessage(args.Message);

                        rtbMessages.ScrollToCaret();
                        prevMessages.Add(txtInput.Text);
                        if (autoCompleteStore && args.Record)
                            txtInput.AutoCompleteCustomSource.Add(txtInput.Text);

                        currentLine = prevMessages.Count - 1;
                    }

                    txtInput.Text = "";
                    ResumeLayout();
                }
                e.Handled = true;
                HideToolTip();
                return;
            }

            if (e.KeyCode == Keys.Up)
            {
                if (currentLine >= 0 && prevMessages.Count > 0)
                {
                    txtInput.Text = prevMessages[currentLine].ToString();
                    txtInput.SelectionLength = 0;
                    txtInput.SelectionStart = txtInput.Text.Length;

                    currentLine--;
                }

                e.Handled = true;
                HideToolTip();
                return;
            }

            if (e.KeyCode == Keys.Down)
            {
                if (currentLine < prevMessages.Count - 2)
                {
                    currentLine++;

                    txtInput.Text = prevMessages[currentLine + 1].ToString();
                    txtInput.SelectionLength = 0;
                    txtInput.SelectionStart = txtInput.Text.Length;
                }

                e.Handled = true;
                HideToolTip();
                return;
            }

            if (e.KeyCode == Keys.Escape)
            {
                if (txtInput.SelectionLength > 0 && txtInput.AutoCompleteMode != AutoCompleteMode.None)
                {
                    txtInput.Text = txtInput.Text.Substring(0, txtInput.SelectionStart);
                    txtInput.SelectionStart = txtInput.Text.Length;
                }
                else
                {
                    if (toolTipCommand.Active == false && cancelOnEsc)
                        txtInput.Text = "";
                }
                HideToolTip();
                e.Handled = true;
                return;
            }
        }

        private void txtInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char) Keys.Return)
            {
                e.Handled = true;
                return;
            }

            if (e.KeyChar == (char) Keys.Back)
            {
                // Hide tooltip if character to be deleted is a delimiter
                if (txtInput.Text.Length > 0)
                {
                    char del = txtInput.Text[txtInput.Text.Length - 1];
                    foreach (char ch in delimiters)
                    {
                        if (ch == del)
                        {
                            bool flag = true;
                            // Check if previous character is delimiter
                            if (txtInput.Text.Length > 1)
                            {
                                char temp = txtInput.Text[txtInput.Text.Length - 2];
                                foreach (char c in delimiters)
                                {
                                    if (c == temp)
                                    {
                                        flag = false;
                                        break;
                                    }
                                }
                            }

                            if (flag)
                                HideToolTip();
                            break;
                        }
                    }
                }
                return;
            }

            // Fire the ParameterEntered event
            bool paramFlag = false;
            foreach (char ch in delimiters)
            {
                if (e.KeyChar == ch)
                {
                    paramFlag = true;
                    break;
                }
            }

            if (paramFlag)
            {
                var parameterEnteredArgs = new ParameterEnteredEventArgs(txtInput.Text, ParseInput());
                OnParameterEntered(parameterEnteredArgs);

                if (parameterEnteredArgs.ToolTip != "")
                {
                    HideToolTip();
                    ShowToolTip(parameterEnteredArgs.ToolTip);
                }
            }
        }

        private void txtInput_TextChanged(object sender, EventArgs e)
        {
            // Fire the CommandEntering event first
            var commandEnteringArgs = new CommandEnteringEventArgs(txtInput.Text);
            OnCommandEntering(commandEnteringArgs);

            if (commandEnteringArgs.ToolTip != "")
            {
                HideToolTip();
                ShowToolTip(commandEnteringArgs.ToolTip);
            }
        }

        private string[] ParseInput()
        {
            string temp = txtInput.Text;
            return temp.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
        }

        private void Prompt_Load(object sender, EventArgs e)
        {
            txtInput.Height = txtInput.PreferredHeight;
        }

        private void Prompt_FontChanged(object sender, EventArgs e)
        {
            txtInput.Font = Font;
        }

        private void Prompt_ForeColorChanged(object sender, EventArgs e)
        {
            txtInput.ForeColor = ForeColor;
            rtbMessages.ForeColor = ForeColor;
        }

        private void Prompt_BackColorChanged(object sender, EventArgs e)
        {
            rtbMessages.BackColor = BackColor;
            txtInput.BackColor = BackColor;
            panelBottom.BackColor = BackColor;
            lblPrompt.BackColor = BackColor;
        }

        private void rtbMessages_TextChanged(object sender, EventArgs e)
        {
            if (showMessages)
            {
                if (rtbMessages.Height < Height - panelBottom.Height)
                    rtbMessages.Height = rtbMessages.Lines.Length*txtInput.PreferredHeight;
                else
                    rtbMessages.ScrollBars = RichTextBoxScrollBars.Both;

                if (rtbMessages.Height > Height - panelBottom.Height)
                {
                    rtbMessages.Height = Height - panelBottom.Height;
                    rtbMessages.ScrollBars = RichTextBoxScrollBars.Both;
                }
                rtbMessages.ScrollToCaret();
            }
        }

        private void rtbMessages_Click(object sender, EventArgs e)
        {
            if (rtbMessages.SelectionLength == 0)
                txtInput.Focus();
        }

        private void CommandPrompt_Resize(object sender, EventArgs e)
        {
            if (showMessages)
            {
                if ((Height - panelBottom.Height)%txtInput.PreferredHeight != 0)
                {
                    Height = ((Height - panelBottom.Height)/txtInput.PreferredHeight + 1)
                             *txtInput.PreferredHeight + panelBottom.Height;
                }
            }
        }

        #region "ToolTip Functions"

        public void ShowToolTip(string tip)
        {
            Point pos = txtInput.GetPositionFromCharIndex(txtInput.Text.Length - 1);
            if (txtInput.Text.Length > 1)
                pos.X +=
                    TextRenderer.MeasureText(txtInput.Text.Substring(txtInput.Text.Length - 1, 1), txtInput.Font).Width;
            int height = TextRenderer.MeasureText(tip, new Font("Tahoma", 9)).Height + 2;
            if (rtbMessages.Size.Height + panelBottom.Height + height > Height)
                pos.Y -= height;
            else
                pos.Y += TextRenderer.MeasureText("test", txtInput.Font).Height;
            toolTipCommand.Show(tip, txtInput, pos);
        }

        public void HideToolTip()
        {
            if (toolTipCommand.Active)
                toolTipCommand.Hide(txtInput);
        }

        #endregion

        #region "Events"

        #region Delegates

        public delegate void CommandEnteringEventHandler(object sender, CommandEnteringEventArgs e);

        public delegate void CommandEventHandler(object sender, CommandEventArgs e);

        public delegate void ParameterEnteredEventHandler(object sender, ParameterEnteredEventArgs e);

        #endregion

        [Description("Event raised when the user enters a command and presses the Enter key")]
        public event CommandEventHandler Command;

        protected virtual void OnCommand(CommandEventArgs e)
        {
            if (Command != null)
                Command(this, e);
        }

        [Description("Event raised on KeyPress in input area")]
        public event CommandEnteringEventHandler CommandEntering;

        protected virtual void OnCommandEntering(CommandEnteringEventArgs e)
        {
            if (CommandEntering != null)
                CommandEntering(this, e);
        }

        [Description("Event raised when user enters a parameter")]
        public event ParameterEnteredEventHandler ParameterEntered;

        protected virtual void OnParameterEntered(ParameterEnteredEventArgs e)
        {
            if (ParameterEntered != null)
                ParameterEntered(this, e);
        }

        #endregion
    }

    public class CommandEventArgs : EventArgs
    {
        private readonly CommandPrompt _cp;

        public CommandEventArgs(string cmd, string[] param, CommandPrompt cp)
        {
            Cancel = false;
            Command = cmd;
            Message = "";
            Record = true;
            Parameters = param;
            _cp = cp;
        }

        [Description("Indicates whether command will be displayed in messages list")]
        public bool Cancel { get; set; }

        [Description("The command entered by the user")]
        public string Command { get; set; }

        [Description("Message to be displayed to user in response to command")]
        public string Message { get; set; }

        [Description("Indicates whether the command should be stored for AutoComplete")]
        public bool Record { get; set; }

        [Description("Parameters of the command")]
        public string[] Parameters { get; set; }

        public void Write(string s)
        {
            _cp.AddMessage(s);
        }
    }

    public class CommandEnteringEventArgs : EventArgs
    {
        public CommandEnteringEventArgs(string comm)
        {
            Command = comm;
            ToolTip = "";
        }

        [Description("Command in the input area")]
        public string Command { get; set; }

        [Description("Tooltip to be displayed")]
        public string ToolTip { get; set; }
    }

    public class ParameterEnteredEventArgs : EventArgs
    {
        public ParameterEnteredEventArgs(string cmd, string[] list)
        {
            Command = cmd;
            Parameters = list;
            EnterKey = false;
            ToolTip = "";
        }

        public ParameterEnteredEventArgs(string cmd, string[] list, bool enter)
        {
            Command = cmd;
            Parameters = list;
            EnterKey = enter;
            ToolTip = "";
        }

        [Description("Command entered currently")]
        public string Command { get; set; }


        [Description("List of paramters entered")]
        public string[] Parameters { get; set; }

        [Description("Determines whether the enter key has been pressed")]
        public bool EnterKey { get; set; }

        [Description("ToolTip to be shown")]
        public string ToolTip { get; set; }
    }
}