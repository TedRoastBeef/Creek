namespace Creek.Compression.Mounting
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// <c>PackerHelper</c> class create's and initialize Shetab Mount Zip Library's objects for use
    /// </summary>
    public class PackerHelper
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern IntPtr LoadLibrary(String csFileName);

        [DllImport("ShetabPacker.dll")]
        static extern int CreateShetabPacker(out IPacker ppstgOpen);

        /// <summary>
        /// Win32 AddAdtom helper, used internally
        /// </summary>
        /// <returns></returns>
        [DllImport("Kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern ushort AddAtom(String lpString);

        /// <summary>
        /// Call it if "ShetabPacker.dll" does not exist in default dll folder
        /// </summary>
        /// <param name="dllFolder">Path to a folder that contains ShetabPacker.dll.</param>
        public static void InitDllFolder(String dllFolder)
        {
            IntPtr res = LoadLibrary(System.IO.Path.Combine(dllFolder, "ShetabPacker.dll"));
            if (res == IntPtr.Zero)
                throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());
        }

        /// <summary>
        /// <c>Create</c> uses for creation an IPacker object that needs in start of working with Shetab Mount Zip Library.
        /// </summary>
        /// <returns>IPacker object of Shetab Mount Zip Library.</returns>
        public static IPacker Create()
        {
            IPacker res;
            CreateShetabPacker(out res);
            return res;
        }
    }
}
