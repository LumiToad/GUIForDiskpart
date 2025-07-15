global using PPhysicalDriveEntry =
    GUIForDiskpart.Presentation.Presenter.UserControls.PPhysicalDriveEntry<GUIForDiskpart.Presentation.View.UserControls.UCPhysicalDriveEntry>;

using System.Windows;
using System.Windows.Controls;

using GUIForDiskpart.Model.Logic.Diskpart;
using GUIForDiskpart.Presentation.View.UserControls;
using GUIForDiskpart.Utils;


namespace GUIForDiskpart.Presentation.Presenter.UserControls
{
    public class PPhysicalDriveEntry<T> : UCPresenter<T> where T : UCPhysicalDriveEntry
    {
        public DiskModel DiskModel { get; private set; }
        public bool? IsSelected { get { return UserControl.EntrySelected.IsChecked; } }

        #region MenuItems

        MenuItem OnOffline =>
            WPFUtils.CreateContextMenuItem(
                IconUtils.Diskpart,
                DiskModel.IsOnline ? "DPOnline" : "DPOffline",
                DiskModel.IsOnline ? "DISKPART - Online" : "DISKPART - Offline",
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
            output += DPFunctions.OnOfflineDisk(DiskModel.DiskIndex, !DiskModel.IsOnline, false);

            MainWindow.Log.Print(output);
            MainWindow.DisplayDiskData(false);
        }

        private void OnDetail_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Log.Print(DPFunctions.DetailDisk(DiskModel.DiskIndex));
        }

        private void OnListPart_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Log.Print(DPFunctions.ListPart(DiskModel.DiskIndex));
        }

        private void OnClean_Click(object sender, RoutedEventArgs e)
        {
            App.Instance.WIM.CreateWPresenter<PClean>(true, DiskModel);
        }

        private void OnConvert_Click(object sender, RoutedEventArgs e)
        {
            App.Instance.WIM.CreateWPresenter<PConvertDrive>(true, DiskModel);
        }

        private void OnCreatePart_Click(object sender, RoutedEventArgs e)
        {
            App.Instance.WIM.CreateWPresenter<PCreatePartition>(true, DiskModel, DiskModel.UnallocatedSpace);
        }

        private void OnCreateVolume_Click(object sender, RoutedEventArgs e)
        {
            App.Instance.WIM.CreateWPresenter<PCreateVolume>(true, DiskModel, DiskModel.UnallocatedSpace);
        }

        private void OnEasyFormat_Click(object sender, RoutedEventArgs e)
        {
            WEasyFormat formatWindow = new WEasyFormat(DiskModel);
            formatWindow.Owner = MainWindow.Window;
            formatWindow.Focus();

            formatWindow.Show();
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
            UserControl.UpdateUI(DiskModel);
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
            DiskModel = (DiskModel)args[0];
        }

        #endregion UCPresenter
    }
}
