﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Permissions;
using System.Runtime.InteropServices;
using System.Runtime.ConstrainedExecution;
using System.Security;

namespace tctianchi.Civ6Trainer.Backend.WindowsApi
{
    #region Process handle

    [SuppressUnmanagedCodeSecurity]
    [HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
    internal sealed class SafeProcessHandle
        : Microsoft.Win32.SafeHandles.SafeHandleZeroOrMinusOneIsInvalid
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal static SafeProcessHandle InvalidHandle = new SafeProcessHandle(IntPtr.Zero);

        internal SafeProcessHandle()
            : base(true)
        {
        }

        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        internal SafeProcessHandle(IntPtr handle)
            : base(true)
        {
            base.SetHandle(handle);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal void InitialSetHandle(IntPtr initialValue)
        {
            base.handle = initialValue;
        }

        protected override bool ReleaseHandle()
        {
            return NativeMethods.CloseHandle(base.handle);
        }
    }

    #endregion

    #region Windows API

    [SuppressUnmanagedCodeSecurity()]
    internal static class NativeMethods
    {
        //////////////////////////////////////////////////////////////////////////
        // Process handle
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern SafeProcessHandle OpenProcess(
            UInt32 desiredAccess,
            [MarshalAs(UnmanagedType.Bool)] bool isInheritHandle,
            int processId);

        [Flags]
        internal enum ProcessAccessFlags : uint
        {
            None             = 0,
            Terminate        = 0x00000001,
            CreateThread     = 0x00000002,
            VMOperation      = 0x00000008,
            VMRead           = 0x00000010,
            VMWrite          = 0x00000020,
            DupHandle        = 0x00000040,
            SetInformation   = 0x00000200,
            QueryInformation = 0x00000400,
            Synchronize      = 0x00100000,
            All              = 0x001F0FFF,
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        internal static extern bool CloseHandle(IntPtr objectHandle);

        //////////////////////////////////////////////////////////////////////////
        // Memory operation
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool ReadProcessMemory(
            SafeProcessHandle processHandle,
            IntPtr baseAddress,
            [In, Out] byte[] buffer,
            UIntPtr size,
            out UIntPtr numberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool WriteProcessMemory(
            SafeProcessHandle processHandle,
            IntPtr baseAddress,
            [Out] byte[] buffer,
            UIntPtr size,
            out UIntPtr numberOfBytesWritten);

        //////////////////////////////////////////////////////////////////////////
        // Process modules
        [DllImport("psapi.dll")]
        internal static extern bool EnumProcessModules(
            SafeProcessHandle processHandle,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U4)] [In][Out] IntPtr[] lphModule,
            int cb,
            [MarshalAs(UnmanagedType.U4)] out int needed);

        [DllImport("psapi.dll")]
        internal static extern bool EnumProcessModulesEx(
            SafeProcessHandle processHandle,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U4)] [In][Out] IntPtr[] lphModule,
            int cb,
            [MarshalAs(UnmanagedType.U4)] out int needed,
            uint filterFlag);

        internal const uint LIST_MODULES_ALL = 0x03;

