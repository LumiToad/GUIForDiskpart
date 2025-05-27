using GUIForDiskpart.Database.Data.diskpart;
using GUIForDiskpart.Model.Logic.Diskpart;
using GUIForDiskpart.ModelLayer;
using System;
using System.Windows;
using System.Windows.Controls;

namespace GUIForDiskpart.Presentation.View.Windows
{
    /// <summary>
    /// Interaction logic for CreateVolumeWindow.xaml
    /// </summary>
    public partial class CreateVolumeWindow : Window
    {
        MainWindow MainWindow => (MainWindow)Application.Current.MainWindow;

        private long sizeInMB;

        private DiskInfo diskInfo;
        public DiskInfo DiskInfo
        {
            get { return diskInfo; }
            set
            {
                diskInfo = value;
                AddTextToConsole();
            }
        }

        public CreateVolumeWindow(DiskInfo disk)
        {
            InitializeComponent();
            DiskInfo = disk;
        }

        public CreateVolumeWindow(DiskInfo disk, long size)
        {
            InitializeComponent();

            size /= 1024;
            size /= 1024;
            this.sizeInMB = size;
            DiskInfo = disk;
            SizeValue.Text = size.ToString();
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            string output = string.Empty;

            output += DPFunctions.CreateVolume(diskInfo.DiskIndex, CreateVolumeOptions.SIMPLE, GetSizeValue(), false);

            MainWindow.AddTextToOutputConsole(output);
            MainWindow.RetrieveAndShowDiskData(false);

            this.Close();
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

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void AddTextToConsole()
        {
            ConsoleReturn.AddTextToOutputConsole(DiskInfo.GetOutputAsString());
        }

        private void SizeValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SizeValue.Text.Length == 0) return;

            long enteredSize = Convert.ToInt64(SizeValue.Text);
            if (enteredSize > sizeInMB)
            {
                SizeValue.Text = sizeInMB.ToString();
            }
        }
    }
}
