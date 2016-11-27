using System;
using System.Diagnostics;

namespace tctianchi.Civ6Trainer.Backend
{
    class GameContext
    {
        public int ProcessId { get; private set; }
        public string ProcessVersion { get; private set; }
        public UInt32 ModuleAddress { get; private set; }
        public UInt32 PlayerAddress { get; private set; }
        public UInt32 PlayerInterval { get; private set; }
        public UInt32 PlayerExt2Address { get; private set; }
        public UInt32 PlayerExt2Interval { get; private set; }
        public UInt32 CityConstAddress { get; private set; }

        // Get a context if the game is running and recognized.
        // Returns null if not running.
        // Raise exception if game version is not recognized.
        public static GameContext FindGameRunning(string processName, string moduleName)
        {
            Process[] processesByName = Process.GetProcessesByName(processName);
            if (processesByName.Length > 0)
            {
                return new GameContext(processesByName[0], moduleName);
            }
            return null;
        }

        public GameContext(Process gameProcess, string moduleName)
        {
            // Get PID
            GetProcessInfo(gameProcess);

            // Find module
            GetModuleInfo(moduleName);

            // Decide addresses according to version
            GetGameAddress();
        }

        private void GetProcessInfo(Process gameProcess)
        {
            try
            {
                this.ProcessId = gameProcess.Id;
            }
            catch
            {
                throw new InvalidOperationException("Failed to fetch process Id");
            }
        }

        private void GetModuleInfo(string moduleName)
        {
            // Find module
            WindowsApi.ProcessModule mainModule =
                new WindowsApi.ProcessModule(
                    ProcessId,
                    moduleName);

            // Check parameters
            string moduleFileName = mainModule.FileName;
            System.Diagnostics.FileVersionInfo moduleVersion = System.Diagnostics.FileVersionInfo.GetVersionInfo(moduleFileName);
            string fileVersion = moduleVersion.FileVersion;
            if (fileVersion == null)
            {
                throw new InvalidOperationException("Bad file version");
            }

            // Save info
            this.ProcessVersion = fileVersion.Replace(", ", ".");
            this.ModuleAddress = (uint)mainModule.BaseAddress;
        }

        private void GetGameAddress()
        {
            switch (this.ProcessVersion)
            {
                case "3.0.3.0":
                    this.PlayerAddress = this.ModuleAddress + 0x3D48B4;
                    this.PlayerInterval = 0xF5B4;
                    this.PlayerExt2Address = this.ModuleAddress + 0x3D5E5C;
                    this.PlayerExt2Interval = 0xBA4;
                    this.CityConstAddress = this.ModuleAddress + 0x3B319C;
                    break;
                default:
                    throw new UnkonwnGameVersionExpection(ProcessVersion);
            }
        }
    }

    #region Exception

    public class UnkonwnGameVersionExpection
        : Exception
    {
        public UnkonwnGameVersionExpection(string version)
            : base(String.Format(Properties.Resources.UITextUnkonwnGameVersion, version))
        {

        }
    }

    #endregion
}
