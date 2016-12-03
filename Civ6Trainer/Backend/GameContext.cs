using System;

namespace tctianchi.Civ6Trainer.Backend
{
    class GameContext
    {
        public int ProcessId { get; private set; }
        public string ProcessVersion { get; private set; }
        public UInt32 ModuleAddress { get; private set; }
        public UInt32 PlayerAddress { get; private set; }

        // Get a context if the game is running and recognized.
        // Returns null if not running.
        // Raise exception if game version is not recognized.
        public static GameContext FindGameRunning(string processName, string moduleName)
        {
            System.Diagnostics.Process[] processesByName = System.Diagnostics.Process.GetProcessesByName(processName);
            if (processesByName.Length > 0)
            {
                return new GameContext(processesByName[0], moduleName);
            }
            return null;
        }

        public GameContext(System.Diagnostics.Process gameProcess, string moduleName)
        {
            // Get PID, version
            GetProcessInfo(gameProcess);

            // Find module
            GetModuleInfo(moduleName);

            // Decide addresses according to version
            GetGameAddress();
        }

        private void GetProcessInfo(System.Diagnostics.Process gameProcess)
        {
            // PID
            try
            {
                ProcessId = gameProcess.Id;
                ProcessVersion = gameProcess.MainModule.FileVersionInfo.FileVersion;
            }
            catch
            {
                throw new Exception(Properties.Resources.UITextInvalidProcessId);
            }

            // Version
            if (ProcessVersion == null)
            {
                throw new Exception(Properties.Resources.UITextGetModuleInfoFailed);
            }
            ProcessVersion = ProcessVersion.Replace(", ", ".");
        }

        private void GetModuleInfo(string moduleName)
        {
            WindowsApi.ProcessModule mainModule =
                new WindowsApi.ProcessModule(
                    ProcessId,
                    moduleName);
            ModuleAddress = (uint)mainModule.BaseAddress;
        }

        private void GetGameAddress()
        {
            switch (ProcessVersion)
            {
                case "1.0.0.26.(221715) (10/07/2016)":
                    PlayerAddress = ModuleAddress + 0x7185D8;
                    break;
                default:
                    throw new Exception(string.Format(Properties.Resources.UITextUnkonwnGameVersion, ProcessVersion));
            }
        }
    }
}
