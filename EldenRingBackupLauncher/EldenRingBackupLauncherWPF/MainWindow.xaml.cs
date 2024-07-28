using Microsoft.Win32;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EldenRingBackupLauncherWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.TbInstallLoc.Text = ConfigManager.InstallLoc;
            this.TbSaveFile.Text = ConfigManager.SaveLoc;
        }

        private void BtnInstallLocFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            DirectoryInfo defaultPlacement = Directory.GetParent(ConfigManager.InstallLoc) ?? new DirectoryInfo("C:\\");

            if (!File.Exists(defaultPlacement.FullName))
            {
                defaultPlacement = new DirectoryInfo(@"C:\Program Files (x86)\Steam\steamapps\common\ELDEN RING\Game");

                while (!Directory.Exists(defaultPlacement.FullName))
                {
                    var directory = defaultPlacement.Parent;

                    if (directory == null)
                    {
                        defaultPlacement = new DirectoryInfo("C:\\");
                        break;
                    }
                    else
                    {
                        defaultPlacement = directory;
                    }
                }
            }

            openFileDialog1.InitialDirectory = defaultPlacement.FullName; 
            openFileDialog1.Filter = "Elden Ring(*.exe, *.bat)|*.exe;*.bat";
            openFileDialog1.FilterIndex = 0;

            bool success = openFileDialog1.ShowDialog() ?? false;

            if (!success) return;

            string selectedFileName = openFileDialog1.FileName;

            this.TbInstallLoc.Text = selectedFileName;
            ConfigManager.InstallLoc = selectedFileName;
        }

        private void BtnSaveFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            string defaultPlacement = ConfigManager.SaveLoc;

            if (string.IsNullOrWhiteSpace(defaultPlacement) || !File.Exists(defaultPlacement))
            {
                defaultPlacement = $@"C:\Users\{Environment.UserName}\AppData\Roaming\EldenRing";

                while (!Directory.Exists(defaultPlacement))
                {
                    var directory = Directory.GetParent(defaultPlacement);

                    if (directory == null)
                    {
                        defaultPlacement = "C:\\";
                        break;
                    }
                    else
                    {
                        defaultPlacement = directory.FullName;
                    }
                }
            }

            openFileDialog1.InitialDirectory = defaultPlacement;
            openFileDialog1.Filter = "Elden Ring save files (*.co2, *.sl2)|*.co2;*.sl2";
            openFileDialog1.FilterIndex = 0;

            bool success = openFileDialog1.ShowDialog() ?? false;

            if (!success) return;

            string selectedFileName = openFileDialog1.FileName;

            this.TbSaveFile.Text = selectedFileName;
            ConfigManager.SaveLoc = selectedFileName;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnAccept_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}