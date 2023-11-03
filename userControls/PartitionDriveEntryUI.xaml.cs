using GUIForDiskpart.diskpart;
using GUIForDiskpart.main;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GUIForDiskpart
{
    /// <summary>
    /// Interaktionslogik für PartitionDriveEntryUI.xaml
    /// </summary>
    public partial class PartitionDriveEntryUI : UserControl
    {
        MainWindow mainWindow;
        WMIPartition wmiPartition;
        WSMPartition wsmPartition;

        private const string partitionBorder = "#FF00C4B4";
        private const string logicalBorder = "#FF0A70C5";
        
        private const string basicBackground = "#FFBBBBBB";
        private const string selectBackground = "#FF308EBF";

        private bool isSelected = false;
        public bool IsSelected 
        { 
            get 
            {
                return isSelected; 
            } 
            
            private set
            {
                isSelected = value;
            }
        }


        public PartitionDriveEntryUI()
        {
            InitializeComponent();

            Initialize();
        }

        private void Initialize()
        {
            mainWindow = (MainWindow)Application.Current.MainWindow;
        }

        public void AddPartitionInfo(WMIPartition wmiPartition)
        {
            this.wmiPartition = wmiPartition;
            PartitionDataToThisUI();
        }

        public void AddPartitionInfo(WSMPartition wsmPartition)
        {
            this.wsmPartition = wsmPartition;
            PartitionDataToThisUI();
        }

        private void PartitionDataToThisUI()
        {
            PartitionNumberValue.Text = wsmPartition.PartitionNumber.ToString();
            BootPartitionValue.IsChecked = wsmPartition.IsBoot;
            
            string bytes = ByteFormatter.FormatBytes(Convert.ToInt64(wsmPartition.Size));

            TotalSizeValue.Text = bytes;
            PartitionTableValue.Text = wsmPartition.PartitionTable;

            DriveLetterValue.Text = wsmPartition.DriveLetter.ToString();

            TypeValue.Text = wsmPartition.PartitionType;

            ChangeUIBorder(partitionBorder);


            //if (wmiPartition.IsLogicalPartition())
            //{
            //    VolumeNameValue.Text = wmiPartition.LogicalDriveInfo.VolumeName;
            //    DriveLetterValue.Text = wmiPartition.LogicalDriveInfo.DriveLetter;
            //    FreeSpaceValue.Text = wmiPartition.LogicalDriveInfo.FreeSpace.ToString();
            //    FileSystemValue.Text = wmiPartition.LogicalDriveInfo.FileSystem;
            //    ChangeUIBorder(logicalBorder);
            //}
            //else
            //{
            //    VolumeNameValue.Text = "";
            //    DriveLetterValue.Text = "";
            //    FreeSpaceValue.Text = "";
            //    ChangeUIBorder(partitionBorder);
            //}
        }

        private void ChangeUIBorder(string borderColorValue)
        {
            var borderBrushColor = new BrushConverter();
            UserControl.BorderBrush = (Brush)borderBrushColor.ConvertFrom(borderColorValue);
        }

        private void ChangeBackgroundColor(string backgroundColorValue)
        {
            var backgroundBrushColor = new BrushConverter();
            MainGrid.Background = (Brush)backgroundBrushColor.ConvertFrom(backgroundColorValue);
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

            output += DPFunctions.Delete(wmiPartition.DiskIndex, wmiPartition.PartitionIndex, false, true);

            mainWindow.AddTextToOutputConsole(output);
            mainWindow.RetrieveAndShowDiskData(false);
        }

        private void Assign_Click(object sender, RoutedEventArgs e)
        {

        }

        private void UserControl_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //Console.WriteLine(sender.ToString());

            MarkAsSelected();
        }

        public void MarkAsSelected()
        {            
            IsSelected = !IsSelected;

            if (IsSelected) 
            {
                ChangeBackgroundColor(selectBackground);
            }
            else
            {
                ChangeBackgroundColor(basicBackground);
            }
        }
    }
}
