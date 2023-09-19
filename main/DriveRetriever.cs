using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;


namespace GUIForDiskpart.main
{
    public class DriveRetriever
    {
        private List<PhysicalDrive> physicalDrives = new List<PhysicalDrive>();
        public List<PhysicalDrive> PhysicalDrives { get { return physicalDrives; } }

        private List<LogicalDrive> logicalDrives = new List<LogicalDrive>();
        public List<LogicalDrive> LogicalDrives { get { return logicalDrives; } }

        private List<ManagementObject> managementObjectDrives = new List<ManagementObject>();

        public void RetrieveDrives()
        {
            RetrievePhysicalDrives();
            RetrieveLogicalDrives();
        }

        public void ReloadDriveInformation()
        {
            DeleteDriveInformation();
            RetrieveDrives();
        }

        private void DeleteDriveInformation()
        {
            physicalDrives.Clear();
            logicalDrives.Clear();
            managementObjectDrives.Clear();
        }

        public string GetLogicalDrivesOutput()
        {
            string output = string.Empty;

            foreach (LogicalDrive logicalDrive in logicalDrives) 
            {
                output += logicalDrive.GetOutputAsString();
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

        private void RetrievePhysicalDrives()
        {
            ManagementObjectSearcher driveQuery = new ManagementObjectSearcher("select * from Win32_DiskDrive");
            foreach (ManagementObject drive in driveQuery.Get())
            {
                string deviceId = Convert.ToString(drive.Properties["DeviceId"].Value);
                string physicalName = Convert.ToString(drive.Properties["Name"].Value);
                string diskName = Convert.ToString(drive.Properties["Caption"].Value);
                string diskModel = Convert.ToString(drive.Properties["Model"].Value);
                string status = Convert.ToString(drive.Properties["Status"].Value);
                bool mediaLoaded = Convert.ToBoolean(drive.Properties["MediaLoaded"].Value);
                UInt64 totalSpace = Convert.ToUInt64(drive.Properties["Size"].Value);
                UInt32 partitions = Convert.ToUInt32(drive.Properties["Partitions"].Value);

                PhysicalDrive physicalDrive = new PhysicalDrive(deviceId, physicalName,
                    diskName, diskModel, status, mediaLoaded, totalSpace, partitions);
                physicalDrives.Add(physicalDrive);

                managementObjectDrives.Add(drive);
            }
        }

        private void RetrieveLogicalDrives()
        {
            foreach (ManagementObject drive in managementObjectDrives)
            {
                var partitionQueryText = string.Format("associators of {{{0}}} where AssocClass = Win32_DiskDriveToDiskPartition", drive.Path.RelativePath);
                var partitionQuery = new ManagementObjectSearcher(partitionQueryText);

                foreach (ManagementObject partition in partitionQuery.Get())
                {
                    Console.WriteLine("Partition Count: " + partitionQuery.Get().Count);
                    var logicalDriveQueryText = string.Format("associators of {{{0}}} where AssocClass = Win32_LogicalDiskToPartition", partition.Path.RelativePath);
                    var logicalDriveQuery = new ManagementObjectSearcher(logicalDriveQueryText);
                    foreach (ManagementObject logicalDrive in logicalDriveQuery.Get())
                    {
                        LogicalDrive newLogicalDrive = new LogicalDrive();

                        newLogicalDrive.PhysicalName = Convert.ToString(drive.Properties["Name"].Value); // \\.\PHYSICALDRIVE2
                        newLogicalDrive.DiskName = Convert.ToString(drive.Properties["Caption"].Value); // WDC WD5001AALS-xxxxxx
                        newLogicalDrive.DiskModel = Convert.ToString(drive.Properties["Model"].Value); // WDC WD5001AALS-xxxxxx
                        newLogicalDrive.DiskInterface = Convert.ToString(drive.Properties["InterfaceType"].Value); // IDE
                        newLogicalDrive.MediaLoaded = Convert.ToBoolean(drive.Properties["MediaLoaded"].Value); // bool
                        newLogicalDrive.MediaType = Convert.ToString(drive.Properties["MediaType"].Value); // Fixed hard disk media
                        newLogicalDrive.MediaSignature = Convert.ToUInt32(drive.Properties["Signature"].Value); // int32
                        newLogicalDrive.MediaStatus = Convert.ToString(drive.Properties["Status"].Value); // OK
                        newLogicalDrive.Partitions = Convert.ToUInt32(drive.Properties["Partitions"].Value);

                        newLogicalDrive.DriveName = Convert.ToString(logicalDrive.Properties["Name"].Value); // C:
                        newLogicalDrive.DriveId = Convert.ToString(logicalDrive.Properties["DeviceId"].Value); // C:
                        newLogicalDrive.DriveCompressed = Convert.ToBoolean(logicalDrive.Properties["Compressed"].Value);
                        newLogicalDrive.DriveType = Convert.ToUInt32(logicalDrive.Properties["DriveType"].Value); // C: - 3
                        newLogicalDrive.FileSystem = Convert.ToString(logicalDrive.Properties["FileSystem"].Value); // NTFS
                        newLogicalDrive.FreeSpace = Convert.ToUInt64(logicalDrive.Properties["FreeSpace"].Value); // in bytes
                        newLogicalDrive.TotalSpace = Convert.ToUInt64(logicalDrive.Properties["Size"].Value); // in bytes
                        newLogicalDrive.DriveMediaType = Convert.ToUInt32(logicalDrive.Properties["MediaType"].Value); // c: 12
                        newLogicalDrive.VolumeName = Convert.ToString(logicalDrive.Properties["VolumeName"].Value); // System
                        newLogicalDrive.VolumeSerial = Convert.ToString(logicalDrive.Properties["VolumeSerialNumber"].Value); // 12345678

                        newLogicalDrive.ParseDriveNumber();

                        logicalDrives.Add(newLogicalDrive);
                        newLogicalDrive.PrintToConsole();
                    }
                }
            }

            logicalDrives = SortLogicalDrivesByDriveNumber(logicalDrives);
        }

        private List<LogicalDrive> SortLogicalDrivesByDriveNumber(List<LogicalDrive> list)
        {
            List<LogicalDrive> sortedList = list.OrderBy(o=>o.DriveNumber).ToList();
            return sortedList;
        }

        private List<PhysicalDrive> SortPhysicalDrivesByDeviceID(List<PhysicalDrive> list)
        {
            List<PhysicalDrive> sortedList = list.OrderBy(o => o.deviceID).ToList();
            return sortedList;
        }
    }
}
