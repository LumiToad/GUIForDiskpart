using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace GUIForDiskpart.main
{
    public class DriveRetriever
    {
        MainWindow mainWindow;

        private List<DriveInfo> physicalDrives = new List<DriveInfo>();
        public List<DriveInfo> PhysicalDrives { get { return physicalDrives; } }

        private List<ManagementObject> managementObjectDrives = new List<ManagementObject>();

        Thread thread;

        public void Initialize()
        {
            mainWindow = (MainWindow)Application.Current.MainWindow;
            SetupDriveChangedWatcher();
        }

        public void RetrieveDrives()
        {
            RetrieveWMIObjectsToList();
            RetrieveDrivesToList();
        }

        public void ReloadDriveInformation()
        {
            DeleteDriveInformation();
            RetrieveDrives();
        }

        private void DeleteDriveInformation()
        {
            physicalDrives.Clear();
            managementObjectDrives.Clear();
        }

        public string GetDrivesOutput()
        {
            string output = string.Empty;

            foreach (DriveInfo physicalDrive in physicalDrives) 
            {
                output += physicalDrive.GetOutputAsString();
            }

            return output;
        }
        
        private void SetupDriveChangedWatcher()
        {
            try
            {
                WqlEventQuery query = new WqlEventQuery("SELECT * FROM Win32_VolumeChangeEvent WHERE EventType = 2 or EventType = 3");
                ManagementEventWatcher watcher = new ManagementEventWatcher();
                watcher.Query = query;
                watcher.EventArrived += new EventArrivedEventHandler(OnDriveChanged);
                watcher.Options.Timeout = TimeSpan.FromSeconds(3);
                watcher.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        
        private void OnDriveChanged(object sender, EventArrivedEventArgs e)
        {
            mainWindow.RetrieveAndShowDriveData(false);
        }

        private void RetrieveWMIObjectsToList()
        {
            ManagementObjectSearcher driveQuery = new ManagementObjectSearcher("select * from Win32_DiskDrive");

            foreach (ManagementObject drive in driveQuery.Get())
            {
                managementObjectDrives.Add(drive);
            }
        }

        private void RetrieveDrivesToList()
        {
            foreach (ManagementObject drive in managementObjectDrives)
            {
                string deviceId = Convert.ToString(drive.Properties["DeviceId"].Value);
                string physicalName = Convert.ToString(drive.Properties["Name"].Value);
                string diskName = Convert.ToString(drive.Properties["Caption"].Value);
                string diskModel = Convert.ToString(drive.Properties["Model"].Value);
                string status = Convert.ToString(drive.Properties["Status"].Value);
                bool mediaLoaded = Convert.ToBoolean(drive.Properties["MediaLoaded"].Value);
                UInt64 totalSpace = Convert.ToUInt64(drive.Properties["Size"].Value);
                UInt32 partitionCount = Convert.ToUInt32(drive.Properties["Partitions"].Value);
                string interfaceType = Convert.ToString(drive.Properties["InterfaceType"].Value);
                string mediaType = Convert.ToString(drive.Properties["MediaType"].Value);
                UInt32 mediaSignature = Convert.ToUInt32(drive.Properties["Signature"].Value);
                string mediaStatus = Convert.ToString(drive.Properties["Status"].Value);
                string driveName = Convert.ToString(drive.Properties["Name"].Value);
                UInt16 availability = Convert.ToUInt16(drive.Properties["Availability"].Value);
                UInt32 bytesPerSector = Convert.ToUInt32(drive.Properties["BytesPerSector"].Value);
                //UInt16 Capabilities[];
                //string CapabilityDescriptions[];
                string compressionMethod = Convert.ToString(drive.Properties["CompressionMethod"].Value);
                UInt32 configManagerErrorCode = Convert.ToUInt32(drive.Properties["ConfigManagerErrorCode"].Value);
                bool configManagerUserConfig = Convert.ToBoolean(drive.Properties["ConfigManagerUserConfig"].Value);
                string creationClassName = Convert.ToString(drive.Properties["CreationClassName"].Value);
                UInt64 defaultBlockSize = Convert.ToUInt64(drive.Properties["DefaultBlockSize"].Value);
                string description = Convert.ToString(drive.Properties["Description"].Value);
                bool errorCleared = Convert.ToBoolean(drive.Properties["ErrorCleared"].Value);
                string errorDescription = Convert.ToString(drive.Properties["ErrorDescription"].Value);
                string errorMethodology = Convert.ToString(drive.Properties["ErrorMethodology"].Value);
                string firmwareRevision = Convert.ToString(drive.Properties["ErrorMethodology"].Value);
                UInt32 index = Convert.ToUInt32(drive.Properties["Index"].Value);
                DateTime installDate = Convert.ToDateTime(drive.Properties["InstallDate"].Value);
                UInt32 lastErrorCode = Convert.ToUInt32(drive.Properties["LastErrorCode"].Value);
                string manufacturer = Convert.ToString(drive.Properties["Manufacturer"].Value);
                UInt64 maxBlockSize = Convert.ToUInt64(drive.Properties["MaxBlockSize"].Value);
                UInt64 maxMediaSize = Convert.ToUInt64(drive.Properties["MaxMediaSize"].Value); 
                UInt64 minBlockSize = Convert.ToUInt64(drive.Properties["MinBlockSize"].Value);
                bool needsCleaning = Convert.ToBoolean(drive.Properties["NeedsCleaning"].Value);
                UInt32 numberOfMediaSupported = Convert.ToUInt32(drive.Properties["NumberOfMediaSupported"].Value);
                string pnpDeviceID = Convert.ToString(drive.Properties["PNPDeviceID"].Value);
                //UInt16 PowerManagementCapabilities[];
                bool powerManagementSupported = Convert.ToBoolean(drive.Properties["PowerManagementSupported"].Value);
                UInt32 scsiBus = Convert.ToUInt32(drive.Properties["SCSIBus"].Value);
                UInt16 scsiLogicalUnit = Convert.ToUInt16(drive.Properties["SCSILogicalUnit"].Value);
                UInt16 scsiPort = Convert.ToUInt16(drive.Properties["SCSIPort"].Value);
                UInt16 scsiTargetId = Convert.ToUInt16(drive.Properties["SCSITargetId"].Value);
                UInt32 sectorsPerTrack = Convert.ToUInt32(drive.Properties["SectorsPerTrack"].Value);
                string serialNumber = Convert.ToString(drive.Properties["SerialNumber"].Value);
                UInt16 statusInfo = Convert.ToUInt16(drive.Properties["StatusInfo"].Value);
                string systemCreationClassName = Convert.ToString(drive.Properties["SystemCreationClassName"].Value);
                string systemName = Convert.ToString(drive.Properties["SystemName"].Value);
                UInt64 totalCylinders = Convert.ToUInt64(drive.Properties["TotalCylinders"].Value);
                UInt32 totalHeads = Convert.ToUInt32(drive.Properties["TotalHeads"].Value);
                UInt64 totalSectors = Convert.ToUInt64(drive.Properties["TotalSectors"].Value);
                UInt64 totalTracks = Convert.ToUInt64(drive.Properties["TotalTracks"].Value);
                UInt32 tracksPerCylinder = Convert.ToUInt32(drive.Properties["TracksPerCylinder"].Value);

                DriveInfo physicalDrive = new DriveInfo(deviceId, physicalName, diskName, diskModel, mediaStatus, mediaLoaded, totalSpace, partitionCount,
                    interfaceType, mediaSignature, driveName, mediaType, availability, bytesPerSector, compressionMethod, configManagerErrorCode,
                    configManagerUserConfig, creationClassName, defaultBlockSize, description, errorCleared, errorDescription, errorMethodology,
                    firmwareRevision, index, installDate, lastErrorCode, manufacturer, maxBlockSize, maxMediaSize, minBlockSize, needsCleaning,
                    numberOfMediaSupported, pnpDeviceID, powerManagementSupported, scsiBus, scsiLogicalUnit, scsiPort, scsiTargetId, sectorsPerTrack,
                    serialNumber, statusInfo, systemCreationClassName, systemName, totalCylinders, totalHeads, totalSectors, totalTracks, tracksPerCylinder);

                var partitionQueryText = string.Format("associators of {{{0}}} where AssocClass = Win32_DiskDriveToDiskPartition", drive.Path.RelativePath);
                var partitionQuery = new ManagementObjectSearcher(partitionQueryText);

                foreach (ManagementObject partition in partitionQuery.Get())
                {
                    physicalDrive.AddPartitionDriveToList(RetrievePartitions(partition, physicalDrive.DriveIndex));
                }

                physicalDrive.UnpartSpace = physicalDrive.CalcUnpartSpace(physicalDrive.TotalSpace);

                physicalDrives.Add(physicalDrive);
            }

            physicalDrives = SortPhysicalDrivesByDeviceID(physicalDrives);
        }

        private PartitionInfo RetrievePartitions(ManagementObject partition, uint driveIndex)
        {
            PartitionInfo newPartition = new PartitionInfo();
            
            newPartition.PartitionName = Convert.ToString(partition.Properties["Name"].Value);
            newPartition.Bootable = Convert.ToBoolean(partition.Properties["Bootable"].Value);
            newPartition.BootPartition = Convert.ToBoolean(partition.Properties["BootPartition"].Value);
            newPartition.PrimaryPartition = Convert.ToBoolean(partition.Properties["PrimaryPartition"].Value);
            newPartition.Size = Convert.ToUInt64(partition.Properties["Size"].Value);
            newPartition.Status = Convert.ToString(partition.Properties["Status"].Value);
            newPartition.Type = Convert.ToString(partition.Properties["Type"].Value);
            newPartition.PartitionIndex = (int)Convert.ToUInt32(partition.Properties["Index"].Value);
            newPartition.DriveIndex = driveIndex;

            var logicalDriveQueryText = string.Format("associators of {{{0}}} where AssocClass = Win32_LogicalDiskToPartition", partition.Path.RelativePath);
            var logicalDriveQuery = new ManagementObjectSearcher(logicalDriveQueryText);
            foreach (ManagementObject logicalDrive in logicalDriveQuery.Get())
            {
                newPartition.AddLogicalDrive(RetrieveLogicalDrives(logicalDrive));
            }

            return newPartition;
        }

        private LogicalDriveInfo RetrieveLogicalDrives(ManagementObject logicalDrive)
        {
            LogicalDriveInfo newLogicalDrive = new LogicalDriveInfo();
            
            newLogicalDrive.DriveType = Convert.ToUInt32(logicalDrive.Properties["DriveType"].Value); // C: - 3
            newLogicalDrive.FileSystem = Convert.ToString(logicalDrive.Properties["FileSystem"].Value); // NTFS
            newLogicalDrive.FreeSpace = Convert.ToUInt64(logicalDrive.Properties["FreeSpace"].Value); // in bytes
            newLogicalDrive.TotalSpace = Convert.ToUInt64(logicalDrive.Properties["Size"].Value); // in bytes
            newLogicalDrive.VolumeName = Convert.ToString(logicalDrive.Properties["VolumeName"].Value); // System
            newLogicalDrive.VolumeSerial = Convert.ToString(logicalDrive.Properties["VolumeSerialNumber"].Value); // 12345678
            newLogicalDrive.DriveLetter = Convert.ToString(logicalDrive.Properties["Name"].Value);

            newLogicalDrive.PrintToConsole();

            return newLogicalDrive;
        }

        private List<DriveInfo> SortPhysicalDrivesByDeviceID(List<DriveInfo> list)
        {
            List<DriveInfo> sortedList = list.OrderBy(o => o.DriveIndex).ToList();
            return sortedList;
        }
    }
}
