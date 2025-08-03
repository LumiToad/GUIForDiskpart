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

            if (logicalDisk == null) return newLogicalDisk;

            newLogicalDisk.DriveType = System.Convert.ToUInt32(logicalDisk.Properties["DriveType"].Value); // C: - 3
            newLogicalDisk.FileSystem = System.Convert.ToString(logicalDisk.Properties["FileSystem"].Value); // NTFS
            newLogicalDisk.FreeSpace = System.Convert.ToUInt64(logicalDisk.Properties["FreeSpace"].Value); // in bytes
            newLogicalDisk.TotalSpace = System.Convert.ToUInt64(logicalDisk.Properties["Size"].Value); // in bytes
            newLogicalDisk.VolumeName = System.Convert.ToString(logicalDisk.Properties["VolumeName"].Value); // System
            newLogicalDisk.VolumeSerial = System.Convert.ToString(logicalDisk.Properties["VolumeSerialNumber"].Value); // 12345678
            newLogicalDisk.DriveLetter = System.Convert.ToString(logicalDisk.Properties["Name"].Value);

            return newLogicalDisk;
        }
    }
}
