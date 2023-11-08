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

        public void DiskEntry_Click(DiskInfo diskInfo)
        {
            EntryData.ItemsSource = diskInfo.GetKeyValuePairs();
            AddPartitionsToStackPanel(diskInfo);
        }

        public void PartitionEntry_Click(PartitionEntryUI entry)
        {
            EntryData.ItemsSource = entry.WSMPartition.GetKeyValuePairs();
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

            AddDisksToStackPanel();

            if (outputText) 
            { 
                AddTextToOutputConsole(DiskRetriever.GetDiskOutput());
            }
        }

        private void AddDisksToStackPanel()
        {
            DriveStackPanel.Children.Clear();

            foreach (DiskInfo physicalDisk in DiskRetriever.PhysicalDrives)
            {
                PhysicalDiskEntryUI diskListEntry = new PhysicalDiskEntryUI();
                diskListEntry.DiskInfo = physicalDisk;

                DriveStackPanel.Children.Add(diskListEntry);
            }
            PhysicalDiskEntryUI entry = (PhysicalDiskEntryUI)DriveStackPanel.Children[0];
            entry.SelectEntryRadioButton();
        }

        private void AddPartitionsToStackPanel(DiskInfo diskInfo)
        {
            PartitionStackPanel.Children.Clear();

            foreach (WSMPartition wsmPartition in diskInfo.WSMPartitions)
            {
                PartitionEntryUI partitionEntry = new PartitionEntryUI();
                partitionEntry.WSMPartition = wsmPartition;

                PartitionStackPanel.Children.Add(partitionEntry);
            }
        }

        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void SaveLog_Click(object sender, RoutedEventArgs e)
        {
            string log = ConsoleReturn.TextBox.Text;

            SaveAsTextfile(log, "log");
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
            string entrieString = string.Empty;

            bool noneSelected = false;

            if (EntryData.SelectedCells.Count == 0)
            {
                EntryData.SelectAllCells();
                noneSelected = true;
            }

            foreach (var entry in EntryData.SelectedCells)
            {
                entrieString += entry.Item + "\n";
            }

            if (noneSelected) 
            {
                EntryData.UnselectAllCells();
            }

            SaveAsTextfile(entrieString, "data");
        }

        private void SaveAsTextfile(string text, string name)
        {
            //should be a static class
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text file (*.txt)|*.txt";

            string currentDateTime = DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss");
            saveFileDialog.FileName = $"guifd_{name}_{currentDateTime}";

            if (saveFileDialog.ShowDialog() == true)
            {
                System.IO.File.WriteAllText(saveFileDialog.FileName, text);
            }
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            EntryData.SelectAllCells();

            List<DataGridCellInfo> cells = new List<DataGridCellInfo>();

            foreach (var entry in EntryData.SelectedCells)
            {
                if (entry.Item.ToString().Contains(SearchBar.Text))
                {
                    cells.Add(entry);
                }
            }

            EntryData.UnselectAllCells();

            if (cells.Count > 0) 
            {
                foreach (var cell in cells) 
                {
                    EntryData.SelectedCells.Add(cell);
                }
            }
            
            if (string.IsNullOrEmpty(SearchBar.Text))
            { 
                EntryData.UnselectAllCells(); 
            }
        }

        private void SearchBar_CursorFocus(object sender, DependencyPropertyChangedEventArgs e) 
        {
            if (!SearchBar.IsFocused)
            {
                SearchBar.Text = "";
            }
        }

        /*
        private void CopySelectedRow_Click(object sender, RoutedEventArgs e)
        {
            string copyToClipboard = string.Empty;

            foreach (var entry in EntryData.SelectedItems)
            {
                copyToClipboard += entry.ToString() + '\n';
            }

            Clipboard.SetDataObject(copyToClipboard);
        }

        private void CopySelectedProperty_Click(object sender, RoutedEventArgs e)
        {
            string copyToClipboard = string.Empty;

            foreach (KeyValuePair<string, object?> entry in EntryData.SelectedItems)
            {
                copyToClipboard += entry.Key + "\n";
            }

            Clipboard.SetDataObject(copyToClipboard);
        }

        private void CopySelectedValue_Click(object sender, RoutedEventArgs e)
        {
            
            string copyToClipboard = string.Empty;

            foreach (KeyValuePair<string, object?> entry in EntryData.SelectedItems)
            {
                copyToClipboard += entry.Value + "\n";
            }

            Clipboard.SetDataObject(copyToClipboard);
        }
        */

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
