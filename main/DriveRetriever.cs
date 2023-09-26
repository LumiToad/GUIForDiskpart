using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Windows.Documents;


namespace GUIForDiskpart.main
{
    public class DriveRetriever
    {
        private List<DriveInfo> physicalDrives = new List<DriveInfo>();
        public List<DriveInfo> PhysicalDrives { get { return physicalDrives; } }

        private List<ManagementObject> managementObjectDrives = new List<ManagementObject>();

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
            ManagementEventWatcher watcher = new ManagementEventWatcher();
            WqlEventQuery query = new WqlEventQuery("SELECT * FROM Win32_VolumeChangeEvent WHERE EventType = 2");
            //watcher.EventArrived += new EventArrivedEventHandler();
            watcher.Query = query;
            watcher.Start();
            watcher.WaitForNextEvent();
        }
        private void OnDriveChanged()
        {

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

                DriveInfo physicalDrive = new DriveInfo(deviceId, physicalName,
                    diskName, diskModel, status, mediaLoaded, totalSpace, partitionCount,
                    interfaceType, mediaType, mediaSignature, mediaStatus);

                var partitionQueryText = string.Format("associators of {{{0}}} where AssocClass = Win32_DiskDriveToDiskPartition", drive.Path.RelativePath);
                var partitionQuery = new ManagementObjectSearcher(partitionQueryText);

                foreach (ManagementObject partition in partitionQuery.Get())
                {
                    physicalDrive.AddPartitionDriveToList(RetrievePartitions(partition));
                }

                physicalDrive.UnpartSpace = physicalDrive.CalcUnpartSpace(physicalDrive.TotalSpace);

                physicalDrives.Add(physicalDrive);
            }

            physicalDrives = SortPhysicalDrivesByDeviceID(physicalDrives);
        }

        private PartitionInfo RetrievePartitions(ManagementObject partition)
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
