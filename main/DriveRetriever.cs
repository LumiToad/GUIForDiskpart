using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;

namespace GUIForDiskpart.main
{
    public class DriveRetriever
    {
        private List<DriveInfo> physicalDrives = new List<DriveInfo>();
        public List<DriveInfo> PhysicalDrives { get { return physicalDrives; } }

        private List<ManagementObject> managementObjectDrives = new List<ManagementObject>();

        public void Initialize()
        {
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
                string caption = Convert.ToString(drive.Properties["Caption"].Value);
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

                DriveInfo physicalDrive = new DriveInfo(deviceId, physicalName, caption, diskModel, mediaStatus, mediaLoaded, totalSpace, partitionCount,
                    interfaceType, mediaSignature, driveName, mediaType, availability, bytesPerSector, compressionMethod, configManagerErrorCode,
                    configManagerUserConfig, creationClassName, defaultBlockSize, description, errorCleared, errorDescription, errorMethodology,
                    firmwareRevision, index, installDate, lastErrorCode, manufacturer, maxBlockSize, maxMediaSize, minBlockSize, needsCleaning,
                    numberOfMediaSupported, pnpDeviceID, powerManagementSupported, scsiBus, scsiLogicalUnit, scsiPort, scsiTargetId, sectorsPerTrack,
                    serialNumber, statusInfo, systemCreationClassName, systemName, totalCylinders, totalHeads, totalSectors, totalTracks, tracksPerCylinder);

                //var partitionQueryText = string.Format("associators of {{{0}}} where AssocClass = Win32_DiskDriveToDiskPartition", drive.Path.RelativePath);

                var partitionQueryText = "SELECT * from Win32_DiskPartition";
                var partitionQuery = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM CIM_DiskPartition");
                //var partitionQuery = new ManagementObjectSearcher(partitionQueryText);

                foreach (ManagementObject partition in partitionQuery.Get())
                {
                    physicalDrive.AddPartitionToList(RetrievePartitions(partition, physicalDrive.DiskIndex));
                }

                physicalDrive.UnpartSpace = physicalDrive.CalcUnpartSpace(physicalDrive.TotalSpace);

                physicalDrives.Add(physicalDrive);
            }

            physicalDrives = SortPhysicalDrivesByDeviceID(physicalDrives);
        }

        private PartitionInfo RetrievePartitions(ManagementObject partition, uint diskIndex)
        {
            PartitionInfo newPartition = new PartitionInfo();
            
            newPartition.Availability = Convert.ToUInt16(partition.Properties["Availability"].Value);
            newPartition.StatusInfo = Convert.ToUInt16(partition.Properties["StatusInfo"].Value);
            newPartition.BlockSize = Convert.ToUInt64(partition.Properties["BlockSize"].Value);
            newPartition.Bootable = Convert.ToBoolean(partition.Properties["Bootable"].Value);
            newPartition.BootPartition = Convert.ToBoolean(partition.Properties["BootPartition"].Value);
            newPartition.Caption = Convert.ToString(partition.Properties["Caption"].Value);

            newPartition.ConfigManagerErrorCode = Convert.ToUInt32(partition.Properties["ConfigManagerErrorCode"].Value);
            newPartition.ConfigManagerUserConfig = Convert.ToBoolean(partition.Properties["ConfigManagerUserConfig"].Value);
            newPartition.CreationClassName = Convert.ToString(partition.Properties["CreationClassName"].Value);
            newPartition.Description = Convert.ToString(partition.Properties["Description"].Value);
            newPartition.DeviceID = Convert.ToString(partition.Properties["DeviceID"].Value);
            newPartition.DiskIndex = Convert.ToUInt32(partition.Properties["DiskIndex"].Value);
            
            newPartition.ErrorCleared = Convert.ToBoolean(partition.Properties["ErrorCleared"].Value);
            newPartition.ErrorDescription = Convert.ToString(partition.Properties["ErrorDescription"].Value);
            newPartition.ErrorMethodology = Convert.ToString(partition.Properties["ErrorMethodology"].Value);

            newPartition.HiddenSectors = Convert.ToUInt32(partition.Properties["ConfigManagerErrorCode"].Value);
            newPartition.PartitionIndex = Convert.ToUInt32(partition.Properties["Index"].Value);

            newPartition.InstallDate = Convert.ToDateTime(partition.Properties["InstallDate"].Value);
            newPartition.LastErrorCode = Convert.ToUInt32(partition.Properties["LastErrorCode"].Value);
            newPartition.PartitionName = Convert.ToString(partition.Properties["Name"].Value);
            newPartition.NumberOfBlocks = Convert.ToUInt64(partition.Properties["NumberOfBlocks"].Value);

            newPartition.PNPDeviceID = Convert.ToString(partition.Properties["PNPDeviceID"].Value);
            newPartition.PowerManagementSupported = Convert.ToBoolean(partition.Properties["PowerManagementSupported"].Value);
            newPartition.PrimaryPartition = Convert.ToBoolean(partition.Properties["PrimaryPartition"].Value);
            newPartition.Purpose = Convert.ToString(partition.Properties["Purpose"].Value);
            newPartition.RewritePartition = Convert.ToBoolean(partition.Properties["RewritePartition"].Value);
            newPartition.Size = Convert.ToUInt64(partition.Properties["Size"].Value);
            newPartition.StartingOffset = Convert.ToUInt64(partition.Properties["StartingOffset"].Value);
            newPartition.Status = Convert.ToString(partition.Properties["Status"].Value);
            newPartition.SystemCreationClassName = Convert.ToString(partition.Properties["SystemCreationClassName"].Value);
            newPartition.SystemName = Convert.ToString(partition.Properties["SystemName"].Value);

            newPartition.Type = Convert.ToString(partition.Properties["Type"].Value);

            Console.WriteLine($"Function diskIndex: {diskIndex}, retrieved diskIndex: {newPartition.DiskIndex}, retrieved partIndex: {newPartition.PartitionIndex}, Size: {newPartition.Size}, " +
                $"\nstartingOffset: {newPartition.StartingOffset}, systemCreationClassName: {newPartition.SystemCreationClassName}, purpose: {newPartition.Purpose}");

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
            List<DriveInfo> sortedList = list.OrderBy(o => o.DiskIndex).ToList();
            return sortedList;
        }
    }
}
