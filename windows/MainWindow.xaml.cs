using GUIForDiskpart.diskpart;
using GUIForDiskpart.main;
using GUIForDiskpart.windows;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Management;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;


namespace GUIForDiskpart
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string websiteURL = "https://github.com/LumiToad/GUIForDiskpart";
        private const string buildStage = "Alpha";

        public MainWindow()
        {
            InitializeComponent();
            Initialize();
            Test();
        }

        private void Initialize()
        {
            RetrieveAndShowDriveData(true);
            SetupDriveChangedWatcher();
        }

        private void Test()
        {
            Console.WriteLine("test");
        }

        private string GetBuildNumberString()
        {
            string build = "";

            build += Assembly.GetExecutingAssembly().GetName().Version.ToString();
            build += " - " + buildStage;

            return build;
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
            AddTextToOutputConsole(DPFunctions.List(diskpart.DPListType.VOLUME));
        }


        private void ListDisk_Click(object sender, RoutedEventArgs e)
        {
            AddTextToOutputConsole(DPFunctions.List(diskpart.DPListType.DISK));

        }

        private void ListPart_Click(object sender, RoutedEventArgs e)
        {
            AddTextToOutputConsole(DPFunctions.List(diskpart.DPListType.PARTITION));

        }

        private void ListVdisk_Click(object sender, RoutedEventArgs e)
        {
            AddTextToOutputConsole(DPFunctions.List(diskpart.DPListType.VDISK));

        }

        private void RetrieveDriveData_Click(object sender, RoutedEventArgs e)
        {
            RetrieveAndShowDriveData(true);
        }

        public void RetrieveAndShowDriveData(bool outputText)
        {
            Application.Current.Dispatcher.Invoke(RetrieveAndShowDriveData_Internal, outputText);
        }

        private void RetrieveAndShowDriveData_Internal(bool outputText)
        {
            DriveRetriever.ReloadDriveInformation();

            AddLogicalDrivesToStackPanel();

            if (outputText) 
            { 
                AddTextToOutputConsole(DriveRetriever.GetDrivesOutput());
            }
        }

        private void AddLogicalDrivesToStackPanel()
        {
            DriveStackPanel.Children.Clear();

            foreach (DriveInfo physicalDrive in DriveRetriever.PhysicalDrives)
            {
                PhysicalDriveEntryUI driveListEntry = new PhysicalDriveEntryUI();
                driveListEntry.AddDriveInfo(physicalDrive);

                DriveStackPanel.Children.Add(driveListEntry);
            }
        }

        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void SaveLog(object sender, RoutedEventArgs e)
        {
            string log = ConsoleReturn.Text;

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text file (*.txt)|*.txt";

            string currentDateTime = DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss");
            saveFileDialog.FileName = "guifd_log_" + currentDateTime;

            if (saveFileDialog.ShowDialog() == true)
            {
                System.IO.File.WriteAllText(saveFileDialog.FileName, log);
            }
        }

        private void Website_Click(object sender, RoutedEventArgs e)
        {
            CommandExecuter.IssueCommand(ProcessType.cmd, "start " + websiteURL);
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow aboutWindow = new AboutWindow(GetBuildNumberString());
            aboutWindow.Owner = this;
            aboutWindow.Show();
        }

        private void SetupDriveChangedWatcher()
        {
            try
            {
                WqlEventQuery query = new WqlEventQuery("SELECT * FROM Win32_VolumeChangeEvent WHERE EventType = 2 or EventType = 3");
                ManagementEventWatcher watcher = new ManagementEventWatcher();
                watcher.Query = query;
                watcher.EventArrived += new EventArrivedEventHandler(OnDriveChanged);
                watcher.Options.Timeout = TimeSpan.FromSeconds(3);
                watcher.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OnDriveChanged(object sender, EventArrivedEventArgs e)
        {
            RetrieveAndShowDriveData(false);
        }
    }
}
