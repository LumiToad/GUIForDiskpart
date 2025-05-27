using GUIForDiskpart.Model;
using GUIForDiskpart.Service;
using GUIForDiskpart.Windows;
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
        Window? MainWindow = GUIForDiskpart.App.AppInstance.MainWindow;

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
            MainWindow.UnallocatedEntry_Click(this);
        }

        public void SelectEntryRadioButton()
        {
            EntrySelected.IsChecked = !EntrySelected.IsChecked;
        }

        private void CreatePart_Click(object sender, RoutedEventArgs e)
        {
            CreatePartitionWindow createPartitionWindow = new CreatePartitionWindow(diskModel, size);
            createPartitionWindow.Owner = MainWindow;
            createPartitionWindow.Focus();

            createPartitionWindow.Show();
        }

        private void CreateVolume_Click(object sender, RoutedEventArgs e)
        {
            Presentation.View.Windows.CreateVolumeWindow createVolumeWindow = new Presentation.View.Windows.CreateVolumeWindow(diskModel, size);
            createVolumeWindow.Owner = MainWindow;
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
