using System;
using System.Collections.Generic;
using System.IO;

namespace GUIForDiskpart.main
{
    public class DriveInfo
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

        private string interfaceType;
        public string InterfaceType { get { return interfaceType; } }

        private UInt32 mediaSignature;
        public UInt32 MediaSignature { get { return mediaSignature; } }

        private string driveName;
        public string DriveName { get { return driveName; } }

        private string driveId;
        public string DriveId { get { return driveId; } }

        private bool driveCompressed;
        public bool DriveCompressed { get { return driveCompressed; } }

        private string mediaType;
        public string MediaType { get { return mediaType; } }
        
        private List<PartitionInfo> partitionDrives = new List<PartitionInfo>();
        public List<PartitionInfo> PartitionInfos { get { return partitionDrives; } }

        public DriveInfo() 
        { }

        public DriveInfo(string deviceID, string physicalName, string diskName,
            string diskModel, string mediaStatus, bool mediaLoaded, ulong totalSpace,
            uint partitionsCount, string interfaceType, string mediaType,
            UInt32 mediaSignature, string driveName)
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
            this.interfaceType = interfaceType;
            this.mediaType = mediaType;
            this.mediaSignature = mediaSignature;
            this.driveName = driveName;
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
            Console.WriteLine("InterfaceType: {0}", InterfaceType);
            Console.WriteLine("MediaType: {0}", MediaType);
            Console.WriteLine("MediaSignature: {0}", MediaSignature);
            Console.WriteLine("DriveName: {0}", DriveName);
            Console.WriteLine("DriveId: {0}", DriveId);
            Console.WriteLine("DriveCompressed: {0}", DriveCompressed);
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
            output += "InterfaceType: " + InterfaceType + "\n";
            output += "MediaType: " + MediaType + "\n";
            output += "MediaSignature: " + MediaSignature + "\n";
            output += "DriveName: " + DriveName + "\n";
            output += "DriveId: " + DriveId + "\n";
            output += "DriveCompressed: " + DriveCompressed + "\n";
            output += "_________________" + "\n";
            
            foreach (PartitionInfo partitionInfo in partitionDrives)
            {
                output += partitionInfo.GetOutputAsString();
            }
            return output;
        }

        public int ParseDriveNumber(string name)
        {
            return int.Parse(name.Substring(name.Length - 1));
        }

        public void AddPartitionDriveToList(PartitionInfo drive)
        {
            partitionDrives.Add(drive);
        }
    }
}
