using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

using GUIForDiskpart.Database.Data.Types;
using GUIForDiskpart.Model.Logic;
using GUIForDiskpart.Model.Logic.Diskpart;
using GUIForDiskpart.Presentation.View.UserControls;
using GUIForDiskpart.Database.Data;


namespace GUIForDiskpart.Presentation.Presenter
{
    public class MainWindow : IPresenter
    {
        GUIFDMainWin MainWin;

        public MainWindow(GUIFDMainWin mainWin)
        {
            MainWin = mainWin;
        }

        // Retriever
        #region RetrieveDisk
        public void OnDiskChanged()
        {
            RetrieveAndShowDiskData(false);
        }

        // Retriever
        public void RetrieveAndShowDiskData(bool outputText)
        {
            App.Instance.Dispatcher.Invoke(RetrieveAndShowDiskData_Internal, outputText);
        }

        // Retriever
        private void RetrieveAndShowDiskData_Internal(bool outputText)
        {
            DiskService.ReLoadDisks();

            AddEntrysToStackPanel<DiskModel>(MainWin.DiskStackPanel, DiskService.PhysicalDrives);

            if (outputText)
            {
                MainWin.Log.Print(DiskService.GetDiskOutput());
            }

            //OnDiskEntry_Click((PhysicalDiskEntryUI)MainWin.DiskStackPanel.Children[0]);
        }

            #region StackPanelLogic_TESTING!!!

