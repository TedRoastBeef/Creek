namespace Creek.Compression.Mounting
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>Manage Boxes for mount and un-mount.</summary>
    [ComImport]
    [Guid("D346AB92-B688-4344-9D94-70A25E0FE0A4")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IBoxManager
	{
        /// <summary>
        /// return temporary folder for internal work specified in IPacker.CreateBoxManager
        /// NOTE: Shetab Mount Zip Library dose not export zip file in temporary folder or any part of disk.
        /// </summary>
        /// <param name="tempFolder">return temporary folder</param>
        void GetTempFolder([MarshalAs(UnmanagedType.BStr)] out string tempFolder);
        /// <summary>
        /// Check is mount service is running or not
        /// </summary>
        /// <param name="mounted">return true if boxes is mounted to process otherwise false</param>
        void IsMounted([MarshalAs(UnmanagedType.Bool)] out bool mounted);
        /// <summary>
        /// Mount boxes in current process.
        /// </summary>
		void Mount();
        /// <summary>
        /// Remove mount from current process.
        /// WARNING: After starting Mapping, you may not reopen the BoxStorage with write access. System may get exclusive access to Storage file.
        /// </summary>
		void Unmount();
        /// <summary>
        /// Set is packed file in boxes should show in standard windows file dialog, may not work in .NET
        /// </summary>
        /// <param name="value">true to show, false to hide.</param>
        void SetShowFileInFileDialog([MarshalAs(UnmanagedType.Bool)] bool value);
        /// <summary>
        /// check whether packed file will show in standard windows file dialog.
        /// </summary>
        /// <param name="showFileInFileDialog">return true if packed will show otherwise false</param>
        void GetShowFileInFileDialog([MarshalAs(UnmanagedType.Bool)] out bool showFileInFileDialog);
        /// <summary>
        /// Find BoxStorage that is specified file exists in it.
        /// </summary>
        /// <param name="pszPathName">the path where you searching for</param>
        /// <param name="findRoot">for internal use, set to false.</param>
        /// <param name="pbstrPathR">return relative path to BoxStorage file if found.</param>
        /// <param name="reserved">for internal use, set to IntPtr.Zero</param>
        /// <param name="ppBoxStorage">return BoxStorage where pszPathName exist in it.</param>
        /// <returns>S_OK if file found; S_FALSE if file not found; COM Error if error</returns>
        [PreserveSig]
        int FindBoxForPath(string pszPathName, [MarshalAs(UnmanagedType.Bool)] bool findRoot, [MarshalAs(UnmanagedType.BStr)] out string pbstrPathR, IntPtr reserved, out IBoxStorage ppBoxStorage);
        /// <summary>
        /// Find BoxStorage by specified file path.
        /// </summary>
        /// <param name="pszBoxFileName">Path of box file.</param>
        /// <param name="ppBoxStorage">return BoxStorage that its storage path is pszBoxFileName</param>
        /// <returns>S_OK if file found; S_FALSE if file not found; COM Error if error</returns>
        [PreserveSig]
        int FindBoxByFile(string pszBoxFileName, out IBoxStorage ppBoxStorage);
        /// <summary>
        /// Add BoxStorage to Manager
        /// </summary>
        /// <param name="pboxStorage"><c>pboxStorage is an object of IBoxStorage type that you want to add in box.</c></param>
        /// <param name="pszbaseFolder">Specific base folder for adding to box.</param>
        void AddBox(IBoxStorage pboxStorage, string pszbaseFolder);
        /// <summary>
        /// Remove pboxStorage from Manager
        /// </summary>
        /// <param name="pboxStorage">Determine box storage that you want to remove from storage. It is type of IBoxStorage.</param>
         void RemoveBox(IBoxStorage pboxStorage);
        /// <summary>
        /// Remove all IBoxStorage from Manager
        /// </summary>
        void RemoveAllBoxes();
	};
}
