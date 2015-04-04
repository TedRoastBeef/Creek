using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MVCSharp.Examples.AdvancedCustomization.ApplicationLogic;

namespace MVCSharp.Examples.AdvancedCustomization.Presentation
{
    [ViewEx(typeof(MainTask), MainTask.PinnedNote, "Post")]
    public partial class PinnedNoteView : NoteView
    {
        public PinnedNoteView()
        {
            InitializeComponent();
        }
    }
}

