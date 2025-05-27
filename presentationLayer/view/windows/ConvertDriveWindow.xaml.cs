using GUIForDiskpart.Database.Data.Diskpart;
using GUIForDiskpart.Model.Logic.Diskpart;
using System.Windows;
using System.Windows.Controls;

namespace GUIForDiskpart.Presentation.View.Windows
{
    /// <summary>
    /// Interaktionslogik für ConvertDriveWindow.xaml
    /// </summary>
    public partial class ConvertDriveWindow : Window
    {
        Window? MainWindow = GUIForDiskpart.App.AppInstance.MainWindow;

        private DiskModel diskModel;
        public DiskModel DiskModel
        {
            get { return DiskModel; } 
            set 
            {
                diskModel = value;
                AddTextToConsole();
            }
        }

        public ConvertDriveWindow(DiskModel disk)
        {
            InitializeComponent();

            diskModel = disk;
        }

        public void AddTextToConsole()
        {
            ConsoleReturn.AddTextToOutputConsole(DiskModel.GetOutputAsString());
        }

        private string SelectedOptionAsString()
        {
            return (string)((ComboBoxItem)ConvertOptionValue.SelectedValue).Content;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            string option = DPConvert.GPT;

            switch (SelectedOptionAsString())
            {
                case ("GPT"):
                    option = DPConvert.GPT;
                    break;
                case ("MBR"):
                    option = DPConvert.MBR;
                    break;
                case ("BASIC"):
                    option = DPConvert.BASIC;
                    break;
                case ("DYNAMIC"):
                    option = DPConvert.DYNAMIC;
                    break;
            }

            string output = string.Empty;
            output = DPFunctions.Convert(DiskModel.DiskIndex, option);

            MainWindow.AddTextToOutputConsole(output);

            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
