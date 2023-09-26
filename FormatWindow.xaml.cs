using GUIForDiskpart.main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
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
        private MainProgram mainProgram;

        private DriveInfo driveInfo;

        public FormatDriveWindow(DriveInfo drive)
        {
            InitializeComponent();

            driveInfo = drive;
            DriveInfoToTextBox();

            mainWindow = (MainWindow)Application.Current.MainWindow;
            mainProgram = mainWindow.mainProgram;
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
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            FileSystem fileSystem = FileSystem.NTFS;

            switch (SelectedFileSystemAsString()) 
            {
                case ("NTFS"):
                    fileSystem = FileSystem.NTFS;                    
                    break;
                case ("FAT"):
                    fileSystem = FileSystem.FAT;
                    break;
                case ("FAT32"):
                    fileSystem = FileSystem.FAT32;
                    break;
                case ("exFAT"):
                    fileSystem = FileSystem.exFAT;
                    break;
            }

            string output = string.Empty;

            if (DriveLetterValue.Text == "")
            {
                output = mainProgram.comfortFunctions.EasyDriveFormat(driveInfo, fileSystem, VolumeValue.Text,
                    Convert.ToUInt64(SizeValue.Text), (bool)QuickFormattingValue.IsChecked, (bool)CompressionValue.IsChecked, false, true, false);
            }
            else
            {
                char driveLetter = DriveLetterValue.Text.ToCharArray()[0];

                output = mainProgram.comfortFunctions.EasyDriveFormat(driveInfo, fileSystem, VolumeValue.Text,
                    driveLetter, Convert.ToUInt64(SizeValue.Text), (bool)QuickFormattingValue.IsChecked, (bool)CompressionValue.IsChecked, false, true, false);
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
    }
}
