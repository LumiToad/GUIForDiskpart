using System;

namespace GUIForDiskpart.main
{
    public class PartitionDrive
    {
        public string partitionName;
        public string PartitionName { get; set; }

        public bool bootable;
        public bool Bootable { get; set; }

        public bool bootPartition;
        public bool BootPartition { get; set; }

        public bool primaryPartition;
        public bool PrimaryPartition { get; set; }

        public UInt64 size;
        public UInt64 Size { get; set; }

        public string status;
        public string Status { get; set; }

        public string type;
        public string Type { get; set; }
    }
}
