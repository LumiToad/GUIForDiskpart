global using PPartitionEntry =
    GUIForDiskpart.Presentation.Presenter.UserControls.PPartitionEntry<GUIForDiskpart.Presentation.View.UserControls.UCPartitionEntry>;

using System.Windows;
using System.Windows.Controls;

using GUIForDiskpart.Database.Data;
using GUIForDiskpart.Database.Data.Types;
using GUIForDiskpart.Model.Logic.Diskpart;
using GUIForDiskpart.Presentation.View.UserControls;
using GUIForDiskpart.Utils;


namespace GUIForDiskpart.Presentation.Presenter.UserControls
{
    /// <summary>
    /// Constructed with:
    /// <value><c>PartitionModel</c> Partition</value>
    /// <br/><br/>
    /// Must be instanced with <c>CreateUCPresenter</c> method of a <c>WPresenter</c> derived class.<br/>
    /// If the UserControl is already present at compile time, this class should be instanced in the <c>InitPresenters</c> method. <br/>
    /// See code example:
    /// <para>
    /// <code>
    /// public override void InitPresenters()
    /// {
    ///     someProperty = CreateUCPresenter&lt;PSomething&gt;(Window.SomeUserControl);
    /// }
    /// </code>
    /// </para>
    /// </summary>
    public class PPartitionEntry<T> : UCPresenter<T> where T : UCPartitionEntry
    {
        public delegate void DOnSelected();
        public event DOnSelected ESelected;

        #region MenuItems

        MenuItem DPOnline =>
            WPFUtils.CreateContextMenuItem(CMenuItems.DPOnline, OnOnline_Click);
        MenuItem DPOffline =>
            WPFUtils.CreateContextMenuItem(CMenuItems.DPOffline, OnOffline_Click);
        MenuItem DPActive =>
            WPFUtils.CreateContextMenuItem(CMenuItems.DPActive, OnActive_Click);
        MenuItem DPInactive =>
            WPFUtils.CreateContextMenuItem(CMenuItems.DPInactive ,OnInactive_Click);
        MenuItem DPAttributes =>
            WPFUtils.CreateContextMenuItem(CMenuItems.DPAttributes, OnAttributes_Click);
        MenuItem DPShrink =>
            WPFUtils.CreateContextMenuItem(CMenuItems.DPShrink, OnShrink_Click);
        MenuItem DPExtend =>
            WPFUtils.CreateContextMenuItem(CMenuItems.DPExtend, OnExtend_Click);
        MenuItem CMDCHDSK =>
            WPFUtils.CreateContextMenuItem(CMenuItems.CMDCHDSK, OnScanVolume_Click);
        MenuItem PSAnalyzeDefrag =>
            WPFUtils.CreateContextMenuItem(CMenuItems.PSAnalyzeDefrag, OnAnalyzeDefrag_Click);

        #endregion MenuItems

        public PartitionModel Partition { get; private set; }

        public bool? IsSelected { get { return UserControl.IsSelected; } }

        public void Select()
        {
            SetEntryRadioButton(true);
            MainWindow.Window.PartitionEntry_Click(UserControl);
        }

