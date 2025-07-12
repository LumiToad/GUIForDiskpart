global using PPartitionEntry =
    GUIForDiskpart.Presentation.Presenter.UserControls.PPartitionEntry<GUIForDiskpart.Presentation.View.UserControls.UCPartitionEntry>;

using System.Windows;
using System.Windows.Controls;

using GUIForDiskpart.Model.Logic.Diskpart;
using GUIForDiskpart.Presentation.Presenter.Windows;
using GUIForDiskpart.Presentation.View.UserControls;
using GUIForDiskpart.Utils;


namespace GUIForDiskpart.Presentation.Presenter.UserControls
{
    public class PPartitionEntry<T> : UCPresenter<T> where T : UCPartitionEntry
    {
        #region MenuItems

        MenuItem Offline =>
            WPFUtils.CreateContextMenuItem(IconUtils.Diskpart, "DPOffline", "DISKPART - Offline", true, OnOnOffline_Click);
        MenuItem Online =>
            WPFUtils.CreateContextMenuItem(IconUtils.Diskpart, "DPOnline", "DISKPART - Online", true, OnOnOffline_Click);
        MenuItem DPInActive =>
            WPFUtils.CreateContextMenuItem(IconUtils.Diskpart, "DPInActive", "DISKPART - " + (Partition.WSMPartition.IsActive ? "Inactive" : "Active"), true, OnActive_Click);
        MenuItem DPAttributes =>
            WPFUtils.CreateContextMenuItem(IconUtils.Diskpart, "DPAttributes", "DISKPART - Attributes", true, OnAttributes_Click);
        MenuItem DPShrink =>
            WPFUtils.CreateContextMenuItem(IconUtils.Diskpart, "DPShrink", "DISKPART - Shrink", true, OnShrink_Click);
        MenuItem DPExtend =>
            WPFUtils.CreateContextMenuItem(IconUtils.Diskpart, "DPExtend", "DISKPART - Extend", true, OnExtend_Click);
        MenuItem CMDCHDSK =>
            WPFUtils.CreateContextMenuItem(IconUtils.CMD, "CMDCHDSK", "CMD - CHKDSK", true, OnScanVolume_Click);
        MenuItem PSAnalyzeDefrag =>
            WPFUtils.CreateContextMenuItem(IconUtils.Commandline, "PSAnalyzeDefrag", "Powershell - DefragAnalysis", true, OnAnalyzeDefrag_Click, PSAnalyzeDefragTT);
        const string PSAnalyzeDefragTT = 
            "Will analyze the fragmentation of this volume.\n" +
            "Will not actually start a defragmentation process.\n" +
            "Retrieved data will be shown in the list entrys below.\n" +
            "Can take a while!";

        #endregion MenuItems

        public PartitionModel Partition { get; private set; }

        public bool? IsSelected { get { return UserControl.IsSelected; } }

        private void PopulateContextMenu()
        {
            if (Partition.WSMPartition.PartitionTable == "MBR")
            {
                UserControl.ContextMenu.Items.Add(DPInActive);
            }

            if (Partition.IsLogicalDisk && Partition.LDModel.DriveLetter != null)
            {
                UserControl.ContextMenu.Items.Add(DPAttributes);
                UserControl.ContextMenu.Items.Add(DPShrink);
                UserControl.ContextMenu.Items.Add(DPExtend);
                UserControl.ContextMenu.Items.Add(Offline);
            }

            if (!UserControl.ContextMenu.Items.Contains(Offline))
            { UserControl.ContextMenu.Items.Add((Partition.WSMPartition.DriveLetter < 65) ? Online : Offline); }

            if (Partition.IsLogicalDisk)
            {
                UserControl.ContextMenu.Items.Add(new Separator());
                UserControl.ContextMenu.Items.Add(CMDCHDSK);
            }

            if (Partition.IsVolume)
            {
                UserControl.ContextMenu.Items.Add(PSAnalyzeDefrag);
            }
        }

        public void ToggleEntryRadioButton()
        {
            UserControl.EntrySelected.IsChecked = !UserControl.EntrySelected.IsChecked;
        }

        public void OpenScanVolumeWindow()
        {
            if (Partition.IsLogicalDisk && Partition.LDModel.DriveLetter == null) return;
            App.Instance.WIM.CreateWPresenter<PCHKDSK>(true, Partition);
        }

        #region OnClick

