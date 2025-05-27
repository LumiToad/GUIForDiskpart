using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Management.Automation;
using GUIForDiskpart.Model.Logic;

namespace GUIForDiskpart.service
{
    public static class Disk
    {
        public delegate void DiskChanged();
        public static event DiskChanged OnDiskChanged;

        private static List<DiskInfo> physicalDisks = new List<DiskInfo>();
        public static List<DiskInfo> PhysicalDrives { get { return physicalDisks; } }

        private static List<ManagementObject> managementObjectDisks = new List<ManagementObject>();

        public static void RetrieveDisks()
        {
            RetrieveWMIObjectsToList();
            RetrieveDisksToList();
        }

        public static void SetupDiskChangedWatcher()
        {
            try
            {
                WqlEventQuery query = new WqlEventQuery("SELECT * FROM Win32_VolumeChangeEvent WHERE EventType = 2 or EventType = 3");
                ManagementEventWatcher watcher = new ManagementEventWatcher();
                watcher.Query = query;
                watcher.EventArrived += OnDiskChangedInternal;
                watcher.Options.Timeout = TimeSpan.FromSeconds(3);
                watcher.Start();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        private static void OnDiskChangedInternal(object sender, EventArrivedEventArgs e)
        {
            OnDiskChanged();
        }

        public static void ReloadDiskInformation()
        {
            DeleteDiskInformation();
            RetrieveDisks();
        }

        private static void DeleteDiskInformation()
        {
            physicalDisks.Clear();
            managementObjectDisks.Clear();
        }

        public static string GetDiskOutput()
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
                DiskInfo physicalDisk = new DiskInfo();

                physicalDisk.DiskIndex = Convert.ToUInt32(disk.Properties["Index"].Value);

                physicalDisk.DeviceID = Convert.ToString(disk.Properties["DeviceId"].Value);
                physicalDisk.PhysicalName = Convert.ToString(disk.Properties["Name"].Value);
                physicalDisk.Caption = Convert.ToString(disk.Properties["Caption"].Value);
                physicalDisk.DiskModel = Convert.ToString(disk.Properties["Model"].Value);
                physicalDisk.MediaStatus = Convert.ToString(disk.Properties["Status"].Value);
                physicalDisk.OperationalStatusValues = GetOperationalStatus(physicalDisk.DiskIndex);
                physicalDisk.MediaLoaded = Convert.ToBoolean(disk.Properties["MediaLoaded"].Value);
                physicalDisk.TotalSpace = Convert.ToUInt64(disk.Properties["Size"].Value);
                physicalDisk.InterfaceType = Convert.ToString(disk.Properties["InterfaceType"].Value);
                physicalDisk.MediaType = Convert.ToString(disk.Properties["MediaType"].Value);
                physicalDisk.MSFTMediaType = GetMediaTypeValue(physicalDisk.Caption);
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



                PartitionRetriever.GetPartitionsAndAddToDisk(disk, physicalDisk);

                physicalDisks.Add(physicalDisk);
            }

            physicalDisks = SortPhysicalDrivesByDeviceID(physicalDisks);
        }

        private static ushort? GetMediaTypeValue(string friendlyName)
        {
            if (string.IsNullOrEmpty(friendlyName)) return null;

            ushort? result = null;

            foreach (var name in friendlyName.Split(new[] { ' ', '-', '_', ':' }))
            {
                string[] commands = new string[2];
                commands[0] += $"$Query = Get-CimInstance -Query \"select * from MSFT_PhysicalDisk WHERE FriendlyName Like '%{name}%'\" -Namespace root\\Microsoft\\Windows\\Storage";
                commands[1] += $"$Query.MediaType";
                List<PSObject> psObjects = CommandExecuter.IssuePowershellCommand(commands);
                PSObject? data = psObjects[0];

                if (data == null) continue;
                result = (ushort)data.BaseObject;
            }
            return result;
        }

        private static ushort[] GetOperationalStatus(uint diskIndex)
        {
            ManagementScope scope = new ManagementScope(@"root\Microsoft\Windows\Storage");
            SelectQuery query = new SelectQuery($"select * from MSFT_Disk WHERE Number={diskIndex}");
            ManagementObjectSearcher msftDiskQuery = new ManagementObjectSearcher(scope, query);
            foreach (ManagementObject msftDisk in msftDiskQuery.Get())
            {
                return (ushort[])msftDisk.Properties["OperationalStatus"].Value;
            }
            return new ushort[1] { 0 };
        }

        private static List<DiskInfo> SortPhysicalDrivesByDeviceID(List<DiskInfo> list)
        {
            List<DiskInfo> sortedList = list.OrderBy(o => o.DiskIndex).ToList();
            return sortedList;
        }
    }
}
