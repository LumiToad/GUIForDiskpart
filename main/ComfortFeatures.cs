using GUIForDiskpart.diskpart;
using System;

namespace GUIForDiskpart.main
{
    public static class ComfortFeatures
    {
        private const string outputConsoleAppliationName = "GUIFD - ";

        public static string EasyDiskFormat(DiskInfo disk, FileSystem fileSystem,
            string volumeName, char driveLetter, UInt64 sizeInMB, bool isQuickFormatting,
            bool isCompressed, bool isOverride, bool isNoWait, bool isNoErr)
        {
            string output = string.Empty;

            output += outputConsoleAppliationName + "Easy disk format\n";

            output += DPFunctions.Clean(disk.DiskIndex, false);
            output += DPFunctions.Convert(disk.DiskIndex, ConvertOptions.GPT);
            output += DPFunctions.Delete(disk.DiskIndex, 1, false, true);

            output += DPFunctions.CreatePartition(disk.DiskIndex, CreatePartitionOptions.PRIMARY, sizeInMB, isNoErr);
            output += DPFunctions.Format(disk.DiskIndex, 1, fileSystem, volumeName, isQuickFormatting, isCompressed, isOverride, isNoWait, isNoErr);
            output += DPFunctions.Assign(disk.DiskIndex, 1, driveLetter, isNoErr);
            
            return output;
        }

        public static string EasyDiskFormat(DiskInfo disk, FileSystem fileSystem,
            string volumeName, UInt64 sizeInMB, bool isQuickFormatting,
            bool isCompressed, bool isOverride, bool isNoWait, bool isNoErr)
        {
            string output = string.Empty;

            output += outputConsoleAppliationName + "Easy disk format\n";

            output += DPFunctions.Clean(disk.DiskIndex, false);
            output += DPFunctions.Convert(disk.DiskIndex, ConvertOptions.GPT);
            output += DPFunctions.Delete(disk.DiskIndex, 1, false, true);

            output += DPFunctions.CreatePartition(disk.DiskIndex, CreatePartitionOptions.PRIMARY, sizeInMB, isNoErr);
            output += DPFunctions.Format(disk.DiskIndex, 1, fileSystem, volumeName, isQuickFormatting, isCompressed, isOverride, isNoWait, isNoErr);
            output += DPFunctions.Assign(disk.DiskIndex, 1, isNoErr);

            return output;
        }

        public static string EasyDiskFormat(DiskInfo disk, FileSystem fileSystem,
            string volumeName, bool isQuickFormatting,
            bool isCompressed, bool isOverride, bool isNoWait, bool isNoErr)
        {
            string output = string.Empty;

            output += outputConsoleAppliationName + "Easy disk format\n";

            output += DPFunctions.Clean(disk.DiskIndex, false);
            output += DPFunctions.Convert(disk.DiskIndex, ConvertOptions.GPT);
            output += DPFunctions.Delete(disk.DiskIndex, 1, false, true);

            output += DPFunctions.CreatePartition(disk.DiskIndex, CreatePartitionOptions.PRIMARY, 0, isNoErr);
            output += DPFunctions.Format(disk.DiskIndex, 1, fileSystem, volumeName, isQuickFormatting, isCompressed, isOverride, isNoWait, isNoErr);
            output += DPFunctions.Assign(disk.DiskIndex, 1, isNoErr);

            return output;
        }
    }
}
