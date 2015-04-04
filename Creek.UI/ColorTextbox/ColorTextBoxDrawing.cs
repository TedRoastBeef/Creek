using System;
using System.Drawing;
using System.Drawing.Text;
using System.Text;
using System.Windows.Forms;

namespace Creek.UI.ColorTextbox
{
    public abstract partial class ColorTextBoxBase<TDocument, TLine, TToken> : UserControl
        where TToken : TokenBase<TToken>, new()
        where TLine : LineBase<TLine, TToken>, new()
        where TDocument : DocumentBase<TLine, TToken>, new()
    {
        /***** Drawing: *****/

        /// <summary>
        /// This method acts as a wrapper for drawing all registered reserved areas. It calculates the clipping
        /// regions for all reserved areas and calls the registered delegate methods for each. If one of these
        /// registered drawing methods returns false, the method will return without further drawing.
        /// </summary>
        /// <param name="paint">Determines if the drawn regions should be immediately rendered to the screen.</param>
        protected void drawReservedAreas(bool paint)
        {
            if (!suspendPaint)
            {
                Graphics g;
                // Draw top reserved areas:
                int len = 0;
                var clip = new Rectangle(ClientScrollRectangle.X, ClientScrollRectangle.Y,
                                         ClientScrollRectangle.Width, len);
                for (int i = reservedAreas[(int) ReservedLocation.Top].Count - 1; i >= 0; i--)
                {
                    clip.Y += len;
                    len = reservedAreas[(int) ReservedLocation.Top][i].Width;
                    clip.Height = len;
                    g = getGraphics(new Region(clip));
                    if (!reservedAreas[(int) ReservedLocation.Top][i].draw(g)) return;
                }
                // Draw left reserved areas:
                len = 0;
                clip = new Rectangle(ClientScrollRectangle.X, ClientScrollRectangle.Y + ReservedTopWidths,
                                     len, ClientScrollRectangle.Height - ReservedTopWidths - ReservedBottomWidths);
                for (int i = reservedAreas[(int) ReservedLocation.Left].Count - 1; i >= 0; i--)
                {
                    clip.X += len;
                    len = reservedAreas[(int) ReservedLocation.Left][i].Width;
                    clip.Width = len;
                    g = getGraphics(new Region(clip));
                    if (!reservedAreas[(int) ReservedLocation.Left][i].draw(g)) return;
                }
                // Draw right reserved areas:
                len = 0;
                clip = new Rectangle(ClientScrollRectangle.Right, ClientScrollRectangle.Y + ReservedTopWidths,
                                     len, ClientScrollRectangle.Height - ReservedTopWidths - ReservedBottomWidths);
                for (int i = reservedAreas[(int) ReservedLocation.Right].Count - 1; i >= 0; i--)
                {
                    len = reservedAreas[(int) ReservedLocation.Right][i].Width;
                    clip.X -= len;
                    clip.Width = len;
                    g = getGraphics(new Region(clip));
                    if (!reservedAreas[(int) ReservedLocation.Right][i].draw(g)) return;
                }
                // Draw bottom reserved areas:
                len = 0;
                clip = new Rectangle(ClientScrollRectangle.X, ClientScrollRectangle.Bottom,
                                     ClientScrollRectangle.Width, len);
                for (int i = reservedAreas[(int) ReservedLocation.Bottom].Count - 1; i >= 0; i--)
                {
                    len = reservedAreas[(int) ReservedLocation.Bottom][i].Width;
                    clip.Y -= len;
                    clip.Height = len;
                    g = getGraphics(new Region(clip));
                    if (!reservedAreas[(int) ReservedLocation.Bottom][i].draw(g)) return;
                }
                drawBackground(getGraphics(getPaddingRegion()));
                if (paint)
                {
                    OnInvalidateRegion(screenGraphics.Clip);
                }
            }
        }

        /// <summary>
        /// This method can be used to call the draw delegate of the given reserved area directly.
        /// If the area has been successfully redrawn this method returns true, otherwise false.
        /// </summary>
        /// <param name="location">Location of the area to redraw.</param>
        /// <param name="id">Id string of the area at the given location.</param>
        /// <param name="paint">Determines if the drawn region should be immediately rendered to the screen.</param>
        /// <returns>True on success.</returns>
        protected bool drawReservedArea(ReservedLocation location, String id, bool paint)
        {
            if (!suspendPaint)
            {
                ReservedArea ra = getResArea(location, id);
                if (ra != null)
                {
                    Region raReg = getResRegion(location, id);
                    if (raReg != null)
                    {
                        Graphics g = getGraphics(raReg);
                        bool res = ra.draw(g);
                        if (res && paint)
                        {
                            OnInvalidateRegion(raReg);
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Content drawing method. Draws the lines contained in the clipping region defined on the passed graphics
        /// object onto the offscreen graphics buffer. Line drawing starts at the passed line and continues until
        /// no more lines are visible or until the end of the clipping region is reached, whichever comes first.
        /// Furthermore the DrawMode parameter determines which parts of the controls area are to be redrawn.
        /// There are 4 modes:
        /// DrawMode.Caret: Redraws only the line part occupied by the caret (clipping region has to be set accordingly)
        /// DrawMode.Lines: Redraws only the lines inside the intersection of the TextArea and the clipping region
        /// DrawMode.Reserved: Redraws only the defined reserved areas by calling the registered draw delegate methods
        /// DrawMode.All: Performs all of the above steps
        /// The paint parameter determines if the changes to the background buffer should immediately rendered
        /// to the screen by invalidating the changed regions and calling Update().
        /// </summary>
        /// <param name="start">Line from which to start redrawing</param>
        /// <param name="g">Graphics object on which to draw.</param>
        /// <param name="mode">Draw mode which determines what is to be redrawn.</param>
        /// <param name="paint">Determines if the changes should be rendered from the buffer to the screen.</param>
        protected virtual void drawContent(TLine start, Graphics g, DrawMode mode, bool paint)
        {
            if (!suspendPaint)
            {
                if (lastVisLine != null && g != null && !lastVisLine.Disposed)
                {
                    Region clipReg = g.Clip;
                    RectangleF boundRect = g.ClipBounds;
                    TLine curr = start;
                    if (curr != null && (mode == DrawMode.Lines || mode == DrawMode.All || mode == DrawMode.Caret))
                    {
                        g.Clear(BackColor);
                        var p = new Point(0, 0);
                        while (curr != null && curr.IsVisible && curr != lastVisLine.Next &&
                               curr.Y + curr.Height <= boundRect.Bottom)
                        {
                            lock (curr)
                            {
                                if (curr.Modified) calcLine(curr, g);
                                if (curr.IsSelected && selection != null) drawSelectedLine(curr, boundRect, p, g);
                                else drawLine(curr, boundRect, p, g);
                            }
                            curr = curr.Next;
                        }
                    }
                    if (mode != DrawMode.Caret)
                    {
                        var clearRect = new Rectangle(new Point(lastVisLine.X, lastVisLine.Y + lastVisLine.Height),
                                                      new Size(TextArea.Width,
                                                               TextArea.Bottom - (lastVisLine.Y + lastVisLine.Height)));
                        g = getGraphics(new Region(clearRect));
                        g.Clear(BackColor);
                    }
                }
                else if (g != null) g.Clear(BackColor);
                if (mode == DrawMode.All || mode == DrawMode.Reserved)
                {
                    drawReservedAreas(paint);
                }
                if (paint)
                {
                    OnInvalidateRegion(screenGraphics.Clip);
                }
            }
        }

        /// <summary>
        /// Draws the given line to the background buffer. Draws only the tokens of a line which are inside
        /// the given bounding rectangle.
        /// </summary>
        /// <param name="l">Line to draw.</param>
        /// <param name="r">Bounding rectangle of the current clipping region.</param>
        /// <param name="p">Starting point of the current line.</param>
        /// <param name="g">Graphics object to draw on.</param>
        protected virtual void drawLine(TLine l, RectangleF r, Point p, Graphics g)
        {
            p.Y = l.Y;
            p.X = l.X + xOffSet;
            TToken t = l.First;
            while (t != null && p.X + t.Width < r.Left)
            {
                p.X += t.Width;
                t = t.Next;
            }
            while (t != null && p.X <= r.Right)
            {
                // TextRenderer refuses to draw strings that are too long --> split very large tokens
                if (t.Length > MAX_TOKEN_LEN)
                {
                    drawLongString(g, t.Value, t.Color, Color.Transparent, ref p, r);
                }
                else
                {
                    drawString(g, t.Value, Font, p, t.Color, Color.Transparent, t.Width);
                    p.X += t.Width;
                }
                t = t.Next;
            }
        }

        /// <summary>
        /// This helper method is responsible for drawing tokens whose length exceeds the value defined
        /// by the MAX_TOKEN_LEN constant. This is necessary because TextRenderer.DrawText does not render
        /// strings which exceed a certain length!
        /// </summary>
        /// <param name="g">Graphics context to draw on.</param>
        /// <param name="s">Long string to render (longer than MAX_TOKEN_LEN).</param>
        /// <param name="tC">Color of the text to be rendered.</param>
        /// <param name="bC">Background color of the text to be rendered.</param>
        /// <param name="p">Start point of the provided token.</param>
        /// <param name="r">Bounding rectangle to draw in.</param>
        protected void drawLongString(Graphics g, String s, Color tC, Color bC, ref Point p, RectangleF r)
        {
            String rendVal = String.Empty;
            int len = s.Length;
            int minLen = 0;
            int ind = 0;
            int strWidth = 0;
            while (ind < len && p.X <= TextArea.Right)
            {
                minLen = Math.Min(MAX_TOKEN_LEN, len - ind);
                rendVal = s.Substring(ind, minLen);
                strWidth = measureString(g, rendVal, Font).Width;
                if (p.X + strWidth >= TextArea.Left && p.X <= TextArea.Right)
                {
                    drawString(g, rendVal, Font, p, tC, bC, strWidth);
                }
                ind += minLen;
                p.X += strWidth;
            }
        }

        /// <summary>
        /// Draws the given selected line to the background buffer. Draws only tokens which are inside the
        /// given bounding rectangle.
        /// </summary>
        /// <param name="l">Line to draw.</param>
        /// <param name="r">Bounding rectangle of the current clipping region.</param>
        /// <param name="p">Starting point of the current line.</param>
        /// <param name="g">Graphics object to draw on.</param>
        protected virtual void drawSelectedLine(TLine l, RectangleF r, Point p, Graphics g)
        {
            p.Y = l.Y;
            p.X = l.X + xOffSet;
            int linePos = 0;
            TToken t = l.First;
            while (t != null && p.X + t.Width < r.Left)
            {
                p.X += t.Width;
                linePos += t.Length;
                t = t.Next;
            }
            // draw lines in which a selection starts and / or ends:
            if (l == selection.From.Line || l == selection.To.Line)
            {
                String s1 = String.Empty;
                int startX;
                int len = 0;
                int tLen = 0;
                while (t != null && p.X <= r.Right)
                {
                    tLen = t.Length;
                    startX = p.X;
                    // draw tokens in which a (visible!) selection starts:
                    if (l == selection.From.Line && linePos <= selection.From.LinePos &&
                        linePos + tLen > selection.From.LinePos)
                    {
                        drawTokenSubstring(t, 0, selection.From.LinePos - linePos, ref p, false, g, r);
                        // draw tokens in which a selection starts and ends:
                        if (l == selection.To.Line && linePos + tLen >= selection.To.LinePos)
                        {
                            len = selection.From.LinePos - linePos;
                            drawTokenSubstring(t, len, selection.To.LinePos - linePos - len, ref p, true, g, r);
                            drawTokenSubstring(t, selection.To.LinePos - linePos,
                                               tLen - (selection.To.LinePos - linePos),
                                               ref p, false, g, r);
                        }
                        else
                        {
                            drawTokenSubstring(t, selection.From.LinePos - linePos,
                                               tLen - (selection.From.LinePos - linePos),
                                               ref p, true, g, r);
                        }
                    }
                        // draw tokens inside which a (visible!) selection ends:
                    else if (l == selection.To.Line && linePos < selection.To.LinePos &&
                             linePos + tLen >= selection.To.LinePos)
                    {
                        drawTokenSubstring(t, 0, selection.To.LinePos - linePos, ref p, true, g, r);
                        drawTokenSubstring(t, selection.To.LinePos - linePos, tLen - (selection.To.LinePos - linePos),
                                           ref p, false, g, r);
                    }
                        // draw unselected tokens:
                    else if (l == selection.From.Line && linePos + tLen <= selection.From.LinePos ||
                             l == selection.To.Line && linePos >= selection.To.LinePos)
                    {
                        if (tLen > MAX_TOKEN_LEN) drawLongString(g, t.Value, t.Color, Color.Transparent, ref p, r);
                        else drawString(g, t.Value, Font, p, t.Color, Color.Transparent, t.Width);
                    }
                        // draw entirely selected tokens:
                    else
                    {
                        if (tLen > MAX_TOKEN_LEN) drawLongString(g, t.Value, selTextColor, selBackColor, ref p, r);
                        else drawString(g, t.Value, Font, p, selTextColor, selBackColor, t.Width);
                    }
                    p.X = startX + t.Width;
                    linePos += tLen;
                    t = t.Next;
                }
            }
                // draw lines which are completely selected:
            else
            {
                var sb = new StringBuilder(l.Length);
                int strWidth = 0;
                while (t != null && p.X + strWidth <= r.Right)
                {
                    sb.Append(t.Value);
                    strWidth += t.Width;
                    t = t.Next;
                }
                if (sb.Length > MAX_TOKEN_LEN) drawLongString(g, sb.ToString(), selTextColor, selBackColor, ref p, r);
                drawString(g, sb.ToString(), Font, p, selTextColor, selBackColor, strWidth);
            }
        }

        /// <summary>
        /// Helper method which draws the substring of a token value defined by the provided start position
        /// and length parameter. The method can be used to draw selected or unselected substrings and 
        /// calculates the start position of the next token or substring to render.
        /// </summary>
        /// <param name="t">Token which should be (partly) drawn.</param>
        /// <param name="start">Start position inside the token.</param>
        /// <param name="len">Length of the substring to render.</param>
        /// <param name="p">Reference to the starting point of the substring to render (has to be set accordingly!)</param>
        /// <param name="selected">Determines if the string to render is selected or not.</param>
        /// <param name="g">Graphics context to draw onto.</param>
        /// <param name="r">Bounding rectangle of the line to render.</param>
        protected virtual void drawTokenSubstring(TToken t, int start, int len, ref Point p, bool selected, Graphics g,
                                                  RectangleF r)
        {
            if (start < 0) start = 0;
            if (start >= t.Length) return;
            if (len < 0) return;
            if (len + start > t.Length) len = t.Length - start;
            String s = t.Value.Substring(start, len);
            int strWidth = measureString(g, s, Font).Width;
            if (s.Length > MAX_TOKEN_LEN)
            {
                if (selected) drawLongString(g, s, selTextColor, selBackColor, ref p, r);
                else drawLongString(g, s, t.Color, Color.Transparent, ref p, r);
            }
            else
            {
                if (selected) drawString(g, s, Font, p, selTextColor, selBackColor, strWidth);
                else drawString(g, s, Font, p, t.Color, Color.Transparent, strWidth);
                p.X += strWidth;
            }
        }

        /// <summary>
        /// Delegate method which can be used to craw margins that have the color of the controls BackColor
        /// property - used to draw the additional padding between the TextArea and the rendered lines.
        /// </summary>
        /// <param name="g">Graphics object to draw on with the clipping area set accordingly.</param>
        /// <returns>True on success. False on error.</returns>
        protected bool drawBackground(Graphics g)
        {
            g.Clear(BackColor);
            return true;
        }

        /// <summary>
        /// Delegate method for drawing additional margins. Called from the delegates within the ReservedAreas
        /// that represent page margins.
        /// </summary>
        /// <param name="g">Graphics object to draw on with the clipping area set accordingly.</param>
        /// <returns>True on success. False on error.</returns>
        protected bool drawMargin(Graphics g)
        {
            g.Clear(mgBackColor);
            return true;
        }

        /// <summary>
        /// Method responsible for drawing the line numbers if there exists a reserved line number area on the
        /// left edge of the client area. If the reserved area is too small to render the line numbers the method
        /// causes a recalculation / redraw of the whole client area and returns false to abort all further 
        /// drawing of reserved areas.
        /// </summary>
        /// <param name="g">Graphics object to draw to - has to have the clipping region set accordingly!</param>
        /// <returns>True on success. False if the area to draw on is too small.</returns>
        protected virtual bool drawLineNumbers(Graphics g)
        {
            g.Clear(lnBackColor);
            int reservedLength = getResLength(ReservedLocation.Left, ColorTextBox.LN_RESAREA_ID);
            if (firstVisLine != null && reservedLength > 0)
            {
                int y = (int) g.ClipBounds.Y + padding.Top;
                var x = (int) g.ClipBounds.X;
                int count = LineCount;
                Size maxSize = measureString(g, count.ToString(), lnFont);
                int maxW = maxSize.Width + RES_BORDER_WIDTH;
                if (maxW > reservedLength || maxW < reservedLength)
                {
                    changeResLength(maxW, ReservedLocation.Left, ColorTextBox.LN_RESAREA_ID);
                    calcLinePositions(true, true);
                    return false;
                }
                TLine curr = firstVisLine;
                int num = FirstVisLineNumber;
                int hOffSet = (LineHeight - maxSize.Height)/2;
                var p = new Point(x + RES_BORDER_WIDTH/2, y + hOffSet);
                while (curr != null && curr != lastVisLine.Next)
                {
                    drawString(g, num.ToString(), lnFont, p, lnTextColor, Color.Transparent, -1);
                    num++;
                    p.Y += LineHeight;
                    curr = curr.Next;
                }
            }
            return true;
        }

        /// <summary>
        /// Delegate method for drawing the current caret position information at the bottom of the editor. 
        /// </summary>
        /// <param name="g">Graphics object to draw on with the clipping area set accordingly.</param>
        /// <returns>True on success. False on error.</returns>
        private bool drawCaretInfo(Graphics g)
        {
            g.Clear(ciBackColor);
            String text = "Line: - Position: -";
            if (caret != null) text = "Line: " + caret.LineNumber + " Position: " + caret.LinePos;
            int strW = measureString(g, text, ciFont).Width;
            float areaW = g.ClipBounds.Width;
            int hPos = (int) (areaW - strW)/2;
            var p = new Point(hPos, (int) g.ClipBounds.Location.Y + 1);
            drawString(g, text, ciFont, p, ciTextColor, Color.Transparent, strW);
            return true;
        }

        /// <summary>
        /// Draws the caret at the current position. To be called from the caret timer as repeated calls to
        /// this method make the caret blink.
        /// </summary>
        protected virtual void invertCaret()
        {
            if (caret != null && caret.Line.IsVisible && !suspendPaint && showCaret)
            {
                if (!caret.IsValid) caret = Pos(caret.LinePos, caret.Line);
                if (caret != null && TextArea.Contains(caret.X + xOffSet, caret.Line.Y + LineHeight))
                {
                    bool caretRedraw = false;
                    var redraw = new Rectangle(caret.X + xOffSet, caret.Line.Y, 1, caret.Line.Height);
                    Graphics g = getGraphics(null);
                    if (!insertMode)
                    {
                        int width = measureString(g, caret.Line.CharAt(caret.LinePos).ToString(), Font).Width;
                        if (width < 1) width = LineHeight/2;
                        redraw.Width = width + 1;
                        redraw.Height++;
                    }
                    if (caretPen.Color == BackColor) caretPen.Color = caretColor;
                    else
                    {
                        // when deleting caret - redraw characters at caret position
                        caretRedraw = true;
                        caretPen.Color = BackColor;
                    }
                    Region redrawReg = null;
                    if (insertMode)
                    {
                        if (Font.Italic)
                        {
                            var topP = new Point();
                            topP.Y = caret.Line.Y;
                            var itOff = (int) (LineHeight/Math.Tan(1.309));
                            topP.X = caret.X - 2 + xOffSet + itOff;
                            redraw.X -= 2;
                            redraw.Width += itOff;
                            redrawReg = new Region(redraw);
                            g = getGraphics(redrawReg);
                            g.DrawLine(caretPen, topP,
                                       new Point(caret.X - 2 + xOffSet, caret.Line.Y + LineHeight));
                        }
                        else
                        {
                            redrawReg = new Region(redraw);
                            g = getGraphics(redrawReg);
                            g.DrawLine(caretPen, new Point(caret.X + xOffSet, caret.Line.Y),
                                       new Point(caret.X + xOffSet, caret.Line.Y + LineHeight));
                        }
                    }
                    else
                    {
                        IntPtr dc = g.GetHdc();
                        var destRect = new RECT(redraw.X, redraw.Y, redraw.Right, redraw.Bottom);
                        PlatformInvokeUSER32.InvertRect(dc, ref destRect);
                        g.ReleaseHdc();
                    }
                    if (caretRedraw)
                    {
                        drawContent(caret.Line, g, DrawMode.Caret, true);
                    }
                    else
                    {
                        OnInvalidateRegion(redrawReg);
                    }
                }
            }
        }

        /// <summary>
        /// Responsible for redrawing the Control when an Insert event occured. Override to use token based
        /// font calculation! This implementation assumes that the insertion line was visible before, i.e. the
        /// caller has to ensure to scroll to the insertion line if necessary before insertion!
        /// </summary>
        /// <param name="args">UpdateEventArgs object of this insert event.</param>
        protected virtual void drawOnInsert(UpdateEventArgs args)
        {
            if (args.From is Position<TLine, TToken> && args.To is Position<TLine, TToken>)
            {
                var fromP = (Position<TLine, TToken>) args.From;
                var toP = (Position<TLine, TToken>) args.To;
                TLine fromL = fromP.Line;
                TLine toL = toP.Line;
                Graphics g = getGraphics(null);
                // Update all fields if multiple lines have been inserted:
                if (args.Lines > 0)
                {
                    TLine oldLastVis = lastVisLine;
                    // correct lastVisLine reference if we inserted into lastVisLine
                    if (fromL == lastVisLine) lastVisLine = toL;
                    TLine startCalc = fromL;
                    // mark old visible lines invisible:
                    setLinesInvisible(fromL, lastVisLine);
                    // check if we have to scroll down and calculate new first visible line if necessary:
                    int dist = DocumentBase<TLine, TToken>.DistanceBetween(toL, lastVisLine);
                    if (args.Lines > dist && dist > 0
                        || dist == 0 && fromL.Y + (args.Lines + 1)*LineHeight >= TextArea.Bottom)
                    {
                        for (int i = 0; i < args.Lines - dist && firstVisLine.Next != null; i++)
                        {
                            firstVisLine.IsVisible = false;
                            firstVisLine = firstVisLine.Next;
                            if (VScroll.Value + 1 <= VScroll.Maximum) VScroll.Value++;
                        }
                        startCalc = firstVisLine;
                    }
                    // Special handling of single-line insertion, e.g. Enter (for performance reasons)
                    if (args.Lines == 1 && firstVisLine != null && lastVisLine != null)
                    {
                        // Insertion requires scroll down:
                        if (startCalc == firstVisLine && fromL != firstVisLine)
                        {
                            // make BitBlt copy of lines before insertion start line
                            var srcR = new Rectangle(firstVisLine.X, firstVisLine.Y,
                                                     TextArea.Width, fromL.Y - firstVisLine.Y);
                            var destR = new Rectangle(TextArea.X, TextArea.Y,
                                                      TextArea.Width, fromL.Y - firstVisLine.Y);
                            calcLinePositions(false, false);
                            makeBitBltCopy(srcR, destR);
                        }
                        else
                        {
                            // first calculate height of the area to copy
                            int height = oldLastVis.Y - fromL.Y;
                            // if current lastVisLine will not be visible afterwards:
                            if (oldLastVis.Y + 2*LineHeight > TextArea.Bottom)
                            {
                                height -= LineHeight;
                            }
                            // make BitBlt copy of lines after insertion end line up to the new lastVisLine
                            var srcR = new Rectangle(fromL.X, fromL.Y + LineHeight,
                                                     TextArea.Width, height);
                            // calculate new positions of lines to determine new position of lastVisLine
                            calcLinePosFrom(fromL, false, false);
                            var destR = new Rectangle(toL.X, toL.Y + LineHeight,
                                                      TextArea.Width, height);
                            makeBitBltCopy(srcR, destR);
                        }
                        // finally redraw the insertion line and its' follower:
                        var bounds = new Rectangle(fromL.X, fromL.Y, TextArea.Width, 2*LineHeight);
                        var boundReg = new Region(bounds);
                        g = getGraphics(boundReg);
                        drawContent(fromL, g, DrawMode.Lines, args.Repaint);
                    }
                        // Handling of all other multi-line insertions:
                    else
                    {
                        // redraw everything if we have to scroll down:
                        if (startCalc == firstVisLine)
                        {
                            firstVisLine.X = TextArea.X;
                            firstVisLine.Y = TextArea.Y;
                            calcLinePosFrom(firstVisLine, true, args.Repaint);
                        }
                            // no scrolling necessary - redraw all lines starting at fromL:
                        else
                        {
                            calcLinePosFrom(fromL, true, args.Repaint);
                        }
                    }
                }
                    // Insertion only in one line
                else
                {
                    var bounds = new Rectangle(fromL.X, fromL.Y, TextArea.Width, LineHeight);
                    g = getGraphics(new Region(bounds));
                    drawContent(fromL, g, DrawMode.Lines, args.Repaint);
                }
                if (args.Repaint)
                {
                    if (args.Action == UpdateAction.Undoable || args.Action == UpdateAction.NonAtomic) setCaret(toP);
                    else setSelection(fromP, toP);
                }
                if (args.Lines > 0)
                {
                    // TODO: may be drawn twice if a scroll event is raised as well! 
                    // redraw line numbers if necessary:
                    if (ShowLineNumbers) drawReservedArea(ReservedLocation.Left, LN_RESAREA_ID, args.Repaint);
                }
                checkHorizScrollBar();
            }
        }

        /// <summary>
        /// Responsible for redrawing the Control when a Delete event occured. Override to use token based
        /// font calculation! (NOTE: This implementation assumes that the line at which the deletion started
        /// was visible before deletion! This contract has to be honored by every procedure that triggers 
        /// delete events! The only exception is when deleting the CRLF token between firstVisLine and its'
        /// previous line which requires a single line scroll up that is handled by this method!)
        /// </summary>
        /// <param name="args">UpdateEventArgs object of this delete event.</param>
        protected virtual void drawOnDelete(UpdateEventArgs args)
        {
            if (args.From is Position<TLine, TToken>)
            {
                var fromP = (Position<TLine, TToken>) args.From;
                TLine delL = fromP.Line;
                Graphics g = getGraphics(null);
                // first always redraw delL (if it is visible!):
                Rectangle bounds;
                if (delL.IsVisible && !lastVisLine.Disposed)
                {
                    bounds = new Rectangle(delL.X, delL.Y, TextArea.Width, LineHeight);
                    g = getGraphics(new Region(bounds));
                    drawContent(delL, g, DrawMode.Lines, args.Repaint);
                }
                if (args.Lines > 0)
                {
                    if (VScroll.Maximum <= 0) VScroll.Maximum = 1;
                    // if we deleted up to the last line - set new lastVisLine, draw delL and clear area below (up to old lastVisLine)
                    if (delL.Next == null && delL.IsVisible)
                    {
                        bounds = new Rectangle(delL.X, delL.Y, TextArea.Width, lastVisLine.Y + LineHeight - delL.Y);
                        g = getGraphics(new Region(bounds));
                        lastVisLine = delL;
                        drawContent(delL, g, DrawMode.Lines, args.Repaint);
                    }
                    else
                    {
                        // check if we only delete firstVisLine - if so make delL new firstVisLine and redraw it
                        if (delL == firstVisLine && !delL.IsVisible && args.Lines == 1)
                        {
                            if (delL.Next == null) lastVisLine = delL;
                            firstVisLine = delL;
                            firstVisLine.X = TextArea.X;
                            firstVisLine.Y = TextArea.Y;
                            firstVisLine.IsVisible = true;
                            bounds = new Rectangle(firstVisLine.X, firstVisLine.Y, TextArea.Width, LineHeight);
                            g = getGraphics(new Region(bounds));
                            drawContent(firstVisLine, g, DrawMode.Lines, args.Repaint);
                            if (VScroll.Value - 1 >= VScroll.Minimum) VScroll.Value--;
                        }
                            // If we deleted across multiple lines (not including last line) and delL was visible before:
                        else
                        {
                            // if the line after delL was visible before deletion make a BitBLT copy of all following lines:
                            if (delL.Next.IsVisible)
                            {
                                TLine firstInv = lastVisLine.Next;
                                int height = lastVisLine.Y + LineHeight - delL.Next.Y;
                                var srcR = new Rectangle(delL.Next.X, delL.Next.Y, TextArea.Width, height);
                                var destR = new Rectangle(delL.X, delL.Y + LineHeight, TextArea.Width, height);
                                makeBitBltCopy(srcR, destR);
                                // recalculate positions of all visible lines following delL:
                                calcLinePosFrom(delL, false, false);
                                // finally redraw all lines following the old lastVisLine (if necessary):
                                if (firstInv != null)
                                {
                                    var redrawRec = new Rectangle(firstInv.X, firstInv.Y, TextArea.Width,
                                                                  lastVisLine.Y + LineHeight - firstInv.Y);
                                    drawContent(firstInv, getGraphics(new Region(redrawRec)), DrawMode.Lines,
                                                args.Repaint);
                                }
                                    // if old lastVisLine was last line clear section below lastVisLine:
                                else
                                {
                                    bounds = new Rectangle(new Point(lastVisLine.X, lastVisLine.Y + lastVisLine.Height),
                                                           new Size(TextArea.Width,
                                                                    TextArea.Height -
                                                                    (lastVisLine.Y + lastVisLine.Height)));
                                    g = getGraphics(new Region(bounds));
                                    g.Clear(BackColor);
                                }
                            }
                                // all visible lines following delL have been deleted --> redraw all lines following delL:
                            else
                            {
                                calcLinePosFrom(delL, true, args.Repaint);
                            }
                        }
                    }
                }
                // set the caret to the deletion point:
                if (args.Repaint) setCaret(fromP);
                // redraw line numbers if necessary:
                if (args.Lines > 0)
                {
                    // TODO: may be drawn twice if a scroll event is raised as well! 
                    // redraw line numbers if necessary:
                    if (ShowLineNumbers) drawReservedArea(ReservedLocation.Left, LN_RESAREA_ID, args.Repaint);
                }
                checkHorizScrollBar();
            }
        }

        /// <summary>
        /// Responsible for redrawing the ColorTextBox when the color of some text in the control is changed.
        /// </summary>
        /// <param name="args">UpdateEventArgs object of this delete event.</param>
        protected virtual void drawOnColorChange(UpdateEventArgs args)
        {
            if (args.From is Position<TLine, TToken> && args.To is Position<TLine, TToken>)
            {
                TLine fromL = ((Position<TLine, TToken>) args.From).Line;
                TLine toL = ((Position<TLine, TToken>) args.To).Line;
                // check if we need to redraw at all:
                if (fromL > lastVisLine) return;
                if (fromL < firstVisLine || fromL == firstVisLine)
                {
                    fromL = firstVisLine;
                }
                var redrawRect = new Rectangle(fromL.X, fromL.Y, textArea.Width,
                                               lastVisLine.Y + LineHeight - fromL.Y);
                drawContent(fromL, getGraphics(new Region(redrawRect)), DrawMode.Lines, args.Repaint);
            }
        }

        /* Helper methods used for drawing: */

        /// <summary>
        /// Calculates the size of the given string to be rendered.
        /// </summary>
        /// <param name="s">String to be measured.</param>
        /// <param name="f">Font to use for calculation.</param>
        /// <param name="g">Graphics context on which to draw.</param>
        /// <returns>Size of the bounding rectangle surrounding the given String.</returns>
        protected Size measureString(Graphics g, String s, Font f)
        {
            if (renderMode == RenderMode.GDI)
            {
                return TextRenderer.MeasureText(g, s.Replace("\t", TAB_STRING), f, propSize, formatFlags);
            }
            else if (renderMode == RenderMode.GDINative)
            {
                PlatformInvokeGDI32.SelectObject(buffHdc, f.ToHfont());
                var retSize = new Size(0, 0);
                s = s.Replace("\t", TAB_STRING).Replace(Environment.NewLine, "");
                PlatformInvokeGDI32.GetTextExtentPoint(buffHdc, s, s.Length, ref retSize);
                return retSize;
            }
            else
            {
                //return g.MeasureString(s.Replace("\t", TAB_STRING).Replace("\r\n", ""), f, g.ClipBounds.Location, stringFormat).ToSize();
                //return MeasureDisplayStringWidth(g, s.Replace("\t", TAB_STRING).Replace("\r\n", ""), f);
                var retS = new Size();
                var calcSF = new SizeF();
                PointF orig = g.ClipBounds.Location;
                s = s.Replace("\t", TAB_STRING).Replace(Environment.NewLine, "");
                foreach (char c in s)
                {
                    calcSF = g.MeasureString(c.ToString(), f, orig, stringFormat);
                    retS.Width += (int) (calcSF.Width*1.06);
                }
                retS.Height = (int) Math.Ceiling(calcSF.Height);
                return retS;
            }
        }

        /// <summary>
        /// Wrapper method that draws the given string to the background buffer.
        /// </summary>
        /// <param name="g">Graphics context to draw onto.</param>
        /// <param name="s">String to render.</param>
        /// <param name="f">Font to use for rendering.</param>
        /// <param name="p">Point determining the rendering position.</param>
        /// <param name="txCol">Color of the string to draw.</param>
        /// <param name="bgCol">Background color of the string to draw.</param>
        /// <param name="width">Optional width of the string to render - if width is unknown pass -1!</param>
        protected void drawString(Graphics g, String s, Font f, Point p, Color txCol, Color bgCol, int width)
        {
            if (renderMode == RenderMode.GDI)
            {
                TextRenderer.DrawText(g, s.Replace("\t", TAB_STRING), f, p, txCol, bgCol, formatFlags);
            }
            else if (renderMode == RenderMode.GDINative)
            {
                PlatformInvokeGDI32.SelectObject(buffHdc, f.ToHfont());
                RectangleF cR = g.ClipBounds;
                var rec = new RECT((int) cR.Left, (int) cR.Top, (int) cR.Right, (int) cR.Bottom);
                s = s.Replace("\t", TAB_STRING).Replace(Environment.NewLine, "");
                ;
                int col = ColorTranslator.ToWin32(txCol);
                PlatformInvokeGDI32.SetTextColor(buffHdc, col);
                col = ColorTranslator.ToWin32(bgCol);
                PlatformInvokeGDI32.SetBkColor(buffHdc, col);
                PlatformInvokeGDI32.TextOut(buffHdc, p.X, p.Y, s, s.Length);
                //PlatformInvokeGDI32.ExtTextOut(buffHdc, p.X, p.Y, 0, ref rec, s, (uint)s.Length, null);
            }
            else
            {
                bool transp = (Color.Transparent == bgCol);
                using (Brush t = new SolidBrush(txCol), b = new SolidBrush(bgCol))
                {
                    if (!transp)
                    {
                        Rectangle bound;
                        if (width >= 0) bound = new Rectangle(p, new Size(width, LineHeight));
                        else bound = new Rectangle(p, measureString(g, s, f));
                        g.FillRectangle(b, bound);
                    }
                    String rend;
                    int rendW;
                    s = s.Replace("\t", TAB_STRING).Replace(Environment.NewLine, "");
                    int sLen = s.Length;
                    for (int i = 0; i < sLen && p.X <= TextArea.Right; i++)
                    {
                        rend = s[i].ToString();
                        rendW = measureString(g, rend, f).Width;
                        if (p.X + rendW >= ClientRectangle.Left) g.DrawString(rend, f, t, p, stringFormat);
                        p.X += rendW;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the graphics buffer used for drawing operations. If a graphics buffer has not yet been initialized
        /// this method will call initGraphics() to do so. The passed region will be set on the returned offscreen
        /// graphics object and additionally be added to the clipping region of the screenGraphics object! Thus
        /// passing a suitably sized clipping region can greatly improve rendering performance!
        /// If a null reference is passed as clipping region the clipping region of the screenGraphics object will
        /// not be changed but the clipping region of the returned offscreen graphics buffer will be set to an 
        /// empty region!
        /// </summary>
        /// <param name="clipRegion">Clipping region to add to the returned buffer.</param>
        /// <returns>The graphics object of the applications graphics buffer.</returns>
        protected Graphics getGraphics(Region clipRegion)
        {
            if (buffGraphics == null || screenGraphics == null) initGraphics();
            if (clipRegion != null)
            {
                buffGraphics.Clip.Dispose();
                buffGraphics.Clip = clipRegion;
                if (screenGraphics.ClipBounds.IsEmpty) screenGraphics.Clip = clipRegion;
                else
                {
                    Region newR = clipRegion.Clone();
                    newR.Union(screenGraphics.Clip);
                    screenGraphics.Clip.Dispose();
                    screenGraphics.Clip = newR;
                }
            }
            else
            {
                buffGraphics.Clip.MakeEmpty();
            }
            return buffGraphics;
        }

        /// <summary>
        /// This method should be called before any drawing operations are performed. It creates a graphics object
        /// for the ColorTextBox and allocates a compatible background graphics object on which all drawing is
        /// performed. When OnPaint() is called the background graphics is rendered to the screenGraphics object.
        /// </summary>
        protected void initGraphics()
        {
            screenGraphics = CreateGraphics();
            if (renderMode == RenderMode.GDI || renderMode == RenderMode.GDIPlus)
            {
                context.MaximumBuffer = new Size(Math.Max(ClientScrollRectangle.Width + 1, 1),
                                                 Math.Max(ClientScrollRectangle.Height + 1, 1));
                grafx = context.Allocate(screenGraphics, ClientScrollRectangle);
                if (grafx != null && grafx.Graphics != null && screenGraphics != null)
                {
                    grafx.Graphics.Clip = new Region(Rectangle.Empty);
                    grafx.Graphics.Clip.MakeEmpty();
                    screenGraphics.Clip = new Region(Rectangle.Empty);
                    screenGraphics.Clip.MakeEmpty();
                    buffGraphics = grafx.Graphics;

                    /*
                    buffGraphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                    screenGraphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                    grafx.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
                    grafx.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
                    grafx.Graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
                    grafx.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.Default;
                    grafx.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Default;
                    grafx.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Default;

                    screenGraphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
                    screenGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
                    screenGraphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
                    screenGraphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.Default;
                    screenGraphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Default;
                    screenGraphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Default;
                    */
                }
            }
            else
            {
                buffHdc = PlatformInvokeGDI32.CreateCompatibleDC(IntPtr.Zero);
                IntPtr hbmp = PlatformInvokeGDI32.CreateBitmap(ClientScrollRectangle.Width,
                                                               ClientScrollRectangle.Height, 1, 32, IntPtr.Zero);
                PlatformInvokeGDI32.SelectObject(buffHdc, hbmp);
                buffGraphics = Graphics.FromHdc(buffHdc);
                //LOGFONT lf = new LOGFONT();
                //Font.ToLogFont(lf, buffGraphics);
                IntPtr gdiFont = Font.ToHfont(); //PlatformInvokeGDI32.CreateFontIndirect(ref lf);
                PlatformInvokeGDI32.SelectObject(buffHdc, gdiFont);
                buffGraphics.Clear(BackColor);
            }
            buffGraphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            screenGraphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
        }

        /// <summary>
        /// This method copies the screen rectangle specified by screenSrc to the rectangle graphDest inside
        /// the graphics buffer. Used for high-performance scrolling.
        /// NOTE: Calling this method sets the clipping region of the background buffer to the provided destination
        /// rectangle - this has to be considered when the caller continues to draw on the buffer after calling
        /// this method!
        /// </summary>
        /// <param name="screenSrc">Source rectangle (in screen coordinates).</param>
        /// <param name="graphDest">Destination rectangle inside the graphics buffer.</param>
        protected void makeBitBltCopy(Rectangle screenSrc, Rectangle graphDest)
        {
            Graphics g = getGraphics(new Region(graphDest));
            IntPtr hdc = buffHdc;
            if (renderMode == RenderMode.GDI || renderMode == RenderMode.GDIPlus) hdc = g.GetHdc();
            PlatformInvokeGDI32.BitBlt(hdc, graphDest.X, graphDest.Y, graphDest.Width, graphDest.Height,
                                       hdc, screenSrc.X, screenSrc.Y, SRC_COPY);
            if (hdc != buffHdc) g.ReleaseHdc();
        }

        /// <summary>
        /// Deallocates graphics context and buffer used for rendering!
        /// </summary>
        protected void disposeGraphics()
        {
            if (context != null) context.Invalidate();
            if (grafx != null) grafx.Dispose();
            if (buffGraphics != null) buffGraphics.Dispose();
            if (screenGraphics != null) screenGraphics.Dispose();
        }
    }
}