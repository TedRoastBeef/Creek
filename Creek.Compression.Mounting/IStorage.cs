namespace Creek.Compression.Mounting
{
    using System;
    using System.Runtime.InteropServices.ComTypes;
    using System.Runtime.InteropServices;

    /// <summary>
    /// See IStorage documents in MSDN. it works in this program with zip files
    /// </summary>
    [ComImport]
    [Guid("0000000b-0000-0000-C000-000000000046")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IStorage
    {
        /// <summary>
        /// See IStorage::CreateStream documents in MSDN.
        /// </summary>
        void CreateStream(/* [string][in] */ string pwcsName, /* [in] */ [MarshalAs(UnmanagedType.U4)] STGM grfMode, /* [in] */ uint reserved1, /* [in] */ uint reserved2, /* [out] */ out IStream ppstm);
        /// <summary>
        /// See IStorage::OpenStream documents in MSDN.
        /// </summary>
        void OpenStream(/* [string][in] */ string pwcsName, /* [unique][in] */ IntPtr reserved1, /* [in] */ [MarshalAs(UnmanagedType.U4)] STGM grfMode, /* [in] */ uint reserved2, /* [out] */ out IStream ppstm);
        /// <summary>
        /// See IStorage::CreateStorage documents in MSDN.
        /// </summary>
        void CreateStorage( /* [string][in] */ string pwcsName, /* [in] */ [MarshalAs(UnmanagedType.U4)] STGM grfMode,/* [in] */ uint reserved1, /* [in] */ uint reserved2, /* [out] */ out IStorage ppstg);
        /// <summary>
        /// See IStorage::OpenStorage documents in MSDN.
        /// </summary>
        void OpenStorage(/* [string][unique][in] */ string pwcsName, /* [unique][in] */ IStorage pstgPriority, /* [in] */ [MarshalAs(UnmanagedType.U4)] STGM grfMode, /* [unique][in] */ IntPtr snbExclude, /* [in] */ uint reserved, /* [out] */ out IStorage ppstg);
        /// <summary>
        /// See IStorage::CopyTo documents in MSDN.
        /// </summary>
        void CopyTo(/* [in] */ uint ciidExclude, /* [size_is][unique][in] */ Guid rgiidExclude, /* [unique][in] */ IntPtr snbExclude, /* [unique][in] */ IStorage pstgDest);
        /// <summary>
        /// See IStorage::MoveElementTo documents in MSDN.
        /// </summary>
        void MoveElementTo( /* [string][in] */ string pwcsName, /* [unique][in] */ IStorage pstgDest, /* [string][in] */ string pwcsNewName, /* [in] */ uint grfFlags);
        /// <summary>
        /// See IStorage::Commit documents in MSDN.
        /// </summary>
        void Commit( /* [in] */ uint grfCommitFlags);
        /// <summary>
        /// See IStorage::Revert documents in MSDN.
        /// </summary>
        void Revert();
        /// <summary>
        /// See IStorage::EnumElements documents in MSDN.
        /// </summary>
        void EnumElements(/* [in] */ uint reserved1, /* [size_is][unique][in] */ IntPtr reserved2, /* [in] */ uint reserved3,/* [out] */ out IEnumSTATSTG ppenum);
        /// <summary>
        /// See IStorage::DestroyElement documents in MSDN.
        /// </summary>
        void DestroyElement(/* [string][in] */ string pwcsName);
        /// <summary>
        /// See IStorage::RenameElement documents in MSDN.
        /// </summary>
        void RenameElement( /* [string][in] */ string pwcsOldName, /* [string][in] */ string pwcsNewName);
        /// <summary>
        /// See IStorage::SetElementTimes documents in MSDN.
        /// </summary>
        void SetElementTimes(/* [string][unique][in] */ string pwcsName, /* [unique][in] */ System.Runtime.InteropServices.ComTypes.FILETIME pctime, /* [unique][in] */ System.Runtime.InteropServices.ComTypes.FILETIME patime, /* [unique][in] */ System.Runtime.InteropServices.ComTypes.FILETIME pmtime);
        /// <summary>
        /// See IStorage::SetClass documents in MSDN.
        /// </summary>
        void SetClass(/* [in] */ Guid clsid);
        /// <summary>
        /// See IStorage::SetStateBits documents in MSDN.
        /// </summary>
        void SetStateBits(/* [in] */ uint grfStateBits, /* [in] */ uint grfMask);
        /// <summary>
        /// See IStorage::Stat documents in MSDN.
        /// </summary>
        void Stat(/* [out] */ out System.Runtime.InteropServices.ComTypes.STATSTG pstatstg, /* [in] */ uint grfStatFlag);

    }
}