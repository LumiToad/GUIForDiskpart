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
    public partial class FormatWindow : Window
    {
        private MainWindow mainWindow;
        private MainProgram mainProgram;

        private DriveInfo driveInfo;

        public FormatWindow(DriveInfo drive)
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
            //if (FileSystemValue.SelectedItem.ToString() == "NTFS")
            //{
            //    CompressionValue.IsEnabled = true;
            //}
            //else
            //{
            //    CompressionValue.IsEnabled = false;
            //}
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            FileSystem fileSystem = FileSystem.NTFS;

            switch (FileSystemValue.SelectedItem.ToString()) 
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

            //cant use partition count, need to do it for each partition number individually

            if (DriveLetterValue.Text == "")
            {
                output = mainProgram.comfortFunctions.FormatWholeDrive((int)driveInfo.DriveNumber, (int)driveInfo.PartitionInfos.Count + 1,
                fileSystem, VolumeValue.Text, Convert.ToUInt64(SizeValue.Text), (bool)QuickFormattingValue.IsChecked, (bool)CompressionValue.IsChecked, false, true, false);
            }
            else
            {
                char driveLetter = DriveLetterValue.Text.ToCharArray()[0];

                output = mainProgram.comfortFunctions.FormatWholeDrive((int)driveInfo.DriveNumber, (int)driveInfo.PartitionInfos.Count + 1,
                fileSystem, VolumeValue.Text, driveLetter, Convert.ToUInt64(SizeValue.Text), (bool)QuickFormattingValue.IsChecked, (bool)CompressionValue.IsChecked, false, true, false);
            }

            

            mainWindow.AddTextToOutputConsole(output);
            
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
