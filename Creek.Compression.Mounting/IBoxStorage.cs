namespace Creek.Compression.Mounting
{
    using System;
    using System.Runtime.InteropServices;
    using System.Runtime.InteropServices.ComTypes;

    /// <summary>
    /// Status of path in box storage.
    /// </summary>
    public enum PathExist
    {
        /// <summary>the path does not exists.</summary>
        None,
        /// <summary>the path is point to stream (file).</summary>
        Stream,
        /// <summary>the path is point to storage (folder).</summary>
        Storage,
    }
    /// <summary>
    /// The <c>IBoxStorage</c> interface lets you read and write data to stream objects. it act as folder in structured storage.
    /// Shetab Mount Zip Library mount any object that implement this interface.
    /// Shetab Mount Zip library already implement this interface with zip functionality in IZipStorage.
    /// The base interface is IStorage, Please read IStream MSDN for more information.
    /// </summary>
    [ComImport]
    [Guid("15FEFF17-2CD5-4a2f-83E0-9B44168C833E")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IBoxStorage : IStorage
	{
        #region IStorage Methods
        /// <summary>
        /// See IStorage::CreateStream documents in MSDN.
        /// Note: Not implemented in Shetab Mount Zip Library.
        /// </summary>
        new void CreateStream(/* [string][in] */ string pwcsName, /* [in] */ [MarshalAs(UnmanagedType.U4)] STGM grfMode, /* [in] */ uint reserved1, /* [in] */ uint reserved2, /* [out] */ out IStream ppstm);
        /// <summary>
        /// See IStorage::OpenStream documents in MSDN.
        /// </summary>
        new void OpenStream(/* [string][in] */ string pwcsName, /* [unique][in] */ IntPtr reserved1, /* [in] */ [MarshalAs(UnmanagedType.U4)] STGM grfMode, /* [in] */ uint reserved2, /* [out] */ out IStream ppstm);
        /// <summary>
        /// See IStorage::CreateStorage documents in MSDN.
        /// </summary>
        new void CreateStorage( /* [string][in] */ string pwcsName, /* [in] */ [MarshalAs(UnmanagedType.U4)] STGM grfMode,/* [in] */ uint reserved1, /* [in] */ uint reserved2, /* [out] */ out IStorage ppstg);
        /// <summary>
        /// See IStorage::OpenStorage documents in MSDN.
        /// </summary>
        new void OpenStorage(/* [string][unique][in] */ string pwcsName, /* [unique][in] */ IStorage pstgPriority, /* [in] */ [MarshalAs(UnmanagedType.U4)] STGM grfMode, /* [unique][in] */ IntPtr snbExclude, /* [in] */ uint reserved, /* [out] */ out IStorage ppstg);
        /// <summary>
        /// See IStorage::CopyTo documents in MSDN.
        /// Note: Not implemented in Shetab Mount Zip Library.
        /// </summary>
        new void CopyTo(/* [in] */ uint ciidExclude, /* [size_is][unique][in] */ Guid rgiidExclude, /* [unique][in] */ IntPtr snbExclude, /* [unique][in] */ IStorage pstgDest);
        /// <summary>
        /// See IStorage::MoveElementTo documents in MSDN.
        /// Note: Not implemented in Shetab Mount Zip Library.
        /// </summary>
        new void MoveElementTo( /* [string][in] */ string pwcsName, /* [unique][in] */ IStorage pstgDest, /* [string][in] */ string pwcsNewName, /* [in] */ uint grfFlags);
        /// <summary>
        /// See IStorage::Commit documents in MSDN.
        /// </summary>
        new void Commit( /* [in] */ uint grfCommitFlags);
        /// <summary>
        /// See IStorage::Revert documents in MSDN.
        /// Note: Not implemented in Shetab Mount Zip Library.
        /// </summary>
        new void Revert();
        /// <summary>
        /// See IStorage::EnumElements documents in MSDN.
        /// </summary>
        new void EnumElements(/* [in] */ uint reserved1, /* [size_is][unique][in] */ IntPtr reserved2, /* [in] */ uint reserved3,/* [out] */ out IEnumSTATSTG ppenum);
        /// <summary>
        /// See IStorage::DestroyElement documents in MSDN.
        /// </summary>
        new void DestroyElement(/* [string][in] */ string pwcsName);
        /// <summary>
        /// See IStorage::RenameElement documents in MSDN.
        /// </summary>
        new void RenameElement( /* [string][in] */ string pwcsOldName, /* [string][in] */ string pwcsNewName);
        /// <summary>
        /// See IStorage::SetElementTimes documents in MSDN.
        /// </summary>
        new void SetElementTimes(/* [string][unique][in] */ string pwcsName, /* [unique][in] */ System.Runtime.InteropServices.ComTypes.FILETIME pctime, /* [unique][in] */ System.Runtime.InteropServices.ComTypes.FILETIME patime, /* [unique][in] */ System.Runtime.InteropServices.ComTypes.FILETIME pmtime);
        /// <summary>
        /// See IStorage::SetClass documents in MSDN.
        /// Note: Not implemented in Shetab Mount Zip Library.
        /// </summary>
        new void SetClass(/* [in] */ Guid clsid);
        /// <summary>
        /// See IStorage::SetStateBits documents in MSDN.
        /// </summary>
        new void SetStateBits(/* [in] */ uint grfStateBits, /* [in] */ uint grfMask);
        /// <summary>
        /// See IStorage::Stat documents in MSDN.
        /// </summary>
        new void Stat(/* [out] */ out System.Runtime.InteropServices.ComTypes.STATSTG pstatstg, /* [in] */ uint grfStatFlag);
        #endregion

        #region IBoxStorage Methods
        /// <summary>
        /// Retrieve the status of specified pszPathName in storage.
        /// </summary>
        /// <param name="pszPathName">Relative path to storage.</param>
        /// <param name="pathExist">return the status of specified pszPathName in storage.</param>
        void IsPathExists(string pszPathName, [MarshalAs(UnmanagedType.I4)] out PathExist pathExist);
        /// <summary>
        /// <c>OpenStorageByPath</c> gets a relative path and out one IBoxStorage. 
        /// </summary>
        /// <param name="pszPathName">Relative path to storage.</param>
        /// <param name="grfMode">See grfMode documents in MSDN.</param>
        /// <param name="ppBoxStorage">Return an IBoxStorage object that out relative to path.</param>
        void OpenStorageByPath(string pszPathName, [MarshalAs(UnmanagedType.U4)] STGM grfMode, out IBoxStorage ppBoxStorage);
        /// <summary>
        /// <c>OpenStreamByPath</c> get a relative path and out an IBoxStream
        /// </summary>
        /// <param name="pszPathName">Relative path to stream.</param>
        /// <param name="grfMode">See grfMode documents in MSDN.</param>
        /// <param name="ppBoxStream">Return an IBoxStorage object relative to path</param>
        void OpenStreamByPath(string pszPathName, [MarshalAs(UnmanagedType.U4)] STGM grfMode, out IBoxStream ppBoxStream);
        /// <summary>
        /// Add a specific file to current storage.
        /// </summary>
        /// <param name="pszSrcFile">Source file path.</param>
        /// <param name="pszDestFile">Destination file path.</param>
        /// <param name="overwrite">Determine that overwrite Source in Destination or no, if it exist.</param>
        void AddFile(string pszSrcFile, string pszDestFile, [MarshalAs(UnmanagedType.Bool)] bool overwrite);
        /// <summary>
        /// Add a specific folder to current storage.
        /// </summary>
        /// <param name="pszSrcFolder">Source folder path.</param>
        /// <param name="pszDesFolder">Destination folder path</param>
        /// <param name="pszWildCard">Specific wild card that determine what contents of file add to storage.</param>
        /// <param name="overwrite">Determine that overwrite Source in Destination or not, if it already exists.</param>
        void AddFolder(string pszSrcFolder, string pszDesFolder, string pszWildCard, [MarshalAs(UnmanagedType.Bool)] bool overwrite);
        /// <summary>
        /// Create a new folder (storage) in current storage.
        /// </summary>
        /// <param name="pszFolder">Specific relative path of folder.</param>
        void CreateFolder(string pszFolder);
        /// <summary>
        /// Remove an exist folder (storage) from current storage.
        /// </summary>
        /// <param name="pszFolder">Specific relative path of folder.</param>
        void RemoveFolder(string pszFolder);
        /// <summary>
        /// Remove existing file from current storage.
        /// </summary>
        /// <param name="pszFile">Specific relative path of file.</param>
        void RemoveFile(string pszFile);
        /// <summary>
        /// <c>ExtractTo</c> extract current storage into specific path.
        /// </summary>
        /// <param name="pszFolder">Determine extract path.</param>
        /// <param name="pszWildCard">Specific wild card for determine where extract current storage.</param>
        /// <param name="recursive">Determine apply this method to all sub folder or not.</param>
        void ExtractTo(string pszFolder, string pszWildCard, [MarshalAs(UnmanagedType.Bool)] bool recursive);
        /// <summary>
        /// Set current password on storage.
        /// </summary>
        /// <param name="pszValue">Password value</param>
        void SetPassword(string pszValue);
        /// <summary>
        /// Get current password on storage. It returns empty string if you don't call SetPassword when create object.
        /// </summary>
        /// <param name="password">Password value</param>
        void GetPassword([MarshalAs(UnmanagedType.BStr)] out string password);
        /// <summary>
        /// See IStorage::Clone documents in MSDN.
        /// </summary>
        /// <param name="ppBoxStorage">See MSDN Documents.</param>
        void Clone(out IBoxStorage ppBoxStorage);
        /// <summary>
        /// Find files (IStream) and folders (IStorage) in current storage according to the wildCards.
        /// </summary>
        /// <param name="penum">an object with type of IEnumSTATSTG for enumeration. see IEnumSTATSTG documents in MSDN.</param>
        /// <param name="wildCards">Search criteria.
        /// <example>*.*</example>
        /// <example>*.txt</example>
        /// </param>
        /// <param name="findFile">If false, files (IStream) name does not include in search otherwise files name include in search.</param>
        /// <param name="findFolder">If false, folders(IStorage) name does not include in search otherwise folders name include in search.</param>
        /// <param name="findRecursive">If true search in sub folders (sub storage).</param>
        void FindFiles(out IEnumSTATSTG penum, string wildCards, [MarshalAs(UnmanagedType.Bool)] bool findFile, [MarshalAs(UnmanagedType.Bool)] bool findFolder, [MarshalAs(UnmanagedType.Bool)] bool findRecursive);
        /// <summary>
        /// Get information of pszFile object. see IStorage::Stat documents in MSDN for more information.
        /// </summary>
        /// <param name="pszFile">Relative path to file or folder.</param>
        /// <param name="pstatstg">Reference to a STATSTG structure in which this method places information about this byte array object. The reference is NULL if an error occurs. see IStorage::Stat documents in MSDN.</param>
        /// <param name="grfStatFlag">Specifies whether this method should supply the pwcsName member of the STATSTG structure through values taken from the STATFLAG enumeration. 
        /// If the STATFLAG_NONAME is specified, the pwcsName member of STATSTG is not supplied, thus saving a memory-allocation operation. The other possible value, STATFLAG_DEFAULT
        /// , indicates that all members of the STATSTG structure be supplied.see IStorage::Stat documents in MSDN.</param>
        /// <returns>return S_OK if object exists and pstatstg is filled, return S_FALSE if pszFile does not exists (not IStream or IStorage).</returns>
        int GetFileStat(string pszFile, out System.Runtime.InteropServices.ComTypes.STATSTG pstatstg, uint grfStatFlag);
        /// <summary>
        /// For internal use.
        /// <param name="busy">return true when no one can use current storage.</param> 
        /// </summary>
        void IsBusy([MarshalAs(UnmanagedType.Bool)] out bool busy);
        /// <summary>
        /// Get the full path of box file. Shetab Zip Mount Library return the path of zip file. 
        /// </summary>
        /// <param name="pbstrBoxFile">Return path of box file.</param>
        void GetBoxFile([MarshalAs(UnmanagedType.BStr)] out string pbstrBoxFile);
        /// <summary>
        /// Get the size of box file. Shetab Zip Mount Library return size of occupied space of zip file.
        /// </summary>
        /// <param name="boxSize">Return size of box file.</param>
        void GetBoxSize(out UInt64 boxSize); 
        #endregion
    };

}
