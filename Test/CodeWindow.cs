using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Creek.UI;
using Creek.UI.AutoCompleteMenu;

namespace Test
{
    public partial class CodeWindow : Form
    {
        public CodeWindow()
        {
            InitializeComponent();

            syntaxRichTextBox1.Settings.Comment = "//";
            syntaxRichTextBox1.Settings.CommentColor = Color.Green;
            syntaxRichTextBox1.Settings.EnableComments = true;
            syntaxRichTextBox1.Settings.EnableIntegers = true;
            syntaxRichTextBox1.Settings.EnableStrings = true;
            syntaxRichTextBox1.Settings.IntegerColor = Color.Red;

            var sl = new List<SyntaxList>
                         {
                             new SyntaxList("function|if|else|do|for|break|case|typeof|var", Color.Blue),
                             new SyntaxList("new", Color.Blue,
                                            new Font(syntaxRichTextBox1.SelectionFont, FontStyle.Italic)),
                             new SyntaxList("Resources|StringBuilder|DialogBuilder|WebClient|Form|Process|Environment",
                                            Color.CadetBlue),
                             new SyntaxList("FormBorderStyle|FormWindowState|Window", Color.Black),
                         };

            syntaxRichTextBox1.Settings.Keywords.AddRange(sl);
            syntaxRichTextBox1.Settings.StringColor = Color.Gray;

            syntaxRichTextBox1.CompileKeywords();
            syntaxRichTextBox1.ProcessAllLines();

            var keys = new List<string>
                           {
                               "Resources",
                               "StringBuilder",
                               "DialogBuilder",
                               "WebClient",
                               "Process",
                               "Form",
                               "Environment",
                               "function",
                               "if",
                               "else",
                               "do",
                               "for",
                               "break",
                               "case",
                               "typeof",
                               "var",
                               "new",
                               "require",
                               "eval",
                               "wait",
                               "auth",
                               "alert",
                               "inputbox"
                           };

            List<AutocompleteItem> acbi = keys.Select(key => new AutocompleteItem(key)).ToList();

            syntaxRichTextBox1.Intelisense.SetAutocompleteItems(acbi);

            syntaxRichTextBox1.Intelisense.SetAutocompleteMenu(syntaxRichTextBox1, syntaxRichTextBox1.Intelisense);
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void CodeWindow_Load(object sender, EventArgs e)
        {
            syntaxRichTextBox1.ExtendWithContextMenu();
        }
    }
}