/******************************************************************/
/*****                                                        *****/
/*****     Project:           Adobe Color Picker Clone 1      *****/
/*****     Filename:          ctrlVerticalColorSlider.cs      *****/
/*****     Original Author:   Danny Blanchard                 *****/
/*****                        - scrabcakes@gmail.com          *****/
/*****     Updates:	                                          *****/
/*****      3/28/2005 - Initial Version : Danny Blanchard     *****/
/*****                                                        *****/
/******************************************************************/

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Creek.UI.Unity3.Controls
{
    /// <summary>
    /// A vertical slider control that shows a range for a color property (a.k.a. Hue, Saturation, Brightness,
    /// Red, Green, Blue) and sends an event when the slider is changed.
    /// </summary>
    public class VerticalColorSlider : UserControl
    {
        #region Class Variables

        public enum eDrawStyle
        {
            Hue,
            Saturation,
            Brightness,
            Red,
            Green,
            Blue
        }

        private readonly Container components = null;


        //	Slider properties
        private bool m_bDragging;

        //	These variables keep track of how to fill in the content inside the box;
        private eDrawStyle m_eDrawStyle = eDrawStyle.Hue;
        private ColorManager.HSL m_hsl;
        private int m_iMarker_Start_Y;
        private Color m_rgb;

        #endregion

        #region Constructors / Destructors

        public VerticalColorSlider()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            //	Initialize Colors
            m_hsl = new ColorManager.HSL();
            m_hsl.H = 1.0;
            m_hsl.S = 1.0;
            m_hsl.L = 1.0;
            m_rgb = ColorManager.HSL_to_RGB(m_hsl);
            m_eDrawStyle = eDrawStyle.Hue;
        }


        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #endregion

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            // 
            // ctrl1DColorBar
            // 
            this.Name = "ctrl1DColorBar";
            this.Size = new System.Drawing.Size(40, 264);
            this.Resize += new System.EventHandler(this.ctrl1DColorBar_Resize);
            this.Load += new System.EventHandler(this.ctrl1DColorBar_Load);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ctrl1DColorBar_MouseUp);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.ctrl1DColorBar_Paint);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ctrl1DColorBar_MouseMove);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ctrl1DColorBar_MouseDown);
        }

        #endregion

        #region Control Events

        private void ctrl1DColorBar_Load(object sender, EventArgs e)
        {
            Redraw_Control();
        }


        private void ctrl1DColorBar_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) //	Only respond to left mouse button events
                return;

            m_bDragging = true; //	Begin dragging which notifies MouseMove function that it needs to update the marker

            int y;
            y = e.Y;
            y -= 4; //	Calculate slider position
            if (y < 0) y = 0;
            if (y > Height - 9) y = Height - 9;

            if (y == m_iMarker_Start_Y) //	If the slider hasn't moved, no need to redraw it.
                return; //	or send a scroll notification

            DrawSlider(y, false); //	Redraw the slider
            ResetHSLRGB(); //	Reset the color

            if (Scroll != null) //	Notify anyone who cares that the controls slider(color) has changed
                Scroll(this, e);
        }


        private void ctrl1DColorBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (!m_bDragging) //	Only respond when the mouse is dragging the marker.
                return;

            int y;
            y = e.Y;
            y -= 4; //	Calculate slider position
            if (y < 0) y = 0;
            if (y > Height - 9) y = Height - 9;

            if (y == m_iMarker_Start_Y) //	If the slider hasn't moved, no need to redraw it.
                return; //	or send a scroll notification

            DrawSlider(y, false); //	Redraw the slider
            ResetHSLRGB(); //	Reset the color

            if (Scroll != null) //	Notify anyone who cares that the controls slider(color) has changed
                Scroll(this, e);
        }


        private void ctrl1DColorBar_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) //	Only respond to left mouse button events
                return;

            m_bDragging = false;

            int y;
            y = e.Y;
            y -= 4; //	Calculate slider position
            if (y < 0) y = 0;
            if (y > Height - 9) y = Height - 9;

            if (y == m_iMarker_Start_Y) //	If the slider hasn't moved, no need to redraw it.
                return; //	or send a scroll notification

            DrawSlider(y, false); //	Redraw the slider
            ResetHSLRGB(); //	Reset the color

            if (Scroll != null) //	Notify anyone who cares that the controls slider(color) has changed
                Scroll(this, e);
        }


        private void ctrl1DColorBar_Paint(object sender, PaintEventArgs e)
        {
            Redraw_Control();
        }


        private void ctrl1DColorBar_Resize(object sender, EventArgs e)
        {
            Redraw_Control();
        }

        #endregion

        #region Events

        public new event EventHandler Scroll;

        #endregion

        #region Public Methods

        /// <summary>
        /// The drawstyle of the contol (Hue, Saturation, Brightness, Red, Green or Blue)
        /// </summary>
        public eDrawStyle DrawStyle
        {
            get { return m_eDrawStyle; }
            set
            {
                m_eDrawStyle = value;

                //	Redraw the control based on the new eDrawStyle
                Reset_Slider(true);
                Redraw_Control();
            }
        }


        /// <summary>
        /// The HSL color of the control, changing the HSL will automatically change the RGB color for the control.
        /// </summary>
        public ColorManager.HSL HSL
        {
            get { return m_hsl; }
            set
            {
                m_hsl = value;
                m_rgb = ColorManager.HSL_to_RGB(m_hsl);

                //	Redraw the control based on the new color.
                Reset_Slider(true);
                DrawContent();
            }
        }


        /// <summary>
        /// The RGB color of the control, changing the RGB will automatically change the HSL color for the control.
        /// </summary>
        public Color RGB
        {
            get { return m_rgb; }
            set
            {
                m_rgb = value;
                m_hsl = ColorManager.RGB_to_HSL(m_rgb);

                //	Redraw the control based on the new color.
                Reset_Slider(true);
                DrawContent();
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Redraws the background over the slider area on both sides of the control
        /// </summary>
        private void ClearSlider()
        {
            Graphics g = CreateGraphics();
            Brush brush = SystemBrushes.Control;
            g.FillRectangle(brush, 0, 0, 8, Height); //	clear left hand slider
            g.FillRectangle(brush, Width - 8, 0, 8, Height); //	clear right hand slider
        }


        /// <summary>
        /// Draws the slider arrows on both sides of the control.
        /// </summary>
        /// <param name="position">position value of the slider, lowest being at the bottom.  The range
        /// is between 0 and the controls height-9.  The values will be adjusted if too large/small</param>
        /// <param name="Unconditional">If Unconditional is true, the slider is drawn, otherwise some logic 
        /// is performed to determine is drawing is really neccessary.</param>
        private void DrawSlider(int position, bool Unconditional)
        {
            if (position < 0) position = 0;
            if (position > Height - 9) position = Height - 9;

            if (m_iMarker_Start_Y == position && !Unconditional) //	If the marker position hasn't changed
                return; //	since the last time it was drawn and we don't HAVE to redraw
            //	then exit procedure

            m_iMarker_Start_Y = position; //	Update the controls marker position

            ClearSlider(); //	Remove old slider

            Graphics g = CreateGraphics();

            var pencil = new Pen(Color.FromArgb(116, 114, 106)); //	Same gray color Photoshop uses
            Brush brush = Brushes.White;

            var arrow = new Point[7]; //	 GGG
            arrow[0] = new Point(1, position); //	G   G
            arrow[1] = new Point(3, position); //	G    G
            arrow[2] = new Point(7, position + 4); //	G     G
            arrow[3] = new Point(3, position + 8); //	G      G
            arrow[4] = new Point(1, position + 8); //	G     G
            arrow[5] = new Point(0, position + 7); //	G    G
            arrow[6] = new Point(0, position + 1); //	G   G
            //	 GGG

            g.FillPolygon(brush, arrow); //	Fill left arrow with white
            g.DrawPolygon(pencil, arrow); //	Draw left arrow border with gray

            //	    GGG
            arrow[0] = new Point(Width - 2, position); //	   G   G
            arrow[1] = new Point(Width - 4, position); //	  G    G
            arrow[2] = new Point(Width - 8, position + 4); //	 G     G
            arrow[3] = new Point(Width - 4, position + 8); //	G      G
            arrow[4] = new Point(Width - 2, position + 8); //	 G     G
            arrow[5] = new Point(Width - 1, position + 7); //	  G    G
            arrow[6] = new Point(Width - 1, position + 1); //	   G   G
            //	    GGG

            g.FillPolygon(brush, arrow); //	Fill right arrow with white
            g.DrawPolygon(pencil, arrow); //	Draw right arrow border with gray
        }


        /// <summary>
        /// Draws the border around the control, in this case the border around the content area between
        /// the slider arrows.
        /// </summary>
        private void DrawBorder()
        {
            Graphics g = CreateGraphics();

            Pen pencil;

            //	To make the control look like Adobe Photoshop's the border around the control will be a gray line
            //	on the top and left side, a white line on the bottom and right side, and a black rectangle (line) 
            //	inside the gray/white rectangle

            pencil = new Pen(Color.FromArgb(172, 168, 153)); //	The same gray color used by Photoshop
            g.DrawLine(pencil, Width - 10, 2, 9, 2); //	Draw top line
            g.DrawLine(pencil, 9, 2, 9, Height - 4); //	Draw left hand line

            pencil = new Pen(Color.White);
            g.DrawLine(pencil, Width - 9, 2, Width - 9, Height - 3); //	Draw right hand line
            g.DrawLine(pencil, Width - 9, Height - 3, 9, Height - 3); //	Draw bottome line

            pencil = new Pen(Color.Black);
            g.DrawRectangle(pencil, 10, 3, Width - 20, Height - 7); //	Draw inner black rectangle
        }


        /// <summary>
        /// Evaluates the DrawStyle of the control and calls the appropriate
        /// drawing function for content
        /// </summary>
        private void DrawContent()
        {
            switch (m_eDrawStyle)
            {
                case eDrawStyle.Hue:
                    Draw_Style_Hue();
                    break;
                case eDrawStyle.Saturation:
                    Draw_Style_Saturation();
                    break;
                case eDrawStyle.Brightness:
                    Draw_Style_Luminance();
                    break;
                case eDrawStyle.Red:
                    Draw_Style_Red();
                    break;
                case eDrawStyle.Green:
                    Draw_Style_Green();
                    break;
                case eDrawStyle.Blue:
                    Draw_Style_Blue();
                    break;
            }
        }

        /// <summary>
        /// Calls all the functions neccessary to redraw the entire control.
        /// </summary>
        private void Redraw_Control()
        {
            DrawSlider(m_iMarker_Start_Y, true);
            DrawBorder();
            switch (m_eDrawStyle)
            {
                case eDrawStyle.Hue:
                    Draw_Style_Hue();
                    break;
                case eDrawStyle.Saturation:
                    Draw_Style_Saturation();
                    break;
                case eDrawStyle.Brightness:
                    Draw_Style_Luminance();
                    break;
                case eDrawStyle.Red:
                    Draw_Style_Red();
                    break;
                case eDrawStyle.Green:
                    Draw_Style_Green();
                    break;
                case eDrawStyle.Blue:
                    Draw_Style_Blue();
                    break;
            }
        }


        /// <summary>
        /// Resets the vertical position of the slider to match the controls color.  Gives the option of redrawing the slider.
        /// </summary>
        /// <param name="Redraw">Set to true if you want the function to redraw the slider after determining the best position</param>
        private void Reset_Slider(bool Redraw)
        {
            //	The position of the marker (slider) changes based on the current drawstyle:
            switch (m_eDrawStyle)
            {
                case eDrawStyle.Hue:
                    m_iMarker_Start_Y = (Height - 8) - Round((Height - 8)*m_hsl.H);
                    break;
                case eDrawStyle.Saturation:
                    m_iMarker_Start_Y = (Height - 8) - Round((Height - 8)*m_hsl.S);
                    break;
                case eDrawStyle.Brightness:
                    m_iMarker_Start_Y = (Height - 8) - Round((Height - 8)*m_hsl.L);
                    break;
                case eDrawStyle.Red:
                    m_iMarker_Start_Y = (Height - 8) - Round((Height - 8)*(double) m_rgb.R/255);
                    break;
                case eDrawStyle.Green:
                    m_iMarker_Start_Y = (Height - 8) - Round((Height - 8)*(double) m_rgb.G/255);
                    break;
                case eDrawStyle.Blue:
                    m_iMarker_Start_Y = (Height - 8) - Round((Height - 8)*(double) m_rgb.B/255);
                    break;
            }

            if (Redraw)
                DrawSlider(m_iMarker_Start_Y, true);
        }


        /// <summary>
        /// Resets the controls color (both HSL and RGB variables) based on the current slider position
        /// </summary>
        private void ResetHSLRGB()
        {
            switch (m_eDrawStyle)
            {
                case eDrawStyle.Hue:
                    m_hsl.H = 1.0 - (double) m_iMarker_Start_Y/(Height - 9);
                    m_rgb = ColorManager.HSL_to_RGB(m_hsl);
                    break;
                case eDrawStyle.Saturation:
                    m_hsl.S = 1.0 - (double) m_iMarker_Start_Y/(Height - 9);
                    m_rgb = ColorManager.HSL_to_RGB(m_hsl);
                    break;
                case eDrawStyle.Brightness:
                    m_hsl.L = 1.0 - (double) m_iMarker_Start_Y/(Height - 9);
                    m_rgb = ColorManager.HSL_to_RGB(m_hsl);
                    break;
                case eDrawStyle.Red:
                    m_rgb = Color.FromArgb(255 - Round(255*(double) m_iMarker_Start_Y/(Height - 9)), m_rgb.G, m_rgb.B);
                    m_hsl = ColorManager.RGB_to_HSL(m_rgb);
                    break;
                case eDrawStyle.Green:
                    m_rgb = Color.FromArgb(m_rgb.R, 255 - Round(255*(double) m_iMarker_Start_Y/(Height - 9)), m_rgb.B);
                    m_hsl = ColorManager.RGB_to_HSL(m_rgb);
                    break;
                case eDrawStyle.Blue:
                    m_rgb = Color.FromArgb(m_rgb.R, m_rgb.G, 255 - Round(255*(double) m_iMarker_Start_Y/(Height - 9)));
                    m_hsl = ColorManager.RGB_to_HSL(m_rgb);
                    break;
            }
        }


        /// <summary>
        /// Kindof self explanitory, I really need to look up the .NET function that does
        /// </summary>
        /// <param name="val">double value to be rounded to an integer</param>
        /// <returns></returns>
        private int Round(double val)
        {
            var ret_val = (int) val;

            var temp = (int) (val*100);

            if ((temp%100) >= 50)
                ret_val += 1;

            return ret_val;
        }

        #region Draw_Style_X - Content drawing functions

        //	The following functions do the real work of the control, drawing the primary content (the area between the slider)
        //	

        /// <summary>
        /// Fills in the content of the control showing all values of Hue (from 0 to 360)
        /// </summary>
        private void Draw_Style_Hue()
        {
            Graphics g = CreateGraphics();

            var _hsl = new ColorManager.HSL();
            _hsl.S = 1.0; //	S and L will both be at 100% for this DrawStyle
            _hsl.L = 1.0;

            for (int i = 0; i < Height - 8; i++) //	i represents the current line of pixels we want to draw horizontally
            {
                _hsl.H = 1.0 - (double) i/(Height - 8); //	H (hue) is based on the current vertical position
                var pen = new Pen(ColorManager.HSL_to_RGB(_hsl)); //	Get the Color for this line

                g.DrawLine(pen, 11, i + 4, Width - 11, i + 4); //	Draw the line and loop back for next line
            }
        }


        /// <summary>
        /// Fills in the content of the control showing all values of Saturation (0 to 100%) for the given
        /// Hue and Luminance.
        /// </summary>
        private void Draw_Style_Saturation()
        {
            Graphics g = CreateGraphics();

            var _hsl = new ColorManager.HSL();
            _hsl.H = m_hsl.H; //	Use the H and L values of the current color (m_hsl)
            _hsl.L = m_hsl.L;

            for (int i = 0; i < Height - 8; i++) //	i represents the current line of pixels we want to draw horizontally
            {
                _hsl.S = 1.0 - (double) i/(Height - 8); //	S (Saturation) is based on the current vertical position
                var pen = new Pen(ColorManager.HSL_to_RGB(_hsl)); //	Get the Color for this line

                g.DrawLine(pen, 11, i + 4, Width - 11, i + 4); //	Draw the line and loop back for next line
            }
        }


        /// <summary>
        /// Fills in the content of the control showing all values of Luminance (0 to 100%) for the given
        /// Hue and Saturation.
        /// </summary>
        private void Draw_Style_Luminance()
        {
            Graphics g = CreateGraphics();

            var _hsl = new ColorManager.HSL();
            _hsl.H = m_hsl.H; //	Use the H and S values of the current color (m_hsl)
            _hsl.S = m_hsl.S;

            for (int i = 0; i < Height - 8; i++) //	i represents the current line of pixels we want to draw horizontally
            {
                _hsl.L = 1.0 - (double) i/(Height - 8); //	L (Luminance) is based on the current vertical position
                var pen = new Pen(ColorManager.HSL_to_RGB(_hsl)); //	Get the Color for this line

                g.DrawLine(pen, 11, i + 4, Width - 11, i + 4); //	Draw the line and loop back for next line
            }
        }


        /// <summary>
        /// Fills in the content of the control showing all values of Red (0 to 255) for the given
        /// Green and Blue.
        /// </summary>
        private void Draw_Style_Red()
        {
            Graphics g = CreateGraphics();

            for (int i = 0; i < Height - 8; i++) //	i represents the current line of pixels we want to draw horizontally
            {
                int red = 255 - Round(255*(double) i/(Height - 8)); //	red is based on the current vertical position
                var pen = new Pen(Color.FromArgb(red, m_rgb.G, m_rgb.B)); //	Get the Color for this line

                g.DrawLine(pen, 11, i + 4, Width - 11, i + 4); //	Draw the line and loop back for next line
            }
        }


        /// <summary>
        /// Fills in the content of the control showing all values of Green (0 to 255) for the given
        /// Red and Blue.
        /// </summary>
        private void Draw_Style_Green()
        {
            Graphics g = CreateGraphics();

            for (int i = 0; i < Height - 8; i++) //	i represents the current line of pixels we want to draw horizontally
            {
                int green = 255 - Round(255*(double) i/(Height - 8)); //	green is based on the current vertical position
                var pen = new Pen(Color.FromArgb(m_rgb.R, green, m_rgb.B)); //	Get the Color for this line

                g.DrawLine(pen, 11, i + 4, Width - 11, i + 4); //	Draw the line and loop back for next line
            }
        }


        /// <summary>
        /// Fills in the content of the control showing all values of Blue (0 to 255) for the given
        /// Red and Green.
        /// </summary>
        private void Draw_Style_Blue()
        {
            Graphics g = CreateGraphics();

            for (int i = 0; i < Height - 8; i++) //	i represents the current line of pixels we want to draw horizontally
            {
                int blue = 255 - Round(255*(double) i/(Height - 8)); //	green is based on the current vertical position
                var pen = new Pen(Color.FromArgb(m_rgb.R, m_rgb.G, blue)); //	Get the Color for this line

                g.DrawLine(pen, 11, i + 4, Width - 11, i + 4); //	Draw the line and loop back for next line
            }
        }

        #endregion

        #endregion
    }
}