using System;
using System.Windows;
using System.Windows.Controls;

using GUIForDiskpart.Model.Logic;
using GUIForDiskpart.Presentation.Presenter;


namespace GUIForDiskpart.Presentation.View.Windows
{
    /// <summary>
    /// Interaktionslogik für FormatWindow.xaml
    /// </summary>
    public partial class FormatDriveWindow : Window
    {
        PMainWindow<GUIFDMainWin> MainWindow = App.Instance.WIM.GetPresenter<PMainWindow<GUIFDMainWin>>();

        private DiskModel diskModel;
        public DiskModel DiskModel
        {
            get { return diskModel; }
            set
            {
                diskModel = value;
                AddTextToConsole();
            }
        }

        public FormatDriveWindow(DiskModel disk)
        {
            InitializeComponent();

            diskModel = disk;
        }

        public void AddTextToConsole()
        {
            ConsoleReturn.Print(DiskModel.GetOutputAsString());
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
            string todo = "Format the whole drive! ALL DATA WILL BE LOST!";
            string confirmKey = DiskModel.PhysicalName;

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

            UInt64 size = GetSizeValue();

            if (size == 0 && fileSystem == FSType.FAT32)
            {
                size = 32768;
            }

            string output = string.Empty;


            if (DriveLetterValue.Text == "")
            {
                output = ComfortFeatures.EasyDiskFormat(diskModel, fileSystem, VolumeValue.Text,
                    size, (bool)QuickFormattingValue.IsChecked, (bool)CompressionValue.IsChecked, false, true, false);
            }
            else
            {
                char driveLetter = DriveLetterValue.Text.ToCharArray()[0];

                output = ComfortFeatures.EasyDiskFormat(diskModel, fileSystem, VolumeValue.Text,
                    driveLetter, size, (bool)QuickFormattingValue.IsChecked, (bool)CompressionValue.IsChecked, false, true, false);
            }

            MainWindow.Log.Print(output);

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
            ClearErrorMessage();
            if (SelectedFileSystemAsString() != "FAT32") return;

            UInt64 size = GetSizeValue();

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
            if (ErrorMessageValue == null) return;
            ErrorMessageValue.Content = " ";
        }

        private void SizeValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            EvaluteFAT32SizeBox();
        }
    }
}
