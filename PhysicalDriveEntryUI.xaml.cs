using GUIForDiskpart.diskpart;
using GUIForDiskpart.main;
using System;
using System.Windows;
using System.Windows.Controls;


namespace GUIForDiskpart
{
    /// <summary>
    /// Interaktionslogik für PhysicalDriveEntryUI.xaml
    /// </summary>
    public partial class PhysicalDriveEntryUI : UserControl
    {
        MainWindow mainWindow;
        DPFunctions dpFunctions;

        DriveInfo driveInfo;

        public int DriveNumber { get { return driveInfo.DriveNumber; } }

        public PhysicalDriveEntryUI()
        {
            InitializeComponent();

            Initialize();
        }

        private void Initialize()
        {
            mainWindow = (MainWindow)Application.Current.MainWindow;
            dpFunctions = mainWindow.mainProgram.dpFunctions;
        }

        public void AddDriveInfo(DriveInfo physicalDrive)
        {
            this.driveInfo = physicalDrive;
            DriveDataToThisUI();
        }

        private void DriveDataToThisUI()
        {
            DriveNumberValueLabel.Content = driveInfo.DriveNumber.ToString();
            DiskNameValueLabel.Content = driveInfo.DiskName;

            ByteFormatter byteFormatter = new ByteFormatter();
            TotalSpaceValueLabel.Content = byteFormatter.FormatBytes((long)driveInfo.TotalSpace);

            StatusValueLabel.Content = driveInfo.MediaStatus;
            PartitionsValueLabel.Content = driveInfo.PartitionCount;

            foreach (PartitionInfo partitionInfo in driveInfo.PartitionInfos)
            {
                PartitionDriveEntryUI partitionDriveEntryUI = new PartitionDriveEntryUI();
                partitionDriveEntryUI.AddPartitionInfo(partitionInfo);
                PartitionStackPanel.Children.Add(partitionDriveEntryUI);
            }
        }

        private void Detail_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.AddTextToOutputConsole(dpFunctions.Detail(DPListType.DISK, driveInfo.DriveNumber));
        }

        private void Format_Click(object sender, RoutedEventArgs e)
        {
            FormatWindow formatWindow = new FormatWindow(driveInfo);

            formatWindow.Show();
        }

        private void ChildrenCount_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Count: " + PartitionStackPanel.Children.Count);
        }
    }
}