        private void OnOnOffline_Click(object sender, RoutedEventArgs e)
        {
            string output = string.Empty;

            uint diskIndex = Partition.WSMPartition.DiskNumber;
            uint partitionIndex = Partition.WSMPartition.PartitionNumber;
            char driveLetter = Partition.WSMPartition.DriveLetter;

            if (Partition.WSMPartition.IsOffline)
            {
                output = DPFunctions.OnlineVolume(diskIndex, partitionIndex, false);
            }
            else
            {
                output = (Partition.WSMPartition.DriveLetter < 65) ?
                DPFunctions.OfflineVolume(diskIndex, partitionIndex, false) :
                DPFunctions.OfflineVolume(driveLetter, false);
            }
            MainWindow.Log.Print(output);
            MainWindow.DisplayDiskData(false);
        }

        private void OnExtend_Click(object sender, RoutedEventArgs e)
        {
            WExtend window = new WExtend(Partition);
            window.Owner = MainWindow.Window;
            window.Show();
        }

        private void OnShrink_Click(object sender, RoutedEventArgs e)
        {
            GUIForDiskpart.Presentation.View.Windows.WShrink window = new GUIForDiskpart.Presentation.View.Windows.WShrink(Partition);
            window.Owner = MainWindow.Window;
            window.Show();
        }

        private void OnAnalyzeDefrag_Click(object sender, RoutedEventArgs e)
        {
            Partition.DefragAnalysis = DAService.AnalyzeVolumeDefrag(Partition);
            MainWindow.Log.Print(Partition.DefragAnalysis.GetOutputAsString());
            MainWindow.EntryData.AddDataToGrid(Partition.GetKeyValuePairs());
        }

        private void OnAttributes_Click(object sender, RoutedEventArgs e)
        {
            App.Instance.WIM.CreateWPresenter<PAttributesVolume>(true, Partition.WSMPartition);
        }

        private void OnActive_Click(object sender, RoutedEventArgs e)
        {
            string title = "Diskpart - Active / Inactive";

            switch (ErrorUtils.ShowMSGBoxWarning(title, ErrorUtils.MBR_ACTIVE_STATUS_WARNING))
            {
                case MessageBoxResult.OK:
                    MainWindow.Log.Print(DPFunctions.Active(Partition.WSMPartition.DiskNumber, Partition.WSMPartition.PartitionNumber, !Partition.WSMPartition.IsActive));
                    MainWindow.DisplayDiskData(false);
                    break;
                case MessageBoxResult.Cancel:
                    break;
            }
        }

        private void OnButton_Click(object sender, RoutedEventArgs e)
        {
            ToggleEntryRadioButton();
            MainWindow.OnPartitionEntry_Click(UserControl);
        }

        private void OnDetail_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Log.Print(DPFunctions.DetailPart(Partition.WSMPartition.DiskNumber, Partition.WSMPartition.PartitionNumber));
        }

        private void OnFormat_Click(object sender, RoutedEventArgs e)
        {
            WFormatPartition formatPartitionWindow = new WFormatPartition(Partition.WSMPartition);
            formatPartitionWindow.Owner = MainWindow.Window;
            formatPartitionWindow.Show();
        }

        private void OnDelete_Click(object sender, RoutedEventArgs e)
        {
            WDelete deleteWindow = new WDelete(Partition.WSMPartition);
            deleteWindow.Owner = MainWindow.Window;
            deleteWindow.Show();
        }

        private void OnAssign_Click(object sender, RoutedEventArgs e)
        {
            App.Instance.WIM.CreateWPresenter<PAssignLetter>(true, Partition.WSMPartition);
        }

        private void OnScanVolume_Click(object sender, RoutedEventArgs e)
        {
            OpenScanVolumeWindow();
        }

        private void OnOpenContextMenu_Click(object sender, RoutedEventArgs e)
        {
            UserControl.ContextMenu.IsOpen = !UserControl.ContextMenu.IsOpen;
        }

        #endregion OnClick

        #region UCPresenter
        public override void Setup()
        {
            UserControl.UpdateUI(Partition);
            PopulateContextMenu();
        }

        protected override void RegisterEventsInternal()
        {
            UserControl.EOnOffline += OnOnOffline_Click;
            UserControl.EExtend += OnExtend_Click;
            UserControl.EShrink += OnShrink_Click;
            UserControl.EAnalyzeDefrag += OnAnalyzeDefrag_Click;
            UserControl.EAttributes += OnAttributes_Click;
            UserControl.EActive += OnActive_Click;
            UserControl.EButton += OnButton_Click;
            UserControl.EDetail += OnDetail_Click;
            UserControl.EFormat += OnFormat_Click;
            UserControl.EDelete += OnDelete_Click;
            UserControl.EAssign += OnAssign_Click;
            UserControl.EScanVolume += OnScanVolume_Click;
            UserControl.EOpenContextMenu += OnOpenContextMenu_Click;
        }

        public override void AddCustomArgs(params object?[] args)
        {
            Partition = (PartitionModel)args[0];
        }

        #endregion UCPresenter
    }
}

