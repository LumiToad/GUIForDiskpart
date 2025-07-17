global using PMainWindow =
    GUIForDiskpart.Presentation.Presenter.Windows.PMainWindow<GUIForDiskpart.Presentation.View.Windows.MainWindow>;

using System;
using System.Windows;

using GUIForDiskpart.Database.Data.Types;
using GUIForDiskpart.Model.Logic;
using GUIForDiskpart.Model.Logic.Diskpart;
using GUIForDiskpart.Presentation.View.UserControls;
using GUIForDiskpart.Database.Data;
using GUIForDiskpart.Presentation.Presenter.UserControls;
using GUIForDiskpart.Presentation.Presenter.UserControls.Components;


namespace GUIForDiskpart.Presentation.Presenter.Windows
{
    public class PMainWindow<T> : WPresenter<T> where T : GUIFDMainWin
    {
        public PLog<UCLog> Log {get; private set;}
        public PEntryData EntryData { get; private set;}
        public PEntryPanel PDiskPanel { get; private set; }
        public PEntryPanel PPartitionPanel { get; private set; }

        public void OnDiskChanged() => DisplayDiskData(false);

        public void DisplayDiskData(bool outputText)
        {
            PDiskPanel.UpdatePanel(DiskService.PhysicalDrives);

            if (outputText)
            {
                Log.Print(DiskService.GetDiskOutput());
            }

            PDiskPanel.SelectPrevious();
            PPartitionPanel.SelectPrevious();
        }

        private void OnWindowContent_Rendered(EventArgs e)
        {
            DiskService.OnDiskChanged += OnDiskChanged;
            DisplayDiskData(true);
        }

        #region TopBarFileMenu

        public void OnSaveEntryData_Click(object sender, RoutedEventArgs e)
        {
            EntryData.OnSaveEntryData_Click(sender, e);
        }

        public void OnQuit_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Save shutdown?
            App.Instance.Shutdown();
        }

        #endregion TopBarFileMenu

        #region EntriesClick

        public void OnDiskEntry_Click(UCPhysicalDriveEntry ucPhysicalDrive)
        {
            var pDisk = PDiskPanel.GetEntryPresenter(ucPhysicalDrive);

            PPartitionPanel.UpdatePanel(pDisk.Disk.Partitions);
            PPartitionPanel.UpdatePanel(pDisk);
            EntryData.AddDataToGrid(pDisk.Disk.GetKeyValuePairs());
        }

        public void OnPartitionEntry_Click(UCPartitionEntry entry)
        {
            var pPartition = PPartitionPanel.GetEntryPresenter(entry);
            EntryData.AddDataToGrid(pPartition.Partition.GetKeyValuePairs());
        }

        public void OnUnallocatedEntry_Click(UCUnallocatedEntry entry)
        {
            var pUnallocated = PPartitionPanel.GetEntryPresenter(entry);
            EntryData.AddDataToGrid(pUnallocated.EntryData);
        }

        public void OnListPart_Click(object sender, RoutedEventArgs e)
        {
            UInt32? index = PDiskPanel.GetSelectedIdx();
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
            App.Instance.WIM.CreateWPresenter<PAttributesVolByIdx>();
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
            var entry = PPartitionPanel.GetSelectedEntry();
            if (entry is not UCPartitionEntry) return;

            var pPartition = PPartitionPanel.GetEntryPresenter((UCPartitionEntry)entry);
            pPartition.OpenScanVolumeWindow();
        }

        #endregion TopBarCommandsMenu

        #region TopBarHelpMenu

        public void OnWebsite_Click(object sender, RoutedEventArgs e)
        {
            CommandExecuter.IssueCommand(ProcessType.CMD, CMDBasic.START_BROWSER + AppInfo.WEBSITE_URL);
        }

        public void OnWiki_Click(object sender, RoutedEventArgs e)
        {
            CommandExecuter.IssueCommand(ProcessType.CMD, CMDBasic.START_BROWSER + AppInfo.WIKI_URL);
        }

        public void OnAbout_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow aboutWindow = new AboutWindow(AppInfo.BuildString);
            aboutWindow.Owner = Window;
            aboutWindow.Show();
        }

        #endregion TopBarHelpMenu

        #region WPresenter
        
        protected override void RegisterEventsInternal()
        {
            base.RegisterEventsInternal();

            Window.ERendered += OnWindowContent_Rendered;

            Window.EDriveEntry_Click += OnDiskEntry_Click;
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
            PDiskPanel = CreateUCPresenter<PEntryPanel>(Window.DiskPanel, new PCDiskPanel());
            PPartitionPanel = CreateUCPresenter<PEntryPanel>(Window.PartitionPanel, new PCPartitionPanel());
        }

        #endregion WPresenter
    }
}

