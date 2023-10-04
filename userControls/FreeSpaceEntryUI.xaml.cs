﻿using GUIForDiskpart.diskpart;
using GUIForDiskpart.main;
using GUIForDiskpart.windows;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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

        private DriveInfo driveInfo;

        private const string freeSpaceBorder = "#FFE3E3E3";

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

        public FreeSpaceEntryUI(UInt64 space, DriveInfo drive)
        {
            InitializeComponent();
            
            freeSpace = space;
            driveInfo = drive;

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
            CreatePartitionWindow createPartitionWindow = new CreatePartitionWindow(driveInfo, freeSpace);
            createPartitionWindow.Owner = mainWindow;
            createPartitionWindow.Focus();
            
            createPartitionWindow.Show();
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

        private void ChangeBackgroundColor(string backgroundColorValue)
        {
            var backgroundBrushColor = new BrushConverter();
            MainGrid.Background = (Brush)backgroundBrushColor.ConvertFrom(backgroundColorValue);
        }
    }
}
