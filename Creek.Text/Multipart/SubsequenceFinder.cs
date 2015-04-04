namespace Creek.Text.Multipart
{
    /// <summary>
    ///     Provides methods to find a subsequence within a
    ///     sequence.
    /// </summary>
    internal class SubsequenceFinder
    {
        #region Public Methods and Operators

        /// <summary>
        /// Finds if a sequence exists within another sequence.
        /// </summary>
        /// <remarks>
        /// This is implemented using the
        ///     <see href="http://en.wikipedia.org/wiki/Knuth%E2%80%93Morris%E2%80%93Pratt_algorithm">
        ///         Knuth-Morris-Pratt
        ///     </see>
        ///     substring algorithm.
        /// </remarks>
        /// <param name="haystack">
        /// The sequence to search
        /// </param>
        /// <param name="needle">
        /// The sequence to look for
        /// </param>
        /// <returns>
        /// The start position of the found sequence or -1 if nothing was found
        /// </returns>
        public static int Search(byte[] haystack, byte[] needle)
        {
            // Special case for size 1 needle.
            if (needle.Length == 1)
            {
                for (int index = 0; index < haystack.Length; ++index)
                {
                    if (haystack[index] == needle[0])
                    {
                        return index;
                    }
                }

                return -1;
            }

            int m = 0;
            int i = 0;
            int[] table = GenerateTable(needle);

            while (m + i < haystack.Length)
            {
                if (needle[i] == haystack[m + i])
                {
                    if (i == (needle.Length - 1))
                    {
                        return m;
                    }

                    i += 1;
                }
                else
                {
                    m = m + i - table[i];
                    i = table[i] > -1 ? table[i] : 0;
                }
            }

            // No matches
            return -1;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Generates a table that is used in the Knuth-Morris-Pratt substring algorithm
        /// </summary>
        /// <param name="needle">
        /// The search subsequence to generate a table from
        /// </param>
        /// <returns>
        /// The generated search table
        /// </returns>
        /// <see cref="Search"/>
        private static int[] GenerateTable(byte[] needle)
        {
            var table = new int[needle.Length];
            int pos = 2;
            int cnd = 0;

            table[0] = -1;
            table[1] = 0;

            while (pos < needle.Length)
            {
                if (needle[pos - 1] == needle[cnd])
                {
                    cnd = cnd + 1;
                    table[pos] = cnd;
                    pos = pos + 1;
                }
                else if (cnd > 0)
                {
                    cnd = table[cnd];
                }
                else
                {
                    table[pos] = 0;
                    pos = pos + 1;
                }
            }

            return table;
        }

        #endregion
    }
}