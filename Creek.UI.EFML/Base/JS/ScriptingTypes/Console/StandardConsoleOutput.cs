using System;
using System.Text;

namespace Creek.UI.EFML.Base.JS.ScriptingTypes.Console
{
    /// <summary>
    /// Represents an implementation of the Firebug API using the standard console.
    /// </summary>
    [Serializable]
    internal class StandardConsoleOutput : IFirebugConsoleOutput
    {
        private int currentIndentation;
        private int indentationDelta = 4;

        /// <summary>
        /// Gets or sets the number of spaces to output before writing any text to the console.
        /// </summary>
        public int CurrentIndentation
        {
            get { return currentIndentation; }
            set
            {
                if (value < 0 || value > 40)
                    throw new ArgumentOutOfRangeException("value");
                currentIndentation = value;
            }
        }

        /// <summary>
        /// Gets or sets the number of spaces to add to the identation when group() is called.
        /// </summary>
        public int IndentationDelta
        {
            get { return indentationDelta; }
            set
            {
                if (value < 0 || value > 40)
                    throw new ArgumentOutOfRangeException("value");
                indentationDelta = value;
            }
        }

        #region IFirebugConsoleOutput Members

        /// <summary>
        /// Logs a message to the console.
        /// </summary>
        /// <param name="style"> A style which influences the icon and text color. </param>
        /// <param name="objects"> The objects to output to the console. These can be strings or
        /// ObjectInstances. </param>
        public void Log(FirebugConsoleMessageStyle style, object[] objects)
        {
#if !SILVERLIGHT
            ConsoleColor original = System.Console.ForegroundColor;
            switch (style)
            {
                case FirebugConsoleMessageStyle.Information:
                    System.Console.ForegroundColor = ConsoleColor.White;
                    break;
                case FirebugConsoleMessageStyle.Warning:
                    System.Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case FirebugConsoleMessageStyle.Error:
                    System.Console.ForegroundColor = ConsoleColor.Red;
                    break;
            }
#endif

            // Convert the objects to a string.
            var message = new StringBuilder();
            for (int i = 0; i < objects.Length; i++)
            {
                message.Append(' ');
                message.Append((objects[i]));
            }

            // Output the message to the console.
            System.Console.WriteLine(message.ToString());


#if !SILVERLIGHT
            if (style != FirebugConsoleMessageStyle.Regular)
                System.Console.ForegroundColor = original;
#endif
        }

        /// <summary>
        /// Clears the console.
        /// </summary>
        public void Clear()
        {
#if !SILVERLIGHT
            System.Console.Clear();
#endif
        }

        /// <summary>
        /// Starts grouping messages together.
        /// </summary>
        /// <param name="title"> The title for the group. </param>
        /// <param name="initiallyCollapsed"> <c>true</c> if subsequent messages should be hidden by default. </param>
        public void StartGroup(string title, bool initiallyCollapsed)
        {
            Log(FirebugConsoleMessageStyle.Regular, new object[] {title});
            CurrentIndentation = System.Math.Min(CurrentIndentation + IndentationDelta, 40);
        }

        /// <summary>
        /// Ends the most recently started group.
        /// </summary>
        public void EndGroup()
        {
            CurrentIndentation = System.Math.Max(CurrentIndentation - IndentationDelta, 0);
        }

        #endregion
    }
}