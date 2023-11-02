using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace GUIForDiskpart.main
{
    public static class LogicalDiskRetriever
    {
        public static void GetAndAddLogicalDisks(ManagementObject partition, PartitionInfo partitionInfo)
        {
            var logicalDriveQueryText = string.Format("associators of {{{0}}} where AssocClass = Win32_LogicalDiskToPartition", partition.Path.RelativePath);
            var logicalDriveQuery = new ManagementObjectSearcher(logicalDriveQueryText);
            foreach (ManagementObject logicalDrive in logicalDriveQuery.Get())
            {
                partitionInfo.AddLogicalDrive(RetrieveLogicalDrives(logicalDrive));
            }
        }

        private static LogicalDriveInfo RetrieveLogicalDrives(ManagementObject logicalDrive)
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
    }
}
