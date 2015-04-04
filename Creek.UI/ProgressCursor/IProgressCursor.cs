using System;
using System.Windows.Forms;

namespace Creek.UI.ProgressCursor
{
    public interface IProgressCursor
    {
        #region Properties

        Cursor CurrentCursor { get; }

        double Max { get; set; }

        double Current { get; set; }

        #endregion

        void End();

        IProgressCursor IncrementTo(double value);
        IProgressCursor IncrementWith(double value);

        event EventHandler<ProgressCursor.CursorPaintEventArgs> CustomDrawCursor;
    }
}