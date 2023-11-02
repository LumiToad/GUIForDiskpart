using GUIForDiskpart.diskpart;
using System;

namespace GUIForDiskpart.main
{
    public static class ComfortFeatures
    {
        private const string outputConsoleAppliationName = "GUIFD - ";

        public static string EasyDiskFormat(DiskInfo drive, FileSystem fileSystem,
            string volumeName, char driveLetter, UInt64 sizeInMB, bool isQuickFormatting,
            bool isCompressed, bool isOverride, bool isNoWait, bool isNoErr)
        {
            string output = string.Empty;

            output += outputConsoleAppliationName + "Easy drive format\n";

            output += DPFunctions.Clean(drive.DiskIndex, false);
            output += DPFunctions.Convert(drive.DiskIndex, ConvertOptions.GPT);
            output += DPFunctions.Delete(drive.DiskIndex, 1, false, true);

            output += DPFunctions.CreatePartition(drive.DiskIndex, CreatePartitionOptions.PRIMARY, sizeInMB, isNoErr);
            output += DPFunctions.Format(drive.DiskIndex, 1, fileSystem, volumeName, isQuickFormatting, isCompressed, isOverride, isNoWait, isNoErr);
            output += DPFunctions.Assign(drive.DiskIndex, 1, driveLetter, isNoErr);
            
            return output;
        }

        public static string EasyDiskFormat(DiskInfo drive, FileSystem fileSystem,
            string volumeName, UInt64 sizeInMB, bool isQuickFormatting,
            bool isCompressed, bool isOverride, bool isNoWait, bool isNoErr)
        {
            string output = string.Empty;

            output += DPFunctions.Clean(drive.DiskIndex, false);
            output += DPFunctions.Convert(drive.DiskIndex, ConvertOptions.GPT);
            output += DPFunctions.Delete(drive.DiskIndex, 1, false, true);

            output += DPFunctions.CreatePartition(drive.DiskIndex, CreatePartitionOptions.PRIMARY, sizeInMB, isNoErr);
            output += DPFunctions.Format(drive.DiskIndex, 1, fileSystem, volumeName, isQuickFormatting, isCompressed, isOverride, isNoWait, isNoErr);
            output += DPFunctions.Assign(drive.DiskIndex, 1, isNoErr);

            return output;
        }

        public static string EasyDiskFormat(DiskInfo drive, FileSystem fileSystem,
            string volumeName, bool isQuickFormatting,
            bool isCompressed, bool isOverride, bool isNoWait, bool isNoErr)
        {
            string output = string.Empty;

            output += DPFunctions.Clean(drive.DiskIndex, false);
            output += DPFunctions.Convert(drive.DiskIndex, ConvertOptions.GPT);
            output += DPFunctions.Delete(drive.DiskIndex, 1, false, true);

            output += DPFunctions.CreatePartition(drive.DiskIndex, CreatePartitionOptions.PRIMARY, 0, isNoErr);
            output += DPFunctions.Format(drive.DiskIndex, 1, fileSystem, volumeName, isQuickFormatting, isCompressed, isOverride, isNoWait, isNoErr);
            output += DPFunctions.Assign(drive.DiskIndex, 1, isNoErr);

            return output;
        }
    }
}
