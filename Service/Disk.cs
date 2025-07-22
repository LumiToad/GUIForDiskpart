using Microsoft.VisualBasic.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using DiskRetriever = GUIForDiskpart.Database.Retrievers.Disk;


namespace GUIForDiskpart.Service
{
    public static class Disk
    {
        public delegate void DDiskChange();
        public static event DDiskChange EDiskChange;

        private static List<DiskModel> physicalDisks = new List<DiskModel>();
        public static List<DiskModel> PhysicalDrives { get { return physicalDisks; } }
        private static List<ManagementObject> managementObjectDisks = new List<ManagementObject>();

        private static DiskRetriever diskRetriever = new();
        
        private const double RELOAD_COOLDOWN = 3000.0d;
        private static System.Timers.Timer reloadCDTimer;
        private static bool isReloadBlocked = false;

        public static void SetupDiskChangedWatcher() => diskRetriever.SetupDiskChangedWatcher();

        public static void ExecuteOnDiskChanged(object sender, EventArrivedEventArgs e) => EDiskChange?.Invoke();

        public static void ReLoadDisks()
        {
            if (isReloadBlocked) return;
            isReloadBlocked = true;
            
            DeleteDiskLists();
            StoreDisksInList();

            reloadCDTimer = new(RELOAD_COOLDOWN);
            reloadCDTimer.Elapsed += (sender, e) => {
                isReloadBlocked = false;
                reloadCDTimer.Stop();
            };
            reloadCDTimer.Start();
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

                physicalDisk.DiskIndex = System.Convert.ToUInt32(disk.Properties["Index"].Value);

                physicalDisk.DeviceID = System.Convert.ToString(disk.Properties["DeviceId"].Value);
                physicalDisk.PhysicalName = System.Convert.ToString(disk.Properties["Name"].Value);
                physicalDisk.Caption = System.Convert.ToString(disk.Properties["Caption"].Value);
                physicalDisk.DiskModelText = System.Convert.ToString(disk.Properties["Model"].Value);
                physicalDisk.MediaStatus = System.Convert.ToString(disk.Properties["Status"].Value);
                physicalDisk.OperationalStatusValues = diskRetriever.GetOperationalStatus(physicalDisk.DiskIndex);
                physicalDisk.MediaLoaded = System.Convert.ToBoolean(disk.Properties["MediaLoaded"].Value);
                physicalDisk.TotalSpace = System.Convert.ToUInt64(disk.Properties["Size"].Value);
                physicalDisk.InterfaceType = System.Convert.ToString(disk.Properties["InterfaceType"].Value);
                physicalDisk.MediaType = System.Convert.ToString(disk.Properties["MediaType"].Value);
                physicalDisk.MSFTMediaType = diskRetriever.GetMediaTypeValue(physicalDisk.Caption);
                physicalDisk.MediaSignature = System.Convert.ToUInt32(disk.Properties["Signature"].Value);
                physicalDisk.MediaStatus = System.Convert.ToString(disk.Properties["Status"].Value);
                physicalDisk.Availability = System.Convert.ToUInt16(disk.Properties["Availability"].Value);
                physicalDisk.BytesPerSector = System.Convert.ToUInt32(disk.Properties["BytesPerSector"].Value);
                physicalDisk.CompressionMethod = System.Convert.ToString(disk.Properties["CompressionMethod"].Value);
                physicalDisk.ConfigManagerErrorCode = System.Convert.ToUInt32(disk.Properties["ConfigManagerErrorCode"].Value);
                physicalDisk.ConfigManagerUserConfig = System.Convert.ToBoolean(disk.Properties["ConfigManagerUserConfig"].Value);
                physicalDisk.CreationClassName = System.Convert.ToString(disk.Properties["CreationClassName"].Value);
                physicalDisk.DefaultBlockSize = System.Convert.ToUInt64(disk.Properties["DefaultBlockSize"].Value);
                physicalDisk.Description = System.Convert.ToString(disk.Properties["Description"].Value);
                physicalDisk.ErrorCleared = System.Convert.ToBoolean(disk.Properties["ErrorCleared"].Value);
                physicalDisk.ErrorDescription = System.Convert.ToString(disk.Properties["ErrorDescription"].Value);
                physicalDisk.ErrorMethodology = System.Convert.ToString(disk.Properties["ErrorMethodology"].Value);
                physicalDisk.FirmwareRevision = System.Convert.ToString(disk.Properties["ErrorMethodology"].Value);
                physicalDisk.InstallDate = System.Convert.ToDateTime(disk.Properties["InstallDate"].Value);
                physicalDisk.LastErrorCode = System.Convert.ToUInt32(disk.Properties["LastErrorCode"].Value);
                physicalDisk.Manufacturer = System.Convert.ToString(disk.Properties["Manufacturer"].Value);
                physicalDisk.MaxBlockSize = System.Convert.ToUInt64(disk.Properties["MaxBlockSize"].Value);
                physicalDisk.MaxMediaSize = System.Convert.ToUInt64(disk.Properties["MaxMediaSize"].Value);
                physicalDisk.MinBlockSize = System.Convert.ToUInt64(disk.Properties["MinBlockSize"].Value);
                physicalDisk.NeedsCleaning = System.Convert.ToBoolean(disk.Properties["NeedsCleaning"].Value);
                physicalDisk.NumberOfMediaSupported = System.Convert.ToUInt32(disk.Properties["NumberOfMediaSupported"].Value);
                physicalDisk.PNPDeviceID = System.Convert.ToString(disk.Properties["PNPDeviceID"].Value);
                physicalDisk.PowerManagementSupported = System.Convert.ToBoolean(disk.Properties["PowerManagementSupported"].Value);
                physicalDisk.SCSIBus = System.Convert.ToUInt32(disk.Properties["SCSIBus"].Value);
                physicalDisk.SCSILogicalUnit = System.Convert.ToUInt16(disk.Properties["SCSILogicalUnit"].Value);
                physicalDisk.SCSIPort = System.Convert.ToUInt16(disk.Properties["SCSIPort"].Value);
                physicalDisk.SCSITargetID = System.Convert.ToUInt16(disk.Properties["SCSITargetId"].Value);
                physicalDisk.SectorsPerTrack = System.Convert.ToUInt32(disk.Properties["SectorsPerTrack"].Value);
                physicalDisk.SerialNumber = System.Convert.ToString(disk.Properties["SerialNumber"].Value);
                physicalDisk.StatusInfo = System.Convert.ToUInt16(disk.Properties["StatusInfo"].Value);
                physicalDisk.SystemCreationClassName = System.Convert.ToString(disk.Properties["SystemCreationClassName"].Value);
                physicalDisk.SystemName = System.Convert.ToString(disk.Properties["SystemName"].Value);
                physicalDisk.TotalCylinders = System.Convert.ToUInt64(disk.Properties["TotalCylinders"].Value);
                physicalDisk.TotalHeads = System.Convert.ToUInt32(disk.Properties["TotalHeads"].Value);
                physicalDisk.TotalSectors = System.Convert.ToUInt64(disk.Properties["TotalSectors"].Value);
                physicalDisk.TotalTracks = System.Convert.ToUInt64(disk.Properties["TotalTracks"].Value);
                physicalDisk.TracksPerCylinder = System.Convert.ToUInt32(disk.Properties["TracksPerCylinder"].Value);

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
