using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Creek.UI
{
    /// <summary>
    /// A FrontPage style table dimensions picker.
    /// </summary>
    public class TablePicker : Form
    {
        private readonly Brush BeigeBrush = Brushes.Beige;
        private readonly Brush BlackBrush = Brushes.Black;
        private readonly Pen BluePen = new Pen(Color.SlateGray, 1);
        private readonly Pen BorderPen = new Pen(SystemColors.ControlDark);

        private readonly Font DispFont = new Font("Tahoma", 8.25F);
        private readonly Brush WhiteBrush = Brushes.White;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private readonly Container components = null;

        private Pen BeigePen = new Pen(Color.Beige, 1);
        private int DispHeight = 20; // Display ("Table 1x1", "Cancel")
        private string DispText = "Cancel"; // Display text
        private Brush GrayBrush = Brushes.Gray;

        private int SelQX = 1; // Number of selected squares (x)
        private int SelQY = 1; // Number of selected squares (y)
        private int SquareQX = 3; // Number of visible squares (X)
        private int SquareQY = 3; // Number of visible squares (Y)
        private int SquareX = 20; // Width of squares 
        private int SquareY = 20; // Height of squares

        private bool bCancel = true; // Determines whether to Cancel
        private bool bHiding;

        public TablePicker()
        {
            // Activates double buffering
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);

            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
        }

        /// <summary>
        /// Similar to <code><see cref="DialogResult"/> 
        /// == <see cref="DialogResult.Cancel"/></code>,
        /// but is used as a state value before the form
        /// is hidden and cancellation is finalized.
        /// </summary>
        public bool Cancel
        {
            get { return bCancel; }
        }

        /// <summary>
        /// Returns the number of columns, or the horizontal / X count,
        /// of the selection.
        /// </summary>
        public int SelectedColumns
        {
            get { return SelQX; }
        }

        /// <summary>
        /// Returns the number of rows, or the vertical / Y count, 
        /// of the selection.
        /// </summary>
        public int SelectedRows
        {
            get { return SelQY; }
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

        private void TablePicker_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            // First, increment the number of visible squares if the 
            // number of selected squares is equal to or greater than the
            // number of visible squares.
            if (SelQX > SquareQX - 1) SquareQX = SelQX + 1;
            if (SelQY > SquareQY - 1) SquareQY = SelQY + 1;

            // Second, expand the dimensions of this form according to the 
            // number of visible squares.
            Width = (SquareX*(SquareQX)) + 5;
            Height = (SquareY*(SquareQY)) + 6 + DispHeight;

            // Draw an outer rectangle for the border.
            g.DrawRectangle(BorderPen, 0, 0, Width - 1, Height - 1);

            // Draw the text to describe the selection. Note that since
            // the text is left-justified, only the Y (vertical) position
            // is calculated.
            int dispY = ((SquareY - 1)*SquareQY) + SquareQY + 4;
            if (Cancel)
            {
                DispText = "Cancel";
            }
            else
            {
                DispText = SelQX.ToString() + " by " + SelQY.ToString() + " Table";
            }
            g.DrawString(DispText, DispFont, BlackBrush, 3, dispY + 2);

            // Draw each of the squares and fill with the default color.
            for (int x = 0; x < SquareQX; x++)
            {
                for (int y = 0; y < SquareQY; y++)
                {
                    g.FillRectangle(WhiteBrush, (x*SquareX) + 3, (y*SquareY) + 3, SquareX - 2, SquareY - 2);
                    g.DrawRectangle(BorderPen, (x*SquareX) + 3, (y*SquareY) + 3, SquareX - 2, SquareY - 2);
                }
            }

            // Go back and paint the squares with selection colors.
            for (int x = 0; x < SelQX; x++)
            {
                for (int y = 0; y < SelQY; y++)
                {
                    g.FillRectangle(BeigeBrush, (x*SquareX) + 3, (y*SquareY) + 3, SquareX - 2, SquareY - 2);
                    g.DrawRectangle(BluePen, (x*SquareX) + 3, (y*SquareY) + 3, SquareX - 2, SquareY - 2);
                }
            }
        }

        /// <summary>
        /// Detect termination. Hides form.
        /// </summary>
        private void TablePicker_Deactivate(object sender, EventArgs e)
        {
            // bCancel = true 
            // and DialogResult = DialogResult.Cancel 
            // were previously already set in MouseLeave.

            Hide();
        }

        /// <summary>
        /// Detects mouse movement. Tracks table dimensions selection.
        /// </summary>
        private void TablePicker_MouseMove(object sender, MouseEventArgs e)
        {
            int sqx = (e.X/SquareX) + 1;
            int sqy = (e.Y/SquareY) + 1;
            bool changed = false;
            if (sqx != SelQX)
            {
                changed = true;
                SelQX = sqx;
            }
            if (sqy != SelQY)
            {
                changed = true;
                SelQY = sqy;
            }

            // Ask Windows to call the Paint event again.
            if (changed) Invalidate();
        }

        /// <summary>
        /// Detects mouse sudden exit from the form to indicate 
        /// escaped (canceling) state.
        /// </summary>
        private void TablePicker_MouseLeave(object sender, EventArgs e)
        {
            if (!bHiding) bCancel = true;
            DialogResult = DialogResult.Cancel;
            Invalidate();
        }

        /// <summary>
        /// Cancels the prior cancellation caused by MouseLeave.
        /// </summary>
        private void TablePicker_MouseEnter(object sender, EventArgs e)
        {
            bHiding = false;
            bCancel = false;
            DialogResult = DialogResult.OK;
            Invalidate();
        }

        /// <summary>
        /// Detects that the user made a selection by clicking.
        /// </summary>
        private void TablePicker_Click(object sender, EventArgs e)
        {
            bHiding = true; // Not the same as Visible == false
            // because bHiding suggests that the control
            // is still "active" (not canceled).
            Hide();
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            // 
            // TablePicker
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(304, 256);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "TablePicker";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "TablePicker";
            this.Click += new System.EventHandler(this.TablePicker_Click);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.TablePicker_Paint);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TablePicker_MouseMove);
            this.MouseEnter += new System.EventHandler(this.TablePicker_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.TablePicker_MouseLeave);
            this.Deactivate += new System.EventHandler(this.TablePicker_Deactivate);
        }

        #endregion
    }
}