        private void AddEntrysToStackPanel<T>(StackPanel stackPanel, List<T> collection)
        {
            stackPanel.Children.Clear();

            foreach (T thing in collection)
            {
                UserControl userControl = new UserControl();

                switch (thing)
                {
                    case DiskModel disk:
                        PhysicalDiskEntryUI diskEntry = new PhysicalDiskEntryUI(disk);
                        userControl = diskEntry;
                        break;
                    case PartitionModel partition:
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
                        return diskEntry.DiskModel.DiskIndex;
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

        #endregion StackPanelLogic_TESTING!!!

        #endregion RetrieveDisk

        #region TopBarFileMenu

        //private void SaveLog_Click(object sender, RoutedEventArgs e)
        //{
        //    MainWin.Log.SaveLog();
        //}

        public void OnSaveEntryData_Click(object sender, RoutedEventArgs e)
        {
            MainWin.EntryDataUI.SaveEntryData_Click(sender, e);
        }

        public void OnQuit_Click(object sender, RoutedEventArgs e)
        {
            App.Instance.Shutdown();
        }

        #endregion TopBarFileMenu

        #region EntriesClick

        public void OnDiskEntry_Click(PhysicalDiskEntryUI entry)
        {
            AddEntrysToStackPanel(MainWin.PartitionStackPanel, entry.DiskModel.Partitions);
            if (entry.DiskModel.UnallocatedSpace > 0)
            {
                UnallocatedEntryUI unallocatedEntryUI = new UnallocatedEntryUI(entry.DiskModel);
                MainWin.PartitionStackPanel.Children.Add(unallocatedEntryUI);
            }
            MainWin.EntryDataUI.AddDataToGrid(entry.DiskModel.GetKeyValuePairs());

            Console.WriteLine("SOMETHING!");
        }

        public void OnPartitionEntry_Click(PartitionEntryUI entry)
        {
            MainWin.EntryDataUI.AddDataToGrid(entry.Partition.GetKeyValuePairs());
        }

        public void OnUnallocatedEntry_Click(UnallocatedEntryUI entry)
        {
            MainWin.EntryDataUI.AddDataToGrid(entry.Entry);
        }

        public void OnListPart_Click(object sender, RoutedEventArgs e)
        {
            UInt32? index = GetDataIndexOfSelected(MainWin.DiskStackPanel);
            if (index == null) return;
            MainWin.Log.Print(DPFunctions.ListPart(index));
        }

        #endregion EntriesClick

        #region TopBarDiskPartMenu

        public void OnListVolume_Click(object sender, RoutedEventArgs e)
        {
            MainWin.Log.Print(DPFunctions.List(DPList.VOLUME));
        }

        public void OnListDisk_Click(object sender, RoutedEventArgs e)
        {
            MainWin.Log.Print(DPFunctions.List(DPList.DISK));
        }

        public void OnListVDisk_Click(object sender, RoutedEventArgs e)
        {
            MainWin.Log.Print(DPFunctions.List(DPList.VDISK));
        }

        public void OnCreateVDisk_Click(object sender, RoutedEventArgs e)
        {
            //create vdisk window
        }

        public void OnAttachVDisk_Click(object sender, RoutedEventArgs e)
        {
            //create vdisk window
        }

        public void OnChildVDisk_Click(object sender, RoutedEventArgs e)
        {
            //create vdisk window
        }

        public void OnCopyVDisk_Click(object sender, RoutedEventArgs e)
        {
            //create vdisk window
        }

        public void OnAttributesVolume_Click(object sender, RoutedEventArgs e)
        {
            AttributesVolumeByIndexWindow attributesVolumeByIndexWindow = new();
            attributesVolumeByIndexWindow.Owner = MainWin;
            attributesVolumeByIndexWindow.Focus();

            attributesVolumeByIndexWindow.Show();
        }

        #endregion TopBarDiskPartMenu

        #region TopBarCommandsMenu

        public void OnRetrieveDiskData_Click(object sender, RoutedEventArgs e)
        {
            RetrieveAndShowDiskData(true);
        }

        public void OnScanVolume_Click(object sender, RoutedEventArgs e)
        {
            foreach (object entry in MainWin.PartitionStackPanel.Children)
            {
                if (entry is not PartitionEntryUI) return;
                if (((PartitionEntryUI)entry).IsSelected == true)
                {
                    ((PartitionEntryUI)entry).OpenScanVolumeWindow();
                }
            }
        }

        #endregion TopBarCommandsMenu

        #region TopBarHelpMenu

        public void OnWebsite_Click(object sender, RoutedEventArgs e)
        {
            CommandExecuter.IssueCommand(ProcessType.CMD, "start " + AppInfo.WEBSITE_URL);
        }

        public void OnWiki_Click(object sender, RoutedEventArgs e)
        {
            CommandExecuter.IssueCommand(ProcessType.CMD, "start " + AppInfo.WIKI_URL);
        }

        public void OnAbout_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow aboutWindow = new AboutWindow(AppInfo.BuildString);
            aboutWindow.Owner = MainWin;
            aboutWindow.Show();
        }

        #endregion TopBarHelpMenu

        #region IPresenter

        public void RegisterEvents()
        {
            /*
            MainWin.DiskEntry_Click += OnDiskEntryClick;
            MainWin.PartitionEntry_Click += OnPartitionEntry_Click;
            MainWin.UnallocatedEntry_Click += OnUnallocatedEntry_Click;
            MainWin.ListPart_Click += OnListPart_Click;

            MainWin.ListVolume_Click += OnListVolume_Click;
            MainWin.ListDisk_Click += OnListDisk_Click;
            MainWin.ListVDisk_Click += OnListVdisk_Click;
            MainWin.CreateVDisk_Click += OnCreateVDisk_Click;
            MainWin.AttachVDisk_Click += OnAttachVDisk_Click;
            MainWin.ChildVDisk_Click += OnChildVDisk_Click;
            MainWin.CopyVDisk_Click += OnCopyVDisk_Click;
            MainWin.AttributesVolume_Click += OnAttributesVolume_Click;

            MainWin.RetrieveDiskData_Click += OnRetrieveDiskData_Click;
            MainWin.ScanVolume_Click += OnScanVolume_Click;

            //MainWin.SaveLog_Click += SaveLog_Click;
            MainWin.SaveEntryData_Click += OnSaveEntryData_Click;
            MainWin.Quit_Click += OnQuit_Click;

            MainWin.Website_Click += OnWebsite_Click;
            MainWin.Wiki_Click += OnWiki_Click;
            MainWin.About_Click += OnAbout_Click;
            */
        }
        
        #endregion IPresenter
    }
}

