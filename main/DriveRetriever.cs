using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Windows.Documents;


namespace GUIForDiskpart.main
{
    public class DriveRetriever
    {
        private List<PhysicalDrive> physicalDrives = new List<PhysicalDrive>();
        public List<PhysicalDrive> PhysicalDrives { get { return physicalDrives; } }

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

            foreach (PhysicalDrive physicalDrive in physicalDrives) 
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

                PhysicalDrive physicalDrive = new PhysicalDrive(deviceId, physicalName,
                    diskName, diskModel, status, mediaLoaded, totalSpace, partitionCount);

                var partitionQueryText = string.Format("associators of {{{0}}} where AssocClass = Win32_DiskDriveToDiskPartition", drive.Path.RelativePath);
                var partitionQuery = new ManagementObjectSearcher(partitionQueryText);

                foreach (ManagementObject partition in partitionQuery.Get())
                {
                    physicalDrive.AddPartitionDriveToList(RetrievePartitions(partition));

                    var logicalDriveQueryText = string.Format("associators of {{{0}}} where AssocClass = Win32_LogicalDiskToPartition", partition.Path.RelativePath);
                    var logicalDriveQuery = new ManagementObjectSearcher(logicalDriveQueryText);
                    foreach (ManagementObject logicalDrive in logicalDriveQuery.Get())
                    {
                        physicalDrive.AddLogicalDriveToList(RetrieveLogicalDrives(logicalDrive));
                    }
                }
                
                physicalDrives.Add(physicalDrive);
            }

            physicalDrives = SortPhysicalDrivesByDeviceID(physicalDrives);
        }

        private PartitionDrive RetrievePartitions(ManagementObject drive)
        {
            PartitionDrive newPartition = new PartitionDrive();
            
            newPartition.PartitionName = Convert.ToString(drive.Properties["Name"].Value);
            newPartition.Bootable = Convert.ToBoolean(drive.Properties["Bootable"].Value);
            newPartition.BootPartition = Convert.ToBoolean(drive.Properties["BootPartition"].Value);
            newPartition.PrimaryPartition = Convert.ToBoolean(drive.Properties["PrimaryPartition"].Value);
            newPartition.Size = Convert.ToUInt64(drive.Properties["Size"].Value);
            newPartition.Status = Convert.ToString(drive.Properties["Status"].Value);
            newPartition.Type = Convert.ToString(drive.Properties["Type"].Value);
            
            return newPartition;
        }

        private LogicalDrive RetrieveLogicalDrives(ManagementObject logicalDrive)
        {
            LogicalDrive newLogicalDrive = new LogicalDrive();
            
            //newLogicalDrive.DiskInterface = Convert.ToString(logicalDrive.Properties["InterfaceType"].Value); // IDE
            newLogicalDrive.MediaType = Convert.ToString(logicalDrive.Properties["MediaType"].Value); // Fixed hard disk media
            //newLogicalDrive.MediaSignature = Convert.ToUInt32(logicalDrive.Properties["Signature"].Value); // int32
            //newLogicalDrive.MediaStatus = Convert.ToString(logicalDrive.Properties["Status"].Value); // OK

            //newLogicalDrive.DriveName = Convert.ToString(logicalDrive.Properties["Name"].Value); // C:
            //newLogicalDrive.DriveId = Convert.ToString(logicalDrive.Properties["DeviceId"].Value); // C:
            //newLogicalDrive.DriveCompressed = Convert.ToBoolean(logicalDrive.Properties["Compressed"].Value);
            newLogicalDrive.DriveType = Convert.ToUInt32(logicalDrive.Properties["DriveType"].Value); // C: - 3
            newLogicalDrive.FileSystem = Convert.ToString(logicalDrive.Properties["FileSystem"].Value); // NTFS
            newLogicalDrive.FreeSpace = Convert.ToUInt64(logicalDrive.Properties["FreeSpace"].Value); // in bytes
            newLogicalDrive.TotalSpace = Convert.ToUInt64(logicalDrive.Properties["Size"].Value); // in bytes
            //newLogicalDrive.DriveMediaType = Convert.ToUInt32(logicalDrive.Properties["MediaType"].Value); // c: 12
            newLogicalDrive.VolumeName = Convert.ToString(logicalDrive.Properties["VolumeName"].Value); // System
            newLogicalDrive.VolumeSerial = Convert.ToString(logicalDrive.Properties["VolumeSerialNumber"].Value); // 12345678
            
            newLogicalDrive.PrintToConsole();

            return newLogicalDrive;
        }

        private List<LogicalDrive> SortLogicalDrivesByDriveNumber(List<LogicalDrive> list)
        {
            List<LogicalDrive> sortedList = list.OrderBy(o=>o.DriveId).ToList();
            return sortedList;
        }

        private List<PhysicalDrive> SortPhysicalDrivesByDeviceID(List<PhysicalDrive> list)
        {
            List<PhysicalDrive> sortedList = list.OrderBy(o => o.DriveNumber).ToList();
            return sortedList;
        }
    }
}
