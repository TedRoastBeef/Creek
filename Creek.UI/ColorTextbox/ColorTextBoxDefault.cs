using System;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace Creek.UI.ColorTextbox
{
    public class Position : Position<Line, Token>
    {
        public Position()
        {
        }

        public Position(int x)
            : base(x)
        {
        }

        public Position(Line l, int pos) : base(0, l, pos)
        {
        }

        public Position(int x, Line l, int pos)
            : base(x, l, pos)
        {
        }
    }

    /// <summary>
    /// This is the default implementation of the abstract generic base class ColorTextBox
    /// A ColorTextBox is an editor control similar to a TextBox. It supports most of the properties and methods
    /// that are found in the TextBoxBase class for editing, selection and caret handling, ... plus some 
    /// functionality of a RichTextBox.
    /// In contrast to a TextBox a ColorTextBox supports different colors and can only be used as a multiline
    /// text editor. With the ShowLineNumbers and ShowCaretInfo properties the user has the ability to add
    /// regions to the control that show the line numbers and information about the caret position.
    /// Additionally the ColorTextBox supports the concept of reserved areas which allows derived
    /// classes to add or remove user defined regions at the border of the control whose areas can be drawn
    /// by the user. See documentation in the base class.
    /// NOTE: The additional methods and properties that are defined in this implementation exist solely for
    /// compatability with the TextBox implementation in the .Net class library. Using these methods may not
    /// be the most efficient way to interact with the ColorTextBox - this applies specifically to methods 
    /// that work with absolute character positions and lengths!
    /// </summary>
    public class ColorTextBox : ColorTextBoxBase<Document, Line, Token>
    {
        /***** PROPERTIES and METHODS that provide compatablity to TextBoxBase: *****/

        /// <summary>
        /// NOTE: This property exists for compatability reasons with the .Net TextBox class and is very inefficient!
        /// Gets or sets the starting point of text selected in the text box.
        /// If no text is selected in the control, this property indicates the insertion point for new text.
        /// If you set this property to a location beyond the length of the text in the control, the selection 
        /// start position will be placed after the last character. When text is selected in the text box 
        /// control, changing this property might decrease the value of the SelectionLength property. 
        /// If the remaining text in the control after the position indicated by the SelectionStart property 
        /// is less than the value of the SelectionLength property, the value of the SelectionLength property 
        /// is automatically decreased. The value of the SelectionStart property never causes an increase in 
        /// the SelectionLength property.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Gets or sets the starting point of text selected in the text box.")]
        public int SelectionStart
        {
            get
            {
                if (selection != null && selection.From != null && selection.To != null)
                {
                    return document.GetCharPosition(selection.From);
                }
                else if (caret != null)
                {
                    return document.GetCharPosition(caret);
                }
                else return 0;
            }
            set
            {
                int newStart = value;
                if (newStart < 0) newStart = 0;
                if (selection != null && selection.From != null && selection.To != null)
                {
                    int len = SelectionLength;
                    setSelection(document.GetPosition(value), document.GetPosition(value + len));
                }
                else
                {
                    if (caret != null) removeCaret();
                    setCaret(document.GetPosition(value));
                }
            }
        }

        /// <summary>
        /// Gets or sets the number of characters selected in control. If there currently exists a selection its
        /// length will be trimmed or extended to the new length. If there is currently no selection the given
        /// amount of characters will be selected beginning at the current caret position. Setting the property
        /// to a negative value has no effect at all.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Gets or sets the number of characters selected in control.")]
        public int SelectionLength
        {
            get
            {
                int ret = 0;
                if (selection != null && selection.From != null && selection.To != null)
                {
                    ret = selection.From.DistanceTo(selection.To);
                    if (ret < 0) ret = 0;
                }
                return ret;
            }
            set
            {
                if (value < 0) return;
                if (selection != null && selection.From != null && selection.To != null)
                {
                    Position<Line, Token> newTo = Pos(selection.From.LinePos + value, selection.From.Line);
                    setSelection(selection.From, newTo);
                }
                else if (caret != null)
                {
                    Position<Line, Token> to = Pos(caret.LinePos + value, caret.Line);
                    setSelection(caret, to);
                }
            }
        }

        /// <summary>
        /// Loads a Unicode or standard ASCII text file into the ColorTextBox control. 
        /// </summary>
        /// <param name="path">Path to the file to load.</param>
        public void LoadFile(String path)
        {
            if (File.Exists(path))
            {
                try
                {
                    String text = File.ReadAllText(path);
                    Text = text;
                }
                catch (Exception ex)
                {
                    throw new IOException("Could not read from file: " + path, ex);
                }
            }
        }

        /// <summary>
        /// Saves the contents of the ColorTextBox to a Unicode file.
        /// </summary>
        /// <param name="path">The name and location of the file to save.</param>
        public void SaveFile(String path)
        {
            try
            {
                if (Text != null)
                {
                    File.WriteAllText(path, Text, Encoding.Unicode);
                }
            }
            catch (Exception ex)
            {
                throw new IOException("Could not write to file: " + path, ex);
            }
        }

        /// <summary>
        /// Selects a range of text in the text box. If the given start position does not exist nothing will be
        /// selected.
        /// </summary>
        /// <param name="start">The position of the first character in the current text selection within 
        /// the text box.</param>
        /// <param name="length">The number of characters to select.</param>
        public void Select(int start, int length)
        {
            Position<Line, Token> fromP = Pos(start, document.First);
            if (fromP != null)
            {
                Position<Line, Token> toP = Pos(fromP.LinePos + length, fromP.Line);
                if (toP != null)
                {
                    setSelection(fromP, toP);
                }
            }
        }

        /// <summary>
        /// Searches the text of a ColorTextBox control for the first instance of a character from a list of 
        /// characters.  
        /// </summary>
        /// <param name="search">Array of characters to search for.</param>
        /// <returns>Start position of the first occurence of any of the chars in the search array.</returns>
        public virtual int Find(char[] search)
        {
            return Find(search, 0);
        }

        /// <summary>
        ///  Searches the text of a ColorTextBox control, at a specific starting point, for the first instance of 
        /// a character from a list of characters.
        /// </summary>
        /// <param name="search">Array of characters to search for.</param>
        /// <param name="start">Start position at which to start searching.</param>
        /// <returns>Start position of the first occurence of any of the chars in the search array.</returns>
        public virtual int Find(char[] search, int start)
        {
            return Find(search, start, -1);
        }

        /// <summary>
        /// Searches a range of text in a ColorTextBox control for the first instance of a character from a list 
        /// of characters. 
        /// </summary>
        /// <param name="search">Array of characters to search for.</param>
        /// <param name="start">The location within the control's text at which to begin searching.</param>
        /// <param name="end">The location within the control's text at which to end searching.</param>
        /// <returns>Start position of the first occurence of any of the chars in the search array.</returns>
        public virtual int Find(char[] search, int start, int end)
        {
            if (search == null) return -1;
            if (start < 0) start = -1;
            if (end < 0) end = document.LastPos;
            if (end > start)
            {
                while (start <= end)
                {
                    char c = document.CharAt(start);
                    foreach (char sc in search)
                    {
                        if (c == sc) return start;
                    }
                    start++;
                }
            }
            return -1;
        }

        /// <summary>
        /// Searches the text in a ColorTextBox control for a string. 
        /// </summary>
        /// <param name="search">String to search for.</param>
        /// <returns>Start position of the first occurence of the search string.</returns>
        public int Find(String search)
        {
            var finds = ColorTextBoxFinds.None;
            return Find(search, finds);
        }

        /// <summary>
        /// Searches the text in a ColorTextBox control for a string with specific options applied to the search. 
        /// </summary>
        /// <param name="search">The string to search for</param>
        /// <param name="finds">A bitwise combination of the ColorTextBoxFinds values.</param>
        /// <returns>Start position of the first occurence of the search string.</returns>
        public int Find(String search, ColorTextBoxFinds finds)
        {
            return Find(search, 0, finds);
        }

        /// <summary>
        /// Searches the text in a ColorTextBox control for a string at a specific location within the control 
        /// and with specific options applied to the search.
        /// </summary>
        /// <param name="search">The string to search for.</param>
        /// <param name="start">The location within the control's text at which to begin searching.</param>
        /// <param name="finds"></param>
        /// <returns>Start position of the first occurence of the search string.</returns>
        public int Find(String search, int start, ColorTextBoxFinds finds)
        {
            return Find(search, start, -1, finds);
        }

        /// <summary>
        /// Searches the text in a RichTextBox control for a string within a range of text within the control 
        /// and with specific options applied to the search.  
        /// </summary>
        /// <param name="search">The string to search for.</param>
        /// <param name="start">The location within the control's text at which to begin searching.</param>
        /// <param name="end">The location within the control's text at which to end searching.</param>
        /// <param name="finds">A bitwise combination of the ColorTextBoxFinds values.</param>
        /// <returns>Start position of the first occurence of the search string.</returns>
        public int Find(String search, int start, int end, ColorTextBoxFinds finds)
        {
            if (start < 0) start = 0;
            if (end < 0) end = document.LastPos;
            bool backwards = ((finds & ColorTextBoxFinds.Reverse) == ColorTextBoxFinds.Reverse);
            if (end > start)
            {
                Position<Line, Token> found = null;
                if (backwards)
                {
                    Position<Line, Token> startP = document.GetPosition(end);
                    if (startP != null)
                    {
                        found = document.Find(search, startP, finds);
                    }
                    if (found != null)
                    {
                        int retPos = document.GetCharPosition(found);
                        if (retPos >= start) return retPos;
                    }
                }
                else
                {
                    Position<Line, Token> startP = document.GetPosition(start);
                    if (startP != null)
                    {
                        found = document.Find(search, startP, finds);
                    }
                    if (found != null)
                    {
                        int retPos = document.GetCharPosition(found);
                        if (retPos <= end) return retPos;
                    }
                }
            }
            return -1;
        }
    }
}