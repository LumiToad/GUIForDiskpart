using System;
using System.Windows.Controls;


namespace GUIForDiskpart
{
    /// <summary>
    /// Interaktionslogik für DriveListEntry.xaml
    /// </summary>
    public partial class DriveListEntry : UserControl
    {
        public DriveListEntry()
        {
            InitializeComponent();
        }

        public void AddLogicalDriveData(int driveNumber, string diskName, UInt64 totalSpace, string mediaStatus)
        {
            DriveNumberValueLabel.Content = driveNumber.ToString();
            DiskNameValueLabel.Content = diskName;

            totalSpace = totalSpace / 1000;
            totalSpace = totalSpace / 1000;
            totalSpace = totalSpace / 1000;

            TotalSpaceValueLabel.Content = totalSpace;

            StatusValueLabel.Content = mediaStatus;
        }
    }
}
