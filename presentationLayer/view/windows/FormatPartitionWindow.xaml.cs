using GUIForDiskpart.Database.Data;
using GUIForDiskpart.Model.Data;
using GUIForDiskpart.Model.Logic.Diskpart;
using System;
using System.Windows;
using System.Windows.Controls;

namespace GUIForDiskpart.Presentation.View.Windows
{
    /// <summary>
    /// Interaction logic for FormatPartitionWindow.xaml
    /// </summary>
    public partial class FormatPartitionWindow : Window
    {
        MainWindow MainWindow => (MainWindow)Application.Current.MainWindow;

        private WSMPartition wsmPartition;
        public WSMPartition WSMPartition
        {
            get { return wsmPartition; }
            set
            {
                wsmPartition = value;
                AddTextToConsole();
            }
        }

        public FormatPartitionWindow(WSMPartition partition)
        {
            InitializeComponent();
            WSMPartition = partition;
        }

        public void AddTextToConsole()
        {
            ConsoleReturn.AddTextToOutputConsole(wsmPartition.GetOutputAsString());
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CompressionValue == null) return;

            if (SelectedFileSystemAsString() == "NTFS")
            {
                CompressionValue.IsEnabled = true;
            }
            else
            {
                CompressionValue.IsEnabled = false;
                CompressionValue.IsChecked = false;
            }

            EvaluteFAT32SizeBox();
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            string todo = "Format the partition! ALL DATA WILL BE LOST!";
            string confirmKey = $"Drive: {WSMPartition.DiskNumber} Partition: {WSMPartition.PartitionNumber}";

            SecurityCheckWindow securityCheckWindow = new SecurityCheckWindow(ExecuteFormat, todo, confirmKey);
            securityCheckWindow.Owner = this;
            securityCheckWindow.Show();
        }

        private void ExecuteFormat(bool value)
        {
            if (!value) return;

            FSType fileSystem = FSType.NTFS;

            Console.WriteLine(SelectedFileSystemAsString());

            switch (SelectedFileSystemAsString())
            {
                case ("NTFS"):
                    fileSystem = FSType.NTFS;
                    break;
                case ("FAT32"):
                    fileSystem = FSType.FAT32;
                    break;
                case ("exFAT"):
                    fileSystem = FSType.exFAT;
                    break;
            }

            string output = string.Empty;

            output = DPFunctions.Format(WSMPartition.DiskNumber, wsmPartition.PartitionNumber, fileSystem,
                    VolumeValue.Text, (bool)QuickFormattingValue.IsChecked, (bool)CompressionValue.IsChecked, false, false, false);

            MainWindow.AddTextToOutputConsole(output);

            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private string SelectedFileSystemAsString()
        {
            return (string)((ComboBoxItem)FileSystemValue.SelectedValue).Content;
        }

        private void EvaluteFAT32SizeBox()
        {
            ClearErrorMessage();
            if (SelectedFileSystemAsString() != "FAT32") return;

            if (((WSMPartition.Size / 1024) / 1024) <= 32768)
            {
                ConfirmButton.IsEnabled = true;
                ClearErrorMessage();
            }
            else
            {
                ConfirmButton.IsEnabled = false;
                SetErrorMessage("ERROR: FAT32 max size is 32768 MB!");
            }
        }

        private void SetErrorMessage(string message)
        {
            ErrorMessageValue.Content = message;
        }

        private void ClearErrorMessage()
        {
            if (ErrorMessageValue == null) return;
            ErrorMessageValue.Content = " ";
        }
    }
}
