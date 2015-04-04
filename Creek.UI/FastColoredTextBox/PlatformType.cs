﻿using System;
using System.Runtime.InteropServices;

namespace Creek.UI.FastColoredTextBox
{
    public static class PlatformType
    {
        private const ushort PROCESSOR_ARCHITECTURE_INTEL = 0;
        private const ushort PROCESSOR_ARCHITECTURE_IA64 = 6;
        private const ushort PROCESSOR_ARCHITECTURE_AMD64 = 9;
        private const ushort PROCESSOR_ARCHITECTURE_UNKNOWN = 0xFFFF;

        [DllImport("kernel32.dll")]
        private static extern void GetNativeSystemInfo(ref SYSTEM_INFO lpSystemInfo);

        [DllImport("kernel32.dll")]
        private static extern void GetSystemInfo(ref SYSTEM_INFO lpSystemInfo);

        public static Platform GetOperationSystemPlatform()
        {
            var sysInfo = new SYSTEM_INFO();

            // WinXP and older - use GetNativeSystemInfo
            if (Environment.OSVersion.Version.Major > 5 ||
                (Environment.OSVersion.Version.Major == 5 && Environment.OSVersion.Version.Minor >= 1))
            {
                GetNativeSystemInfo(ref sysInfo);
            }
                // else use GetSystemInfo
            else
            {
                GetSystemInfo(ref sysInfo);
            }

            switch (sysInfo.wProcessorArchitecture)
            {
                case PROCESSOR_ARCHITECTURE_IA64:
                case PROCESSOR_ARCHITECTURE_AMD64:
                    return Platform.X64;

                case PROCESSOR_ARCHITECTURE_INTEL:
                    return Platform.X86;

                default:
                    return Platform.Unknown;
            }
        }

        #region Nested type: SYSTEM_INFO

        [StructLayout(LayoutKind.Sequential)]
        private struct SYSTEM_INFO
        {
            public readonly ushort wProcessorArchitecture;
            public readonly ushort wReserved;
            public readonly uint dwPageSize;
            public readonly IntPtr lpMinimumApplicationAddress;
            public readonly IntPtr lpMaximumApplicationAddress;
            public readonly UIntPtr dwActiveProcessorMask;
            public readonly uint dwNumberOfProcessors;
            public readonly uint dwProcessorType;
            public readonly uint dwAllocationGranularity;
            public readonly ushort wProcessorLevel;
            public readonly ushort wProcessorRevision;
        };

        #endregion
    }

    public enum Platform
    {
        X86,
        X64,
        Unknown
    }
}