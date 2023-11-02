using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;


namespace GUIForDiskpart.main
{
    public static class DiskRetriever
    {
        private static List<DiskInfo> physicalDisks = new List<DiskInfo>();
        public static List<DiskInfo> PhysicalDrives { get { return physicalDisks; } }

        private static List<ManagementObject> managementObjectDisks = new List<ManagementObject>();

        public static void RetrieveDisks()
        {
            RetrieveWMIObjectsToList();
            RetrieveDisksToList();
        }

        public static void ReloadDriveInformation()
        {
            DeleteDiskInformation();
            RetrieveDisks();
        }

        private static void DeleteDiskInformation()
        {
            physicalDisks.Clear();
            managementObjectDisks.Clear();
        }

        public static string GetDrivesOutput()
        {
            string output = string.Empty;

            foreach (DiskInfo physicalDisk in physicalDisks) 
            {
                output += physicalDisk.GetOutputAsString();
            }

            return output;
        }

        private static void RetrieveWMIObjectsToList()
        {
            ManagementObjectSearcher diskDriveQuery = new ManagementObjectSearcher("select * from Win32_DiskDrive");

            foreach (ManagementObject disk in diskDriveQuery.Get())
            {
                managementObjectDisks.Add(disk);
            }
        }

        private static void RetrieveDisksToList()
        {
            foreach (ManagementObject disk in managementObjectDisks)
            {
                string deviceId = Convert.ToString(disk.Properties["DeviceId"].Value);
                string physicalName = Convert.ToString(disk.Properties["Name"].Value);
                string caption = Convert.ToString(disk.Properties["Caption"].Value);
                string diskModel = Convert.ToString(disk.Properties["Model"].Value);
                string status = Convert.ToString(disk.Properties["Status"].Value);
                bool mediaLoaded = Convert.ToBoolean(disk.Properties["MediaLoaded"].Value);
                UInt64 totalSpace = Convert.ToUInt64(disk.Properties["Size"].Value);
                UInt32 partitionCount = Convert.ToUInt32(disk.Properties["Partitions"].Value);
                string interfaceType = Convert.ToString(disk.Properties["InterfaceType"].Value);
                string mediaType = Convert.ToString(disk.Properties["MediaType"].Value);
                UInt32 mediaSignature = Convert.ToUInt32(disk.Properties["Signature"].Value);
                string mediaStatus = Convert.ToString(disk.Properties["Status"].Value);
                string name = Convert.ToString(disk.Properties["Name"].Value);
                UInt16 availability = Convert.ToUInt16(disk.Properties["Availability"].Value);
                UInt32 bytesPerSector = Convert.ToUInt32(disk.Properties["BytesPerSector"].Value);
                //UInt16 Capabilities[];
                //string CapabilityDescriptions[];
                string compressionMethod = Convert.ToString(disk.Properties["CompressionMethod"].Value);
                UInt32 configManagerErrorCode = Convert.ToUInt32(disk.Properties["ConfigManagerErrorCode"].Value);
                bool configManagerUserConfig = Convert.ToBoolean(disk.Properties["ConfigManagerUserConfig"].Value);
                string creationClassName = Convert.ToString(disk.Properties["CreationClassName"].Value);
                UInt64 defaultBlockSize = Convert.ToUInt64(disk.Properties["DefaultBlockSize"].Value);
                string description = Convert.ToString(disk.Properties["Description"].Value);
                bool errorCleared = Convert.ToBoolean(disk.Properties["ErrorCleared"].Value);
                string errorDescription = Convert.ToString(disk.Properties["ErrorDescription"].Value);
                string errorMethodology = Convert.ToString(disk.Properties["ErrorMethodology"].Value);
                string firmwareRevision = Convert.ToString(disk.Properties["ErrorMethodology"].Value);
                UInt32 index = Convert.ToUInt32(disk.Properties["Index"].Value);
                DateTime installDate = Convert.ToDateTime(disk.Properties["InstallDate"].Value);
                UInt32 lastErrorCode = Convert.ToUInt32(disk.Properties["LastErrorCode"].Value);
                string manufacturer = Convert.ToString(disk.Properties["Manufacturer"].Value);
                UInt64 maxBlockSize = Convert.ToUInt64(disk.Properties["MaxBlockSize"].Value);
                UInt64 maxMediaSize = Convert.ToUInt64(disk.Properties["MaxMediaSize"].Value); 
                UInt64 minBlockSize = Convert.ToUInt64(disk.Properties["MinBlockSize"].Value);
                bool needsCleaning = Convert.ToBoolean(disk.Properties["NeedsCleaning"].Value);
                UInt32 numberOfMediaSupported = Convert.ToUInt32(disk.Properties["NumberOfMediaSupported"].Value);
                string pnpDeviceID = Convert.ToString(disk.Properties["PNPDeviceID"].Value);
                //UInt16 PowerManagementCapabilities[];
                bool powerManagementSupported = Convert.ToBoolean(disk.Properties["PowerManagementSupported"].Value);
                UInt32 scsiBus = Convert.ToUInt32(disk.Properties["SCSIBus"].Value);
                UInt16 scsiLogicalUnit = Convert.ToUInt16(disk.Properties["SCSILogicalUnit"].Value);
                UInt16 scsiPort = Convert.ToUInt16(disk.Properties["SCSIPort"].Value);
                UInt16 scsiTargetId = Convert.ToUInt16(disk.Properties["SCSITargetId"].Value);
                UInt32 sectorsPerTrack = Convert.ToUInt32(disk.Properties["SectorsPerTrack"].Value);
                string serialNumber = Convert.ToString(disk.Properties["SerialNumber"].Value);
                UInt16 statusInfo = Convert.ToUInt16(disk.Properties["StatusInfo"].Value);
                string systemCreationClassName = Convert.ToString(disk.Properties["SystemCreationClassName"].Value);
                string systemName = Convert.ToString(disk.Properties["SystemName"].Value);
                UInt64 totalCylinders = Convert.ToUInt64(disk.Properties["TotalCylinders"].Value);
                UInt32 totalHeads = Convert.ToUInt32(disk.Properties["TotalHeads"].Value);
                UInt64 totalSectors = Convert.ToUInt64(disk.Properties["TotalSectors"].Value);
                UInt64 totalTracks = Convert.ToUInt64(disk.Properties["TotalTracks"].Value);
                UInt32 tracksPerCylinder = Convert.ToUInt32(disk.Properties["TracksPerCylinder"].Value);

                DiskInfo physicalDisk = new DiskInfo(deviceId, physicalName, caption, diskModel, mediaStatus, mediaLoaded, totalSpace, partitionCount,
                    interfaceType, mediaSignature, name, mediaType, availability, bytesPerSector, compressionMethod, configManagerErrorCode,
                    configManagerUserConfig, creationClassName, defaultBlockSize, description, errorCleared, errorDescription, errorMethodology,
                    firmwareRevision, index, installDate, lastErrorCode, manufacturer, maxBlockSize, maxMediaSize, minBlockSize, needsCleaning,
                    numberOfMediaSupported, pnpDeviceID, powerManagementSupported, scsiBus, scsiLogicalUnit, scsiPort, scsiTargetId, sectorsPerTrack,
                    serialNumber, statusInfo, systemCreationClassName, systemName, totalCylinders, totalHeads, totalSectors, totalTracks, tracksPerCylinder);

                PartitionRetriever.GetAndAddWSMPartitionToDisk(physicalDisk);
                PartitionRetriever.GetAndAddWMIPartitionsToDisk(disk, physicalDisk);
                Console.WriteLine(physicalDisk.WMIPartitions.Count);
                Console.WriteLine(physicalDisk.WSMPartitions.Count);

                physicalDisk.UnpartSpace = physicalDisk.CalcUnpartSpace(physicalDisk.TotalSpace);

                physicalDisks.Add(physicalDisk);
            }

            physicalDisks = SortPhysicalDrivesByDeviceID(physicalDisks);
        }

        private static List<DiskInfo> SortPhysicalDrivesByDeviceID(List<DiskInfo> list)
        {
            List<DiskInfo> sortedList = list.OrderBy(o => o.DiskIndex).ToList();
            return sortedList;
        }
    }
}
