namespace Creek.UI.ProgressCursor
{
    public class CursorHelper
    {
        /// <summary>
        /// Starts the progress cursor.
        /// </summary>
        /// <param name="max">The max.</param>
        /// <returns></returns>
        public static IProgressCursor StartProgressCursor(double max = 1.0)
        {
            var progressCursor = new ProgressCursor {Max = max, Current = 0};
            return progressCursor;
        }
    }
}