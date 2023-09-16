using System;
using System.Collections.Generic;
using System.Management;


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

                PhysicalDrive physicalDrive = new PhysicalDrive(deviceId, physicalName,
                    diskName, diskModel, status, mediaLoaded, totalSpace);
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
                    var logicalDriveQueryText = string.Format("associators of {{{0}}} where AssocClass = Win32_LogicalDiskToPartition", partition.Path.RelativePath);
                    var logicalDriveQuery = new ManagementObjectSearcher(logicalDriveQueryText);
                    foreach (ManagementObject logicalDrive in logicalDriveQuery.Get())
                    {
                        LogicalDrive newDrive = new LogicalDrive();

                        newDrive.PhysicalName = Convert.ToString(drive.Properties["Name"].Value); // \\.\PHYSICALDRIVE2
                        newDrive.DiskName = Convert.ToString(drive.Properties["Caption"].Value); // WDC WD5001AALS-xxxxxx
                        newDrive.DiskModel = Convert.ToString(drive.Properties["Model"].Value); // WDC WD5001AALS-xxxxxx
                        newDrive.DiskInterface = Convert.ToString(drive.Properties["InterfaceType"].Value); // IDE
                        newDrive.MediaLoaded = Convert.ToBoolean(drive.Properties["MediaLoaded"].Value); // bool
                        newDrive.MediaType = Convert.ToString(drive.Properties["MediaType"].Value); // Fixed hard disk media
                        newDrive.MediaSignature = Convert.ToUInt32(drive.Properties["Signature"].Value); // int32
                        newDrive.MediaStatus = Convert.ToString(drive.Properties["Status"].Value); // OK

                        newDrive.DriveName = Convert.ToString(logicalDrive.Properties["Name"].Value); // C:
                        newDrive.DriveId = Convert.ToString(logicalDrive.Properties["DeviceId"].Value); // C:
                        newDrive.DriveCompressed = Convert.ToBoolean(logicalDrive.Properties["Compressed"].Value);
                        newDrive.DriveType = Convert.ToUInt32(logicalDrive.Properties["DriveType"].Value); // C: - 3
                        newDrive.FileSystem = Convert.ToString(logicalDrive.Properties["FileSystem"].Value); // NTFS
                        newDrive.FreeSpace = Convert.ToUInt64(logicalDrive.Properties["FreeSpace"].Value); // in bytes
                        newDrive.TotalSpace = Convert.ToUInt64(logicalDrive.Properties["Size"].Value); // in bytes
                        newDrive.DriveMediaType = Convert.ToUInt32(logicalDrive.Properties["MediaType"].Value); // c: 12
                        newDrive.VolumeName = Convert.ToString(logicalDrive.Properties["VolumeName"].Value); // System
                        newDrive.VolumeSerial = Convert.ToString(logicalDrive.Properties["VolumeSerialNumber"].Value); // 12345678

                        newDrive.ParseDriveNumber();

                        logicalDrives.Add(newDrive);
                        newDrive.PrintToConsole();
                    }
                }
            }
        }
    }
}
