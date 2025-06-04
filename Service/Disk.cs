using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Threading.Tasks;
using DiskRetriever = GUIForDiskpart.Database.Retrievers.Disk;


namespace GUIForDiskpart.Service
{
    public static class Disk
    {
        public delegate void DiskChanged();
        public static event DiskChanged OnDiskChanged;

        private static List<DiskModel> physicalDisks = new List<DiskModel>();
        public static List<DiskModel> PhysicalDrives { get { return physicalDisks; } }
        private static List<ManagementObject> managementObjectDisks = new List<ManagementObject>();

        private static DiskRetriever diskRetriever = new();
        
        public static void ExecuteOnDiskChanged(object sender, EventArrivedEventArgs e) => OnDiskChanged();

        public static void ReLoadDisks()
        {
            DeleteDiskLists();
            StoreDisksInList();
        }

        public static void SetupDiskChangedWatcher()
        {
            diskRetriever.SetupDiskChangedWatcher();
        }

        private static void DeleteDiskLists()
        {
            physicalDisks.Clear();
            managementObjectDisks.Clear();
        }

        public static string GetDiskOutput()
        {
            string output = string.Empty;

            foreach (DiskModel physicalDisk in physicalDisks)
            {
                output += physicalDisk.GetOutputAsString();
            }

            return output;
        }

