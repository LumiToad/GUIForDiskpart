using System;
using System.Windows.Controls;

namespace GUIForDiskpart.main
{
    public class PartitionInfo
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

        public void PrintToConsole()
        {
            Console.WriteLine("PartitionName: {0}", PartitionName);
            Console.WriteLine("Bootable: {0}", Bootable);
            Console.WriteLine("BootPartition: {0}", BootPartition);
            Console.WriteLine("PrimaryPartition: {0}", PrimaryPartition);
            Console.WriteLine("TotalSize: {0}", Size);
            Console.WriteLine("MediaStatus: {0}", Status);
            Console.WriteLine("MediaType: {0}", Type);
            Console.WriteLine(new string('-', 79));
        }

        public string GetOutputAsString()
        {
            string output = string.Empty;
            output += "\tPartitionName: {0}" + PartitionName + "\n";
            output += "\tBootable: {0}" + Bootable + "\n";
            output += "\tBootPartition: " + BootPartition + "\n";
            output += "\tPrimaryPartition: " + PrimaryPartition + "\n";
            output += "\tTotalSize: " + Size + "\n";
            output += "\tMediaStatus: " + Status + "\n";
            output += "\tMediaType: " + Type + "\n";
            output += "\t_________________" + "\n";

            return output;
        }
    }
}
