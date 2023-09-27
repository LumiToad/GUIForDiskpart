using GUIForDiskpart.diskpart;
using GUIForDiskpart.main;
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
        private const string partitionBackground = "#FFBBBBBB";

        private const string logicalBorder = "#FF0A70C5";
        private const string logicalBackground = "#FFBBBBBB";

        private const string freeSpaceBorder = "#FFE3E3E3";
        private const string freeSpaceBackground = "#FFBBBBBB";


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
            BootPartitionValue.Text = partitionInfo.BootPartition.ToString();
            TotalSizeValue.Text = partitionInfo.Size.ToString();
            PartitionTableValue.Text = partitionInfo.Type;

            if (partitionInfo.IsLogicalPartition())
            {
                VolumeNameValue.Text = partitionInfo.LogicalDriveInfo.VolumeName;
                DriveLetterValue.Text = partitionInfo.LogicalDriveInfo.FileSystem;
                FreeSpaceValue.Text = partitionInfo.LogicalDriveInfo.FreeSpace.ToString();
                ChangeUIColor(logicalBorder, logicalBackground);
            }
            else
            {
                VolumeNameValue.Text = "";
                DriveLetterValue.Text = "";
                FreeSpaceValue.Text = "";
                ChangeUIColor(partitionBorder, partitionBackground);
            }
        }

        private void ChangeUIColor(string border, string background)
        {
            var borderBrushColor = new BrushConverter();
            UserControl.BorderBrush = (Brush)borderBrushColor.ConvertFrom(border);

            var backgroundBrushColor = new BrushConverter();
            MainGrid.Background = (Brush)backgroundBrushColor.ConvertFrom(background);
        }

        private void Detail_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.AddTextToOutputConsole(dpFunctions.DetailPart(partitionInfo.DriveIndex, partitionInfo.PartitionIndex));
        }

        private void Format_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Assign_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
