using GUIForDiskpart.diskpart;
using GUIForDiskpart.main;
using System;
using System.Windows;
using System.Windows.Controls;

namespace GUIForDiskpart
{
    /// <summary>
    /// Interaktionslogik für FreeSpaceEntryUI.xaml
    /// </summary>
    public partial class FreeSpaceEntryUI : UserControl
    {
        MainWindow mainWindow;
        DPFunctions dpFunctions;

        private ulong freeSpace;

        private int driveIndex;
        
        public FreeSpaceEntryUI(UInt64 space, int driveIndex)
        {
            InitializeComponent();
            
            freeSpace = space;
            this.driveIndex = driveIndex;

            Initialize();
        }

        private void Initialize()
        {
            mainWindow = (MainWindow)Application.Current.MainWindow;
            dpFunctions = mainWindow.mainProgram.dpFunctions;

            AddSpaceToUI();
        }

        public void AddSpaceToUI()
        {
            ByteFormatter byteFormatter = new ByteFormatter();

            UnPartSpaceValue.Content = byteFormatter.FormatBytes((long)freeSpace);
            
        }

        private void CreatePartition_Click(object sender, RoutedEventArgs e)
        {
            ///Just a test!
            dpFunctions.CreatePartition(driveIndex, CreatePartitionOptions.PRIMARY, 0, false);
        }

        private void CreateVolume_Click(object sender, RoutedEventArgs e)
        {
            //dpFunctions.CreatePartition();
        }

        private void CreateVDisk_Click(object sender, RoutedEventArgs e)
        {
            //dpFunctions.CreatePartition();
        }

        private void MultiCreate_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
