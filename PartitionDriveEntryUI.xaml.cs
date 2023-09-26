using GUIForDiskpart.diskpart;
using GUIForDiskpart.main;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace GUIForDiskpart
{
    /// <summary>
    /// Interaktionslogik für PartitionDriveEntryUI.xaml
    /// </summary>
    public partial class PartitionDriveEntryUI : UserControl
    {
        MainWindow mainWindow;
        DPFunctions dpFunctions;

        PartitionInfo partitionInfo;
        int driveIndex = 0;

        public PartitionDriveEntryUI()
        {
            InitializeComponent();

            Initialize();
        }

        private void Initialize()
        {
            mainWindow = (MainWindow)Application.Current.MainWindow;
            dpFunctions = mainWindow.mainProgram.dpFunctions;
        }

        public void AddPartitionInfo(PartitionInfo partitionInfo, int driveIndex)
        {
            this.partitionInfo = partitionInfo;
            this.driveIndex = driveIndex;
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

        private void Detail_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.AddTextToOutputConsole(dpFunctions.Detail(DPListType.PARTITION, driveIndex));
        }

        private void Format_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Assign_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
