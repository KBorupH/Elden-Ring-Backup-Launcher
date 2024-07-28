using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace EldenRingBackupLauncherWPF
{
    internal static class ConfigManager
    {

        private static string file = "LauncherConfig.ini";

        private static string installLoc = "";
        public static string InstallLoc
        {
            get 
            {
                return installLoc; 
            }
            set 
            { 
                installLoc = value;
                WriteConfig();
            }
        }

        private static string saveLoc = "";
        public static string SaveLoc
        {
            get
            {
                return saveLoc;
            }
            set
            {
                saveLoc = value;
                WriteConfig();
            }
        }

        public static void ReadConfig()
        { 
            if (!File.Exists(file)) return;

            StreamReader sr = new StreamReader(file);
            string? line = sr.ReadLine();
            while (line != null)
            {
                switch (line)
                {
                    case string x when x.StartsWith("InstallLoc"):
                        installLoc = x.Replace("InstallLoc: ", "");
                        break;
                    case string x when x.StartsWith("SaveLoc"):
                        saveLoc = x.Replace("SaveLoc: ", "");
                        break;
                    default:
                        break;
                }
                line = sr.ReadLine();
            }
            sr.Close();
        }

        private static void WriteConfig() 
        {
            string fileContents = $"InstallLoc: {installLoc}\nSaveLoc: {saveLoc}";


            File.WriteAllText(file, fileContents);
        }
    }
}
