using GUIForDiskpart.diskpart;
using GUIForDiskpart.main;
using GUIForDiskpart.windows;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GUIForDiskpart
{
    /// <summary>
    /// Interaktionslogik für PhysicalDriveEntryUI.xaml
    /// </summary>
    public partial class PhysicalDiskEntryUI : UserControl
    {
        MainWindow mainWindow;
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

        public PhysicalDiskEntryUI()
        {
            InitializeComponent();
            Initialize();
        }

        private void Initialize()
        {
            mainWindow = (MainWindow)Application.Current.MainWindow;
        }

        private void DriveDataToThisUI()
        {
            DiskIndex.Content = $"#{diskInfo.DiskIndex}";
            DiskModel.Content = diskInfo.DiskModel;
            TotalSpace.Content = diskInfo.FormattedTotalSpace;
            WSMPartitionCount.Content = $"{diskInfo.WSMPartitionCount} partitions";


            DiskIcon.Source = GetDiskIcon();
            MediaTypeIcon.Source = GetMediaTypeIcon();
        }

        private void Detail_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.AddTextToOutputConsole(DPFunctions.DetailDisk(diskInfo.DiskIndex));
        }

        private void ListPart_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.AddTextToOutputConsole(DPFunctions.ListPart(diskInfo.DiskIndex));
        }

        private void Clean_Click(object sender, RoutedEventArgs e)
        {
            //Still needs "clean all" option

            MessageBoxResult messageBoxResult = MessageBox.Show("This will delete and also clean everything on this drive!",
               "Are you sure?", MessageBoxButton.OKCancel, MessageBoxImage.Warning);

            if (messageBoxResult == MessageBoxResult.OK) 
            {
                string output = string.Empty;

                output = DPFunctions.Clean(diskInfo.DiskIndex, false);

                mainWindow.AddTextToOutputConsole(output);

                return;
            }
            else
            {
                return;
            }
        }

        private void Convert_Click(object sender, RoutedEventArgs e)
        {
            ConvertDriveWindow convertDriveWindow = new ConvertDriveWindow(diskInfo);
            convertDriveWindow.Owner = mainWindow;
            convertDriveWindow.Focus();
            
            convertDriveWindow.Show();
        }

        private void CreatePart_Click(object sender, RoutedEventArgs e)
        {
            CreatePartitionWindow createPartitionWindow = new CreatePartitionWindow(diskInfo);
            createPartitionWindow.Owner = mainWindow;
            createPartitionWindow.Focus();

            createPartitionWindow.Show();
        }

        private void CreateVolume_Click(object sender, RoutedEventArgs e)
        {
            CreateVolumeWindow createVolumeWindow = new CreateVolumeWindow(diskInfo);
            createVolumeWindow.Owner = mainWindow;
            createVolumeWindow.Focus();

            createVolumeWindow.Show();
        }

        private void Format_Click(object sender, RoutedEventArgs e)
        {
            FormatDriveWindow formatWindow = new FormatDriveWindow(diskInfo);
            formatWindow.Owner = mainWindow;
            formatWindow.Focus();

            formatWindow.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SelectEntryRadioButton();
        }

        public void SelectEntryRadioButton()
        {
            EntrySelected.IsChecked = true;
            mainWindow.DiskEntry_Click(DiskInfo);
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
    }
}
