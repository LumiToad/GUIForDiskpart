global using PPartitionEntry =
    GUIForDiskpart.Presentation.Presenter.UserControls.PPartitionEntry<GUIForDiskpart.Presentation.View.UserControls.UCPartitionEntry>;

using System.Windows;
using System.Windows.Controls;

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
        #region MenuItems

        MenuItem Offline =>
            WPFUtils.CreateContextMenuItem(IconUtils.Diskpart, "DPOffline", "DISKPART - Offline", true, OnOnOffline_Click);
        MenuItem Online =>
            WPFUtils.CreateContextMenuItem(IconUtils.Diskpart, "DPOnline", "DISKPART - Online", true, OnOnOffline_Click);
        MenuItem DPInActive =>
            WPFUtils.CreateContextMenuItem(IconUtils.Diskpart, "DPInActive", "DISKPART - " + (Partition.WSM.IsActive ? "Inactive" : "Active"), true, OnActive_Click);
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
            if (Partition.WSM.PartitionTable == "MBR")
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
            { UserControl.ContextMenu.Items.Add((!Partition.HasDriveLetter()) ? Online : Offline); }

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

        private string GetDriveNameText(PartitionModel partition)
        {
            string driveNameText = string.Empty;

            if (partition.IsLogicalDisk && (!string.IsNullOrEmpty(partition.LDModel.VolumeName)))
            {
                driveNameText += $"{partition.LDModel.VolumeName} ";
            }

            if (partition.HasDriveLetter())
            {
                driveNameText += $"[{partition.GetDriveLetter()}:\\]";
            }

            if (driveNameText == string.Empty)
            {
                driveNameText = "No letter";
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
                return "No filesystem";
            }

            return "No Windows Volume";
        }

        #region OnClick

        private void OnOnOffline_Click(object sender, RoutedEventArgs e)
        {
            string output = string.Empty;

            uint diskIndex = Partition.WSM.DiskNumber;
            uint partitionIndex = Partition.WSM.PartitionNumber;
            char driveLetter = Partition.GetDriveLetter();

            if (Partition.WSM.IsOffline)
            {
                output = DPFunctions.OnlineVolume(diskIndex, partitionIndex, false);
            }
            else
            {
                output = (!Partition.HasDriveLetter()) ?
                DPFunctions.OfflineVolume(diskIndex, partitionIndex, false) :
                DPFunctions.OfflineVolume(driveLetter, false);
            }
            MainWindow.Log.Print(output);
            MainWindow.DisplayDiskData(false);
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
            string title = "Diskpart - Active / Inactive";

            switch (ErrorUtils.ShowMSGBoxWarning(title, ErrorUtils.MBR_ACTIVE_STATUS_WARNING))
            {
                case MessageBoxResult.OK:
                    MainWindow.Log.Print(DPFunctions.Active(Partition.WSM.DiskNumber, Partition.WSM.PartitionNumber, !Partition.WSM.IsActive));
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

            UserControl.UpdateUI(Partition, driveName, fileSystem);
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

