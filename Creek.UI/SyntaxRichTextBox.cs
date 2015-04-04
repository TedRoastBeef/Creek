using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Creek.UI.AutoCompleteMenu;

namespace Creek.UI
{
    public class SyntaxRichTextBox : RichTextBox
    {
        private static bool m_bPaint = true;
        private readonly SyntaxSettings m_settings = new SyntaxSettings();
        public AutocompleteMenu Intelisense;
        private int m_nContentLength;
        private int m_nCurSelection;
        private int m_nLineEnd;
        private int m_nLineLength;
        private int m_nLineStart;
        private string m_strKeywords = "";
        private string m_strLine = "";

        public SyntaxRichTextBox()
        {
            Intelisense = new AutocompleteMenu();
        }

        /// <summary>
        /// The settings.
        /// </summary>
        public SyntaxSettings Settings
        {
            get { return m_settings; }
        }

        #region Interop-Defines

        private const int WM_USER = 0x0400;
        private const int EM_GETCHARFORMAT = WM_USER + 58;
        private const int EM_SETCHARFORMAT = WM_USER + 68;

        private const int SCF_SELECTION = 0x0001;
        private const int SCF_WORD = 0x0002;
        private const int SCF_ALL = 0x0004;

        #region CHARFORMAT2 Flags

        private const UInt32 CFE_BOLD = 0x0001;
        private const UInt32 CFE_ITALIC = 0x0002;
        private const UInt32 CFE_UNDERLINE = 0x0004;
        private const UInt32 CFE_STRIKEOUT = 0x0008;
        private const UInt32 CFE_PROTECTED = 0x0010;
        private const UInt32 CFE_LINK = 0x0020;
        private const UInt32 CFE_AUTOCOLOR = 0x40000000;
        private const UInt32 CFE_SUBSCRIPT = 0x00010000; /* Superscript and subscript are */
        private const UInt32 CFE_SUPERSCRIPT = 0x00020000; /*  mutually exclusive			 */

        private const int CFM_SMALLCAPS = 0x0040; /* (*)	*/
        private const int CFM_ALLCAPS = 0x0080; /* Displayed by 3.0	*/
        private const int CFM_HIDDEN = 0x0100; /* Hidden by 3.0 */
        private const int CFM_OUTLINE = 0x0200; /* (*)	*/
        private const int CFM_SHADOW = 0x0400; /* (*)	*/
        private const int CFM_EMBOSS = 0x0800; /* (*)	*/
        private const int CFM_IMPRINT = 0x1000; /* (*)	*/
        private const int CFM_DISABLED = 0x2000;
        private const int CFM_REVISED = 0x4000;

        private const int CFM_BACKCOLOR = 0x04000000;
        private const int CFM_LCID = 0x02000000;
        private const int CFM_UNDERLINETYPE = 0x00800000; /* Many displayed by 3.0 */
        private const int CFM_WEIGHT = 0x00400000;
        private const int CFM_SPACING = 0x00200000; /* Displayed by 3.0	*/
        private const int CFM_KERNING = 0x00100000; /* (*)	*/
        private const int CFM_STYLE = 0x00080000; /* (*)	*/
        private const int CFM_ANIMATION = 0x00040000; /* (*)	*/
        private const int CFM_REVAUTHOR = 0x00008000;


        private const UInt32 CFM_BOLD = 0x00000001;
        private const UInt32 CFM_ITALIC = 0x00000002;
        private const UInt32 CFM_UNDERLINE = 0x00000004;
        private const UInt32 CFM_STRIKEOUT = 0x00000008;
        private const UInt32 CFM_PROTECTED = 0x00000010;
        private const UInt32 CFM_LINK = 0x00000020;
        private const UInt32 CFM_SIZE = 0x80000000;
        private const UInt32 CFM_COLOR = 0x40000000;
        private const UInt32 CFM_FACE = 0x20000000;
        private const UInt32 CFM_OFFSET = 0x10000000;
        private const UInt32 CFM_CHARSET = 0x08000000;
        private const UInt32 CFM_SUBSCRIPT = CFE_SUBSCRIPT | CFE_SUPERSCRIPT;
        private const UInt32 CFM_SUPERSCRIPT = CFM_SUBSCRIPT;

