using GUIForDiskpart.Model.Data;
using GUIForDiskpart.Model.Logic;
using GUIForDiskpart.Model.Logic.Diskpart;
using System;
using System.Windows;
using System.Windows.Controls;


namespace GUIForDiskpart.Presentation.Presenter.Windows.Components
{
    public class PCFormat
    {
        public PLog Log;

        public const string FS_NTFS = "NTFS";
        public const string FS_FAT32 = "FAT32";
        public const string FS_EXFAT = "exFAT";
        public const string FAT32_MAX_SIZE_TEXT = "32768";
        public const string FAT32_SIZE0 = $"Size will be {FAT32_MAX_SIZE_TEXT} MB -> {FS_FAT32} maximum.";
        public const string FAT32_OVER_SIZE = $"ERROR: {FS_FAT32} max size is {FAT32_MAX_SIZE_TEXT} MB!";
        public const string SEC_WIN_WARN_DRIVE = "Format the whole drive! ALL DATA WILL BE LOST!";
        public const string SEC_WIN_WARN_PART = "Format the partition! ALL DATA WILL BE LOST!";

        public ulong FAT32_Max => ulong.Parse(FAT32_MAX_SIZE_TEXT);

        public void ExecuteFormat(bool value, WEasyFormat window, PEasyFormat presenter)
        {
            if (!value) return;

            FSType fileSystem = FSType.NTFS;

            Console.WriteLine(SelectedFileSystemAsString(window));

            switch (SelectedFileSystemAsString(window))
            {
                case (FS_NTFS):
                    fileSystem = FSType.NTFS;
                    break;
                case (FS_FAT32):
                    fileSystem = FSType.FAT32;
                    break;
                case (FS_EXFAT):
                    fileSystem = FSType.exFAT;
                    break;
            }

            UInt64 size = presenter.GetSizeValue();

            if (size == 0 && fileSystem == FSType.FAT32)
            {
                size = FAT32_Max;
            }

            string output = string.Empty;

            string driveLetter = window.DriveLetterValue.Text;
            string newVolName = window.VolumeValue.Text;
            bool isQuickFormat = (bool)window.QuickFormattingValue.IsChecked;
            bool isCompression = (bool)window.CompressionValue.IsChecked;

            if (driveLetter == string.Empty)
            {
                output = ComfortFeatures.EasyDiskFormat(presenter.Disk, fileSystem, newVolName,
                    size, isQuickFormat, isCompression, false, true, false);
            }
            else
            {
                output = ComfortFeatures.EasyDiskFormat(presenter.Disk, fileSystem, newVolName,
                    driveLetter.ToCharArray()[0], size, isQuickFormat, isCompression, false, true, false);
            }

            presenter.MainWindow.Log.Print(output);

            presenter.Close();
        }

        public void ExecuteFormat(bool value, WFormatPartition window, PFormatPartition presenter)
        {
            if (!value) return;

            FSType fileSystem = FSType.NTFS;

            Console.WriteLine(SelectedFileSystemAsString(window));

            switch (SelectedFileSystemAsString(window))
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

        public string SelectedFileSystemAsString(WFormatPartition window)
        {
            return (string)((ComboBoxItem)window.FileSystemValue.SelectedValue).Content;
        }

        public string SelectedFileSystemAsString(WEasyFormat window)
        {
            return (string)((ComboBoxItem)window.FileSystemValue.SelectedValue).Content;
        }

        public void EvaluteFAT32SizeBox(WSMModel wsm, WFormatPartition window, PFormatPartition presenter)
        {
            ClearErrorMessage(window);
            if (SelectedFileSystemAsString(window) != FS_FAT32) return;

            if (((wsm.Size / 1024) / 1024) <= FAT32_Max)
            {
                window.ConfirmButton.IsEnabled = true;
                ClearErrorMessage(window);
            }
            else
            {
                window.ConfirmButton.IsEnabled = false;
                SetErrorMessage(FAT32_OVER_SIZE, window);
            }
        }

        public void EvaluteFAT32SizeBox(WEasyFormat window, PEasyFormat presenter)
        {
            ClearErrorMessage(window);
            if (SelectedFileSystemAsString(window) != FS_FAT32) return;

            UInt64 size = presenter.GetSizeValue();

            if (size <= FAT32_Max)
            {
                window.ConfirmButton.IsEnabled = true;
                ClearErrorMessage(window);
            }
            else
            {
                window.ConfirmButton.IsEnabled = false;
                SetErrorMessage(FAT32_OVER_SIZE, window);
            }

            if (size == 0)
            {
                SetErrorMessage(FAT32_SIZE0, window);
            }
        }

        public void SetErrorMessage(string message, WFormatPartition window)
        {
            window.ErrorMessageValue.Content = message;
        }

        public void SetErrorMessage(string message, WEasyFormat window)
        {
            window.ErrorMessageValue.Content = message;
        }

        public void ClearErrorMessage(WFormatPartition window)
        {
            if (window.ErrorMessageValue == null) return;
            window.ErrorMessageValue.Content = " ";
        }

        public void ClearErrorMessage(WEasyFormat window)
        {
            if (window.ErrorMessageValue == null) return;
            window.ErrorMessageValue.Content = " ";
        }

        public void OnComboBox_SelectionChanged(dynamic window, dynamic presenter)
        {
            bool isTrue = (window is WEasyFormat);
            isTrue = isTrue == false ? (window is WFormatPartition) : isTrue;
            if (!isTrue) return;

            isTrue = (presenter is PEasyFormat);
            isTrue = isTrue == false ? (presenter is PFormatPartition) : isTrue;
            if (!isTrue) return;

            if (window.CompressionValue == null) return;

            if (SelectedFileSystemAsString(window) == "NTFS")
            {
                window.CompressionValue.IsEnabled = true;
            }
            else
            {
                window.CompressionValue.IsEnabled = false;
                window.CompressionValue.IsChecked = false;
            }

            if (window is WEasyFormat && presenter is PEasyFormat)
            {
                EvaluteFAT32SizeBox(window as WEasyFormat, presenter as PEasyFormat);
            }
            else if (window is WFormatPartition && presenter is PFormatPartition)
            {
                var p = presenter as PFormatPartition;
                EvaluteFAT32SizeBox(p.WSM, window as WFormatPartition, p);
            }
        }
    }
}
