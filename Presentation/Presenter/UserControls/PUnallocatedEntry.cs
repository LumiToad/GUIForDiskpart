global using PUnallocatedEntry =
    GUIForDiskpart.Presentation.Presenter.UserControls.PUnallocatedEntry<GUIForDiskpart.Presentation.View.UserControls.UCUnallocatedEntry>;

using System.Collections.Generic;
using System.Windows;

using GUIForDiskpart.Presentation.View.UserControls;
using GUIForDiskpart.Utils;


namespace GUIForDiskpart.Presentation.Presenter.UserControls
{
    public class PUnallocatedEntry<T> : UCPresenter<T> where T : UCUnallocatedEntry
    {
        private long size;
        public DiskModel DiskModel { get; private set; }

        public bool? IsSelected { get { return UserControl.EntrySelected.IsChecked; } }

        public Dictionary<string, object?> EntryData
        {
            get
            {
                Dictionary<string, object?> dict = new Dictionary<string, object?>();
                dict.Add("Unallocated Space", UserControl.Size.Content);

                return dict;
            }
        }

        public void SelectEntryRadioButton()
        {
            UserControl.EntrySelected.IsChecked = !UserControl.EntrySelected.IsChecked;
        }

        private void SetSize(string size)
        {
            UserControl.Size.Content = size;
        }

        #region OnClick

        private void OnButton_Click(object sender, RoutedEventArgs e)
        {
            SelectEntryRadioButton();
            MainWindow.OnUnallocatedEntry_Click(UserControl);
        }

        private void OnCreatePart_Click(object sender, RoutedEventArgs e)
        {
            App.Instance.WIM.CreateWPresenter<PCreatePartition>(true, DiskModel, size);
        }

        private void OnCreateVolume_Click(object sender, RoutedEventArgs e)
        {
            WCreateVolume createVolumeWindow = new WCreateVolume(DiskModel, size);
            createVolumeWindow.Owner = MainWindow.Window;
            createVolumeWindow.Focus();

            createVolumeWindow.Show();
        }

        private void OnOpenContextMenu_Click(object sender, RoutedEventArgs e)
        {
            UserControl.ContextMenu.IsOpen = !UserControl.ContextMenu.IsOpen;
        }

        #endregion OnClick

        #region UCPresenter

        public override void Setup()
        {
            size = DiskModel.UnallocatedSpace;
            SetSize(ByteFormatter.BytesToUnitAsString(size));
        }

        protected override void RegisterEventsInternal()
        {
            UserControl.EButton_Click += OnButton_Click;
            UserControl.ECreatePart_Click += OnCreatePart_Click;
            UserControl.ECreateVolume_Click += OnCreateVolume_Click;
            UserControl.EOpenContextMenu_Click += OnOpenContextMenu_Click;
        }

        public override void AddCustomArgs(params object?[] args)
        {
            DiskModel = (DiskModel)args[0];
        }

        #endregion UCPresenter
    }
}