        private const byte CFU_UNDERLINENONE = 0x00000000;
        private const byte CFU_UNDERLINE = 0x00000001;
        private const byte CFU_UNDERLINEWORD = 0x00000002; /* (*) displayed as ordinary underline	*/
        private const byte CFU_UNDERLINEDOUBLE = 0x00000003; /* (*) displayed as ordinary underline	*/
        private const byte CFU_UNDERLINEDOTTED = 0x00000004;
        private const byte CFU_UNDERLINEDASH = 0x00000005;
        private const byte CFU_UNDERLINEDASHDOT = 0x00000006;
        private const byte CFU_UNDERLINEDASHDOTDOT = 0x00000007;
        private const byte CFU_UNDERLINEWAVE = 0x00000008;
        private const byte CFU_UNDERLINETHICK = 0x00000009;
        private const byte CFU_UNDERLINEHAIRLINE = 0x0000000A; /* (*) displayed as ordinary underline	*/

        #endregion

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        [StructLayout(LayoutKind.Sequential)]
        private struct CHARFORMAT2_STRUCT
        {
            public UInt32 cbSize;
            public UInt32 dwMask;
            public UInt32 dwEffects;
            public readonly Int32 yHeight;
            public readonly Int32 yOffset;
            public readonly Int32 crTextColor;
            public readonly byte bCharSet;
            public readonly byte bPitchAndFamily;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)] public char[] szFaceName;
            public readonly UInt16 wWeight;
            public readonly UInt16 sSpacing;
            public readonly int crBackColor; // Color.ToArgb() -> int
            public readonly int lcid;
            public readonly int dwReserved;
            public readonly Int16 sStyle;
            public readonly Int16 wKerning;
            public readonly byte bUnderlineType;
            public readonly byte bAnimation;
            public readonly byte bRevAuthor;
            public readonly byte bReserved1;
        }

        #endregion

        public void ExtendWithContextMenu()
        {
            var ctx = new ContextMenuStrip();

            var cut = new ToolStripMenuItem(
                "Ausschneiden", null, (sender, e) => Cut());

            var copy = new ToolStripMenuItem(
                "Kopieren", null, (sender, e) => Copy());

            var paste = new ToolStripMenuItem(
                "Einfügen", null, (sender, e) => Paste());

            ctx.Items.Add(cut);
            ctx.Items.Add(copy);
            ctx.Items.Add(paste);

            ContextMenuStrip = ctx;

            ctx.Opening +=
                (sender, e) =>
                    {
                        bool noSelectedText = string.IsNullOrEmpty(SelectedText);
                        cut.Enabled = !noSelectedText;
                        copy.Enabled = !noSelectedText;

                        bool noClipboardText = string.IsNullOrEmpty(Clipboard.GetText());
                        paste.Enabled = !noClipboardText;
                    };
        }

        public void InsertLink(string text)
        {
            InsertLink(text, SelectionStart);
        }

        /// <summary>
        /// Insert a given text at a given position as a link. 
        /// </summary>
        /// <param name="text">Text to be inserted</param>
        /// <param name="position">Insert position</param>
        public void InsertLink(string text, int position)
        {
            if (position < 0 || position > Text.Length)
                throw new ArgumentOutOfRangeException("position");

            SelectionStart = position;
            SelectedText = text;
            Select(position, text.Length);
            SetSelectionLink(true);
            Select(position + text.Length, 0);
        }

        /// <summary>
        /// Insert a given text at at the current input position as a link.
        /// The link text is followed by a hash (#) and the given hyperlink text, both of
        /// them invisible.
        /// When clicked on, the whole link text and hyperlink string are given in the
        /// LinkClickedEventArgs.
        /// </summary>
        /// <param name="text">Text to be inserted</param>
        /// <param name="hyperlink">Invisible hyperlink string to be inserted</param>
        public void InsertLink(string text, string hyperlink)
        {
            InsertLink(text, hyperlink, SelectionStart);
        }

        /// <summary>
        /// Insert a given text at a given position as a link. The link text is followed by
        /// a hash (#) and the given hyperlink text, both of them invisible.
        /// When clicked on, the whole link text and hyperlink string are given in the
        /// LinkClickedEventArgs.
        /// </summary>
        /// <param name="text">Text to be inserted</param>
        /// <param name="hyperlink">Invisible hyperlink string to be inserted</param>
        /// <param name="position">Insert position</param>
        public void InsertLink(string text, string hyperlink, int position)
        {
            if (position < 0 || position > Text.Length)
                throw new ArgumentOutOfRangeException("position");

            SelectionStart = position;
            SelectedRtf = @"{\rtf1\ansi " + text + @"\v #" + hyperlink + @"\v0}";
            Select(position, text.Length + hyperlink.Length + 1);
            SetSelectionLink(true);
            Select(position + text.Length + hyperlink.Length + 1, 0);
        }

        /// <summary>
        /// Set the current selection's link style
        /// </summary>
        /// <param name="link">true: set link style, false: clear link style</param>
        public void SetSelectionLink(bool link)
        {
            SetSelectionStyle(CFM_LINK, link ? CFE_LINK : 0);
        }

        /// <summary>
        /// Get the link style for the current selection
        /// </summary>
        /// <returns>0: link style not set, 1: link style set, -1: mixed</returns>
        public int GetSelectionLink()
        {
            return GetSelectionStyle(CFM_LINK, CFE_LINK);
        }


        private void SetSelectionStyle(UInt32 mask, UInt32 effect)
        {
            var cf = new CHARFORMAT2_STRUCT();
            cf.cbSize = (UInt32) Marshal.SizeOf(cf);
            cf.dwMask = mask;
            cf.dwEffects = effect;

            var wpar = new IntPtr(SCF_SELECTION);
            IntPtr lpar = Marshal.AllocCoTaskMem(Marshal.SizeOf(cf));
            Marshal.StructureToPtr(cf, lpar, false);

            IntPtr res = SendMessage(Handle, EM_SETCHARFORMAT, wpar, lpar);

            Marshal.FreeCoTaskMem(lpar);
        }

        private int GetSelectionStyle(UInt32 mask, UInt32 effect)
        {
            var cf = new CHARFORMAT2_STRUCT();
            cf.cbSize = (UInt32) Marshal.SizeOf(cf);
            cf.szFaceName = new char[32];

            var wpar = new IntPtr(SCF_SELECTION);
            IntPtr lpar = Marshal.AllocCoTaskMem(Marshal.SizeOf(cf));
            Marshal.StructureToPtr(cf, lpar, false);

            IntPtr res = SendMessage(Handle, EM_GETCHARFORMAT, wpar, lpar);

            cf = (CHARFORMAT2_STRUCT) Marshal.PtrToStructure(lpar, typeof (CHARFORMAT2_STRUCT));

            int state;
            // dwMask holds the information which properties are consistent throughout the selection:
            if ((cf.dwMask & mask) == mask)
            {
                if ((cf.dwEffects & effect) == effect)
                    state = 1;
                else
                    state = 0;
            }
            else
            {
                state = -1;
            }

            Marshal.FreeCoTaskMem(lpar);
            return state;
        }

        /// <summary>
        /// WndProc
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x00f)
            {
                if (m_bPaint)
                    base.WndProc(ref m);
                else
                    m.Result = IntPtr.Zero;
            }
            else
                base.WndProc(ref m);
        }

        /// <summary>
        /// OnTextChanged
        /// </summary>
        /// <param name="e"></param>
        protected override void OnTextChanged(EventArgs e)
        {
            // Calculate shit here.
            m_nContentLength = TextLength;

            int nCurrentSelectionStart = SelectionStart;
            int nCurrentSelectionLength = SelectionLength;

            m_bPaint = false;

            // Find the start of the current line.
            m_nLineStart = nCurrentSelectionStart;
            while ((m_nLineStart > 0) && (Text[m_nLineStart - 1] != '\n'))
                m_nLineStart--;
            // Find the end of the current line.
            m_nLineEnd = nCurrentSelectionStart;
            while ((m_nLineEnd < Text.Length) && (Text[m_nLineEnd] != '\n'))
                m_nLineEnd++;
            // Calculate the length of the line.
            m_nLineLength = m_nLineEnd - m_nLineStart;
            // Get the current line.
            m_strLine = Text.Substring(m_nLineStart, m_nLineLength);

            // Process this line.
            ProcessLine();

            m_bPaint = true;
        }

        /// <summary>
        /// Process a line.
        /// </summary>
        private void ProcessLine()
        {
            // Save the position and make the whole line black
            int nPosition = SelectionStart;
            SelectionStart = m_nLineStart;
            SelectionLength = m_nLineLength;
            SelectionColor = Color.Black;

            // Process the keywords
            foreach (SyntaxList mStrKeyword in m_settings.Keywords)
            {
                ProcessRegex(mStrKeyword.Keyword, mStrKeyword.Color, mStrKeyword.FontStyle);
            }

            // Process numbers
            if (Settings.EnableIntegers)
                ProcessRegex("\\b(?:[0-9]*\\.)?[0-9]+\\b", Settings.IntegerColor);
            // Process strings
            if (Settings.EnableStrings)
                ProcessRegex("\"[^\"\\\\\\r\\n]*(?:\\\\.[^\"\\\\\\r\\n]*)*\"", Settings.StringColor);
            // Process comments
            if (Settings.EnableComments && !string.IsNullOrEmpty(Settings.Comment))
                ProcessRegex(Settings.Comment + ".*$", Settings.CommentColor);

            SelectionStart = nPosition;
            SelectionLength = 0;
            SelectionColor = Color.Black;

            m_nCurSelection = nPosition;
        }

        /// <summary>
        /// Process a regular expression.
        /// </summary>
        /// <param name="strRegex">The regular expression.</param>
        /// <param name="color">The color.</param>
        /// <param name="font"> </param>
        public void ProcessRegex(string strRegex, Color color, Font font = null)
        {
            var regKeywords = new Regex(strRegex, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            Match regMatch;

            for (regMatch = regKeywords.Match(m_strLine); regMatch.Success; regMatch = regMatch.NextMatch())
            {
                // Process the words
                int nStart = m_nLineStart + regMatch.Index;
                int nLenght = regMatch.Length;
                SelectionStart = nStart;
                SelectionLength = nLenght;
                if (font != null)
                    SelectionFont = font;
                SelectionColor = color;
            }
        }

        /// <summary>
        /// Compiles the keywords as a regular expression.
        /// </summary>
        public void CompileKeywords()
        {
            for (int i = 0; i < Settings.Keywords.Count; i++)
            {
                string strKeyword = Settings.Keywords[i].Keyword;

                if (i == Settings.Keywords.Count - 1)
                    m_strKeywords += "\\b" + strKeyword + "\\b";
                else
                    m_strKeywords += "\\b" + strKeyword + "\\b|";
            }
        }

        public void ProcessAllLines()
        {
            m_bPaint = false;

            int nStartPos = 0;
            int i = 0;
            int nOriginalPos = SelectionStart;
            while (i < Lines.Length)
            {
                m_strLine = Lines[i];
                m_nLineStart = nStartPos;
                m_nLineEnd = m_nLineStart + m_strLine.Length;

                ProcessLine();
                i++;

                nStartPos += m_strLine.Length + 1;
            }

            m_bPaint = true;
        }
    }

    /// <summary>
    /// Class to store syntax objects in.
    /// </summary>
    public class SyntaxList
    {
        public SyntaxList(string keyword, Color color, Font fs = null)
        {
            Keyword = keyword;
            Color = color;
            FontStyle = fs;
        }

        public string Keyword { get; set; }
        public Color Color { get; set; }
        public Font FontStyle { get; set; }
    }

    /// <summary>
    /// Settings for the keywords and colors.
    /// </summary>
    public class SyntaxSettings
    {
        private readonly List<SyntaxList> m_rgKeywords = new List<SyntaxList>();
        private bool m_bEnableComments = true;
        private bool m_bEnableIntegers = true;
        private bool m_bEnableStrings = true;
        private Color m_colorComment = Color.Green;
        private Color m_colorInteger = Color.Red;
        private Color m_colorString = Color.Gray;
        private string m_strComment = "";

        #region Properties

        /// <summary>
        /// A list containing all keywords.
        /// </summary>
        public List<SyntaxList> Keywords
        {
            get { return m_rgKeywords; }
        }

        /// <summary>
        /// A string containing the comment identifier.
        /// </summary>
        public string Comment
        {
            get { return m_strComment; }
            set { m_strComment = value; }
        }

        /// <summary>
        /// The color of comments.
        /// </summary>
        public Color CommentColor
        {
            get { return m_colorComment; }
            set { m_colorComment = value; }
        }

        /// <summary>
        /// Enables processing of comments if set to true.
        /// </summary>
        public bool EnableComments
        {
            get { return m_bEnableComments; }
            set { m_bEnableComments = value; }
        }

        /// <summary>
        /// Enables processing of integers if set to true.
        /// </summary>
        public bool EnableIntegers
        {
            get { return m_bEnableIntegers; }
            set { m_bEnableIntegers = value; }
        }

        /// <summary>
        /// Enables processing of strings if set to true.
        /// </summary>
        public bool EnableStrings
        {
            get { return m_bEnableStrings; }
            set { m_bEnableStrings = value; }
        }

        /// <summary>
        /// The color of strings.
        /// </summary>
        public Color StringColor
        {
            get { return m_colorString; }
            set { m_colorString = value; }
        }

        /// <summary>
        /// The color of integers.
        /// </summary>
        public Color IntegerColor
        {
            get { return m_colorInteger; }
            set { m_colorInteger = value; }
        }

        #endregion
    }
}