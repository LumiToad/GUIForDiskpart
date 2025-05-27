using System;
using System.Management;
using GUIForDiskpart.Model.Data;

namespace GUIForDiskpart.Database.Repository
{
    public static class LogicalDisk
    {
        public static void GetAndAddLogicalDisks(ManagementObject partition, WMIPartition partitionInfo)
        {
            var logicalDriveQueryText = string.Format("associators of {{{0}}} where AssocClass = Win32_LogicalDiskToPartition", partition.Path.RelativePath);
            var logicalDriveQuery = new ManagementObjectSearcher(logicalDriveQueryText);
            foreach (ManagementObject logicalDisk in logicalDriveQuery.Get())
            {
                partitionInfo.AddLogicalDisk(RetrieveLogicalDisks(logicalDisk));
            }
        }

        private static LogicalDiskInfo RetrieveLogicalDisks(ManagementObject logicalDisk)
        {
            LogicalDiskInfo newLogicalDisk = new LogicalDiskInfo();

            newLogicalDisk.DriveType = Convert.ToUInt32(logicalDisk.Properties["DriveType"].Value); // C: - 3
            newLogicalDisk.FileSystem = Convert.ToString(logicalDisk.Properties["FileSystem"].Value); // NTFS
            newLogicalDisk.FreeSpace = Convert.ToUInt64(logicalDisk.Properties["FreeSpace"].Value); // in bytes
            newLogicalDisk.TotalSpace = Convert.ToUInt64(logicalDisk.Properties["Size"].Value); // in bytes
            newLogicalDisk.VolumeName = Convert.ToString(logicalDisk.Properties["VolumeName"].Value); // System
            newLogicalDisk.VolumeSerial = Convert.ToString(logicalDisk.Properties["VolumeSerialNumber"].Value); // 12345678
            newLogicalDisk.DriveLetter = Convert.ToString(logicalDisk.Properties["Name"].Value);

            newLogicalDisk.PrintToConsole();

            return newLogicalDisk;
        }
    }
}
