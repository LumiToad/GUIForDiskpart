using GUIForDiskpart.diskpart;
using GUIForDiskpart.main;
using GUIForDiskpart.userControls;
using GUIForDiskpart.windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace GUIForDiskpart
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string WEBSITE_URL = "https://github.com/LumiToad/GUIForDiskpart";
        private const string WIKI_URL = "https://github.com/LumiToad/GUIForDiskpart/wiki";
        private const string BUILD_STAGE = "Beta";

        private StartupLoadingWindow startup;

        public MainWindow()
        {
            InitializeComponent();
            ShowStartupScreen();

            Initialize();
            
            if (startup != null)
            {
                StartupWindowClose();
                Focus();
            }
        }

        private void Initialize()
        {
            try
            {
                RetrieveAndShowDiskData(true);
                DiskRetriever.SetupDiskChangedWatcher();
                DiskRetriever.OnDiskChanged += OnDiskChanged;
            }
            catch (Exception ex)
            {
                FileUtilites.SaveExceptionCrashLog(ex);
            }
        }

        private string GetBuildNumberString()
        {
            string build = "";

            build += Assembly.GetExecutingAssembly().GetName().Version.ToString();
            build += " - " + BUILD_STAGE;

            return build;
        }

        public void AddTextToOutputConsole(string text)
        {
            ConsoleReturn.AddTextToOutputConsole(text);
        }

        #region EntriesClick

        public void DiskEntry_Click(PhysicalDiskEntryUI entry)
        {
            AddEntrysToStackPanel(PartitionStackPanel, entry.DiskInfo.Partitions);
            if (entry.DiskInfo.UnallocatedSpace > 0) 
            {
                UnallocatedEntryUI unallocatedEntryUI = new UnallocatedEntryUI(entry.DiskInfo);
                PartitionStackPanel.Children.Add(unallocatedEntryUI);
            }
            EntryDataUI.AddDataToGrid(entry.DiskInfo.GetKeyValuePairs());
        }

        public void PartitionEntry_Click(PartitionEntryUI entry)
        {
            EntryDataUI.AddDataToGrid(entry.Partition.GetKeyValuePairs());
        }

        public void UnallocatedEntry_Click(UnallocatedEntryUI entry)
        {
            EntryDataUI.AddDataToGrid(entry.Entry);
        }

        private void ListPart_Click(object sender, RoutedEventArgs e)
        {
            UInt32? index = GetDataIndexOfSelected(DiskStackPanel);
            if (index == null) return;
            AddTextToOutputConsole(DPFunctions.ListPart(index));
        }

        #endregion EntriesClick

        #region TopBarDiskPartMenu

        private void ListVolume_Click(object sender, RoutedEventArgs e)
        {
            AddTextToOutputConsole(DPFunctions.List(diskpart.DPListType.VOLUME));
        }

        private void ListDisk_Click(object sender, RoutedEventArgs e)
        {
            AddTextToOutputConsole(DPFunctions.List(diskpart.DPListType.DISK));

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

        private void AttributesVolume_Click(object sender, RoutedEventArgs e)
        {
            AttributesVolumeByIndexWindow attributesVolumeByIndexWindow = new();
            attributesVolumeByIndexWindow.Owner = this;
            attributesVolumeByIndexWindow.Focus();

            attributesVolumeByIndexWindow.Show();
        }

        #endregion TopBarDiskPartMenu

        #region TopBarCommandsMenu

        private void RetrieveDiskData_Click(object sender, RoutedEventArgs e)
        {
            RetrieveAndShowDiskData(true);
        }

        private void ScanVolume_Click(object sender, RoutedEventArgs e)
        {
            foreach (object entry in PartitionStackPanel.Children) 
            {
                if (entry is not PartitionEntryUI) return;
                if (((PartitionEntryUI)entry).IsSelected == true)
                {
                    ((PartitionEntryUI)entry).OpenScanVolumeWindow();
                }
            }
        }

        #endregion TopBarCommandsMenu

        #region StackPanelLogic

        private void AddEntrysToStackPanel<T>(StackPanel stackPanel, List<T> collection)
        {
            stackPanel.Children.Clear();

            foreach (T thing in collection)
            {
                UserControl userControl = new UserControl();

                switch (thing) 
                {
                    case DiskInfo disk:
                        PhysicalDiskEntryUI diskEntry = new PhysicalDiskEntryUI(disk);
                        userControl = diskEntry;
                        break;
                    case Partition partition:
                        PartitionEntryUI partitionEntry = new PartitionEntryUI(partition);
                        userControl = partitionEntry;
                        break;
                }
                stackPanel.Children.Add(userControl);
            }
        }

        private UInt32? GetDataIndexOfSelected(StackPanel stackPanel)
        {
            PhysicalDiskEntryUI diskEntry;
            PartitionEntryUI partitionEntry;

            foreach (object? entry in stackPanel.Children)
            {
                if (entry.GetType() == typeof(PhysicalDiskEntryUI)) 
                {
                    diskEntry = (PhysicalDiskEntryUI)entry;
                    if (diskEntry != null && diskEntry.IsSelected == true) 
                        return diskEntry.DiskInfo.DiskIndex;
                }

                if (entry.GetType() == typeof(PartitionEntryUI))
                {
                    partitionEntry = (PartitionEntryUI)entry;
                    if (partitionEntry != null && partitionEntry.IsSelected == true)
                        return partitionEntry.Partition.WSMPartition.PartitionNumber;
                }
            }

            return null;
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            StartupWindowClose();
        }

        #endregion StackPanelLogic

        #region TopBarFileMenu

        private void SaveLog_Click(object sender, RoutedEventArgs e)
        {
            ConsoleReturn.SaveLog();
        }

        private void SaveEntryData_Click(object sender, RoutedEventArgs e)
        {
            EntryDataUI.SaveEntryData_Click(sender, e);
        }

        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        #endregion TopBarFileMenu

        #region TopBarHelpMenu

        private void Website_Click(object sender, RoutedEventArgs e)
        {
            CommandExecuter.IssueCommand(ProcessType.CMD, "start " + WEBSITE_URL);
        }

        private void Wiki_Click(object sender, RoutedEventArgs e)
        {
            CommandExecuter.IssueCommand(ProcessType.CMD, "start " + WIKI_URL);
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow aboutWindow = new AboutWindow(GetBuildNumberString());
            aboutWindow.Owner = this;
            aboutWindow.Show();
        }

        #endregion TopBarHelpMenu

        #region RetrieveDisk
        private void OnDiskChanged()
        {
            RetrieveAndShowDiskData(false);
        }

        public void RetrieveAndShowDiskData(bool outputText)
        {
            Application.Current.Dispatcher.Invoke(RetrieveAndShowDiskData_Internal, outputText);
        }

        private void RetrieveAndShowDiskData_Internal(bool outputText)
        {
            DiskRetriever.ReloadDiskInformation();

            AddEntrysToStackPanel<DiskInfo>(DiskStackPanel, DiskRetriever.PhysicalDrives);

            if (outputText)
            {
                AddTextToOutputConsole(DiskRetriever.GetDiskOutput());
            }

            DiskEntry_Click((PhysicalDiskEntryUI)DiskStackPanel.Children[0]);
        }

        #endregion RetrieveDisk

        #region StartupWindow

        private void ShowStartupScreen()
        {
            Thread startupWindowThread = new Thread(new ThreadStart(StartupWindowThreadEntryPoint));
            startupWindowThread.SetApartmentState(ApartmentState.STA);
            startupWindowThread.IsBackground = true;
            startupWindowThread.Start();
        }

        private void StartupWindowClose()
        {
            if (startup.Dispatcher.CheckAccess())
            {
                startup.Close();
            }
            else
            {
                startup.Dispatcher.Invoke(DispatcherPriority.Normal, new ThreadStart(startup.Close));
            }
        }

        public void StartupWindowThreadEntryPoint()
        {
            startup = new StartupLoadingWindow();
            startup.Show();
            Dispatcher.Run();
        }

        #endregion StartupWindow
    }
}
