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
using System.Linq;



namespace GUIForDiskpart.Presentation.Presenter
{
    public class PMainWindow<T> : WPresenter<T> where T : GUIFDMainWin
    {
        public PLog<UCLog> Log {get; private set;}

        private Dictionary<UCPhysicalDrive, PPhysicalDrive<UCPhysicalDrive>> K_ucPhysicalDrives_V_pPhysicalDrives = new();


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
            var ucPhysicalDrive = Window.DiskStackPanel.Children[0] as UCPhysicalDrive;
            var pPhysicalDrive = K_ucPhysicalDrives_V_pPhysicalDrives[ucPhysicalDrive];
            OnDiskEntry_Click(pPhysicalDrive);
        }

        #region StackPanelLogic_TESTING!!!

        private void AddEntrysToStackPanel<T>(StackPanel stackPanel, List<T> collection)
        {
            stackPanel.Children.Clear();
            K_ucPhysicalDrives_V_pPhysicalDrives.Clear();

            foreach (T thing in collection)
            {
                UserControl userControl = new UserControl();

                switch (thing)
                {
                    case DiskModel diskModel:
                        var ucPhysicalDrive = new UCPhysicalDrive();
                        var pPhysicalDrive = CreateUCPresenter<PPhysicalDrive<UCPhysicalDrive>>(ucPhysicalDrive, diskModel);
                        K_ucPhysicalDrives_V_pPhysicalDrives.Add(ucPhysicalDrive, pPhysicalDrive);
                        userControl = ucPhysicalDrive;
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
            UCPhysicalDrive ucPhysicalDrive;
            PartitionEntryUI partitionEntry;

            foreach (object? entry in stackPanel.Children)
            {
                if (entry.GetType() == typeof(UCPhysicalDrive))
                {
                    ucPhysicalDrive = (UCPhysicalDrive)entry;
                    var pPhysicalDrive = K_ucPhysicalDrives_V_pPhysicalDrives[ucPhysicalDrive];
                    if (ucPhysicalDrive != null && ucPhysicalDrive.IsSelected == true)
                        return pPhysicalDrive.DiskModel.DiskIndex;
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

        public void OnDiskEntry_Click<UCType>(PPhysicalDrive<UCType> pPhysicalDrive) where UCType : UCPhysicalDrive
        {
            AddEntrysToStackPanel(Window.PartitionStackPanel, pPhysicalDrive.DiskModel.Partitions);
            if (pPhysicalDrive.DiskModel.UnallocatedSpace > 0)
            {
                UnallocatedEntryUI unallocatedEntryUI = new UnallocatedEntryUI(pPhysicalDrive.DiskModel);
                Window.PartitionStackPanel.Children.Add(unallocatedEntryUI);
            }
            Window.EntryDataUI.AddDataToGrid(pPhysicalDrive.DiskModel.GetKeyValuePairs());
        }

        public void OnPartitionEntry_Click(PartitionEntryUI entry)
        {
            Window.EntryDataUI.AddDataToGrid(entry.Partition.GetKeyValuePairs());
        }

        public void OnUnallocatedEntry_Click(UnallocatedEntryUI entry)
        {
            Window.EntryDataUI.AddDataToGrid(entry.Entry);
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
            AttributesVolumeByIndexWindow attributesVolumeByIndexWindow = new();
            attributesVolumeByIndexWindow.Owner = Window;
            attributesVolumeByIndexWindow.Focus();

            attributesVolumeByIndexWindow.Show();
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
            Log = CreateUCPresenter<PLog<UCLog>>(Window.MainLog);
        }

        #endregion WPresenterOverrides
    }
}

