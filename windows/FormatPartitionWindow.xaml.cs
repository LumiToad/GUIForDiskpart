using GUIForDiskpart.diskpart;
using GUIForDiskpart.main;
using System;
using System.Windows;
using System.Windows.Controls;

namespace GUIForDiskpart.windows
{
    /// <summary>
    /// Interaction logic for FormatPartitionWindow.xaml
    /// </summary>
    public partial class FormatPartitionWindow : Window
    {
        private MainWindow mainWindow;

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

            mainWindow = (MainWindow)Application.Current.MainWindow;
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

            SecurityCheckWindow securityCheckWindow = new SecurityCheckWindow(todo, confirmKey);
            securityCheckWindow.Owner = this;
            securityCheckWindow.OnClick += ExecuteFormat;
            securityCheckWindow.Show();
        }

        private void ExecuteFormat(bool value)
        {
            if (!value) return;

            FileSystem fileSystem = FileSystem.NTFS;

            Console.WriteLine(SelectedFileSystemAsString());

            switch (SelectedFileSystemAsString())
            {
                case ("NTFS"):
                    fileSystem = FileSystem.NTFS;
                    break;
                case ("FAT32"):
                    fileSystem = FileSystem.FAT32;
                    break;
                case ("exFAT"):
                    fileSystem = FileSystem.exFAT;
                    break;
            }

            UInt64 size = GetSizeValue();

            if (size == 0 && fileSystem == FileSystem.FAT32)
            {
                size = 32768;
            }

            string output = string.Empty;

            output = DPFunctions.Format(WSMPartition.DiskNumber, wsmPartition.PartitionNumber, fileSystem,
                    VolumeValue.Text, (bool)QuickFormattingValue.IsChecked, (bool)CompressionValue.IsChecked, false, true, false);

            mainWindow.AddTextToOutputConsole(output);

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

        private UInt64 GetSizeValue()
        {
            UInt64 size = 0;

            if (SizeValue.Text != "")
            {
                UInt64.TryParse(SizeValue.Text, out size);
            }

            return size;
        }

        private void EvaluteFAT32SizeBox()
        {
            if (SelectedFileSystemAsString() != "FAT32") return;

            UInt64 size = GetSizeValue();
            Console.WriteLine(size.ToString());
            Console.WriteLine(size);


            if (size <= 32768)
            {
                ConfirmButton.IsEnabled = true;
                ClearErrorMessage();
            }
            else
            {
                ConfirmButton.IsEnabled = false;
                SetErrorMessage("ERROR: FAT32 max size is 32768 MB!");
            }

            if (size == 0)
            {
                SetErrorMessage("Size will be 32768 MB -> FAT32 maximum.");
            }
        }

        private void SetErrorMessage(string message)
        {
            ErrorMessageValue.Content = message;
        }

        private void ClearErrorMessage()
        {
            ErrorMessageValue.Content = "";
        }

        private void SizeValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            EvaluteFAT32SizeBox();
        }
    }
}
