using GUIForDiskpart.diskpart;
using GUIForDiskpart.main;
using System.Windows;
using System.Windows.Controls;

namespace GUIForDiskpart.windows
{
    /// <summary>
    /// Interaktionslogik für ConvertDriveWindow.xaml
    /// </summary>
    public partial class ConvertDriveWindow : Window
    {
        private MainWindow mainWindow;

        private DiskInfo diskInfo;

        public ConvertDriveWindow(DiskInfo disk)
        {
            InitializeComponent();

            diskInfo = disk;
            mainWindow = (MainWindow)Application.Current.MainWindow;
        }

        public void DriveInfoToTextBox()
        {
            DiskDetailValue.Text = diskInfo.GetOutputAsString();
        }

        private string SelectedOptionAsString()
        {
            return (string)((ComboBoxItem)ConvertOptionValue.SelectedValue).Content;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            string option = ConvertOptions.GPT;

            switch (SelectedOptionAsString())
            {
                case ("GPT"):
                    option = ConvertOptions.GPT;
                    break;
                case ("MBR"):
                    option = ConvertOptions.MBR;
                    break;
                case ("BASIC"):
                    option = ConvertOptions.BASIC;
                    break;
                case ("DYNAMIC"):
                    option = ConvertOptions.DYNAMIC;
                    break;
            }

            string output = string.Empty;
            output = DPFunctions.Convert(diskInfo.DiskIndex, option);

            mainWindow.AddTextToOutputConsole(output);

            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
