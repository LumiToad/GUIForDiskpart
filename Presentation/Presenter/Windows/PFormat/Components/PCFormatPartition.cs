using System;
using System.Windows.Controls;

using GUIForDiskpart.Database.Data.Types;
using GUIForDiskpart.Model.Logic.Diskpart;
using GUIForDiskpart.Utils;


namespace GUIForDiskpart.Presentation.Presenter.Windows.Components
{
    public class PCFormatPartition : PCFormatBase
    {
        WFormatPartition window;
        PFormatPartition presenter;

        public PCFormatPartition(WFormatPartition window, PFormatPartition presenter)
        {
            this.window = window;
            this.presenter = presenter;
        }

        public void ExecuteFormat(bool value)
        {
            if (!value) return;

            FSType fileSystem = FSType.NTFS;

            Console.WriteLine(SelectedFileSystemAsString());

            switch (SelectedFileSystemAsString())
            {
                case (FSTypeStrings.FS_NTFS):
                    fileSystem = FSType.NTFS;
                    break;
                case (FSTypeStrings.FS_FAT32):
                    fileSystem = FSType.FAT32;
                    break;
                case (FSTypeStrings.FS_EXFAT):
                    fileSystem = FSType.exFAT;
                    break;
            }

            string output = string.Empty;

            string driveLetter = window.DriveLetterValue.Text;
            uint diskNumber = presenter.WSM.DiskNumber;
            uint partNumber = presenter.WSM.PartitionNumber;
            string newVolName = window.VolumeValue.Text;
            bool isQuickFormat = (bool)window.QuickFormattingValue.IsChecked;
            bool isCompression = (bool)window.CompressionValue.IsChecked;

            output = DPFunctions.Format(diskNumber, partNumber, fileSystem,
                    newVolName, isQuickFormat, isCompression, false, false, false);

            presenter.MainWindow.Log.Print(output);

            presenter.Close();
        }

        public string SelectedFileSystemAsString()
        {
            return (string)((ComboBoxItem)window.FileSystemValue.SelectedValue).Content;
        }

        public void EvaluteFAT32SizeBox()
        {
            ClearErrorMessage();
            if (SelectedFileSystemAsString() != FSTypeStrings.FS_FAT32) return;

            ulong inMB = ByteFormatter.BytesToUnit<ulong, ulong>(presenter.WSM.Size, Unit.MB);

            if (inMB <= FAT32_Max)
            {
                window.ConfirmButton.IsEnabled = true;
                ClearErrorMessage();
            }
            else
            {
                window.ConfirmButton.IsEnabled = false;
                SetErrorMessage(FAT32_OVER_SIZE);
            }
        }

        public void SetErrorMessage(string message)
        {
            window.ErrorMessageValue.Content = message;
        }

        public void ClearErrorMessage()
        {
            if (window.ErrorMessageValue == null) return;
            window.ErrorMessageValue.Content = " ";
        }

        public void OnComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (window.CompressionValue == null) return;

            if (SelectedFileSystemAsString() == FSTypeStrings.FS_NTFS)
            {
                window.CompressionValue.IsEnabled = true;
            }
            else
            {
                window.CompressionValue.IsEnabled = false;
                window.CompressionValue.IsChecked = false;
            }

            EvaluteFAT32SizeBox();
        }
    }
}
