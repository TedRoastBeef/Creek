using System;
using System.Windows.Forms;

namespace Creek.UI.Vista
{
    internal class CueTextBox : TextBox
    {
        private string cuetext_ = "";

        public string CueText
        {
            get { return cuetext_; }
            set
            {
                cuetext_ = value;
                SetCueText(cuetext_);
            }
        }

        public void SetCueText(string Cue_Text)
        {
            VistaConstants.SendMessage(Handle, VistaConstants.EM_SETCUEBANNER, IntPtr.Zero, Cue_Text);
        }
    }
}