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

        private Partition partition;
        public Partition Partition
        {
            get { return partition; }
            set
            {
                partition = value;
                PartitionDataToThisUI();
            }
        }

        private const string partitionBorder = "#FF00C4B4";
        private const string logicalBorder = "#FF0A70C5";

        private const string basicBackground = "#FFBBBBBB";
        private const string selectBackground = "#FF308EBF";

        public bool? IsSelected { get { return EntrySelected.IsChecked; } }

        public PartitionEntryUI(Partition partition)
        {
            InitializeComponent();

            Partition = partition;
        }

        private void PartitionDataToThisUI()
        {
            PartitionNumber.Content = $"#{Partition.WSMPartition.PartitionNumber}";
            DriveNameAndLetter.Content = GetDriveNameText();
            TotalSpace.Content = Partition.WSMPartition.FormattedSize;
            FileSystemText.Content = GetFileSystemText();
            PartitionType.Content = $"{Partition.WSMPartition.PartitionTable}: {Partition.WSMPartition.PartitionType}";

            if (Partition.HasWSMPartition && Partition.IsLogicalDisk)
            {
                SetValueInProgressBar(Partition.WSMPartition.Size, Partition.LogicalDiskInfo.UsedSpace);
            }

            if (Partition.WSMPartition.IsBoot)
            {
                WinVolumeIcon.Source = IconUtilities.GetSystemIconByType(SystemIconType.WinLogo);
            }

            PopulateContextMenu();
        }

        private void PopulateContextMenu()
        {
            if (Partition.WSMPartition.PartitionTable == "MBR")
            {
                string header = "Diskpart - " + (Partition.WSMPartition.IsActive ? "Inactive" : "Active");
                string name = "DPInActive";
                MenuItem menuItem = WPFUtilites.CreateContextMenuItem(IconUtilities.Diskpart, name, header, true, Active_Click);
                ContextMenu.Items.Add(menuItem);
            }

            if (Partition.IsLogicalDisk && Partition.LogicalDiskInfo.DriveLetter != null)
            {
                string header = "DISKPART - Attributes";
                string name = "DPAttributes";
                MenuItem menuItem = WPFUtilites.CreateContextMenuItem(IconUtilities.Diskpart, name, header, true, Attributes_Click);
                ContextMenu.Items.Add(menuItem);
            }

            if (Partition.IsLogicalDisk) 
            {
                string header = "CMD - CHKDSK";
                string name = "CMDCHDSK";
                MenuItem menuItem = WPFUtilites.CreateContextMenuItem(IconUtilities.CMD, name, header, true, ScanVolume_Click);
                ContextMenu.Items.Add(new Separator());
                ContextMenu.Items.Add(menuItem);
            }

            if (Partition.IsVolume)
            {
                string header = "Powershell - DefragAnalysis";
                string name = "PSAnalyzeDefrag";
                MenuItem menuItem = WPFUtilites.CreateContextMenuItem(IconUtilities.Commandline, name, header, true, AnalyzeDefrag_Click);
                menuItem.ToolTip = 
                    (
                    new ToolTip().Content = 
                    "Will analyze the fragmentation of this volume." +
                    " Will not actually start a defragmentation process." +
                    " Retrieved data will be shown in the list entrys below." +
                    " Can take a while!"
                    );
                ContextMenu.Items.Add(menuItem);
            }
        }

        private void AnalyzeDefrag_Click(object sender, RoutedEventArgs e)
        {
            Partition.DefragAnalysis = DefragAnalysisRetriever.AnalyzeVolumeDefrag(Partition);
            MainWindow.EntryDataUI.AddDataToGrid(Partition.GetKeyValuePairs());
        }

        private void Attributes_Click(object sender, RoutedEventArgs e) 
        {
            AttributesVolumeWindow window = new AttributesVolumeWindow(Partition.WSMPartition);
            window.Owner = MainWindow;
            window.Show();
        }

        private void Active_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show
                (
                "Setting an MBR partition active or inactive can result in the computer no longer booting correctly!",
                "Diskpart - Active / Inactive",
                MessageBoxButton.OKCancel);
            switch (result)
            {
                case MessageBoxResult.OK:
                    MainWindow.AddTextToOutputConsole(DPFunctions.Active(Partition.WSMPartition.DiskNumber, Partition.WSMPartition.PartitionNumber, !Partition.WSMPartition.IsActive));
                    MainWindow.RetrieveAndShowDiskData(false);
                    break;
                case MessageBoxResult.Cancel:
                    break;
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
            MainWindow.AddTextToOutputConsole(DPFunctions.DetailPart(partition.WSMPartition.DiskNumber, partition.WSMPartition.PartitionNumber));
        }

        private void Format_Click(object sender, RoutedEventArgs e)
        {
            FormatPartitionWindow formatPartitionWindow = new FormatPartitionWindow(Partition.WSMPartition);
            formatPartitionWindow.Owner = MainWindow;
            formatPartitionWindow.Show();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            DeleteWindow deleteWindow = new DeleteWindow(Partition.WSMPartition);
            deleteWindow.Owner = MainWindow;
            deleteWindow.Show();
        }

        private void ExecuteDelete(bool value)
        {
            string output = string.Empty;

            output += DPFunctions.Delete(Partition.WSMPartition.DiskNumber, Partition.WSMPartition.PartitionNumber, false, true);

            MainWindow.AddTextToOutputConsole(output);
            MainWindow.RetrieveAndShowDiskData(false);
        }

        private void Assign_Click(object sender, RoutedEventArgs e)
        {
            AssignLetterWindow assignLetterWindow = new AssignLetterWindow(Partition.WSMPartition);
            assignLetterWindow.Owner = MainWindow;

            assignLetterWindow.Show();
        }

        private void ScanVolume_Click(object sender, RoutedEventArgs e)
        {
            OpenScanVolumeWindow();
        }

        public void OpenScanVolumeWindow()
        {
            if (Partition.IsLogicalDisk && Partition.LogicalDiskInfo.DriveLetter == null) return;
            CHKDSKWindow window = new CHKDSKWindow(Partition);
            window.Owner = MainWindow;
            window.Show();
        }

        private string GetDriveNameText()
        {
            string driveNameText = string.Empty;
            
            if (Partition.IsLogicalDisk && (!string.IsNullOrEmpty(Partition.LogicalDiskInfo.VolumeName)))
            {
                driveNameText += $"{Partition.LogicalDiskInfo.VolumeName} ";
            }

            if (Partition.WSMPartition.DriveLetter > 65)
            {
                driveNameText += $"[{Partition.WSMPartition.DriveLetter}:\\]";
            }

            if (driveNameText == string.Empty)
            {
                driveNameText = "No letter";
            }

            return driveNameText;
        }

        private string GetFileSystemText()
        {
            if (Partition.IsLogicalDisk && Partition.LogicalDiskInfo.FileSystem != null )
            {
                return Partition.LogicalDiskInfo.FileSystem;
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

        private void SetValueInProgressBar(ulong totalSize, ulong usedSpace)
        {
            SizeBar.Maximum = totalSize;
            SizeBar.Minimum = 0;
            SizeBar.Value = usedSpace;
        }
    }
}
