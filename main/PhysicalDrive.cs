using System;

namespace GUIForDiskpart.main
{
    public class PhysicalDrive
    {
        public string deviceID;
        public string physicalName;
        public string diskName;
        public string diskModel;
        public string mediaStatus;
        public bool mediaLoaded;
        public UInt64 totalSpace;
        public UInt32 partitions;

        public PhysicalDrive() 
        { }

        public PhysicalDrive(string deviceID, string physicalName, string diskName, string diskModel, string mediaStatus, bool mediaLoaded, ulong totalSpace, uint partitions)
        {
            this.deviceID = deviceID;
            this.physicalName = physicalName;
            this.diskName = diskName;
            this.diskModel = diskModel;
            this.mediaStatus = mediaStatus;
            this.mediaLoaded = mediaLoaded;
            this.totalSpace = totalSpace;
            this.partitions = partitions;
        }

        public string DeviceId { get; set; }
        public string PhysicalName { get; set; }

        public string DiskName { get; set; }

        public string DiskModel { get; set; }

        public string MediaStatus { get; set; }

        public bool MediaLoaded { get; set; }

        public UInt64 TotalSpace { get; set; }

        public UInt32 Partitions { get; set; }
    }
}
