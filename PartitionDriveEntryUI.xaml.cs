using GUIForDiskpart.main;
using System.Windows.Controls;

namespace GUIForDiskpart
{
    /// <summary>
    /// Interaktionslogik für PartitionDriveEntryUI.xaml
    /// </summary>
    public partial class PartitionDriveEntryUI : UserControl
    {
        PartitionInfo partitionInfo;

        public PartitionDriveEntryUI()
        {
            InitializeComponent();
        }

        public void AddPartitionInfo(PartitionInfo partitionInfo)
        {
            this.partitionInfo = partitionInfo;
            PartitionDataToThisUI();
        }

        public void PartitionDataToThisUI()
        {
            PartitionNameValue.Text = partitionInfo.PartitionName;
            BootPartitionValue.Text = partitionInfo.BootPartition.ToString();
            TotalSizeValue.Text = partitionInfo.Size.ToString();
            PartitionTableValue.Text = partitionInfo.Type;

            if (partitionInfo.IsLogicalPartition())
            {
                VolumeNameValue.Text = partitionInfo.LogicalDriveInfo.VolumeName;
                DriveLetterValue.Text = partitionInfo.LogicalDriveInfo.FileSystem;
                FreeSpaceValue.Text = partitionInfo.LogicalDriveInfo.FreeSpace.ToString();
            }

        }
    }
}