        private static void StoreDisksInList()
        {
            managementObjectDisks = diskRetriever.GetAllWMIObjects();

            foreach (ManagementObject disk in managementObjectDisks)
            {
                DiskModel physicalDisk = new DiskModel();

                physicalDisk.DiskIndex = Convert.ToUInt32(disk.Properties["Index"].Value);

                physicalDisk.DeviceID = Convert.ToString(disk.Properties["DeviceId"].Value);
                physicalDisk.PhysicalName = Convert.ToString(disk.Properties["Name"].Value);
                physicalDisk.Caption = Convert.ToString(disk.Properties["Caption"].Value);
                physicalDisk.DiskModelText = Convert.ToString(disk.Properties["Model"].Value);
                physicalDisk.MediaStatus = Convert.ToString(disk.Properties["Status"].Value);
                physicalDisk.OperationalStatusValues = diskRetriever.GetOperationalStatus(physicalDisk.DiskIndex);
                physicalDisk.MediaLoaded = Convert.ToBoolean(disk.Properties["MediaLoaded"].Value);
                physicalDisk.TotalSpace = Convert.ToUInt64(disk.Properties["Size"].Value);
                physicalDisk.InterfaceType = Convert.ToString(disk.Properties["InterfaceType"].Value);
                physicalDisk.MediaType = Convert.ToString(disk.Properties["MediaType"].Value);
                physicalDisk.MSFTMediaType = diskRetriever.GetMediaTypeValue(physicalDisk.Caption);
                physicalDisk.MediaSignature = Convert.ToUInt32(disk.Properties["Signature"].Value);
                physicalDisk.MediaStatus = Convert.ToString(disk.Properties["Status"].Value);
                physicalDisk.Availability = Convert.ToUInt16(disk.Properties["Availability"].Value);
                physicalDisk.BytesPerSector = Convert.ToUInt32(disk.Properties["BytesPerSector"].Value);
                physicalDisk.CompressionMethod = Convert.ToString(disk.Properties["CompressionMethod"].Value);
                physicalDisk.ConfigManagerErrorCode = Convert.ToUInt32(disk.Properties["ConfigManagerErrorCode"].Value);
                physicalDisk.ConfigManagerUserConfig = Convert.ToBoolean(disk.Properties["ConfigManagerUserConfig"].Value);
                physicalDisk.CreationClassName = Convert.ToString(disk.Properties["CreationClassName"].Value);
                physicalDisk.DefaultBlockSize = Convert.ToUInt64(disk.Properties["DefaultBlockSize"].Value);
                physicalDisk.Description = Convert.ToString(disk.Properties["Description"].Value);
                physicalDisk.ErrorCleared = Convert.ToBoolean(disk.Properties["ErrorCleared"].Value);
                physicalDisk.ErrorDescription = Convert.ToString(disk.Properties["ErrorDescription"].Value);
                physicalDisk.ErrorMethodology = Convert.ToString(disk.Properties["ErrorMethodology"].Value);
                physicalDisk.FirmwareRevision = Convert.ToString(disk.Properties["ErrorMethodology"].Value);
                physicalDisk.InstallDate = Convert.ToDateTime(disk.Properties["InstallDate"].Value);
                physicalDisk.LastErrorCode = Convert.ToUInt32(disk.Properties["LastErrorCode"].Value);
                physicalDisk.Manufacturer = Convert.ToString(disk.Properties["Manufacturer"].Value);
                physicalDisk.MaxBlockSize = Convert.ToUInt64(disk.Properties["MaxBlockSize"].Value);
                physicalDisk.MaxMediaSize = Convert.ToUInt64(disk.Properties["MaxMediaSize"].Value);
                physicalDisk.MinBlockSize = Convert.ToUInt64(disk.Properties["MinBlockSize"].Value);
                physicalDisk.NeedsCleaning = Convert.ToBoolean(disk.Properties["NeedsCleaning"].Value);
                physicalDisk.NumberOfMediaSupported = Convert.ToUInt32(disk.Properties["NumberOfMediaSupported"].Value);
                physicalDisk.PNPDeviceID = Convert.ToString(disk.Properties["PNPDeviceID"].Value);
                physicalDisk.PowerManagementSupported = Convert.ToBoolean(disk.Properties["PowerManagementSupported"].Value);
                physicalDisk.SCSIBus = Convert.ToUInt32(disk.Properties["SCSIBus"].Value);
                physicalDisk.SCSILogicalUnit = Convert.ToUInt16(disk.Properties["SCSILogicalUnit"].Value);
                physicalDisk.SCSIPort = Convert.ToUInt16(disk.Properties["SCSIPort"].Value);
                physicalDisk.SCSITargetID = Convert.ToUInt16(disk.Properties["SCSITargetId"].Value);
                physicalDisk.SectorsPerTrack = Convert.ToUInt32(disk.Properties["SectorsPerTrack"].Value);
                physicalDisk.SerialNumber = Convert.ToString(disk.Properties["SerialNumber"].Value);
                physicalDisk.StatusInfo = Convert.ToUInt16(disk.Properties["StatusInfo"].Value);
                physicalDisk.SystemCreationClassName = Convert.ToString(disk.Properties["SystemCreationClassName"].Value);
                physicalDisk.SystemName = Convert.ToString(disk.Properties["SystemName"].Value);
                physicalDisk.TotalCylinders = Convert.ToUInt64(disk.Properties["TotalCylinders"].Value);
                physicalDisk.TotalHeads = Convert.ToUInt32(disk.Properties["TotalHeads"].Value);
                physicalDisk.TotalSectors = Convert.ToUInt64(disk.Properties["TotalSectors"].Value);
                physicalDisk.TotalTracks = Convert.ToUInt64(disk.Properties["TotalTracks"].Value);
                physicalDisk.TracksPerCylinder = Convert.ToUInt32(disk.Properties["TracksPerCylinder"].Value);

                foreach (PartitionModel item in PartitionService.GetAllPartitions(disk, physicalDisk))
                {
                    physicalDisk.Partitions.Add(item);
                }

                physicalDisks.Add(physicalDisk);
            }

            physicalDisks = SortPhysicalDrivesByDeviceID(physicalDisks);
        }

        private static List<DiskModel> SortPhysicalDrivesByDeviceID(List<DiskModel> list)
        {
            List<DiskModel> sortedList = list.OrderBy(o => o.DiskIndex).ToList();
            return sortedList;
        }
    }
}
