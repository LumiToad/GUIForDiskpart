global using PMainWindow =
    GUIForDiskpart.Presentation.Presenter.Windows.PMainWindow<GUIForDiskpart.Presentation.View.Windows.MainWindow>;

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

using GUIForDiskpart.Database.Data.Types;
using GUIForDiskpart.Model.Logic;
using GUIForDiskpart.Model.Logic.Diskpart;
using GUIForDiskpart.Presentation.View.UserControls;
using GUIForDiskpart.Database.Data;
using GUIForDiskpart.Presentation.Presenter.UserControls;
using GUIForDiskpart.Presentation.Presenter.Windows;


namespace GUIForDiskpart.Presentation.Presenter.Windows
{
    public class PMainWindow<T> : WPresenter<T> where T : GUIFDMainWin
    {
        public PLog<UCLog> Log {get; private set;}
        public PEntryData<UCEntryData> EntryData { get; private set;}

        private Dictionary<UCPhysicalDriveEntry, PPhysicalDriveEntry> K_ucPhysicalDrives_V_pPhysicalDrives = new();
        private Dictionary<UCPartitionEntry, PPartitionEntry> K_ucPartitionEntry_V_pPartitionEntry = new();
        private Dictionary<UCUnallocatedEntry, PUnallocatedEntry> K_ucUnallocatedEntry_V_pUnallocatedEntry = new();


        // Retriever
        #region RetrieveDisk
        public void OnDiskChanged()
        {
            DisplayDiskData(false);
        }

        // Retriever
        public void DisplayDiskData(bool outputText)
        {
            AddEntrysToStackPanel<DiskModel>(Window.DiskStackPanel, DiskService.PhysicalDrives);
            if (outputText)
            {
                Log.Print(DiskService.GetDiskOutput());
            }
            var ucPhysicalDrive = Window.DiskStackPanel.Children[0] as UCPhysicalDriveEntry;
            var pPhysicalDrive = K_ucPhysicalDrives_V_pPhysicalDrives[ucPhysicalDrive];
            OnDiskEntry_Click(pPhysicalDrive);
        }

        #region StackPanelLogic_TESTING!!!

        private void AddEntrysToStackPanel<T>(StackPanel stackPanel, List<T> collection)
        {
            stackPanel.Children.Clear();
            K_ucPhysicalDrives_V_pPhysicalDrives.Clear();
            K_ucPartitionEntry_V_pPartitionEntry.Clear();

            foreach (T thing in collection)
            {
                UserControl userControl = new UserControl();

                switch (thing)
                {
                    case DiskModel diskModel:
                        var ucPhysicalDrive = new UCPhysicalDriveEntry();
                        var pPhysicalDrive = CreateUCPresenter<PPhysicalDriveEntry>(ucPhysicalDrive, diskModel);
                        K_ucPhysicalDrives_V_pPhysicalDrives.Add(ucPhysicalDrive, pPhysicalDrive);
                        userControl = ucPhysicalDrive;
                        break;
                    case PartitionModel partition:
                        var ucPartitionEntry = new UCPartitionEntry();
                        var pPartitionEntry = CreateUCPresenter<PPartitionEntry>(ucPartitionEntry, partition);
                        K_ucPartitionEntry_V_pPartitionEntry.Add(ucPartitionEntry, pPartitionEntry);
                        userControl = ucPartitionEntry;
                        break;
                }
                stackPanel.Children.Add(userControl);
            }
        }

        private UInt32? GetDataIndexOfSelected(StackPanel stackPanel)
        {
            foreach (object? entry in stackPanel.Children)
            {
                if (entry.GetType() == typeof(UCPhysicalDriveEntry))
                {
                    UCPhysicalDriveEntry ucPhysicalDrive = (UCPhysicalDriveEntry)entry;
                    var pPhysicalDrive = K_ucPhysicalDrives_V_pPhysicalDrives[ucPhysicalDrive];
                    if (pPhysicalDrive != null && pPhysicalDrive.IsSelected == true)
                        return pPhysicalDrive.DiskModel.DiskIndex;
                }

                if (entry.GetType() == typeof(UCPartitionEntry))
                {
                    UCPartitionEntry ucPartitionEntry = (UCPartitionEntry)entry;
                    var pPartitionEntry = K_ucPartitionEntry_V_pPartitionEntry[ucPartitionEntry];
                    if (ucPartitionEntry != null && ucPartitionEntry.IsSelected == true)
                        return pPartitionEntry.Partition.WSMPartition.PartitionNumber;
                }
            }

            return null;
        }

        #endregion StackPanelLogic_TESTING!!!

        #endregion RetrieveDisk

        #region TopBarFileMenu

        public void OnSaveEntryData_Click(object sender, RoutedEventArgs e)
        {
            Window.EntryDataUI.SaveEntryData_Click(sender, e);
        }

        public void OnQuit_Click(object sender, RoutedEventArgs e)
        {
            App.Instance.Shutdown();
        }

        #endregion TopBarFileMenu

