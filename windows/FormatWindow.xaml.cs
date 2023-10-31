using GUIForDiskpart.main;
using System;
using System.Windows;
using System.Windows.Controls;

namespace GUIForDiskpart
{
    /// <summary>
    /// Interaktionslogik für FormatWindow.xaml
    /// </summary>
    public partial class FormatDriveWindow : Window
    {
        private MainWindow mainWindow;

        private DriveInfo driveInfo;

        public FormatDriveWindow(DriveInfo drive)
        {
            InitializeComponent();

            driveInfo = drive;
            DriveInfoToTextBox();

            mainWindow = (MainWindow)Application.Current.MainWindow;
        }

        public void DriveInfoToTextBox()
        {
            DiskDetailValue.Text = driveInfo.GetOutputAsString();
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


            if (DriveLetterValue.Text == "")
            {
                output = ComfortFeatures.EasyDriveFormat(driveInfo, fileSystem, VolumeValue.Text,
                    size, (bool)QuickFormattingValue.IsChecked, (bool)CompressionValue.IsChecked, false, true, false);
            }
            else
            {
                char driveLetter = DriveLetterValue.Text.ToCharArray()[0];

                output = ComfortFeatures.EasyDriveFormat(driveInfo, fileSystem, VolumeValue.Text,
                    driveLetter, size, (bool)QuickFormattingValue.IsChecked, (bool)CompressionValue.IsChecked, false, true, false);
            }

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
