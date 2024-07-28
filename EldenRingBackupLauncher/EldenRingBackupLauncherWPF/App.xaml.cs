using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;

namespace EldenRingBackupLauncherWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        void App_Startup(object sender, StartupEventArgs e)
        {
            ConfigManager.ReadConfig();

            if (string.IsNullOrWhiteSpace(ConfigManager.InstallLoc) || string.IsNullOrWhiteSpace(ConfigManager.SaveLoc) || AnyKeyPressed())
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.ShowDialog();
            }

            if (!string.IsNullOrWhiteSpace(ConfigManager.SaveLoc))
            {
                DirectoryInfo saveFoler = Directory.GetParent(ConfigManager.SaveLoc) ?? new DirectoryInfo("C:\\");
                string backupFolder = Path.Combine(saveFoler.FullName, "EldenRingBackupSaves");
                string currentSaveFolder = Path.Combine(backupFolder, DateTime.Now.ToString("yyyy_MM_dd__HH-mm-ss"));
                Directory.CreateDirectory(backupFolder);
                Directory.CreateDirectory(currentSaveFolder);

                File.Copy(ConfigManager.SaveLoc, Path.Combine(currentSaveFolder, Path.GetFileName(ConfigManager.SaveLoc)));
            }

            if (!string.IsNullOrWhiteSpace(ConfigManager.InstallLoc))
            {
                ProcessStartInfo processInfo = new ProcessStartInfo(ConfigManager.InstallLoc);
                processInfo.WorkingDirectory = Path.GetDirectoryName(ConfigManager.InstallLoc);
                processInfo.UseShellExecute = false;
                Process.Start(processInfo);
            }
        }



        [DllImport("user32.dll", EntryPoint = "GetKeyboardState", SetLastError = true)]
        private static extern bool NativeGetKeyboardState([Out] byte[] keyStates);

        private static bool GetKeyboardState(byte[] keyStates)
        {
            if (keyStates == null)
                throw new ArgumentNullException("keyState");
            if (keyStates.Length != 256)
                throw new ArgumentException("The buffer must be 256 bytes long.", "keyState");
            return NativeGetKeyboardState(keyStates);
        }

        private static byte[] GetKeyboardState()
        {
            byte[] keyStates = new byte[256];
            if (!GetKeyboardState(keyStates))
                throw new Win32Exception(Marshal.GetLastWin32Error());
            return keyStates;
        }

        private static bool AnyKeyPressed()
        {
            byte[] keyState = GetKeyboardState();
            // skip the mouse buttons
            return keyState.Skip(8).Any(state => (state & 0x80) != 0);
        }


    }



}
