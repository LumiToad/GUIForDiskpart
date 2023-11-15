using GUIForDiskpart.diskpart;
using GUIForDiskpart.main;
using GUIForDiskpart.windows;
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
        MainWindow MainWindow => (MainWindow)Application.Current.MainWindow;

        private DiskInfo diskInfo;
        public DiskInfo DiskInfo
        { 
            get { return diskInfo; } 
            set 
            { 
                diskInfo = value; 
                DriveDataToThisUI();
                PopulateContextMenu();
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
            WSMPartitionCount.Content = $"{diskInfo.PartitionCount} partitions";
            SetValueInProgressBar(diskInfo.TotalSpace, diskInfo.UsedSpace);

            DiskIcon.Source = GetDiskIcon();
            MediaTypeIcon.Source = GetMediaTypeIcon();
        }

        private void PopulateContextMenu()
        {
            string header = string.Empty;
            string name = string.Empty;

            header = "DISKPART - Online";
            name = "DPOnline";

            if (DiskInfo.IsOnline) 
            {
                header = "DISKPART - Offline";
                name = "DPOffline";
            }
            MenuItem menuItem = WPFUtilites.CreateContextMenuItem(IconUtilities.Diskpart, name, header, true, OnOffline_Click);
            ContextMenu.Items.Add(menuItem);
        }

        private void OnOffline_Click(object sender, RoutedEventArgs e)
        {
            string output = string.Empty;

            if (DiskInfo.IsOnline) 
            {
                output += DPFunctions.OnOfflineDisk(DiskInfo.DiskIndex, false, false);
            }
            else 
            {
                output += DPFunctions.OnOfflineDisk(DiskInfo.DiskIndex, true, false);
            }

            MainWindow.AddTextToOutputConsole(output);
            MainWindow.RetrieveAndShowDiskData(false);
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

        private void EasyFormat_Click(object sender, RoutedEventArgs e)
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
            ImageSource? result = IconUtilities.GetShellIconByType(Shell32IconType.Drive, true);

            if (diskInfo.InterfaceType == "USB")
            {
                result = IconUtilities.GetShellIconByType(Shell32IconType.USB, true);
            }

            return result;
        }

        private ImageSource? GetMediaTypeIcon()
        {
            ImageSource? result = IconUtilities.GetShellIconByType(Shell32IconType.QuestionMark, true);

            switch (diskInfo.MediaType)
            {
                case ("External hard disk media"):
                    result = IconUtilities.GetShellIconByType(Shell32IconType.UpArrow, true);
                    break;
                case ("Removable Media"):
                    result = IconUtilities.GetShellIconByType(Shell32IconType.UpArrow, true);
                    break;
                case ("Fixed hard disk media"):
                    result = IconUtilities.GetShellIconByType(Shell32IconType.Fixed, true);
                    break;
                case ("Unknown"):
                    result = IconUtilities.GetShellIconByType(Shell32IconType.QuestionMark, true);
                    break;
            }

            return result;
        }

        private void OpenContextMenu_Click(object sender, RoutedEventArgs e)
        {
            ContextMenu.IsOpen = !ContextMenu.IsOpen;
        }

        private void SetValueInProgressBar(ulong totalSize, ulong usedSpace)
        {
            SizeBar.Maximum = totalSize;
            SizeBar.Minimum = 0;
            SizeBar.Value = usedSpace;
        }
    }
}
