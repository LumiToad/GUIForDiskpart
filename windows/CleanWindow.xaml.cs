using GUIForDiskpart.diskpart;
using GUIForDiskpart.main;
using System.Windows;

namespace GUIForDiskpart.windows
{
    /// <summary>
    /// Interaction logic for CleanWindow.xaml
    /// </summary>
    public partial class CleanWindow : Window
    {
        MainWindow MainWindow => (MainWindow)Application.Current.MainWindow;

        private DiskInfo diskInfo;
        public DiskInfo DiskInfo
        {
            get { return diskInfo; }
            set
            {
                diskInfo = value;
                AddTextToConsole(diskInfo.GetOutputAsString());
            }
        }

        public CleanWindow(DiskInfo diskInfo)
        {
            InitializeComponent();

            DiskInfo = diskInfo;
        }

        private void ExecuteClean(bool value)
        {
            if (!value) return;

            string output = string.Empty;

            output += DPFunctions.Clean(DiskInfo.DiskIndex, (bool)CleanAll.IsChecked);

            MainWindow.AddTextToOutputConsole(output);
            MainWindow.RetrieveAndShowDiskData(false);

            this.Close();
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            string todo = "Clean the whole drive! ALL DATA WILL BE LOST!";
            if ((bool)CleanAll.IsChecked)
            {
                todo = "Clean and override the whole drive! MAKES DATA RESCUE CLOSE TO IMPOSSIBLE!";
            }
            string confirmKey = DiskInfo.DiskName;

            SecurityCheckWindow securityCheckWindow = new SecurityCheckWindow(ExecuteClean,todo, confirmKey);
            securityCheckWindow.Owner = this;
            securityCheckWindow.Show();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AddTextToConsole(string text)
        {
            ConsoleReturn.AddTextToOutputConsole(text);
        }
    }
}
