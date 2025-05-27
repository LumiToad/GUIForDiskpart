using System;
using System.Management;

using LDRetriever = GUIForDiskpart.Database.Retrievers.LogicalDisk;

namespace GUIForDiskpart.Service
{
    public static class LogicalDisk
    {
        private static LDRetriever ldRetriever = new();

        public static LDModel GetLogicalDisk(ManagementObject partition)
        {
            ManagementObject? logicalDisk = ldRetriever.LDQuery(partition);

            LDModel newLogicalDisk = new LDModel();

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
