using GUIForDiskpart.diskpart;
using GUIForDiskpart.main;
using System;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GUIForDiskpart
{
    /// <summary>
    /// Interaktionslogik für PartitionDriveEntryUI.xaml
    /// </summary>
    public partial class PartitionEntryUI : UserControl
    {
        MainWindow mainWindow;

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

        public PartitionEntryUI()
        {
            InitializeComponent();

            Initialize();
        }

        private void Initialize()
        {
            mainWindow = (MainWindow)Application.Current.MainWindow;
        }

        private void PartitionDataToThisUI()
        {
            PartitionNumber.Content = $"#{WSMPartition.PartitionNumber}";

            DriveNameAndLetter.Content = GetDriveNameText();

            TotalSpace.Content = WSMPartition.FormattedSize;
            IsBoot.IsChecked = WSMPartition.IsBoot;
            PartitionType.Content = $"{WSMPartition.PartitionTable}: {WSMPartition.PartitionType}";

            if (WSMPartition.IsBoot)
            {
               WinVolumeIcon.Source = Win32Icons.GetSystemIconByType(SystemIconType.WinLogo);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SelectEntryRadioButton();
        }

        public void SelectEntryRadioButton()
        {
            EntrySelected.IsChecked = !EntrySelected.IsChecked;
            mainWindow.PartitionEntry_Click(this);
        }

        private void Detail_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.AddTextToOutputConsole(DPFunctions.DetailPart(wsmPartition.DiskNumber, wsmPartition.PartitionNumber));
        }

        private void Format_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            string output = string.Empty;

            //output += DPFunctions.Delete(WSMPartition.DiskNumber, WSMPartition.PartitionNumber, false, true);

            mainWindow.AddTextToOutputConsole(output);
            mainWindow.RetrieveAndShowDiskData(false);
        }

        private void Assign_Click(object sender, RoutedEventArgs e)
        {

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
    }
}