        //////////////////////////////////////////////////////////////////////////
        // Module information
        [DllImport("psapi.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true, BestFitMapping = false)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal static extern uint GetModuleFileNameEx(
            SafeProcessHandle processHandle,
            IntPtr moduleHandle,
            [Out] StringBuilder baseName,
            [In] [MarshalAs(UnmanagedType.U4)] int size);

        [DllImport("psapi.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true, BestFitMapping = false)]
        internal static extern uint GetModuleBaseName(
            SafeProcessHandle processHandle,
            IntPtr moduleHandle,
            [Out] StringBuilder baseName,
            [In] [MarshalAs(UnmanagedType.U4)] int size);

        [DllImport("psapi.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        internal static extern bool GetModuleInformation(
            SafeProcessHandle processHandle,
            IntPtr moduleHandle,
            NtModuleInfo ntModuleInfo,
            int size);

        [StructLayout(LayoutKind.Sequential)]
        internal class NtModuleInfo
        {
            public IntPtr BaseOfDll = (IntPtr)0;
            public int SizeOfImage = 0;
            public IntPtr EntryPoint = (IntPtr)0;
        }
    }

    #endregion

    #region ProcessMemory

    public class ProcessMemory
        : IDisposable
    {
        private readonly int _processId;
        private readonly SafeProcessHandle _processHandle;

        #region Build from PID

        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public ProcessMemory(int processId)
        {
            // Save paramter
            _processId = processId;
            
            // Open handle
            _processHandle = NativeMethods.OpenProcess(
                (uint)NativeMethods.ProcessAccessFlags.All,
                false,
                _processId);
            if (_processHandle.IsInvalid)
            {
                throw new Exception(Properties.Resources.UITextInvalidProcessId);
            }
        }

        #endregion

        private bool _isDisposed;

        #region IDisposable

        ~ProcessMemory()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        [SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
        protected virtual void Dispose(bool isDisposing)
        {
            if (!_isDisposed)
            {
                _isDisposed = true;
                if (isDisposing)
                {
                    // Free managed resources here
                    if (_processHandle != null && !_processHandle.IsInvalid)
                    {
                        // Free the handle
                        _processHandle.Dispose();
                    }
                }

                // Free unmanaged resources here
            }
        }

        #endregion

        #region Core R/W

        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public byte[] ReadBytes(IntPtr baseAddress, int size, out int bytesRead)
        {
            byte[] buffer = new byte[size];
            UIntPtr apiSize = (UIntPtr)size, apiBytesRead;

            // Note: Memory access error will be ignored
            NativeMethods.ReadProcessMemory(
                _processHandle,
                baseAddress,
                buffer,
                apiSize,
                out apiBytesRead);
            if ((ulong)apiBytesRead <= (ulong)int.MaxValue)
            {
                bytesRead = (int)apiBytesRead;
            }
            else
            {
                bytesRead = 0;
            }
            return buffer;
        }

        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public void WriteBytes(byte[] buffer, IntPtr baseAddress, int size, out int bytesWriten)
        {
            UIntPtr apiSize = (UIntPtr)size, apiBytesWriten;

            // Note: Memory access error will be ignored
            NativeMethods.WriteProcessMemory(
                _processHandle,
                baseAddress,
                buffer,
                apiSize,
                out apiBytesWriten);
            if ((ulong)apiBytesWriten <= (ulong)int.MaxValue)
            {
                bytesWriten = (int)apiBytesWriten;
            }
            else
            {
                bytesWriten = 0;
            }
        }

        #endregion

        //////////////////////////////////////////////////////////////////////////
        // Access helper
        #region int/float/string R/W

        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "float")]
        public float ReadFloat(IntPtr baseAddress)
        {
            int bytesRead;
            byte[] buffer = ReadBytes(baseAddress, sizeof(float), out bytesRead);
            return BitConverter.ToSingle(buffer, 0);
        }

        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public void WriteFloat(IntPtr baseAddress, float value)
        {
            int bytesWriten;
            WriteBytes(
                BitConverter.GetBytes(value),
                baseAddress, sizeof(float), out bytesWriten);
        }

        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public UInt64 ReadUInt64(IntPtr baseAddress)
        {
            int bytesRead;
            byte[] buffer = ReadBytes(baseAddress, sizeof(UInt64), out bytesRead);
            return BitConverter.ToUInt64(buffer, 0);
        }

        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public void WriteUInt64(IntPtr baseAddress, UInt64 value)
        {
            int bytesWriten;
            WriteBytes(
                BitConverter.GetBytes(value),
                baseAddress, sizeof(UInt64), out bytesWriten);
        }

        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public Int64 ReadInt64(IntPtr baseAddress)
        {
            int bytesRead;
            byte[] buffer = ReadBytes(baseAddress, sizeof(Int64), out bytesRead);
            return BitConverter.ToInt64(buffer, 0);
        }

        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public void WriteInt64(IntPtr baseAddress, Int64 value)
        {
            int bytesWriten;
            WriteBytes(
                BitConverter.GetBytes(value),
                baseAddress, sizeof(Int64), out bytesWriten);
        }

        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public UInt32 ReadUInt32(IntPtr baseAddress)
        {
            int bytesRead;
            byte[] buffer = ReadBytes(baseAddress, sizeof(UInt32), out bytesRead);
            return BitConverter.ToUInt32(buffer, 0);
        }

        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public void WriteUInt32(IntPtr baseAddress, UInt32 value)
        {
            int bytesWriten;
            WriteBytes(
                BitConverter.GetBytes(value),
                baseAddress, sizeof(UInt32), out bytesWriten);
        }

        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public Int32 ReadInt32(IntPtr baseAddress)
        {
            int bytesRead;
            byte[] buffer = ReadBytes(baseAddress, sizeof(Int32), out bytesRead);
            return BitConverter.ToInt32(buffer, 0);
        }

        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public void WriteInt32(IntPtr baseAddress, Int32 value)
        {
            int bytesWriten;
            WriteBytes(
                BitConverter.GetBytes(value),
                baseAddress, sizeof(Int32), out bytesWriten);
        }
        
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public UInt16 ReadUInt16(IntPtr baseAddress)
        {
            int bytesRead;
            byte[] buffer = ReadBytes(baseAddress, sizeof(UInt16), out bytesRead);
            return BitConverter.ToUInt16(buffer, 0);
        }

        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public void WriteUInt16(IntPtr baseAddress, UInt16 value)
        {
            int bytesWriten;
            WriteBytes(
                BitConverter.GetBytes(value),
                baseAddress, sizeof(UInt16), out bytesWriten);
        }

        // Return null if failed
        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public string ReadCString(IntPtr baseAddress)
        {
            List<byte> buffer = new List<byte>();
            for (int currentBytes = 0; currentBytes < 1024; currentBytes++)
            {
                int bytesRead;
                byte[] oneByte = ReadBytes(baseAddress + currentBytes, 1, out bytesRead);
                if (bytesRead != 1)
                {
                    break;
                }
                if (oneByte[0] == 0)
                {
                    break;
                }
                buffer.Add(oneByte[0]);
            }
            try
            {
                return Encoding.UTF8.GetString(buffer.ToArray(), 0, buffer.Count);
            }
            catch (System.Text.DecoderFallbackException)
            {
                return null;
            }
        }

        #endregion
    }

    #endregion

    #region ProcessModule

    public class ProcessModule
    {
        public readonly IntPtr ModuleHandle;
        public readonly IntPtr BaseAddress;
        public readonly string FileName;

        public ProcessModule(int processId, string moduleNamePattern)
        {
            // Open handle
            using (SafeProcessHandle processHandle = NativeMethods.OpenProcess(
                (uint)NativeMethods.ProcessAccessFlags.All,
                false,
                processId))
            {
                if (processHandle.IsInvalid)
                {
                    throw new Exception(Properties.Resources.UITextInvalidProcessId);
                }

                // Get all modules
                IntPtr[] allModules = GetProcessModulesHandle(processHandle);
                foreach (IntPtr moduleHandle in allModules)
                {
                    // Get module name
                    string moduleName = GetModuleName(processHandle, moduleHandle);

                    // Match name
                    if (moduleName.ToLower() == moduleNamePattern.ToLower())
                    {
                        // Bingo!
                        ModuleHandle = moduleHandle;

                        // Get other info
                        BaseAddress = GetModuleBaseAddress(processHandle, moduleHandle);
                        FileName = GetModuleFileName(processHandle, moduleHandle);

                        return;
                    }
                }
            }

            throw new Exception(Properties.Resources.UITextGetModuleInfoFailed);
        }

        internal static IntPtr[] GetProcessModulesHandle(SafeProcessHandle processHandle)
        {
            try
            {
                // 64 bit machine
                return GetProcessModulesHandle64(processHandle);
            }
            catch (EntryPointNotFoundException)
            {
                // 32 bit machine
                return GetProcessModulesHandle32(processHandle);
            }
        }

        // Note: EnumProcessModulesEx does not exist in 32 bit windows
        private static IntPtr[] GetProcessModulesHandle64(SafeProcessHandle processHandle)
        {
            int sizeNeeded = 0;
            IntPtr[] modulesHandle = new IntPtr[0];

            // Call EnumProcessModules the first time to get the size of the array needed
            NativeMethods.EnumProcessModulesEx(
                processHandle,
                modulesHandle,
                0,
                out sizeNeeded,
                NativeMethods.LIST_MODULES_ALL);
            modulesHandle = new IntPtr[sizeNeeded / IntPtr.Size];

            // Call EnumProcessModules the second time to get content
            NativeMethods.EnumProcessModulesEx(
                processHandle,
                modulesHandle,
                modulesHandle.Length * IntPtr.Size,
                out sizeNeeded,
                NativeMethods.LIST_MODULES_ALL);
            return modulesHandle;
        }

        private static IntPtr[] GetProcessModulesHandle32(SafeProcessHandle processHandle)
        {
            int sizeNeeded = 0;
            IntPtr[] modulesHandle = new IntPtr[0];

            // Call EnumProcessModules the first time to get the size of the array needed
            NativeMethods.EnumProcessModules(
                processHandle,
                modulesHandle,
                0,
                out sizeNeeded);
            modulesHandle = new IntPtr[sizeNeeded / IntPtr.Size];

            // Call EnumProcessModules the second time to get content
            NativeMethods.EnumProcessModules(
                processHandle,
                modulesHandle,
                modulesHandle.Length * IntPtr.Size,
                out sizeNeeded);
            return modulesHandle;
        }

        internal static string GetModuleFileName(SafeProcessHandle processHandle, IntPtr moduleHandle)
        {
            StringBuilder name = new StringBuilder(256);
            if (NativeMethods.GetModuleFileNameEx(
                processHandle,
                moduleHandle,
                name,
                name.Capacity) != 0)
            {
                return name.ToString();
            }
            else
                return null;
        }

        internal static string GetModuleName(SafeProcessHandle processHandle, IntPtr moduleHandle)
        {
            StringBuilder name = new StringBuilder(256);
            if (NativeMethods.GetModuleBaseName(
                processHandle,
                moduleHandle,
                name,
                name.Capacity) != 0)
            {
                return name.ToString();
            }
            else
            {
                return null;
            }
        }

        internal static IntPtr GetModuleBaseAddress(SafeProcessHandle processHandle, IntPtr moduleHandle)
        {
            NativeMethods.NtModuleInfo ntModuleInfo = new NativeMethods.NtModuleInfo();
            if (!NativeMethods.GetModuleInformation(
                processHandle,
                moduleHandle,
                ntModuleInfo,
                Marshal.SizeOf(ntModuleInfo)))
            {
                throw new Exception(Properties.Resources.UITextGetModuleInfoFailed);
            }
            return ntModuleInfo.BaseOfDll;
        }
    }

    #endregion
}
