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

        DriveInfo physicalDrive;

        public int DriveNumber { get { return physicalDrive.DriveNumber; } }

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

        public void AddPhysicalDriveData(DriveInfo physicalDrive)
        {
            this.physicalDrive = physicalDrive;
            DriveDataToThisUI();
        }

        private void DriveDataToThisUI()
        {
            DriveNumberValueLabel.Content = physicalDrive.DriveNumber.ToString();
            DiskNameValueLabel.Content = physicalDrive.DiskName;

            ByteFormatter byteFormatter = new ByteFormatter();
            TotalSpaceValueLabel.Content = byteFormatter.FormatBytes((long)physicalDrive.TotalSpace);

            StatusValueLabel.Content = physicalDrive.MediaStatus;
            PartitionsValueLabel.Content = physicalDrive.PartitionCount;
        }

        private void Detail_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.AddTextToOutputConsole(dpFunctions.Detail(DPListType.DISK, physicalDrive.DriveNumber));
        }
    }
}
