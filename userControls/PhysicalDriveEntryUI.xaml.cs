using GUIForDiskpart.diskpart;
using GUIForDiskpart.main;
using GUIForDiskpart.windows;
using System;
using System.Windows;
using System.Windows.Controls;


namespace GUIForDiskpart
{
    /// <summary>
    /// Interaktionslogik für PhysicalDriveEntryUI.xaml
    /// </summary>
    public partial class PhysicalDiskEntryUI : UserControl
    {
        MainWindow mainWindow;

        DiskInfo diskInfo;

        public uint DiskIndex { get { return diskInfo.DiskIndex; } }

        public PhysicalDiskEntryUI()
        {
            InitializeComponent();

            Initialize();
        }

        private void Initialize()
        {
            mainWindow = (MainWindow)Application.Current.MainWindow;
        }

        public void AddDriveInfo(DiskInfo physicalDisk)
        {
            this.diskInfo = physicalDisk;
            DriveDataToThisUI();
        }

        private void DriveDataToThisUI()
        {
            DriveNumberValueLabel.Content = diskInfo.DiskIndex.ToString();
            DiskNameValueLabel.Content = diskInfo.DiskName;

            ByteFormatter byteFormatter = new ByteFormatter();
            TotalSpaceValueLabel.Content = byteFormatter.FormatBytes((long)diskInfo.TotalSpace);

            StatusValueLabel.Content = diskInfo.MediaStatus;
            PartitionsValueLabel.Content = diskInfo.PartitionCount;

            foreach (WSMPartition wsmPartition in diskInfo.WSMPartitions)
            {
                PartitionDriveEntryUI partitionDriveEntryUI = new PartitionDriveEntryUI();
                Console.WriteLine("COUNT: " + diskInfo.WSMPartitions.Count);
                partitionDriveEntryUI.AddPartitionInfo(wsmPartition);
                PartitionStackPanel.Children.Add(partitionDriveEntryUI);
            }

            if (diskInfo.UnpartSpace > 0) 
            {
                Console.WriteLine(diskInfo.UnpartSpace.ToString());
                FreeSpaceEntryUI freeSpaceEntryUI = new FreeSpaceEntryUI(diskInfo.UnpartSpace, diskInfo);
                PartitionStackPanel.Children.Add(freeSpaceEntryUI);
            }
        }

        private void Detail_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.AddTextToOutputConsole(DPFunctions.DetailDisk(diskInfo.DiskIndex));
        }

        private void Clean_Click(object sender, RoutedEventArgs e)
        {
            //Still needs "clean all" option

            MessageBoxResult messageBoxResult = MessageBox.Show("This will delete and also clean everything on this drive!",
               "Are you sure?", MessageBoxButton.OKCancel, MessageBoxImage.Warning);

            if (messageBoxResult == MessageBoxResult.OK) 
            {
                string output = string.Empty;

                output = DPFunctions.Clean(diskInfo.DiskIndex, false);

                mainWindow.AddTextToOutputConsole(output);

                return;
            }
            else
            {
                return;
            }
        }

        private void Convert_Click(object sender, RoutedEventArgs e)
        {
            ConvertDriveWindow convertDriveWindow = new ConvertDriveWindow(diskInfo);
            convertDriveWindow.Owner = mainWindow;
            convertDriveWindow.Focus();
            
            convertDriveWindow.Show();
        }

        private void CreatePart_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CreateVolume_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CreateVDisk_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Format_Click(object sender, RoutedEventArgs e)
        {
            FormatDriveWindow formatWindow = new FormatDriveWindow(diskInfo);
            formatWindow.Owner = mainWindow;
            formatWindow.Focus();

            formatWindow.Show();
        }

        private void ChildrenCount_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Count: " + PartitionStackPanel.Children.Count);
        }
    }
}
