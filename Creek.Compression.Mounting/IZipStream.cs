namespace Creek.Compression.Mounting
{
    using System;
    using System.Runtime.InteropServices;
    using System.Runtime.InteropServices.ComTypes;

    /// <summary>
    /// <c>IZipStream</c> derived from IBoxStream for support reading and writing data in zip stream object. 
    /// see IStream interface in MSDN.
    /// </summary>
    [ComImport]
    [Guid("14B5A91A-A28E-4b28-90C0-4EF3AABF7EAC")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IZipStream : IBoxStream
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
        /// </summary>
        new void Read(byte[] pv, int cb, IntPtr pcbRead);
        /// <summary>
        /// See IStream.Revert documents in MSDN.
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
        new void CanFastSeek([MarshalAs(UnmanagedType.Bool)] out bool pCanFastSeek);
        #endregion

        #region IZipStream Methods
        /// <summary>
        /// Retrieve current file position within the current stream.
        /// </summary>
        /// <param name="pCurrentPosition">return position within the current stream.</param>
        void GetCurrentPosition(out UInt64 pCurrentPosition);
        /// <summary>
        /// Retrieve size of compressed zip stream.
        /// </summary>
        /// <param name="pCompressedSize">return size of compressed zip stream.</param>
        void GetCompressedSize(out UInt64 pCompressedSize);
        /// <summary>
        /// Retrieve size of uncompressed zip stream.
        /// </summary>
        /// <param name="pUncompressedSize">return size of uncompressed zip stream.</param>
        void GetUncompressedSize(out UInt64 pUncompressedSize);
        /// <summary>
        /// Check is zip stream compressed
        /// </summary>
        /// <param name="pCompressed">true if zip stream compressed, otherwise false.</param>
        void IsCompressed([MarshalAs(UnmanagedType.Bool)] out bool pCompressed);
        /// <summary>
        /// Check is zip stream encrypted.
        /// </summary>
        /// <param name="pEncrypted">return true if zip stream encrypted, otherwise false.</param>
        void IsEncrypted([MarshalAs(UnmanagedType.Bool)] out bool pEncrypted); 
        #endregion
	};}
