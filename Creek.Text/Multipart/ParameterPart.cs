namespace Creek.Text.Multipart
{
    /// <summary>
    ///     Represents a single parameter extracted from a multipart/form-data
    ///     stream.
    /// </summary>
    /// <remarks>
    ///     For our purposes a "parameter" is defined as any non-file data
    ///     in the multipart/form-data stream.
    /// </remarks>
    public class ParameterPart
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterPart"/> class.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="data">
        /// The data.
        /// </param>
        public ParameterPart(string name, string data)
        {
            this.Name = name;
            this.Data = data;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the data.
        /// </summary>
        public string Data { get; private set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        #endregion
    }
}