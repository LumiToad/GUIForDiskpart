﻿using GUIForDiskpart.diskpart;
using GUIForDiskpart.main;
using System;
using System.Windows;
using System.Windows.Controls;


namespace GUIForDiskpart
{
    /// <summary>
    /// Interaktionslogik für PhysicalDriveEntryUI.xaml
    /// </summary>
    public partial class PhysicalDriveEntryUI : UserControl
    {
        MainWindow mainWindow;
        DPFunctions dpFunctions;

        DriveInfo driveInfo;

        public int DriveIndex { get { return driveInfo.DriveIndex; } }

        public PhysicalDriveEntryUI()
        {
            InitializeComponent();

            Initialize();
        }

        private void Initialize()
        {
            mainWindow = (MainWindow)Application.Current.MainWindow;
            dpFunctions = mainWindow.mainProgram.dpFunctions;
        }

        public void AddDriveInfo(DriveInfo physicalDrive)
        {
            this.driveInfo = physicalDrive;
            DriveDataToThisUI();
        }

        private void DriveDataToThisUI()
        {
            DriveNumberValueLabel.Content = driveInfo.DriveIndex.ToString();
            DiskNameValueLabel.Content = driveInfo.DiskName;

            ByteFormatter byteFormatter = new ByteFormatter();
            TotalSpaceValueLabel.Content = byteFormatter.FormatBytes((long)driveInfo.TotalSpace);

            StatusValueLabel.Content = driveInfo.MediaStatus;
            PartitionsValueLabel.Content = driveInfo.PartitionCount;

            foreach (PartitionInfo partitionInfo in driveInfo.PartitionDrives)
            {
                PartitionDriveEntryUI partitionDriveEntryUI = new PartitionDriveEntryUI();
                partitionDriveEntryUI.AddPartitionInfo(partitionInfo);
                PartitionStackPanel.Children.Add(partitionDriveEntryUI);
            }

            if (driveInfo.UnpartSpace > 0) 
            {
                Console.WriteLine(driveInfo.UnpartSpace.ToString());
                FreeSpaceEntryUI freeSpaceEntryUI = new FreeSpaceEntryUI(driveInfo.UnpartSpace, driveInfo.DriveIndex);
                PartitionStackPanel.Children.Add(freeSpaceEntryUI);
            }
        }

        private void Detail_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.AddTextToOutputConsole(dpFunctions.DetailDisk(driveInfo.DriveIndex));
        }

        private void Clean_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Convert_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CreatePart_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CreateVolume_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CreateVDisk_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Format_Click(object sender, RoutedEventArgs e)
        {
            FormatDriveWindow formatWindow = new FormatDriveWindow(driveInfo);
            formatWindow.Owner = mainWindow;
            formatWindow.Focus();

            formatWindow.Show();
        }

        private void ChildrenCount_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Count: " + PartitionStackPanel.Children.Count);
        }
    }
}