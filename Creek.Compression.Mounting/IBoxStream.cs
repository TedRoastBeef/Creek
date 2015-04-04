namespace Creek.Compression.Mounting
{
    using System;
    using System.Runtime.InteropServices;
    using System.Runtime.InteropServices.ComTypes;

    /// <summary>
    /// The <c>IBoxStream</c> interface lets you read and write data to stream objects.
    /// Shetab Mount Zip Library mount any object that implement this interface.
    /// Shetab Mount Zip library already implement this interface with zip functionality in IZipStream.
    /// The base interface is IStream, Please read IStream MSDN for more information.
    /// </summary>
    [ComImport]
    [Guid("89DF0C91-2BA4-4a90-ADFC-9C78723F6E5D")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IBoxStream : IStream
	{
        #region IStream Methods
        /// <summary>
        /// See IStream.Clone documents in MSDN.
        /// Note: Not implemented in Shetab Mount Zip Library.
        /// </summary>
        new void Clone(out IStream ppstm);
        /// <summary>
        /// See IStream.Commit documents in MSDN.
        /// Note: Not implemented in Shetab Mount Zip Library.
        /// </summary>
        new void Commit(int grfCommitFlags);
        /// <summary>
        /// See IStream.CopyTo documents in MSDN.
        /// Note: Not implemented in Shetab Mount Zip Library.
        /// </summary>
        new void CopyTo(IStream pstm, long cb, IntPtr pcbRead, IntPtr pcbWritten);
        /// <summary>
        /// See IStream.LockRegion documents in MSDN.
        /// Note: Not implemented in Shetab Mount Zip Library.
        /// </summary>
        new void LockRegion(long libOffset, long cb, int dwLockType);
        /// <summary>
        /// See IStream.Read documents in MSDN.
        /// Note: Not implemented in Shetab Mount Zip Library.
        /// </summary>
        new void Read(byte[] pv, int cb, IntPtr pcbRead);
        /// <summary>
        /// See IStream.Revert documents in MSDN.
        /// Note: Not implemented in Shetab Mount Zip Library.
        /// </summary>
        new void Revert();
        /// <summary>
        /// See IStream.Seek documents in MSDN.
        /// Note: Not implemented in Shetab Mount Zip Library.
        /// </summary>
        new void Seek(long dlibMove, int dwOrigin, IntPtr plibNewPosition);
        /// <summary>
        /// See IStream.SetSize documents in MSDN.
        /// </summary>
        new void SetSize(long libNewSize);
        /// <summary>
        /// See IStream.Stat documents in MSDN.
        /// Note: Not implemented in Shetab Mount Zip Library.
        /// </summary>
        new void Stat(out System.Runtime.InteropServices.ComTypes.STATSTG pstatstg, int grfStatFlag);
        /// <summary>
        /// See IStream.UnlockRegion documents in MSDN.
        /// </summary>
        new void UnlockRegion(long libOffset, long cb, int dwLockType);
        /// <summary>
        /// See IStream.Write documents in MSDN.
        /// Note: Not implemented in Shetab Mount Zip Library.
        /// </summary>
        new void Write(byte[] pv, int cb, IntPtr pcbWritten); 
        #endregion

        #region IBoxStream Methods
	    /// <summary>
        /// Check can have fast random access to stream.
        /// Shetab Mount Zip library in IZipStream implementation can have random access to zip stream if zip stream no compressed and not encrypted with standard zip encryption. 
        /// With Shetab Mount Zip library you have random access to uncompressed stream with no encryption or any AES and internal Shetab encryption. Random access in compressed stream via deflate algorithm may be implemented in future.
        /// If stream return false for CanFastSeek then seeking in stream may be too slow and sometimes require to reading entire of file.
        /// </summary>
        /// <param name="pCanFastSeek">return true if fast seek (fast random access) is available in stream, otherwise false.</param>
		void CanFastSeek([MarshalAs(UnmanagedType.Bool)] out bool pCanFastSeek); 
        #endregion	
    };
}
