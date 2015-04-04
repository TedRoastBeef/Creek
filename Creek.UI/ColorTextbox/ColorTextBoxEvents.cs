using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Creek.UI.ColorTextbox
{
    /// <summary>
    /// This enumeration defines mouse events that can be handled by reserved areas as well. A registered mouse event
    /// handler for a reserved area receives one of the defined MouseEventTypes and can so determine if it wants to
    /// handle it or not.
    /// </summary>
    public enum MouseEventType
    {
        MouseMove,
        MouseDown,
        MouseUp,
        MouseClick,
        MouseDoubleClick,
        MouseWheel
    }

    public abstract partial class ColorTextBoxBase<TDocument, TLine, TToken> : UserControl
        where TToken : TokenBase<TToken>, new()
        where TLine : LineBase<TLine, TToken>, new()
        where TDocument : DocumentBase<TLine, TToken>, new()
    {
        protected bool textAreaMouseEvent = false;

        #region IColorTextBox Members

        /// <summary>
        /// Event that can be used to send message strings to an attached GUI
        /// </summary>
        public event MessageEventHandler GUIMessageEvent;

        #endregion

        /// <summary>
        /// Occurs when the user or code scrolls through the client area.
        /// </summary>
        public new event ScrollEventHandler Scroll;

        /// <summary>
        /// Occurs when the position of the caret changes.
        /// </summary>
        public event CaretEventHandler CaretChanged;

        // indicates if a mouse event series (Mouse down - move - up) was triggered inside the TextArea

        /// <summary>
        /// Returns the start position of the previous word
        /// </summary>
        /// <returns></returns>
        protected Position<TLine, TToken> PosPrevWord(Position<TLine, TToken> curr)
        {
            char currChar = curr.Line.CharAt(curr.LinePos);
            TLine line = curr.Line;
            int pos = curr.LinePos;
            do
            {
                pos--;
                if (pos < 0)
                {
                    line = line.Prev;
                    if (line != null) pos = line.LastPos;
                }
                if (line != null) currChar = line.CharAt(pos);
            } while (!DELIM_CHARS.Contains(currChar.ToString()) && line != null);
            if (line != null) return Pos(pos, line);
            else return Pos(0, FirstLine);
        }

        /// <summary>
        /// Returns the start position of the next word
        /// </summary>
        /// <returns></returns>
        protected Position<TLine, TToken> PosNextWord(Position<TLine, TToken> curr)
        {
            char currChar = curr.Line.CharAt(curr.LinePos);
            TLine line = curr.Line;
            int pos = curr.LinePos;
            do
            {
                pos++;
                if (pos > line.LastPos)
                {
                    line = line.Next;
                    pos = 0;
                }
                if (line != null) currChar = line.CharAt(pos);
            } while (!DELIM_CHARS.Contains(currChar.ToString()) && line != null);
            if (line != null) return Pos(pos, line);
            else return Pos(LastLine.LastPos, LastLine);
        }

        /// <summary>
        /// Inverts the casing of the provided string, i.e. lowercase letters are converted to uppercase and
        /// vice versa.
        /// </summary>
        /// <param name="casing">Character casing to change to</param>
        /// <param name="s">String to invert.</param>
        /// <returns>Inverted string.</returns>
        protected static String changeCasing(CharacterCasing casing, String s)
        {
            switch (casing)
            {
                case CharacterCasing.Normal:
                    {
                        var sb = new StringBuilder(s.Length);
                        foreach (char c in s)
                        {
                            if (Char.IsUpper(c)) sb.Append(Char.ToLower(c));
                            else if (Char.IsLower(c)) sb.Append(Char.ToUpper(c));
                            else sb.Append(c);
                        }
                        return sb.ToString();
                    }
                case CharacterCasing.Lower:
                    return s.ToLower();
                case CharacterCasing.Upper:
                    return s.ToUpper();
                default:
                    return s;
            }
        }

        /// <summary>
        /// Edit method which changes the casing of the character at the current caret position
        /// or the casing of the current selection according to the provided CharacterCasing parameter.
        /// </summary>
        /// <param name="casing">Parameter which determines how to change the casing.</param>
        protected void editChangeCasing(CharacterCasing casing)
        {
            if (selection != null)
            {
                String s = document.SubString(selection.From, selection.To);
                s = changeCasing(casing, s);
                document.Replace(selection.From, selection.To, s, UpdateAction.Undoable, true);
            }
            else if (caret != null)
            {
                String s;
                if (caret.LinePos != caret.Line.LastPos)
                {
                    s = caret.Line.SubString(caret.LinePos, 1);
                    s = changeCasing(casing, s);
                    document.Replace(caret, Pos(caret.LinePos + 1, caret.Line), s, UpdateAction.Undoable, true);
                }
                else if (caret.Line.Next != null)
                {
                    s = caret.Line.Next.SubString(0, 1);
                    s = changeCasing(casing, s);
                    document.Replace(Pos(0, caret.Line.Next), Pos(1, caret.Line.Next), s, UpdateAction.Undoable, true);
                }
            }
        }

        /***** Keyboard handling: *****/

        /// <summary>
        /// Returns a boolean value for a specified key which indicates if the KeyEvent should be
        /// processed directly by the ColorTextBox (true) or first by the containing control (false).
        /// </summary>
        /// <param name="keyData">Key to process.</param>
        /// <returns>The value of the enabled property.</returns>
        protected override bool IsInputKey(Keys keyData)
        {
            if (keyData == Keys.Return && !AcceptsReturn) return false;
            if (keyData == Keys.Tab && !AcceptsTab) return false;
            return Focused;
        }

        /// <summary>
        /// Determines if a character is an input character that the ColorTextBox recognizes.
        /// </summary>
        /// <param name="charCode">Character to check.</param>
        /// <returns>True for characters that are accepted.</returns>
        protected override bool IsInputChar(char charCode)
        {
            if (charCode.Equals('\r') || charCode.Equals('\n'))
            {
                if (acceptsReturn) return true;
                else return false;
            }
            else if (charCode.Equals('\t'))
            {
                if (acceptsTab) return true;
                else return false;
            }
            else return base.IsInputChar(charCode);
        }

        /// <summary>
        /// Overrides UserControl.OnKeyDown. Called when a key is down.
        /// </summary>
        /// <param name="e">KeyEventArgs object of this key event.</param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            caretTimer.Stop();
            // handle key events with shift key pressed:
            if (e.Shift)
            {
                bool shift = true;
                if (IsKeyLocked(Keys.CapsLock)) shift = false;
                if (shift)
                {
                    if (shortCutsEnabled)
                    {
                        if (e.Control) handleCtrlShiftKey(e);
                        else handleShiftKey(e);
                    }
                }
                else handleKey(e);
            }
                // handle key events when control key is pressed:
            else if (e.Control && !e.Alt)
            {
                if (shortCutsEnabled) handleCtrlKey(e);
            }
            else if (!e.Control && e.Alt)
            {
                if (shortCutsEnabled) handleAltKey(e);
            }
                // handle function key events:
            else if (e.KeyCode >= Keys.F1 && e.KeyCode <= Keys.F9)
            {
                /* TODO: handle function key events */
            }
                // handle all other keys:
            else handleKey(e);
        }

        /// <summary>
        /// Handles control key events when the Control key is pressed
        /// </summary>
        /// <param name="e">KeyEventArgs for the key event.</param>
        protected virtual void handleCtrlKey(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                    // Copy current selection
                case Keys.C:
                case Keys.Insert:
                    Copy();
                    break;
                    // Insert text from clipboard
                case Keys.V:
                    Paste();
                    break;
                    // Cut current selection
                case Keys.X:
                    Cut();
                    break;
                    // Undo the last operation
                case Keys.Z:
                    Undo();
                    break;
                    // Redo the last operation from the undo buffer
                case Keys.Y:
                    Redo();
                    break;
                    // Select all text in the ColorTextBox
                case Keys.A:
                    SelectAll();
                    break;
                    // Invert the casing at the caret position or of the selection
                case Keys.K:
                    editChangeCasing(CharacterCasing.Normal);
                    break;
                    // Change casing of current position or selection to lower
                case Keys.L:
                    editChangeCasing(CharacterCasing.Lower);
                    break;
                    // Change casing of current position or selection to upper
                case Keys.U:
                    editChangeCasing(CharacterCasing.Upper);
                    break;
                    // Increase the indentation of the current line
                case Keys.I:
                    {
                        if (caret != null)
                        {
                            if (selection == null)
                            {
                                setSelection(Pos(0, caret.Line), Pos(caret.Line.LastPos, caret.Line));
                            }
                            changeSelectionIndent(1);
                        }
                    }
                    break;
                    // Join current line with next or all selected lines:
                case Keys.J:
                    {
                        if (selection != null)
                        {
                            Position<TLine, TToken> cPos = selection.From;
                            TLine search = selection.From.Line;
                            TLine to = selection.To.Line;
                            removeSelection();
                            if (search == to) document.Delete(search, search.LastPos, 1, UpdateAction.Undoable, true);
                            else
                            {
                                while (search.Next != null && search.Next != to)
                                {
                                    document.Delete(search, search.LastPos, 1, UpdateAction.Undoable, false);
                                }
                                document.Delete(search, search.LastPos, 1, UpdateAction.Undoable, true);
                            }
                            setCaret(cPos);
                        }
                        else if (caret != null)
                        {
                            document.Delete(caret.Line, caret.Line.LastPos, 1, UpdateAction.Undoable, true);
                        }
                    }
                    break;
                    // Scroll up one line
                case Keys.Up:
                    scrollUp(1);
                    break;
                    // Scroll down one line
                case Keys.Down:
                    scrollDown(1);
                    break;
                    // Set caret to the next word
                case Keys.Right:
                    {
                        if (selection != null) removeSelection();
                        if (caret != null) setCaret(PosNextWord(caret));
                    }
                    ;
                    break;
                    // Set caret to the previous word
                case Keys.Left:
                    {
                        if (selection != null) removeSelection();
                        if (caret != null) setCaret(PosPrevWord(caret));
                    }
                    ;
                    break;
                    // Set caret to the end of the document
                case Keys.End:
                    {
                        if (selection != null) removeSelection();
                        setCaret(Pos(LastLine.LastPos, LastLine));
                    }
                    break;
                    // Set caret to the beginning of the document
                case Keys.Home:
                    {
                        if (selection != null) removeSelection();
                        setCaret(Pos(0, FirstLine));
                    }
                    break;
                    // Delete the previous word
                case Keys.Back:
                    {
                        if (caret != null)
                        {
                            Position<TLine, TToken> delPos = PosPrevWord(caret);
                            Delete(delPos, caret);
                        }
                    }
                    break;
                    // Delete the next word
                case Keys.Delete:
                    {
                        if (caret != null)
                        {
                            Position<TLine, TToken> delPos = PosNextWord(caret);
                            Delete(caret, delPos);
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Handles control key events with pressed shift button.
        /// </summary>
        /// <param name="e">KeyEventArgs for the key event.</param>
        protected virtual void handleShiftKey(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                    // Decrease the indent level of selected lines or delete tab characters before caret
                case Keys.Tab:
                    {
                        // if there is a selection unindent the selected lines
                        if (selection != null)
                        {
                            changeSelectionIndent(-1);
                        }
                            // else check if previous character is tab character and remove if applicable
                        else if (caret != null && caret.LinePos > 0 &&
                                 caret.Line.CharAt(caret.LinePos - 1) == '\t')
                        {
                            setCaret(Pos(caret.LinePos - 1, caret.Line));
                            deleteAfterCaret(1);
                        }
                    }
                    break;
                    // Update selection to the left
                case Keys.Left:
                    {
                        moveCaretLeft(true);
                    }
                    break;
                    // Update selection to the right
                case Keys.Right:
                    {
                        moveCaretRight(true);
                    }
                    break;
                    // Update selection to prev line
                case Keys.Up:
                    {
                        moveCaretUp(true);
                    }
                    break;
                    // Update selection to next line
                case Keys.Down:
                    {
                        moveCaretDown(true);
                    }
                    break;
                    // Update selection to the previous page
                case Keys.PageUp:
                    {
                        int lines = TextArea.Height/LineHeight;
                        Position<TLine, TToken> newPos = caret;
                        if (caret != null)
                        {
                            TLine line = caret.Line;
                            int count = 0;
                            while (count < lines && line != null)
                            {
                                line = line.Prev;
                                count++;
                            }
                            if (line != null) newPos = Pos(Math.Min(caret.LinePos, line.LastPos), line);
                            else newPos = Pos(0, FirstLine);
                            if (selection != null) updateSelection(newPos);
                            else setSelection(caret, newPos);
                        }
                    }
                    break;
                    // Update selection to the next page
                case Keys.PageDown:
                    {
                        int lines = TextArea.Height/LineHeight;
                        Position<TLine, TToken> newPos = caret;
                        if (caret != null)
                        {
                            TLine line = caret.Line;
                            int count = 0;
                            while (count < lines && line != null)
                            {
                                line = line.Next;
                                count++;
                            }
                            if (line != null) newPos = Pos(Math.Min(caret.LinePos, line.LastPos), line);
                            else newPos = Pos(LastLine.LastPos, LastLine);
                            if (selection != null) updateSelection(newPos);
                            else setSelection(caret, newPos);
                        }
                    }
                    break;
                    // Update selection to the document end
                case Keys.End:
                    {
                        if (caret != null)
                        {
                            if (selection != null) updateSelection(Pos(caret.Line.LastPos, caret.Line));
                            else setSelection(caret, Pos(caret.Line.LastPos, caret.Line));
                        }
                    }
                    break;
                    // Update selection to the document start
                case Keys.Home:
                    {
                        if (caret != null)
                        {
                            if (selection != null) updateSelection(Pos(0, caret.Line));
                            else setSelection(caret, Pos(0, caret.Line));
                        }
                    }
                    break;
                    // Cut the current selection
                case Keys.Delete:
                    Cut();
                    break;
                    // Paste text from the clipboard
                case Keys.Insert:
                    Paste();
                    break;
            }
        }

        /// <summary>
        /// Handles control key events with pressed Ctrl and Shift key.
        /// </summary>
        /// <param name="e">KeyEventArgs for the key event.</param>
        protected virtual void handleCtrlShiftKey(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                    // Decreases the indentation of the current line
                case Keys.I:
                    {
                        if (caret != null)
                        {
                            if (selection == null)
                            {
                                setSelection(Pos(0, caret.Line), Pos(caret.Line.LastPos, caret.Line));
                            }
                            changeSelectionIndent(-1);
                        }
                    }
                    break;
                    // Update selection to the next word
                case Keys.Right:
                    {
                        Position<TLine, TToken> newPos = caret;
                        if (caret != null)
                        {
                            newPos = PosNextWord(caret);
                            if (selection != null) updateSelection(newPos);
                            else setSelection(caret, newPos);
                        }
                    }
                    ;
                    break;
                    // Update selection to the previous word
                case Keys.Left:
                    {
                        Position<TLine, TToken> newPos = caret;
                        if (caret != null)
                        {
                            newPos = PosPrevWord(caret);
                            if (selection != null) updateSelection(newPos);
                            else setSelection(caret, newPos);
                        }
                    }
                    ;
                    break;
                    // Update selection to the document end
                case Keys.End:
                    {
                        if (caret != null)
                        {
                            if (selection != null) updateSelection(Pos(LastLine.LastPos, LastLine));
                            else setSelection(caret, Pos(LastLine.LastPos, LastLine));
                        }
                    }
                    break;
                    // Update selection to the document start
                case Keys.Home:
                    {
                        if (caret != null)
                        {
                            if (selection != null) updateSelection(Pos(0, FirstLine));
                            else setSelection(caret, Pos(0, FirstLine));
                        }
                    }
                    break;
                    // Delete up to the end of the line
                case Keys.Delete:
                    {
                        if (caret != null)
                        {
                            deleteAfterCaret(caret.Line.LastPos - caret.LinePos);
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Handles control key events with pressed alt button.
        /// </summary>
        /// <param name="e">KeyEventArgs for the key event.</param>
        protected virtual void handleAltKey(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                    // Delete the whole content of the document
                case Keys.Delete:
                    Clear();
                    break;
                    // Move caret to the last visible line
                case Keys.End:
                    {
                        if (lastVisLine != null)
                        {
                            if (selection != null) removeSelection();
                            if (caret != null)
                            {
                                int pos;
                                if (caret.LinePos > lastVisLine.LastPos) pos = lastVisLine.LastPos;
                                else pos = caret.LinePos;
                                setCaret(Pos(pos, lastVisLine));
                            }
                            else setCaret(Pos(0, lastVisLine));
                        }
                    }
                    break;
                    // Move caret to the first visible line
                case Keys.Home:
                    {
                        if (firstVisLine != null)
                        {
                            if (caret != null)
                            {
                                if (selection != null) removeSelection();
                                int pos;
                                if (caret.LinePos > firstVisLine.LastPos) pos = firstVisLine.LastPos;
                                else pos = caret.LinePos;
                                setCaret(Pos(pos, firstVisLine));
                            }
                            else setCaret(Pos(0, firstVisLine));
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Handles all normal control key events
        /// </summary>
        /// <param name="e">KeyEventArgs for the key event.</param>
        protected virtual void handleKey(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                    // Move caret to the left
                case Keys.Left:
                    {
                        if (selection != null)
                        {
                            if (!selection.From.Line.IsVisible) scrollToLine(selection.From.Line);
                            setCaret(selection.From);
                            removeSelection();
                        }
                        else moveCaretLeft(false);
                    }
                    break;
                    // Move caret to the right
                case Keys.Right:
                    {
                        if (selection != null)
                        {
                            if (!selection.To.Line.IsVisible) scrollToLine(selection.To.Line);
                            //ensureCaretIsDrawn();
                            setCaret(selection.To);
                            removeSelection();
                        }
                        else moveCaretRight(false);
                    }
                    break;
                    // Move caret up
                case Keys.Up:
                    {
                        if (selection != null)
                        {
                            if (!selection.From.Line.IsVisible) scrollToLine(selection.From.Line);
                            setCaret(selection.From);
                            removeSelection();
                        }
                        else moveCaretUp(false);
                    }
                    break;
                    // Move caret down
                case Keys.Down:
                    {
                        if (selection != null)
                        {
                            if (!selection.To.Line.IsVisible) scrollToLine(selection.To.Line);
                            //ensureCaretIsDrawn();
                            setCaret(selection.To);
                            removeSelection();
                        }
                        else moveCaretDown(false);
                    }
                    break;
                    // Increase indentation level of selection or insert tab
                case Keys.Tab:
                    {
                        if (selection != null)
                        {
                            changeSelectionIndent(1);
                        }
                        else
                        {
                            insertChar('\t');
                        }
                    }
                    break;
                    // Change the insert mode
                case Keys.Insert:
                    {
                        if (selection == null)
                        {
                            caretTimer.Stop();
                            if (caretColor != BackColor)
                            {
                                invertCaret();
                            }
                            ;
                            insertMode = !insertMode;
                            caretTimer.Start();
                        }
                    }
                    break;
                    // Delete current selection or the char at the caret position
                case Keys.Delete:
                    {
                        if (selection != null) deleteSelection();
                        else if (caret != null)
                        {
                            deleteAfterCaret(1);
                        }
                    }
                    break;
                    // Delete current selection or char before current caret position
                case Keys.Back:
                    {
                        if (selection != null) deleteSelection();
                        else if (caret != null)
                        {
                            if (!(caret.Line == FirstLine && caret.LinePos == 0))
                            {
                                Position<TLine, TToken> newP = Pos(caret.LinePos - 1, caret.Line);
                                if (caret.Line == firstVisLine && caret.LinePos == 0)
                                {
                                    caret = newP;
                                }
                                else setCaret(newP);
                                deleteAfterCaret(1);
                            }
                        }
                    }
                    break;
                    // Scroll up one page
                case Keys.PageUp:
                    {
                        Position<TLine, TToken> oldCaret = caret;
                        int lines = TextArea.Height/LineHeight;
                        scrollUp(lines);
                        if (selection == null && oldCaret != null)
                        {
                            TLine line = oldCaret.Line;
                            int i = 0;
                            while (line.Prev != null && i < lines)
                            {
                                line = line.Prev;
                                i++;
                            }
                            if (oldCaret.LinePos > line.LastPos) i = line.LastPos;
                            else i = oldCaret.LinePos;
                            setCaret(Pos(i, line));
                        }
                    }
                    break;
                    // Scroll down one page
                case Keys.PageDown:
                    {
                        Position<TLine, TToken> oldCaret = caret;
                        int lines = TextArea.Height/LineHeight;
                        scrollDown(lines);
                        if (selection == null && oldCaret != null)
                        {
                            TLine line = oldCaret.Line;
                            int i = 0;
                            while (line.Next != null && i < lines)
                            {
                                line = line.Next;
                                i++;
                            }
                            if (oldCaret.LinePos > line.LastPos) i = line.LastPos;
                            else i = oldCaret.LinePos;
                            setCaret(Pos(i, line));
                        }
                    }
                    break;
                    // Set caret to the end of the line
                case Keys.End:
                    {
                        if (selection != null) removeSelection();
                        if (caret != null) setCaret(Pos(caret.Line.LastPos, caret.Line));
                    }
                    break;
                    // Set caret to the beginning of the line
                case Keys.Home:
                    {
                        if (selection != null) removeSelection();
                        if (caret != null) setCaret(Pos(0, caret.Line));
                    }
                    break;
                    // Insert a newline at the current caret position
                case Keys.Enter:
                    insertChar('\n');
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Checks if a mouse event should be processed by the event handler method of a reserved area.
        /// Returns true if a reserved area already handled the event or false if the ColorTextBox should
        /// handle the mouse event itself.
        /// </summary>
        /// <param name="args">MouseEventArgs of the mouse event.</param>
        /// <param name="type">Determines the type of the MouseEvent that occured.</param>
        /// <returns></returns>
        protected bool resAreaMouseEvent(MouseEventArgs args, MouseEventType type)
        {
            //if (ClientRectangle.Contains(e.Location) && !TextArea.Contains(e.Location)) {
            //}
            if (!textAreaMouseEvent)
            {
                // check if a reserved area is responsible for 
                if (args.X <= ClientRectangle.Left + ReservedLeftWidths &&
                    args.X >= ClientRectangle.Left &&
                    args.Y > ClientRectangle.Top + ReservedTopWidths &&
                    args.Y < ClientRectangle.Bottom - ReservedBottomWidths)
                {
                    List<ReservedArea> areas = reservedAreas[(int) (ReservedLocation.Left)];
                    int len = 0;
                    foreach (ReservedArea a in areas)
                    {
                        if (args.X > TextArea.Left - len - a.Width)
                        {
                            if (a.HoverCursor != null)
                            {
                                Cursor = a.HoverCursor;
                            }
                            return a.handleMouse(args, type);
                        }
                        len += a.Width;
                    }
                    return false;
                }
                else if (args.X >= ClientRectangle.Right - ReservedRightWidths &&
                         args.X <= ClientRectangle.Right &&
                         args.Y > ClientRectangle.Top + ReservedTopWidths &&
                         args.Y < ClientRectangle.Bottom - ReservedBottomWidths)
                {
                    return true;
                }
                else if (args.Y <= ClientRectangle.Top + ReservedTopWidths &&
                         args.Y >= ClientRectangle.Top)
                {
                    return true;
                }
                else if (args.Y >= ClientRectangle.Bottom - ReservedBottomWidths &&
                         args.Y <= ClientRectangle.Bottom)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Event handle method that handles mouse events occuring inside the reserved area used for 
        /// displaying the line numbers.
        /// </summary>
        /// <param name="args">MouseEventArgs of the mouse event.</param>
        /// <param name="type">Determines the type of the MouseEvent that occured.</param>
        protected bool handleLNumMouseEvt(MouseEventArgs args, MouseEventType type)
        {
            if (args.Button == MouseButtons.Left &&
                (type == MouseEventType.MouseMove || type == MouseEventType.MouseDown))
            {
                int yPos = args.Y;
                TLine l = GetLineAtYPos(ref yPos);
                if (l != null)
                {
                    int length = l.LastPos;
                    var sel = new Selection<TLine, TToken>();
                    if (ModifierKeys == Keys.Shift || ModifierKeys == Keys.Control)
                    {
                        if (selection != null && selectionAnchor.Line != l)
                        {
                            if (selectionAnchor.Line < l)
                            {
                                sel.From = Pos(length, l);
                                sel.To = selectionAnchor;
                            }
                            else
                            {
                                sel.From = selectionAnchor;
                                sel.To = Pos(0, l);
                            }
                        }
                        else if (caret != null && caret.Line != l)
                        {
                            if (caret.Line < l)
                            {
                                sel.From = caret;
                                sel.To = Pos(length, l);
                            }
                            else
                            {
                                sel.From = Pos(length, l);
                                sel.To = caret;
                            }
                        }
                    }
                    else
                    {
                        sel.From = Pos(0, l);
                        sel.To = Pos(length, l);
                    }
                    if (sel != null && sel.From != null && sel.To != null)
                    {
                        if (selection == null ||
                            !(sel.From.Equals(selection.From) && sel.To.Equals(selection.To) ||
                              sel.From.Equals(selection.To) && sel.To.Equals(selection.From)))
                        {
                            //if (caret != null && selection != null && caret.Equals(selection.From)) ensureCaretIsDrawn();
                            setSelection(sel.From, sel.To);
                        }
                    }
                    return true;
                }
            }
            else if (args.Button == MouseButtons.Right) return false;
            else if (args.Button == MouseButtons.Middle) return false;
            else if (type == MouseEventType.MouseWheel && args.Delta != 0)
            {
                if (args.Delta >= 120)
                {
                    scrollUp(SystemInformation.MouseWheelScrollLines);
                }
                if (args.Delta <= -120)
                {
                    scrollDown(SystemInformation.MouseWheelScrollLines);
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Overrides UserControl.OnKeyPress. Called when key is pressed.
        /// </summary>
        /// <param name="e">KeyPressEventArgs of the key event.</param>
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            //base.OnKeyPress(e);
            if (Char.IsControl(e.KeyChar))
            {
                bool shift = false;
                if (IsKeyLocked(Keys.CapsLock)) shift = true;
                if ((ModifierKeys & Keys.Shift) == Keys.Shift) shift = !shift;
                if (shift)
                {
                    /* TODO: handle key events when shift is pressed */
                }
            }
            else if (Char.IsLetterOrDigit(e.KeyChar))
            {
                insertChar(e.KeyChar);
            }
            else if (Char.IsWhiteSpace(e.KeyChar))
            {
                switch (e.KeyChar)
                {
                    case ' ':
                        insertChar(' ');
                        break;
                    default:
                        break;
                }
            }
            else if (Char.IsPunctuation(e.KeyChar)) insertChar(e.KeyChar);
            else if (Char.IsSeparator(e.KeyChar)) insertChar(e.KeyChar);
            else if (Char.IsSymbol(e.KeyChar)) insertChar(e.KeyChar);
            else insertChar(e.KeyChar);
            //else if (selection != null) deleteSelection();
        }

        /// <summary>
        /// Overrides UserControl.OnKeyUp. Called when key goes up.
        /// </summary>
        /// <param name="e">KeyEventArgs of the key event.</param>
        protected override void OnKeyUp(KeyEventArgs e)
        {
            //base.OnKeyUp(e);
            if (!caretTimer.Enabled) caretTimer.Start();
        }

        /***** Mouse handling: *****/

        /// <summary>
        /// Called when mouse is moved over control.
        /// </summary>
        /// <param name="e">MouseEventArgs of the event.</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (TextArea.Contains(e.Location))
            {
                Cursor = Cursors.IBeam;
            }
            else Cursor = DefaultCursor;
            if (!resAreaMouseEvent(e, MouseEventType.MouseMove))
            {
                if (e.Button == MouseButtons.Left && firstVisLine != null)
                {
                    // Scroll to mouse position if necessary:
                    if (!insertMode) insertMode = !insertMode;
                    int eX = e.X;
                    int eY = e.Y;
                    /* TODO: Try to handle scrolling through calls to setCaret() !!!*/
                    if (e.X > TextArea.Right && HScroll.Value < HScroll.Maximum)
                    {
                        // right of text area
                        if (-xOffSet + H_SCROLL_DELTA <= maxWidth)
                        {
                            xOffSet = xOffSet - H_SCROLL_DELTA;
                            scrollRight(H_SCROLL_DELTA);
                        }
                        else
                        {
                            int len = maxWidth + xOffSet;
                            xOffSet = -maxWidth;
                            scrollRight(len);
                        }
                        eX = TextArea.Right;
                        /*if (selectionAnchor == selection.From) updateSelection(Pos(selection.To.LinePos + 1, selection.To.Line));
                        else updateSelection(Pos(selection.From.LinePos + 1, selection.From.Line));*/
                    }
                    else if (e.X < TextArea.X && HScroll.Value > HScroll.Minimum)
                    {
                        // left of text area
                        if (-xOffSet - H_SCROLL_DELTA >= 0)
                        {
                            xOffSet = xOffSet + H_SCROLL_DELTA;
                            scrollLeft(H_SCROLL_DELTA);
                        }
                        else
                        {
                            int len = -xOffSet;
                            xOffSet = 0;
                            scrollLeft(len);
                        }
                        eX = TextArea.Left;
                        /*if (selectionAnchor == selection.From) updateSelection(Pos(selection.To.LinePos - 1, selection.To.Line));
                        else updateSelection(Pos(selection.From.LinePos - 1, selection.From.Line));*/
                    }
                    if (e.Y > TextArea.Bottom)
                    {
                        // below text area
                        scrollDown(1);
                        eY = lastVisLine.Y + BaseLineDistance;
                    }
                    else if (e.Y < TextArea.Top)
                    {
                        // above text area
                        scrollUp(1);
                        eY = firstVisLine.Y + BaseLineDistance;
                    }
                    // Calculate new mouse position // TODO: NULL POINTER EXCEPTION WHEN PLAYING AROUND STUPID...
                    Position<TLine, TToken> newMousePos = Pos(eX - xOffSet, eY);
                    if (newMousePos != null && newMousePos.Line.X >= TextArea.Left && selection != null &&
                        // TODO: NULL POINTER EXCEPTION WHEN OPENING LARGE FILE AND MOUSE MOVEMENT
                        newMousePos != selection.From && newMousePos != selection.To)
                    {
                        updateSelection(newMousePos);
                    }
                }
                else if (e.Button == MouseButtons.Right)
                {
                    //showMessage("\nRight Mouse dragged!");
                }
                else if (e.Button == MouseButtons.Middle)
                {
                    //showMessage("\nMiddle Mouse dragged!");
                }
            }
        }

        /// <summary>
        /// Called when the mouse wheel is moved.
        /// </summary>
        /// <param name="e">MouseEventArgs of the event.</param>
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            if (!resAreaMouseEvent(e, MouseEventType.MouseWheel))
            {
                if (e.Delta >= 120)
                {
                    scrollUp(SystemInformation.MouseWheelScrollLines);
                }
                if (e.Delta <= -120)
                {
                    scrollDown(SystemInformation.MouseWheelScrollLines);
                }
            }
        }

        /// <summary>
        /// Called when a mouse button goes down.
        /// </summary>
        /// <param name="e">MouseEventArgs of the mouse event.</param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (!resAreaMouseEvent(e, MouseEventType.MouseDown))
            {
                textAreaMouseEvent = true;
                if (e.Button == MouseButtons.Left)
                {
                    int eY = e.Y;
                    if (lastVisLine != null && e.Y > lastVisLine.Y + LineHeight)
                    {
                        scrollDown(1);
                        eY = lastVisLine.Y + BaseLineDistance;
                    }
                    if (ModifierKeys == Keys.Shift)
                    {
                        if (selection != null && selectionAnchor != null)
                        {
                            updateSelection(Pos(e.X - xOffSet, eY));
                        }
                        else
                        {
                            setSelection(caret, Pos(e.X - xOffSet, eY));
                        }
                    }
                    else
                    {
                        removeSelection();
                        removeCaret();
                        selectionAnchor = Pos(e.X - xOffSet, eY);
                        selection = new Selection<TLine, TToken>(selectionAnchor, selectionAnchor);
                    }
                }
                else if (e.Button == MouseButtons.Right)
                {
                    //showMessage("\nRight Mouse down!");
                    if (selection == null)
                    {
                        Position<TLine, TToken> pos = Pos(e.X + (-xOffSet), e.Y);
                        setCaret(pos);
                    }
                }
                else if (e.Button == MouseButtons.Middle)
                {
                    //showMessage("\nMiddle Mouse down!");
                }
            }
        }

        /// <summary>
        /// Called when mouse button goes up.
        /// </summary>
        /// <param name="e">MouseEventArgs of the mouse event.</param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (!resAreaMouseEvent(e, MouseEventType.MouseUp))
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (selection != null && selection.From != null && selection.To != null &&
                        !selection.From.Equals(selection.To))
                    {
                        // we have a valid selection
                        /*showMessage("\nWe have a valid selection from pos x: "+selection.From.BaseLineX+" y: "+
                            selection.From.BaseLineY + " tpos: " + selection.From.LinePos + " to line pos x: " +
                            selection.To.BaseLineX + " y: " + selection.To.BaseLineY + " tpos: " + selection.To.LinePos);
                         * */
                    }
                    else
                    {
                        if (selection != null && selectionAnchor != null)
                        {
                            setCaret(selectionAnchor);
                            removeSelection();
                        }
                        else
                        {
                            removeSelection();
                            if (firstVisLine != null && lastVisLine != null)
                            {
                                if (e.Y > TextArea.Bottom) setCaret(Pos(lastVisLine.LastPos, lastVisLine));
                                else if (e.Y < TextArea.Top) setCaret(Pos(0, firstVisLine));
                                else setCaret(Pos(e.X - xOffSet, e.Y));
                            }
                        }
                    }
                }
                else if (e.Button == MouseButtons.Right)
                {
                    //showMessage("\nRight Mouse up!");
                }
                else if (e.Button == MouseButtons.Middle)
                {
                    //showMessage("\nMiddle Mouse up!");
                }
                textAreaMouseEvent = false;
            }
        }

        /// <summary>
        /// Called when mouse is clicked inside control.
        /// </summary>
        /// <param name="e">MouseEventArgs of the event.</param>
        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (!resAreaMouseEvent(e, MouseEventType.MouseClick))
            {
                /* ADD CLICK EVENT HANDLING CODE HERE */
            }
            //showMessage("\nMouse button " + e.Button + " clicked "+e.Clicks+" times!");
        }

        /// <summary>
        /// Called when a double click occurs inside the control. Selects the word on which the user clicked - 
        /// a word is delimited by the chars contained in the DELIM_CHARS string. If the user double clicks on
        /// one of these characters only the respective char will be selected.
        /// </summary>
        /// <param name="e">MouseEventArgs of the event.</param>
        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            if (!resAreaMouseEvent(e, MouseEventType.MouseDoubleClick))
            {
                if (selectionAnchor != null)
                {
                    setCaret(selectionAnchor);
                    char currChar = caret.Line.CharAt(caret.LinePos);
                    if (currChar == '\r' || currChar == '\n' || currChar == Char.MaxValue) return;
                    if (DELIM_CHARS.Contains(currChar.ToString()))
                    {
                        selectionAnchor = caret;
                        selection = new Selection<TLine, TToken>(selectionAnchor, selectionAnchor);
                        updateSelection(Pos(caret.LinePos + 1, caret.Line));
                    }
                    else
                    {
                        int startPos = caret.LinePos - 1;
                        while (startPos >= 0 && !DELIM_CHARS.Contains(caret.Line.CharAt(startPos).ToString()))
                            startPos--;
                        int endPos = caret.LinePos + 1;
                        int lastP = caret.Line.LastPos;
                        while (endPos <= lastP && !DELIM_CHARS.Contains(caret.Line.CharAt(endPos).ToString())) endPos++;
                        selectionAnchor = Pos(startPos + 1, caret.Line);
                        selection = new Selection<TLine, TToken>(selectionAnchor, selectionAnchor);
                        updateSelection(Pos(endPos, caret.Line));
                    }
                }
            }
        }

        /// <summary>
        /// Called when the mouse is dragged over the control (moved over the control from the outside with 
        /// a button pressed)
        /// </summary>
        /// <param name="drgevent">DragEventArgs of the drag event.</param>
        protected override void OnDragOver(DragEventArgs drgevent)
        {
            base.OnDragOver(drgevent);
        }

        /// <summary>
        /// Called when the mouse is dragged over the control for the first time.
        /// </summary>
        /// <param name="drgevent">DragEventArgs of the drag event.</param>
        protected override void OnDragEnter(DragEventArgs drgevent)
        {
            base.OnDragEnter(drgevent);
            drgevent.Effect = DragDropEffects.Copy;
        }

        /// <summary>
        /// Called when a mouse is dragged to the outside of the control.
        /// </summary>
        /// <param name="e">EventArgs of the drag event.</param>
        protected override void OnDragLeave(EventArgs e)
        {
            base.OnDragLeave(e);
        }

        /// <summary>
        /// Called when a drag-drop operation is performed inside the control.
        /// </summary>
        /// <param name="drgevent">DragEventArgs of the drag event.</param>
        protected override void OnDragDrop(DragEventArgs drgevent)
        {
            base.OnDragDrop(drgevent);
            if (AllowDrop)
            {
                string[] formats = drgevent.Data.GetFormats();
                bool supported = false;
                string format = String.Empty;
                foreach (string s in formats)
                {
                    if (CanPaste(s))
                    {
                        format = s;
                        supported = true;
                        break;
                    }
                }
                if (supported)
                {
                    Point clientP = PointToClient(new Point(MousePosition.X, MousePosition.Y));
                    Position<TLine, TToken> pos = Pos(clientP.X, clientP.Y);
                    if (format.Equals(ColorTextBoxFormat))
                    {
                        var il = (TLine) drgevent.Data.GetData(format);
                        if (pos != null && il != null)
                            document.Insert(il, pos.Line, pos.LinePos, UpdateAction.Undoable, true);
                    }
                    else
                    {
                        var insert = (String) drgevent.Data.GetData(format);
                        if (pos != null && insert != null)
                            document.Insert(insert, pos.Line, pos.LinePos, UpdateAction.Undoable, true);
                    }
                }
            }
        }

        /***** Other event handlers: *****/

        /// <summary>
        /// Overridden OnPaint method for the ColorTextBox. The ColorTextBox control uses the following mechanism 
        /// for painting:
        /// All changes to the internal data are first rendered to an offscreen graphics buffer. These changes are
        /// then immediately drawn to the screen by calling this method with a null reference as parameter (which
        /// indicates that the repaint event originated from internal code) So when the argument is null the 
        /// invalidated region defined on the screenGraphics object will be painted.
        /// Whenever the OS needs to repaint the control it will call the method with a non-null parameter which
        /// will cause the invalidated rectangle defined in the PaintEventArgs to be redrawn (from the background
        /// buffer).
        /// Painting of internal changes can be suspended by calling SuspendPainting().
        /// </summary>
        /// <param name="e">PaintEventArgs of the paint event - passing a null reference will not call the
        /// base implementation but cause the invalidated region defined on the screenGraphics object to be painted.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (buffGraphics != null && screenGraphics != null)
            {
                // rendering not suspended - paint buffer to screen:
                if (!suspendPaint)
                {
                    if (firstVisLine != null && lastVisLine != null)
                    {
                        var rect = new Rectangle((int) screenGraphics.ClipBounds.X,
                                                 (int) screenGraphics.ClipBounds.Y,
                                                 (int) screenGraphics.ClipBounds.Width,
                                                 (int) screenGraphics.ClipBounds.Height);
                        bool userpaint = rect.Width > 0 && rect.Height > 0;
                        bool winpaint = e.ClipRectangle.Width > 0 && e.ClipRectangle.Height > 0;
                        IntPtr srcHdc;
                        IntPtr destHdc = screenGraphics.GetHdc();
                        if (renderMode == RenderMode.GDI || renderMode == RenderMode.GDIPlus)
                            srcHdc = buffGraphics.GetHdc();
                        else srcHdc = buffHdc;
                        if (userpaint)
                        {
                            PlatformInvokeGDI32.BitBlt(destHdc, rect.X, rect.Y, rect.Width, rect.Height,
                                                       srcHdc, rect.X, rect.Y, SRC_COPY);
                        }
                        if (winpaint)
                        {
                            PlatformInvokeGDI32.BitBlt(destHdc, e.ClipRectangle.X, e.ClipRectangle.Y,
                                                       e.ClipRectangle.Width, e.ClipRectangle.Height,
                                                       srcHdc, e.ClipRectangle.X, e.ClipRectangle.Y, SRC_COPY);
                        }
                        screenGraphics.ReleaseHdc();
                        if (srcHdc != buffHdc) buffGraphics.ReleaseHdc();
                    }
                    else
                    {
                        getGraphics(new Region(TextArea)).Clear(BackColor);
                        paintTextArea();
                    }
                    buffGraphics.Clip.Dispose();
                    buffGraphics.Clip = new Region(Rectangle.Empty);
                    buffGraphics.Clip.MakeEmpty();
                    screenGraphics.Clip.Dispose();
                    screenGraphics.Clip = new Region(Rectangle.Empty);
                    screenGraphics.Clip.MakeEmpty();
                }
                    // still draw old buffer content to screen when WM_PAINT msgs arrive but do not empty clipping regions
                else paintTextArea();
            }
        }

        protected void paintTextArea()
        {
            if (renderMode == RenderMode.GDI || renderMode == RenderMode.GDIPlus) grafx.Render(screenGraphics);
            else
            {
                IntPtr destHdc = screenGraphics.GetHdc();
                PlatformInvokeGDI32.BitBlt(destHdc, TextArea.X, TextArea.Y, TextArea.Width, TextArea.Height,
                                           buffHdc, TextArea.X, TextArea.Y, SRC_COPY);
                screenGraphics.ReleaseHdc();
            }
        }

        /// <summary>
        /// Calls Invalidate on the control with the passed clipping region set and causes immediate repaint by
        /// calling Update().
        /// </summary>
        /// <param name="updateReg">Region to be invalidated.</param>
        protected virtual void OnInvalidateRegion(Region updateReg)
        {
            if (!suspendPaint)
            {
                Invalidate(updateReg);
                Update();
            }
        }

        /// <summary>
        /// Raises the GUIMessageEvent which allows registered delegates to process internal messages generated by
        /// the ColorTextBox. The default implementation of ColorTextBox never raises GUIMessageEvents!
        /// </summary>
        /// <param name="args">The message string to send.</param>
        protected virtual void OnMessage(MessageEventArgs args)
        {
            if (GUIMessageEvent != null) GUIMessageEvent(this, args);
        }

        /// <summary>
        /// This method is called when a scroll event occurs. The base implementation uses this method to redraw the
        /// line number area if necessary. Thus if overriding this method the base implementation has to be called in
        /// order to update the line numbers!
        /// </summary>
        /// <param name="se"></param>
        protected override void OnScroll(ScrollEventArgs se)
        {
            if (Scroll != null) Scroll(this, se);
            if (se.ScrollOrientation == ScrollOrientation.VerticalScroll)
            {
                int lines = se.NewValue - se.OldValue;
                if (lines != 0) checkHorizScrollBar();
            }
            if (ShowLineNumbers && se.ScrollOrientation == ScrollOrientation.VerticalScroll)
            {
                drawReservedArea(ReservedLocation.Left, LN_RESAREA_ID, true);
            }
        }

        /// <summary>
        /// Checks if the horizontal ScrollBar has to be visible or not and updates the maximum
        /// value of the ScrollBar if necessary.
        /// </summary>
        protected virtual void checkHorizScrollBar()
        {
            if (HScroll.Visible)
            {
                TLine search = firstVisLine;
                int width = 0;
                maxWidth = 0;
                while (search != null && search != lastVisLine.Next)
                {
                    width = search.Width;
                    if (width > maxWidth) maxWidth = width;
                    search = search.Next;
                }
                HScroll.Maximum = maxWidth;
            }
        }

        /// <summary>
        /// Handles resize events. Initializes a new graphics buffer with the new size and invalidates the 
        /// locally stored textArea field.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (ClientScrollRectangle.Width >= 0 && ClientScrollRectangle.Height >= 0)
            {
                textArea = Rectangle.Empty;
                Rectangle newArea = TextArea;
                disposeGraphics();
                initGraphics();
                setLinesInvisible(firstVisLine, lastVisLine);
                calcLinePositions(true, true);
                int newVLC = TextArea.Height/LineHeight;
                ;
                int newHLC = TextArea.Width;
                if (newVLC > 0) VScroll.LargeChange = newVLC;
                if (newHLC > 0) HScroll.LargeChange = newHLC;
            }
        }

        /// <summary>
        /// Overridden OnLostFocus implementation - raises the LostFocus event and disables
        /// caret drawing.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            ShowCaret = false;
        }

        /// <summary>
        /// Overridden OnGotFocus implementation - raises the GotFocus event and enables 
        /// caret drawing.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            ShowCaret = true;
        }

        /// <summary>
        /// Handles font change events. Recalculates the line height and base line distance for the new Font
        /// and redraws the whole text area.
        /// </summary>
        /// <param name="e">EventArgs of the font change event.</param>
        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            if (renderMode == RenderMode.GDINative)
            {
                //LOGFONT lf = new LOGFONT();
                //Font.ToLogFont(lf, buffGraphics);
                IntPtr gdiFont = Font.ToHfont();
                //IntPtr gdiFont = PlatformInvokeGDI32.CreateFontIndirect(ref lf);
                PlatformInvokeGDI32.SelectObject(buffHdc, gdiFont);
            }
            lineHeight = -1;
            lineBaseDist = calcBaseDistance(Font);
            if (Font.SizeInPoints < LineNumFont.SizeInPoints)
            {
                lnFont = new Font(lnFont.FontFamily, Font.SizeInPoints, lnFont.Style);
            }
            if (ShowLineNumbers)
            {
                int maxW = measureString(getGraphics(null), LineCount.ToString(), lnFont).Width;
                changeResLength(maxW + RES_BORDER_WIDTH, ReservedLocation.Left, LN_RESAREA_ID);
            }
            TLine search = document.First;
            while (search != null)
            {
                search.Modified = true;
                search = search.Next;
            }
            setLinesInvisible(firstVisLine, lastVisLine);
            calcLinePositions(true, true);
        }

        /// <summary>
        /// Raises the CaretEvent event. Overriding subclasses should call this base implementation to allow
        /// handling of the CaretEvent through registered delegates!
        /// </summary>
        /// <param name="e">The CaretEventArgs of the CaretEvent.</param>
        protected virtual void OnCaretChange(CaretEventArgs e)
        {
            if (CaretChanged != null)
            {
                CaretChanged(this, e);
            }
            if (ShowCaretInfo) drawReservedArea(ReservedLocation.Bottom, CI_RESAREA_ID, true);
        }
    }
}