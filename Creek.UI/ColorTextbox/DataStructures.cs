using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Creek.UI.ColorTextbox
{
    /// <summary>
    /// Represents the method that is called when the ColorTextBox control wants to send messages to the GUI
    /// in order to inform the user about internal processes (like exceptions, ...). This delegate is primarily
    /// for debugging purposes when developing a new derived ColorTextBox but it can be used to send messages
    /// to the GUI in an application as well (such as search results, warnings, ...).
    /// However, note that the default implementation in the abstract base class does not send such messages at all!
    /// </summary>
    /// <param name="source">Additional data of the message event or the source of the event.</param>
    /// <param name="msg">Message string to send.</param>
    public delegate void MessageEventHandler(object source, MessageEventArgs msg);

    /// <summary>
    /// Represents the method that is called when a change to the documents' data occurs (like Insert, Delete, ...)
    /// </summary>
    /// <param name="source">The source that triggered the update.</param>
    /// <param name="args">UpdateEventArgs of the UpdateEvent.</param>
    public delegate void UpdateEventHandler(object source, UpdateEventArgs args);

    /// <summary>
    /// Represents the method that is called when the caret changes (added and (re)moved)
    /// </summary>
    /// <param name="source">The source that triggered the update.</param>
    /// <param name="args">CaretEventArgs of the CaretEvent.</param>
    public delegate void CaretEventHandler(object source, CaretEventArgs args);

    /// <summary>
    /// Represents the EventArgs that encapsulate a message string to send to the UI.
    /// </summary>
    public class MessageEventArgs : EventArgs
    {
        public String Message;

        public MessageEventArgs()
        {
            Message = String.Empty;
        }

        public MessageEventArgs(String msg)
        {
            Message = msg;
        }
    }

    /// <summary>
    /// Represents the EventArgs that encapsulate the position data of caret position changes
    /// </summary>
    public class CaretEventArgs : EventArgs
    {
        /// <summary>
        /// The new caret position
        /// </summary>
        public IPosition NewPos;

        /// <summary>
        /// The old caret position before the event occured
        /// </summary>
        public IPosition OldPos;

        /// <summary>
        /// The type of the CaretEvent that occured
        /// </summary>
        public CaretEventType Type;

        public CaretEventArgs()
        {
            OldPos = null;
            NewPos = null;
            Type = CaretEventType.None;
        }

        public CaretEventArgs(IPosition oldPos, IPosition newPos)
        {
            OldPos = oldPos;
            NewPos = newPos;
            if (oldPos == null)
            {
                if (newPos == null) Type = CaretEventType.None;
                else Type = CaretEventType.Set;
            }
            else if (newPos == null) Type = CaretEventType.Removed;
            else Type = CaretEventType.Changed;
        }
    }

    /// <summary>
    /// Defines the type of a CaretEvent.
    /// </summary>
    public enum CaretEventType
    {
        /// <summary>
        /// Invalid (empty) caret event
        /// </summary>
        None,

        /// <summary>
        /// The caret position has changed
        /// </summary>
        Changed,

        /// <summary>
        /// The caret has been removed
        /// </summary>
        Removed,

        /// <summary>
        /// The caret has been set
        /// </summary>
        Set
    }

    /// <summary>
    /// This enumeration is to be used when sending UpdateEvents to the ColorTextBox to determine if an event
    /// should be added to the undo or redo buffer. When an update event originated from user input the 
    /// UpdateAction will be set to Undoable. If an action originated from an undo or redo operation the 
    /// UpdateAction will be set to Undo or Redo respectively.
    /// The NonAtomic flag indicates that an action is part of a composite operation (e.g. Replace fires both
    /// a delete and an insert event) and should therefore be ignored.
    /// </summary>
    [Flags]
    public enum UpdateAction
    {
        /// <summary>
        /// The action was initiated by the user and should be added to the undo buffer.
        /// </summary>
        Undoable = 0,

        /// <summary>
        /// An action from the undo buffer has been undone and should be added to the redo buffer.
        /// </summary>
        Undo = 1,

        /// <summary>
        /// An action from the redo buffer has benn redone and should be added to the undo buffer.
        /// </summary>
        Redo = 2,

        /// <summary>
        /// The action is part of a compound action and should not be added to the redo or undo buffer.
        /// </summary>
        NonAtomic = 3
    }

    /// <summary>
    /// This enum can be used to determine the type of an UpdateEvent. It can be used to find out about the 
    /// actual type of the data stored in the Data field of an UpdateEventArg object.
    /// If the given UpdateEventTypes should be extended it's recommended to define a new enum containing the
    /// additional types and to derive a new class from UpdateEventArgs that contains a field of the newly 
    /// defined enum. The original Type field of UpdateEventArgs can then be set to UpdateEventType.User to 
    /// indicate that an event is of the derived type! (alternatively the Type field can be redefined in the
    /// subclass and a type check can be performed on an UpdateEventArgs object to determine the actual type!)
    /// </summary>
    public enum UpdateEventType
    {
        /// <summary>
        /// Defines an empty UpdateEvent
        /// </summary>
        None = 0,

        /// <summary>
        /// Defines an UpdateEvent that occurs when the Content of the ColorTextBox is set via one of the 
        /// Lines, Text or FirstLine properties.
        /// </summary>
        NewDocument = 10,

        /// <summary>
        /// Defines an UpdateEvent that occurs when text is inserted.
        /// </summary>
        Insert = 20,

        /// <summary>
        /// Defines an UpdateEvent that occurs when text is deleted.
        /// </summary>
        Delete = 30,

        /// <summary>
        /// Defines an UpdateEvent that occurs when text is replaced.
        /// </summary>
        Replace = 40,

        /// <summary>
        /// Defines an UpdateEvent that occurs when the Font property is changed.
        /// </summary>
        UpdateFont = 50,

        /// <summary>
        /// Defines an UpdateEvent that occurs when the SelectionColor property is changed.
        /// </summary>
        UpdateColor = 60,

        /// <summary>
        /// A user defined UpdateEvent occured.
        /// </summary>
        Custom = 100
    }

    /// <summary>
    /// Specifies how a text search is carried out in a ColorTextBox control. 
    /// This enumeration has a FlagsAttribute attribute that allows a bitwise combination of its member values.
    /// </summary>
    [Flags]
    public enum ColorTextBoxFinds
    {
        /// <summary>
        /// Locate all instances of the search text, whether the instances found in the search are whole words or not.
        /// </summary>
        None = 0,

        /// <summary>
        /// Locate only instances of the search text that have the exact casing.
        /// </summary>
        MatchCase = 1,

        /// <summary>
        /// The search starts at the end of the control's document and searches to the beginning of the document.
        /// </summary>
        Reverse = 2
    }

    /// <summary>
    /// This abstract generic class must be inherited by all concrete implementations of line tokens to be used
    /// by LineBase and it's derivates. A token holds a value to be displayed in a line and can be extended 
    /// to hold any other information to be used in a concrete text viewer / editor (such as styling
    /// information, fonts, ...). For the simplest concrete implementation refer to the class Token.
    /// </summary>
    [Serializable]
    public abstract class TokenBase<TToken> : IToken
        where TToken : TokenBase<TToken>, new()
    {
        /***** PROTECTED FIELDS: *****/

        protected static Color DefaultColor = Color.Black;
        protected Color color;
        protected int height;

        protected Object lockObj = new Object();
        protected TToken next;
        protected TToken prev;
        protected char[] value;
        protected int width;

        /***** CONSTRUCTORS: *****/

        public TokenBase()
        {
            value = String.Empty.ToCharArray(); // new char[0];
            next = null;
            prev = null;
            color = DefaultColor;
        }

        /// <summary>
        /// Gets or sets the next LineToken.
        /// </summary>
        public TToken Next
        {
            get
            {
                lock (lockObj)
                {
                    return next;
                }
            }
            protected internal set
            {
                lock (lockObj)
                {
                    next = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the previous LineToken.
        /// </summary>
        public TToken Prev
        {
            get
            {
                lock (lockObj)
                {
                    return prev;
                }
            }
            protected internal set
            {
                lock (lockObj)
                {
                    prev = value;
                }
            }
        }

        /***** STATIC METHODS: *****/

        #region IToken Members

        /// <summary>
        /// Overridden ToString() method which returns the Value property - used for debugging purposes
        /// </summary>
        /// <returns>The value of this token.</returns>
        public override string ToString()
        {
            lock (lockObj)
            {
                return Value;
            }
        }

        /// <summary>
        /// Overloaded equality check - this method does not test if both tokens refer to the same object.
        /// Two tokens are considered equal if they have the same type, the same value and the same color.
        /// NOTE: This method should be overridden by each subclass to compare additional features.
        /// </summary>
        /// <param name="t">The token to check for equality.</param>
        /// <returns>True if the current token is equal to the token to compare to.</returns>
        public virtual bool Equals(IToken t)
        {
            lock (lockObj)
            {
                if (t != null)
                {
                    if (t is TToken && t.Color == Color && t.Length == Length &&
                        t.Value.Equals(Value)) return true;
                }
                return false;
            }
        }

        /***** PROPERTIES: *****/

        /// <summary>
        /// Gets or sets the value of this token.
        /// NOTE: Setting the value of a token directly will not trigger any update events nor will it set the
        /// Modified property of the containing line to true!
        /// </summary>
        public String Value
        {
            get
            {
                lock (lockObj)
                {
                    if (value != null && Length > 0) return new String(value);
                    return String.Empty;
                }
            }
            protected internal set
            {
                lock (lockObj)
                {
                    try
                    {
                        if (value != null) value = new string(value.ToCharArray());
                        else value = new string[0].ToString();
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        value = new string[0].ToString();
                    }
                }
            }
        }

        /// <summary>
        /// Indexer for array-like access to the token value. Throws an IndexOutOfRangeException if the
        /// given index is out of range.
        /// </summary>
        /// <param name="index">Index of the character to set or get.</param>
        /// <returns>Character at the given index.</returns>
        public char this[int index]
        {
            get
            {
                lock (lockObj)
                {
                    if (index < 0 || index >= Length)
                    {
                        throw new IndexOutOfRangeException();
                    }
                    else return value[index];
                }
            }
            protected internal set
            {
                lock (lockObj)
                {
                    if (index < 0 || index >= Length)
                    {
                        throw new IndexOutOfRangeException();
                    }
                    else this.value[index] = value;
                }
            }
        }

        /// <summary>
        /// Returns the length of this LineToken's value.
        /// </summary>
        public int Length
        {
            get
            {
                lock (lockObj)
                {
                    if (value != null)
                    {
                        return value.Length;
                    }
                    return 0;
                }
            }
        }

        /// <summary>
        /// Returns true if this token is an end of line token.
        /// </summary>
        public bool IsNewLineToken
        {
            get
            {
                lock (lockObj)
                {
                    return Value.Equals(Environment.NewLine);
                }
            }
        }

        /// <summary>
        /// Gets or sets the color of this token.
        /// </summary>
        public Color Color
        {
            get
            {
                lock (lockObj)
                {
                    return color;
                }
            }
            set
            {
                lock (lockObj)
                {
                    color = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the rendering width of the current token - to be used by the rendering control.
        /// </summary>
        public int Width
        {
            get
            {
                lock (lockObj)
                {
                    return width;
                }
            }
            set
            {
                lock (lockObj)
                {
                    width = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the height of the current token - to be used by the rendering control.
        /// </summary>
        public int Height
        {
            get
            {
                lock (lockObj)
                {
                    return height;
                }
            }
            set
            {
                lock (lockObj)
                {
                    height = value;
                }
            }
        }

        #endregion

        /// <summary>
        /// Constructs a Token with a value set to System.Environment.NewLine. Only tokens returned by
        /// this factory method should be used as end of line tokens, as their value is platform independent.
        /// </summary>
        public static TToken NewLineToken()
        {
            var nlT = new TToken();
            nlT.Value = Environment.NewLine;
            return nlT;
        }

        /***** METHODS: *****/

        /// <summary>
        /// Returns a deep copy of this token (i.e. all fields of this token are cloned not including any
        /// references to other tokens!). The default implementation can only provide copies of the fields 
        /// defined in the TokenBase class! When inheriting from TokenBase this method should be overridden
        /// to return a token constructed via a call to this base implementation (and cast to the type of
        /// the derived class) extended with copies of additional fields defined in the derived class.
        /// </summary>
        /// <returns>A deep copy of this Token.</returns>
        public virtual TToken Clone()
        {
            lock (lockObj)
            {
                var t = new TToken();
                t.Value = Value;
                t.Color = Color.FromArgb(color.R, color.G, color.B);
                t.width = width;
                t.height = height;
                return t;
            }
        }

        /// <summary>
        /// Inserts the given character at the given index of the token. If index is less than 0 or bigger 
        /// than Length the character will be inserted at the beginning respectively at the end of the token.
        /// </summary>
        /// <param name="c">Character to insert.</param>
        /// <param name="index">Position at which to insert.</param>
        protected internal void Insert(char c, int index)
        {
            lock (lockObj)
            {
                if (index < 0) index = 0;
                if (index > Length) index = Length;
                var newVal = new char[Length + 1];
                try
                {
                    Array.ConstrainedCopy(value, 0, newVal, 0, index);
                    newVal[index] = c;
                    Array.ConstrainedCopy(value, index, newVal, index + 1, Length - index);
                    value = newVal;
                }
                catch (Exception)
                {
                    return;
                }
            }
        }

        /// <summary>
        /// Inserts the given string at the given index of the token. If index is less than 0 or bigger 
        /// than Length the string will be inserted at the beginning respectively at the end of the token.
        /// </summary>
        /// <param name="s">Unicode string to insert.</param>
        /// <param name="index">Position at which to insert.</param>
        protected internal void Insert(String s, int index)
        {
            lock (lockObj)
            {
                if (index < 0) index = 0;
                if (index > Length) index = Length;
                var newVal = new char[Length + s.Length];
                try
                {
                    if (Length + s.Length > ColorTextBox.MAX_TOKEN_LEN)
                    {
                    }
                    Array.ConstrainedCopy(value, 0, newVal, 0, index);
                    char[] tmp = s.ToCharArray();
                    Array.ConstrainedCopy(tmp, 0, newVal, index, tmp.Length);
                    Array.ConstrainedCopy(value, index, newVal, index + tmp.Length, Length - index);
                    value = newVal;
                }
                catch (Exception)
                {
                    return;
                }
            }
        }

        /// <summary>
        /// Removes the characters from the given start position up to the given length or the end of the
        /// token, whichever comes first.
        /// </summary>
        /// <param name="pos">Position at which to start removing.</param>
        /// <param name="len">Number of characters to remove.</param>
        /// <returns>Number of deleted characters.</returns>
        protected internal int Delete(int pos, int len)
        {
            lock (lockObj)
            {
                if (pos < 0 || pos >= Length) return 0;
                else
                {
                    char[] newVal;
                    int deleted;
                    try
                    {
                        if (pos + len < Length)
                        {
                            newVal = new char[Length - len];
                            deleted = len;
                            Array.ConstrainedCopy(value, 0, newVal, 0, pos);
                            Array.ConstrainedCopy(value, pos + len, newVal, pos, Length - (pos + len));
                        }
                        else
                        {
                            newVal = new char[pos];
                            deleted = Length - pos;
                            Array.ConstrainedCopy(value, 0, newVal, 0, pos);
                        }
                    }
                    catch (Exception)
                    {
                        return 0;
                    }
                    value = newVal;
                    return deleted;
                }
            }
        }

        /// <summary>
        /// Splits this token at the given position. The first of the two resulting tokens is the original
        /// token with its value clipped at the split position (but all references are kept!). 
        /// The second token will be a clone of the original token with its value set to the remaining part
        /// of the original value and with all references set accordingly. This method will never produce
        /// empty tokens (with Value == String.Empty), i.e. pos must be larger than 0 and smaller than Length 
        /// in order to succeed!
        /// NOTE: Attributes used for rendering (width, height) have to be recalculated after splitting to 
        /// reflect the changes!
        /// </summary>
        /// <param name="pos">Position inside the token at which to split. If pos is outside the tokens range 
        /// no modifications will be made!</param>
        protected internal virtual bool Split(int pos)
        {
            lock (lockObj)
            {
                if (pos > 0 && pos < Length)
                {
                    TToken newT = Clone();
                    newT.Value = Value.Substring(pos);
                    Value = Value.Substring(0, pos);
                    // update all references:
                    if (Next != null)
                    {
                        Next.Prev = newT;
                        newT.Next = Next;
                    }
                    newT.Prev = (TToken) this;
                    Next = newT;
                    return true;
                }
                return false;
            }
        }
    }

    /// <summary>
    /// The abstract generic collection LineBase represents a line of text to be displayed in a text editor.
    /// A line consists of double linked TToken tokens (TToken has to be a concrete implementation of
    /// the abstract generic class TokenBase) which store the text value and optionally additional attributes for 
    /// rendering. A whole document can be represented as a double linked list of Line collections.
    /// For the simplest concrete implementation of LineBase refer to the class Line.
    /// </summary>
    /// <typeparam name="TToken">Concrete implementation of type TokenBase.</typeparam>
    /// <typeparam name="TLine">Concrete implementation of type LineBase.</typeparam>
    [Serializable]
    public abstract class LineBase<TLine, TToken> : ILine
        where TToken : TokenBase<TToken>, new()
        where TLine : LineBase<TLine, TToken>, new()
    {
        /***** PROTECTED FIELDS *****/

        protected int baseLine;
        protected bool disposed; // Indicates if Dispose() has been called on the line
        protected TToken head; // first token in a line
        protected int height;
        protected bool isVisible; // Indicates if this line is currently visible
        protected Object lockObj = new Object();
        protected bool modified; // Dirty flag indicating that the line has been modified
        protected TLine next; // Reference to next line
        protected TLine prev; // Reference to previous line
        protected bool selected; // Flag indicating if that line is currently part of a selection
        protected TToken tail; // last token in a line - has to contain a newline string for a
        protected int width;

        protected int x, y;

        /***** CONSTRUCTORS *****/

        /// <summary>
        /// Default Constructor: constructs an empty line.
        /// </summary>
        public LineBase()
        {
            head = null;
            tail = null;
            next = null;
            prev = null;
            x = 0;
            y = 0;
            width = 0;
            height = 0;
            baseLine = 0;
            isVisible = false;
            disposed = false;
            Modified = true;
        }

        /// <summary>
        /// Constructs a line with the given token as the first token of the line.
        /// If there is no CRLF-token contained in the passed token-list the line will be invalid!
        /// If there are multiple CRLF-tokens contained the token-list will be clipped at the first
        /// occurence of a CRLF-token!
        /// </summary>
        /// <param name="head">A token(list) representing the content of the line.</param>
        public LineBase(TToken head) : this()
        {
            makeNewLine(head);
        }

        /// <summary>
        /// Gets or sets the next line.
        /// </summary>
        public TLine Next
        {
            get
            {
                lock (lockObj)
                {
                    return next;
                }
            }
            set
            {
                lock (lockObj)
                {
                    next = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the previous line.
        /// </summary>
        public TLine Prev
        {
            get
            {
                lock (lockObj)
                {
                    return prev;
                }
            }
            set
            {
                lock (lockObj)
                {
                    prev = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the Token-List of this line.
        /// </summary>
        public TToken First
        {
            get
            {
                lock (lockObj)
                {
                    return head;
                }
            }
            set
            {
                lock (lockObj)
                {
                    makeNewLine(value);
                }
            }
        }

        /// <summary>
        /// Gets the last token of a line.
        /// </summary>
        public TToken Last
        {
            get
            {
                lock (lockObj)
                {
                    return tail;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating if this line is currently (partly) selected.
        /// </summary>
        public bool IsSelected
        {
            get
            {
                lock (lockObj)
                {
                    return selected;
                }
            }
            set
            {
                lock (lockObj)
                {
                    selected = value;
                }
            }
        }

        /// <summary>
        /// This property is set to true whenever the data of a line is changed and can be used by the 
        /// ColorTextBox to determine when line metrics have to be recalculated.
        /// </summary>
        public virtual bool Modified
        {
            get
            {
                lock (lockObj)
                {
                    return modified;
                }
            }
            set
            {
                lock (lockObj)
                {
                    modified = value;
                }
            }
        }

        /// <summary>
        /// Indicates if Dispose() has been called on the given line. For every line that has been deleted
        /// from a document this property will return true and the line should not be used anymore!
        /// </summary>
        public bool Disposed
        {
            get { return disposed; }
        }

        #region ILine Members

        /// <summary>
        /// This method concatenates the values of all contained pieces to one string which is returned.
        /// </summary>
        /// <returns>String representation of this line</returns>
        public override String ToString()
        {
            var ret = new StringBuilder();
            lock (lockObj)
            {
                TToken search = head;
                while (search != null)
                {
                    ret.Append(search.Value);
                    search = search.Next;
                }
            }
            return ret.ToString();
        }

        /// <summary>
        /// Returns the character at the given position. If there is no character at the given position the 
        /// method returns the constant value Char.MaxValue (which corresponds to the Unicode point 0xFFFF,
        /// which is guarenteed not to refer to an actual character value!) - this is only the case for the last
        /// position of a file, i.e. Char.MaxValue represents the end of file!
        /// </summary>
        /// <param name="pos">Position of character to return.</param>
        /// <returns>Character at given position.</returns>
        public char CharAt(int pos)
        {
            char ret = Char.MaxValue;
            lock (lockObj)
            {
                int lastP = LastPos;
                if (pos < 0) pos = 0;
                if (pos > lastP) pos = lastP;
                int tokenStart;
                TToken posToken = TokenAt(pos, out tokenStart);
                if (posToken != null)
                {
                    try
                    {
                        char c = posToken[pos - tokenStart];
                        if (c == '\r' || c == '\f') return '\n';
                        ret = c;
                    }
                    catch (IndexOutOfRangeException)
                    {
                        ret = Char.MaxValue;
                    }
                }
                else ret = Char.MaxValue;
            }
            return ret;
        }

        /// <summary>
        /// Get the Substring of the given length starting at the given position of the line. If len exceeds
        /// the lines' length the returned string will be clipped at the line end.
        /// </summary>
        /// <param name="pos">Start position of Substring.</param>
        /// <param name="len">Length of Substring.</param>
        /// <returns>Substring starting at pos with length len. String.Empty if pos is out of range.</returns>
        public String SubString(int pos, int len)
        {
            String ret = String.Empty;
            lock (lockObj)
            {
                int lineLen = Length;
                int last = LastPos;
                if (pos < 0) pos = 0;
                if (pos + len > lineLen) len = lineLen - pos;
                if (pos > last) return ret;
                else
                {
                    int startSP = 0;
                    int endSP = 0;
                    TToken start = TokenAt(pos, out startSP);
                    TToken end = TokenAt(pos + len, out endSP);
                    if (start != null && end != null)
                    {
                        var sb = new StringBuilder(len);
                        if (start == end)
                        {
                            sb.Append(start.Value.Substring(pos - startSP, len));
                            return sb.ToString();
                        }
                        sb.Append(start.Value.Substring(pos - startSP));
                        TToken search = start.Next;
                        while (search != null && search != end)
                        {
                            sb.Append(search.Value);
                            search = search.Next;
                        }
                        len -= sb.Length;
                        if (search == end)
                        {
                            sb.Append(search.Value.Substring(0, len));
                        }
                        return sb.ToString();
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// Overloaded equality check - this method does not test if both lines refer to the same object.
        /// Two lines are considered equal if they have the same type, equal length, the same number of tokens 
        /// and if the overloaded Equals(...) method for each token of both lines returns true.
        /// NOTE: This method should be overridden by each subclass to compare additional features.
        /// </summary>
        /// <param name="line">The line to check for equality.</param>
        /// <returns>True if the current line is equal to the line to compare to.</returns>
        public virtual bool Equals(ILine line)
        {
            bool ret = false;
            lock (lockObj)
            {
                if (line != null)
                {
                    if (line is TLine && line.Length == Length && line.TokenCount == TokenCount)
                    {
                        var compL = (TLine) line;
                        TToken thisSearch = First;
                        TToken lineSearch = compL.First;
                        if (thisSearch == null && lineSearch == null) ret = true;
                        else
                        {
                            while (thisSearch != null && thisSearch.Equals(lineSearch))
                            {
                                thisSearch = thisSearch.Next;
                                lineSearch = lineSearch.Next;
                            }
                            if (thisSearch == null) ret = true;
                        }
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// Returns a boolean value indicating if this is a valid line (i.e. the line contains a CRLF token at the end).
        /// </summary>
        public bool IsValid
        {
            get
            {
                lock (lockObj)
                {
                    if (tail != null) return (tail.IsNewLineToken);
                    else return false;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating if this line is currently visible.
        /// </summary>
        public bool IsVisible
        {
            get
            {
                lock (lockObj)
                {
                    return isVisible;
                }
            }
            set
            {
                lock (lockObj)
                {
                    isVisible = value;
                }
            }
        }

        /// <summary>
        /// Returns true if this line is empty, i.e. it contains only a newline token.
        /// </summary>
        public virtual bool IsEmpty
        {
            get
            {
                lock (lockObj)
                {
                    return (head != null && head.IsNewLineToken);
                }
            }
        }

        /// <summary>
        /// Returns the total number of tokens contained in this line (including newline tokens).
        /// </summary>
        public virtual int TokenCount
        {
            get
            {
                lock (lockObj)
                {
                    TToken search = head;
                    int num = 0;
                    while (search != null)
                    {
                        num++;
                        search = search.Next;
                    }
                    return num;
                }
            }
        }

        /// <summary>
        /// Returns the length of this line, including new line characters.
        /// </summary>
        public virtual int Length
        {
            get
            {
                lock (lockObj)
                {
                    TToken search = head;
                    int len = 0;
                    while (search != null)
                    {
                        len += search.Length;
                        search = search.Next;
                    }
                    return len;
                }
            }
        }

        /// <summary>
        /// Returns the last position inside a line (i.e. for valid lines the position of the CRLF
        /// token is returned - otherwise this property returns a value equal to Length)
        /// </summary>
        public virtual int LastPos
        {
            get
            {
                lock (lockObj)
                {
                    if (IsValid) return Length - Environment.NewLine.Length;
                    return Length;
                }
            }
        }

        /// <summary>
        /// Gets or sets the X position of this line inside the Control
        /// </summary>
        public virtual int X
        {
            get
            {
                lock (lockObj)
                {
                    return x;
                }
            }
            set
            {
                lock (lockObj)
                {
                    x = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the Y position of this line inside the Control
        /// </summary>
        public virtual int Y
        {
            get
            {
                lock (lockObj)
                {
                    return y;
                }
            }
            set
            {
                lock (lockObj)
                {
                    y = value;
                }
            }
        }

        /// <summary>
        /// Gets the width of this line.
        /// </summary>
        public virtual int Width
        {
            get
            {
                lock (lockObj)
                {
                    TToken t = First;
                    int w = 0;
                    while (t != null)
                    {
                        w += t.Width;
                        t = t.Next;
                    }
                    return w;
                }
            }
        }

        /// <summary>
        /// Gets or sets the height of this line.
        /// </summary>
        public virtual int Height
        {
            get
            {
                lock (lockObj)
                {
                    return height;
                }
            }
            set
            {
                lock (lockObj)
                {
                    height = value;
                }
            }
        }

        #endregion

        /***** PROTECTED METHODS: *****/

        /// <summary>
        /// Constructs a new line from the given piece list - list is clipped on first occurence of
        /// a new line token. If a null reference is passed this method constructs an invalid empty line.
        /// </summary>
        /// <param name="head">Token list for the new line</param>
        protected void makeNewLine(TToken head)
        {
            lock (lockObj)
            {
                if (head != null)
                {
                    head.Prev = null;
                    this.head = head;
                    tail = head;
                    while (tail.Next != null)
                    {
                        if (tail.IsNewLineToken) tail.Next = null;
                        else tail = tail.Next;
                    }
                }
                else
                {
                    head = tail = null;
                }
                Modified = true;
            }
        }

        /// <summary>
        /// Removes the given piece from the line - NOTE: piece has to be contained in line!
        /// </summary>
        /// <param name="p">Piece to remove.</param>
        protected void removeContainedToken(TToken p)
        {
            lock (lockObj)
            {
                if (p == head)
                {
                    if (p == tail) head = tail = null;
                    else
                    {
                        head = p.Next;
                        head.Prev = null;
                    }
                }
                else if (p == tail)
                {
                    tail = tail.Prev;
                    tail.Next = null;
                }
                else
                {
                    p.Prev.Next = p.Next;
                    if (p.Next != null) p.Next.Prev = p.Prev;
                }
                Modified = true;
            }
        }

        /// <summary>
        /// Helper method that ensures that a token list does not contain multiple CRLF tokens. Checks if the
        /// passed token list contains a CRLF token and removes all tokens that maybe contained afterwards.
        /// Furthermore any tokens that maybe contained before the passed token list start will be removed.
        /// Returns a reference to the last token of the list.
        /// </summary>
        /// <param name="t">Token list to check.</param>
        /// <returns>Reference to the last token inside the list.</returns>
        protected TToken clipTokenList(TToken t)
        {
            TToken retT = null;
            lock (lockObj)
            {
                if (t.Prev != null)
                {
                    t.Prev.Next = null;
                    t.Prev = null;
                }
                TToken search = t;
                TToken last = t;
                while (search != null && !search.IsNewLineToken)
                {
                    last = search;
                    search = search.Next;
                }
                if (search != null && search.IsNewLineToken)
                {
                    if (search.Next != null) search.Next.Prev = null;
                    search.Next = null;
                    retT = search;
                }
                else retT = last;
            }
            return retT;
        }

        /// <summary>
        /// This helper method can be used to reset the tail pointer of a line which may be necessary when 
        /// inserting at the end of the line or when splitting tokens.
        /// </summary>
        protected void resetLineTail()
        {
            lock (lockObj)
            {
                if (head != null)
                {
                    tail = head;
                    while (tail.Next != null)
                    {
                        tail = tail.Next;
                    }
                }
            }
        }

        /// <summary>
        /// Normalizes the line by removing all empty tokens (i.e. tokens with their value set to String.Empty).
        /// Calling this method may require all line properties to be recalculated
        /// </summary>
        protected virtual void normalize()
        {
            if (head != null)
            {
                lock (lockObj)
                {
                    TToken t = head;
                    TToken check;
                    while (t != null)
                    {
                        check = t;
                        t = t.Next;
                        if (check.Value.Length == 0)
                        {
                            removeContainedToken(check);
                            Modified = true;
                        }
                    }
                }
            }
        }

        /***** PUBLIC METHODS *****/

        /// <summary>
        /// Splits the line at the given position resulting in two connected lines with the first one always 
        /// being invalid! (i.e. there will be no CRLF token at the end of the line!).
        /// If the position lies outside the lines range the method will split the line at position 0 or
        /// at the end respectively. A valid line which should be split at the end will always be split before 
        /// the CRLF token.
        /// </summary>
        /// <param name="pos">Position at which to split the line.</param>
        protected internal void Split(int pos)
        {
            lock (lockObj)
            {
                int length = Length;
                if (pos > length) pos = length;
                if (pos < 0) pos = 0;
                int tStart = 0;
                TToken t = TokenAt(pos, out tStart);
                TToken newLast = null; // new last token of this line
                TToken newFirst = null; // first token of the new line
                // there is a token at pos:
                if (t != null)
                {
                    if (t.IsNewLineToken)
                    {
                        // split line before CRLF token:
                        newLast = t.Prev;
                        newFirst = t;
                    }
                    else
                    {
                        // split found token if necessary:
                        if (pos > tStart)
                        {
                            t.Split(pos - tStart);
                            newLast = t;
                            newFirst = t.Next;
                        }
                        else
                        {
                            newLast = t.Prev;
                            newFirst = t;
                        }
                    }
                }
                    // there is no token at pos - either line is empty or we try to split at (or after) line end
                else
                {
                    // valid lines are to be split before the final CRLF token:
                    if (IsValid)
                    {
                        newLast = tail.Prev;
                        newFirst = tail;
                    }
                        // invalid lines are split at end --> new line will be empty!
                    else newLast = tail;
                }
                // construct a new line containing all tokens starting at newFirst:
                if (newFirst != null) newFirst.Prev = null;
                var newL = new TLine();
                newL.First = newFirst;
                // update the current line with newLast:
                if (newLast != null)
                {
                    tail = newLast;
                    newLast.Next = null;
                }
                else First = null;
                // insert the new line:
                if (Next != null)
                {
                    Next.Prev = newL;
                    newL.Next = Next;
                }
                newL.Prev = (TLine) this;
                Next = newL;
                Modified = true;
            }
        }

        /// <summary>
        /// Concatenates the content of the next line to the line and removes the next line. This method can only
        /// succeed if the line is invalid.
        /// </summary>
        /// <returns>True on success. False if next line is empty or if the line is already valid.</returns>
        protected internal bool AddNext()
        {
            bool ret = false;
            lock (lockObj)
            {
                if (Next != null && !IsValid)
                {
                    TLine tmp = Next;
                    if (tail != null)
                    {
                        tail.Next = Next.First;
                        if (Next.First != null)
                        {
                            Next.First.Prev = tail;
                            tail = Next.Last;
                        }
                    }
                    else
                    {
                        First = Next.First;
                    }
                    if (Next.Next != null)
                    {
                        Next.Next.Prev = (TLine) this;
                    }
                    Next = Next.Next;
                    Modified = true;
                    ret = true;
                    tmp.Dispose();
                    tmp = null;
                }
            }
            return ret;
        }

        /// <summary>
        /// Gets the Substring of the given length starting at the given position of the line encapsulated 
        /// inside a token list. The returned token list contains deep copies (via Clone()) of the original 
        /// tokens. If the start or end positions of the desired substring lie inside a token, the token
        /// value will be clipped accordingly.
        /// </summary>
        /// <param name="pos">Start position of token list.</param>
        /// <param name="len">Length of the substring to return.</param>
        /// <returns>The substring defined by the passed parameter encapsulated inside a token list.
        /// Null if pos is out of range.</returns>
        public TToken SubTokens(int pos, int len)
        {
            TToken ret = null;
            lock (lockObj)
            {
                if (pos < 0) pos = 0;
                if (pos >= Length || len <= 0) ret = null;
                else
                {
                    int startStPos = 0;
                    int endStPos = 0;
                    TToken start = TokenAt(pos, out startStPos);
                    TToken end = TokenAt(pos + len, out endStPos);
                    if (start != null)
                    {
                        ret = start.Clone();
                        if (start == end)
                        {
                            ret.Value = start.Value.Substring(pos - startStPos, len);
                        }
                        else
                        {
                            ret.Value = start.Value.Substring(pos - startStPos);
                            TToken curr = ret;
                            start = start.Next;
                            while (start != end)
                            {
                                curr.Next = start.Clone();
                                curr.Next.Prev = curr;
                                curr = curr.Next;
                                start = start.Next;
                            }
                            if (end != null && endStPos != pos + len)
                            {
                                curr.Next = end.Clone();
                                curr.Next.Prev = curr;
                                curr.Next.Value = end.Value.Substring(0, pos + len - endStPos);
                            }
                        }
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// Appends the given token (list) to the end of the line. All tokens of the passed list are appended
        /// until a CRLF token is encountered or there are no more tokens to append.
        /// When the line was valid before and the passed token list contains a CRLF token a new line containing
        /// only a CRLF token will be inserted after the current line.
        /// NOTE: Appending a token directly does not trigger an update event and therefore the changes will not
        /// be visible in the editor!
        /// </summary>
        /// <param name="t">Token to add at the end of the line.</param>
        /// <returns>True on success.</returns>
        protected internal bool Append(TToken t)
        {
            bool ret = false;
            lock (lockObj)
            {
                if (t != null)
                {
                    TToken last = clipTokenList(t);
                    // list is terminated by CRLF token!
                    if (last.IsNewLineToken)
                    {
                        if (IsValid || Next == null)
                        {
                            Split(Length);
                            if (tail != null)
                            {
                                tail.Next = t;
                                t.Prev = tail;
                            }
                                // line is empty after splitting:
                            else head = t;
                        }
                            // line was not valid before:
                        else
                        {
                            if (tail != null)
                            {
                                tail.Next = t;
                                t.Prev = tail;
                            }
                            else head = t;
                        }
                        tail = last;
                    }
                        // list is not terminated by CRLF token!
                    else
                    {
                        // line already contains a CRLF token
                        if (IsValid)
                        {
                            if (tail.Prev != null)
                            {
                                tail.Prev.Next = t;
                                t.Prev = tail.Prev;
                            }
                            else head = t;
                            tail.Prev = last;
                            last.Next = tail;
                        }
                            // line contains no CRLF token
                        else
                        {
                            if (tail != null)
                            {
                                tail.Next = t;
                                t.Prev = tail;
                            }
                            else head = t;
                            tail = last;
                        }
                    }
                    Modified = true;
                    normalize();
                    ret = true;
                }
            }
            return ret;
        }

        /// <summary>
        /// Inserts the given token list at the given character position of the line. If the insertion point lies
        /// between two tokens the token list will be inserted in between, but if the insertion point lies 
        /// inside a token this token will be split into two tokens and the given token list will be inserted 
        /// in between.
        /// If the token list contains a CRLF token the list will be clipped after it and the line will be split
        /// so that the new line contains the remaining line tokens after the insertion point.
        /// If the insertion point is less than 0 or bigger than Length the token will be added at the 
        /// beginning or end respectively.
        /// </summary>
        /// <param name="t">Token list to insert.</param>
        /// <param name="pos">Insertion position.</param>
        /// <returns>True on success. False on any error</returns>
        protected internal bool Insert(TToken t, int pos)
        {
            bool ret = false;
            lock (lockObj)
            {
                if (t != null)
                {
                    if (pos < 0) pos = 0;
                    if (pos >= Length) return Append(t);
                    int tokenStart = 0;
                    TToken posT = TokenAt(pos, out tokenStart);
                    if (posT != null)
                    {
                        TToken last = clipTokenList(t);
                        // list is terminated by a CRLF token:
                        if (last.IsNewLineToken)
                        {
                            Split(pos);
                            if (tail != null)
                            {
                                tail.Next = t;
                                t.Prev = tail;
                            }
                            else head = t;
                            tail = last;
                        }
                            // list is not terminated by a CRLF token:
                        else
                        {
                            // we have to split the token
                            if (pos > tokenStart && !posT.IsNewLineToken)
                            {
                                posT.Split(pos - tokenStart);
                                TToken nextPosT = posT.Next;
                                posT.Next = t;
                                t.Prev = posT;
                                nextPosT.Prev = last;
                                last.Next = nextPosT;
                            }
                                // insert before posT token:
                            else
                            {
                                if (posT.Prev != null)
                                {
                                    posT.Prev.Next = t;
                                    t.Prev = posT.Prev;
                                }
                                else head = t;
                                last.Next = posT;
                                posT.Prev = last;
                            }
                        }
                        Modified = true;
                        normalize();
                        ret = true;
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// Remove the given token from the line. Returns true on success or false if the passed
        /// token is not contained!
        /// </summary>
        /// <param name="t">Token to remove from the line</param>
        /// <returns></returns>
        protected internal bool Remove(TToken t)
        {
            bool ret = false;
            lock (lockObj)
            {
                TToken search = head;
                while (search != null && search != t)
                {
                    search = search.Next;
                }
                if (search != null)
                {
                    removeContainedToken(search);
                    Modified = true;
                    ret = true;
                }
            }
            return ret;
        }

        /// <summary>
        /// Sets the color of the text in this line starting at the given position up to the given length.
        /// If the start and / or end position lies inside an existing token, the respective token will be
        /// split and the color will be set accordingly.
        /// </summary>
        /// <param name="pos">Start position inside the line.</param>
        /// <param name="len">Length of the text.</param>
        /// <param name="col">Color to set.</param>
        /// <returns>True on success. False if len is less than 0.</returns>
        protected internal bool SetColor(int pos, int len, Color col)
        {
            bool ret = false;
            lock (lockObj)
            {
                if (pos < 0) pos = 0;
                if (pos > LastPos) pos = LastPos;
                if (len < 0) ret = false;
                if (len == 0) return true;
                else
                {
                    int startStPos = 0;
                    int endStPos = 0;
                    TToken start = TokenAt(pos, out startStPos);
                    TToken end = TokenAt(pos + len, out endStPos);
                    TToken colSt = start;
                    if (start != null)
                    {
                        Modified = true;
                        if (start == end)
                        {
                            start.Split(pos - startStPos + len);
                            if (start.Split(pos - startStPos))
                            {
                                colSt = start.Next;
                            }
                            colSt.Color = col;
                        }
                        else
                        {
                            if (start.Split(pos - startStPos))
                            {
                                colSt = start.Next;
                            }
                            if (end != null && endStPos != pos + len)
                            {
                                end.Split(pos - endStPos + len);
                                end.Color = col;
                            }
                            while (colSt != end)
                            {
                                colSt.Color = col;
                                colSt = colSt.Next;
                            }
                        }
                        Flatten();
                        ret = true;
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// Returns the token at the specified position of the line or null if the token doesn't exist. 
        /// If the passed character position is less than 0 the first token of the line is returned, but
        /// passing a position bigger than line.Length will always return null!
        /// </summary>
        /// <param name="i">Character position to search for.</param>
        /// <param name="startPos">Start position of the returned token inside the line.</param>
        /// <returns>The Token at the given position or null if it doesn't exist.</returns>
        public TToken TokenAt(int i, out int startPos)
        {
            TToken ret = null;
            lock (lockObj)
            {
                if (i < 0) i = 0;
                if (head == null)
                {
                    startPos = 0;
                }
                else
                {
                    TToken search = head;
                    int j = head.Length;
                    startPos = 0;
                    while (search != null && j <= i)
                    {
                        startPos += search.Length;
                        search = search.Next;
                        if (search != null) j += search.Length;
                    }
                    ret = search;
                    // special case for last position of a line:
                    if (search == null && tail != null && j == i)
                    {
                        startPos -= tail.Length;
                        ret = tail;
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// Removes the token at the specified index of the line and returns it without any references
        /// to the previous or next token it had in the line! Returns null if this token doesn't exist!
        /// </summary>
        /// <param name="i">Index of the token to remove</param>
        /// <returns>The removed token with Next and Prev set to null. Null if token doesn't exist.</returns>
        protected internal TToken RemoveAt(int i)
        {
            TToken ret = null;
            lock (lockObj)
            {
                int startPos;
                TToken search = TokenAt(i, out startPos);
                if (search != null)
                {
                    removeContainedToken(search);
                }
                search.Next = null;
                search.Prev = null;
                Modified = true;
                ret = search;
            }
            return ret;
        }

        /// <summary>
        /// "Disposes" a line by setting all references to null and all properties and fields to the
        /// default values. To be called when a line is deleted from a document.
        /// NOTE: Current implementation relies on coordinates of disposed lines - must not be reset!!!
        /// </summary>
        public virtual void Dispose()
        {
            lock (lockObj)
            {
                disposed = true;
                next = null;
                prev = null;
                head = null;
                tail = null;
                isVisible = false;
                modified = false;
            }
        }

        /***** VIRTUAL METHODS AND PROPERTIES: *****/

        /***** METHODS: *****/

        /// <summary>
        /// Inserts the given character at the given position of the line. Characters with pos less than 0 
        /// or pos bigger than Length are inserted at the beginning or the end of the line.
        /// The default implementation of Insert(...) always adds the given character to the token at the
        /// insertion position. If pos is at the end of the line the given character will be appended to the
        /// last non-newline token of the line. (if it doesn't exist it will be created)
        /// </summary>
        /// <param name="c">Character to insert.</param>
        /// <param name="pos">Position at which to insert.</param>
        /// <param name="col">Color of character to insert.</param>
        /// <returns>The token the character has been added to.</returns>
        protected internal virtual TToken Insert(char c, int pos, Color col)
        {
            lock (lockObj)
            {
                return Insert(c.ToString(), pos, col);
            }
        }

        /// <summary>
        /// Inserts the given String at the given position of the line. Strings with pos less than 0 
        /// or pos bigger than Length are inserted at the beginning or the end of the line.
        /// The default implementation of Insert always adds the given String to the token at the
        /// insertion position if the provided color and the color at the given position match - otherwise
        /// a new token with the given color will be added.
        /// NOTE: This implementation must not be used for inserting strings which contain newline characters 
        /// (such as '\r', '\n', ...)! For performance reasons the passed string will not be checked if it
        /// contains any of these and therefore it is possible to insert multiple newline characters which will
        /// lead to unpredictable results!
        /// </summary>
        /// <param name="s">String to insert.</param>
        /// <param name="pos">Position at which to insert.</param>
        /// <param name="col">Color of string to insert.</param>
        /// <returns>The token the String has been added to.</returns>
        protected internal virtual TToken Insert(String s, int pos, Color col)
        {
            lock (lockObj)
            {
                int length = Length;
                if (pos < 0) pos = 0;
                if (pos > length) pos = length;
                int tokenStart;
                TToken posToken = TokenAt(pos, out tokenStart);
                var insertT = new TToken();
                insertT.Value = s;
                insertT.Color = col;
                Modified = true;
                if (posToken != null)
                {
                    // we do not append or insert into newline tokens
                    if (posToken.IsNewLineToken)
                    {
                        // append to previous token
                        if (posToken.Prev != null)
                        {
                            if (posToken.Prev.Color == col)
                            {
                                posToken.Prev.Insert(s, posToken.Prev.Length);
                                return posToken.Prev;
                            }
                            else
                            {
                                Insert(insertT, pos);
                                return insertT;
                            }
                        }
                            // if there is no previous token - append new head
                        else
                        {
                            head = insertT;
                            head.Next = posToken;
                            posToken.Prev = head;
                            return insertT;
                        }
                    }
                    else
                    {
                        // insert into found token if colors match
                        if (posToken.Color == col)
                        {
                            posToken.Insert(s, pos - tokenStart);
                            return posToken;
                        }
                            // if pos is between tokens check color of previous token:
                        else if (pos == tokenStart && posToken.Prev != null && posToken.Prev.Color == col)
                        {
                            posToken.Prev.Insert(s, posToken.Prev.Length);
                            return posToken.Prev;
                        }
                            // otherwise insert new token:
                        else
                        {
                            Insert(insertT, pos);
                            return insertT;
                        }
                    }
                }
                    // insert at end of line
                else
                {
                    // line is completely empty:
                    if (head == null)
                    {
                        head = tail = insertT;
                        return insertT;
                    }
                        // no newline token at end --> append to last token
                    else
                    {
                        if (tail.Color == col)
                        {
                            tail.Insert(s, tail.Length);
                            return tail;
                        }
                        else
                        {
                            Insert(insertT, pos);
                            return insertT;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Deletes the characters from the given position up to the given length or the end of the line,
        /// which ever comes first. This method does not delete accross multiple lines!
        /// </summary>
        /// <param name="pos">Start position of deletion.</param>
        /// <param name="len">Number of characters to delete.</param>
        /// <returns>Number of deleted characters. (Returns 0 if pos is out of bounds)</returns>
        protected internal virtual int Delete(int pos, int len)
        {
            lock (lockObj)
            {
                if (pos < 0 || pos >= Length || len == 0)
                {
                    return 0;
                }
                else
                {
                    int startPosFrom;
                    int deleted = 0;
                    TToken fromT = TokenAt(pos, out startPosFrom);
                    TToken delT;
                    if (fromT != null)
                    {
                        if (fromT.IsNewLineToken)
                        {
                            deleted = Environment.NewLine.Length;
                            delT = fromT;
                            removeContainedToken(delT);
                            fromT = null;
                        }
                        else
                        {
                            deleted = fromT.Delete(pos - startPosFrom, len);
                            delT = fromT;
                            fromT = fromT.Next;
                            if (delT.Length == 0)
                            {
                                // delete token if empty
                                removeContainedToken(delT);
                            }
                        }
                    }
                    len -= deleted;
                    int d;
                    while (len > 0 && fromT != null)
                    {
                        d = fromT.Delete(0, len);
                        len -= d;
                        deleted += d;
                        delT = fromT;
                        fromT = fromT.Next;
                        if (delT.Length == 0)
                        {
                            // delete token if empty
                            removeContainedToken(delT);
                        }
                    }
                    normalize();
                    Modified = true;
                    return deleted;
                }
            }
        }

        /// <summary>
        /// "Flattens" the line by concatenating all adjacent tokens that have the same color. Override this 
        /// method to implement a different behaviour. Calling this method requires all line properties to be
        /// recalculated prior to rendering!
        /// </summary>
        protected internal virtual void Flatten()
        {
            lock (lockObj)
            {
                if (head != null)
                {
                    TToken newHead = tail = head.Clone();
                    TToken search = head;
                    StringBuilder b;
                    while (search != null)
                    {
                        b = new StringBuilder(search.Length);
                        while (search != null && !search.IsNewLineToken &&
                               search.Color.ToArgb() == tail.Color.ToArgb())
                        {
                            b.Append(search.Value);
                            search = search.Next;
                        }
                        tail.Value = b.ToString();
                        if (search != null)
                        {
                            tail.Next = search.Clone();
                            tail.Next.Prev = tail;
                            tail = tail.Next;
                            if (search.IsNewLineToken)
                            {
                                search = null;
                            }
                        }
                    }
                    head = newHead;
                    Modified = true;
                }
            }
        }

        /***** OVERLOADED OPERATORS: *****/

        /// <summary>
        /// Returns true if line l1 is contained before line l2.
        /// </summary>
        /// <param name="l1">Line to compare.</param>
        /// <param name="l2">Line to compare to.</param>
        /// <returns>True if line l1 is contained before l2. False otherwise.</returns>
        public static bool operator <(LineBase<TLine, TToken> l1, LineBase<TLine, TToken> l2)
        {
            if (l1 != l2)
            {
                while (l1 != null && l1 != l2)
                {
                    l1 = l1.Next;
                }
                if (l1 == l2) return true;
            }
            return false;
        }

        /// <summary>
        /// Returns true if the line l1 is contained after line l2.
        /// </summary>
        /// <param name="l1">Line to compare.</param>
        /// <param name="l2">Line to compare to.</param>
        /// <returns>True if line l1 is contained after l2. False otherwise.</returns>
        public static bool operator >(LineBase<TLine, TToken> l1, LineBase<TLine, TToken> l2)
        {
            if (l1 != l2)
            {
                while (l1 != null && l1 != l2)
                {
                    l1 = l1.Prev;
                }
                if (l1 == l2) return true;
            }
            return false;
        }

        /***** PROPERTIES: *****/
    }

    /// <summary>
    /// The class UpdateEventArgs encapsulates changes to the underlying data model (e.g. delete, insert, ...).
    /// UpdateEventArgs are sent to registered views via an UpdateEventHandler delegate (in the DocumentBase
    /// class) and are used to update the data view in a ColorTextBox as well as for undo / redo operations.
    /// Note that the Data field will only contain the changed lines when Action isn't set to UpdateAction.Ignore.
    /// This is because this data should be only used for undo / redo operations.
    /// </summary>
    public class UpdateEventArgs : EventArgs
    {
        /// <summary>
        /// Flag indicating if an UpdateEvent should be added to the redo or the undo buffer or to none of both.
        /// </summary>
        public UpdateAction Action;

        /// <summary>
        /// Contains the old value for Update events (old size, old style, ...). For Insert events this field 
        /// contains the inserted text and for Delete events the deleted text.
        /// </summary>
        public Object Data;

        /// <summary>
        /// This position is equal to the position defined by FromLine and FromPos - the UpdateEvent includes this
        /// reference as well for performance reasons (otherwise the GUI would need to get the line via the index!)
        /// </summary>
        public IPosition From;

        /// <summary>
        /// Start line index of update event (needed for undo and redo operations)
        /// </summary>
        public int FromLine;

        /// <summary>
        /// Number of affected lines.
        /// </summary>
        public int Lines;

        /// <summary>
        /// Flag indicating if an UpdateEvent should trigger a repaint of the changed area. Note that changes to
        /// the data are always rendered to the offscreen buffer - this flag only indicates if the changes should
        /// be rendered to the screen as well by invalidating and updating the affected area.
        /// </summary>
        public bool Repaint;

        /// <summary>
        /// This position is equal to the position defined by ToLine and ToPos - the UpdateEvent includes this
        /// reference as well for performance reasons (otherwise the GUI would need to get the line via the index!)
        /// </summary>
        public IPosition To;

        /// <summary>
        /// End line index of update event. (needed for undo and redo operations)
        /// </summary>
        public int ToLine;

        /// <summary>
        /// Type of this UpdateEvent.
        /// </summary>
        public UpdateEventType Type;

        /// <summary>
        /// Default constructor for an invalid UpdateEvent.
        /// </summary>
        public UpdateEventArgs()
        {
            Type = UpdateEventType.None;
            FromLine = 1;
            ToLine = 1;
            Action = UpdateAction.NonAtomic;
            Repaint = true;
        }

        /// <summary>
        /// Constructor for setting all UpdateEventArgs fields.
        /// </summary>
        /// <param name="type">Type of the UpdateEvent.</param>
        /// <param name="from">Start position reference of the update event.</param>
        /// <param name="to">End position reference of the update event.</param>
        /// <param name="fromL">Start line index of the update event.</param>
        /// <param name="toL">End line index of update event.</param>
        /// <param name="lines">Number of affected lines.</param>
        /// <param name="data">Old value for Update events, or null.</param>
        /// <param name="action">Action to perform for the UpdateEvent.</param>
        /// <param name="repaint">Indicates if changes should be painted to the screen.</param>
        public UpdateEventArgs(UpdateEventType type, IPosition from, IPosition to,
                               int fromL, int toL, int lines, Object data, UpdateAction action, bool repaint)
        {
            Type = type;
            From = from;
            To = to;
            FromLine = fromL;
            ToLine = toL;
            Lines = lines;
            Data = data;
            Action = action;
            Repaint = repaint;
        }
    }

    public abstract class DocumentBase<TLine, TToken> : IDocument
        where TToken : TokenBase<TToken>, new()
        where TLine : LineBase<TLine, TToken>, new()
    {
        /***** PROTECTED FIELDS *****/

        protected CharacterCasing casing; // Determines if characters should be inserted normally
        protected Color color; // Currently active color
        protected TLine first; // Reference to first line in the document
        protected TLine last; // Reference to last line in the document
        protected int lines; // The total number of lines currently contained in the document

        protected Object lockObj = new Object();

        /***** CONSTRUCTORS *****/

        /// <summary>
        /// Default Constructor: constructs an empty document.
        /// </summary>
        protected DocumentBase()
        {
            casing = CharacterCasing.Normal;
            first = last = new TLine();
            lock (first)
            {
                first.Append(new TToken());
            }
            color = Color.Black;
            lines = 0;
        }

        /// <summary>
        /// Gets or sets the first line in the document.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TLine First
        {
            get
            {
                lock (lockObj)
                {
                    TLine ret = null;
                    lock (first)
                    {
                        ret = first;
                    }
                    return ret;
                }
            }
            set
            {
                lock (lockObj)
                {
                    lock (first)
                    {
                        Clear();
                        if (value != null)
                        {
                            Insert(value, first, 0, UpdateAction.NonAtomic, true);
                        }
                        notifyNewDocument();
                    }
                }
            }
        }

        /// <summary>
        /// Determines if inserted characters should be left alone or converted to uppercase or lowercase.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CharacterCasing CharacterCasing
        {
            get { return casing; }
            set { casing = value; }
        }

        /// <summary>
        /// Returns the last line in the document.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TLine Last
        {
            get
            {
                lock (lockObj)
                {
                    TLine ret = null;
                    lock (last)
                    {
                        ret = last;
                    }
                    return ret;
                }
            }
        }

        /// <summary>
        /// Read-only indexer for array-like access to the stored lines. Throws an IndexOutOfRangeException 
        /// if the given index is out of range. NOTE: Line indices start at index 1 not 0!
        /// </summary>
        /// <param name="index">Index of the line to set or get.</param>
        /// <returns>Line at the given index.</returns>
        public TLine this[int index]
        {
            get
            {
                lock (lockObj)
                {
                    if (index < 1 || index > LineCount)
                    {
                        throw new IndexOutOfRangeException();
                    }
                    else
                    {
                        TLine search;
                        // count from first line forwards if index is less than LineCount / 2
                        if (index <= LineCount/2)
                        {
                            search = first;
                            int i = 1;
                            while (i != index && search != null)
                            {
                                search = search.Next;
                                i++;
                            }
                        }
                            // count from last line backwards if index is greater than LineCount / 2
                        else
                        {
                            search = last;
                            int i = LineCount;
                            while (i != index && search != null)
                            {
                                search = search.Prev;
                                i--;
                            }
                        }
                        return search;
                    }
                }
            }
        }

        #region IDocument Members

        /// <summary>
        /// An UpdateEvent is fired each time a change to the data model occurs.
        /// </summary>
        public event UpdateEventHandler UpdateEvent;

        /***** PUBLIC METHODS *****/

        /// <summary>
        /// This method can be used to undo operations determined by UpdateEventArgs objects. Override if
        /// implementing user defined UpdateEventArgs and call this base implementation for update events that
        /// are not handled by the overridden implementation!
        /// NOTE: Undo and Redo operations should be atomic, i.e. for each operation that can be undone the 
        /// inverse operation should be a single operation (in the redo buffer) as well!
        /// </summary>
        /// <param name="args">UpdateEventArgs for which to fire an inverse operation.</param>
        public virtual void InvokeInverseOp(UpdateEventArgs args)
        {
            lock (lockObj)
            {
                switch (args.Type)
                {
                    case UpdateEventType.Insert:
                        {
                            var fromP = new Position<TLine, TToken>();
                            var toP = new Position<TLine, TToken>();
                            try
                            {
                                fromP.Line = this[args.FromLine];
                                toP.Line = LineRelativeTo(fromP.Line, args.FromLine, args.ToLine);
                                fromP.LinePos = args.From.LinePos;
                                toP.LinePos = args.To.LinePos;
                                Delete(fromP, toP, args.Action, true);
                            }
                            catch (IndexOutOfRangeException)
                            {
                                return;
                            }
                        }
                        break;
                    case UpdateEventType.Delete:
                        {
                            TLine fromL = null;
                            try
                            {
                                fromL = this[args.FromLine];
                                Insert((TLine) args.Data, fromL, args.From.LinePos, args.Action, true);
                            }
                            catch (IndexOutOfRangeException)
                            {
                                return;
                            }
                        }
                        break;
                    case UpdateEventType.Replace:
                        {
                            var fromP = new Position<TLine, TToken>();
                            var toP = new Position<TLine, TToken>();
                            try
                            {
                                fromP.Line = this[args.FromLine];
                                toP.Line = LineRelativeTo(fromP.Line, args.FromLine, args.ToLine);
                                fromP.LinePos = args.From.LinePos;
                                toP.LinePos = args.To.LinePos;
                                Replace(fromP, toP, (TLine) (args.Data), args.Action, true);
                            }
                            catch (IndexOutOfRangeException)
                            {
                                return;
                            }
                        }
                        break;
                    case UpdateEventType.UpdateFont:
                        {
                            /* TODO: implement global font change */
                        }
                        break;
                        // Color changes are being undone by deleting the content defined by the positions in args
                        // and then restoring the original lines stored in the Data field!
                    case UpdateEventType.UpdateColor:
                        {
                            var fP = new Position<TLine, TToken>();
                            var tP = new Position<TLine, TToken>();
                            try
                            {
                                TLine startL = fP.Line = this[args.FromLine];
                                tP.Line = LineRelativeTo(startL, args.FromLine, args.ToLine);
                                fP.LinePos = args.From.LinePos;
                                tP.LinePos = args.To.LinePos;
                                TLine currData = SubLines(fP, tP);
                                Delete(fP, tP, UpdateAction.NonAtomic, true);
                                Insert((TLine) (args.Data), startL, args.From.LinePos, UpdateAction.NonAtomic, true);
                                args.Repaint = true;
                                args.Data = currData;
                                OnUpdateEvent(args);
                            }
                            catch (IndexOutOfRangeException)
                            {
                                return;
                            }
                        }
                        break;
                    case UpdateEventType.None:
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Clears all content from the current document!
        /// </summary>
        public void Clear()
        {
            lock (lockObj)
            {
                if (first != null)
                {
                    lock (first)
                    {
                        lock (last)
                        {
                            TLine search = first;
                            TLine dispLine;
                            while (search != null)
                            {
                                dispLine = search;
                                search = search.Next;
                                dispLine.Dispose();
                            }
                            first = null;
                            last = null;
                            last = first = new TLine();
                            first.First = new TToken();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Returns the text of the given length starting at the given character position inside the document.
        /// NOTE: The returned string maybe longer than the given dist parameter depending on the platforms newline
        /// encoding!
        /// </summary>
        /// <param name="charPos">Start position.</param>
        /// <param name="dist">Distance to the given start position.</param>
        /// <returns>Substring between the positions defined by the parameters.</returns>
        public String SubString(int charPos, int dist)
        {
            lock (lockObj)
            {
                String ret = String.Empty;
                Position<TLine, TToken> start = GetPosition(charPos);
                if (start != null && start.Line != null)
                {
                    lock (start.Line)
                    {
                        Position<TLine, TToken> end = GetRelativePos(start.Line, start.LinePos, dist);
                        if (end != null && end.Line != null)
                        {
                            lock (end.Line)
                            {
                                ret = SubString(start, end);
                            }
                        }
                    }
                }
                return ret;
            }
        }

        /// <summary>
        /// Returns the character at the given text position inside the document. If the given position is less than
        /// 0 or larger than the documents last position the character at the first or last position respectively
        /// will be returned. This method treats newline strings as a single character!
        /// </summary>
        /// <param name="charPos">Text position for which to get the character.</param>
        /// <returns>Character at the given position.</returns>
        public char CharAt(int charPos)
        {
            lock (lockObj)
            {
                return CharAt(GetPosition(charPos));
            }
        }

        /// <summary>
        /// Gets or sets the Text contained in this document.
        /// </summary>
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public String Text
        {
            get
            {
                lock (lockObj)
                {
                    var text = new StringBuilder(String.Empty);
                    TLine search = first;
                    while (search != null)
                    {
                        lock (search)
                        {
                            text.Append(search);
                        }
                        search = search.Next;
                    }
                    return text.ToString();
                }
            }
            set
            {
                lock (lockObj)
                {
                    Clear();
                    if (value != null)
                    {
                        Insert(value, first, 0, UpdateAction.NonAtomic, false);
                    }
                    notifyNewDocument();
                }
            }
        }

        /// <summary>
        /// Gets or sets the lines in this document.
        /// </summary>
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public String[] Lines
        {
            get
            {
                lock (lockObj)
                {
                    var strList = new List<String>();
                    if (first != null)
                    {
                        TLine search = first;
                        while (search != null)
                        {
                            lock (search)
                            {
                                strList.Add(search.ToString().TrimEnd(new[] {'\n', '\r'}));
                            }
                            search = search.Next;
                        }
                        return strList.ToArray();
                    }
                    return new[] {String.Empty};
                }
            }
            set
            {
                lock (lockObj)
                {
                    Clear();
                    if (value != null)
                    {
                        var sb = new StringBuilder();
                        foreach (string s in value)
                        {
                            sb.Append(s + Environment.NewLine);
                        }
                        sb.Remove(sb.Length - Environment.NewLine.Length, Environment.NewLine.Length);
                        Insert(sb.ToString(), first, 0, UpdateAction.NonAtomic, false);
                    }
                    notifyNewDocument();
                }
            }
        }

        /// <summary>
        /// Gets or sets the lines in the document as a reference to the first line.
        /// NOTE: Caller has to ensure the content of the document is valid (each line terminated with
        /// a newline token, ...) when setting the content
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ILine FirstLine
        {
            get { return first; }
            set
            {
                var newFirst = value as TLine;
                if (newFirst != null)
                {
                    First = newFirst;
                }
            }
        }

        /// <summary>
        /// Gets the last line contained in the document.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ILine LastLine
        {
            get { return last; }
        }

        /// <summary>
        /// Returns the total number of lines contained in the document.
        /// </summary>
        public int LineCount
        {
            get
            {
                lock (lockObj)
                {
                    return lines;
                }
            }
        }

        /// <summary>
        /// Gets or sets the currently active color to be used for insert operations.
        /// </summary>
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color Color
        {
            get
            {
                lock (lockObj)
                {
                    return color;
                }
            }
            set
            {
                lock (lockObj)
                {
                    if (value != null)
                    {
                        color = value;
                    }
                }
            }
        }

        /// <summary>
        /// Returns the length of the whole document including newline characters.
        /// </summary>
        public int Length
        {
            get
            {
                lock (lockObj)
                {
                    TLine search = First;
                    int len = 0;
                    while (search != null)
                    {
                        lock (search)
                        {
                            len += search.Length;
                        }
                        search = search.Next;
                    }
                    return len;
                }
            }
        }

        /// <summary>
        /// Returns the last text position inside the document
        /// </summary>
        public int LastPos
        {
            get
            {
                lock (lockObj)
                {
                    TLine search = First;
                    int pos = 0;
                    while (search != null)
                    {
                        lock (search)
                        {
                            pos += search.LastPos + 1;
                        }
                        search = search.Next;
                    }
                    return pos - 1;
                }
            }
        }

        #endregion

        /// <summary>
        /// Inserts the given text into the passed line at the given position. This method adds a new line for
        /// each CRLF encountered in the passed String. If the length of the String to insert is bigger than 1 
        /// the default implementation adds the given text as a new token at the specified position of the line
        /// - more tokens are only added when one of the defined newline strings is encountered in the given text.
        /// Strings that contain only a single character are added to the token at the current position.
        /// If the currently active document color and the color at the insertion point differ there will always 
        /// be a new token inserted with the color set to the currently active color!
        /// Newline strings include '\f', '\r', '\n' and "\r\n" and are converted to the platform independent
        /// value System.Environment.Newline when inserted.
        /// All other unicode characters are inserted unchanged!
        /// </summary>
        /// <param name="text">Text to insert into the given line.</param>
        /// <param name="atL">The line to insert into - must be contained in document.</param>
        /// <param name="linePos">Position inside the line.</param>
        /// <param name="action">Indicates if operation should be added to Undo buffer.</param>
        /// <param name="repaint">Indicates if insertion should lead to a redraw of the affected area.</param>
        /// <returns>The number of inserted characters (linefeeds count as one character)</returns>
        public virtual int Insert(String text, TLine atL, int linePos, UpdateAction action, bool repaint)
        {
            lock (lockObj)
            {
                int ins = 0;
                if (atL != null)
                {
                    lock (atL)
                    {
                        int fromInd = IndexOf(atL);
                        if (fromInd > 0 && text != null && text.Length > 0)
                        {
                            if (casing != CharacterCasing.Normal) text = changeCasing(text);
                            var args = new UpdateEventArgs();
                            args.Type = UpdateEventType.Insert;
                            args.Action = action;
                            args.Repaint = repaint;
                            args.From = new Position<TLine, TToken>(0, atL, linePos);
                            args.FromLine = fromInd;
                            args.ToLine = fromInd;
                            int newPos = linePos;
                            char insert = text[0];
                            TLine startL = atL;
                            TLine tmp = atL;
                            if (text.Length == 1 && insert != '\n' && insert != '\r' && insert != '\f')
                            {
                                lock (tmp)
                                {
                                    tmp.Insert(insert, linePos, Color);
                                }
                                newPos++;
                                ins = 1;
                            }
                            else
                            {
                                List<TToken> inserts = tokenizeString(text);
                                int tLen;
                                foreach (TToken t in inserts)
                                {
                                    if (t != null && t.Value.Length > 0)
                                    {
                                        if (t.IsNewLineToken)
                                        {
                                            bool insVar = false;
                                            lock (tmp)
                                            {
                                                insVar = tmp.Insert(t, newPos);
                                            }
                                            if (insVar)
                                            {
                                                if (last == tmp) last = tmp.Next;
                                                tmp = tmp.Next;
                                                newPos = 0;
                                                args.Lines++;
                                                ins++;
                                            }
                                        }
                                        else
                                        {
                                            tLen = t.Length;
                                            lock (tmp)
                                            {
                                                tmp.Insert(t, newPos);
                                            }
                                            newPos += tLen;
                                            ins += tLen;
                                        }
                                    }
                                }
                            }
                            args.To = new Position<TLine, TToken>(0, tmp, newPos);
                            args.ToLine = args.FromLine + args.Lines;
                            if (action != UpdateAction.NonAtomic)
                            {
                                //args.Data = SubLines(GetRelativePos(startL, linePos, 0), GetRelativePos(startL, linePos, ins));//b.ToString();
                            }
                            lines += args.Lines;
                            OnUpdateEvent(args);
                        }
                    }
                }
                return ins;
            }
        }

        /// <summary>
        /// Inserts the line list insertL into the given line at the given position. If any of the lines
        /// contains multiple CRLF tokens the token list will be clipped at the first occurence of a CRLF token.
        /// Furthermore this implementation does currently not check if each token list of a line is terminated
        /// by a CRLF token (which may result in the insertion of invalid lines!)
        /// </summary>
        /// <param name="insertL">Line list to insert.</param>
        /// <param name="atL">Line to insert into.</param>
        /// <param name="linePos">Position inside the line.</param>
        /// <param name="action">Indicates if operation should be added to Undo buffer.</param>
        /// <param name="repaint">Indicates if insertion should lead to a redraw of the affected area.</param>
        /// <returns>The number of inserted characters (linefeeds count as one character)</returns>
        public virtual int Insert(TLine insertL, TLine atL, int linePos, UpdateAction action, bool repaint)
        {
            lock (lockObj)
            {
                int ins = 0;
                if (atL != null)
                {
                    lock (atL)
                    {
                        int fromInd = IndexOf(atL);
                        if (fromInd > 0)
                        {
                            if (casing != CharacterCasing.Normal) changeCasing(insertL);
                            var args = new UpdateEventArgs();
                            args.Type = UpdateEventType.Insert;
                            args.Action = action;
                            args.Repaint = repaint;
                            args.FromLine = fromInd;
                            args.From = new Position<TLine, TToken>(0, atL, linePos);
                            args.ToLine = fromInd;
                            TLine curr = insertL;
                            TLine tmp = atL;
                            bool valid;
                            int length;
                            while (curr != null)
                            {
                                valid = curr.IsValid;
                                length = curr.Length;
                                lock (tmp)
                                {
                                    tmp.Insert(curr.First, linePos);
                                }
                                // we inserted a line containing a CRLF token
                                if (valid)
                                {
                                    if (last == tmp) last = tmp.Next;
                                    args.Lines++;
                                    tmp = tmp.Next;
                                    linePos = 0;
                                    ins += (length - Environment.NewLine.Length) + 1;
                                }
                                else
                                {
                                    ins += length;
                                    linePos += length;
                                }
                                curr = curr.Next;
                            }
                            args.To = new Position<TLine, TToken>(0, tmp, linePos);
                            args.ToLine = args.FromLine + args.Lines;
                            if (action != UpdateAction.NonAtomic)
                            {
                                //args.Data = SubLines(GetRelativePos(tmp, linePos, 0), GetRelativePos(tmp, linePos, ins));
                            }
                            lines += args.Lines;
                            OnUpdateEvent(args);
                            return ins;
                        }
                    }
                }
                return ins;
            }
        }

        /// <summary>
        /// Inserts the token list into the given line at the given position. If the passed token list contains
        /// multiple CRLF tokens the list will be clipped at the first occurence of a CRLF token!
        /// </summary>
        /// <param name="insertT">Token list to insert.</param>
        /// <param name="atL">Line to insert into.</param>
        /// <param name="linePos">Position inside the line.</param>
        /// <param name="action">Indicates if operation should be added to Undo buffer.</param>
        /// <param name="repaint">Indicates if insertion should lead to a redraw of the affected area.</param>
        /// <returns>The number of inserted characters (linefeeds count as one character)</returns>
        public virtual int Insert(TToken insertT, TLine atL, int linePos, UpdateAction action, bool repaint)
        {
            lock (lockObj)
            {
                int ins = 0;
                if (atL != null)
                {
                    lock (atL)
                    {
                        int fromInd = IndexOf(atL);
                        if (fromInd > 0)
                        {
                            if (casing != CharacterCasing.Normal) changeCasing(insertT);
                            var args = new UpdateEventArgs();
                            args.Type = UpdateEventType.Insert;
                            args.Action = action;
                            args.Repaint = repaint;
                            args.FromLine = fromInd;
                            args.From = new Position<TLine, TToken>(0, atL, linePos);
                            args.ToLine = fromInd;
                            atL.Insert(insertT, linePos);
                            TToken search = insertT;
                            TToken prev = insertT;
                            int length;
                            while (search != null)
                            {
                                prev = search;
                                search = search.Next;
                                length = search.Length;
                                ins += length;
                                linePos += length;
                            }
                            if (prev != null && prev.IsNewLineToken)
                            {
                                args.Lines++;
                                linePos = 0;
                                ins++;
                            }
                            if (args.Lines == 0) args.To = new Position<TLine, TToken>(0, atL, linePos);
                            else args.To = new Position<TLine, TToken>(0, atL.Next, linePos);
                            args.ToLine = args.FromLine + args.Lines;
                            if (action != UpdateAction.NonAtomic)
                            {
                                //args.Data = SubLines(GetRelativePos(atL, linePos, 0), GetRelativePos(atL, linePos, ins));
                            }
                            lines += args.Lines;
                            OnUpdateEvent(args);
                            return ins;
                        }
                    }
                }
                return ins;
            }
        }

        /// <summary>
        /// Replaces the text between the given positions with the passed text by calling the appropriate
        /// Delete and Insert methods.
        /// </summary>
        /// <param name="fromPos">Start position of text to replace.</param>
        /// <param name="toPos">End position of text to replace.</param>
        /// <param name="text">Text to insert instead of current text.</param>
        /// <param name="action">Indicates if operation should be added to undo / redo buffer.</param>
        /// <param name="repaint">Indicates if replacing should redraw the affected area.</param>
        /// <returns>True on success.</returns>
        public virtual bool Replace(Position<TLine, TToken> fromPos, Position<TLine, TToken> toPos,
                                    String text, UpdateAction action, bool repaint)
        {
            lock (lockObj)
            {
                if (fromPos == null || toPos == null || fromPos.Line == null || toPos.Line == null || text == null)
                    return false;
                bool ret = false;
                lock (fromPos.Line)
                {
                    lock (toPos.Line)
                    {
                        if (!(fromPos < toPos)) switchPositions(ref fromPos, ref toPos);
                        int fromInd = IndexOf(fromPos.Line);
                        if (fromInd != -1)
                        {
                            var args = new UpdateEventArgs();
                            if (action != UpdateAction.NonAtomic)
                            {
                                args.Data = SubLines(fromPos, toPos);
                            }
                            args.Type = UpdateEventType.Replace;
                            args.Action = action;
                            args.Repaint = repaint;
                            args.FromLine = fromInd;
                            args.From = fromPos;
                            args.To = toPos;
                            if (text.Length == 0)
                            {
                                Delete(fromPos, toPos, action, true);
                                return true;
                            }
                            int len = Delete(fromPos, toPos, UpdateAction.NonAtomic, false);
                            if (len > 0)
                            {
                                len = Insert(text, fromPos.Line, fromPos.LinePos, UpdateAction.NonAtomic, true);
                                Position<TLine, TToken> insToPos = GetRelativePos(fromPos.Line, fromPos.LinePos, len);
                                args.ToLine = IndexRelativeTo(fromPos.Line, fromInd, insToPos.Line, true);
                                args.To = insToPos;
                                args.Lines = args.ToLine - args.FromLine;
                                OnUpdateEvent(args);
                                ret = true;
                            }
                        }
                    }
                }
                return ret;
            }
        }

        /// <summary>
        /// Replaces the text between the given positions with the passed text by calling the appropriate
        /// Delete and Insert methods.
        /// </summary>
        /// <param name="fromPos">Start position of text to replace.</param>
        /// <param name="toPos">End position of text to replace.</param>
        /// <param name="insertL">Lines to insert instead of current text.</param>
        /// <param name="action">Indicates if operation should be added to undo / redo buffer.</param>
        /// <param name="repaint">Indicates if replacing should redraw the affected area.</param>
        /// <returns>True on success.</returns>
        public virtual bool Replace(Position<TLine, TToken> fromPos, Position<TLine, TToken> toPos,
                                    TLine insertL, UpdateAction action, bool repaint)
        {
            lock (lockObj)
            {
                if (fromPos == null || toPos == null || fromPos.Line == null || toPos.Line == null || insertL == null)
                    return false;
                bool ret = false;
                lock (fromPos.Line)
                {
                    lock (toPos.Line)
                    {
                        if (!(fromPos < toPos)) switchPositions(ref fromPos, ref toPos);
                        int fromInd = IndexOf(fromPos.Line);
                        if (fromInd != -1)
                        {
                            var args = new UpdateEventArgs();
                            if (action != UpdateAction.NonAtomic)
                            {
                                args.Data = SubLines(fromPos, toPos);
                            }
                            args.Type = UpdateEventType.Replace;
                            args.Action = action;
                            args.Repaint = repaint;
                            args.FromLine = fromInd;
                            args.From = fromPos;
                            args.To = toPos;
                            int len = Delete(fromPos, toPos, UpdateAction.NonAtomic, false);
                            if (len > 0)
                            {
                                len = Insert(insertL, fromPos.Line, fromPos.LinePos, UpdateAction.NonAtomic, true);
                                Position<TLine, TToken> insToPos = GetRelativePos(fromPos.Line, fromPos.LinePos, len);
                                args.ToLine = IndexRelativeTo(fromPos.Line, fromInd, insToPos.Line, true);
                                args.To = insToPos;
                                args.Lines = args.ToLine - args.FromLine;
                                OnUpdateEvent(args);
                                ret = true;
                            }
                        }
                    }
                }
                return ret;
            }
        }

        /// <summary>
        /// Replaces the text between the given positions with the passed text by calling the appropriate
        /// Delete and Insert methods.
        /// </summary>
        /// <param name="fromPos">Start position of text to replace.</param>
        /// <param name="toPos">End position of text to replace.</param>
        /// <param name="insertT">Tokens to insert instead of current text.</param>
        /// <param name="action">Indicates if operation should be added to undo / redo buffer.</param>
        /// <param name="repaint">Indicates if replacing should redraw the affected area.</param>
        /// <returns>True on success.</returns>
        public virtual bool Replace(Position<TLine, TToken> fromPos, Position<TLine, TToken> toPos,
                                    TToken insertT, UpdateAction action, bool repaint)
        {
            lock (lockObj)
            {
                if (fromPos == null || toPos == null || fromPos.Line == null || toPos.Line == null || insertT == null)
                    return false;
                bool ret = false;
                lock (fromPos.Line)
                {
                    lock (toPos.Line)
                    {
                        if (!(fromPos < toPos)) switchPositions(ref fromPos, ref toPos);
                        int fromInd = IndexOf(fromPos.Line);
                        if (fromInd != -1)
                        {
                            var args = new UpdateEventArgs();
                            if (action != UpdateAction.NonAtomic)
                            {
                                args.Data = SubLines(fromPos, toPos);
                            }
                            args.Type = UpdateEventType.Replace;
                            args.Action = action;
                            args.Repaint = repaint;
                            args.FromLine = fromInd;
                            args.From = fromPos;
                            args.To = toPos;
                            int len = Delete(fromPos, toPos, UpdateAction.NonAtomic, false);
                            if (len > 0)
                            {
                                len = Insert(insertT, fromPos.Line, fromPos.LinePos, UpdateAction.NonAtomic, true);
                                Position<TLine, TToken> insToPos = GetRelativePos(fromPos.Line, fromPos.LinePos, len);
                                args.ToLine = IndexRelativeTo(fromPos.Line, fromInd, insToPos.Line, true);
                                args.To = insToPos;
                                args.Lines = args.ToLine - args.FromLine;
                                OnUpdateEvent(args);
                                ret = true;
                            }
                        }
                    }
                }
                return ret;
            }
        }

        /// <summary>
        /// Deletes the given amount of characters starting at the given line and position.
        /// </summary>
        /// <param name="line">Line at which to start deletion.</param>
        /// <param name="linePos">Position inside line.</param>
        /// <param name="length">Number of characters to delete</param>
        /// <param name="action">Indicates if operation should be added to Undo buffer.</param>
        /// <param name="repaint">Indicates if deletion should lead to a redraw of the affected area.</param>
        /// <returns>The number of deleted characters.</returns>
        public virtual int Delete(TLine line, int linePos, int length, UpdateAction action, bool repaint)
        {
            lock (lockObj)
            {
                int ret = 0;
                if (line != null)
                {
                    lock (line)
                    {
                        var fromP = new Position<TLine, TToken>();
                        fromP.Line = line;
                        fromP.LinePos = linePos;
                        Position<TLine, TToken> toP = GetRelativePos(line, linePos, length);
                        if (toP != null && toP.Line != null)
                        {
                            lock (toP.Line)
                            {
                                ret = Delete(fromP, toP, action, repaint);
                            }
                        }
                    }
                }
                return ret;
            }
        }

        /// <summary>
        /// Deletes the content between the two given positions. This implementation should be significantly 
        /// faster than the Delete(TLine line, int linePos, int length, bool undo) - method (especially for 
        /// deleting across multiple lines!) and thus should be used whenever possible!
        /// </summary>
        /// <param name="fromPos">Start position.</param>
        /// <param name="toPos">End position.</param>
        /// <param name="action">Indicates if operation should be added to Undo buffer.</param>
        /// <param name="repaint">Indicates if deletion should lead to a redraw of the affected area.</param>
        /// <returns>Returns the number of deleted characters.</returns>
        public virtual int Delete(Position<TLine, TToken> fromPos, Position<TLine, TToken> toPos, UpdateAction action,
                                  bool repaint)
        {
            lock (lockObj)
            {
                if (fromPos == null || toPos == null || fromPos.Line == null || toPos.Line == null) return 0;
                int del = 0;
                lock (fromPos.Line)
                {
                    lock (toPos.Line)
                    {
                        // switch positions if fromPos comes after toPos:
                        if (!(fromPos < toPos)) switchPositions(ref fromPos, ref toPos);
                        int fromInd = IndexOf(fromPos.Line);
                        if (fromInd != -1)
                        {
                            var args = new UpdateEventArgs();
                            if (action != UpdateAction.NonAtomic)
                            {
                                TLine undoLines = SubLines(fromPos, toPos);
                                args.Data = undoLines;
                            }
                            del = fromPos.DistanceTo(toPos);
                            args.Type = UpdateEventType.Delete;
                            args.Action = action;
                            args.Repaint = repaint;
                            args.FromLine = fromInd;
                            args.From = fromPos;
                            // if positions are on the same line:
                            if (fromPos.Line == toPos.Line)
                            {
                                fromPos.Line.Delete(fromPos.LinePos, toPos.LinePos - fromPos.LinePos);
                            }
                            else
                            {
                                TLine search = fromPos.Line.Next;
                                while (search != null && search != toPos.Line)
                                {
                                    search.IsVisible = false;
                                    search = search.Next;
                                    args.Lines++;
                                }
                                // delete all content from the lines:
                                fromPos.Line.Delete(fromPos.LinePos, fromPos.Line.Length);
                                toPos.Line.Delete(0, toPos.LinePos);
                                if (fromPos.Line.Next != toPos.Line)
                                {
                                    fromPos.Line.Next = toPos.Line;
                                    toPos.Line.Prev = fromPos.Line;
                                    TLine sLine = fromPos.Line.Next;
                                    while (sLine != null && sLine != toPos.Line)
                                    {
                                        TLine dispLine = sLine;
                                        sLine = sLine.Next;
                                        dispLine.Dispose();
                                    }
                                }
                                // finally add toPos.Line to fromPos.Line
                                if (fromPos.Line.AddNext()) args.Lines++;
                                if (fromPos.Line.Next == null) last = fromPos.Line;
                            }
                            args.ToLine = args.FromLine;
                            args.To = args.From;
                            lines -= args.Lines;
                            OnUpdateEvent(args);
                        }
                    }
                }
                return del;
            }
        }

        /// <summary>
        /// Sets the color of the text starting at the given line and position up to the given length to
        /// the supplied new color. The default implementation does not support undoing color changes!
        /// </summary>
        /// <param name="col">The new color to set.</param>
        /// <param name="line">Start line of the color change.</param>
        /// <param name="linePos">Start position of the color change.</param>
        /// <param name="length">Length of text for which to set the new color.</param>
        /// <param name="action">Indicates if color change event should be added to Undo buffer (the default
        /// implementation ignores this flag!)</param>
        /// <param name="repaint">Indicates if color change should trigger a redraw of the affected area.</param>
        /// <returns>True on success. False if the color could not be changed.</returns>
        public virtual bool SetColor(Color col, TLine line, int linePos, int length, UpdateAction action, bool repaint)
        {
            lock (lockObj)
            {
                bool ret = false;
                if (line != null)
                {
                    lock (line)
                    {
                        var fromP = new Position<TLine, TToken>();
                        fromP.Line = line;
                        fromP.LinePos = linePos;
                        Position<TLine, TToken> toP = GetRelativePos(line, linePos, length);
                        if (toP != null && toP.Line != null)
                        {
                            lock (toP.Line)
                            {
                                ret = SetColor(col, fromP, toP, action, repaint);
                            }
                        }
                    }
                }
                return ret;
            }
        }

        /// <summary>
        /// Sets the color of the text between the given positions to the supplied new color.
        /// The default implementation does not support undoing color changes!
        /// </summary>
        /// <param name="col">The new color to set.</param>
        /// <param name="fromPos">Start position.</param>
        /// <param name="toPos">End position.</param>
        /// <param name="action">Indicates if color change event should be added to Undo buffer (the default
        /// implementation ignores this flag!)</param>
        /// <param name="repaint">Indicates if color change should trigger a redraw of the affected area.</param>
        /// <returns>True on success. False if the color could not be changed.</returns>
        public virtual bool SetColor(Color col, Position<TLine, TToken> fromPos, Position<TLine, TToken> toPos,
                                     UpdateAction action, bool repaint)
        {
            lock (lockObj)
            {
                if (fromPos == null || toPos == null || fromPos.Line == null || toPos.Line == null) return false;
                bool ret = false;
                lock (fromPos.Line)
                {
                    lock (toPos.Line)
                    {
                        // switch positions if fromPos comes after toPos:
                        if (!(fromPos < toPos)) switchPositions(ref fromPos, ref toPos);
                        int fromInd = IndexOf(fromPos.Line);
                        int toInd = IndexRelativeTo(fromPos.Line, fromInd, toPos.Line, true);
                        if (fromInd != -1 && toInd != -1)
                        {
                            ret = true;
                            var args = new UpdateEventArgs();
                            args.Type = UpdateEventType.UpdateColor;
                            args.Action = action;
                            args.Repaint = repaint;
                            args.FromLine = fromInd;
                            args.From = fromPos;
                            args.ToLine = toInd;
                            args.To = toPos;
                            if (action != UpdateAction.NonAtomic)
                            {
                                args.Data = SubLines(fromPos, toPos);
                            }
                            TLine curr = fromPos.Line;
                            int linePos = fromPos.LinePos;
                            while (curr != null && curr != toPos.Line && ret)
                            {
                                lock (curr)
                                {
                                    ret = curr.SetColor(linePos, curr.Length, col);
                                    linePos = 0;
                                    args.Lines++;
                                }
                                curr = curr.Next;
                            }
                            if (ret && curr != null)
                            {
                                lock (curr)
                                {
                                    ret = curr.SetColor(linePos, (toPos.LinePos - linePos), col);
                                }
                            }
                            if (ret) OnUpdateEvent(args);
                        }
                    }
                }
                return ret;
            }
        }

        /// <summary>
        /// Returns the text inside the document contained between the two given positions.
        /// </summary>
        /// <param name="fromPos">Start position.</param>
        /// <param name="toPos">End position.</param>
        /// <returns>Text between fromPos and toPos.</returns>
        public String SubString(Position<TLine, TToken> fromPos, Position<TLine, TToken> toPos)
        {
            lock (lockObj)
            {
                if (fromPos == null || fromPos.Line == null || toPos == null || toPos.Line == null) return String.Empty;
                String ret = String.Empty;
                lock (fromPos.Line)
                {
                    lock (toPos.Line)
                    {
                        // switch positions if fromPos comes after toPos:
                        if (!(fromPos < toPos)) switchPositions(ref fromPos, ref toPos);
                        // if positions are on the same line:
                        if (fromPos.Line == toPos.Line)
                        {
                            return fromPos.Line.SubString(fromPos.LinePos, toPos.LinePos - fromPos.LinePos);
                        }
                        var b = new StringBuilder();
                        b.Append(fromPos.Line.SubString(fromPos.LinePos, fromPos.Line.Length));
                        TLine curr = fromPos.Line.Next;
                        while (curr != null && curr != toPos.Line)
                        {
                            lock (curr)
                            {
                                b.Append(curr);
                            }
                            curr = curr.Next;
                        }
                        if (curr != null && curr == toPos.Line)
                        {
                            b.Append(toPos.Line.SubString(0, toPos.LinePos));
                        }
                        ret = b.ToString();
                    }
                }
                return ret;
            }
        }

        /// <summary>
        /// Returns the text inside the document contained between the two given positions encapsulated in
        /// a list of lines. The returned lines are deep copies of the original lines and tokens constructed 
        /// via calls to the SubTokens() methods of the respective lines.
        /// </summary>
        /// <param name="fromPos">Start position.</param>
        /// <param name="toPos">End position.</param>
        /// <returns>Text between fromPos and toPos encapsulated in a list of lines. Null if any of the passed
        /// parameters is null.</returns>
        public TLine SubLines(Position<TLine, TToken> fromPos, Position<TLine, TToken> toPos)
        {
            lock (lockObj)
            {
                if (fromPos == null || toPos == null || fromPos.Line == null || toPos.Line == null) return null;
                var ret = new TLine();
                lock (fromPos.Line)
                {
                    lock (toPos.Line)
                    {
                        // switch positions if fromPos comes after toPos:
                        if (!(fromPos < toPos)) switchPositions(ref fromPos, ref toPos);
                        // if positions are on the same line:
                        if (fromPos.Line == toPos.Line)
                        {
                            int dist = toPos.LinePos - fromPos.LinePos;
                            ret.First = SubTokens(fromPos.Line, fromPos.LinePos, dist);
                        }
                        else
                        {
                            ret.First = fromPos.Line.SubTokens(fromPos.LinePos, fromPos.Line.Length);
                            TLine curr = ret;
                            TLine search = fromPos.Line.Next;
                            while (search != null && search != toPos.Line)
                            {
                                lock (search)
                                {
                                    curr.Next = new TLine();
                                    curr.Next.Prev = curr;
                                    curr = curr.Next;
                                    curr.First = search.SubTokens(0, search.Length);
                                }
                                search = search.Next;
                            }
                            if (search != null)
                            {
                                curr.Next = new TLine();
                                curr.Next.Prev = curr;
                                curr = curr.Next;
                                curr.First = toPos.Line.SubTokens(0, toPos.LinePos);
                            }
                        }
                    }
                }
                return ret;
            }
        }

        /// <summary>
        /// Returns the substring of length len starting at pos inside the given line as a list of tokens. 
        /// The returned token list is a deep copy of the original tokens contained in the line.
        /// If the requested text does not exist (pos less than 0 or line is empty) the method returns null.
        /// </summary>
        /// <param name="line">Line containing the requested text.</param>
        /// <param name="pos">Start position inside the line.</param>
        /// <param name="len">Length of the text to return.</param>
        /// <returns>A token list containing the requested substring. Null if pos less than 0 or line is empty.</returns>
        public TToken SubTokens(TLine line, int pos, int len)
        {
            lock (lockObj)
            {
                if (line != null)
                {
                    TToken retT;
                    lock (line)
                    {
                        retT = line.SubTokens(pos, len);
                    }
                    return retT;
                }
                return null;
            }
        }

        /// <summary>
        /// Calculates the distance between the two given lines inside the document. On success this method
        /// always returns a positive integer. If either of the two lines is not contained the method returns -1.
        /// This method calculates the distance by starting at Line l1 and searching forward to Line l2. If l2
        /// is not found after l1 a second loop will search backwards for l2 - therefore for maximum performance
        /// the caller should make sure that l1 is contained before l2!
        /// </summary>
        /// <param name="l1">First line.</param>
        /// <param name="l2">Second line.</param>
        /// <returns>Distance between l1 and l2 or -1 if either of the two lines is not contained!</returns>
        public static int DistanceBetween(TLine l1, TLine l2)
        {
            int dist = 0;
            if (l1 == null || l2 == null) return -1;
            TLine search = l1;
            while (search != l2 && search != null)
            {
                search = search.Next;
                dist++;
            }
            if (search == null)
            {
                dist = 0;
                search = l1;
                while (search != l2 && search != null)
                {
                    search = search.Prev;
                    dist++;
                }
                if (search == null) return -1;
            }
            return dist;
        }

        /// <summary>
        /// Returns the index of the given line inside the current document. If the line is not contained this
        /// method returns -1.
        /// </summary>
        /// <param name="line">Line for which to get the index.</param>
        /// <returns>Index of the given line or -1 if it's not contained.</returns>
        public int IndexOf(TLine line)
        {
            lock (lockObj)
            {
                TLine search = first;
                int ind = 1;
                while (search != null && search != line)
                {
                    search = search.Next;
                    ind++;
                }
                if (search != null) return ind;
                return -1;
            }
        }

        /// <summary>
        /// This helper method can be used to determine the index of a line relative to the index of a known start
        /// line. This method exists for performance reasons only when the index of multiple lines is required and
        /// their relative position is known.
        /// </summary>
        /// <param name="start">Start line for which the index is known.</param>
        /// <param name="startInd">Index of the given start line.</param>
        /// <param name="line">Line contained before or after the given start line.</param>
        /// <param name="forward">Direction parameter - True means that the given line is contained after the
        /// start line - False searches in the opposite direction.</param>
        /// <returns>Index of the given line relative to the start line or -1 if the line is not found.</returns>
        public int IndexRelativeTo(TLine start, int startInd, TLine line, bool forward)
        {
            lock (lockObj)
            {
                int ret = startInd;
                if (forward)
                {
                    while (start != null && start != line)
                    {
                        ret++;
                        start = start.Next;
                    }
                }
                else
                {
                    while (start != null && start != line)
                    {
                        ret--;
                        start = start.Prev;
                    }
                }
                if (start != null) return ret;
                return -1;
            }
        }

        /// <summary>
        /// This method can be used to retrieve a line by index relative to a given line whose index is known.
        /// NOTE: Line indices start at index 1 not 0!
        /// </summary>
        /// <param name="start">Start line at the given start index.</param>
        /// <param name="startInd">Index of the given start line.</param>
        /// <param name="newInd">Index of the line to retrieve.</param>
        /// <returns>The line at index newInd or null if such a line doesn't exist.</returns>
        public TLine LineRelativeTo(TLine start, int startInd, int newInd)
        {
            lock (lockObj)
            {
                if (start != null && startInd > 0 && newInd > 0)
                {
                    int dist = startInd - newInd;
                    TLine search = start;
                    if (dist > 0)
                    {
                        while (search != null && dist != 0)
                        {
                            search = search.Prev;
                            dist--;
                        }
                    }
                    else if (dist < 0)
                    {
                        while (search != null && dist != 0)
                        {
                            search = search.Next;
                            dist++;
                        }
                    }
                    return search;
                }
                return null;
            }
        }

        /// <summary>
        /// Returns the color of the character before the given position or the color of the character at 
        /// the given position if pos is the first position of the document. If the document contains no 
        /// characters at all the method returns the currently set default color.
        /// </summary>
        /// <param name="pos">Position for which to get the color.</param>
        /// <returns>Color of the char at the previous position.</returns>
        public virtual Color ColorAt(Position<TLine, TToken> pos)
        {
            lock (lockObj)
            {
                Color retCol = color;
                if (pos != null && pos.Line != null)
                {
                    int startPos = 0;
                    TToken t;
                    lock (pos.Line)
                    {
                        // special handling of last line:
                        if (!pos.Line.IsValid && pos.LinePos > 0 && pos.LinePos == pos.Line.Length)
                        {
                            t = pos.Line.TokenAt(pos.LinePos - 1, out startPos);
                        }
                        else t = pos.Line.TokenAt(pos.LinePos, out startPos);
                        if (t != null)
                        {
                            if ((startPos != pos.LinePos || (pos.LinePos == 0 && pos.Line == first))
                                && !t.IsNewLineToken) retCol = t.Color;
                            else if (t.Prev != null) retCol = t.Prev.Color;
                            else if (pos.Line.Prev != null) retCol = ColorAt(pos.Line.Prev, pos.Line.Prev.LastPos);
                        }
                            // if no token is found try previous position:
                        else if (pos.LinePos > 0)
                        {
                            retCol = ColorAt(pos.Line, pos.LinePos - 1);
                        }
                        else if (pos.Line.Prev != null)
                        {
                            retCol = ColorAt(pos.Line.Prev, pos.Line.Prev.LastPos);
                        }
                    }
                }
                return retCol;
            }
        }

        /// <summary>
        /// Returns the color of the character before the given position or the color of the character at 
        /// the given position if pos is the first position of the document. If the document contains no 
        /// characters at all the method returns the currently set default color.
        /// </summary>
        /// <param name="line">Line for which to get the color.</param>
        /// <param name="pos">Position inside line for which to get the color.</param>
        /// <returns>Color at given position or Color.Empty if there is no text at the given position.</returns>
        public virtual Color ColorAt(TLine line, int pos)
        {
            lock (lockObj)
            {
                Color retCol = color;
                if (line != null)
                {
                    int startPos = 0;
                    TToken t;
                    lock (line)
                    {
                        // special handling of last line:
                        if (!line.IsValid && pos > 0 && pos == line.Length)
                        {
                            t = line.TokenAt(pos - 1, out startPos);
                        }
                        else t = line.TokenAt(pos, out startPos);
                        if (t != null)
                        {
                            if ((startPos != pos || (pos == 0 && line == first))
                                && !t.IsNewLineToken) retCol = t.Color;
                            else if (t.Prev != null) retCol = t.Prev.Color;
                            else if (line.Prev != null) retCol = ColorAt(line.Prev, line.Prev.LastPos);
                        }
                            // if no token is found try previous position:
                        else if (pos > 0)
                        {
                            retCol = ColorAt(line, pos - 1);
                        }
                        else if (line.Prev != null)
                        {
                            retCol = ColorAt(line.Prev, line.Prev.LastPos);
                        }
                    }
                }
                return retCol;
            }
        }

        /// <summary>
        /// Returns the character at the given position inside the document. Returns Char.MaxValue if there is no
        /// character at the given position (corresponds to EOF) or if the position is not contained!
        /// </summary>
        /// <param name="pos">Position for which to get the character.</param>
        /// <returns>Character at the given position.</returns>
        public char CharAt(Position<TLine, TToken> pos)
        {
            lock (lockObj)
            {
                char retCh = Char.MaxValue;
                if (pos != null && pos.Line != null)
                {
                    lock (pos.Line)
                    {
                        retCh = pos.Line.CharAt(pos.LinePos);
                    }
                }
                return retCh;
            }
        }

        /// <summary>
        /// Returns the character position of the given position inside the document. This method treats newline
        /// strings as a single character independent of the actual length.
        /// </summary>
        /// <param name="pos">Position for which to return the character position.</param>
        /// <returns>The char position inside the document or -1 if the position is not contained.</returns>
        public int GetCharPosition(Position<TLine, TToken> pos)
        {
            lock (lockObj)
            {
                TLine search = First;
                int charPos = -1;
                if (pos != null && pos.Line != null)
                {
                    lock (pos.Line)
                    {
                        while (search != null && search != pos.Line)
                        {
                            charPos += search.LastPos + 1;
                            search = search.Next;
                        }
                        if (search != null) charPos += pos.LinePos;
                        else charPos = -1;
                    }
                }
                return charPos;
            }
        }

        /// <summary>
        /// Returns a position object for the given character position inside the document. If the passed char
        /// position is less than 0 or larger then the documents last position the first or last position is returned
        /// respectively. This method treats newline strings as a single character independent of the actual 
        /// length. 
        /// </summary>
        /// <param name="charPos">Character position inside the document.</param>
        /// <returns>A position object for the given character position.</returns>
        public Position<TLine, TToken> GetPosition(int charPos)
        {
            lock (lockObj)
            {
                return GetRelativePos(first, 0, charPos);
            }
        }

        /// <summary>
        /// Returns the Position inside the document that comes the given amount of characters after the 
        /// start position defined by the passed line and line position. If the passed distance is negative or 0 
        /// the start position will be returned.
        /// If the distance to the start position lies outside the documents' scope the last position inside the 
        /// document will be returned! This method always treats newline tokens as a single character.
        /// </summary>
        /// <param name="line">Line containing the start position.</param>
        /// <param name="linePos">Start position relative to the line start.</param>
        /// <param name="dist">Positive distance to the given position.</param>
        /// <returns>Position object for the position that comes dist characters after the start position or 
        /// null if the passed start line is null or the passed line position is not contained in the start line.</returns>
        public Position<TLine, TToken> GetRelativePos(TLine line, int linePos, int dist)
        {
            lock (lockObj)
            {
                if (line != null && linePos >= 0 && linePos <= line.LastPos)
                {
                    Position<TLine, TToken> retPos = null;
                    lock (line)
                    {
                        retPos = new Position<TLine, TToken>();
                        retPos.IsValid = false;
                        retPos.Line = line;
                        retPos.LinePos = linePos;
                        int lastP = line.LastPos;
                        if (dist > 0)
                        {
                            if (linePos + dist <= lastP)
                            {
                                retPos.LinePos += dist;
                            }
                            else
                            {
                                dist -= (lastP + 1 - linePos);
                                TLine last = retPos.Line;
                                retPos.Line = line.Next;
                                retPos.LinePos = 0;
                                while (retPos.Line != null && dist > 0)
                                {
                                    lastP = retPos.Line.LastPos;
                                    if (dist <= lastP)
                                    {
                                        retPos.LinePos = dist;
                                        break;
                                    }
                                    dist -= (lastP + 1);
                                    last = retPos.Line;
                                    retPos.Line = retPos.Line.Next;
                                }
                                if (retPos.Line == null)
                                {
                                    retPos.Line = last;
                                    retPos.LinePos = last.LastPos;
                                }
                            }
                        }
                    }
                    return retPos;
                }
                return null;
            }
        }

        /// <summary>
        /// Searches for any character in the given array of characters beginning at the given position and returns
        /// the first position containing any of the characters.
        /// </summary>
        /// <param name="chars">Array of characters to search for.</param>
        /// <param name="startPos">Position at which to begin search.</param>
        /// <returns>The first position containing any of the characters in the array.</returns>
        public Position<TLine, TToken> Find(char[] chars, Position<TLine, TToken> startPos)
        {
            lock (lockObj)
            {
                if (startPos == null || startPos.Line == null || chars == null || chars.Length == 0) return null;
                TLine search = startPos.Line;
                int searchPos = startPos.LinePos;
                int lastP;
                while (search != null)
                {
                    lock (search)
                    {
                        lastP = search.LastPos;
                        while (searchPos <= lastP)
                        {
                            char ch = search.CharAt(searchPos);
                            foreach (char c in chars)
                            {
                                if (ch == c) return new Position<TLine, TToken>(search.X, search, searchPos);
                            }
                            searchPos++;
                        }
                    }
                    search = search.Next;
                    searchPos = 0;
                }
                return null;
            }
        }

        /// <summary>
        /// Searches for the given string inside the document starting at the given position and taking into 
        /// account the options set in the ColorTextBoxFinds parameter.
        /// </summary>
        /// <param name="str">String to search for.</param>
        /// <param name="startPos">Position at which to start search.</param>
        /// <param name="finds">ColorTextBoxFinds to apply.</param>
        /// <returns>The start position of the first occurence of the given string or null if it isn't found.</returns>
        public Position<TLine, TToken> Find(String str, Position<TLine, TToken> startPos, ColorTextBoxFinds finds)
        {
            lock (lockObj)
            {
                if (startPos == null || startPos.Line == null || str == null || str.Length == 0) return null;
                bool backwards = ((finds & ColorTextBoxFinds.Reverse) == ColorTextBoxFinds.Reverse);
                bool matchCase = ((finds & ColorTextBoxFinds.MatchCase) == ColorTextBoxFinds.MatchCase);
                StringComparison comp;
                if (matchCase) comp = StringComparison.Ordinal;
                else comp = StringComparison.OrdinalIgnoreCase;
                String subst = String.Empty;
                int searchLen = str.Length;
                int searchPos = startPos.LinePos;
                TLine search = startPos.Line;
                int lineLen;
                if (backwards)
                {
                    searchPos -= searchLen;
                    while (search != null)
                    {
                        lock (search)
                        {
                            lineLen = search.Length;
                            while (searchPos >= 0)
                            {
                                if (searchPos + searchLen <= lineLen)
                                {
                                    subst = search.SubString(searchPos, searchLen);
                                    if (subst.Equals(str, comp))
                                        return new Position<TLine, TToken>(search.X, search, searchPos);
                                }
                                searchPos--;
                            }
                        }
                        search = search.Prev;
                        if (search != null) searchPos = search.LastPos - searchLen;
                    }
                }
                else
                {
                    while (search != null)
                    {
                        lock (search)
                        {
                            lineLen = search.Length;
                            while (searchPos + searchLen <= lineLen)
                            {
                                subst = search.SubString(searchPos, searchLen);
                                if (subst.Equals(str, comp))
                                    return new Position<TLine, TToken>(search.X, search, searchPos);
                                searchPos++;
                            }
                        }
                        search = search.Next;
                        searchPos = 0;
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// "Disposes" a document by calling Dispose() on all contained lines and removing any registered
        /// UpdateEventListeners!
        /// </summary>
        public virtual void Dispose()
        {
            lock (lockObj)
            {
                TLine search = first;
                while (search != null)
                {
                    search.Dispose();
                    search = search.Next;
                }
                first = null;
                last = null;
                color = Color.Black;
                lines = 0;
                UpdateEvent = null;
                GC.Collect();
            }
        }

        /***** PROTECTED HELPER METHODS *****/

        /// <summary>
        /// This helper method is used for sending an UpdateEvent to the text box when the 
        /// content of the document is initialized through one of the Text, Lines or First
        /// properties.
        /// </summary>
        protected void notifyNewDocument()
        {
            lock (lockObj)
            {
                var fromP = new Position<TLine, TToken>(0, first, 0);
                var toP = new Position<TLine, TToken>(0, last, last.LastPos);
                var args = new UpdateEventArgs(UpdateEventType.NewDocument, fromP, toP,
                                               IndexOf(first), IndexOf(last), DistanceBetween(first, last),
                                               null, UpdateAction.NonAtomic, true);
                lines = args.Lines + 1;
                OnUpdateEvent(args);
            }
        }

        /// <summary>
        /// Raises the UpdateEvent that occurs when changes to the documents data are being made.
        /// Note that the data object that is contained in the UpdateEventArgs object will only contain data when the
        /// UpdateAction is not set to ignore!
        /// </summary>
        /// <param name="args">UpdateEventArgs of the event.</param>
        protected virtual void OnUpdateEvent(UpdateEventArgs args)
        {
            lock (lockObj)
            {
                if (UpdateEvent != null)
                {
                    UpdateEvent(this, args);
                }
            }
        }

        /// <summary>
        /// Changes the character casing of the provided string according to the CharacterCasing 
        /// property value and returns the converted string.
        /// </summary>
        /// <param name="text">String to convert.</param>
        /// <returns>The provided text string converted to upper- or lowercase.</returns>
        protected virtual String changeCasing(String text)
        {
            switch (casing)
            {
                case CharacterCasing.Normal:
                    return text;
                case CharacterCasing.Upper:
                    return text.ToUpper();
                case CharacterCasing.Lower:
                    return text.ToLower();
                default:
                    return text;
            }
        }

        /// <summary>
        /// Changes the character casing of the provided line (list) according to the CharacterCasing 
        /// property value.
        /// </summary>
        protected virtual void changeCasing(TLine line)
        {
            TLine search = line;
            TToken t;
            while (search != null)
            {
                t = line.First;
                while (t != null)
                {
                    changeCasing(t);
                    t = t.Next;
                }
                search = search.Next;
            }
        }

        /// <summary>
        /// Changes the character casing of the provided token according to the CharacterCasing 
        /// property value.
        /// </summary>
        protected virtual void changeCasing(TToken token)
        {
            if (!token.IsNewLineToken && token.Length > 0)
            {
                switch (casing)
                {
                    case CharacterCasing.Normal:
                        return;
                    case CharacterCasing.Upper:
                        {
                            token.Value = token.Value.ToUpper();
                            return;
                        }
                    case CharacterCasing.Lower:
                        {
                            token.Value = token.Value.ToLower();
                            return;
                        }
                    default:
                        return;
                }
            }
        }

        /// <summary>
        /// Helper method to produce tokens from a given string. The default implementation splits the passed
        /// String at all occurences of CRLF and produces tokens for all Substrings (including Newline - Tokens)
        /// with the color set to the currently active document color!
        /// Furthermore all occurences of "\r", "\r\n", "\n" and "\f" are converted to tokens with the platform
        /// independent value System.Environment.Newline. 
        /// Example:
        /// text = "a sample \ntext\racross multiple\flines"
        /// returns a list of tokens with values: {"a sample ", System.Environment.Newline, "text",
        /// System.Environment.Newline, "across multiple", System.Environment.Newline, "lines"}
        /// </summary>
        /// <param name="text">Text string to tokenize.</param>
        /// <returns>An ArrayList of TTokens.</returns>
        protected virtual List<TToken> tokenizeString(String text)
        {
            lock (lockObj)
            {
                //unify newline handling - formfeeds are treated as newline chars
                text = text.Replace("\r\n", "\n");
                text = text.Replace('\r', '\n');
                text = text.Replace('\f', '\n');
                TToken t;
                var tokens = new List<TToken>();
                String[] splitText = SplitString(text, new[] {'\n'});
                foreach (String s in splitText)
                {
                    if (s != null && s.Length > 0)
                    {
                        if (s.Equals("\n")) t = TokenBase<TToken>.NewLineToken();
                        else
                        {
                            t = new TToken();
                            t.Value = s;
                            t.Color = Color;
                        }
                        tokens.Add(t);
                    }
                }
                return tokens;
            }
        }

        /// <summary>
        /// Splits the passed String text at occurences of the passed splitChar char[] and adds all Substrings
        /// to an Array (including Strings for characters defined in splitChar!) which is returned. Example:
        /// text = "a two-line string\ndelimited by spaces "
        /// splitChars = {'\n', ' '}
        /// returns the array: {"a", " ", "two-line", "string", "\n", "delimited", " ", "by", "spaces", " "}
        /// </summary>
        /// <param name="text">String to split.</param>
        /// <param name="splitChars">Split characters to use.</param>
        /// <returns>A String array containing the split String including Strings for characters in splitChars!</returns>
        public static String[] SplitString(String text, char[] splitChars)
        {
            if (text == null) return new[] {String.Empty};
            var result = new List<String>();
            String subSt = String.Empty;
            int i = text.IndexOfAny(splitChars);
            int oldi = 0;
            char splitC;
            try
            {
                while (i >= 0)
                {
                    subSt = text.Substring(oldi, i - oldi);
                    if (subSt.Length > 0) result.Add(subSt);
                    splitC = text[i];
                    result.Add(splitC.ToString());
                    oldi = i + 1;
                    i = text.IndexOfAny(splitChars, oldi);
                }
                subSt = text.Substring(oldi);
                if (subSt.Length > 0) result.Add(subSt);
            }
            catch (ArgumentOutOfRangeException)
            {
            }
            return result.ToArray();
        }

        /// <summary>
        /// Helper method that simply switches the references to the given positions - can be used to switch 
        /// positions if they are passed in the wrong order.
        /// </summary>
        /// <param name="first">Reference to first position.</param>
        /// <param name="second">Reference to second position.</param>
        protected void switchPositions(ref Position<TLine, TToken> first, ref Position<TLine, TToken> second)
        {
            lock (lockObj)
            {
                Position<TLine, TToken> tmp = first;
                first = second;
                second = tmp;
            }
        }

        /***** PROPERTIES *****/
    }

    public class Position<TLine, TToken> : IPosition
        where TLine : LineBase<TLine, TToken>, new()
        where TToken : TokenBase<TToken>, new()
    {
        /***** PROTECTED FIELDS: *****/

        /// <summary>
        /// Can be used to store the line number of the line that contains the position. This field is intended to be
        /// used for storing the caret line number only!
        /// </summary>
        public int LineNumber;

        protected TLine line; // line containing this position
        protected int linePos; // char position inside line
        protected bool valid; // indicates if position is visible
        protected int x; // the x position inside a line

        /***** CONSTRUCTORS: *****/

        public Position()
        {
            x = 0;
            line = null;
            linePos = 0;
            valid = false;
        }

        public Position(int x) : this()
        {
            this.x = x;
            valid = false;
        }

        public Position(int x, TLine line, int pos)
            : this(x)
        {
            this.line = line;
            linePos = pos;
            valid = false;
        }

        /// <summary>
        /// Gets or sets the line containing this position.
        /// </summary>
        public TLine Line
        {
            get { return line; }
            set { line = value; }
        }

        /// <summary>
        /// This flag indicates if a position is valid, i.e. it's coordinates are properly calculated to 
        /// reflect the actual char position inside a line
        /// </summary>
        public bool IsValid
        {
            get { return valid; }
            set { valid = value; }
        }

        #region IPosition Members

        /// <summary>
        /// Gets or sets the x coordinate of this position.
        /// </summary>
        public int X
        {
            get { return x; }
            set { x = value; }
        }

        /// <summary>
        /// Gets the ILine containing this position.
        /// </summary>
        public ILine ILine
        {
            get { return line; }
        }

        /// <summary>
        /// Gets or sets the character position inside the containing line.
        /// </summary>
        public int LinePos
        {
            get { return linePos; }
            set { linePos = value; }
        }

        #endregion

        /***** METHODS: *****/

        /// <summary>
        /// Returns true if two positions are equal (i.e. they are in the same line at the same position).
        /// </summary>
        /// <param name="pos">Position to compare to.</param>
        /// <returns>True if this position is equal to pos.</returns>
        public virtual bool Equals(Position<TLine, TToken> pos)
        {
            if (pos != null && Line == pos.Line && LinePos == pos.LinePos) return true;
            return false;
        }

        /// <summary>
        /// Calculates the distance between two positions and returns it as a positive integer (no matter which
        /// position comes first). If one position is null or if the line corresponding to one of the positions
        /// is null the method returns -1. The method treats newline chars as a single character!
        /// </summary>
        /// <param name="pos">The position to calculate the distance to.</param>
        /// <returns>The distance between two positions.</returns>
        public int DistanceTo(Position<TLine, TToken> pos)
        {
            if (pos != null && pos.Line != null && Line != null)
            {
                Position<TLine, TToken> from = this;
                Position<TLine, TToken> to = pos;
                if (!(from < to))
                {
                    from = pos;
                    to = this;
                }
                TLine search = from.Line;
                int dist = 0;
                while (search != null && search != to.Line.Next)
                {
                    if (search == from.Line)
                    {
                        if (search == to.Line)
                        {
                            dist = to.LinePos - from.LinePos;
                            return dist;
                        }
                        dist += search.LastPos + 1 - from.LinePos;
                    }
                    else if (search == to.Line)
                    {
                        dist += to.LinePos;
                    }
                    else
                    {
                        dist += search.LastPos + 1;
                    }
                    search = search.Next;
                }
                return dist;
            }
            return -1;
        }

        /***** OVERLOADED OPERATORS: *****/

        /// <summary>
        /// Returns true if position p1 lies before position p2.
        /// </summary>
        /// <param name="p1">Position to compare.</param>
        /// <param name="p2">Position to compare to.</param>
        /// <returns>True if position p1 is contained before p2. False otherwise.</returns>
        public static bool operator <(Position<TLine, TToken> p1, Position<TLine, TToken> p2)
        {
            if (p1 == null || p2 == null || p1.Line == null || p2.Line == null) return false;
            if (p1.Line == p2.Line)
            {
                if (p1.LinePos < p2.LinePos) return true;
                else return false;
            }
            if (p1.Line < p2.Line) return true;
            return false;
        }

        /// <summary>
        /// Returns true if position p1 lies after position p2.
        /// </summary>
        /// <param name="p1">Position to compare.</param>
        /// <param name="p2">Position to compare to.</param>
        /// <returns>True if position p1 is contained after p2. False otherwise.</returns>
        public static bool operator >(Position<TLine, TToken> p1, Position<TLine, TToken> p2)
        {
            if (p1 == null || p2 == null || p1.Line == null || p2.Line == null) return false;
            if (p1.Line == p2.Line)
            {
                if (p1.LinePos > p2.LinePos) return true;
                else
                {
                    return false;
                }
            }
            if (p1.Line > p2.Line) return true;
            return false;
        }

        /***** PROPERTIES: *****/
    }

    public class Selection<TLine, TToken> : ISelection
        where TLine : LineBase<TLine, TToken>, new()
        where TToken : TokenBase<TToken>, new()
    {
        /***** PROTECTED FIELDS: *****/

        protected Position<TLine, TToken> fromPos;
        protected Position<TLine, TToken> toPos;

        /***** CONSTRUCTORS: *****/

        public Selection()
        {
            fromPos = null;
            toPos = null;
        }

        public Selection(Position<TLine, TToken> from, Position<TLine, TToken> to)
            : this()
        {
            fromPos = from;
            toPos = to;
        }

        /***** METHODS: *****/

        /***** PROPERTIES: *****/

        /// <summary>
        /// Gets or sets the start position of this selection.
        /// </summary>
        public Position<TLine, TToken> From
        {
            get { return fromPos; }
            set { fromPos = value; }
        }

        /// <summary>
        /// Gets or sets the end position of this selection.
        /// </summary>
        public Position<TLine, TToken> To
        {
            get { return toPos; }
            set { toPos = value; }
        }

        /// <summary>
        /// This property returns true if the current selection has theoretically valid values for the From and
        /// To positions that represent the selection. It does not check if the From.Line comes before To.Line
        /// nor does it check if the from and to lines are connected at all!
        /// </summary>
        public bool HasValidPositions
        {
            get
            {
                return From != null && To != null && From.Line != null && To.Line != null &&
                       From.LinePos >= 0 && From.LinePos <= From.Line.Length &&
                       To.LinePos >= 0 && To.LinePos <= To.Line.Length;
            }
        }

        /// <summary>
        /// Checks if the given position is contained in the current selection.
        /// </summary>
        /// <param name="pos">Position to check.</param>
        /// <returns>True if pos lies inside the selection, false otherwise.</returns>
        public bool Contains(Position<TLine, TToken> pos)
        {
            if (pos > fromPos && pos < toPos) return true;
            return false;
        }

        /// <summary>
        /// Checks if the given line or parts of it are contained in the current selection.
        /// </summary>
        /// <param name="line">Line to check.</param>
        /// <returns>True if parts of the line are contained in the selection, false otherwise.</returns>
        public bool Contains(TLine line)
        {
            if (fromPos != null && toPos != null)
            {
                TLine search = fromPos.Line;
                while (search != null && search != toPos.Line.Next)
                {
                    if (search == line) return true;
                    search = search.Next;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns the content of a selection as a String.
        /// </summary>
        /// <returns>The text contained in the selection or String.Empty for invalid selections.</returns>
        public override String ToString()
        {
            if (From != null && To != null && From.Line != null && To.Line != null)
            {
                var sb = new StringBuilder();
                if (From.Line == To.Line) return From.Line.SubString(From.LinePos, To.LinePos - From.LinePos);
                sb.Append(From.Line.SubString(From.LinePos, From.Line.Length - From.LinePos));
                TLine search = From.Line.Next;
                while (search != null && search != To.Line)
                {
                    sb.Append(search);
                    search = search.Next;
                }
                if (search == To.Line) sb.Append(search.SubString(0, To.LinePos));
                return sb.ToString();
            }
            return String.Empty;
        }

        /// <summary>
        /// Overridden equality check - this method checks if the passed object is a Selection of the same
        /// type as the current and if so calls the overloaded implementation. Otherwise the base implementation
        /// of Equals is being called.
        /// </summary>
        /// <param name="obj">The object to check for equality.</param>
        /// <returns>True if the current object is equal to the passed object.</returns>
        public override bool Equals(object obj)
        {
            var sel = obj as Selection<TLine, TToken>;
            if (sel != null) return Equals(sel);
            else return base.Equals(obj);
        }

        /// <summary>
        /// Serves as a hash function for the Selection class.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Overridden equality check. Checks if the passed Selection refers to the same lines and line 
        /// positions as the current. This method will only return true for Selections whose HasValidPositions
        /// property returns true as well!
        /// </summary>
        /// <param name="sel">The selection to compare to.</param>
        /// <returns>True if the passed object is a selection with the same start and end positions as the
        /// current. False otherwise or if a null reference is passed.</returns>
        public virtual bool Equals(Selection<TLine, TToken> sel)
        {
            if (sel != null && sel.HasValidPositions)
            {
                if (sel.From.Equals(From) && sel.To.Equals(To)) return true;
            }
            return false;
        }
    }

    /// <summary>
    /// Simplest possible implementation of a Token to be used in a ColorTextBox.
    /// </summary>
    [Serializable]
    public class Token : TokenBase<Token>
    {
    }

    /// <summary>
    /// Simplest possible implementation of a Line to be used in a ColorTextBox. Uses the Token implementation
    /// to make up the lines.
    /// </summary>
    [Serializable]
    public class Line : LineBase<Line, Token>
    {
    }

    /// <summary>
    /// Simplest possible implementation of a Document to be used in a ColorTextBox. Uses the Line and Token
    /// implementations to make up the document.
    /// </summary>
    public class Document : DocumentBase<Line, Token>
    {
    }
}