using GUIForDiskpart.diskpart;
using GUIForDiskpart.main;
using GUIForDiskpart.windows;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace GUIForDiskpart
{
    /// <summary>
    /// Interaktionslogik für PhysicalDriveEntryUI.xaml
    /// </summary>
    public partial class PhysicalDiskEntryUI : UserControl
    {
        MainWindow MainWindow => (MainWindow)Application.Current.MainWindow;

        private DiskInfo diskInfo;
        public DiskInfo DiskInfo
        { 
            get { return diskInfo; } 
            set 
            { 
                diskInfo = value; 
                DriveDataToThisUI();
            }
        }

        public bool? IsSelected { get { return EntrySelected.IsChecked; } }

        public PhysicalDiskEntryUI(DiskInfo diskInfo)
        {
            InitializeComponent();
            DiskInfo = diskInfo;
        }

        private void DriveDataToThisUI()
        {
            DiskIndex.Content = $"#{diskInfo.DiskIndex}";
            DiskModel.Content = diskInfo.DiskModel;
            TotalSpace.Content = diskInfo.FormattedTotalSpace;
            WSMPartitionCount.Content = $"{diskInfo.WSMPartitionCount} partitions";
            SetValueInProgressBar(diskInfo.TotalSpace, diskInfo.FreeSpace);

            DiskIcon.Source = GetDiskIcon();
            MediaTypeIcon.Source = GetMediaTypeIcon();
        }

        private void Detail_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.AddTextToOutputConsole(DPFunctions.DetailDisk(diskInfo.DiskIndex));
        }

        private void ListPart_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.AddTextToOutputConsole(DPFunctions.ListPart(diskInfo.DiskIndex));
        }

        private void Clean_Click(object sender, RoutedEventArgs e)
        {
            CleanWindow cleanWindow = new CleanWindow(DiskInfo);
            cleanWindow.Owner = MainWindow;

            cleanWindow.Show();
        }

        private void Convert_Click(object sender, RoutedEventArgs e)
        {
            ConvertDriveWindow convertDriveWindow = new ConvertDriveWindow(diskInfo);
            convertDriveWindow.Owner = MainWindow;
            convertDriveWindow.Focus();
            
            convertDriveWindow.Show();
        }

        private void CreatePart_Click(object sender, RoutedEventArgs e)
        {
            CreatePartitionWindow createPartitionWindow = new CreatePartitionWindow(diskInfo);
            createPartitionWindow.Owner = MainWindow;
            createPartitionWindow.Focus();

            createPartitionWindow.Show();
        }

        private void CreateVolume_Click(object sender, RoutedEventArgs e)
        {
            CreateVolumeWindow createVolumeWindow = new CreateVolumeWindow(diskInfo);
            createVolumeWindow.Owner = MainWindow;
            createVolumeWindow.Focus();

            createVolumeWindow.Show();
        }

        private void Format_Click(object sender, RoutedEventArgs e)
        {
            FormatDriveWindow formatWindow = new FormatDriveWindow(diskInfo);
            formatWindow.Owner = MainWindow;
            formatWindow.Focus();

            formatWindow.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SelectEntryRadioButton();
            MainWindow.DiskEntry_Click(this);
        }

        public void SelectEntryRadioButton()
        {
            EntrySelected.IsChecked = true;
        }

        private ImageSource? GetDiskIcon()
        {
            ImageSource? result = Win32Icons.GetShellIconByType(Shell32IconType.Drive, true);

            if (diskInfo.InterfaceType == "USB")
            {
                result = Win32Icons.GetShellIconByType(Shell32IconType.USB, true);
            }

            return result;
        }

        private ImageSource? GetMediaTypeIcon()
        {
            ImageSource? result = Win32Icons.GetShellIconByType(Shell32IconType.QuestionMark, true);

            switch (diskInfo.MediaType)
            {
                case ("External hard disk media"):
                    result = Win32Icons.GetShellIconByType(Shell32IconType.UpArrow, true);
                    break;
                case ("Removable Media"):
                    result = Win32Icons.GetShellIconByType(Shell32IconType.UpArrow, true);
                    break;
                case ("Fixed hard disk media"):
                    result = Win32Icons.GetShellIconByType(Shell32IconType.Fixed, true);
                    break;
                case ("Unknown"):
                    result = Win32Icons.GetShellIconByType(Shell32IconType.QuestionMark, true);
                    break;
            }

            return result;
        }

        private void OpenContextMenu_Click(object sender, RoutedEventArgs e)
        {
            ContextMenu.IsOpen = !ContextMenu.IsOpen;
        }

        private void SetValueInProgressBar(ulong totalSize, ulong freeSize)
        {
            SizeBar.Maximum = totalSize;
            SizeBar.Minimum = 0;
            SizeBar.Value = totalSize - freeSize;
        }
    }
}
