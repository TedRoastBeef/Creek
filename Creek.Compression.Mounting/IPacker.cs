namespace Creek.Compression.Mounting
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// See <c>STGM</c> documents in MSDN.
    /// </summary>
    [Flags]
    public enum STGM : uint
    {
        /// <summary>See <c>STGM</c> documents in MSDN.</summary>
        DIRECT = 0x00000000,
        /// <summary>See <c>STGM</c> documents in MSDN.</summary>
        TRANSACTED = 0x00010000,
        /// <summary>See <c>STGM</c> documents in MSDN.</summary>
        SIMPLE = 0x08000000,
        /// <summary>See <c>STGM</c> documents in MSDN.</summary>
        READ = 0x00000000,
        /// <summary>See <c>STGM</c> documents in MSDN.</summary>
        WRITE = 0x00000001,
        /// <summary>See <c>STGM</c> documents in MSDN.</summary>
        READWRITE = 0x00000002,
        /// <summary>See <c>STGM</c> documents in MSDN.</summary>
        SHARE_DENY_NONE = 0x00000040,
        /// <summary>See <c>STGM</c> documents in MSDN.</summary>
        SHARE_DENY_READ = 0x00000030,
        /// <summary>See <c>STGM</c> documents in MSDN.</summary>
        SHARE_DENY_WRITE = 0x00000020,
        /// <summary>See <c>STGM</c> documents in MSDN.</summary>
        SHARE_EXCLUSIVE = 0x00000010,
        /// <summary>See <c>STGM</c> documents in MSDN.</summary>
        PRIORITY = 0x00040000,
        /// <summary>See <c>STGM</c> documents in MSDN.</summary>
        DELETEONRELEASE = 0x04000000,
        /// <summary>See <c>STGM</c> documents in MSDN.</summary>
        NOSCRATCH = 0x00100000,
        /// <summary>See <c>STGM</c> documents in MSDN.</summary>
        CREATE = 0x00001000,
        /// <summary>See <c>STGM</c> documents in MSDN.</summary>
        CONVERT = 0x00020000,
        /// <summary>See <c>STGM</c> documents in MSDN.</summary>
        FAILIFTHERE = 0x00000000,
        /// <summary>See <c>STGM</c> documents in MSDN.</summary>
        NOSNAPSHOT = 0x00200000,
        /// <summary>See <c>STGM</c> documents in MSDN.</summary>
        DIRECT_SWMR = 0x00400000,
    }
    ///<summary>Contain method for creating Shetab Mount Zip Library objects.</summary> 
    [ComImport]
    [Guid("6852A92F-2443-4ae7-A3C9-457F312EAD1B")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IPacker
    {
        /// <summary>
        /// Create an object of IBoxManager that can use for mounting zip file.
        /// </summary>
        /// <param name="pszTempFolder">Temporary folder for Shetab Mount Zip Library.
        /// NOTE: Shetab Mount Zip Library dose not export zip file in temporary folder or any part of disk.
        /// </param>
        /// <param name="ppboxManager">Return an IBoxManager object.</param>
        void CreateBoxManager(string pszTempFolder, out IBoxManager ppboxManager);
        /// <summary>
        /// Create an object of IZipStorage from exiting zip archive file.
        /// </summary>
        /// <param name="pszFile">Path to zip archive file, it can be exe or zip file.</param>
        /// <param name="pszPassword">Password of zip archive.</param>
        /// <param name="readOnly">If true archive will open as read-only.</param>
        /// <param name="ppzipStorage">Return reference to new zip storage.</param>
        void OpenZipStorage(string pszFile, string pszPassword, [MarshalAs(UnmanagedType.Bool)] bool readOnly, out IZipStorage ppzipStorage);
        /// <summary>
        /// Create zip archive file and return an object of IZipStorage.
        /// </summary>
        /// <param name="pszFile">Path to zip archive file.</param>
        /// <param name="pszPassword">Password of zip archive.</param>
        /// <param name="overwrite">if true and file already exists then the old file will be overwrite, if false and file already exists then an exception will be throw.</param>
        /// <param name="ppzipStorage"></param>
        void CreateZipStorage(string pszFile, string pszPassword, [MarshalAs(UnmanagedType.Bool)] bool overwrite, out IZipStorage ppzipStorage);
        /// <summary>
        /// Create zip archive file on exe file, usually used for self extracting programs.
        /// </summary>
        /// <param name="pszFile">Path to exe file.</param>
        /// <param name="pszPassword">Password of zip archive.</param>
        /// <param name="ppzipStorage">Return reference to new zip storage.</param>
        void CreateZipStorageOnExeFile(string pszFile, string pszPassword, out IZipStorage ppzipStorage);
    };
}
