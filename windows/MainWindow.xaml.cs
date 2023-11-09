using GUIForDiskpart.diskpart;
using GUIForDiskpart.main;
using GUIForDiskpart.userControls;
using GUIForDiskpart.windows;
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

        public DiskInfo LastSelectedDisk { get; set; }
        public WSMPartition LastSelectedPartition { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            Initialize();
        }

        private void Initialize()
        {
            RetrieveAndShowDiskData(true);
            SetupDiskChangedWatcher();
        }

        private string GetBuildNumberString()
        {
            string build = "";

            build += Assembly.GetExecutingAssembly().GetName().Version.ToString();
            build += " - " + buildStage;

            return build;
        }

        public void AddTextToOutputConsole(string text)
        {
            ConsoleReturn.AddTextToOutputConsole(text);
        }

        public void DiskEntry_Click(PhysicalDiskEntryUI entry)
        {
            AddEntrysToStackPanel(PartitionStackPanel, entry.DiskInfo.WSMPartitions);
            if (entry.DiskInfo.UnpartSpace > 0) 
            {
                UnallocatedEntryUI unallocatedEntryUI = new UnallocatedEntryUI(entry.DiskInfo.FormattedUnpartSpace);
                PartitionStackPanel.Children.Add(unallocatedEntryUI);
            }
            EntryDataUI.AddDataToGrid(entry.DiskInfo.GetKeyValuePairs());
            LastSelectedDisk = entry.DiskInfo;
        }

        public void PartitionEntry_Click(PartitionEntryUI entry)
        {
            EntryDataUI.AddDataToGrid(entry.WSMPartition.GetKeyValuePairs());
            LastSelectedPartition = entry.WSMPartition;
        }

        public void UnallocatedEntry_Click(UnallocatedEntryUI entry)
        {
            EntryDataUI.AddDataToGrid(entry.Entry);
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
            UInt32? index = GetIndexOfSelected(DiskStackPanel);
            if (index == null) return;
            AddTextToOutputConsole(DPFunctions.ListPart(index));
        }

        private void ListVdisk_Click(object sender, RoutedEventArgs e)
        {
            AddTextToOutputConsole(DPFunctions.List(diskpart.DPListType.VDISK));
        }

        private void CreateVDisk_Click(object sender, RoutedEventArgs e)
        {
            //create vdisk window
        }

        private void AttachVDisk_Click(object sender, RoutedEventArgs e)
        {
            //create vdisk window
        }

        private void ChildVDisk_Click(object sender, RoutedEventArgs e)
        {
            //create vdisk window
        }

        private void CopyVDisk_Click(object sender, RoutedEventArgs e)
        {
            //create vdisk window
        }

        private void RetrieveDiskData_Click(object sender, RoutedEventArgs e)
        {
            RetrieveAndShowDiskData(true);
        }

        public void RetrieveAndShowDiskData(bool outputText)
        {
            Application.Current.Dispatcher.Invoke(RetrieveAndShowDiskData_Internal, outputText);
        }

        private void RetrieveAndShowDiskData_Internal(bool outputText)
        {
            DiskRetriever.ReloadDsikInformation();

            AddEntrysToStackPanel<DiskInfo>(DiskStackPanel, DiskRetriever.PhysicalDrives);

            if (outputText) 
            { 
                AddTextToOutputConsole(DiskRetriever.GetDiskOutput());
            }
        }

        private void AddEntrysToStackPanel<T>(StackPanel stackPanel, List<T> collection)
        {
            stackPanel.Children.Clear();

            foreach (T thing in collection)
            {
                UserControl userControl = new UserControl();

                switch (thing) 
                {
                    case DiskInfo disk:
                        PhysicalDiskEntryUI diskEntry = new PhysicalDiskEntryUI();
                        diskEntry.DiskInfo = disk;
                        userControl = diskEntry;
                        break;
                    case WSMPartition partition:
                        PartitionEntryUI partitionEntry = new PartitionEntryUI();
                        partitionEntry.WSMPartition = partition;
                        userControl = partitionEntry;
                        break;
                }
                stackPanel.Children.Add(userControl);
            }
        }

        private UInt32? GetIndexOfSelected(StackPanel stackPanel)
        {
            PhysicalDiskEntryUI disk;
            PartitionEntryUI partition;

            foreach (object? entry in stackPanel.Children)
            {
                if (entry.GetType() == typeof(PhysicalDiskEntryUI)) 
                {
                    disk = (PhysicalDiskEntryUI)entry;
                    if (disk != null && disk.IsSelected == true) return disk.DiskInfo.DiskIndex;
                }

                if (entry.GetType() == typeof(PartitionEntryUI))
                {
                    partition = (PartitionEntryUI)entry;
                    if (partition != null && partition.IsSelected == true) return partition.WSMPartition.PartitionNumber;
                }
            }

            return null;
        }

        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void SaveLog_Click(object sender, RoutedEventArgs e)
        {
            string log = ConsoleReturn.TextBox.Text;

            SaveFile.SaveAsTextfile(log, "log");
        }

        private void Website_Click(object sender, RoutedEventArgs e)
        {
            CommandExecuter.IssueCommand(ProcessType.CMD, "start " + websiteURL);
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow aboutWindow = new AboutWindow(GetBuildNumberString());
            aboutWindow.Owner = this;
            aboutWindow.Show();
        }

        private void SaveEntryData_Click(object sender, RoutedEventArgs e) 
        {
            EntryDataUI.SaveEntryData_Click(sender, e);
        }

        #region DiskChangedWatcher

        private void SetupDiskChangedWatcher()
        {
            try
            {
                WqlEventQuery query = new WqlEventQuery("SELECT * FROM Win32_VolumeChangeEvent WHERE EventType = 2 or EventType = 3");
                ManagementEventWatcher watcher = new ManagementEventWatcher();
                watcher.Query = query;
                watcher.EventArrived += new EventArrivedEventHandler(OnDiskChanged);
                watcher.Options.Timeout = TimeSpan.FromSeconds(3);
                watcher.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OnDiskChanged(object sender, EventArrivedEventArgs e)
        {
            RetrieveAndShowDiskData(false);
        }

        #endregion DiskChangedWatcher
    }
}
