using System;
using System.Management;
using System.Windows;
using GUIForDiskpart.main;

namespace GUIForDiskpart
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainProgram mainProgram;

        public MainWindow()
        {
            InitializeComponent();

            Initialize();
        }

        private void Initialize()
        {
            mainProgram = new MainProgram();
            mainProgram.Initialize();
            
        }

        private void ConsoleReturn_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            ConsoleReturn.ScrollToEnd();
        }

        private void AddTextToOutputConsole(string text)
        {
            ConsoleReturn.Text += text;
        }

        private void ListVolume_Click(object sender, RoutedEventArgs e)
        {
            ConsoleReturn.Text += mainProgram.dpFunctions.List(diskpart.DPListType.VOLUME);
        }


        private void ListDisk_Click(object sender, RoutedEventArgs e)
        {
            ConsoleReturn.Text += mainProgram.dpFunctions.List(diskpart.DPListType.DISK);

        }

        private void ListPart_Click(object sender, RoutedEventArgs e)
        {
            ConsoleReturn.Text += mainProgram.dpFunctions.List(diskpart.DPListType.PARTITION);

        }

        private void ListVdisk_Click(object sender, RoutedEventArgs e)
        {
            ConsoleReturn.Text += mainProgram.dpFunctions.List(diskpart.DPListType.VDISK);

        }

        private void RetrieveDriveData_Click(object sender, RoutedEventArgs e)
        {
            mainProgram.driveRetriever.RetrieveDrives();
            AddTextToOutputConsole(mainProgram.driveRetriever.GetLogicalDrivesOutput());
        }

        private void ListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private void DriveListEntry_Loaded(object sender, RoutedEventArgs e)
        {
            LogicalDrive logicalDrive = mainProgram.driveRetriever.LogicalDrives[0];
            DriveListEntryElement.AddLogicalDriveData(logicalDrive.DriveNumber, logicalDrive.DiskName, logicalDrive.TotalSpace, logicalDrive.MediaStatus);
        }
    }
}
