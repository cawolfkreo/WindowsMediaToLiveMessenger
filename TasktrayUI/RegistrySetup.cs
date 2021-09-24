using Microsoft.Win32;

namespace AudioSyncMSNUI
{
    class RegistrySetup
    {
        private readonly string RegistryPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
        
        public void SetRegistry(string AppName)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryPath, true);

            if(key.GetValue(AppName) == null)
            {
                key.SetValue(AppName, System.Windows.Forms.Application.ExecutablePath);
            }
        }
    }
}
