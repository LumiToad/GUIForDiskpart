using GUIForDiskpart.Presentation.Presenter;
using GUIForDiskpart.Service;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;


namespace GUIForDiskpart.Presentation.View.UserControls
{
    /// <summary>
    /// Interaction logic for UnallocatedEntryUI.xaml
    /// </summary>
    public partial class UnallocatedEntryUI : UserControl
    {
        PMainWindow<GUIFDMainWin> MainWindow = App.Instance.WIM.GetPresenter<PMainWindow<GUIFDMainWin>>();

        private long size;
        private DiskModel diskModel;

        public Dictionary<string, object?> Entry
        {
            get 
            {
                Dictionary<string, object?> dict = new Dictionary<string, object?>();
                dict.Add("Unallocated Space", Size.Content); 

                return dict;
            }
        }

        public bool? IsSelected { get { return EntrySelected.IsChecked; } }

        public UnallocatedEntryUI(DiskModel diskModel)
        {
            InitializeComponent();
            this.diskModel = diskModel;
            size = diskModel.UnallocatedSpace;
            SetSize(ByteFormatter.FormatBytes(size));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SelectEntryRadioButton();
            MainWindow.OnUnallocatedEntry_Click(this);
        }

        public void SelectEntryRadioButton()
        {
            EntrySelected.IsChecked = !EntrySelected.IsChecked;
        }

        private void CreatePart_Click(object sender, RoutedEventArgs e)
        {
            CreatePartitionWindow createPartitionWindow = new CreatePartitionWindow(diskModel, size);
            createPartitionWindow.Owner = MainWindow.Window;
            createPartitionWindow.Focus();

            createPartitionWindow.Show();
        }

        private void CreateVolume_Click(object sender, RoutedEventArgs e)
        {
            CreateVolumeWindow createVolumeWindow = new CreateVolumeWindow(diskModel, size);
            createVolumeWindow.Owner = MainWindow.Window;
            createVolumeWindow.Focus();

            createVolumeWindow.Show();
        }

        private void OpenContextMenu_Click(object sender, RoutedEventArgs e)
        {
            ContextMenu.IsOpen = !ContextMenu.IsOpen;
        }

        private void SetSize(string size)
        {
            Size.Content = size;
        }
    }
}
