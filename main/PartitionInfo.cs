using System;
using System.Windows.Controls;

namespace GUIForDiskpart.main
{
    public class PartitionInfo
    {
        public string partitionName;
        public string PartitionName 
        { 
            get {  return partitionName; }
            set 
            {
                partitionName = value;
                PartitionIndex = ParsePartitionNumber(partitionName);
            } 
        }

        public int partitionIndex;
        public int PartitionIndex { get; set; }

        private uint driveIndex;
        public uint DriveIndex { get; set; }

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

        public LogicalDriveInfo logicalDriveInfo;
        public LogicalDriveInfo LogicalDriveInfo { get { return logicalDriveInfo; } }

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

            if (logicalDriveInfo != null) 
            { 
                output += logicalDriveInfo.GetOutputAsString();
            }

            output += "\t_________________" + "\n";
            
            return output;
        }

        public void AddLogicalDrive(LogicalDriveInfo drive)
        {
            logicalDriveInfo = drive;
        }

        public bool IsLogicalPartition()
        {  return logicalDriveInfo != null; }

        public int ParsePartitionNumber(string name)
        {
            int spacerIndex = 0;

            for (int i = name.Length -1; i != 0; i-- )
            {
                if (name[i] == ' ')
                {
                    spacerIndex = i;
                    break;
                }
            }

            return int.Parse(name.Substring(spacerIndex));
        }
    }
}
