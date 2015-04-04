namespace Creek.Database.Exceptions
{
    /// <summary>
    ///   An exception thrown by ODB when a corrupted block is found
    /// </summary>
    public sealed class CorruptedDatabaseException : OdbRuntimeException
    {
        internal CorruptedDatabaseException(IError error) : base(error)
        {
        }
    }
}
