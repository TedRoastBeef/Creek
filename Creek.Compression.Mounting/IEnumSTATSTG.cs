namespace Creek.Compression.Mounting
{
    using System.Runtime.InteropServices;

    /// <summary>See <c>IEnumSTATSTG</c> documents in MSDN.</summary>
    [ComImport]
    [Guid("0000000d-0000-0000-C000-000000000046")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IEnumSTATSTG
    {
        /// <summary>
        /// See IEnumSTATSTG::Next documents in MSDN.
        /// </summary>
        [PreserveSig]
        uint Next(uint celt, [MarshalAs(UnmanagedType.LPArray), Out] System.Runtime.InteropServices.ComTypes.STATSTG[] rgelt, out uint pceltFetched);
        /// <summary>
        /// See IEnumSTATSTG::Skip documents in MSDN.
        /// </summary>
        [PreserveSig]
        uint Skip(uint celt);
        /// <summary>
        /// See IEnumSTATSTG::Reset documents in MSDN.
        /// </summary>
        void Reset();
        /// <summary>
        /// See IEnumSTATSTG::Clone documents in MSDN.
        /// </summary>
        void Clone(out IEnumSTATSTG ppenum);
    }
}
