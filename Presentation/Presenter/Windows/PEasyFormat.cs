global using PEasyFormat =
   GUIForDiskpart.Presentation.Presenter.Windows.PEasyFormat<GUIForDiskpart.Presentation.View.Windows.WEasyFormat>;

using System;
using System.Windows;
using System.Windows.Controls;

using GUIForDiskpart.Model.Logic;



namespace GUIForDiskpart.Presentation.Presenter.Windows
{
    public class PEasyFormat<T> : WPresenter<T> where T : WEasyFormat
    {
        private PLog Log;

        public DiskModel DiskModel { get; private set; }

        private const string FS_NTFS = "NTFS";
        private const string FS_FAT32 = "FAT32";
        private const string FS_EXFAT = "exFAT";
        private const string FAT32_MAX_SIZE_TEXT = "32768";
        private const string FAT32_SIZE0 = $"Size will be {FAT32_MAX_SIZE_TEXT} MB -> {FS_FAT32} maximum.";
        private const string FAT32_OVER_SIZE = $"ERROR: {FS_FAT32} max size is {FAT32_MAX_SIZE_TEXT} MB!";
        private const string SEC_WIN_WARN_MSG = "Format the whole drive! ALL DATA WILL BE LOST!";

        private ulong FAT32_Max => ulong.Parse(FAT32_MAX_SIZE_TEXT);
        private void ExecuteFormat(bool value)
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

            UInt64 size = GetSizeValue();

            if (size == 0 && fileSystem == FSType.FAT32)
            {
                size = FAT32_Max;
            }

            string output = string.Empty;


            if (Window.DriveLetterValue.Text == "")
            {
                output = ComfortFeatures.EasyDiskFormat(DiskModel, fileSystem, Window.VolumeValue.Text,
                    size, (bool)Window.QuickFormattingValue.IsChecked, (bool)Window.CompressionValue.IsChecked, false, true, false);
            }
            else
            {
                char driveLetter = Window.DriveLetterValue.Text.ToCharArray()[0];

                output = ComfortFeatures.EasyDiskFormat(DiskModel, fileSystem, Window.VolumeValue.Text,
                    driveLetter, size, (bool)Window.QuickFormattingValue.IsChecked, (bool)Window.CompressionValue.IsChecked, false, true, false);
            }

            MainWindow.Log.Print(output);

            this.Close();
        }



        private string SelectedFileSystemAsString()
        {
            return (string)((ComboBoxItem)Window.FileSystemValue.SelectedValue).Content;
        }

        private UInt64 GetSizeValue()
        {
            UInt64 size = 0;

            if (Window.SizeValue.Text != "")
            {
                UInt64.TryParse(Window.SizeValue.Text, out size);
            }

            return size;
        }

        private void EvaluteFAT32SizeBox()
        {
            ClearErrorMessage();
            if (SelectedFileSystemAsString() != FS_FAT32) return;

            UInt64 size = GetSizeValue();

            if (size <= FAT32_Max)
            {
                Window.ConfirmButton.IsEnabled = true;
                ClearErrorMessage();
            }
            else
            {
                Window.ConfirmButton.IsEnabled = false;
                SetErrorMessage(FAT32_OVER_SIZE);
            }

            if (size == 0)
            {
                SetErrorMessage(FAT32_SIZE0);
            }
        }

        private void SetErrorMessage(string message)
        {
            Window.ErrorMessageValue.Content = message;
        }

        private void ClearErrorMessage()
        {
            if (Window.ErrorMessageValue == null) return;
            Window.ErrorMessageValue.Content = " ";
        }

        private void OnSizeValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            EvaluteFAT32SizeBox();
        }

        private void OnComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Window.CompressionValue == null) return;

            if (SelectedFileSystemAsString() == FS_NTFS)
            {
                Window.CompressionValue.IsEnabled = true;
            }
            else
            {
                Window.CompressionValue.IsEnabled = false;
                Window.CompressionValue.IsChecked = false;
            }

            EvaluteFAT32SizeBox();
        }

        #region OnClick

        private void OnConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            string confirmKey = DiskModel.PhysicalName;

            var secCheck = App.Instance.WIM.CreateWPresenter<PSecurityCheck>(true, SEC_WIN_WARN_MSG, confirmKey);
            secCheck.Window.Owner = Window;
            secCheck.ESecCheck += ExecuteFormat;
        }

        private void OnCancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        #endregion OnClick

        #region WPresenter

        public override void Setup()
        {
            Log.Print(DiskModel.GetOutputAsString());
        }

        protected override void AddCustomArgs(params object?[] args)
        {
            DiskModel = (DiskModel)args[0];
        }

        protected override void RegisterEventsInternal()
        {
            base.RegisterEventsInternal();

            Window.EConfirm += OnConfirmButton_Click;
            Window.ECancel += OnConfirmButton_Click;
            Window.ESelectionChanged += OnComboBox_SelectionChanged;
            Window.ETextChanged += OnSizeValue_TextChanged;
        }

        public override void InitPresenters()
        {
            Log = CreateUCPresenter<PLog>(Window.Log);
        }

        #endregion WPresenter
    }
}
