namespace Creek.Text
{
    using System;
    using System.Collections;

    public class ConsoleTable
    {
        #region Align enum

        public enum Align
        {
            Left,
            Right
        };

        #endregion

        private readonly Align CellAlignment = Align.Left;
        private readonly string[] headers;
        private readonly int tableYStart;

        /// <summary>
        /// The last line of the table (gotton from Console.CursorTop). -1 = No printed data
        /// </summary>
        public int LastPrintEnd = -1;

        /// <summary>
        /// Helps create a table
        /// </summary>
        /// <param name="TableStart">What line to start the table on.</param>
        /// <param name="Alignment">The alignment of each cell\'s text.</param>
        public ConsoleTable(int TableStart, Align Alignment, string[] headersi)
        {
            this.headers = headersi;
            this.CellAlignment = Alignment;
            this.tableYStart = TableStart;
        }

        public void ClearData()
        {
            //Clear Previous data
            if (this.LastPrintEnd != -1) //A set of data has already been printed
            {
                for (int i = this.tableYStart; i < this.LastPrintEnd; i++)
                {
                    this.ClearLine(i);
                }
            }
            this.LastPrintEnd = -1;
        }

        public void RePrint(ArrayList data)
        {
            //Set buffers
            if (data.Count > Console.BufferHeight)
                Console.BufferHeight = data.Count;
            //Clear Previous data
            this.ClearData();

            Console.CursorTop = this.tableYStart;
            Console.CursorLeft = 0;
            if (data.Count == 0)
            {
                Console.WriteLine("No Records");
                this.LastPrintEnd = Console.CursorTop;
                return;
            }

            //Get max lengths on each column
            int ComWidth = ((string[]) data[0]).Length*2 + 1;
            var ColumnLengths = new int[((string[]) data[0]).Length];

            foreach (string[] row in data)
            {
                for (int i = 0; i < row.Length; i++)
                {
                    if (row[i].Length > ColumnLengths[i])
                    {
                        ComWidth -= ColumnLengths[i];
                        ColumnLengths[i] = row[i].Length;
                        ComWidth += ColumnLengths[i];
                    }
                }
            }
            //Don't forget to check headers
            for (int i = 0; i < this.headers.Length; i++)
            {
                if (this.headers[i].Length > ColumnLengths[i])
                {
                    ComWidth -= ColumnLengths[i];
                    ColumnLengths[i] = this.headers[i].Length;
                    ComWidth += ColumnLengths[i];
                }
            }


            if (Console.BufferWidth < ComWidth)
                Console.BufferWidth = ComWidth + 1;
            this.PrintLine(ComWidth);
            //Print Data
            bool first = true;
            foreach (string[] row in data)
            {
                if (first)
                {
                    //Print Header
                    this.PrintRow(this.headers, ColumnLengths);
                    this.PrintLine(ComWidth);
                    first = false;
                }
                this.PrintRow(row, ColumnLengths);
            }
            this.PrintLine(ComWidth);
            this.LastPrintEnd = Console.CursorTop;
        }

        private void ClearLine(int line)
        {
            int oldtop = Console.CursorTop;
            Console.CursorTop = line;
            int oldleft = Console.CursorLeft;
            Console.CursorLeft = 0;
            int top = Console.CursorTop;

            while (Console.CursorTop == top)
            {
                Console.Write(" ");
            }
            Console.CursorLeft = oldleft;
            Console.CursorTop = oldtop;
        }

        private void PrintLine(int width)
        {
            Console.WriteLine(new string('-', width));
        }

        private void PrintRow(string[] row, int[] Widths)
        {
            string s = "|";
            for (int i = 0; i < row.Length; i++)
            {
                if (this.CellAlignment == Align.Left)
                    s += row[i] + new string(' ', Widths[i] - row[i].Length + 1) + "|";
                else if (this.CellAlignment == Align.Right)
                    s += new string(' ', Widths[i] - row[i].Length + 1) + row[i] + "|";
            }
            if (s == "|")
                throw new Exception("PrintRow input must not be empty");

            Console.WriteLine(s);
        }
    }
}