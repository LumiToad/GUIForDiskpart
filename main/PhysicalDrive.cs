using System;
using System.Collections.Generic;
using System.IO;

namespace GUIForDiskpart.main
{
    public class PhysicalDrive
    {
        public string deviceID;
        public string DeviceId { get { return deviceID; } }

        public string physicalName;
        public string PhysicalName { get { return physicalName; } }

        public string diskName;
        public string DiskName { get { return diskName; } }

        public string diskModel;
        public string DiskModel { get { return diskModel; } }

        public string mediaStatus;
        public string MediaStatus { get { return mediaStatus; } }

        public bool mediaLoaded;
        public bool MediaLoaded { get { return mediaLoaded; } }

        public UInt64 totalSpace;
        public UInt64 TotalSpace { get { return totalSpace; } }

        public UInt32 partitionCount;
        public UInt32 PartitionCount { get { return partitionCount; } }

        public int driveNumber;
        public int DriveNumber { get { return driveNumber; } }

        private List<LogicalDrive> logicalDrives = new List<LogicalDrive>();
        
        private List<PartitionDrive> partitionDrives = new List<PartitionDrive>();

        public PhysicalDrive() 
        { }

        public PhysicalDrive(string deviceID, string physicalName, string diskName,
            string diskModel, string mediaStatus, bool mediaLoaded, ulong totalSpace, uint partitionsCount)
        {
            this.deviceID = deviceID;
            this.physicalName = physicalName;
            this.diskName = diskName;
            this.diskModel = diskModel;
            this.mediaStatus = mediaStatus;
            this.mediaLoaded = mediaLoaded;
            this.totalSpace = totalSpace;
            this.partitionCount = partitionsCount;
            this.driveNumber = ParseDriveNumber(physicalName);
        }

        public void PrintToConsole()
        {
            Console.WriteLine("PhysicalName: {0}", PhysicalName);
            Console.WriteLine("DriveNumber: {0}", DriveNumber);
            Console.WriteLine("DiskName: {0}", DiskName);
            Console.WriteLine("DiskModel: {0}", DiskModel);
            Console.WriteLine("MediaLoaded: {0}", MediaLoaded);
            Console.WriteLine("MediaStatus: {0}", MediaStatus);
            Console.WriteLine("TotalSpace: {0}", TotalSpace);

            Console.WriteLine(new string('-', 79));
        }

        public string GetOutputAsString()
        {
            string output = string.Empty;
            output += "PhysicalName: " + PhysicalName + "\n";
            output += "DriveNumber: " + DriveNumber + "\n";
            output += "DiskName: " + DiskName + "\n";
            output += "DiskModel: " + DiskModel + "\n";
            output += "MediaLoaded: " + MediaLoaded + "\n";
            output += "MediaStatus: " + MediaStatus + "\n";
            output += "TotalSpace: " + TotalSpace + "\n";
            output += "_________________" + "\n";

            return output;
        }

        public int ParseDriveNumber(string name)
        {
            return int.Parse(name.Substring(name.Length - 1));
        }

        public void AddLogicalDriveToList(LogicalDrive drive)
        {
            logicalDrives.Add(drive);
        }

        public void AddPartitionDriveToList(PartitionDrive drive)
        {
            partitionDrives.Add(drive);
        }
    }
}
