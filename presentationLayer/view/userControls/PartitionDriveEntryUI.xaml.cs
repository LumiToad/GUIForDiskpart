using System.Windows;
using System.Windows.Controls;

using GUIForDiskpart.Model.Data;
using GUIForDiskpart.Model.Logic.Diskpart;
using GUIForDiskpart.Service;
using GUIForDiskpart.Utils;


namespace GUIForDiskpart.Presentation.View.UserControls
{
    /// <summary>
    /// Interaktionslogik für PartitionDriveEntryUI.xaml
    /// </summary>
    public partial class PartitionEntryUI : UserControl
    {
        Window? MainWindow = GUIForDiskpart.App.AppInstance.MainWindow;

        private PartitionModel partition;
        public PartitionModel Partition
        {
            get { return partition; }
            set
            {
                partition = value;
                PartitionDataToThisUI();
                PopulateContextMenu();
            }
        }

        private const string PARTITIONBORDER = "#FF00C4B4";
        private const string LOGICALBORDER = "#FF0A70C5";

        private const string BASICBACKGROUND = "#FFBBBBBB";
        private const string SELECTBACKGROUND = "#FF308EBF";

        public bool? IsSelected { get { return EntrySelected.IsChecked; } }

        public PartitionEntryUI(PartitionModel partition)
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
                SetValueInProgressBar(Partition.WSMPartition.Size, Partition.LDModel.UsedSpace);
            }

            if (Partition.WSMPartition.IsBoot)
            {
                WinVolumeIcon.Source = IconUtils.GetSystemIconByType(SystemIconType.WinLogo);
            }
        }

        private void PopulateContextMenu()
        {
            MenuItem offline = WPFUtils.CreateContextMenuItem(IconUtils.Diskpart, "DPOffline", "DISKPART - Offline", true, OnOffline_Click);
            MenuItem online = WPFUtils.CreateContextMenuItem(IconUtils.Diskpart, "DPOnline", "DISKPART - Online", true, OnOffline_Click);

            if (Partition.WSMPartition.PartitionTable == "MBR")
            {
                string header = "DISKPART - " + (Partition.WSMPartition.IsActive ? "Inactive" : "Active");
                string name = "DPInActive";
                MenuItem menuItem = WPFUtils.CreateContextMenuItem(IconUtils.Diskpart, name, header, true, Active_Click);
                ContextMenu.Items.Add(menuItem);
            }

            if (Partition.IsLogicalDisk && Partition.LDModel.DriveLetter != null)
            {
                string header = "DISKPART - Attributes";
                string name = "DPAttributes";
                MenuItem menuItem = WPFUtils.CreateContextMenuItem(IconUtils.Diskpart, name, header, true, Attributes_Click);
                ContextMenu.Items.Add(menuItem);

                header = "DISKPART - Shrink";
                name = "DPShrink";
                menuItem = WPFUtils.CreateContextMenuItem(IconUtils.Diskpart, name, header, true, Shrink_Click);
                ContextMenu.Items.Add(menuItem);

                header = "DISKPART - Extend";
                name = "DPExtend";
                menuItem = WPFUtils.CreateContextMenuItem(IconUtils.Diskpart, name, header, true, Extend_Click);
                ContextMenu.Items.Add(menuItem);

                ContextMenu.Items.Add(offline);
            }

            if (!ContextMenu.Items.Contains(offline))
            { ContextMenu.Items.Add((Partition.WSMPartition.DriveLetter < 65) ? online : offline); }

            if (Partition.IsLogicalDisk) 
            {
                string header = "CMD - CHKDSK";
                string name = "CMDCHDSK";
                MenuItem menuItem = WPFUtils.CreateContextMenuItem(IconUtils.CMD, name, header, true, ScanVolume_Click);
                ContextMenu.Items.Add(new Separator());
                ContextMenu.Items.Add(menuItem);
            }

            if (Partition.IsVolume)
            {
                string header = "Powershell - DefragAnalysis";
                string name = "PSAnalyzeDefrag";
                MenuItem menuItem = WPFUtils.CreateContextMenuItem(IconUtils.Commandline, name, header, true, AnalyzeDefrag_Click);
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

        private void OnOffline_Click(object sender, RoutedEventArgs e)
        {
            string output = string.Empty;

            uint diskIndex = Partition.WSMPartition.DiskNumber;
            uint partitionIndex = Partition.WSMPartition.PartitionNumber;
            char driveLetter = Partition.WSMPartition.DriveLetter;

            if (Partition.WSMPartition.IsOffline)
            {
                output = DPFunctions.OnlineVolume(diskIndex, partitionIndex, false);
            }
            else 
            {
                output = (Partition.WSMPartition.DriveLetter < 65) ?
                DPFunctions.OfflineVolume(diskIndex, partitionIndex, false) :
                DPFunctions.OfflineVolume(driveLetter, false);
            }
            MainWindow.AddTextToOutputConsole(output);
            MainWindow.RetrieveAndShowDiskData(false);
        }

        private void Extend_Click(object sender, RoutedEventArgs e)
        {
            ExtendWindow window = new ExtendWindow(Partition);
            window.Owner = MainWindow;
            window.Show();
        }

        private void Shrink_Click(object sender, RoutedEventArgs e)
        {
            GUIForDiskpart.Presentation.View.Windows.ShrinkWindow window = new GUIForDiskpart.Presentation.View.Windows.ShrinkWindow(Partition);
            window.Owner = MainWindow;
            window.Show();
        }

        private void AnalyzeDefrag_Click(object sender, RoutedEventArgs e)
        {
            Partition.DefragAnalysis = DefragAnalysisRetriever.AnalyzeVolumeDefrag(Partition);
            MainWindow.AddTextToOutputConsole(Partition.DefragAnalysis.GetOutputAsString());
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
                MessageBoxButton.OKCancel
                );
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
            if (Partition.IsLogicalDisk && Partition.LDModel.DriveLetter == null) return;
            CHKDSKWindow window = new CHKDSKWindow(Partition);
            window.Owner = MainWindow;
            window.Show();
        }

        private string GetDriveNameText()
        {
            string driveNameText = string.Empty;
            
            if (Partition.IsLogicalDisk && (!string.IsNullOrEmpty(Partition.LDModel.VolumeName)))
            {
                driveNameText += $"{Partition.LDModel.VolumeName} ";
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
            if (Partition.IsLogicalDisk && !string.IsNullOrWhiteSpace(Partition.LDModel.FileSystem))
            {
                return Partition.LDModel.FileSystem;
            }
            
            if (Partition.IsLogicalDisk && string.IsNullOrWhiteSpace(Partition.LDModel.FileSystem))
            {
                return "No filesystem";
            }

            return "No Windows Volume";
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
