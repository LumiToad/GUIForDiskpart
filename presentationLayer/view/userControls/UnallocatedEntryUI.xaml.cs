using GUIForDiskpart.Model;
using GUIForDiskpart.Service;
using GUIForDiskpart.Windows;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GUIForDiskpart.Presentation.View.UserControls
{
    /// <summary>
    /// Interaction logic for UnallocatedEntryUI.xaml
    /// </summary>
    public partial class UnallocatedEntryUI : UserControl
    {
        MainWindow MainWindow => (MainWindow)Application.Current.MainWindow;

        private long size;
        private DiskInfo diskInfo;

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

        public UnallocatedEntryUI(DiskInfo diskInfo)
        {
            InitializeComponent();
            this.diskInfo = diskInfo;
            size = diskInfo.UnallocatedSpace;
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
            CreatePartitionWindow createPartitionWindow = new CreatePartitionWindow(diskInfo, size);
            createPartitionWindow.Owner = MainWindow;
            createPartitionWindow.Focus();

            createPartitionWindow.Show();
        }

        private void CreateVolume_Click(object sender, RoutedEventArgs e)
        {
            CreateVolumeWindow createVolumeWindow = new CreateVolumeWindow(diskInfo, size);
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
