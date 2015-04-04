using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Creek.UI.ColorTextbox
{
    /// <summary>
    /// This interface is implemented by the abstract generic base class ColorTextBox. 
    /// It can be used as non-generic base type for methods that wish to work with different concrete 
    /// implementations of a ColorTextbox.
    /// The interface declares only public methods and properties that have non-generic parameters.
    /// </summary>
    public interface IColorTextBox
    {
        /// <summary>
        /// Gets or sets the document contained in the ColorTextBox. (Represents the data model)
        /// </summary>
        IDocument Document { get; set; }

        /// <summary>
        /// Gets or sets the file name associated to the current ColorTextBox instance.
        /// </summary>
        [Browsable(false)]
        String FileName { get; set; }

        /// <summary>
        /// Gets or sets the full path of the file associated to the current ColorTextBox instance.
        /// </summary>
        [Browsable(false)]
        String FilePath { get; set; }

        /// <summary>
        /// Gets or sets the current caret position. Sets the caret to the specified position and ensures 
        /// that the caret is visible by scrolling to the new position. If a null reference is passed as 
        /// position the caret will be removed!
        /// Changing the caret position causes any current selection to be removed!
        /// </summary>
        IPosition Caret { get; set; }

        /// <summary>
        /// Gets or sets the Text contained in the ColorTextBox. When setting the Text the constructed lines
        /// will only consist of 2 tokens: one containing the text of the line and a newline token.
        /// </summary>
        [Browsable(true)]
        [DefaultValue(null)]
        String Text { get; set; }

        /// <summary>
        /// Gets or sets the lines in the ColorTextBox (without newline 
        /// characters at the end of each string!)
        /// </summary>
        [Browsable(true)]
        [DefaultValue(new String[0])]
        String[] Lines { get; set; }

        /// <summary>
        /// Gets or sets the selected text within the text box.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        String SelectedText { get; set; }

        /// <summary>
        /// Gets the text in the text box as an rtf formatted string.
        /// </summary>
        String Rtf { get; }

        /// <summary>
        /// Gets the currently selected text in the text box as an rtf formatted string.
        /// </summary>
        String SelectedRtf { get; }

        /// <summary>
        /// Gets the current selection start position. If there currently is no selection a null reference 
        /// is returned.
        /// </summary>
        IPosition SelectionFromPos { get; }

        /// <summary>
        /// Gets the current selection end position. If there currently is no selection a null reference is
        /// returned.
        /// </summary>
        IPosition SelectionToPos { get; }

        /// <summary>
        /// Gets or sets the text color of the current text selection or insertion point. If there is a 
        /// selection this property always returns the color at the end of the selection otherwise the
        /// color at the current caret position is returned!
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        Color SelectionColor { get; set; }

        /// <summary>
        /// Gets or sets the currently active color to be used when typing text.
        /// </summary>
        Color ForeColor { get; set; }

        /// <summary>
        /// Gets or sets the color of the caret.
        /// </summary>
        Color CaretColor { get; set; }

        /// <summary>
        /// Gets or sets the color of the text used for line number display.
        /// </summary>
        Color LineNumForeColor { get; set; }

        /// <summary>
        /// Gets or sets the background color of the area used for line number display.
        /// </summary>
        Color LineNumBackColor { get; set; }

        /// <summary>
        /// Gets or sets the color of the text used for caret info display.
        /// </summary>
        Color CaretInfoForeColor { get; set; }

        /// <summary>
        /// Gets or sets the background color of the area used for caret info display.
        /// </summary>
        Color CaretInfoBackColor { get; set; }

        /// <summary>
        /// Gets or sets the font that is used to display the line numbers. Setting the font to a font larger
        /// than the one currently used in the text box causes the height of the new font to be reset to the 
        /// height value of the text box's font.
        /// </summary>
        Font LineNumFont { get; set; }

        /// <summary>
        /// Gets or sets the font to be used for drawing caret position info.
        /// </summary>
        Font CaretInfoFont { get; set; }

        /// <summary>
        /// Returns the length of text contained in the ColorTextBox.
        /// </summary>
        int TextLength { get; }

        /// <summary>
        /// Returns the number of lines contained in the ColorTextBox.
        /// </summary>
        int LineCount { get; }

        /// <summary>
        /// Returns the line number of the first visible line.
        /// </summary>
        int FirstVisLineNumber { get; }

        /// <summary>
        /// Returns the DataFormat String of a ColorTextBox instance used to store and retrieve data from 
        /// the Clipboard in an application specific format.
        /// </summary>
        String ColorTextBoxFormat { get; }

        /// <summary>
        /// Returns true if there is currently a caret set in the ColorTextBox.
        /// </summary>
        bool IsCaretSet { get; }

        /// <summary>
        /// Returns true if there currently exists a selection.
        /// </summary>
        bool IsTextSelected { get; }

        /// <summary>
        /// Gets or sets a value that indicates that the text box control has been modified by the user 
        /// since the control was created or its contents were last set. 
        /// </summary>
        bool Modified { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether text in the text box is read-only. Read-only means that
        /// the content of the text box can only be modified programmatically, i.e. the control does not react
        /// to user input that would change the content!
        /// </summary>
        bool ReadOnly { get; set; }

        /// <summary>
        /// Turns rendering of line numbers on the left edge of the control on or off.
        /// </summary>
        bool ShowLineNumbers { get; set; }

        /// <summary>
        /// Turns rendering of caret position information area at the bottom of the control on or off.
        /// </summary>
        bool ShowCaretInfo { get; set; }

        /// <summary>
        /// Gets a value indicating whether the user can undo the previous operation in a text box control.
        /// </summary>
        bool CanUndo { get; }

        /// <summary>
        /// Gets a value indicating whether there are actions that have occurred within the text box that 
        /// can be reapplied.
        /// </summary>
        bool CanRedo { get; }

        /// <summary>
        /// Gets or sets a value indicating whether control's elements are aligned to support locales using 
        /// right-to-left fonts. This property is not supported by ColorTextBox and therefore changing it has no
        /// effect.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(0)]
        RightToLeft RightToLeft { get; set; }

        /// <summary>
        /// The padding property is not supported in the ColorTextBox control!
        /// </summary>
        [Browsable(false)]
        Padding Padding { get; }

        /// <summary>
        /// Gets or sets the size that is the lower limit that System.Windows.Forms.Control.GetPreferredSize(System.Drawing.Size)
        /// can specify.
        /// </summary>
        Size MinimumSize { get; set; }

        /// <summary>
        /// Returns the Rectangle on which lines can be drawn.
        /// </summary>
        Rectangle TextArea { get; }

        /// <summary>
        /// If there are scrollbars visible in the ColorTextBox this property returns the ClientRectangle without
        /// the width or height of the scrollbars. If there are no scrollbars visible the returned Rectangle is
        /// equal to the Rectangle returned by the ClientRectangle property.
        /// </summary>
        Rectangle ClientScrollRectangle { get; }

        /// <summary>
        /// Gets the current line height for the ColorTextBox (depending on set font)
        /// </summary>
        int LineHeight { get; }

        /// <summary>
        /// Returns the distance from the top of a line to the baseline.
        /// </summary>
        int BaseLineDistance { get; }

        /// <summary>
        /// Returns a Region that contains all reserved areas (useful for repainting,...)
        /// </summary>
        Region ReservedAreas { get; }

        /// <summary>
        /// Gets the sum of all reserved widths on the left side of the client area.
        /// </summary>
        int ReservedLeftWidths { get; }

        /// <summary>
        /// Gets the sum of all reserved widths on the right side of the client area.
        /// </summary>
        int ReservedRightWidths { get; }

        /// <summary>
        /// Gets the sum of all reserved heights at the top of the client area.
        /// </summary>
        int ReservedTopWidths { get; }

        /// <summary>
        /// Gets the sum of all reserved heights at the bottom of the client area.
        /// </summary>
        int ReservedBottomWidths { get; }

        event MessageEventHandler GUIMessageEvent;

        /// <summary>
        /// Sets the rendering mode to the provided parameter.
        /// </summary>
        /// <param name="mode">The new rendering mode to apply</param>
        void SetRenderMode(RenderMode mode);

        /// <summary>
        /// Suspends all further painting to the screen. After calling this method all changes to the documents data
        /// will only be drawn to the background buffer until ResumePainting() is called.
        /// </summary>
        void SuspendPainting();

        /// <summary>
        /// Resumes painting to the screen and optionally draws all pending changes immediately.
        /// </summary>
        /// <param name="paintPending">When true all pending changes are painted immediately</param>
        void ResumePainting(bool paintPending);

        /// <summary>
        /// Tries to undo the last operation in the undo buffer and moves it to the redo buffer.
        /// Override when implementing own UpdateEventArgs!
        /// </summary>
        /// <returns>True on success.</returns>
        bool Undo();

        /// <summary>
        /// Removes all available undo operations from the undo buffer.
        /// </summary>
        void ClearUndo();

        /// <summary>
        /// Tries to redo the last undo operation in the redo buffer and adds it to the undo buffer again.
        /// Override when implementing own UpdateEventArgs!
        /// </summary>
        /// <returns></returns>
        bool Redo();

        /// <summary>
        /// Copies the currently selected text to the platforms' clipboard. Returns true on success or false
        /// if there isn't a valid selection to copy. This implementation places the selected text in the
        /// following data formats into the clipboard:
        /// ColorTextBoxFormat, StringFormat, UnicodeText and Rtf.
        /// </summary>
        /// <returns>True on success. False if there is no selection.</returns>
        bool Copy();

        /// <summary>
        /// Tries to insert text from the system clipboard into the document at the current caret position.
        /// If there is currently some text selected it will be deleted prior to insertion.
        /// This method searches the clipboard for the following text formats and inserts the first one 
        /// encountered: ColorTextBoxFormat, StringFormat, UnicodeText and Text.
        /// </summary>
        /// <returns>True on success. False if there is no suitable data found in the clipboard.</returns>
        bool Paste();

        /// <summary>
        /// Returns a boolean value indicating if text of the given format can be inserted into the
        /// ColorTextBox.
        /// </summary>
        /// <param name="format">Format to check.</param>
        /// <returns>True if content of the given format can be inserted.</returns>
        bool CanPaste(string format);

        /// <summary>
        /// Moves the currently selected text to the clipboard. Returns true on success or false if there
        /// isn't a valid selection to cut. This implementation places the selected text in the following
        /// data formats into the clipboard:
        /// ColorTextBoxFormat, StringFormat, UnicodeText and Rtf.
        /// </summary>
        /// <returns></returns>
        bool Cut();

        /// <summary>
        /// Deletes all text contained in the ColorTextBox.
        /// </summary>
        void Clear();

        /// <summary>
        /// Appends the given string to the end of the ColorTextBox. Use this method instead of appending 
        /// text to the Text property with the "+" operator!
        /// </summary>
        /// <param name="text">The text to append at the end of the text box.</param>
        void AppendText(String text);

        /// <summary>
        /// Inserts the given text at the current caret position. If there exists a selection the selected
        /// text will be replaced with the given string.
        /// If there is neither a caret nor a selection no changes will be made.
        /// </summary>
        /// <param name="text">The text to insert into the text box.</param>
        void Insert(String text);

        /// <summary>
        /// Removes the caret if there currently is any.
        /// </summary>
        void RemoveCaret();

        /// <summary>
        /// Ensures that the line containing the current caret position is visible.
        /// </summary>
        void ScrollToCaret();

        /// <summary>
        /// Scrolls to the line at the given index if it is currently not visible
        /// </summary>
        /// <param name="index">Index of the line to scroll to</param>
        void ScrollToLine(int index);

        /// <summary>
        /// Scrolls to the given line if it is currently not visible
        /// </summary>
        /// <param name="line">Line to scroll to</param>
        void ScrollToLine(ILine line);

        /// <summary>
        /// Selects all text in the text box. 
        /// </summary>
        void SelectAll();

        /// <summary>
        /// Specifies that the value of the SelectionLength property is zero so that no characters are 
        /// selected in the control. 
        /// </summary>
        void DeselectAll();

        /// <summary>
        /// Deletes the text contained between the given positions.
        /// </summary>
        /// <param name="from">Start position of deletion.</param>
        /// <param name="to">End position of deletion.</param>
        void Delete(IPosition from, IPosition to);

        /// <summary>
        /// Deletes the given amount of characters 
        /// </summary>
        /// <param name="start">Start position of deletion.</param>
        /// <param name="length">Number of chars to delete.</param>
        void Delete(IPosition start, int length);

        /// <summary>
        /// Replaces the text between the given positions with the given string.
        /// </summary>
        /// <param name="from">Start position of replacing.</param>
        /// <param name="to">End position of replacing.</param>
        /// <param name="text">String to replace with.</param>
        void Replace(IPosition from, IPosition to, string text);

        /// <summary>
        /// Selects the text between the given positions.
        /// </summary>
        /// <param name="from">Start position of the text to select.</param>
        /// <param name="to">End position of the text to select.</param>
        void Select(IPosition from, IPosition to);

        /// <summary>
        /// Selects the given amount of postions following the given start position.
        /// </summary>
        /// <param name="start">Start position of the text to select.</param>
        /// <param name="length">Number of positions to select.</param>
        void Select(IPosition start, int length);

        /// <summary>
        /// Searches the text in a ColorTextBox control for a string. 
        /// </summary>
        /// <param name="search">String to search for.</param>
        /// <returns>Start position of the first occurence of the search string.</returns>
        IPosition Search(String search);

        /// <summary>
        /// Searches the text in a ColorTextBox control for a string with specific options applied to the search. 
        /// </summary>
        /// <param name="search">The string to search for</param>
        /// <param name="finds">A bitwise combination of the ColorTextBoxFinds values.</param>
        /// <returns>Start position of the first occurence of the search string.</returns>
        IPosition Search(String search, ColorTextBoxFinds finds);

        /// <summary>
        /// Searches the text in a ColorTextBox control for a string at a specific location within the control 
        /// and with specific options applied to the search.
        /// </summary>
        /// <param name="search">The string to search for.</param>
        /// <param name="start">The location within the control's text at which to begin searching.</param>
        /// <param name="finds">A bitwise combination of the ColorTextBoxFinds values.</param>
        /// <returns>Start position of the first occurence of the search string.</returns>
        IPosition Search(String search, IPosition start, ColorTextBoxFinds finds);

        /// <summary>
        /// Searches the text in a ColorTextBox control for a given string between the given positions.
        /// </summary>
        /// <param name="search">The string to search for.</param>
        /// <param name="start">Start position of search.</param>
        /// <param name="end">End position of search.</param>
        /// <param name="finds">A bitwise combination of the ColorTextBoxFinds values.</param>
        /// <returns>Start position of the first occurence of the search string or null if nothing was found.</returns>
        IPosition Search(String search, IPosition start, IPosition end, ColorTextBoxFinds finds);

        /// <summary>
        /// Returns the position at the specified line index and line position. If there is no line at the
        /// given index the method returns null. If linePos is not part of the given line (if it is negative
        /// or greater than the last position of the line) the respective following or previous line that 
        /// contains the position will be returned.
        /// NOTE: The returned position may not be visible!
        /// </summary>
        /// <param name="lineIndex">Index of the line containing the position.</param>
        /// <param name="linePos">Position inside the given line.</param>
        /// <returns>The Position object for the specified line and line position.</returns>
        IPosition GetPosition(int lineIndex, int linePos);

        /// <summary>
        /// Returns the Position that is nearest to the given Point. If there are currently no
        /// visible lines in the control the method returns null!
        /// NOTE: If a position is found it is guaranteed to be visible!
        /// </summary>
        /// <param name="p">The Point for which to get the position.</param>
        /// <returns></returns>
        IPosition GetPosition(Point p);

        /// <summary>
        /// This non-generic method returns the number of the passed line inside the document iff it is
        /// of the correct type for this ColorTextBox implementation!
        /// If the passed line has the wrong type or the line isn't contained the method returns -1.
        /// </summary>
        /// <param name="line">Line for which to get the number.</param>
        /// <returns>The number of the given line or -1 if it is not contained.</returns>
        int GetLineNumber(ILine line);
    }

    /// <summary>
    /// This interface is implemented by the abstract generic base class TokenBase and can be used as a 
    /// non-generic base type for methods that wish to work with different implementations of TokenBase.
    /// The interface declares only public methods and properties that have non-generic parameters.
    /// </summary>
    public interface IToken
    {
        /// <summary>
        /// Gets or sets the value of this token.
        /// NOTE: Setting the value of a token directly will not trigger any update events!
        /// </summary>
        String Value { get; }

        /// <summary>
        /// Indexer for array-like access to the token value. Throws an IndexOutOfRangeException if the
        /// given index is out of range.
        /// </summary>
        /// <param name="index">Index of the character to set or get.</param>
        /// <returns>Character at the given index.</returns>
        char this[int index] { get; }

        /// <summary>
        /// Returns the length of this LineToken's value.
        /// </summary>
        int Length { get; }

        /// <summary>
        /// Returns true if this token is an end of line token.
        /// </summary>
        bool IsNewLineToken { get; }

        /// <summary>
        /// Gets or sets the color of this token.
        /// </summary>
        Color Color { get; set; }

        /// <summary>
        /// Gets or sets the rendering width of the current token - to be used by the rendering control.
        /// </summary>
        int Width { get; }

        /// <summary>
        /// Gets or sets the height of the current token - to be used by the rendering control.
        /// </summary>
        int Height { get; }

        /// <summary>
        /// Overridden ToString() method which returns the Value property - used for debugging purposes
        /// </summary>
        /// <returns>The value of this token.</returns>
        String ToString();


        /// <summary>
        /// Overloaded equality check - this method does not test if both tokens refer to the same object.
        /// Two tokens are considered equal if they have the same type, the same value and the same color.
        /// NOTE: This method should be overridden by each subclass to compare additional features.
        /// </summary>
        /// <param name="t">The token to check for equality.</param>
        /// <returns>True if the current token is equal to the token to compare to.</returns>
        bool Equals(IToken t);
    }

    /// <summary>
    /// This interface is implemented by the abstract generic base class LineBase and can be used as a 
    /// non-generic base type for methods that wish to work with different implementations of LineBase.
    /// The interface declares only public methods and properties that have non-generic parameters.
    /// </summary>
    public interface ILine
    {
        /***** PROPERTIES *****/

        /// <summary>
        /// Returns a boolean value indicating if this is a valid line (i.e. the line contains a CRLF token at the end).
        /// </summary>
        bool IsValid { get; }

        /// <summary>
        /// Gets or sets a value indicating if this line is currently visible.
        /// </summary>
        bool IsVisible { get; set; }

        /// <summary>
        /// Returns true if this line is empty, i.e. it contains only a newline token.
        /// </summary>
        bool IsEmpty { get; }

        /***** VIRTUAL METHODS AND PROPERTIES: *****/

        /***** PROPERTIES: *****/

        /// <summary>
        /// Returns the total number of tokens contained in this line (including newline tokens).
        /// </summary>
        int TokenCount { get; }

        /// <summary>
        /// Returns the length of this line, including new line characters.
        /// </summary>
        int Length { get; }

        /// <summary>
        /// Returns the last position inside a line (i.e. for valid lines the position of the CRLF
        /// token is returned - otherwise this property returns a value equal to Length)
        /// </summary>
        int LastPos { get; }

        /// <summary>
        /// Gets or sets the X position of this line inside the Control
        /// </summary>
        int X { get; set; }

        /// <summary>
        /// Gets or sets the Y position of this line inside the Control
        /// </summary>
        int Y { get; set; }

        /// <summary>
        /// Gets the width of this line.
        /// </summary>
        int Width { get; }

        /// <summary>
        /// Gets or sets the height of this line.
        /// </summary>
        int Height { get; set; }

        /// <summary>
        /// This method concatenates the values of all contained pieces to one string which is returned.
        /// </summary>
        /// <returns>String representation of this line</returns>
        String ToString();

        /// <summary>
        /// Returns the character at the given position. If there is no character at the given position the 
        /// method returns the constant value Char.MaxValue (which corresponds to the Unicode point 0xFFFF,
        /// which is guarenteed not to refer to an actual character value!) - this is only the case for the last
        /// position of a file, i.e. Char.MaxValue represents the end of file!
        /// </summary>
        /// <param name="pos">Position of character to return.</param>
        /// <returns>Character at given position.</returns>
        char CharAt(int pos);

        /// <summary>
        /// Get the Substring of the given length starting at the given position of the line. If len exceeds
        /// the lines' length the returned string will be clipped at the line end.
        /// </summary>
        /// <param name="pos">Start position of Substring.</param>
        /// <param name="len">Length of Substring.</param>
        /// <returns>Substring starting at pos with length len. String.Empty if pos is out of range.</returns>
        String SubString(int pos, int len);

        /// <summary>
        /// Overloaded equality check - this method does not test if both lines refer to the same object.
        /// Two lines are considered equal if they have the same type, equal length, the same number of tokens 
        /// and if the overloaded Equals(...) method for each token of both lines returns true.
        /// NOTE: This method should be overridden by each subclass to compare additional features.
        /// </summary>
        /// <param name="line">The line to check for equality.</param>
        /// <returns>True if the current line is equal to the line to compare to.</returns>
        bool Equals(ILine line);
    }

    /// <summary>
    /// This interface is implemented by the abstract generic base class DocumentBase and can be used as a 
    /// non-generic base type for methods that wish to work with different implementations of DocumentBase.
    /// The interface declares only public methods and properties that have non-generic parameters.
    /// </summary>
    public interface IDocument
    {
        /// <summary>
        /// Gets or sets the Text contained in this document.
        /// </summary>
        String Text { get; set; }

        /// <summary>
        /// Gets or sets the lines in this document.
        /// </summary>
        String[] Lines { get; set; }

        /// <summary>
        /// Gets or sets the lines in the document as a reference to the first line.
        /// NOTE: Caller has to ensure the content of the document is valid (each line terminated with
        /// a newline token, ...) when setting the content
        /// </summary>
        ILine FirstLine { get; set; }

        /// <summary>
        /// Gets the last line contained in the document.
        /// </summary>
        ILine LastLine { get; }

        /// <summary>
        /// Returns the total number of lines contained in the document.
        /// </summary>
        int LineCount { get; }

        /// <summary>
        /// Gets or sets the currently active color to be used for insert operations.
        /// </summary>
        Color Color { get; set; }

        /// <summary>
        /// Returns the length of the whole document including newline characters.
        /// </summary>
        int Length { get; }

        /// <summary>
        /// Returns the last text position inside the document
        /// </summary>
        int LastPos { get; }

        /// <summary>
        /// An UpdateEvent is fired each time a change to the data model occurs.
        /// </summary>
        event UpdateEventHandler UpdateEvent;

        /// <summary>
        /// This method can be used to undo operations determined by UpdateEventArgs objects. Override if
        /// implementing user defined UpdateEventArgs and call this base implementation for update events that
        /// are not handled by the overridden implementation!
        /// NOTE: Undo and Redo operations should be atomic, i.e. for each operation that can be undone the 
        /// inverse operation should be a single operation (in the redo buffer) as well!
        /// </summary>
        /// <param name="args">UpdateEventArgs for which to fire an inverse operation.</param>
        void InvokeInverseOp(UpdateEventArgs args);

        /// <summary>
        /// Clears all content from the current document!
        /// </summary>
        void Clear();

        /// <summary>
        /// Returns the text of the given length starting at the given character position inside the document.
        /// NOTE: The returned string maybe longer than the given dist parameter depending on the platforms newline
        /// encoding!
        /// </summary>
        /// <param name="charPos">Start position.</param>
        /// <param name="dist">Distance to the given start position.</param>
        /// <returns>Substring between the positions defined by the parameters.</returns>
        String SubString(int charPos, int dist);

        /// <summary>
        /// Returns the character at the given text position inside the document. If the given position is less than
        /// 0 or larger than the documents last position the character at the first or last position respectively
        /// will be returned. This method treats newline strings as a single character!
        /// </summary>
        /// <param name="charPos">Text position for which to get the character.</param>
        /// <returns>Character at the given position.</returns>
        char CharAt(int charPos);
    }

    /// <summary>
    /// This interface is implemented by the generic base class Selection and can be used as a 
    /// non-generic base type for methods that wish to work with different instances of a Selection.
    /// The interface declares only public methods and properties that have non-generic parameters.
    /// </summary>
    public interface ISelection
    {
    }

    /// <summary>
    /// This interface is implemented by the generic base class Position and can be used as a 
    /// non-generic base type for methods that wish to work with different instances of a Position.
    /// The interface declares only public methods and properties that have non-generic parameters.
    /// </summary>
    public interface IPosition
    {
        /// <summary>
        /// Gets or sets the x coordinate of this position.
        /// </summary>
        int X { get; set; }

        /// <summary>
        /// Gets or sets the character position inside the containing line.
        /// </summary>
        int LinePos { get; set; }

        /// <summary>
        /// Returns the line containing this position
        /// </summary>
        ILine ILine { get; }
    }
}