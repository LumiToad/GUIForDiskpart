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
        DPFunctions dpFunctions;

        PartitionInfo partitionInfo;

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
            dpFunctions = mainWindow.mainProgram.dpFunctions;
        }

        public void AddPartitionInfo(PartitionInfo partitionInfo)
        {
            this.partitionInfo = partitionInfo;
            PartitionDataToThisUI();
        }

        public void PartitionDataToThisUI()
        {
            PartitionNameValue.Text = partitionInfo.PartitionName;
            BootPartitionValue.IsChecked = partitionInfo.BootPartition;
            TotalSizeValue.Text = partitionInfo.Size.ToString();
            PartitionTableValue.Text = partitionInfo.Type;

            if (partitionInfo.IsLogicalPartition())
            {
                VolumeNameValue.Text = partitionInfo.LogicalDriveInfo.VolumeName;
                DriveLetterValue.Text = partitionInfo.LogicalDriveInfo.DriveLetter;
                FreeSpaceValue.Text = partitionInfo.LogicalDriveInfo.FreeSpace.ToString();
                FileSystemValue.Text = partitionInfo.LogicalDriveInfo.FileSystem;
                ChangeUIBorder(logicalBorder);
            }
            else
            {
                VolumeNameValue.Text = "";
                DriveLetterValue.Text = "";
                FreeSpaceValue.Text = "";
                ChangeUIBorder(partitionBorder);
            }
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
            mainWindow.AddTextToOutputConsole(dpFunctions.DetailPart(partitionInfo.DiskIndex, partitionInfo.PartitionIndex));
        }

        private void Format_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            DPFunctions dPFunctions = new DPFunctions();

            string output = string.Empty;

            output += dpFunctions.Delete(partitionInfo.DiskIndex, partitionInfo.PartitionIndex, false, true);

            mainWindow.AddTextToOutputConsole(output);
            mainWindow.RetrieveAndShowDriveData(false);
        }

        private void Assign_Click(object sender, RoutedEventArgs e)
        {

        }

        private void UserControl_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Console.WriteLine(sender.ToString());

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
