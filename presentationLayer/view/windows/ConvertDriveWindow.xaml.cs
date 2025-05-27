using GUIForDiskpart.Database.Data.diskpart;
using GUIForDiskpart.Model.Logic.Diskpart;
using GUIForDiskpart.ModelLayer;
using System.Windows;
using System.Windows.Controls;

namespace GUIForDiskpart.Presentation.View.Windows
{
    /// <summary>
    /// Interaktionslogik für ConvertDriveWindow.xaml
    /// </summary>
    public partial class ConvertDriveWindow : Window
    {
        MainWindow MainWindow => (MainWindow)Application.Current.MainWindow;

        private DiskInfo diskInfo;
        public DiskInfo DiskInfo
        {
            get { return diskInfo; } 
            set 
            {
                diskInfo = value;
                AddTextToConsole();
            }
        }

        public ConvertDriveWindow(DiskInfo disk)
        {
            InitializeComponent();

            DiskInfo = disk;
        }

        public void AddTextToConsole()
        {
            ConsoleReturn.AddTextToOutputConsole(diskInfo.GetOutputAsString());
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

            MainWindow.AddTextToOutputConsole(output);

            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