        #region EntriesClick

        public void OnDiskEntry_Click<UCType>(PPhysicalDriveEntry<UCType> pPhysicalDrive) where UCType : UCPhysicalDriveEntry
        {
            AddEntrysToStackPanel(Window.PartitionStackPanel, pPhysicalDrive.DiskModel.Partitions);
            if (pPhysicalDrive.DiskModel.UnallocatedSpace > 0)
            {
                var ucUnallocatedEntry = new UCUnallocatedEntry();
                var pUnallocatedEntry = CreateUCPresenter<PUnallocatedEntry>(ucUnallocatedEntry, pPhysicalDrive.DiskModel);
                Window.PartitionStackPanel.Children.Add(ucUnallocatedEntry);
                K_ucUnallocatedEntry_V_pUnallocatedEntry.Add(ucUnallocatedEntry, pUnallocatedEntry);
            }
            EntryData.AddDataToGrid(pPhysicalDrive.DiskModel.GetKeyValuePairs());
        }

        public void OnPartitionEntry_Click(UCPartitionEntry entry)
        {
            var pPartitionEntry = K_ucPartitionEntry_V_pPartitionEntry[entry];
            EntryData.AddDataToGrid(pPartitionEntry.Partition.GetKeyValuePairs());
        }

        public void OnUnallocatedEntry_Click(UCUnallocatedEntry entry)
        {
            var pUnallocatedEntry = K_ucUnallocatedEntry_V_pUnallocatedEntry[entry];
            EntryData.AddDataToGrid(pUnallocatedEntry.EntryData);
        }

        public void OnListPart_Click(object sender, RoutedEventArgs e)
        {
            UInt32? index = GetDataIndexOfSelected(Window.DiskStackPanel);
            if (index == null) return;
            Log.Print(DPFunctions.ListPart(index));
        }

        #endregion EntriesClick

        #region TopBarDiskPartMenu

        public void OnListVolume_Click(object sender, RoutedEventArgs e)
        {
            Log.Print(DPFunctions.List(DPList.VOLUME));
        }

        public void OnListDisk_Click(object sender, RoutedEventArgs e)
        {
            Log.Print(DPFunctions.List(DPList.DISK));
        }

        public void OnListVDisk_Click(object sender, RoutedEventArgs e)
        {
            Log.Print(DPFunctions.List(DPList.VDISK));
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
            App.Instance.WIM.CreateWPresenter<PAttributesVolByIndex>();
        }

        #endregion TopBarDiskPartMenu

        #region TopBarCommandsMenu

        public void OnRetrieveDiskData_Click(object sender, RoutedEventArgs e)
        {
            DiskService.ReLoadDisks();
            DisplayDiskData(true);
        }

        public void OnScanVolume_Click(object sender, RoutedEventArgs e)
        {
            foreach (object entry in Window.PartitionStackPanel.Children)
            {
                if (entry is not UCPartitionEntry) return;
                if (((UCPartitionEntry)entry).IsSelected == true)
                {
                    var pPartitionEntry = K_ucPartitionEntry_V_pPartitionEntry[(UCPartitionEntry)entry];
                    pPartitionEntry.OpenScanVolumeWindow();
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
            aboutWindow.Owner = Window;
            aboutWindow.Show();
        }

        #endregion TopBarHelpMenu

        #region WPresenterOverrides
        
        protected override void RegisterEventsInternal()
        {
            base.RegisterEventsInternal();

            //Window.EDiskEntry_Click += OnDiskEntryClick;
            Window.EPartitionEntry_Click += OnPartitionEntry_Click;
            Window.EUnallocatedEntry_Click += OnUnallocatedEntry_Click;
            Window.EListPart_Click += OnListPart_Click;
                   
            Window.EListVolume_Click += OnListVolume_Click;
            Window.EListDisk_Click += OnListDisk_Click;
            Window.EListVDisk_Click += OnListVDisk_Click;
            Window.ECreateVDisk_Click += OnCreateVDisk_Click;
            Window.EAttachVDisk_Click += OnAttachVDisk_Click;
            Window.EChildVDisk_Click += OnChildVDisk_Click;
            Window.ECopyVDisk_Click += OnCopyVDisk_Click;
            Window.EAttributesVolume_Click += OnAttributesVolume_Click;
                   
            Window.ERetrieveDiskData_Click += OnRetrieveDiskData_Click;
            Window.EScanVolume_Click += OnScanVolume_Click;
                   
            Window.ESaveEntryData_Click += OnSaveEntryData_Click;
            Window.EQuit_Click += OnQuit_Click;
                   
            Window.EWebsite_Click += OnWebsite_Click;
            Window.EWiki_Click += OnWiki_Click;
            Window.EAbout_Click += OnAbout_Click;
        }

        public override void InitPresenters()
        {
            Log = CreateUCPresenter<PLog>(Window.MainLog);
            EntryData = CreateUCPresenter<PEntryData>(Window.EntryDataUI);
        }

        #endregion WPresenterOverrides
    }
}

