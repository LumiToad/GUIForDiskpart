using GUIForDiskpart.diskpart;
using GUIForDiskpart.main;
using System.Windows;

namespace GUIForDiskpart.windows
{
    /// <summary>
    /// Interaction logic for AttributesWindow.xaml
    /// </summary>
    public partial class AttributesDiskWindow : Window
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

        public AttributesDiskWindow(DiskInfo diskInfo)
        {
            InitializeComponent();
            DiskInfo = diskInfo;
        }

        private void SetButton_Clear(object sender, RoutedEventArgs e)
        {
            string output = string.Empty;

            output += DPFunctions.AttributesDisk(DiskInfo.DiskIndex, true, false);

            MainWindow.AddTextToOutputConsole(output);
            this.Close();
        }

        private void ClearButton_Clear(object sender, RoutedEventArgs e)
        {
            string output = string.Empty;

            output += DPFunctions.AttributesDisk(DiskInfo.DiskIndex, false, false);

            MainWindow.AddTextToOutputConsole(output);
            this.Close();
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
