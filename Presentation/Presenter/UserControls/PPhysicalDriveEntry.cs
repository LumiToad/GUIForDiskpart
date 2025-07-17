global using PPhysicalDriveEntry =
    GUIForDiskpart.Presentation.Presenter.UserControls.PPhysicalDriveEntry<GUIForDiskpart.Presentation.View.UserControls.UCPhysicalDriveEntry>;

using System.Windows;
using System.Windows.Controls;

using GUIForDiskpart.Model.Logic.Diskpart;
using GUIForDiskpart.Presentation.View.UserControls;
using GUIForDiskpart.Utils;


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
        public DiskModel Disk { get; private set; }
        public bool? IsSelected { get { return UserControl.EntrySelected.IsChecked; } }

        #region MenuItems

        MenuItem OnOffline =>
            WPFUtils.CreateContextMenuItem(
                IconUtils.Diskpart,
                Disk.IsOnline ? "DPOffline" : "DPOnline",
                Disk.IsOnline ? "DISKPART - Offline" : "DISKPART - Online",
                true,
                OnOnOffline_Click
                );
        
        #endregion MenuItems

        private void PopulateContextMenu()
        {
            UserControl.ContextMenu.Items.Add(OnOffline);
        }

        #region OnClick

        public void OnOnOffline_Click(object sender, RoutedEventArgs e)
        {
            string output = string.Empty;
            output += DPFunctions.OnOfflineDisk(Disk.DiskIndex, !Disk.IsOnline, false);

            MainWindow.Log.Print(output);
            MainWindow.DisplayDiskData(false);
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
            UserControl.SelectEntryRadioButton();
            MainWindow.OnDiskEntry_Click(this);
        }

        private void OnOpenContextMenu_Click(object sender, RoutedEventArgs e)
        {
            UserControl.ContextMenu.IsOpen = !UserControl.ContextMenu.IsOpen;
        }

        #endregion OnClick

        #region UCPresenter

        public override void Setup()
        {
            UserControl.UpdateUI(Disk);
            PopulateContextMenu();
        }

        protected override void RegisterEventsInternal()
        {
            UserControl.EOnOffline_Click += OnOnOffline_Click;
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
