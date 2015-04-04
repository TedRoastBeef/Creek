using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Creek.UI.ColorTextbox
{
    /// <summary>
    /// Determines the mode used for rendering the text on the screen
    /// </summary>
    public enum RenderMode
    {
        /// <summary>
        /// Uses native .Net 2.0 code to render the lines (Graphics.DrawString)
        /// </summary>
        GDIPlus,

        /// <summary>
        /// Uses .Net 2.0 wrapper classes to access the GDI32 library (TextRenderer)
        /// </summary>
        GDI,

        /// <summary>
        /// Uses only native GDI32 libraries for rendering
        /// NOTE: This RenderMode setting is for testing purposes only and does currently NOT work!
        /// </summary>
        GDINative
    }

    /// <summary>
    /// A ColorTextBox represents a user control for editing text similar to a TextBox or RichTextBox. The main
    /// difference to the mentioned controls is the way in which a ColorTextBox stores the text internally.
    /// Text is stored directly inside a linked list of lines which consist of a linked list of tokens themselves.
    /// Each token contains the value of the part of the line it refers to and can be extended to contain many
    /// more properties to describe the respective part of the line (the abstract default implementation
    /// TokenBase contains fields for value, color, rendering height and width).
    /// </summary>
    /// <typeparam name="TLine">A concrete implementation of the abstract base class LineBase.</typeparam>
    /// <typeparam name="TToken">A concrete implementation of the abstract base class TokenBase.</typeparam>
    /// <typeparam name="TDocument">A concrete implementation of the abstract base class DocumentBase.</typeparam>
    public abstract partial class ColorTextBoxBase<TDocument, TLine, TToken> : UserControl, IColorTextBox
        where TToken : TokenBase<TToken>, new()
        where TLine : LineBase<TLine, TToken>, new()
        where TDocument : DocumentBase<TLine, TToken>, new()
    {
        /***** PROTECTED FIELDS: *****/

        // Constants for general use:
        protected const char EOF = Char.MaxValue; // constant character value that represents the end of a file
        protected const int H_SCROLL_DELTA = 50; // amount of pixels to scroll horizontally when scrolling one notch

        protected const int H_SCROLL_DIST = 10;
        // distance in pixels between the caret and the left or right border that

        // triggers a horizontal scroll of H_SCROLL_DELTA
        protected const String TAB_STRING = "  "; // String to insert instead of tab characters

        protected const String DELIM_CHARS = " \t\r\n <>|,.;:-#'+*~^°!\"§$%&/()=?´`²³{[]}\\@€";
        // characters that delimit words

        protected const int MARGIN_WIDTH = 0; // width of margins around TextArea and between reserved areas
        protected const int PADDING_WIDTH = 2; // Margin between TextArea and actual text.
        protected const int RES_BORDER_WIDTH = 4; // Width to add to reserved areas that render text (e.g. line numbers)
        protected const int MAX_UNDO_DEPTH = 100; // Depth of undo buffer
        protected const int MAX_REDO_DEPTH = 100; // Depth of redo buffer
        protected const String LN_RESAREA_ID = "LNRA"; // Id string of the reserved area used for line number display
        protected const String CI_RESAREA_ID = "CIRA"; // Id string of the reserved area used for caret info display
        protected const String MG_RESAREA_ID = "MGRA"; // Id string for reserved areas that represent margins
        protected const String LN_MARGIN_ID = "LNMG"; // Id string for the additional line number area margin
        protected const String CI_MARGIN_ID = "CIMG"; // Id string for the additional caret info area marging
        protected const int BLINK_INTERVAL = 500; // Interval in ms used for blink timer
        protected const int SRC_COPY = 13369376; // BitBlt mode used for copying areas of the buffer

        public const int MAX_TOKEN_LEN = 512;
        //24576;      // Determines the maximum character length of a token - needed for 

        /// <summary>
        /// The default background color of a ColorTextBox.
        /// </summary>
        public new static readonly Color DefaultBackColor = Color.White;

        /// <summary>
        /// The default forecolor (= text color) of a ColorTextBox.
        /// </summary>
        public new static readonly Color DefaultForeColor = Color.Black;

        /// <summary>
        /// The default background color of a selection.
        /// </summary>
        public static readonly Color DefaultSelectionBackColor = Color.CornflowerBlue;

        /// <summary>
        /// The default text color of a selection.
        /// </summary>
        public static readonly Color DefaultSelectionColor = Color.White;

        /// <summary>
        /// The default color of additional margins around the TextArea of ColorTextBox.
        /// </summary>
        public static readonly Color DefaultMarginColor = Color.FromArgb(230, 230, 230, 240);

        /// <summary>
        /// The default color of the caret.
        /// </summary>
        public static readonly Color DefaultCaretColor = Color.Black;

        /// <summary>
        /// The default background color of the caret position info area.
        /// </summary>
        public static readonly Color DefaultCaretInfoBackColor = Color.BlanchedAlmond;

        /// <summary>
        /// The default foreground (= text color) of the caret position info area.
        /// </summary>
        public static readonly Color DefaultCaretInfoForeColor = Color.Gray;

        /// <summary>
        /// The default background color of the line number area.
        /// </summary>
        public static readonly Color DefaultLineNumberBackColor = Color.BlanchedAlmond;

        /// <summary>
        /// The default foreground (= text color) of the line number area.
        /// </summary>
        public static readonly Color DefaultLineNumberForeColor = Color.Gray;

        /// <summary>
        /// The default font of the caret position info area.
        /// </summary>
        public static readonly Font DefaultCaretInfoFont = new Font("Tahoma", 8.25f, FontStyle.Regular);

        /// <summary>
        /// The default font of the ColorTextBox.
        /// </summary>
        public new static readonly Font DefaultFont = new Font("Microsoft Sans Serif", 8.25f);

        /// <summary>
        /// The default font of the line number area.
        /// </summary>
        public static readonly Font DefaultLineNumberFont = new Font(FontFamily.GenericMonospace, 8.25f);

        // line drawing because TextRenderer.DrawText somehow refuses to draw very long strings! 
        // A list of ReservedArea's that the control may not use for rendering text.

        // Fields for storing and manipulating contained text:
        private static RichTextBox rtb; // RichTextBox used for conversion from and to RTF
        protected new HScrollBar HScroll;
        protected new VScrollBar VScroll;
        private bool acceptsReturn;
        private bool acceptsTab;
        protected int bottomWidths; // reserver bottom area width
        protected Graphics buffGraphics;
        protected IntPtr buffHdc;

        protected Position<TLine, TToken> caret; // The caret
        protected Color caretColor; // Color of the caret
        protected Pen caretPen; // Pen used for drawing the text
        protected Timer caretTimer; // Timer used for caret blinking
        private CharacterCasing casing;
        private Color ciBackColor;
        private Font ciFont;
        private Color ciTextColor;
        protected BufferedGraphicsContext context;
        protected DataFormats.Format ctbFormat; // DataFormat of the current ColorTextBox implementation
        private bool ctbfSupported;
        protected TDocument document; // document contained in Control
        protected String fileName;
        protected String filePath;
        protected TLine firstVisLine; // first visible line
        protected TextFormatFlags formatFlags; // TODO: remove when refactoring finished!
        protected BufferedGraphics grafx;
        protected bool insertMode; // Determines editing mode (true = chars will be inserted,
        private bool isReadOnly;
        protected TLine lastVisLine; // last visible line
        protected int leftWidths; // reserved left area width
        protected int lineBaseDist; // Distance of the line base from line start (we use consistent font sizes!)
        protected int lineHeight; // Height of the lines (we use consistent font sizes!)
        // false = text replaces current caret character)

        // Fields for Selection handling:
        private Color lnBackColor;
        private Font lnFont;
        private Color lnTextColor;

        // Fields for caret info area display:

        // Fields for drawing operations:
        protected Padding margin; // Margin around TextArea
        protected int maxWidth; // width of the longest visible line
        protected Color mgBackColor; // Fill Color for TextArea margins
        private bool modified;
        protected Position<TLine, TToken> oldCaret; // Backup of the previous caret if it changes
        protected Padding padding; // Additional padding for the content of the TextArea
        protected Size propSize; // Size structure used to calculate line metrics
        protected ArrayList redo;
        protected RenderMode renderMode; // determines if GDI, native GDI or GDI+ rendering is enabled
        protected List<ReservedArea>[] reservedAreas;
        protected int rightWidths; // reserved right area width

        private bool rtfSupported;
        protected Graphics screenGraphics;
        private ScrollBars scrollBars;
        protected Color selBackColor;
        protected Color selTextColor;
        protected Selection<TLine, TToken> selection; // stores start and end positions of a selected text
        protected Position<TLine, TToken> selectionAnchor; // the anchor position where a mouse drag started
        private bool shortCutsEnabled;
        private bool showCaret;
        private bool showCaretInfo;
        private bool showLineNums;
        protected StringFormat stringFormat; // formatting flags for rendering with Graphics.DrawString
        private bool suspendPaint;
        protected Rectangle textArea; // the current display area for text
        protected int topWidths; // reserved top area width
        protected ArrayList undo;
        protected int xOffSet; // Offset to add to the start x position of each line

        /***** CONSTRUCTORS: *****/

        /// <summary>
        /// Parameterless default constructor.
        /// </summary>
        protected ColorTextBoxBase()
        {
            /* Initialize local fields: */
            context = new BufferedGraphicsContext();
            renderMode = RenderMode.GDI;
            document = new TDocument();
            document.UpdateEvent += handleUpdateEvent;
            undo = new ArrayList();
            redo = new ArrayList();
            HScroll = new HScrollBar();
            VScroll = new VScrollBar();
            (VScroll).BackColor = Color.Blue;
            (HScroll).BackColor = Color.Blue;
            HScroll.Visible = false;
            VScroll.Visible = false;
            VScroll.VisibleChanged += ScrollBar_VisibleChanged;
            HScroll.VisibleChanged += ScrollBar_VisibleChanged;
            scrollBars = ScrollBars.None;
            HScroll.Enabled = true;
            VScroll.Enabled = true;
            VScroll.Cursor = Cursors.Arrow;
            HScroll.Cursor = Cursors.Arrow;
            Cursor = Cursors.IBeam;
            HScroll.Dock = DockStyle.Bottom;
            VScroll.Dock = DockStyle.Right;
            HScroll.Scroll += HScroll_Scroll;
            VScroll.Scroll += VScroll_Scroll;
            Controls.Add(HScroll);
            Controls.Add(VScroll);
            reservedAreas = new List<ReservedArea>[4];
            for (int i = 0; i < 4; i++)
            {
                reservedAreas[i] = new List<ReservedArea>(2);
            }
            margin = new Padding(MARGIN_WIDTH);
            padding = new Padding(PADDING_WIDTH);
            topWidths = -1;
            leftWidths = -1;
            rightWidths = -1;
            bottomWidths = -1;
            showLineNums = false;
            showCaret = true;
            showCaretInfo = false;
            suspendPaint = false;
            acceptsReturn = true;
            acceptsTab = true;
            ctbfSupported = true;
            rtfSupported = false;
            shortCutsEnabled = true;
            casing = CharacterCasing.Normal;
            BackColor = ColorTextBox.DefaultBackColor;
            ForeColor = ColorTextBox.DefaultForeColor;
            selBackColor = ColorTextBox.DefaultSelectionBackColor;
            selTextColor = ColorTextBox.DefaultSelectionColor;
            mgBackColor = ColorTextBox.DefaultMarginColor;
            caretColor = ColorTextBox.DefaultCaretColor;
            ciBackColor = ColorTextBox.DefaultCaretInfoBackColor;
            ciTextColor = ColorTextBox.DefaultCaretInfoForeColor;
            lnBackColor = ColorTextBox.DefaultLineNumberBackColor;
            lnTextColor = ColorTextBox.DefaultLineNumberForeColor;
            caretPen = new Pen(BackColor);
            lineHeight = 0;
            lineBaseDist = 0;
            insertMode = true;
            xOffSet = 0;
            textArea = Rectangle.Empty;
            propSize = new Size(int.MaxValue, int.MaxValue);
            caretTimer = new Timer();
            caretTimer.Interval = BLINK_INTERVAL;
            caretTimer.Tick += caretTimer_Tick;
            caretTimer.Start();
            selection = null;
            // add additional margins to the left and right side and dummy margins to the top and bottom

            reservedAreas[(int) ReservedLocation.Left].Add(new ReservedArea(MG_RESAREA_ID, margin.Left, drawMargin, null,
                                                                            null));
            reservedAreas[(int) ReservedLocation.Right].Add(new ReservedArea(MG_RESAREA_ID, margin.Right, drawMargin,
                                                                             null, null));
            reservedAreas[(int) ReservedLocation.Top].Add(new ReservedArea(MG_RESAREA_ID, margin.Top, drawMargin, null,
                                                                           null));
            reservedAreas[(int) ReservedLocation.Bottom].Add(new ReservedArea(MG_RESAREA_ID, margin.Bottom, drawMargin,
                                                                              null, null));

            /* Initialize inherited fields: */
            ctbFormat = DataFormats.GetFormat(GetType().Name + "_DataFormat");
            SetStyle(ControlStyles.UserPaint
                     | ControlStyles.UserMouse
                     | ControlStyles.ResizeRedraw
                     | ControlStyles.AllPaintingInWmPaint
                     | ControlStyles.Opaque
                     | ControlStyles.CacheText
                     | ControlStyles.EnableNotifyMessage
                     //| ControlStyles.OptimizedDoubleBuffer
                     , true);
            SetStyle(ControlStyles.ContainerControl
                     | ControlStyles.OptimizedDoubleBuffer
                     , false);
            DoubleBuffered = false;
            UpdateStyles();
            // TODO: remove when migration to Graphics.DrawString is done
            formatFlags = TextFormatFlags.NoPadding
                          | TextFormatFlags.PreserveGraphicsClipping
                          | TextFormatFlags.NoPrefix
                          | TextFormatFlags.GlyphOverhangPadding
                          | TextFormatFlags.Left
                //| TextFormatFlags.ExpandTabs
                ;
            stringFormat = new StringFormat(StringFormat.GenericTypographic);
            stringFormat.Trimming = StringTrimming.None;
            //stringFormat.LineAlignment = StringAlignment.Near;
            stringFormat.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces
                                        //| StringFormatFlags.DirectionVertical
                                        | StringFormatFlags.NoFontFallback
                //| StringFormatFlags.NoWrap
                //| StringFormatFlags.LineLimit
                //| StringFormatFlags.DirectionRightToLeft
                //| StringFormatFlags.FitBlackBox
                //| StringFormatFlags.NoClip
                ;
            initGraphics();
            ciFont = ColorTextBox.DefaultCaretInfoFont;
            Font = ColorTextBox.DefaultFont;
            lnFont = ColorTextBox.DefaultLineNumberFont;
            // reset default Control values:
            BorderStyle = BorderStyle.FixedSingle;
            AllowDrop = true;
            Text = String.Empty;
        }

        /// <summary>
        /// Gets or sets the first line in the ColorTextBox. Setting the content via this property allows the
        /// insertion of invalid lines - so the caller has to ensure that the line list to set is valid!
        /// </summary>
        [Browsable(false)]
        [DefaultValue(null)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(
            "Gets or sets the first line in the ColorTextBox. Setting the content via this property allows the insertion of invalid lines - so the caller has to ensure that the line list to set is valid!"
            )]
        public TLine FirstLine
        {
            get { return document.First; }
            set { document.First = value; }
        }

        /// <summary>
        /// Gets the last line in the ColorTextBox.
        /// </summary>
        [Browsable(false)]
        [Description("Gets the last line in the ColorTextBox.")]
        public TLine LastLine
        {
            get { return document.Last; }
        }

        /// <summary>
        /// This property can be used to enable or disable caret drawing. Setting the property to
        /// false will not remove the current caret (if there is any) and changing the caret position
        /// by clicking the control is still possible (although it will not be drawn anymore!).
        /// </summary>
        [Browsable(true)]
        [DefaultValue(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Description("Turns the rendering of the caret on or off - use the Caret property to set or remove the caret!")]
        public bool ShowCaret
        {
            get { return showCaret; }
            set
            {
                if (!value && caret != null && caretPen.Color != BackColor) invertCaret();
                showCaret = value;
            }
        }

        /// <summary>
        /// Gets or sets the background color of the text area.
        /// </summary>
        [Browsable(true)]
        [Description("Gets or sets the background color of the text area.")]
        public override Color BackColor
        {
            get { return base.BackColor; }
            set
            {
                base.BackColor = value;
                drawContent(firstVisLine, getGraphics(new Region(TextArea)), DrawMode.All, true);
            }
        }

        /// <summary>
        /// Gets or sets the color used for rendering the background of selections.
        /// </summary>
        [Browsable(true)]
        [Description("Gets or sets the color used for rendering the background of selections.")]
        public Color SelectionBackColor
        {
            get { return selBackColor; }
            set
            {
                selBackColor = value;
                drawContent(firstVisLine, getGraphics(new Region(TextArea)), DrawMode.All, true);
            }
        }

        /// <summary>
        /// Gets or sets the color used for rendering selected text.
        /// </summary>
        [Browsable(true)]
        [Description("Gets or sets the color used for rendering selected text.")]
        public Color SelectionTextColor
        {
            get { return selTextColor; }
            set
            {
                selTextColor = value;
                drawContent(firstVisLine, getGraphics(new Region(TextArea)), DrawMode.All, true);
            }
        }

        /// <summary>
        /// Gets or sets the color used for rendering additional margins.
        /// </summary>
        [Browsable(true)]
        [Description("Gets or sets the color used for rendering additional margins.")]
        public Color MarginColor
        {
            get { return mgBackColor; }
            set
            {
                mgBackColor = value;
                drawContent(firstVisLine, getGraphics(new Region(TextArea)), DrawMode.All, true);
            }
        }

        /// <summary>
        /// Returns the first visible line.
        /// </summary>
        [Browsable(false)]
        [Description("Returns the first visible line.")]
        public TLine FirstVisibleLine
        {
            get { return firstVisLine; }
        }

        /// <summary>
        /// Returns the last visible line.
        /// </summary>
        [Browsable(false)]
        [Description("Returns the last visible line.")]
        public TLine LastVisibleLine
        {
            get { return lastVisLine; }
        }

        /// <summary>
        /// Gets or sets a value indicating if the ColorTextBox should place and retrieve data from
        /// the clipboard in the own ColorTextBoxFormat - setting this property to false improves
        /// performance but disables the support for colors in copy-paste operations.
        /// Furthermore setting this property to false also disables support for the RTF format!
        /// </summary>
        [Browsable(true)]
        [DefaultValue(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Description(
            "Gets or sets a value indicating whether the ColorTextBoxFormat is supported for Copy/Paste operations. Disabling improves performance!"
            )]
        public bool ColorTextBoxFormatSupported
        {
            get { return ctbfSupported; }
            set
            {
                ctbfSupported = value;
                if (!ctbfSupported) rtfSupported = false;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating if the ColorTextBox should place and retrieve data from
        /// the clipboard in the RichTextFormat. This property defaults to false as the conversion
        /// from and to RTF is currently extremely slow! Use it only to enable color support for
        /// copy-paste operations between different applications where performance is not an issue!
        /// Furthermore setting this property to true will also enable support for the 
        /// ColorTextBoxFormat which takes precedence over RTF!
        /// </summary>
        [Browsable(true)]
        [DefaultValue(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Description(
            "Gets or sets a value indicating whether RTF is supported for Copy/Paste operations. NOTE: This feature is currently very slow!"
            )]
        public bool RichTextFormatSupported
        {
            get { return rtfSupported; }
            set
            {
                rtfSupported = value;
                if (rtfSupported) ctbfSupported = true;
            }
        }

        /// <summary>
        /// Sets the margins between the TextArea and all reserved areas that may have been added to
        /// the control. 
        /// </summary>
        [Browsable(true)]
        [DefaultValue(typeof (Padding), "0")]
        [Description(
            "Sets the margins between the TextArea and all reserved areas that may have been added to the control. ")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public virtual Padding ReservedMargin
        {
            get { return margin; }
            set
            {
                if (value != null)
                {
                    margin = value;
                    changeResLength(margin.Left, ReservedLocation.Left, MG_RESAREA_ID);
                    changeResLength(margin.Right, ReservedLocation.Right, MG_RESAREA_ID);
                    changeResLength(margin.Top, ReservedLocation.Top, MG_RESAREA_ID);
                    changeResLength(margin.Bottom, ReservedLocation.Bottom, MG_RESAREA_ID);
                    changeResLength(margin.Left, ReservedLocation.Left, LN_MARGIN_ID);
                    changeResLength(margin.Bottom, ReservedLocation.Bottom, CI_MARGIN_ID);
                    calcLinePositions(true, true);
                }
            }
        }

        /// <summary>
        /// Gets or sets the current Font in the ColorTextBox.
        /// </summary>
        [Browsable(true)]
        [DefaultValue(typeof (Font), "FontFamily.GenericMonospace, 8.25f")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Description("Gets or sets the current Font in the ColorTextBox.")]
        public override Font Font
        {
            get { return base.Font; }
            set
            {
                if (value != null)
                {
                    removeSelection();
                    base.Font = value;
                }
            }
        }

        /// <summary>
        /// Returns a value indicating if painting to screen is currently disabled.
        /// </summary>
        [Browsable(false)]
        [Description("Returns a value indicating if painting to screen is currently disabled.")]
        public bool PaintSuspended
        {
            get { return suspendPaint; }
        }

        /// <summary>
        /// Gets or sets the BorderStyle of the ColorTextBox
        /// </summary>
        [Browsable(true)]
        [DefaultValue(typeof (BorderStyle), "1")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Description("Gets or sets the BorderStyle of the ColorTextBox")]
        public new BorderStyle BorderStyle
        {
            get { return base.BorderStyle; }
            set { base.BorderStyle = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether pressing ENTER in a ColorTextBox
        /// control creates a new line of text in the control or activates the default
        /// button for the form.
        /// </summary>
        [DefaultValue(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Description("Indicates if return characters are accepted as input for the ColorTextBox.")]
        public bool AcceptsReturn
        {
            get { return acceptsReturn; }
            set { acceptsReturn = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether pressing TAB in a ColorTextBox
        /// inserts a tab character or cycles through the controls.
        /// </summary>
        [DefaultValue(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Description("Indicates if tab characters are accepted as input for the ColorTextBox.")]
        public bool AcceptsTab
        {
            get { return acceptsTab; }
            set { acceptsTab = value; }
        }

        /// <summary>
        /// Indicates if all characters should be left alone or converted to uppercase or lowercase.
        /// </summary>
        [DefaultValue(0)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Description("Indicates if all characters should be left alone or converted to uppercase or lowercase.")]
        public CharacterCasing CharacterCasing
        {
            get { return casing; }
            set
            {
                casing = value;
                if (document != null) document.CharacterCasing = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the defined shortcuts are enabled.
        /// </summary>
        [DefaultValue(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Description("Gets or sets a value indicating whether the defined shortcuts are enabled.")]
        public virtual bool ShortcutsEnabled
        {
            get { return shortCutsEnabled; }
            set { shortCutsEnabled = value; }
        }

        /// <summary>
        /// This property is not supported by the ColorTextBox control.
        /// </summary>
        [DefaultValue(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("This property is not supported by the ColorTextBox control.")]
        public override bool AutoScroll
        {
            get { return base.AutoScroll; }
            set { }
        }

        /// <summary>
        /// This property is not supported by the ColorTextBox control.
        /// </summary>
        [DefaultValue(typeof (Size), "0, 0")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("This property is not supported by the ColorTextBox control.")]
        public new Size AutoScrollMinSize
        {
            get { return base.AutoScrollMinSize; }
            set { }
        }

        /// <summary>
        /// This property is not supported by the ColorTextBox control.
        /// </summary>
        [DefaultValue(typeof (Size), "0, 0")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("This property is not supported by the ColorTextBox control.")]
        public new Size AutoScrollMargin
        {
            get { return base.AutoScrollMargin; }
            set { }
        }

        /// <summary>
        /// This property is not supported in the ColorTextBox control.
        /// </summary>
        [Description("This property is not supported in the ColorTextBox control.")]
        public override Point AutoScrollOffset
        {
            get { return base.AutoScrollOffset; }
            set { }
        }

        /// <summary>
        /// Gets or sets which scroll bars should appear in a multiline TextBox control. 
        /// </summary>
        [Browsable(true)]
        [Description("Gets or sets which scroll bars should appear in a multiline TextBox control. ")]
        public ScrollBars ScrollBars
        {
            get { return scrollBars; }
            set
            {
                scrollBars = value;
                switch (value)
                {
                    case ScrollBars.None:
                        {
                            VScroll.Visible = false;
                            HScroll.Visible = false;
                        }
                        break;
                    case ScrollBars.Vertical:
                        {
                            HScroll.Visible = false;
                            VScroll.Visible = true;
                        }
                        break;
                    case ScrollBars.Horizontal:
                        {
                            VScroll.Visible = false;
                            HScroll.Visible = true;
                        }
                        break;
                    case ScrollBars.Both:
                        {
                            VScroll.Visible = true;
                            HScroll.Visible = true;
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        /***** PUBLIC METHODS: *****/

        #region IColorTextBox Members

        /// <summary>
        /// Suspends all further painting to the screen. After calling this method all changes to the documents data
        /// will not be drawn (neither to the background buffer nor to the screen!)
        /// </summary>
        public void SuspendPainting()
        {
            caretTimer.Stop();
            // remove any drawn selection:
            if (selection != null && selection.From != null && selection.To != null)
            {
                if (!selection.From.Equals(selection.To))
                {
                    TLine drawStart = selection.From.Line;
                    Graphics g = getGraphics(new Region(getVisibleBounds(selection.From, selection.To)));
                    if (!drawStart.IsVisible)
                    {
                        Selection<TLine, TToken> sel = getVisibleSelection(selection.From, selection.To);
                        if (sel != null) drawStart = sel.From.Line;
                    }
                    drawContent(drawStart, g, DrawMode.All, true);
                }
            }
            // remove the caret if it is drawn:
            if (caret != null && caretPen.Color != BackColor) invertCaret();
            suspendPaint = true;
        }

        /// <summary>
        /// Resumes painting to the screen and optionally draws all pending changes immediately.
        /// </summary>
        /// <param name="paintPending">When true all pending changes are painted immediately</param>
        public void ResumePainting(bool paintPending)
        {
            suspendPaint = false;
            if (paintPending)
            {
                calcLinePositions(true, true);
                if (selection != null) setSelection(selection.From, selection.To);
                else if (caret != null) setCaret(caret);
            }
            caretTimer.Start();
        }

        /// <summary>
        /// Tries to undo the last operation in the undo buffer and moves it to the redo buffer.
        /// Override when implementing own UpdateEventArgs!
        /// </summary>
        /// <returns>True on success.</returns>
        public virtual bool Undo()
        {
            if (undo.Count > 0 && !isReadOnly)
            {
                var args = (UpdateEventArgs) undo[undo.Count - 1];
                undo.RemoveAt(undo.Count - 1);
                TLine undoL;
                try
                {
                    undoL = document[args.FromLine];
                }
                catch (IndexOutOfRangeException)
                {
                    return false;
                }
                if (!undoL.IsVisible) scrollToLine(args.FromLine);
                removeCaret();
                removeSelection();
                args.Action = UpdateAction.Undo;
                document.InvokeInverseOp(args);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Removes all available undo operations from the undo buffer.
        /// </summary>
        public void ClearUndo()
        {
            if (undo != null && undo.Count > 0)
            {
                undo.Clear();
            }
        }

        /// <summary>
        /// Tries to redo the last undo operation in the redo buffer and adds it to the undo buffer again.
        /// Override when implementing own UpdateEventArgs!
        /// </summary>
        /// <returns></returns>
        public virtual bool Redo()
        {
            if (redo.Count > 0 && !isReadOnly)
            {
                var args = (UpdateEventArgs) redo[redo.Count - 1];
                redo.RemoveAt(redo.Count - 1);
                TLine redoL;
                try
                {
                    redoL = document[args.FromLine];
                }
                catch (IndexOutOfRangeException)
                {
                    return false;
                }
                if (!redoL.IsVisible) scrollToLine(args.FromLine);
                removeCaret();
                removeSelection();
                args.Action = UpdateAction.Redo;
                document.InvokeInverseOp(args);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Copies the currently selected text to the platforms' clipboard. Returns true on success or false
        /// if there isn't a valid selection to copy. This implementation places the selected text in the
        /// following data formats into the clipboard:
        /// ColorTextBoxFormat, StringFormat, UnicodeText and Rtf.
        /// </summary>
        /// <returns>True on success. False if there is no selection.</returns>
        public virtual bool Copy()
        {
            if (selection != null)
            {
                String plainText = document.SubString(selection.From, selection.To);
                var ctbDO = new DataObject();
                if (ctbfSupported)
                {
                    TLine clipLine = document.SubLines(selection.From, selection.To);
                    ctbDO.SetData(ColorTextBoxFormat, clipLine);
                    if (rtfSupported) ctbDO.SetData(DataFormats.Rtf, convertToRtf(clipLine));
                }
                ctbDO.SetData(DataFormats.UnicodeText, plainText);
                Clipboard.SetDataObject(ctbDO, true);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Tries to insert text from the system clipboard into the document at the current caret position.
        /// If there is currently some text selected it will be deleted prior to insertion.
        /// This method searches the clipboard for the following text formats and inserts the first one 
        /// encountered: ColorTextBoxFormat, StringFormat, UnicodeText and Text.
        /// </summary>
        /// <returns>True on success. False if there is no suitable data found in the clipboard.</returns>
        public virtual bool Paste()
        {
            if (!isReadOnly)
            {
                int format = -1;
                if (ctbfSupported && Clipboard.ContainsData(ColorTextBoxFormat)) format = 0;
                else if (rtfSupported && Clipboard.ContainsData(DataFormats.Rtf)) format = 4;
                else if (Clipboard.ContainsData(DataFormats.StringFormat)) format = 1;
                else if (Clipboard.ContainsData(DataFormats.UnicodeText)) format = 2;
                else if (Clipboard.ContainsData(DataFormats.Text)) format = 3;
                if (format > -1)
                {
                    Position<TLine, TToken> fromP = caret;
                    Position<TLine, TToken> toP = null;
                    if (selection != null && selection.From != null && selection.To != null)
                    {
                        fromP = selection.From;
                        toP = selection.To;
                    }
                    if (fromP != null)
                    {
                        redo.Clear();
                        if (!fromP.Line.IsVisible) scrollToLine(fromP.Line);
                        removeSelection();
                        removeCaret();
                        IDataObject ctbDO = Clipboard.GetDataObject();
                        String text = String.Empty;
                        switch (format)
                        {
                            case 0:
                                {
                                    var clipLine = (TLine) ctbDO.GetData(ColorTextBoxFormat);
                                    if (toP != null)
                                    {
                                        return document.Replace(fromP, toP, clipLine, UpdateAction.Undoable, true);
                                    }
                                    else
                                    {
                                        int ins1 = document.Insert(clipLine, fromP.Line, fromP.LinePos,
                                                                   UpdateAction.Undoable, true);
                                        if (ins1 > 0) return true;
                                        return false;
                                    }
                                }
                            case 1:
                                {
                                    text = (String) ctbDO.GetData(DataFormats.StringFormat);
                                }
                                break;
                            case 2:
                                {
                                    text = (String) ctbDO.GetData(DataFormats.UnicodeText);
                                }
                                break;
                            case 3:
                                {
                                    text = (String) ctbDO.GetData(DataFormats.Text);
                                }
                                break;
                            case 4:
                                {
                                    var rtf = (String) ctbDO.GetData(DataFormats.Rtf);
                                    TLine clipLine = convertFromRtf(rtf);
                                    if (toP != null)
                                    {
                                        return document.Replace(fromP, toP, clipLine, UpdateAction.Undoable, true);
                                    }
                                    else
                                    {
                                        int ins1 = document.Insert(clipLine, fromP.Line, fromP.LinePos,
                                                                   UpdateAction.Undoable, true);
                                        if (ins1 > 0) return true;
                                        return false;
                                    }
                                }
                            default:
                                break;
                        }
                        if (toP != null)
                        {
                            return document.Replace(fromP, toP, text, UpdateAction.Undoable, true);
                        }
                        else
                        {
                            int ins2 = document.Insert(text, fromP.Line, fromP.LinePos, UpdateAction.Undoable, true);
                            if (ins2 > 0) return true;
                            return false;
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Returns a boolean value indicating if text of the given format can be inserted into the
        /// ColorTextBox.
        /// </summary>
        /// <param name="format">Format to check.</param>
        /// <returns>True if content of the given format can be inserted.</returns>
        public virtual bool CanPaste(string format)
        {
            if (format.Equals(DataFormats.StringFormat) ||
                format.Equals(DataFormats.UnicodeText) ||
                format.Equals(DataFormats.Text) ||
                format.Equals(ColorTextBoxFormat))
            {
                if (!isReadOnly) return true;
                else return false;
            }
            return false;
        }

        /// <summary>
        /// Moves the currently selected text to the clipboard. Returns true on success or false if there
        /// isn't a valid selection to cut. This implementation places the selected text in the following
        /// data formats into the clipboard:
        /// ColorTextBoxFormat, StringFormat, UnicodeText and Rtf.
        /// </summary>
        /// <returns></returns>
        public bool Cut()
        {
            if (!isReadOnly)
            {
                bool ret = Copy();
                deleteSelection();
                return ret;
            }
            return false;
        }

        /// <summary>
        /// Deletes all text contained in the ColorTextBox.
        /// </summary>
        public void Clear()
        {
            if (FirstLine != null && document.Last != null)
            {
                Position<TLine, TToken> start = Pos(0, FirstLine);
                Position<TLine, TToken> end = Pos(document.Last.Length, document.Last);
                if (!start.Line.IsVisible) scrollToLine(start.Line);
                document.Delete(start, end, UpdateAction.Undoable, true);
            }
        }

        /// <summary>
        /// Appends the given string to the end of the ColorTextBox. Use this method instead of appending 
        /// text to the Text property with the "+" operator!
        /// </summary>
        /// <param name="text"></param>
        public void AppendText(String text)
        {
            redo.Clear();
            removeSelection();
            removeCaret();
            if (!document.Last.IsVisible) scrollToLine(document.Last);
            document.Insert(text, document.Last, document.Last.LastPos, UpdateAction.Undoable, true);
        }

        /// <summary>
        /// Inserts the given text at the current caret position. If there exists a selection the selected
        /// text will be replaced with the given string.
        /// If there is neither a caret nor a selection no changes will be made.
        /// </summary>
        /// <param name="text">The text to insert into the text box.</param>
        public void Insert(String text)
        {
            if (selection != null && selection.From != null && selection.To != null)
            {
                Position<TLine, TToken> fromP = selection.From;
                Position<TLine, TToken> toP = selection.To;
                if (!selection.From.Line.IsVisible)
                {
                    scrollToLine(selection.From.Line);
                    redo.Clear();
                    removeSelection();
                    removeCaret();
                    document.Replace(fromP, toP, text, UpdateAction.Undoable, true);
                }
            }
            else if (caret != null)
            {
                if (!caret.Line.IsVisible) scrollToLine(caret.Line);
                Position<TLine, TToken> fromP = caret;
                redo.Clear();
                removeCaret();
                document.Insert(text, fromP.Line, fromP.LinePos, UpdateAction.Undoable, true);
            }
        }

        /// <summary>
        /// Removes the caret if there currently is any.
        /// </summary>
        public void RemoveCaret()
        {
            Caret = null;
        }

        /// <summary>
        /// Deletes the text contained between the given positions.
        /// </summary>
        /// <param name="from">Start position of deletion.</param>
        /// <param name="to">End position of deletion.</param>
        public void Delete(IPosition from, IPosition to)
        {
            if (from != null && to != null)
            {
                var ngF = from as Position<TLine, TToken>;
                var ngT = to as Position<TLine, TToken>;
                if (ngF != null && ngT != null)
                {
                    removeSelection();
                    removeCaret();
                    redo.Clear();
                    if (!ngF.Line.IsVisible) scrollToLine(ngF.Line);
                    document.Delete(ngF, ngT, UpdateAction.Undoable, true);
                }
            }
        }

        /// <summary>
        /// Deletes the given amount of characters. Passing a negative length as parameter will try
        /// to delete backwards.
        /// </summary>
        /// <param name="start">Start position of deletion.</param>
        /// <param name="length">Number of chars to delete.</param>
        public void Delete(IPosition start, int length)
        {
            if (start != null && start.ILine != null && length != 0)
            {
                var ngS = start as Position<TLine, TToken>;
                if (ngS != null)
                {
                    removeSelection();
                    removeCaret();
                    redo.Clear();
                    if (!ngS.Line.IsVisible) scrollToLine(ngS.Line);
                    if (length > 0) document.Delete(ngS.Line, ngS.LinePos, length, UpdateAction.Undoable, true);
                    else
                    {
                        Position<TLine, TToken> newStart = Pos(start.LinePos + length, (TLine) start.ILine);
                        document.Delete(newStart, ngS, UpdateAction.Undoable, true);
                    }
                }
            }
        }

        /// <summary>
        /// Replaces the text between the given positions with the given string.
        /// </summary>
        /// <param name="from">Start position of replacing.</param>
        /// <param name="to">End position of replacing.</param>
        /// <param name="text">String to replace with.</param>
        public void Replace(IPosition from, IPosition to, string text)
        {
            if (from != null && to != null && text != null)
            {
                var ngF = from as Position<TLine, TToken>;
                var ngT = to as Position<TLine, TToken>;
                if (ngF != null && ngT != null)
                {
                    redo.Clear();
                    removeCaret();
                    removeSelection();
                    if (!ngF.Line.IsVisible) scrollToLine(ngF.Line);
                    document.Replace(ngF, ngT, text, UpdateAction.Undoable, true);
                }
            }
        }

        /// <summary>
        /// Ensures that the line containing the current caret position is visible.
        /// </summary>
        public void ScrollToCaret()
        {
            if (caret != null)
            {
                if (!caret.Line.IsVisible)
                {
                    scrollToLine(caret.Line);
                }
            }
        }

        /// <summary>
        /// Scrolls to the line at the given index if it is currently not visible
        /// </summary>
        /// <param name="index">Index of the line to scroll to</param>
        public void ScrollToLine(int index)
        {
            scrollToLine(index);
        }

        /// <summary>
        /// Scrolls to the given line if it is currently not visible
        /// </summary>
        /// <param name="line">Line to scroll to</param>
        public void ScrollToLine(ILine line)
        {
            var l = line as TLine;
            if (l != null) scrollToLine(l);
        }

        /// <summary>
        /// Selects the text between the given positions.
        /// </summary>
        /// <param name="from">Start position of the text to select.</param>
        /// <param name="to">End position of the text to select.</param>
        public void Select(IPosition from, IPosition to)
        {
            if (from == null || to == null || from.ILine == null || to.ILine == null) return;
            var ngS = from as Position<TLine, TToken>;
            var ngE = to as Position<TLine, TToken>;
            if (ngS != null && ngE != null && ngS.Line != null && ngE.Line != null)
            {
                if (!suspendPaint) setSelection(ngS, ngE);
                else selection = new Selection<TLine, TToken>(ngS, ngE);
            }
        }

        /// <summary>
        /// Selects the given amount of postions following the given start position.
        /// </summary>
        /// <param name="start">Start position of the text to select.</param>
        /// <param name="length">Number of positions to select.</param>
        public void Select(IPosition start, int length)
        {
            if (start != null && start.ILine != null)
            {
                var ngS = start as Position<TLine, TToken>;
                //if (start is Position<TLine, TToken>) ngS = (Position<TLine, TToken>)start;
                if (ngS != null)
                {
                    Position<TLine, TToken> toP = document.GetRelativePos(ngS.Line, ngS.LinePos, length);
                    if (toP != null)
                    {
                        if (!suspendPaint) setSelection(ngS, toP);
                        else selection = new Selection<TLine, TToken>(ngS, toP);
                    }
                }
            }
        }

        /// <summary>
        /// Selects all text in the text box. 
        /// </summary>
        public void SelectAll()
        {
            Position<TLine, TToken> fromP = Pos(0, document.First);
            Position<TLine, TToken> toP = Pos(document.Last.LastPos, document.Last);
            if (fromP != null && toP != null)
            {
                setSelection(fromP, toP);
            }
        }

        /// <summary>
        /// Ensures that no text is selected.
        /// </summary>
        public void DeselectAll()
        {
            removeSelection();
        }

        /// <summary>
        /// Searches the text in a ColorTextBox control for a string.
        /// NOTE: Since this method doesn't define a start or end point for searching it will always search from
        /// the document start down to the document end position returning the first encountered string position.
        /// </summary>
        /// <param name="search">String to search for.</param>
        /// <returns>Start position of the first occurence of the search string.</returns>
        public IPosition Search(String search)
        {
            var finds = ColorTextBoxFinds.None;
            return Search(search, finds);
        }

        /// <summary>
        /// Searches the text in a ColorTextBox control for a string with specific options applied to the search.
        /// NOTE: Since this method doesn't define a start or end point for searching it will always search from 
        /// the first down to the last document position or vice versa, depending on the Reverse flag in the 
        /// ColorTextBoxFinds enumeration.
        /// </summary>
        /// <param name="search">The string to search for</param>
        /// <param name="finds">A bitwise combination of the ColorTextBoxFinds values.</param>
        /// <returns>Start position of the first occurence of the search string.</returns>
        public IPosition Search(String search, ColorTextBoxFinds finds)
        {
            if ((finds & ColorTextBoxFinds.Reverse) == ColorTextBoxFinds.Reverse)
            {
                return Search(search, Pos(document.Last.LastPos, document.Last), finds);
            }
            else
            {
                return Search(search, Pos(0, FirstLine), finds);
            }
        }

        /// <summary>
        /// Searches the text in a ColorTextBox control for a string at a specific location within the control 
        /// and with specific options applied to the search.
        /// NOTE: Since this method doesn't define an end point for the search this method will always search from
        /// the passed start position down to the last document position or up to the first one depending on the 
        /// Reverse flag in the ColorTextBoxFinds enumeration.
        /// </summary>
        /// <param name="search">The string to search for.</param>
        /// <param name="start">The location within the control's text at which to begin searching.</param>
        /// <param name="finds">A bitwise combination of the ColorTextBoxFinds values.</param>
        /// <returns>Start position of the first occurence of the search string.</returns>
        public IPosition Search(String search, IPosition start, ColorTextBoxFinds finds)
        {
            if ((finds & ColorTextBoxFinds.Reverse) == ColorTextBoxFinds.Reverse)
            {
                return Search(search, Pos(0, FirstLine), start, finds);
            }
            else
            {
                return Search(search, start, Pos(document.Last.LastPos, document.Last), finds);
            }
        }

        /// <summary>
        /// Searches the text in a ColorTextBox control for a given string between the given positions.
        /// NOTE: If the Reverse flag in the ColorTextBoxFinds enumeration is set to false the text will be searched
        /// beginning at "start" down to "end", otherwise the search will begin at "end" going up to "start".
        /// So it is important that the end position is always contained after the start position!
        /// </summary>
        /// <param name="search">The string to search for.</param>
        /// <param name="start">Start position of search.</param>
        /// <param name="end">End position of search.</param>
        /// <param name="finds">A bitwise combination of the ColorTextBoxFinds values.</param>
        /// <returns>Start position of the first occurence of the search string or null if nothing was found.</returns>
        public IPosition Search(String search, IPosition start, IPosition end, ColorTextBoxFinds finds)
        {
            if (start == null || end == null || start.ILine == null || end.ILine == null) return null;
            var ngS = start as Position<TLine, TToken>;
            var ngE = end as Position<TLine, TToken>;
            if (ngS != null && ngE != null && ngS.Line != null && ngE.Line != null)
            {
                bool backwards = ((finds & ColorTextBoxFinds.Reverse) == ColorTextBoxFinds.Reverse);
                if (ngE > ngS)
                {
                    Position<TLine, TToken> found = null;
                    if (backwards)
                    {
                        found = document.Find(search, ngE, finds);
                        if (found != null && found > ngS) return found;
                    }
                    else
                    {
                        found = document.Find(search, ngS, finds);
                        if (found != null && found < ngE) return found;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// This non-generic method returns the number of the passed line inside the document iff it is
        /// of the correct type for this ColorTextBox implementation!
        /// If the passed line has the wrong type or the line isn't contained the method returns -1.
        /// NOTE: This methods runtime complexity is linear (O(n) = n) so it should be used with care when
        /// a ColorTextBox is used to hold long texts!
        /// </summary>
        /// <param name="line">Line for which to get the number.</param>
        /// <returns>The number of the given line or -1 if it is not contained.</returns>
        public int GetLineNumber(ILine line)
        {
            if (line != null && line is TLine)
            {
                return document.IndexOf((TLine) line);
            }
            return -1;
        }

        /// <summary>
        /// Returns the position at the specified line index and line position. If there is no line at the
        /// given index the method returns null. If linePos is not part of the given line (if it is negative
        /// or greater than the last position of the line) the respective following or previous line that 
        /// contains the position will be returned. The returned position may not be visible!
        /// NOTE: This methods runtime complexity is linear (O(n) = n) so it should be used with care when
        /// a ColorTextBox is used to hold long texts!
        /// </summary>
        /// <param name="lineIndex">Index of the line containing the position.</param>
        /// <param name="linePos">Position inside the given line.</param>
        /// <returns>The Position object for the specified line and line position.</returns>
        public IPosition GetPosition(int lineIndex, int linePos)
        {
            TLine line = GetLineAt(lineIndex);
            if (line != null)
            {
                return Pos(linePos, line);
            }
            return null;
        }

        /// <summary>
        /// Returns the Position that is nearest to the given Point. If there are currently no
        /// visible lines in the control the method returns null!
        /// NOTE: If a position is found it is guaranteed to be visible!
        /// </summary>
        /// <param name="p">The Point for which to get the position.</param>
        /// <returns></returns>
        public IPosition GetPosition(Point p)
        {
            return Pos(p.X, p.Y);
        }

        /// <summary>
        /// Sets the render mode to GDI if true is passed as parameter - otherwise GDI+ rendering will be used
        /// </summary>
        /// <param name="useGDI">True sets rendering to GDI. False to GDI+</param>
        public void SetRenderMode(RenderMode m)
        {
            if (renderMode == m) return;
            disposeGraphics();
            renderMode = m;
            initGraphics();
            TLine search = document.First;
            while (search != null)
            {
                search.Modified = true;
                search = search.Next;
            }
            calcLinePositions(true, true);
        }

        /***** PROPERTIES: *****/

        /// <summary>
        /// Gets or sets the file name associated to the current ColorTextBox instance.
        /// </summary>
        [Browsable(false)]
        [DefaultValue(null)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Gets or sets the file name associated to the current ColorTextBox instance.")]
        public String FileName
        {
            get
            {
                if (fileName == null) return String.Empty;
                return fileName;
            }
            set { fileName = value; }
        }

        /// <summary>
        /// Gets or sets the full path of the file associated to the current ColorTextBox instance.
        /// </summary>
        [Browsable(false)]
        [DefaultValue(null)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Gets or sets the full path of the file associated to the current ColorTextBox instance.")]
        public String FilePath
        {
            get
            {
                if (filePath == null) return String.Empty;
                return filePath;
            }
            set { filePath = value; }
        }

        /// <summary>
        /// Gets or sets the document contained in the ColorTextBox. (Represents the data model)
        /// </summary>
        [Browsable(false)]
        [DefaultValue(null)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Gets or sets the document contained in the ColorTextBox. (Represents the data model)")]
        public IDocument Document
        {
            get { return document; }
            set
            {
                var newDoc = value as TDocument;
                document.UpdateEvent -= handleUpdateEvent;
                document.Dispose();
                if (newDoc == null) document = new TDocument();
                else document = newDoc;
                document.CharacterCasing = casing;
                document.UpdateEvent += handleUpdateEvent;
                initNewContent();
            }
        }

        /// <summary>
        /// Gets or sets the Text contained in the ColorTextBox. When setting the Text the constructed lines
        /// will only consist of 2 tokens: one containing the text of the line and a newline token.
        /// </summary>
        [Browsable(true)]
        [DefaultValue(null)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Description(
            "Gets or sets the Text contained in the ColorTextBox. When setting the Text the constructed lines will only consist of 2 tokens: one containing the text of the line and a newline token."
            )]
        public override String Text
        {
            get { return document.Text; }
            set
            {
                if (value != null)
                {
                    document.Text = value;
                }
                else document.Text = String.Empty;
            }
        }

        /// <summary>
        /// Gets or sets the lines in the ColorTextBox (without newline 
        /// characters at the end of each string!)
        /// </summary>
        [Browsable(true)]
        [DefaultValue(new String[0])]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Description(
            "Gets or sets the lines in the ColorTextBox (without newline characters at the end of each string!)")]
        public String[] Lines
        {
            get { return document.Lines; }
            set
            {
                if (value != null) document.Lines = value;
                else document.Lines = new String[0];
            }
        }

        /// <summary>
        /// Gets or sets the current caret position. Sets the caret to the specified position and ensures 
        /// that the caret is visible by scrolling to the new position. If a null reference is passed as 
        /// position the caret will be removed!
        /// Changing the caret position causes any current selection to be removed!
        /// </summary>
        [Browsable(false)]
        [DefaultValue(null)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description(
            "Gets or sets the current caret position. Sets the caret to the specified position and ensures  that the caret is visible by scrolling to the new position. If a null reference is passed as position the caret will be removed! Changing the caret position causes any current selection to be removed!"
            )]
        public IPosition Caret
        {
            get { return caret; }
            set
            {
                if (value != null && value.ILine != null)
                {
                    var caretPos = value as Position<TLine, TToken>;
                    if (caretPos != null)
                    {
                        if (!suspendPaint)
                        {
                            removeSelection();
                            setCaret(caretPos);
                        }
                        else
                        {
                            selection = null;
                            caret = caretPos;
                        }
                    }
                }
                else
                {
                    var cArgs = new CaretEventArgs(caret, null);
                    if (!suspendPaint) removeCaret();
                    else caret = null;
                    OnCaretChange(cArgs);
                }
            }
        }

        /// <summary>
        /// Returns true if there is currently a caret set in the ColorTextBox.
        /// </summary>
        [Browsable(false)]
        [Description("Returns true if there is currently a caret set in the ColorTextBox.")]
        public bool IsCaretSet
        {
            get { return (caret == null); }
        }

        /// <summary>
        /// Gets the current selection start position. If there currently is no selection a null reference 
        /// is returned.
        /// </summary>
        [Browsable(false)]
        [Description(
            "Gets the current selection start position. If there currently is no selection a null reference is returned."
            )]
        public IPosition SelectionFromPos
        {
            get
            {
                if (selection != null && selection.From != null && selection.To != null)
                {
                    return selection.From;
                }
                return null;
            }
        }

        /// <summary>
        /// Gets the current selection end position. If there currently is no selection a null reference is
        /// returned.
        /// </summary>
        [Browsable(false)]
        [Description(
            "Gets the current selection end position. If there currently is no selection a null reference is returned.")
        ]
        public IPosition SelectionToPos
        {
            get
            {
                if (selection != null && selection.From != null && selection.To != null)
                {
                    return selection.To;
                }
                return null;
            }
        }

        /// <summary>
        /// Returns true if there currently exists a selection.
        /// </summary>
        [Browsable(false)]
        [Description("Returns true if there currently exists a selection.")]
        public bool IsTextSelected
        {
            get
            {
                if (selection != null && selection.From != null && selection.To != null) return true;
                return false;
            }
        }

        /// <summary>
        /// Gets or sets the selected text within the text box.
        /// </summary>
        [Browsable(false)]
        [DefaultValue(null)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Gets or sets the selected text within the text box.")]
        public String SelectedText
        {
            get
            {
                if (selection != null)
                {
                    return document.SubString(selection.From, selection.To);
                }
                else return String.Empty;
            }
            set
            {
                if (value != null)
                {
                    Position<TLine, TToken> fromP = caret;
                    Position<TLine, TToken> toP = null;
                    if (selection != null)
                    {
                        fromP = selection.From;
                        toP = selection.To;
                    }
                    if (fromP != null)
                    {
                        if (!caret.Line.IsVisible) scrollToLine(caret.Line);
                        removeCaret();
                        removeSelection();
                        if (toP != null)
                        {
                            document.Replace(fromP, toP, value, UpdateAction.Undoable, true);
                        }
                        else
                        {
                            document.Insert(value, fromP.Line, fromP.LinePos, UpdateAction.Undoable, true);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets the text in the text box as an rtf formatted string.
        /// </summary>
        [Browsable(false)]
        [Description("Gets the text in the text box as an rtf formatted string.")]
        public String Rtf
        {
            get { return convertToRtf(FirstLine); }
        }

        /// <summary>
        /// Gets the currently selected text in the text box as an rtf formatted string.
        /// </summary>
        [Browsable(false)]
        [Description("Gets the currently selected text in the text box as an rtf formatted string.")]
        public String SelectedRtf
        {
            get
            {
                if (selection != null)
                {
                    TLine selL = document.SubLines(selection.From, selection.To);
                    return convertToRtf(selL);
                }
                else return String.Empty;
            }
        }

        /// <summary>
        /// Gets or sets the text color of the current text selection or insertion point. If there is a 
        /// selection this property always returns the color at the end of the selection otherwise the
        /// color at the current caret position is returned!
        /// </summary>
        [Browsable(true)]
        [Description(
            "Gets or sets the text color of the current text selection or insertion point. If there is a selection this property always returns the color at the end of the selection otherwise the color at the current caret position is returned!"
            )]
        public Color SelectionColor
        {
            get
            {
                if (selection != null)
                {
                    return document.ColorAt(selection.To);
                }
                if (caret != null)
                {
                    return document.ColorAt(caret);
                }
                return document.Color;
            }
            set
            {
                if (value != null)
                {
                    if (selection != null)
                    {
                        document.SetColor(value, selection.From, selection.To, UpdateAction.Undoable, true);
                        redo.Clear();
                    }
                    else
                    {
                        document.Color = value;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the currently active color to be used when typing text.
        /// </summary>
        [Browsable(true)]
        [Description("Gets or sets the currently active color to be used when typing text.")]
        public override Color ForeColor
        {
            get { return document.Color; }
            set { if (value != null) document.Color = value; }
        }

        /// <summary>
        /// Gets or sets the color of the caret.
        /// </summary>
        [Browsable(true)]
        [Description("Gets or sets the color of the caret.")]
        public Color CaretColor
        {
            get { return caretColor; }
            set { caretColor = value; }
        }

        /// <summary>
        /// Gets or sets the color of the text used for line number display.
        /// </summary>
        [Browsable(true)]
        [Description("Gets or sets the color of the text used for line number display.")]
        public Color LineNumForeColor
        {
            get { return lnTextColor; }
            set
            {
                lnTextColor = value;
                drawReservedArea(ReservedLocation.Left, LN_RESAREA_ID, true);
            }
        }

        /// <summary>
        /// Gets or sets the background color of the area used for line number display.
        /// </summary>
        [Browsable(true)]
        [Description("Gets or sets the background color of the area used for line number display.")]
        public Color LineNumBackColor
        {
            get { return lnBackColor; }
            set
            {
                lnBackColor = value;
                drawReservedArea(ReservedLocation.Left, LN_RESAREA_ID, true);
            }
        }

        /// <summary>
        /// Gets or sets the color of the text used for caret info display.
        /// </summary>
        [Browsable(true)]
        [Description("Gets or sets the color of the text used for caret info display.")]
        public Color CaretInfoForeColor
        {
            get { return ciTextColor; }
            set
            {
                ciTextColor = value;
                drawReservedArea(ReservedLocation.Bottom, CI_RESAREA_ID, true);
            }
        }

        /// <summary>
        /// Gets or sets the background color of the area used for caret info display.
        /// </summary>
        [Browsable(true)]
        [Description("Gets or sets the background color of the area used for caret info display.")]
        public Color CaretInfoBackColor
        {
            get { return ciBackColor; }
            set
            {
                ciBackColor = value;
                drawReservedArea(ReservedLocation.Bottom, CI_RESAREA_ID, true);
            }
        }

        /// <summary>
        /// Gets or sets the font that is used to display the line numbers. Setting the font to a font larger
        /// than the one currently used in the text box causes the height of the new font to be reset to the 
        /// height value of the text box's font.
        /// </summary>
        [Browsable(true)]
        [Description("Gets or sets the font that is used to display the line numbers.")]
        public Font LineNumFont
        {
            get
            {
                if (lnFont == null)
                {
                    lnFont = Font;
                }
                return lnFont;
            }
            set
            {
                lnFont = value;
                if (lnFont.SizeInPoints > Font.SizeInPoints)
                {
                    lnFont = new Font(value.FontFamily, Font.SizeInPoints, value.Style);
                }
                Graphics g = getGraphics(null);
                int newW = measureString(g, LineCount.ToString(), lnFont).Width + RES_BORDER_WIDTH;
                changeResLength(newW, ReservedLocation.Left, LN_RESAREA_ID);
                calcLinePositions(true, true);
            }
        }

        /// <summary>
        /// Gets or sets the font to be used for drawing caret position info.
        /// </summary>
        [Browsable(true)]
        [Description("Gets or sets the font to be used for drawing caret position info.")]
        public Font CaretInfoFont
        {
            get { return ciFont; }
            set
            {
                ciFont = value;
                Graphics g = getGraphics(null);
                int newH = measureString(g, "M", ciFont).Height + RES_BORDER_WIDTH;
                changeResLength(newH, ReservedLocation.Bottom, CI_RESAREA_ID);
                calcLinePositions(true, true);
            }
        }

        /// <summary>
        /// Returns the length of text contained in the ColorTextBox.
        /// </summary>
        [Browsable(false)]
        [Description("Returns the length of the text contained in the ColorTextBox.")]
        public int TextLength
        {
            get
            {
                int len = 0;
                if (document.First != null)
                {
                    TLine curr = document.First;
                    while (curr != null)
                    {
                        len += curr.Length;
                        curr = curr.Next;
                    }
                }
                return len;
            }
        }

        /// <summary>
        /// Returns the number of lines contained in the ColorTextBox.
        /// </summary>
        [Browsable(false)]
        [Description("Returns the number of lines contained in the ColorTextBox.")]
        public int LineCount
        {
            get { return document.LineCount; }
        }

        /// <summary>
        /// Returns the line number of the first visible line.
        /// </summary>
        [Browsable(false)]
        [Description("Returns the line number of the first visible line.")]
        public virtual int FirstVisLineNumber
        {
            get { return VScroll.Value; //document.IndexOf(firstVisLine);
            }
        }

        /// <summary>
        /// Returns the DataFormat String of a ColorTextBox instance used to store and retrieve data from 
        /// the Clipboard in an application specific format.
        /// </summary>
        [Browsable(false)]
        [Description(
            "Returns the DataFormat String of a ColorTextBox instance used to store and retrieve data from the Clipboard in an application specific format."
            )]
        public virtual String ColorTextBoxFormat
        {
            get { return ctbFormat.Name; }
        }

        /// <summary>
        /// Gets or sets a value that indicates that the text box control has been modified by the user 
        /// since the control was created or its contents were last set. 
        /// </summary>
        [Browsable(false)]
        [Description(
            "Gets or sets a value that indicates that the text box control has been modified by the user since the control was created or its contents were last set."
            )]
        public bool Modified
        {
            get { return modified; }
            set { modified = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether text in the text box is read-only. Read-only means that
        /// the content of the text box can only be modified programmatically, i.e. the control does not react
        /// to user input that would change the content!
        /// </summary>
        [Browsable(true)]
        [DefaultValue(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Description("Gets or sets a value indicating whether text in the text box is read-only.")]
        public bool ReadOnly
        {
            get { return isReadOnly; }
            set { isReadOnly = value; }
        }

        /// <summary>
        /// Turns rendering of line numbers on the left edge of the control on or off.
        /// </summary>
        [Browsable(true)]
        [DefaultValue(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Description("Turns rendering of line numbers on the left edge of the control on or off.")]
        public bool ShowLineNumbers
        {
            get { return showLineNums; }
            set
            {
                if (value)
                {
                    if (!showLineNums)
                    {
                        showLineNums = true;
                        int maxLNW = measureString(getGraphics(null), LineCount.ToString(), Font).Width;
                        addResArea(LN_MARGIN_ID, reservedAreas[(int) ReservedLocation.Left].Count, margin.Left,
                                   ReservedLocation.Left, drawMargin, null, Cursors.Default);
                        addResArea(ColorTextBox.LN_RESAREA_ID, reservedAreas[(int) ReservedLocation.Left].Count - 2,
                                   maxLNW + RES_BORDER_WIDTH, ReservedLocation.Left, drawLineNumbers, handleLNumMouseEvt,
                                   Cursors.Default);
                    }
                }
                else
                {
                    if (showLineNums)
                    {
                        showLineNums = false;
                        removeResArea(ReservedLocation.Left, LN_MARGIN_ID);
                        removeResArea(ReservedLocation.Left, ColorTextBox.LN_RESAREA_ID);
                    }
                }
                calcLinePositions(true, true);
            }
        }

        /// <summary>
        /// Turns rendering of caret position information area at the bottom of the control on or off.
        /// </summary>
        [Browsable(true)]
        [DefaultValue(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Description("Turns rendering of caret position information area at the bottom of the control on or off.")]
        public bool ShowCaretInfo
        {
            get { return showCaretInfo; }
            set
            {
                if (value)
                {
                    if (!showCaretInfo)
                    {
                        showCaretInfo = true;
                        int cInfoH = measureString(getGraphics(null), "M", ciFont).Height + RES_BORDER_WIDTH;
                        addResArea(CI_MARGIN_ID, 1, margin.Bottom, ReservedLocation.Bottom, drawMargin, null,
                                   Cursors.Default);
                        addResArea(CI_RESAREA_ID, reservedAreas[(int) ReservedLocation.Bottom].Count - 2, cInfoH,
                                   ReservedLocation.Bottom, drawCaretInfo, null, Cursors.Default);
                    }
                }
                else
                {
                    if (showCaretInfo)
                    {
                        showCaretInfo = false;
                        removeResArea(ReservedLocation.Bottom, CI_MARGIN_ID);
                        removeResArea(ReservedLocation.Bottom, CI_RESAREA_ID);
                    }
                }
                calcLinePositions(true, true);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the user can undo the previous operation in a text box control.
        /// </summary>
        [Browsable(false)]
        [Description("Gets a value indicating whether the user can undo the previous operation in a text box control.")]
        public bool CanUndo
        {
            get { return (undo != null && undo.Count > 0); }
        }

        /// <summary>
        /// Gets a value indicating whether there are actions that have occurred within the text box that 
        /// can be reapplied.
        /// </summary>
        [Browsable(false)]
        [Description(
            "Gets a value indicating whether there are actions that have occurred within the text box that can be reapplied."
            )]
        public bool CanRedo
        {
            get { return (redo != null && redo.Count > 0); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether control's elements are aligned to support locales using 
        /// right-to-left fonts. This property is not supported by ColorTextBox and therefore changing it has no
        /// effect.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(0)]
        [Description("This property is not supported by the ColorTextBox control")]
        public override RightToLeft RightToLeft
        {
            get { return base.RightToLeft; }
            set { base.RightToLeft = RightToLeft.No; }
        }

        /// <summary>
        /// Gets or sets the additional margins between the edge of the TextArea and the actual lines.
        /// </summary>
        [Browsable(true)]
        [DefaultValue(typeof (Padding), "2")]
        [Description("Gets or sets the additional margins between the edge of the TextArea and the actual lines.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Localizable(true)]
        public new Padding Padding
        {
            get { return padding; }
            set
            {
                if (value != null)
                {
                    padding = value;
                    textArea = Rectangle.Empty;
                    invalidatePositions();
                    calcLinePositions(true, true);
                }
            }
        }

        /// <summary>
        /// Gets or sets the size that is the lower limit that System.Windows.Forms.Control.GetPreferredSize(System.Drawing.Size)
        /// can specify.
        /// </summary>
        [Browsable(true)]
        [DefaultValue(typeof (Size), "0, 0")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Description(
            "Gets or sets the size that is the lower limit that System.Windows.Forms.Control.GetPreferredSize(System.Drawing.Size) can specify."
            )]
        public override Size MinimumSize
        {
            get
            {
                Size retSize = base.MinimumSize;
                retSize.Width -= (ReservedLeftWidths + ReservedRightWidths);
                retSize.Height -= (ReservedTopWidths + ReservedBottomWidths + LineHeight);
                return retSize;
            }
            set
            {
                value.Width += ReservedLeftWidths + ReservedRightWidths;
                value.Height += ReservedTopWidths + ReservedBottomWidths + LineHeight;
                base.MinimumSize = value;
            }
        }

        /// <summary>
        /// Returns the Rectangle on which lines can be drawn.
        /// </summary>
        [Browsable(false)]
        [Description("Returns the Rectangle on which lines can be drawn.")]
        public Rectangle TextArea
        {
            get
            {
                if (textArea == Rectangle.Empty)
                {
                    int x, y, w, h;
                    x = ClientScrollRectangle.X + ReservedLeftWidths + padding.Left;
                    y = ClientScrollRectangle.Y + ReservedTopWidths + padding.Top;
                    h = ClientScrollRectangle.Height -
                        (ReservedBottomWidths + ReservedTopWidths + padding.Top + padding.Bottom);
                    w = ClientScrollRectangle.Width -
                        (ReservedLeftWidths + ReservedRightWidths + padding.Left + padding.Right);
                    var ret = new Rectangle(x, y, w, h);
                    textArea = ret;
                    return ret;
                }
                else return textArea;
            }
        }

        /// <summary>
        /// If there are scrollbars visible in the ColorTextBox this property returns the ClientRectangle without
        /// the width or height of the scrollbars. If there are no scrollbars visible the returned Rectangle is
        /// equal to the Rectangle returned by the ClientRectangle property.
        /// </summary>
        [Browsable(false)]
        [Description(
            "If there are scrollbars visible in the ColorTextBox this property returns the ClientRectangle without the width or height of the scrollbars."
            )]
        public Rectangle ClientScrollRectangle
        {
            get
            {
                Rectangle cR = ClientRectangle;
                if (VScroll.Visible) cR.Width -= VScroll.Width;
                if (HScroll.Visible) cR.Height -= HScroll.Height;
                if (cR.Width <= 0) cR.Width = 1;
                if (cR.Height <= 0) cR.Height = 1;
                return cR;
            }
        }

        /// <summary>
        /// Gets the current line height for the ColorTextBox (depending on set font)
        /// </summary>
        [Browsable(false)]
        [Description("Gets the current line height for the ColorTextBox (depending on set font)")]
        public int LineHeight
        {
            get
            {
                if (lineHeight <= 0)
                {
                    lineHeight = measureString(getGraphics(null), "M", Font).Height;
                }
                return lineHeight;
            }
        }

        /// <summary>
        /// Returns the distance from the top of a line to the baseline.
        /// </summary>
        [Browsable(false)]
        [Description("Returns the distance from the top of a line to the baseline.")]
        public virtual int BaseLineDistance
        {
            get
            {
                if (lineBaseDist <= 0)
                {
                    lineBaseDist = calcBaseDistance(Font);
                }
                return lineBaseDist;
            }
        }

        /// <summary>
        /// Returns a Region that contains all reserved areas (useful for repainting,...)
        /// </summary>
        [Browsable(false)]
        [Description("Returns a Region that contains all reserved areas (useful for repainting,...)")]
        public Region ReservedAreas
        {
            get
            {
                var r = new Region(Rectangle.Empty);
                r.Union(new Rectangle(ClientScrollRectangle.X, ClientScrollRectangle.Y,
                                      ClientScrollRectangle.Width, ReservedTopWidths));
                r.Union(new Rectangle(ClientScrollRectangle.X, ClientScrollRectangle.Bottom - ReservedBottomWidths,
                                      ClientScrollRectangle.Width, ReservedBottomWidths));
                r.Union(new Rectangle(ClientScrollRectangle.X, ClientScrollRectangle.Y, ReservedLeftWidths,
                                      ClientScrollRectangle.Height));
                r.Union(new Rectangle(ClientScrollRectangle.Right - ReservedRightWidths, ClientScrollRectangle.Y,
                                      ReservedRightWidths, ClientScrollRectangle.Height));
                return r;
            }
        }

        /// <summary>
        /// Gets the sum of all reserved widths on the left side of the client area.
        /// </summary>
        [Browsable(false)]
        [Description("Gets the sum of all reserved widths on the left side of the client area.")]
        public int ReservedLeftWidths
        {
            get
            {
                if (leftWidths == -1)
                {
                    leftWidths = 0;
                    foreach (ReservedArea a in reservedAreas[(int) ReservedLocation.Left])
                    {
                        leftWidths += a.Width;
                    }
                }
                return leftWidths;
            }
        }

        /// <summary>
        /// Gets the sum of all reserved widths on the right side of the client area.
        /// </summary>
        [Browsable(false)]
        [Description("Gets the sum of all reserved widths on the right side of the client area.")]
        public int ReservedRightWidths
        {
            get
            {
                if (rightWidths == -1)
                {
                    rightWidths = 0;
                    foreach (ReservedArea a in reservedAreas[(int) ReservedLocation.Right])
                    {
                        rightWidths += a.Width;
                    }
                }
                return rightWidths;
            }
        }

        /// <summary>
        /// Gets the sum of all reserved heights at the top of the client area.
        /// </summary>
        [Browsable(false)]
        [Description("Gets the sum of all reserved heights at the top of the client area.")]
        public int ReservedTopWidths
        {
            get
            {
                if (topWidths == -1)
                {
                    topWidths = 0;
                    foreach (ReservedArea a in reservedAreas[(int) ReservedLocation.Top])
                    {
                        topWidths += a.Width;
                    }
                }
                return topWidths;
            }
        }

        /// <summary>
        /// Gets the sum of all reserved heights at the bottom of the client area.
        /// </summary>
        [Browsable(false)]
        [Description("Gets the sum of all reserved heights at the bottom of the client area.")]
        public int ReservedBottomWidths
        {
            get
            {
                if (bottomWidths == -1)
                {
                    bottomWidths = 0;
                    foreach (ReservedArea a in reservedAreas[(int) ReservedLocation.Bottom])
                    {
                        bottomWidths += a.Width;
                    }
                }
                return bottomWidths;
            }
        }

        #endregion

        protected static RichTextBox getRTB()
        {
            if (rtb == null)
            {
                rtb = new RichTextBox();
                rtb.Visible = false;
                rtb.Enabled = false;
            }
            return rtb;
        }

        /// <summary>
        /// Destructor for a ColorTextBox that disposes all resources in use.
        /// </summary>
        ~ColorTextBoxBase()
        {
            disposeGraphics();
        }

        /***** PROTECTED METHODS: *****/

        /// <summary>
        /// Called from the set methods for the content of the ColorTextBox. Initializes all fields and data
        /// structures necessary for drawing.
        /// </summary>
        protected virtual void initNewContent()
        {
            firstVisLine = document.First;
            modified = false;
            textArea = Rectangle.Empty;
            leftWidths = -1;
            topWidths = -1;
            rightWidths = -1;
            bottomWidths = -1;
            if (grafx != null) disposeGraphics();
            initGraphics();
            undo = new ArrayList();
            redo = new ArrayList();
            VScroll.SmallChange = 1;
            VScroll.Minimum = 1;
            VScroll.Maximum = LineCount; //+ TextArea.Height / LineHeight;
            VScroll.LargeChange = TextArea.Height/LineHeight;
            VScroll.Value = VScroll.Minimum;
            HScroll.Minimum = 0;
            HScroll.Maximum = maxWidth;
            HScroll.SmallChange = 5;
            HScroll.LargeChange = TextArea.Width;
            HScroll.Value = HScroll.Minimum;
            if (showLineNums)
            {
                int maxLNW = measureString(getGraphics(null), LineCount.ToString(), lnFont).Width;
                removeResArea(ReservedLocation.Left, LN_RESAREA_ID);
                addResArea(LN_RESAREA_ID, reservedAreas[(int) ReservedLocation.Left].Count - 2,
                           maxLNW + RES_BORDER_WIDTH, ReservedLocation.Left,
                           drawLineNumbers, handleLNumMouseEvt, Cursors.Default);
            }
            if (showCaretInfo)
            {
                int cInfoH = measureString(getGraphics(null), "M", ciFont).Height + RES_BORDER_WIDTH;
                removeResArea(ReservedLocation.Bottom, CI_RESAREA_ID);
                addResArea(CI_RESAREA_ID, reservedAreas[(int) ReservedLocation.Bottom].Count - 2, cInfoH,
                           ReservedLocation.Bottom, drawCaretInfo, null, Cursors.Default);
            }
            calcLinePositions(true, true);
            setCaret(Pos(0, FirstLine));
            //this.Refresh();
        }

        /***** Position handling: *****/

        /// <summary>
        /// Gets the line position nearest to the given x and y coordinates. If there are currently no visible
        /// lines the method returns null. This implementation calculates the line position based on the font
        /// set for the ColorTextBox control - override if you want to use token-based fonts.
        /// </summary>
        /// <param name="x">X position.</param>
        /// <param name="y">Y position.</param>
        /// <returns>The Position nearest to the given coordinates. Null if there are no visible lines.</returns>
        protected virtual Position<TLine, TToken> Pos(int x, int y)
        {
            var pos = new Position<TLine, TToken>();
            pos.IsValid = true;
            Graphics g = getGraphics(null);
            pos.Line = GetLineAtYPos(ref y);
            if (pos.Line != null)
            {
                if (pos.Line.Modified) calcLine(pos.Line, g);
                int lineWidth = pos.Line.Width;
                if (x >= pos.Line.X + lineWidth)
                {
                    pos.X = pos.Line.X + lineWidth;
                    pos.LinePos = pos.Line.LastPos;
                }
                else
                {
                    TToken t = pos.Line.First;
                    TToken prev = t;
                    pos.X = pos.Line.X;
                    int w = 0;
                    int lPos = 0;
                    pos.LinePos = 0;
                    if (t == null) return pos;
                    // find containing token:
                    while (t != null && x > pos.X + w)
                    {
                        pos.LinePos += lPos;
                        pos.X += w;
                        w = measureString(g, t.Value, Font).Width;
                        lPos = t.Length;
                        prev = t;
                        t = t.Next;
                    }
                    // find exact position inside token:
                    if (x < pos.X + w)
                    {
                        calcPosInToken(g, x, prev, ref pos);
                        /*int charWidth = stringSize(g, pos.Line.CharAt(pos.LinePos).ToString(), Font).Width;
                        while (x >= pos.X+charWidth) {
                            pos.X += charWidth;
                            pos.LinePos++;
                            charWidth = stringSize(g, pos.Line.CharAt(pos.LinePos).ToString(), Font).Width;
                        }*/
                    }
                }
                return pos;
            }
            return null;
        }

        /// <summary>
        /// Helper method that searches for the exact line and X position inside a given token. Uses
        /// a divide-and-conquer approach (binary search) so large tokens can be searched fast as well.
        /// </summary>
        /// <param name="g">Graphics object</param>
        /// <param name="x">X position to search for.</param>
        /// <param name="t">Token containing the position to search for.</param>
        /// <param name="pos">Reference to the search position - has to be set to the start position of
        /// the provided token!</param>
        private void calcPosInToken(Graphics g, int x, TToken t, ref Position<TLine, TToken> pos)
        {
            int sumLen = t.Length;
            int len = sumLen/2;
            int width = 0;
            while (len > 0)
            {
                width = measureString(g, pos.Line.SubString(pos.LinePos, len), Font).Width;
                if (x >= pos.X + width)
                {
                    pos.X += width;
                    pos.LinePos += len;
                    sumLen -= len;
                    len = sumLen/2;
                }
                else
                {
                    sumLen = len;
                    len = sumLen/2;
                }
            }
        }

        /// <summary>
        /// Gets the line position for the given char position inside the given line. If the passed line is
        /// null this method returns null as well. For tpos less than 0 or bigger than line.Length the method
        /// returns the corresponding position inside a previous or following line - if such a line doesn't exist 
        /// the position will be set to tpos = 0 or tpos = line.Length respectively.
        /// The method treats newline strings as a single character independent of the real length.
        /// This implementation calculates the line position based on the font set for the ColorTextBox 
        /// control - override if you want to use token-based fonts.
        /// </summary>
        /// <param name="tpos">Character position inside the line.</param>
        /// <param name="line">Line containing the returned Position.</param>
        /// <returns>The Position of the given tpos inside line. Null if line is null.</returns>
        protected virtual Position<TLine, TToken> Pos(int tpos, TLine line)
        {
            if (line != null)
            {
                var pos = new Position<TLine, TToken>();
                pos.IsValid = true;
                Graphics g = getGraphics(null);
                pos.Line = line;
                if (tpos < 0)
                {
                    if (line.Prev != null)
                    {
                        return Pos(line.Prev.LastPos + 1 + tpos, line.Prev);
                    }
                    else tpos = 0;
                }
                else
                {
                    int lastP = line.LastPos;
                    if (tpos > lastP)
                    {
                        if (line.Next != null)
                        {
                            return Pos(tpos - lastP - 1, line.Next);
                        }
                        else tpos = lastP;
                    }
                }
                String val = pos.Line.SubString(0, tpos);
                pos.X = measureString(g, val, Font).Width + TextArea.X;
                pos.LinePos = tpos;
                return pos;
            }
            return null;
        }

        /// <summary>
        /// This helper method sets the IsValid flag of each position (caret, selection) to false.
        /// This causes the X coordinates to be recalculated the next time any of these positions will be 
        /// rendered.
        /// Useful when changes to the left or right reserved areas are being made.
        /// </summary>
        protected void invalidatePositions()
        {
            textArea = Rectangle.Empty;
            if (selection != null && selection.From != null && selection.To != null)
            {
                selection.From.IsValid = false;
                selection.To.IsValid = false;
            }
            if (selectionAnchor != null) selectionAnchor.IsValid = false;
            if (caret != null) caret.IsValid = false;
        }

        /***** Caret handling: *****/

        /// <summary>
        /// Sets the caret to the given position. If the given position is out of the horizontal or vertical
        /// viewing range the view will be scrolled so that the caret is visible.
        /// </summary>
        /// <param name="pos">New caret position.</param>
        protected virtual void setCaret(Position<TLine, TToken> pos)
        {
            if (caret != null && caret.Equals(pos)) return;
            if (caret != null) removeCaret();
            var cArgs = new CaretEventArgs(oldCaret, pos);
            if (pos == null)
            {
                caret = null;
                OnCaretChange(cArgs);
                return;
            }
            if (caretPen.Color == caretColor) invertCaret();
            caretTimer.Stop();
            if (!pos.Line.IsVisible)
            {
                if (caret != null && caret.Line.IsVisible &&
                    !(caret.Line == firstVisLine && caret.Line == lastVisLine))
                {
                    if (pos.Line == firstVisLine.Prev) scrollUp(1);
                    else if (pos.Line == lastVisLine.Next) scrollDown(1);
                    else scrollToLine(pos.Line);
                }
                else scrollToLine(pos.Line);
            }
            bool redraw = false;
            int oldOffSet = xOffSet;
            // check if position is valid:
            if (!pos.IsValid) pos = Pos(pos.LinePos, pos.Line);
            // ensure horizontal visibility:
            if (pos.X >= TextArea.Right - xOffSet)
            {
                xOffSet = TextArea.X + TextArea.Width - (pos.X + H_SCROLL_DELTA);
                redraw = true;
            }
            if (pos.X < TextArea.X - xOffSet && pos.X >= TextArea.X)
            {
                xOffSet = TextArea.X - pos.X;
                redraw = true;
            }
            if (redraw)
            {
                int diff = (-oldOffSet) + xOffSet;
                if (diff < 0) scrollRight(-diff); // scroll right
                else scrollLeft(diff); // scroll left
            }
            // search for line number of new caret position:
            caret = pos;
            caret.LineNumber = GetLineNumberRelative(caret.Line, true);
            Color col = document.ColorAt(caret);
            if (col != Color.Empty)
            {
                document.Color = col;
            }
            else
            {
                document.Color = DefaultForeColor;
            }
            invertCaret();
            if (!caretTimer.Enabled) caretTimer.Start();
            OnCaretChange(cArgs);
        }

        /// <summary>
        /// This method removes the caret by setting it to null and stops the caret timer. If there is a caret 
        /// visible this method first removes the caret.
        /// </summary>
        protected void removeCaret()
        {
            if (caret != null)
            {
                oldCaret = caret;
                caretTimer.Stop();
                if (caret != null && caretPen.Color != BackColor) invertCaret();
                caret = null;
            }
        }

        /// <summary>
        /// Event handler method for the caret blink timer. Calls invertCaret() at the specified timer
        /// interval to draw or delete the caret at the given position.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">EventArgs associated to the event.</param>
        protected void caretTimer_Tick(object sender, EventArgs e)
        {
            invertCaret();
        }

        /// <summary>
        /// Moves the caret one position to the left.
        /// <param name="select">Indicates if the char should be selected when moving the caret left.</param>
        /// </summary>
        protected virtual void moveCaretLeft(bool select)
        {
            if (caret != null)
            {
                TLine newLine = caret.Line;
                int newPos = caret.LinePos - 1;
                if (caret.LinePos == 0)
                {
                    newLine = caret.Line.Prev;
                    if (newLine != null) newPos = newLine.LastPos;
                }
                if (newLine != null)
                {
                    Position<TLine, TToken> pos = Pos(newPos, newLine);
                    if (select)
                    {
                        if (selection != null) updateSelection(pos);
                        else setSelection(caret, pos);
                    }
                    else setCaret(pos);
                }
            }
        }

        /// <summary>
        /// Moves the caret one position to the right.
        /// <param name="select">Indicates if the char should be selected when moving the caret right.</param>
        /// </summary>
        protected virtual void moveCaretRight(bool select)
        {
            if (caret != null)
            {
                TLine newLine = caret.Line;
                int newPos = caret.LinePos + 1;
                if (caret.LinePos == newLine.LastPos)
                {
                    newLine = caret.Line.Next;
                    newPos = 0;
                }
                if (newLine != null)
                {
                    Position<TLine, TToken> pos = Pos(newPos, newLine);
                    if (select)
                    {
                        if (selection != null) updateSelection(pos);
                        else setSelection(caret, pos);
                    }
                    else setCaret(pos);
                }
            }
        }

        /// <summary>
        /// Moves the caret one line up.
        /// <param name="select">Indicates if the line should be selected when moving the caret up.</param>
        /// </summary>
        protected virtual void moveCaretUp(bool select)
        {
            if (caret != null)
            {
                if (caret.Line.Prev != null)
                {
                    int newPos = caret.LinePos;
                    int lastP = caret.Line.Prev.LastPos;
                    if (caret.LinePos > lastP)
                    {
                        newPos = lastP;
                    }
                    Position<TLine, TToken> pos = Pos(newPos, caret.Line.Prev);
                    if (select)
                    {
                        if (selection != null) updateSelection(pos);
                        else setSelection(caret, pos);
                    }
                    else setCaret(pos);
                }
            }
        }

        /// <summary>
        /// Moves the caret one line down.
        /// <param name="select">Indicates if the line should be selected when moving the caret down.</param>
        /// </summary>
        protected virtual void moveCaretDown(bool select)
        {
            if (caret != null)
            {
                if (caret.Line.Next != null)
                {
                    int newPos = caret.LinePos;
                    int lastP = caret.Line.Next.LastPos;
                    if (caret.LinePos > lastP)
                    {
                        newPos = lastP;
                    }
                    Position<TLine, TToken> pos = Pos(newPos, caret.Line.Next);
                    if (select)
                    {
                        if (selection != null) updateSelection(pos);
                        else setSelection(caret, pos);
                    }
                    else setCaret(pos);
                }
            }
        }

        /***** Selection handling: *****/

        /// <summary>
        /// Sets the selection to the text between the given from and to positions and draws it. This method
        /// removes any selection or caret that's currently active. If the passed positions are equal the caret
        /// will be set at the given position.
        /// </summary>
        /// <param name="anchor">The anchor position of the new selection.</param>
        /// <param name="toPos">The second position determining the selection.</param>
        protected virtual void setSelection(Position<TLine, TToken> anchor, Position<TLine, TToken> toPos)
        {
            if (anchor == null || toPos == null) return;
            if (selection != null) removeSelection();
            if (caret != null) removeCaret();
            if (anchor.Equals(toPos))
            {
                setCaret(anchor);
                return;
            }
            if (!anchor.IsValid) anchor = Pos(anchor.LinePos, anchor.Line);
            if (!toPos.IsValid) toPos = Pos(toPos.LinePos, toPos.Line);
            selectionAnchor = anchor;
            selection = new Selection<TLine, TToken>(selectionAnchor, selectionAnchor);
            updateSelection(toPos);
        }

        /// <summary>
        /// This method is to be called when a currently active selection changes it's size. The parameter newPos
        /// is the start or end position of the selection relative to the selectionAnchor field (which must be 
        /// non-null for a valid active selection, as well as the selection field). This method causes the part of
        /// the selection which has changed to be redrawn in order to reflect the changes! Furthermore this method
        /// sets the caret to the new position!
        /// </summary>
        /// <param name="newPos">The new selection end point relative to the selectionAnchor position.</param>
        protected virtual void updateSelection(Position<TLine, TToken> newPos)
        {
            if (selection != null && newPos != null && selectionAnchor != null)
            {
                Position<TLine, TToken> inv1 = newPos;
                Position<TLine, TToken> inv2 = selection.To;
                if (selection.From.Equals(selectionAnchor))
                {
                    // new pos is before selection start
                    if (newPos < selection.From)
                    {
                        deselectLines(selection.From.Line, selection.To.Line);
                        selection.From = newPos;
                        selection.To = selectionAnchor;
                        selectLines(selection.From.Line, selection.To.Line);
                    }
                        // new pos is after selection end
                    else if (newPos > selection.To)
                    {
                        inv1 = selection.To;
                        inv2 = newPos;
                        selectLines(selection.To.Line, newPos.Line);
                        selection.To = newPos;
                    }
                        // new pos is between selection start and end
                    else if (newPos > selection.From)
                    {
                        deselectLines(newPos.Line, selection.To.Line);
                        selection.To = newPos;
                        selection.To.Line.IsSelected = true;
                    }
                        // new pos is at selection start
                    else
                    {
                        deselectLines(selection.From.Line, selection.To.Line);
                        selection.To = selectionAnchor;
                    }
                }
                else
                {
                    inv2 = selection.From;
                    // new pos is after selection end
                    if (newPos > selection.To)
                    {
                        inv1 = selection.From;
                        inv2 = newPos;
                        deselectLines(selection.From.Line, selection.To.Line);
                        selection.From = selectionAnchor;
                        selection.To = newPos;
                        selectLines(selection.From.Line, selection.To.Line);
                    }
                        // new pos is before selection start
                    else if (newPos < selection.From)
                    {
                        selectLines(newPos.Line, selection.From.Line);
                        selection.From = newPos;
                    }
                        // new pos is between selection start and end
                    else if (newPos < selection.To)
                    {
                        inv1 = selection.From;
                        inv2 = newPos;
                        deselectLines(selection.From.Line, newPos.Line);
                        selection.From = newPos;
                        selection.From.Line.IsSelected = true;
                    }
                        // new pos is at selection end
                    else
                    {
                        inv1 = selection.From;
                        inv2 = newPos;
                        deselectLines(selection.From.Line, selection.To.Line);
                        selection.From = selectionAnchor;
                    }
                }
                if (caret != null && caretPen.Color != BackColor) invertCaret();
                //removeCaret();
                if (!inv1.Line.IsVisible)
                {
                    Selection<TLine, TToken> sel = getVisibleSelection(inv1, inv2);
                    if (sel != null) inv1 = sel.From;
                }
                drawContent(inv1.Line, getGraphics(new Region(getVisibleBounds(inv1, inv2))), DrawMode.Lines, true);
                // ensure vertical visibility:
                if (!newPos.Line.IsVisible)
                {
                    if (newPos.Line.Prev != null && newPos.Line.Prev.IsVisible)
                    {
                        scrollDown(1);
                    }
                    else if (newPos.Line.Next != null && newPos.Line.Next.IsVisible)
                    {
                        scrollUp(1);
                    }
                }
                setCaret(newPos);
            }
        }

        /// <summary>
        /// Removes the current selection (if there is any) by calling drawContent and sets
        /// the global selection field to null.
        /// </summary>
        protected void removeSelection()
        {
            if (selection != null && selection.From != null && selection.To != null)
            {
                if (!selection.From.Equals(selection.To))
                {
                    TLine search = selection.From.Line;
                    while (search != null && search != selection.To.Line.Next)
                    {
                        search.IsSelected = false;
                        search = search.Next;
                    }
                    Graphics g = getGraphics(new Region(getVisibleBounds(selection.From, selection.To)));
                    if (!selection.From.Line.IsVisible)
                    {
                        Selection<TLine, TToken> sel = getVisibleSelection(selection.From, selection.To);
                        if (sel != null) selection.From = sel.From;
                    }
                    drawContent(selection.From.Line, g, DrawMode.Lines, false);
                }
            }
            selectionAnchor = null;
            selection = null;
        }

        /// <summary>
        /// Sets the IsSelected property of the lines between from and to to true.
        /// </summary>
        /// <param name="from">first line to select</param>
        /// <param name="to">last line to select</param>
        protected virtual void selectLines(TLine from, TLine to)
        {
            if (from != null && to != null)
            {
                TLine search = from;
                while (search != null && search != to.Next)
                {
                    search.IsSelected = true;
                    search = search.Next;
                }
            }
        }

        /// <summary>
        /// Sets the IsSelected property of the lines between from and to to false.
        /// </summary>
        /// <param name="from">first line to deselect</param>
        /// <param name="to">last line to deselect</param>
        protected virtual void deselectLines(TLine from, TLine to)
        {
            if (from != null && to != null)
            {
                TLine search = from;
                while (search != null && search != to.Next)
                {
                    search.IsSelected = false;
                    search = search.Next;
                }
            }
        }

        /***** Scrolling: *****/

        /// <summary>
        /// Sets the firstVisLine reference to the passed line and then calculates the positions of all followers
        /// and redraws them.
        /// NOTE: This method will in most cases be much slower than the overloaded implementation which takes a
        /// line index as parameter as this method always searches for the given line beginning at the first line in
        /// the document!
        /// </summary>
        /// <param name="l">Line to set as new first visible line.</param>
        protected virtual void scrollToLine(TLine l)
        {
            if (l != null)
            {
                int oldVal = VScroll.Value;
                int num = GetLineNumber(l);
                if (num <= VScroll.Maximum && num >= VScroll.Minimum)
                {
                    setLinesInvisible(firstVisLine, lastVisLine);
                    firstVisLine = l;
                    firstVisLine.X = TextArea.X;
                    firstVisLine.Y = TextArea.Y;
                    calcLinePosFrom(firstVisLine, true, true);
                    VScroll.Value = num;
                    notifyScrollEvent(ScrollOrientation.VerticalScroll, oldVal, num);
                }
            }
        }

        /// <summary>
        /// Sets the firstVisLine reference to the line at the given index and then calculates the positions of all 
        /// followers and redraws them.
        /// NOTE: This method should in most cases perform much better than it's counterpart which takes a line 
        /// reference as parameter as this method determines the line to scroll to relative to the current first
        /// visible line or the document's first or last line (whichever is closest to the given index!)
        /// </summary>
        /// <param name="index">Index of the line to scroll to.</param>
        protected virtual void scrollToLine(int index)
        {
            if (index <= VScroll.Maximum && index >= VScroll.Minimum)
            {
                int lastDist = document.LineCount - index;
                int firstVisDist = Math.Abs(VScroll.Value - index);
                int oldVal = VScroll.Value;
                TLine newL;
                if (firstVisDist < index && firstVisDist < lastDist)
                {
                    newL = document.LineRelativeTo(firstVisLine, oldVal, index);
                }
                else newL = document[index];
                if (newL != null && newL != firstVisLine)
                {
                    setLinesInvisible(firstVisLine, lastVisLine);
                    firstVisLine = newL;
                    firstVisLine.X = TextArea.X;
                    firstVisLine.Y = TextArea.Y;
                    calcLinePosFrom(firstVisLine, true, true);
                    VScroll.Value = index;
                    notifyScrollEvent(ScrollOrientation.VerticalScroll, oldVal, index);
                }
            }
        }

        /// <summary>
        /// This method scrolls up the given amount of lines (or less - depending on how much scrolling is possible)
        /// and calls the OnScroll() method with the appropriate parameters to allow subclasses to handle scroll events.
        /// </summary>
        /// <param name="lines">The desired amount of lines to scroll up.</param>
        protected virtual void scrollUp(int lines)
        {
            if (firstVisLine != null && firstVisLine != FirstLine)
            {
                int oldVal = VScroll.Value;
                // if we do not scroll a whole page:
                if (lines < TextArea.Height/LineHeight)
                {
                    TLine oldFirstV = firstVisLine;
                    TLine newLastV = lastVisLine;
                    int i = 0;
                    // search for new first visible line:
                    while (i < lines && firstVisLine.Prev != null)
                    {
                        firstVisLine.IsVisible = true;
                        if (VScroll.Value - 1 >= VScroll.Minimum) VScroll.Value--;
                        firstVisLine = firstVisLine.Prev;
                        i++;
                    }
                    int scrollH = (i + 1)*LineHeight;
                    // check if new lastVisLine will be different from old one:
                    if ((lastVisLine.Y + scrollH) > TextArea.Bottom)
                    {
                        // search for new last visible line:
                        int j = 0;
                        while (j < i && (newLastV.Y + scrollH) > TextArea.Bottom)
                        {
                            newLastV.IsVisible = false;
                            newLastV = newLastV.Prev;
                            j++;
                        }
                    }
                    // make BitBLT copy of old firstVisLine up to new last visible line:
                    int copyH = newLastV.Y + LineHeight - oldFirstV.Y;
                    int newOldFPos = TextArea.Y + i*LineHeight;
                    var srcR = new Rectangle(oldFirstV.X, oldFirstV.Y, TextArea.Width, copyH);
                    var destR = new Rectangle(TextArea.X, newOldFPos, TextArea.Width, copyH);
                    makeBitBltCopy(srcR, destR);
                    // then calculate the new positions of the copied lines:
                    calcLinePositions(false, false);
                    // finally calculate properties of newly visible lines and draw them:
                    var redrawRect = new Rectangle(firstVisLine.X, firstVisLine.Y, TextArea.Width,
                                                   oldFirstV.Y - firstVisLine.Y);
                    drawContent(firstVisLine, getGraphics(new Region(redrawRect)), DrawMode.Lines, false);
                }
                else
                {
                    // handle scroll up of whole page:
                    TLine curr = firstVisLine;
                    int num = 0;
                    setLinesInvisible(firstVisLine, lastVisLine);
                    while (num < lines && curr != null)
                    {
                        curr = curr.Prev;
                        num++;
                    }
                    if (curr != null) firstVisLine = curr;
                    else
                    {
                        num--;
                        firstVisLine = document.First;
                    }
                    if (VScroll.Value - num >= VScroll.Minimum) VScroll.Value -= num;
                    else VScroll.Value = VScroll.Minimum;
                    firstVisLine.X = TextArea.X;
                    firstVisLine.Y = TextArea.Y;
                    calcLinePosFrom(firstVisLine, true, false);
                }
                int newVal = VScroll.Value;
                OnInvalidateRegion(screenGraphics.Clip);
                notifyScrollEvent(ScrollOrientation.VerticalScroll, oldVal, newVal);
            }
        }

        /// <summary>
        /// This method scrolls down the given amount of lines (or less - depending on how much scrolling is possible)
        /// and calls the OnScroll() method with the appropriate parameters to allow subclasses to handle scroll events.
        /// </summary>
        /// <param name="lines">The desired amount of lines to scroll down.</param>
        protected virtual void scrollDown(int lines)
        {
            if (firstVisLine != null && firstVisLine.Next != null)
            {
                int oldVal = VScroll.Value;
                // if we do not scroll a whole page:
                if (lines < TextArea.Height/LineHeight)
                {
                    TLine newFirstV = firstVisLine;
                    TLine oldLastV = lastVisLine;
                    int i = 0;
                    // search for new first visible line
                    while (i < lines && newFirstV.Next != null)
                    {
                        newFirstV.IsVisible = false;
                        if (VScroll.Value + 1 <= VScroll.Maximum) VScroll.Value++;
                        newFirstV = newFirstV.Next;
                        i++;
                    }
                    // new first visible line is new last:
                    if (i < lines || newFirstV.Next == null)
                    {
                        firstVisLine = newFirstV;
                        firstVisLine.X = TextArea.X;
                        firstVisLine.Y = TextArea.Y;
                        calcLinePosFrom(firstVisLine, true, false);
                    }
                        // search for new last visible line:
                    else
                    {
                        i = 0;
                        while (i < lines && lastVisLine.Next != null)
                        {
                            lastVisLine = lastVisLine.Next;
                            i++;
                        }
                        // make BitBLT copy from new first visible line up to old last visible line
                        var srcR = new Rectangle(newFirstV.X, newFirstV.Y, TextArea.Width,
                                                 oldLastV.Y + LineHeight - newFirstV.Y);
                        var destR = new Rectangle(TextArea.X, TextArea.Y, TextArea.Width,
                                                  oldLastV.Y + LineHeight - newFirstV.Y);
                        makeBitBltCopy(srcR, destR);
                        firstVisLine = newFirstV;
                        calcLinePositions(false, false);
                        // new last visible line is last line --> clear area below:
                        if (lastVisLine == oldLastV)
                        {
                            var bounds = new Rectangle(lastVisLine.X, lastVisLine.Y + LineHeight, TextArea.Width,
                                                       TextArea.Bottom - (lastVisLine.Y + LineHeight));
                            Graphics g = getGraphics(new Region(bounds));
                            g.Clear(BackColor);
                        }
                            // new last visible line is not last line --> draw newly visible lines:
                        else
                        {
                            TLine from = oldLastV.Next;
                            var redrawRect = new Rectangle(from.X, from.Y, TextArea.Width,
                                                           lastVisLine.Y + LineHeight - from.Y);
                            drawContent(from, getGraphics(new Region(redrawRect)), DrawMode.Lines, false);
                        }
                    }
                }
                    // handle scroll down of a whole page:
                else
                {
                    TLine curr = firstVisLine;
                    TLine oldC = curr;
                    int num = 0;
                    setLinesInvisible(firstVisLine, lastVisLine);
                    while (num < lines && curr != null)
                    {
                        oldC = curr;
                        curr = curr.Next;
                        num++;
                    }
                    if (curr != null) firstVisLine = curr;
                    else
                    {
                        firstVisLine = oldC;
                        num--;
                    }
                    if (VScroll.Value + num <= VScroll.Maximum) VScroll.Value += num;
                    else VScroll.Value = VScroll.Maximum;
                    firstVisLine.X = TextArea.X;
                    firstVisLine.Y = TextArea.Y;
                    calcLinePosFrom(firstVisLine, true, false);
                }
                int newVal = VScroll.Value;
                OnInvalidateRegion(screenGraphics.Clip);
                notifyScrollEvent(ScrollOrientation.VerticalScroll, oldVal, newVal);
            }
        }

        /// <summary>
        /// Scrolls the given amount of pixels to the right and calls the OnScroll() method with the appropriate 
        /// parameters to allow subclasses to handle scroll events.
        /// </summary>
        /// <param name="width">Amount of pixels to scroll to the right.</param>
        protected virtual void scrollRight(int width)
        {
            int oldVal = HScroll.Value;
            Rectangle bounds;
            if (width < TextArea.Width)
            {
                var srcR = new Rectangle(TextArea.X + width, TextArea.Y, TextArea.Width - width, TextArea.Height);
                var destR = new Rectangle(TextArea.X, TextArea.Y, TextArea.Width - width, TextArea.Height);
                makeBitBltCopy(srcR, destR);
                bounds = new Rectangle(TextArea.Right - width, TextArea.Y, width, TextArea.Height);
            }
            else
            {
                bounds = new Rectangle(TextArea.X, TextArea.Y, TextArea.Width, TextArea.Height);
            }
            drawContent(firstVisLine, getGraphics(new Region(bounds)), DrawMode.Lines, true);
            if (-xOffSet > HScroll.Maximum) HScroll.Value = HScroll.Maximum;
            else if (-xOffSet < HScroll.Minimum) HScroll.Value = HScroll.Minimum;
            else HScroll.Value = -xOffSet;
            int newVal = HScroll.Value;
            OnInvalidateRegion(screenGraphics.Clip);
            notifyScrollEvent(ScrollOrientation.HorizontalScroll, oldVal, newVal);
        }

        /// <summary>
        /// Scrolls the given amount of pixels to the left and calls the OnScroll() method with the appropriate 
        /// parameters to allow subclasses to handle scroll events.
        /// </summary>
        /// <param name="width">Amount of pixels to scroll to the left.</param>
        protected virtual void scrollLeft(int width)
        {
            int oldVal = HScroll.Value;
            Rectangle bounds;
            if (width < TextArea.Width)
            {
                var srcR = new Rectangle(TextArea.X, TextArea.Y, TextArea.Width - width, TextArea.Height);
                var destR = new Rectangle(TextArea.X + width, TextArea.Y, TextArea.Width - width, TextArea.Height);
                makeBitBltCopy(srcR, destR);
                bounds = new Rectangle(TextArea.X, TextArea.Y, width, TextArea.Height);
            }
            else bounds = new Rectangle(TextArea.X, TextArea.Y, TextArea.Width, TextArea.Height);
            drawContent(firstVisLine, getGraphics(new Region(bounds)), DrawMode.Lines, true);
            if (-xOffSet > HScroll.Maximum) HScroll.Value = HScroll.Maximum;
            else if (-xOffSet < HScroll.Minimum) HScroll.Value = HScroll.Minimum;
            else HScroll.Value = -xOffSet;
            int newVal = HScroll.Value;
            OnInvalidateRegion(screenGraphics.Clip);
            notifyScrollEvent(ScrollOrientation.HorizontalScroll, oldVal, newVal);
        }

        /// <summary>
        /// This helper method is to be called from the scroll{Up|Down|Left|Right} methods to allow handling of scroll
        /// events through the OnScroll() method.
        /// </summary>
        /// <param name="o">ScrollBar orientation.</param>
        /// <param name="oldVal">The old scrollbar value.</param>
        /// <param name="newVal">The new scrollbar value.</param>
        protected void notifyScrollEvent(ScrollOrientation o, int oldVal, int newVal)
        {
            if (newVal == oldVal) return;
            ScrollEventType type;
            ScrollBar curr;
            if (o == ScrollOrientation.VerticalScroll) curr = VScroll;
            else curr = HScroll;
            int diff = newVal - oldVal;
            if (newVal == curr.Minimum) type = ScrollEventType.First;
            else if (newVal == curr.Maximum) type = ScrollEventType.Last;
            else if (diff == -curr.SmallChange) type = ScrollEventType.SmallDecrement;
            else if (diff == -curr.LargeChange) type = ScrollEventType.LargeDecrement;
            else if (diff == curr.SmallChange) type = ScrollEventType.SmallIncrement;
            else if (diff == curr.LargeChange) type = ScrollEventType.LargeIncrement;
            else type = ScrollEventType.ThumbPosition;
            OnScroll(new ScrollEventArgs(type, oldVal, newVal, o));
        }

        /// <summary>
        /// Event handler method for horizontal scroll events. Occurs when user moves slider.
        /// </summary>
        /// <param name="sender">Origin of the scroll event.</param>
        /// <param name="e">ScrollEventArgs object.</param>
        protected virtual void HScroll_Scroll(object sender, ScrollEventArgs e)
        {
            int diff = e.NewValue - e.OldValue;
            if (diff != 0)
            {
                xOffSet = -e.NewValue;
                if (diff > 0)
                {
                    scrollRight(diff);
                }
                else
                {
                    scrollLeft(-diff);
                }
            }
        }

        /// <summary>
        /// Event handler method for vertical scroll events. Occurs when user moves slider.
        /// </summary>
        /// <param name="sender">Origin of the scroll event.</param>
        /// <param name="e">ScrollEventArgs object.</param>
        protected virtual void VScroll_Scroll(object sender, ScrollEventArgs e)
        {
            int diff = e.NewValue - e.OldValue;
            if (diff != 0)
            {
                //if (Math.Abs(diff) < (2 * TextArea.Height / LineHeight)) {
                if (diff > 0)
                {
                    scrollDown(diff);
                }
                else
                {
                    scrollUp(-diff);
                }
                /*}
                else if (firstVisLine != null) {
                    scrollToLine(e.NewValue);
                }*/
            }
        }

        /// <summary>
        /// Event handler method for changes to the visibility status of the Scrollbars. Redraws
        /// the ClientArea if necessary.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void ScrollBar_VisibleChanged(object sender, EventArgs e)
        {
            invalidatePositions();
            calcLinePositions(true, true);
        }

        /***** Line handling: *****/

        /// <summary>
        /// This method simply calculates the positions of all visible lines starting with the line to which the
        /// firstVisLine reference is currently set. The start position of the firstVisLine will be set to the 
        /// position of the upper left corner of the TextArea and all following lines will be calculated relative
        /// to that position. The method also sets the visibility flag of all calculated lines to true and updates
        /// the lastVisLine reference.
        /// The redraw flag indicates if the control should be redrawn after the calculation whereas this method also
        /// causes all registered reserved areas to be redrawn - if only the calculated lines are to be redrawn use 
        /// the calcLinePosFrom method with the appropriate parameters.
        /// </summary>
        /// <param name="redraw">Determines if all lines should be drawn after calculation.</param>
        /// <param name="paint">When redraw is set to true this flag determines if the drawn lines should be 
        /// immediately rendered to the screen - otherwise it has no effect.</param>
        protected virtual void calcLinePositions(bool redraw, bool paint)
        {
            Graphics g;
            if (firstVisLine != null)
            {
                TLine curr = firstVisLine;
                int x = TextArea.X;
                int y = TextArea.Y;
                int lastStartPos = TextArea.Bottom - LineHeight;
                while (y < lastStartPos && curr != null)
                {
                    curr.X = x;
                    curr.Y = y;
                    curr.IsVisible = true;
                    y += LineHeight;
                    lastVisLine = curr;
                    curr = curr.Next;
                }
                if (redraw)
                {
                    var bounds = new Rectangle(firstVisLine.X, firstVisLine.Y, TextArea.Width,
                                               lastVisLine.Y - firstVisLine.Y + LineHeight);
                    g = getGraphics(new Region(bounds));
                    drawContent(firstVisLine, g, DrawMode.All, paint);
                }
            }
            else if (redraw)
            {
                // clear whole text area and draw reserved areas if control is empty:
                g = getGraphics(new Region(ClientScrollRectangle));
                g.Clear(BackColor);
                drawContent(null, g, DrawMode.Reserved, paint);
            }
        }

        /// <summary>
        /// This method calculates the line positions of all visible lines beginning at the given line down to the 
        /// last visible line in the text box and the lastVisLine reference will be set accordingly.
        /// The position of the start line has to be set beforehand as it acts as the baseline for the calculation
        /// of all following lines.
        /// The redraw flag indicates if the calculated lines should be redrawn - in contrast to calcLinePositions
        /// this method never causes any reserved areas to be redrawn!
        /// </summary>
        /// <param name="from">Line at which to start calculation (provides base coordinates for calculation)</param>
        /// <param name="redraw">Determines if the calculated lines should be redrawn.</param>
        /// <param name="paint">When redraw is set to true this flag determines if the drawn changes should be
        /// immediately rendered to the screen - otherwise it has no effect!</param>
        protected virtual void calcLinePosFrom(TLine from, bool redraw, bool paint)
        {
            Graphics g;
            if (from != null)
            {
                from.IsVisible = true;
                lastVisLine = from;
                TLine curr = from.Next;
                int y = from.Y + LineHeight;
                int x = from.X;
                int lastStartPos = TextArea.Bottom - LineHeight;
                while (y < lastStartPos && curr != null)
                {
                    curr.X = x;
                    curr.Y = y;
                    curr.IsVisible = true;
                    y += LineHeight;
                    lastVisLine = curr;
                    curr = curr.Next;
                }
                if (redraw)
                {
                    var bounds = new Rectangle(from.X, from.Y, TextArea.Width, lastVisLine.Y + LineHeight - from.Y);
                    g = getGraphics(new Region(bounds));
                    drawContent(from, g, DrawMode.Lines, paint);
                }
            }
            else calcLinePositions(redraw, paint);
        }

        /// <summary>
        /// Calculates the width and height of a line by traversing through all contained tokens for the
        /// given Graphics object. This method sets the Modified flag of the line to false. 
        /// Uses the ColorTextBox's default Font for calculating line metrics. 
        /// Override this method if you want to use different fonts for each line token!
        /// </summary>
        /// <param name="l">Line for which to calculate the described parameters.</param>
        /// <param name="g">Graphics object on which to render the line.</param>
        protected virtual void calcLine(TLine l, Graphics g)
        {
            lock (l)
            {
                int width = 0;
                TToken t = l.First;
                Size tSize;
                while (t != null && !t.IsNewLineToken)
                {
                    tSize = measureString(g, t.Value, Font);
                    t.Height = LineHeight;
                    t.Width = tSize.Width;
                    width += t.Width;
                    t = t.Next;
                }
                l.Modified = false;
                l.Height = LineHeight;
            }
        }

        /// <summary>
        /// Calculates the distance of the base line from the line start for the given font.
        /// </summary>
        /// <param name="f">Font to use for calculation.</param>
        /// <returns>Rounded distance of baseline from line start in pixel.</returns>
        protected int calcBaseDistance(Font f)
        {
            float size = f.Size;
            int emSize = f.FontFamily.GetEmHeight(f.Style);
            int descent = f.FontFamily.GetCellDescent(f.Style);
            float descF = descent*size/emSize;
            var lineBase = (int) Math.Ceiling(descF);
            return lineBase;
        }

        /// <summary>
        /// Sets the visibility flag of all lines between the given points to false. Must be called before
        /// the currently visible lines are changed (insertion of lines, scrolling, ...)
        /// </summary>
        /// <param name="fromY">Y coordinate of the first line to set to invisible.</param>
        /// <param name="toY">Y coordinate of the last line to set to invisible</param>
        protected virtual void setLinesInvisible(int fromY, int toY)
        {
            TLine start = GetLineAtYPos(ref fromY);
            TLine end = GetLineAtYPos(ref toY);
            while (start != null && start != end)
            {
                start.IsVisible = false;
                start = start.Next;
            }
            if (start != null) start.IsVisible = false;
        }

        /// <summary>
        /// Sets the visibility flag for all lines between from and to (including from and to) to false. Must
        /// be called before the currently visible lines are changed (insertion, scrolling, ...)
        /// </summary>
        /// <param name="from">First line to set to invisible</param>
        /// <param name="to">Last line to set to invisible.</param>
        protected virtual void setLinesInvisible(TLine from, TLine to)
        {
            TLine start = from;
            while (start != null && start != to)
            {
                start.IsVisible = false;
                start = start.Next;
            }
            if (start != null) start.IsVisible = false;
        }

        /***** Editing: *****/

        /* TODO: Performance improvements for drawing updates */

        /// <summary>
        /// Helper method which converts the given line list to an RTF string. The default implementation
        /// uses a RichTextBox for the conversion. It sets the font of the returned RTF string to the 
        /// globally used ColorTextBox font and only uses each token's color information for the conversion!
        /// (Override to use different token font styles, sizes, ...)
        /// </summary>
        /// <param name="l">The line to convert to RTF.</param>
        /// <returns></returns>
        protected virtual String convertToRtf(TLine l)
        {
            RichTextBox rtb = getRTB();
            rtb.Font = Font;
            rtb.Clear();
            var sb = new StringBuilder();
            TToken t;
            while (l != null)
            {
                t = l.First;
                while (t != null)
                {
                    rtb.SelectionColor = t.Color;
                    rtb.AppendText(t.Value);
                    t = t.Next;
                }
                //sb.Append(rtb.Rtf);
                //rtb.Clear();
                l = l.Next;
            }
            return rtb.Rtf;
        }

        /// <summary>
        /// Helper method which converts the provided formatted RTF string to a line list.
        /// </summary>
        /// <param name="rtf">An RTF formatted string.</param>
        /// <returns>A line list representing the provided RTF string.</returns>
        protected virtual TLine convertFromRtf(String rtf)
        {
            RichTextBox rtb = getRTB();
            rtb.Clear();
            rtb.Rtf = rtf;
            var first = new TLine();
            TLine curr = first;
            Color col;
            char c;
            bool crlf = false, cr = false;
            int len = rtb.TextLength;
            for (int i = 0; i < len; i++)
            {
                rtb.SelectionStart = i;
                rtb.SelectionLength = 1;
                col = rtb.SelectionColor;
                c = rtb.SelectedText[0];
                if (c == '\r')
                {
                    cr = true;
                    crlf = true;
                }
                else if (c == '\n' && cr)
                {
                    crlf = false;
                    cr = false;
                    continue;
                }
                else if (c == '\n') crlf = true;
                else
                {
                    crlf = false;
                    cr = false;
                }
                if (crlf) curr.Insert(TokenBase<TToken>.NewLineToken(), curr.LastPos);
                else curr.Insert(c, curr.LastPos, col);
                if (curr.Next != null) curr = curr.Next;
            }
            return first;
        }

        /// <summary>
        /// This helper method is used for changing the indentation level of selected lines (by pressing TAB
        /// or SHIFT+TAB when there is a selection visible) by the given amount. If amount is a positive integer
        /// amount tab characters will be inserted at the beginning of each selected line. If amount is a 
        /// negative integer a maximum of amount tab characters will be removed from the beginning of each 
        /// selected line. Note: this method does not check if there currently exists a selection - this must be
        /// done prior to calling this method!
        /// </summary>
        /// <param name="amount">Number of tab chars to add to or remove from the beginning of selected lines.</param>
        protected virtual void changeSelectionIndent(int amount)
        {
            if (!isReadOnly)
            {
                redo.Clear();
                Position<TLine, TToken> newPos;
                Selection<TLine, TToken> backSel = selection;
                removeCaret();
                removeSelection();
                if (amount > 0)
                {
                    TLine search = backSel.From.Line;
                    while (search != null && search != backSel.To.Line.Next)
                    {
                        document.Insert("\t", search, 0, UpdateAction.Undoable, false);
                        if (search == backSel.From.Line)
                        {
                            newPos = Pos(backSel.From.LinePos + 1, backSel.From.Line);
                            backSel.From = newPos;
                        }
                        if (search == backSel.To.Line)
                        {
                            newPos = Pos(backSel.To.LinePos + 1, backSel.To.Line);
                            backSel.To = newPos;
                        }
                        search = search.Next;
                    }
                }
                else if (amount < 0)
                {
                    TLine search = backSel.From.Line;
                    while (search != null && search != backSel.To.Line.Next)
                    {
                        if (search.CharAt(0) == '\t')
                        {
                            document.Delete(search, 0, 1, UpdateAction.Undoable, false);
                            if (search == backSel.From.Line)
                            {
                                newPos = Pos(backSel.From.LinePos - 1, backSel.From.Line);
                                backSel.From = newPos;
                            }
                            if (search == backSel.To.Line)
                            {
                                newPos = Pos(backSel.To.LinePos - 1, backSel.To.Line);
                                backSel.To = newPos;
                            }
                        }
                        search = search.Next;
                    }
                }
                TLine updateFrom = backSel.From.Line;
                TLine updateTo = backSel.To.Line;
                setSelection(backSel.From, backSel.To);
            }
        }

        /// <summary>
        /// If there is a valid caret this method inserts the given character at the caret position and updates the
        /// TextArea. If there is currently text selected it will be replaced by the character!
        /// </summary>
        /// <param name="c">Character to insert.</param> 
        protected virtual void insertChar(char c)
        {
            if (!isReadOnly)
            {
                redo.Clear();
                if (selection != null && selection.From != null && selection.To != null)
                {
                    Position<TLine, TToken> fromP = selection.From;
                    Position<TLine, TToken> toP = selection.To;
                    removeCaret();
                    removeSelection();
                    if (!fromP.Line.IsVisible) scrollToLine(fromP.Line);
                    if (c == '\n' || c == '\r' || c == '\f')
                    {
                        document.Replace(fromP, toP, Environment.NewLine, UpdateAction.Undoable, true);
                    }
                    else
                    {
                        document.Replace(fromP, toP, c.ToString(), UpdateAction.Undoable, true);
                    }
                }
                else if (caret != null)
                {
                    Position<TLine, TToken> insertP = caret;
                    removeCaret();
                    removeSelection();
                    if (!insertP.Line.IsVisible) scrollToLine(insertP.Line);
                    // special handling of new line char
                    if (c == '\n' || c == '\r' || c == '\f')
                    {
                        document.Insert(Environment.NewLine, insertP.Line, insertP.LinePos, UpdateAction.Undoable, true);
                    }
                        // insertion of all other chars:
                    else
                    {
                        if (insertMode || insertP.LinePos == insertP.Line.LastPos)
                        {
                            document.Insert(c.ToString(), insertP.Line, insertP.LinePos, UpdateAction.Undoable, true);
                        }
                        else
                            document.Replace(insertP, Pos(insertP.LinePos + 1, insertP.Line), c.ToString(),
                                             UpdateAction.Undoable, true);
                    }
                }
            }
        }

        /// <summary>
        /// Deletes num chars after the caret position starting with the char at the current caret position.
        /// Linefeeds are treated as a single character.
        /// </summary>
        /// <param name="num">Number of characters to delete.</param>
        /// <returns>The actual number of characters that have been deleted.</returns>
        protected virtual int deleteAfterCaret(int num)
        {
            if (!isReadOnly)
            {
                if (caret != null)
                {
                    /* ensure caret is visible before deletion by scrolling to caret.Line-
                     * HACK: when deletion requires
                     * to scroll up only one line, scrolling is handled in the according event handler method 
                     * for performance reasons (only new firstVisLine has to be redrawn!)! 
                     */
                    if (!caret.Line.IsVisible)
                    {
                        if (caret.Line != firstVisLine.Prev) scrollToLine(caret.Line);
                        else firstVisLine = firstVisLine.Prev;
                    }
                    Position<TLine, TToken> delPos = caret;
                    removeCaret();
                    removeSelection();
                    redo.Clear();
                    return document.Delete(delPos.Line, delPos.LinePos, num, UpdateAction.Undoable, true);
                }
            }
            return 0;
        }

        /// <summary>
        /// Deletes a selected text if there is an active selection.
        /// </summary>
        protected virtual void deleteSelection()
        {
            if (!isReadOnly)
            {
                if (selection != null)
                {
                    if (!selection.From.Line.IsVisible) scrollToLine(selection.From.Line);
                    //int delete = selection.From.DistanceTo(selection.To);
                    Position<TLine, TToken> from = selection.From;
                    Position<TLine, TToken> to = selection.To;
                    removeSelection();
                    removeCaret();
                    redo.Clear();
                    document.Delete(from, to, UpdateAction.Undoable, true);
                }
            }
        }

        /// <summary>
        /// Handles update events from the document contained in the ColorTextBox. Triggers redrawing of the
        /// Control after a change by calling the appropriate drawOn{UpdateType} method and adds the changes 
        /// from the UpdateEventArgs to the Undo or Redo buffer.
        /// </summary>
        /// <param name="source">Source of the UpdateEvent.</param>
        /// <param name="args">UpdateEventArgs of this UpdateEvent.</param>
        protected virtual void handleUpdateEvent(object source, UpdateEventArgs args)
        {
            modified = true;
            if (args.Action == UpdateAction.Redo || args.Action == UpdateAction.Undoable)
            {
                undo.Add(args);
                if (undo.Count > MAX_UNDO_DEPTH)
                {
                    undo.RemoveAt(0);
                }
            }
            else if (args.Action == UpdateAction.Undo)
            {
                redo.Add(args);
                if (redo.Count > MAX_REDO_DEPTH)
                {
                    redo.RemoveAt(0);
                }
            }
            switch (args.Type)
            {
                case UpdateEventType.Insert:
                    {
                        VScroll.Maximum += args.Lines;
                        if (!suspendPaint) drawOnInsert(args);
                        else Caret = args.To;
                    }
                    break;
                case UpdateEventType.Delete:
                    {
                        if (VScroll.Maximum - args.Lines >= VScroll.Minimum) VScroll.Maximum -= args.Lines;
                        else VScroll.Maximum = VScroll.Minimum;
                        if (!suspendPaint) drawOnDelete(args);
                        else Caret = args.From;
                    }
                    break;
                case UpdateEventType.NewDocument:
                    initNewContent();
                    break;
                case UpdateEventType.UpdateColor:
                    {
                        if (!suspendPaint) drawOnColorChange(args);
                    }
                    break;
                case UpdateEventType.Replace:
                    {
                        if (!(args.FromLine == args.ToLine && args.From.LinePos == args.To.LinePos - 1) &&
                            args.From is Position<TLine, TToken> &&
                            args.To is Position<TLine, TToken>)
                        {
                            Select(args.From, args.To);
                        }
                    }
                    break;
                case UpdateEventType.None:
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// This helper method can be used to calculate the visible bounding rectangle between two positions. The 
        /// width of the returned rectangle will always be set to TextArea.Width!
        /// If no part of the area between fromP and toP is currently visible the method returns Rectangle.Empty!
        /// </summary>
        /// <param name="fromP">Start position.</param>
        /// <param name="toP">End position.</param>
        /// <returns>The visible portion of the rectangle surrounding the two positions.</returns>
        protected virtual Rectangle getVisibleBounds(Position<TLine, TToken> fromP, Position<TLine, TToken> toP)
        {
            Rectangle ret = Rectangle.Empty;
            if (fromP != null && toP != null)
            {
                /* TODO: Test if check was necessary!
                if (!(fromP < toP)) {
                    Position<TLine, TToken> tmp = fromP;
                    fromP = toP;
                    toP = tmp;
                }*/
                if (fromP.Line.IsVisible)
                {
                    if (toP.Line.IsVisible)
                    {
                        // fromP and toP are visible:
                        ret.X = fromP.Line.X;
                        ret.Y = fromP.Line.Y;
                        ret.Width = TextArea.Width;
                        ret.Height = toP.Line.Y + LineHeight - fromP.Line.Y;
                    }
                    else
                    {
                        // only fromP is visible:
                        ret.X = fromP.Line.X;
                        ret.Y = fromP.Line.Y;
                        ret.Width = TextArea.Width;
                        ret.Height = lastVisLine.Y + LineHeight - fromP.Line.Y;
                    }
                }
                else if (toP.Line.IsVisible)
                {
                    // only toP is visible:
                    ret.X = firstVisLine.X;
                    ret.Y = firstVisLine.Y;
                    ret.Width = TextArea.Width;
                    ret.Height = toP.Line.Y + LineHeight - firstVisLine.Y;
                }
                else
                {
                    // worst case: check all lines between fromP and toP for visibility
                    TLine search = fromP.Line.Next;
                    bool visible = false;
                    while (search != null && search != toP.Line && !visible)
                    {
                        if (search.IsVisible) visible = true;
                        search = search.Next;
                    }
                    if (visible)
                    {
                        // rectangle is between first and last visible position.
                        ret.X = firstVisLine.X;
                        ret.Y = firstVisLine.Y;
                        ret.Width = TextArea.Width;
                        ret.Height = lastVisLine.Y + LineHeight - firstVisLine.Y;
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// Returns the visible part of a selection defined by the two passed positions or null if such a part 
        /// doesn't exist!
        /// </summary>
        /// <param name="fromP">Start position of a selection.</param>
        /// <param name="toP">End position of a selection.</param>
        /// <returns></returns>
        protected virtual Selection<TLine, TToken> getVisibleSelection(Position<TLine, TToken> fromP,
                                                                       Position<TLine, TToken> toP)
        {
            Selection<TLine, TToken> ret = null;
            if (fromP != null && toP != null)
            {
                // TODO: test if check was necessary!
                /*if (!(fromP < toP)) {
                    Position<TLine, TToken> tmp = fromP;
                    fromP = toP;
                    toP = tmp;
                }*/
                if (fromP.Line.IsVisible)
                {
                    if (toP.Line.IsVisible)
                    {
                        // fromP and toP are visible:
                        ret = new Selection<TLine, TToken>(fromP, toP);
                    }
                    else
                    {
                        // only fromP is visible:
                        ret = new Selection<TLine, TToken>(fromP, Pos(lastVisLine.LastPos, lastVisLine));
                    }
                }
                else if (toP.Line.IsVisible)
                {
                    // only toP is visible:
                    ret = new Selection<TLine, TToken>(Pos(0, firstVisLine), toP);
                }
                else
                {
                    // worst case: check all lines between fromP and toP for visibility
                    TLine search = fromP.Line.Next;
                    bool visible = false;
                    while (search != null && search != toP.Line && !visible)
                    {
                        if (search.IsVisible) visible = true;
                        search = search.Next;
                    }
                    if (visible)
                    {
                        // whole area between first and last visible line is part of selection:
                        ret = new Selection<TLine, TToken>(Pos(0, firstVisLine), Pos(lastVisLine.LastPos, lastVisLine));
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// Calculates a Region object for the area occupied by additional paddings.
        /// </summary>
        /// <returns></returns>
        protected Region getPaddingRegion()
        {
            var reg = new Region();
            reg.MakeEmpty();
            var topLeft = new Point(TextArea.X - padding.Left, TextArea.Y - padding.Top);
            var bottomRight = new Point(TextArea.Right + padding.Right, TextArea.Bottom + padding.Bottom);
            var s = new Size(TextArea.Width + padding.Horizontal, padding.Top);
            var rect = new Rectangle(topLeft, s);
            reg.Union(rect);
            rect.Width = padding.Left;
            rect.Height = TextArea.Height + padding.Vertical;
            reg.Union(rect);
            rect.Location = bottomRight;
            rect.Width = -padding.Right;
            rect.Height = -rect.Height;
            reg.Union(rect);
            rect.Width = -(TextArea.Width + padding.Horizontal);
            rect.Height = -padding.Bottom;
            reg.Union(rect);
            return reg;
        }

        /***** RESERVED AREA HANDLING: *****/

        /* Reserved areas are regions that can be added to the four sides of the client area for user
         * defined painting. The line rendering will always be limited to the area surrounded by all
         * reserved areas that have been added to the Control. To add a reserved area the width or height
         * of the area, the location of the area and a method responsible for drawing the area have to be
         * supplied. Reserved areas need to have a unique identifier string for a given location and can be
         * inserted at a given index (starting with 0 for the innermost area)
         * For clarity the follwing graphic shows the concept for a ColorTextBox with 1 top, 2 left, 
         * 1 right and 2 bottom areas:
         * 
         *       _________________________
         *      |____________0____________|
         *      | | |                   | |
         *      | | |                   | |
         *      |1|0|     Text Area     |0| 
         *      | | |                   | |
         *      |_|_|___________________|_|
         *      |____________0____________|
         *      |____________1____________|
         * 
         * 
         * NOTE: It's not recommended to put a lot of complex logic inside the registered draw method of a
         * reserved area as this can slow down the whole rendering process significantly!
         * 
         */

        /// <summary>
        /// Adds the given width to the list of reserved areas on which no line drawing is permitted.
        /// The caller has to supply a unique name for the area at this location and an index at which the
        /// area should be added, as well as a drawing method that is responsible for drawing the specified
        /// area.
        /// </summary>
        /// <param name="id">A unique identifier string for the reserved area - can be used by the caller
        /// to retrieve the reserved area later on.</param>
        /// <param name="index">The preferred index at which to add the area - if the index is bigger than the
        /// actual number of contained areas, the area will be added to the last position!</param>
        /// <param name="width">Reserved width in pixel.</param>
        /// <param name="loc">Location to add the reserved area.</param>
        /// <param name="drawMeth">Method responsible for drawing this area.</param>
        /// <param name="eventMeth">Method responsible for handling mouse events in this area. May be null if
        /// the area should not react on mouse events.</param>
        /// <param name="hoverCursor">A cursor to be displayed when the mouse enters the reserved area.
        /// May be null in which case an arrow will be displayed.</param>
        /// <returns>True if the defined area was successfully added. False if the area could not be added
        /// (Name already defined, invalid width definition, no draw method supplied, invalid index)</returns>
        protected bool addResArea(String id, int index, int width, ReservedLocation loc,
                                  ReservedArea.DrawArea drawMeth, ReservedArea.HandleMouseEvent eventMeth,
                                  Cursor hoverCursor)
        {
            if (drawMeth == null || width < 0 || containsResArea(loc, id)) return false;
            List<ReservedArea> ral = reservedAreas[(int) loc];
            index += 1;
            if (index > ral.Count) index = ral.Count;
            if (index < 0) index = 0;
            var newArea = new ReservedArea(id, width, drawMeth, eventMeth, hoverCursor);
            ral.Insert(index, newArea);
            // invalidate local TextArea variable:
            textArea = Rectangle.Empty;
            // invalidate local width variables:
            switch (loc)
            {
                case ReservedLocation.Top:
                    topWidths = -1;
                    break;
                case ReservedLocation.Left:
                    leftWidths = -1;
                    break;
                case ReservedLocation.Right:
                    rightWidths = -1;
                    break;
                case ReservedLocation.Bottom:
                    bottomWidths = -1;
                    break;
            }
            invalidatePositions();
            return true;
        }

        /// <summary>
        /// Checks if an area of the given id is already contained at the given location.
        /// </summary>
        /// <param name="loc">Reserved location to search in.</param>
        /// <param name="id">Id string to search for.</param>
        /// <returns>True if the given location contains an area of the given id.</returns>
        protected bool containsResArea(ReservedLocation loc, String id)
        {
            List<ReservedArea> ral = reservedAreas[(int) loc];
            if (ral != null && ral.Count > 0 && id != null)
            {
                foreach (ReservedArea r in ral)
                {
                    if (r.Id.Equals(id)) return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Removes the reserved area with the given id from the list of reserved areas at the given location.
        /// </summary>
        /// <param name="loc">Location of the reserved area to remove.</param>
        /// <param name="id">Id of the reserved area to remove.</param>
        /// <returns>True if area was successfully removed. False if area doesn't exist.</returns>
        protected bool removeResArea(ReservedLocation loc, String id)
        {
            if (id != null)
            {
                ReservedArea remA = getResArea(loc, id);
                if (remA == null) return false;
                List<ReservedArea> ral = reservedAreas[(int) loc];
                if (ral.Remove(remA))
                {
                    // invalidate local TextArea variable:
                    textArea = Rectangle.Empty;
                    // invalidate local width variables:
                    switch (loc)
                    {
                        case ReservedLocation.Top:
                            topWidths = -1;
                            break;
                        case ReservedLocation.Left:
                            leftWidths = -1;
                            break;
                        case ReservedLocation.Right:
                            rightWidths = -1;
                            break;
                        case ReservedLocation.Bottom:
                            bottomWidths = -1;
                            break;
                    }
                    invalidatePositions();
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Helper method that returns the reserved area with the given id at the given location if it exists.
        /// </summary>
        /// <param name="loc">Reserved location to search in.</param>
        /// <param name="id">Id of reserved area to search for.</param>
        /// <returns>The reserved area at the specified location or null if it doesn't exist.</returns>
        private ReservedArea getResArea(ReservedLocation loc, String id)
        {
            if (id != null)
            {
                List<ReservedArea> ral = reservedAreas[(int) loc];
                foreach (ReservedArea ra in ral)
                {
                    if (ra.Id.Equals(id)) return ra;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the width of the reserved area with the given name.
        /// </summary>
        /// <param name="loc">Location of the reserved area.</param>
        /// <param name="id">Id of the reserved area.</param>
        /// <returns>Width of reserved area with the given id or -1 if it doesn't exist.</returns>
        protected int getResLength(ReservedLocation loc, String id)
        {
            if (id != null)
            {
                ReservedArea ra = getResArea(loc, id);
                if (ra != null) return ra.Width;
            }
            return -1;
        }

        /// <summary>
        /// Returns the Region that is occupied by the given reserved area. If the given region does not exist the
        /// method returns null. This method can be used to determine the region of an area for redrawing.
        /// </summary>
        /// <param name="loc">Location of the reserved area.</param>
        /// <param name="id">Id string of the reserved area.</param>
        /// <returns>The region occupied by the reserved area or null if it doesn't exist.</returns>
        protected Region getResRegion(ReservedLocation loc, String id)
        {
            List<ReservedArea> ral = reservedAreas[(int) loc];
            int x = 0, y = 0, width = 0, height = 0;
            bool found = false;
            switch (loc)
            {
                case ReservedLocation.Top:
                    {
                        x = ClientScrollRectangle.X;
                        y = ClientScrollRectangle.X + ReservedTopWidths;
                        width = ClientScrollRectangle.Width;
                        foreach (ReservedArea ra in ral)
                        {
                            y -= ra.Width;
                            height = ra.Width;
                            if (ra.Id.Equals(id))
                            {
                                found = true;
                                break;
                            }
                        }
                    }
                    break;
                case ReservedLocation.Bottom:
                    {
                        x = ClientScrollRectangle.X;
                        y = ClientScrollRectangle.Bottom - ReservedBottomWidths;
                        width = ClientScrollRectangle.Width;
                        foreach (ReservedArea ra in ral)
                        {
                            height = ra.Width;
                            if (ra.Id.Equals(id))
                            {
                                found = true;
                                break;
                            }
                            y += ra.Width;
                        }
                    }
                    break;
                case ReservedLocation.Left:
                    {
                        x = ClientScrollRectangle.X + ReservedLeftWidths;
                        y = ClientScrollRectangle.Y + ReservedTopWidths;
                        height = ClientScrollRectangle.Height - ReservedTopWidths - ReservedBottomWidths;
                        foreach (ReservedArea ra in ral)
                        {
                            x -= ra.Width;
                            width = ra.Width;
                            if (ra.Id.Equals(id))
                            {
                                found = true;
                                break;
                            }
                        }
                    }
                    break;
                case ReservedLocation.Right:
                    {
                        x = ClientScrollRectangle.Right - ReservedRightWidths;
                        y = ClientScrollRectangle.Y + ReservedTopWidths;
                        height = ClientScrollRectangle.Height - ReservedTopWidths - ReservedBottomWidths;
                        foreach (ReservedArea ra in ral)
                        {
                            width = ra.Width;
                            if (ra.Id.Equals(id))
                            {
                                found = true;
                                break;
                            }
                            x += ra.Width;
                        }
                    }
                    break;
                default:
                    return null;
            }
            if (found)
            {
                return new Region(new Rectangle(x, y, width, height));
            }
            return null;
        }

        /// <summary>
        /// Changes the width of the reserved area with the given id at the given location and redraws the 
        /// whole client area to reflect the changes. Returns false if the reserved area at the given location
        /// doesn't exist.
        /// </summary>
        /// <param name="newLength">New length (width or height) of the reserved area.</param>
        /// <param name="loc">Location of the reserved area.</param>
        /// <param name="id">Name of the reserved area.</param>
        /// <returns>True on success. False if the area doesn't exist.</returns>
        protected bool changeResLength(int newLength, ReservedLocation loc, String id)
        {
            if (id != null)
            {
                ReservedArea ra = getResArea(loc, id);
                if (ra != null)
                {
                    ra.Width = newLength;
                    // invalidate current text area.
                    textArea = Rectangle.Empty;
                    // invalidate local width variable:
                    switch (loc)
                    {
                        case ReservedLocation.Top:
                            topWidths = -1;
                            break;
                        case ReservedLocation.Left:
                            leftWidths = -1;
                            break;
                        case ReservedLocation.Right:
                            rightWidths = -1;
                            break;
                        case ReservedLocation.Bottom:
                            bottomWidths = -1;
                            break;
                    }
                    invalidatePositions();
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Inserts the given line (list) at the current caret position. If there exists a selection the
        /// selected text will be replaced with the given line content.
        /// If there is neither a caret nor a selection no changes will be made.
        /// NOTE: The current implementation does not check the inserted lines for validity, so it is possible
        /// to insert invalid lines (lines without or with multiple newline tokens!)
        /// </summary>
        /// <param name="lines">Lines to insert into the text box.</param>
        public void Insert(TLine lines)
        {
            if (selection != null && selection.From != null && selection.To != null)
            {
                Position<TLine, TToken> fromP = selection.From;
                Position<TLine, TToken> toP = selection.To;
                if (!selection.From.Line.IsVisible)
                {
                    scrollToLine(selection.From.Line);
                    redo.Clear();
                    removeSelection();
                    removeCaret();
                    document.Replace(fromP, toP, lines, UpdateAction.Undoable, true);
                }
            }
            else if (caret != null)
            {
                if (!caret.Line.IsVisible) scrollToLine(caret.Line);
                Position<TLine, TToken> fromP = caret;
                redo.Clear();
                removeCaret();
                document.Insert(lines, fromP.Line, fromP.LinePos, UpdateAction.Undoable, true);
            }
        }

        /// <summary>
        /// Inserts the given token (list) at the current caret position. If there exists a selection the
        /// selected text will be replaced with the given line content.
        /// If there is neither a caret nor a selection no changes will be made.
        /// NOTE: The inserted token list will be clipped at the first occurence of a newline token.
        /// </summary>
        /// <param name="tokens">Tokens to insert into the text box.</param>
        public void Insert(TToken tokens)
        {
            if (selection != null && selection.From != null && selection.To != null)
            {
                Position<TLine, TToken> fromP = selection.From;
                Position<TLine, TToken> toP = selection.To;
                if (!selection.From.Line.IsVisible)
                {
                    scrollToLine(selection.From.Line);
                    redo.Clear();
                    removeSelection();
                    removeCaret();
                    document.Replace(fromP, toP, tokens, UpdateAction.Undoable, true);
                }
            }
            else if (caret != null)
            {
                if (!caret.Line.IsVisible) scrollToLine(caret.Line);
                Position<TLine, TToken> fromP = caret;
                redo.Clear();
                removeCaret();
                document.Insert(tokens, fromP.Line, fromP.LinePos, UpdateAction.Undoable, true);
            }
        }

        /// <summary>
        /// Sets the color of the text between the two provided positions.
        /// </summary>
        /// <param name="start">Start position for the color change.</param>
        /// <param name="end">End position for the color change.</param>
        /// <param name="color">New text color.</param>
        public void SetColor(IPosition start, IPosition end, Color color)
        {
            if (start != null && start.ILine != null && end != null && end.ILine != null)
            {
                var ngE = end as Position<TLine, TToken>;
                var ngS = start as Position<TLine, TToken>;
                if (ngS != null && ngE != null)
                {
                    document.SetColor(color, ngS, ngE, UpdateAction.Undoable, true);
                }
            }
        }

        /// <summary>
        /// Returns the index of the given line inside the control (starting with 1 for the first line). If the passed
        /// line is not contained in the control the method returns -1.
        /// </summary>
        /// <param name="line">Line for which to get the index.</param>
        /// <returns>Index of the given line or -1 if it is not contained.</returns>
        public int GetLineNumber(TLine line)
        {
            return document.IndexOf(line);
        }

        /// <summary>
        /// This method returns the line number relative to the line number of the first visible line. The second
        /// parameter indicates the search direction (true == forward).
        /// NOTE: When calling this method ensure that the line is really contained before or after (depending on
        /// direction parameter) the current firstVisLine - otherwise the method will search through the whole
        /// document just to return -1!
        /// </summary>
        /// <param name="line">The line for which to get the index.</param>
        /// <param name="forward">Boolean value indicating search direction.</param>
        /// <returns>Index of the given line or -1 if the line wasn't found.</returns>
        public int GetLineNumberRelative(TLine line, bool forward)
        {
            TLine search = firstVisLine;
            int lineNum = FirstVisLineNumber;
            if (forward)
            {
                while (search != null && search != line)
                {
                    search = search.Next;
                    lineNum++;
                }
            }
            else
            {
                while (search != null && search != line)
                {
                    search = search.Prev;
                    lineNum--;
                }
            }
            if (search == null) return -1;
            return lineNum;
        }

        /// <summary>
        /// Returns the line with the given line number (line numbers start at 1 for the FirstLine). If there 
        /// is no line at the given index the method returns null.
        /// NOTE: This methods runtime complexity is linear (O(n) = n) so it should be used with care when
        /// a ColorTextBox is used to hold long texts!
        /// </summary>
        /// <param name="index">Number of line to retrieve.</param>
        /// <returns>The line with the given number or null if it doesn't exist.</returns>
        public TLine GetLineAt(int index)
        {
            try
            {
                return document[index];
            }
            catch (IndexOutOfRangeException)
            {
                return null;
            }
        }

        /// <summary>
        /// Returns the line with the given line number (line numbers start at 1 for the FirstLine). If there is
        /// no line at the given index the method returns null.
        /// This method searches for the line at the given index starting at the currently first visible line!
        /// NOTE: This method exists for performance reasons only and should be used when the index to search for
        /// is nearer to the index of the first visible line than to the index of the first or last line of the 
        /// document (in those cases the GetLineAt(int index) method is faster!)
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public TLine GetLineAtRelative(int index)
        {
            return document.LineRelativeTo(firstVisLine, FirstVisLineNumber, index);
        }

        /// <summary>
        /// Returns the line nearest to the given y position. The passed y coordinate is set to the baseline
        /// of the returned line. If the passed y coordinate lies above or below the viewable area the line before
        /// the first visible line or after the last visible line will be returned (if they exist!)
        /// </summary>
        /// <param name="y">Y position of the line.</param>
        /// <returns>The line nearest to the given y position or null if there aren't any visible lines.
        /// </returns>
        public virtual TLine GetLineAtYPos(ref int y)
        {
            TLine curr = firstVisLine;
            if (curr != null)
            {
                if (y < firstVisLine.Y)
                {
                    if (firstVisLine.Prev != null)
                    {
                        y = firstVisLine.Y - LineHeight + BaseLineDistance;
                        return firstVisLine.Prev;
                    }
                    else
                    {
                        y = firstVisLine.Y + BaseLineDistance;
                        return firstVisLine;
                    }
                }
                if (y > lastVisLine.Y + lastVisLine.Height)
                {
                    if (lastVisLine.Next != null)
                    {
                        y = lastVisLine.Y + LineHeight + BaseLineDistance;
                        return lastVisLine.Next;
                    }
                    else
                    {
                        y = lastVisLine.Y + BaseLineDistance;
                        return lastVisLine;
                    }
                }
                TLine last = curr;
                while (curr != null && y >= curr.Y + curr.Height)
                {
                    last = curr;
                    curr = curr.Next;
                }
                if (curr != null)
                {
                    y = curr.Y + BaseLineDistance;
                    return curr;
                }
                else
                {
                    y = last.Y + BaseLineDistance;
                    return last;
                }
            }
            return null;
        }

        /// <summary>
        /// Returns the Line at the given text position and sets the output parameter startPos to the start
        /// text position of the returned line.
        /// NOTE: This methods runtime complexity is linear (O(tpos) = tpos) so it should be used with care when
        /// a ColorTextBox is used to hold long texts!
        /// </summary>
        /// <param name="tpos">Text position to search for.</param>
        /// <param name="startPos">Start text postition of the returned line.</param>
        /// <returns>The line at the given text position. Null if it doesn't exist.</returns>
        public TLine GetLineAtTxPos(int tpos, out int startPos)
        {
            if (tpos < 0) tpos = 0;
            TLine curr = document.First;
            int currPos = startPos = 0;
            if (curr != null) currPos = curr.LastPos;
            while (curr != null && tpos > currPos)
            {
                startPos = currPos;
                currPos += curr.LastPos + 1;
                curr = curr.Next;
            }
            if (curr != null) return curr;
            return null;
        }

        /// <summary>
        /// TODO: REMOVE DEBUG METHOD!
        /// </summary>
        /// <returns></returns>
        public virtual String debugCurrLine()
        {
            var b = new StringBuilder(String.Empty);
            if (caret != null)
            {
                b.Append("\nCURRENT LINE IS NUMBER: " + GetLineNumber(caret.Line));
                TToken t = caret.Line.First;
                int i = 1;
                b.Append("\n");
                while (t != null)
                {
                    b.Append("Token " + i + ": \"" + t.Value + "\"\n");
                    i++;
                    t = t.Next;
                }
                if (caret.Line.Last != null) b.Append("TAIL SET TO: \"" + caret.Line.Last.Value + "\"\n");
                b.Append("VISIBLE: " + caret.Line.IsVisible + "\n");
                b.Append("FIRSTVISIBLE: " + (caret.Line == firstVisLine) + "\n");
                b.Append("LASTVISIBLE: " + (caret.Line == lastVisLine) + "\n");
            }
            return b.ToString();
        }

        /// <summary>
        /// TODO: REMOVE DEBUG METHOD!
        /// </summary>
        /// <returns></returns>
        public virtual String debugSelection()
        {
            var b = new StringBuilder();
            if (selection != null)
            {
                b.Append("VALID SELECTION FROM:\nLine " + GetLineNumber(selection.From.Line) + " Pos " +
                         selection.From.LinePos);
                b.Append("\nTO:\nLine " + GetLineNumber(selection.To.Line) + " Pos " + selection.To.LinePos);
                b.Append("\nSELECTION TEXT: \n" + document.SubString(selection.From, selection.To));
            }
            return b.ToString();
        }

        /// <summary>
        /// TODO: REMOVE DEBUG METHOD!
        /// </summary>
        /// <returns></returns>
        public virtual String debugVScroll()
        {
            return "\nVSCROLL - MIN: " + VScroll.Minimum + " MAX: " + VScroll.Maximum + " VALUE: " + VScroll.Value;
        }

        #region Nested type: DrawMode

        /// <summary>
        /// Enumeration which determines which parts should be redrawn when calling the drawLines(...) method.
        /// </summary>
        protected enum DrawMode
        {
            All, // redraws all lines and reserved areas
            Caret, // redraws only the line (piece) occupied by caret
            Lines, // redraws all lines but no restricted areas
            Reserved // redraws only reserved areas
        }

        #endregion

        #region Nested type: ReservedArea

        /// <summary>
        /// Represents an area at one of the edges of the Control on which no text rendering is allowed.
        /// Instead a draw method and a mouse event handler method can be supplied that allow
        /// for user defined drawing and event handling in the specified area.
        /// </summary>
        protected class ReservedArea
        {
            #region Delegates

            public delegate bool DrawArea(Graphics g);

            public delegate bool HandleMouseEvent(MouseEventArgs args, MouseEventType type);

            #endregion

            private readonly String id;

            private DrawArea drawArea;
            private HandleMouseEvent handleMouseEvent;
            private int width;

            /// <summary>
            /// Constructor for a new reserved area with the provided parameters set.
            /// </summary>
            /// <param name="id">A unique ID for the area at the given location</param>
            /// <param name="width">Width of the reserved area (corresponds to the height for areas
            /// added to the top or bottom of a ColorTextBox)</param>
            /// <param name="drawMeth">Delegate to be called when area needs to be redrawn.</param>
            /// <param name="eventMeth">Event handling delegate for mouse events occuring inside the area.</param>
            /// <param name="hoverCursor">Cursor to show when mouse is being hovered over the area.</param>
            public ReservedArea(String id, int width, DrawArea drawMeth,
                                HandleMouseEvent eventMeth, Cursor hoverCursor)
            {
                this.id = id;
                this.width = width;
                drawArea = drawMeth;
                handleMouseEvent = eventMeth;
                if (hoverCursor != null) HoverCursor = hoverCursor;
                else HoverCursor = Cursors.Default;
            }

            public int Width
            {
                get { return width; }
                set { if (value >= 0) width = value; }
            }

            public String Id
            {
                get { return id; }
            }

            public Cursor HoverCursor { get; set; }

            public HandleMouseEvent MouseEventHandler
            {
                set { handleMouseEvent = value; }
            }

            public DrawArea DrawMethod
            {
                set { drawArea = value; }
            }

            public bool draw(Graphics g)
            {
                if (drawArea != null) return drawArea(g);
                return false;
            }

            public bool handleMouse(MouseEventArgs args, MouseEventType type)
            {
                if (handleMouseEvent != null) return handleMouseEvent(args, type);
                return false;
            }
        }

        #endregion

        #region Nested type: ReservedLocation

        /// <summary>
        /// Determines the position of a reserved area inside the client area
        /// </summary>
        protected enum ReservedLocation
        {
            Left = 0,
            Right = 1,
            Top = 2,
            Bottom = 3
        }

        #endregion
    }
}