        private void PopulateContextMenu()
        {
            UserControl.ContextMenu.Items.Add(Partition.WSM.IsOffline ? DPOnline : DPOffline);

            if (Partition.WSM.PartitionTable == CommonTypes.MBR)
            {
                UserControl.ContextMenu.Items.Add(Partition.WSM.IsActive ? DPInactive : DPActive);
            }

            if (Partition.IsLogicalDisk && Partition.LDModel.DriveLetter != null)
            {
                UserControl.ContextMenu.Items.Add(DPAttributes);
                UserControl.ContextMenu.Items.Add(DPShrink);
                UserControl.ContextMenu.Items.Add(DPExtend);
            }

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

        public void UpdateUI(PartitionModel partition, string driveName, string fileSystem)
        {
            UserControl.PartitionNumber.Content = $"#{partition.WSM.PartitionNumber}";
            UserControl.DriveNameAndLetter.Content = driveName;
            UserControl.TotalSpace.Content = partition.WSM.FormattedSize;
            UserControl.FileSystemText.Content = fileSystem;
            UserControl.PartitionType.Content = $"{partition.WSM.PartitionTable}: {partition.WSM.PartitionType}";
        }

        public void SetEntryRadioButton(bool value)
        {
            UserControl.EntrySelected.IsChecked = value;
        }

        public void OpenScanVolumeWindow()
        {
            if (Partition.IsLogicalDisk && Partition.LDModel.DriveLetter == null) return;
            App.Instance.WIM.CreateWPresenter<PCHKDSK>(true, Partition);
        }

        private string GetDriveNameText(PartitionModel partition)
        {
            string driveNameText = string.Empty;

            if (partition.IsLogicalDisk && (!string.IsNullOrEmpty(partition.LDModel.VolumeName)))
            {
                driveNameText += $"{partition.LDModel.VolumeName} ";
            }

            if (partition.HasDriveLetter())
            {
                driveNameText += $"[{partition.GetDriveLetter()}{CommonStrings.PATH_FORMAT}]";
            }

            if (driveNameText == string.Empty)
            {
                driveNameText = CommonStrings.NO_LETTER;
            }

            return driveNameText;
        }

        private void SetValueInProgressBar(ulong totalSize, ulong usedSpace)
        {
            UserControl.SizeBar.Maximum = totalSize;
            UserControl.SizeBar.Minimum = 0;
            UserControl.SizeBar.Value = usedSpace;
        }

        private string GetFileSystemText(PartitionModel partition)
        {
            if (partition.IsLogicalDisk && !string.IsNullOrWhiteSpace(partition.LDModel.FileSystem))
            {
                return partition.LDModel.FileSystem;
            }

            if (partition.IsLogicalDisk && string.IsNullOrWhiteSpace(partition.LDModel.FileSystem))
            {
                return CommonStrings.NO_FS;
            }

            return CommonStrings.NO_WIN_VOL;
        }

        #region OnClick

        private void OnOnline_Click(object sender, RoutedEventArgs e)
        {
            string output = string.Empty;

            uint diskIndex = Partition.WSM.DiskNumber;
            uint partitionIndex = Partition.WSM.PartitionNumber;
            char driveLetter = Partition.GetDriveLetter();

            if (Partition.WSM.IsOffline)
            {
                output = DPFunctions.OnlineVolume(diskIndex, partitionIndex, false);
            }
            
            MainWindow.Log.Print(output);
            MainWindow.UpdatePanels(false);
        }

        private void OnOffline_Click(object sender, RoutedEventArgs e)
        {
            string output = string.Empty;

            uint diskIndex = Partition.WSM.DiskNumber;
            uint partitionIndex = Partition.WSM.PartitionNumber;
            char driveLetter = Partition.GetDriveLetter();

            if (!Partition.WSM.IsOffline) 
            {
                output = (!Partition.HasDriveLetter()) ?
                DPFunctions.OfflineVolume(diskIndex, partitionIndex, false) :
                DPFunctions.OfflineVolume(driveLetter, false);
            }

            MainWindow.Log.Print(output);
            MainWindow.UpdatePanels(false);
        }

        private void OnExtend_Click(object sender, RoutedEventArgs e)
        {
            App.Instance.WIM.CreateWPresenter<PExtend>(true, Partition);
        }

        private void OnShrink_Click(object sender, RoutedEventArgs e)
        {
            App.Instance.WIM.CreateWPresenter<PShrink>(true, Partition);
        }

        private void OnAnalyzeDefrag_Click(object sender, RoutedEventArgs e)
        {
            Partition.DefragAnalysis = DAService.AnalyzeVolumeDefrag(Partition);
            MainWindow.Log.Print(Partition.DefragAnalysis.GetOutputAsString());
            MainWindow.EntryData.AddDataToGrid(Partition.GetKeyValuePairs());
        }

        private void OnAttributes_Click(object sender, RoutedEventArgs e)
        {
            App.Instance.WIM.CreateWPresenter<PAttributesVolume>(true, Partition);
        }

        private void OnActive_Click(object sender, RoutedEventArgs e)
        {
            var cmItem = CMenuItems.GetCMenuItemData(CMenuItems.DPActive);

            switch (ErrorUtils.ShowMSGBoxWarning(cmItem.Header, ErrorUtils.MBR_ACTIVE_STATUS_WARNING))
            {
                case MessageBoxResult.OK:
                    string result = DPFunctions.Active(Partition.WSM.DiskNumber, Partition.WSM.PartitionNumber, true);
                    MainWindow.Log.Print(result);
                    MainWindow.UpdatePanels(false);
                    break;
                case MessageBoxResult.Cancel:
                    break;
            }
        }

        private void OnInactive_Click(object sender, RoutedEventArgs e)
        {
            var cmItem = CMenuItems.GetCMenuItemData(CMenuItems.DPInactive);

            switch (ErrorUtils.ShowMSGBoxWarning(cmItem.Header, ErrorUtils.MBR_ACTIVE_STATUS_WARNING))
            {
                case MessageBoxResult.OK:
                    string result = DPFunctions.Active(Partition.WSM.DiskNumber, Partition.WSM.PartitionNumber, false);
                    MainWindow.Log.Print(result);
                    MainWindow.UpdatePanels(false);
                    break;
                case MessageBoxResult.Cancel:
                    break;
            }
        }

        private void OnButton_Click(object sender, RoutedEventArgs e)
        {
            Select();
            ESelected?.Invoke();
        }

        private void OnDetail_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Log.Print(DPFunctions.DetailPart(Partition.WSM.DiskNumber, Partition.WSM.PartitionNumber));
        }

        private void OnFormat_Click(object sender, RoutedEventArgs e)
        {
            App.Instance.WIM.CreateWPresenter<PFormatPartition>(true, Partition.WSM);
        }

        private void OnDelete_Click(object sender, RoutedEventArgs e)
        {
            App.Instance.WIM.CreateWPresenter<PDelete>(true, Partition.WSM);
        }

        private void OnAssign_Click(object sender, RoutedEventArgs e)
        {
            App.Instance.WIM.CreateWPresenter<PAssignLetter>(true, Partition.WSM);
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
            string driveName = GetDriveNameText(Partition);
            string fileSystem = GetFileSystemText(Partition);

            UpdateUI(Partition, driveName, fileSystem);
            if (Partition.HasWSMPartition && Partition.IsLogicalDisk)
            {
                SetValueInProgressBar(Partition.WSM.Size, Partition.LDModel.UsedSpace);
            }

            if (Partition.WSM.IsBoot)
            {
                UserControl.WinVolumeIcon.Source = IconUtils.GetSystemIconByType(SystemIconType.WinLogo);
            }
            PopulateContextMenu();
        }

        protected override void RegisterEventsInternal()
        {
            UserControl.EOnline += OnOnline_Click;
            UserControl.EOffline += OnOffline_Click;
            UserControl.EExtend += OnExtend_Click;
            UserControl.EShrink += OnShrink_Click;
            UserControl.EAnalyzeDefrag += OnAnalyzeDefrag_Click;
            UserControl.EAttributes += OnAttributes_Click;
            UserControl.EActive += OnActive_Click;
            UserControl.EInactive += OnInactive_Click;
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

