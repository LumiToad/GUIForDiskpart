﻿global using PPhysicalDriveEntry =
    GUIForDiskpart.Presentation.Presenter.UserControls.PPhysicalDriveEntry<GUIForDiskpart.Presentation.View.UserControls.UCPhysicalDriveEntry>;
using GUIForDiskpart.Database.Data;
using GUIForDiskpart.Model.Logic.Diskpart;
using GUIForDiskpart.Presentation.View.UserControls;
using GUIForDiskpart.Utils;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ScrollBar;


namespace GUIForDiskpart.Presentation.Presenter.UserControls
{
    /// <summary>
    /// Constructed with:
    /// <value><c>DiskModel</c> Disk</value>
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
    public class PPhysicalDriveEntry<T> : UCPresenter<T> where T : UCPhysicalDriveEntry
    {
        public delegate void DOnSelected();
        public event DOnSelected ESelected;

        public DiskModel Disk { get; private set; }
        public bool? IsSelected { get { return UserControl.EntrySelected.IsChecked; } }

        private const string EXTERNAL_HD_MEDIA = "External hard disk media";
        private const string REMOVABLE_MEDIA = "Removable Media";
        private const string FIXED_HD_MEDIA = "Fixed hard disk media";
        private const string UNKNOWN_MEDIA = "Unknown";

        #region MenuItems

        MenuItem DPOnline =>
           WPFUtils.CreateContextMenuItem(CMenuItems.DPOnline, OnOnline_Click);
        MenuItem DPOffline =>
            WPFUtils.CreateContextMenuItem(CMenuItems.DPOffline, OnOffline_Click);

        #endregion MenuItems

        public void SetEntryRadioButton(bool value) => UserControl.EntrySelected.IsChecked = value;

        public void SetValueInProgressBar(ulong totalSize, ulong usedSpace)
        {
            UserControl.SizeBar.Maximum = totalSize;
            UserControl.SizeBar.Minimum = 0;
            UserControl.SizeBar.Value = usedSpace;
        }

        public void UpdateUI(DiskModel diskModel)
        {
            UserControl.DiskIndex.Content = $"#{diskModel.DiskIndex}";
            UserControl.DiskModelText.Content = diskModel.DiskModelText;
            UserControl.TotalSpace.Content = diskModel.FormattedTotalSpace;
            UserControl.WSMPartitionCount.Content = $"{diskModel.PartitionCount} partitions";
            SetValueInProgressBar(diskModel.TotalSpace, diskModel.UsedSpace);

            UserControl.DiskIcon.Source = GetDiskIcon(diskModel);
            UserControl.MediaTypeIcon.Source = GetMediaTypeIcon(diskModel);
        }

        private ImageSource? GetDiskIcon(DiskModel diskModel)
        {
            ImageSource? result = IconUtils.GetShellIconByType(Shell32IconType.Drive, true);

            if (diskModel.InterfaceType == CommonStrings.USB)
            {
                result = IconUtils.GetShellIconByType(Shell32IconType.USB, true);
            }

            return result;
        }

        private ImageSource? GetMediaTypeIcon(DiskModel diskModel)
        {
            ImageSource? result = IconUtils.GetShellIconByType(Shell32IconType.QuestionMark, true);

            switch (diskModel.MediaType)
            {
                case (EXTERNAL_HD_MEDIA):
                    result = IconUtils.GetShellIconByType(Shell32IconType.UpArrow, true);
                    break;
                case (REMOVABLE_MEDIA):
                    result = IconUtils.GetShellIconByType(Shell32IconType.UpArrow, true);
                    break;
                case (FIXED_HD_MEDIA):
                    result = IconUtils.GetShellIconByType(Shell32IconType.Fixed, true);
                    break;
                case (UNKNOWN_MEDIA):
                    result = IconUtils.GetShellIconByType(Shell32IconType.QuestionMark, true);
                    break;
            }

            return result;
        }

        private void PopulateContextMenu()
        {
            UserControl.ContextMenu.Items.Add(Disk.IsOnline ? DPOffline : DPOnline);
        }

        public void Select()
        {
            SetEntryRadioButton(true);
            MainWindow.Window.DiskEntry_Click(UserControl);
        }

        #region OnClick

        private void OnOnline_Click(object sender, RoutedEventArgs e)
        {
            string output = string.Empty;
            output += DPFunctions.OnOfflineDisk(Disk.DiskIndex, true, false);

            MainWindow.Log.Print(output);
            MainWindow.UpdatePanels(false);
        }

        private void OnOffline_Click(object sender, RoutedEventArgs e)
        {
            string output = string.Empty;
            output += DPFunctions.OnOfflineDisk(Disk.DiskIndex, false, false);

            MainWindow.Log.Print(output);
            MainWindow.UpdatePanels(false);
        }

        private void OnDetail_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Log.Print(DPFunctions.DetailDisk(Disk.DiskIndex));
        }

        private void OnListPart_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Log.Print(DPFunctions.ListPart(Disk.DiskIndex));
        }

        private void OnClean_Click(object sender, RoutedEventArgs e)
        {
            App.Instance.WIM.CreateWPresenter<PClean>(true, Disk);
        }

        private void OnConvert_Click(object sender, RoutedEventArgs e)
        {
            App.Instance.WIM.CreateWPresenter<PConvertDrive>(true, Disk);
        }

        private void OnCreatePart_Click(object sender, RoutedEventArgs e)
        {
            App.Instance.WIM.CreateWPresenter<PCreatePartition>(true, Disk, Disk.UnallocatedSpace);
        }

        private void OnCreateVolume_Click(object sender, RoutedEventArgs e)
        {
            App.Instance.WIM.CreateWPresenter<PCreateVolume>(true, Disk, Disk.UnallocatedSpace);
        }

        private void OnEasyFormat_Click(object sender, RoutedEventArgs e)
        {
            App.Instance.WIM.CreateWPresenter<PEasyFormat>(true, Disk);
        }

        private void OnButton_Click(object sender, RoutedEventArgs e)
        {
            Select();
            ESelected?.Invoke();
        }

        private void OnOpenContextMenu_Click(object sender, RoutedEventArgs e)
        {
            UserControl.ContextMenu.IsOpen = !UserControl.ContextMenu.IsOpen;
        }

        #endregion OnClick

        #region UCPresenter

        public override void Setup()
        {
            UpdateUI(Disk);
            PopulateContextMenu();
        }

        protected override void RegisterEventsInternal()
        {
            UserControl.EOnline_Click += OnOnline_Click;
            UserControl.EOffline_Click += OnOnline_Click;
            UserControl.EDetail_Click += OnDetail_Click;
            UserControl.EListPart_Click += OnListPart_Click;
            UserControl.EClean_Click += OnClean_Click;
            UserControl.EConvert_Click += OnConvert_Click;
            UserControl.ECreatePart_Click += OnCreatePart_Click;
            UserControl.ECreateVolume_Click += OnCreateVolume_Click;
            UserControl.EEasyFormat_Click += OnEasyFormat_Click;
            UserControl.EButton_Click += OnButton_Click;
            UserControl.EOpenContextMenu_Click += OnOpenContextMenu_Click;
        }

        public override void AddCustomArgs(params object?[] args)
        {
            Disk = (DiskModel)args[0];
        }

        #endregion UCPresenter
    }
}
