using System;
using System.Windows.Controls;

using GUIForDiskpart.Model.Logic;


namespace GUIForDiskpart.Presentation.Presenter.Windows.Components
{
    public class PCEasyFormat : PCFormatBase
    {
        WEasyFormat window;
        PEasyFormat presenter;

        public PCEasyFormat(WEasyFormat window, PEasyFormat presenter)
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
        public string SelectedFileSystemAsString()
        {
            return (string)((ComboBoxItem)window.FileSystemValue.SelectedValue).Content;
        }

        public void EvaluteFAT32SizeBox()
        {
            ClearErrorMessage();
            if (SelectedFileSystemAsString() != FS_FAT32) return;

            UInt64 size = presenter.GetSizeValue();

            if (size <= FAT32_Max)
            {
                window.ConfirmButton.IsEnabled = true;
                ClearErrorMessage();
            }
            else
            {
                window.ConfirmButton.IsEnabled = false;
                SetErrorMessage(FAT32_OVER_SIZE);
            }

            if (size == 0)
            {
                SetErrorMessage(FAT32_SIZE0);
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

            if (SelectedFileSystemAsString() == FS_NTFS)
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
