using GUIForDiskpart.diskpart;
using GUIForDiskpart.main;
using GUIForDiskpart.windows;
using System.Windows;
using System.Windows.Controls;

namespace GUIForDiskpart
{
    /// <summary>
    /// Interaktionslogik für PartitionDriveEntryUI.xaml
    /// </summary>
    public partial class PartitionEntryUI : UserControl
    {
        MainWindow MainWindow => (MainWindow)Application.Current.MainWindow;

        private WSMPartition wsmPartition;
        public WSMPartition WSMPartition
        {
            get { return wsmPartition; }
            set
            {
                wsmPartition = value;
                PartitionDataToThisUI();
            }
        }

        private const string partitionBorder = "#FF00C4B4";
        private const string logicalBorder = "#FF0A70C5";

        private const string basicBackground = "#FFBBBBBB";
        private const string selectBackground = "#FF308EBF";

        public bool? IsSelected { get { return EntrySelected.IsChecked; } }

        public PartitionEntryUI(WSMPartition wsmPartition)
        {
            InitializeComponent();

            WSMPartition = wsmPartition;
        }

        private void PartitionDataToThisUI()
        {
            PartitionNumber.Content = $"#{WSMPartition.PartitionNumber}";
            DriveNameAndLetter.Content = GetDriveNameText();
            TotalSpace.Content = WSMPartition.FormattedSize;
            FileSystemText.Content = GetFileSystemText();
            PartitionType.Content = $"{WSMPartition.PartitionTable}: {WSMPartition.PartitionType}";

            if (WSMPartition.WMIPartition != null && WSMPartition.WMIPartition.LogicalDriveInfo != null)
            {
                SetValueInProgressBar(WSMPartition.Size, WSMPartition.WMIPartition.LogicalDriveInfo.FreeSpace);
            }

            if (WSMPartition.IsBoot)
            {
                WinVolumeIcon.Source = Win32Icons.GetSystemIconByType(SystemIconType.WinLogo);
            }

            PopulateContextMenu();
        }

        private void PopulateContextMenu()
        {
            if (WSMPartition.WMIPartition != null && WSMPartition.WMIPartition.LogicalDriveInfo != null) 
            {
                MenuItem menuItem = new MenuItem();
                menuItem.Icon = "..\\..\\resources\\CmdC.png";
                menuItem.Header = "CMD - CHKDSK";
                menuItem.Click += ScanVolume_Click;
                ContextMenu.Items.Add(new Separator());
                ContextMenu.Items.Add(menuItem);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SelectEntryRadioButton();
            MainWindow.PartitionEntry_Click(this);
        }

        public void SelectEntryRadioButton()
        {
            EntrySelected.IsChecked = !EntrySelected.IsChecked;
        }

        private void Detail_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.AddTextToOutputConsole(DPFunctions.DetailPart(wsmPartition.DiskNumber, wsmPartition.PartitionNumber));
        }

        private void Format_Click(object sender, RoutedEventArgs e)
        {
            FormatPartitionWindow formatPartitionWindow = new FormatPartitionWindow(WSMPartition);
            formatPartitionWindow.Owner = MainWindow;
            formatPartitionWindow.Show();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            DeleteWindow deleteWindow = new DeleteWindow(WSMPartition);
            deleteWindow.Owner = MainWindow;
            deleteWindow.Show();
        }

        private void ExecuteDelete(bool value)
        {
            string output = string.Empty;

            output += DPFunctions.Delete(WSMPartition.DiskNumber, WSMPartition.PartitionNumber, false, true);

            MainWindow.AddTextToOutputConsole(output);
            MainWindow.RetrieveAndShowDiskData(false);
        }

        private void Assign_Click(object sender, RoutedEventArgs e)
        {
            AssignLetterWindow assignLetterWindow = new AssignLetterWindow(WSMPartition);
            assignLetterWindow.Owner = MainWindow;

            assignLetterWindow.Show();
        }

        private void ScanVolume_Click(object sender, RoutedEventArgs e)
        {
            OpenScanVolumeWindow();
        }

        public void OpenScanVolumeWindow()
        {
            CHKDSKWindow window = new CHKDSKWindow(WSMPartition);
            window.Owner = MainWindow;
            window.Show();
        }

        private string GetDriveNameText()
        {
            string driveNameText = string.Empty;
            
            if 
                (
                (WSMPartition.WMIPartition) != null &&
                (WSMPartition.WMIPartition.LogicalDriveInfo) != null &&
                (!string.IsNullOrEmpty(WSMPartition.WMIPartition.LogicalDriveInfo.VolumeName))
                )
            {
                driveNameText += $"{WSMPartition.WMIPartition.LogicalDriveInfo.VolumeName} ";
            }

            if (WSMPartition.DriveLetter > 65)
            {
                driveNameText += $"[{WSMPartition.DriveLetter}:\\]";
            }

            if (driveNameText == string.Empty)
            {
                driveNameText = "No letter";
            }

            return driveNameText;
        }

        private string GetFileSystemText()
        {
            if
                (
                WSMPartition.WMIPartition != null &&
                WSMPartition.WMIPartition.LogicalDriveInfo != null &&
                WSMPartition.WMIPartition.LogicalDriveInfo.FileSystem != null
                )
            {
                return WSMPartition.WMIPartition.LogicalDriveInfo.FileSystem;
            }
            else
            {
                return "No Windows Volume";
            }
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
