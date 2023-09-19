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

        LogicalDrive logicalDrive;

        public int DriveNumber { get { return logicalDrive.DriveNumber; } }

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

        public void AddLogicalDriveData(LogicalDrive logicalDrive)
        {
            this.logicalDrive = logicalDrive;
            DriveDataToThisUI();
        }

        private void DriveDataToThisUI()
        {
            DriveNumberValueLabel.Content = logicalDrive.DriveNumber.ToString();
            DiskNameValueLabel.Content = logicalDrive.DiskName;

            ByteFormatter byteFormatter = new ByteFormatter();
            TotalSpaceValueLabel.Content = byteFormatter.FormatBytes((long)logicalDrive.TotalSpace);

            StatusValueLabel.Content = logicalDrive.MediaStatus;
            PartitionsValueLabel.Content = logicalDrive.Partitions;
        }

        private void Detail_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.AddTextToOutputConsole(dpFunctions.Detail(DPListType.DISK, logicalDrive.DriveNumber));
        }
    }
}
