using GUIForDiskpart.Presentation.View.UserControls;
using GUIForDiskpart.Service;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Windows;
using System.Windows.Controls;


namespace GUIForDiskpart.Presentation.Presenter
{
    public class PUnallocatedEntry<T> : UCPresenter<T> where T : UCUnallocatedEntry
    {
        private long size;
        private DiskModel diskModel;

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
            WCreatePartition createPartitionWindow = new WCreatePartition(diskModel, size);
            createPartitionWindow.Owner = MainWindow.Window;
            createPartitionWindow.Focus();

            createPartitionWindow.Show();
        }

        private void OnCreateVolume_Click(object sender, RoutedEventArgs e)
        {
            WCreateVolume createVolumeWindow = new WCreateVolume(diskModel, size);
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
            size = diskModel.UnallocatedSpace;
            SetSize(ByteFormatter.FormatBytes(size));
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
            diskModel = (DiskModel)args[0];
        }

        #endregion UCPresenter
    }
}
