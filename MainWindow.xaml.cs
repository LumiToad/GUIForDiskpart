using System;
using System.Windows;
using GUIForDiskpart.main;

namespace GUIForDiskpart
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainProgram mainProgram;

        public MainWindow()
        {
            InitializeComponent();
            Initialize();
        }

        private void Initialize()
        {
            mainProgram = new MainProgram();
            RetrieveAndShowDriveData();
        }

        private void ConsoleReturn_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            ConsoleReturn.ScrollToEnd();
        }

        public void AddTextToOutputConsole(string text)
        {
            ConsoleReturn.Text += "\n";
            ConsoleReturn.Text += "[" + DateTime.Now + "]\n";
            ConsoleReturn.Text += text;
        }

        private void ListVolume_Click(object sender, RoutedEventArgs e)
        {
            AddTextToOutputConsole(mainProgram.dpFunctions.List(diskpart.DPListType.VOLUME));
        }


        private void ListDisk_Click(object sender, RoutedEventArgs e)
        {
            AddTextToOutputConsole(mainProgram.dpFunctions.List(diskpart.DPListType.DISK));

        }

        private void ListPart_Click(object sender, RoutedEventArgs e)
        {
            AddTextToOutputConsole(mainProgram.dpFunctions.List(diskpart.DPListType.PARTITION));

        }

        private void ListVdisk_Click(object sender, RoutedEventArgs e)
        {
            AddTextToOutputConsole(mainProgram.dpFunctions.List(diskpart.DPListType.VDISK));

        }

        private void RetrieveDriveData_Click(object sender, RoutedEventArgs e)
        {
            RetrieveAndShowDriveData();
        }

        private void RetrieveAndShowDriveData()
        {
            mainProgram.driveRetriever.ReloadDriveInformation();
            AddTextToOutputConsole(mainProgram.driveRetriever.GetDrivesOutput());
            AddLogicalDrivesToStackPanel();
        }

        private void AddLogicalDrivesToStackPanel()
        {
            someStackPanel.Children.Clear();

            foreach (PhysicalDrive physicalDrive in mainProgram.driveRetriever.PhysicalDrives)
            {
                PhysicalDriveEntryUI driveListEntry = new PhysicalDriveEntryUI();
                driveListEntry.AddPhysicalDriveData(physicalDrive);

                someStackPanel.Children.Add(driveListEntry);
            }
        }
    }
}
