using GUIForDiskpart.Database.Data;
using GUIForDiskpart.Database.Data.diskpart;
using GUIForDiskpart.Model.Logic.Diskpart;
using Markdig.Parsers;
using System;

namespace GUIForDiskpart.Model.Logic
{
    public static class ComfortFeatures
    {
        private const string OUTPUT_CONSOLE_APPLIATION_NAME = "GUIFD - ";

        public static string EasyDiskFormat(DiskInfo diskInfo, FileSystem fileSystem,
            string volumeName, char driveLetter, ulong sizeInMB, bool isQuickFormatting,
            bool isCompressed, bool isOverride, bool isNoWait, bool isNoErr)
        {
            string output = string.Empty;

            output += OUTPUT_CONSOLE_APPLIATION_NAME + "Easy disk format\n";
            if (!diskInfo.IsOnline)
            {
                output += DPFunctions.OnOfflineDisk(diskInfo.DiskIndex, true, false);
            }
            output += DPFunctions.Clean(diskInfo.DiskIndex, false);
            output += DPFunctions.Convert(diskInfo.DiskIndex, ConvertOptions.GPT);
            output += DPFunctions.Delete(diskInfo.DiskIndex, 1, false, true);

            output += DPFunctions.CreatePartition(diskInfo.DiskIndex, CreatePartitionOptions.PRIMARY, sizeInMB, isNoErr);
            output += DPFunctions.Format(diskInfo.DiskIndex, 1, fileSystem, volumeName, isQuickFormatting, isCompressed, isOverride, isNoWait, isNoErr);
            output += DPFunctions.Assign(diskInfo.DiskIndex, 1, driveLetter, isNoErr);

            return output;
        }

        public static string EasyDiskFormat(DiskInfo diskInfo, FileSystem fileSystem,
            string volumeName, ulong sizeInMB, bool isQuickFormatting,
            bool isCompressed, bool isOverride, bool isNoWait, bool isNoErr)
        {
            string output = string.Empty;

            output += OUTPUT_CONSOLE_APPLIATION_NAME + "Easy disk format\n";
            if (!diskInfo.IsOnline)
            {
                output += DPFunctions.OnOfflineDisk(diskInfo.DiskIndex, true, false);
            }
            output += DPFunctions.Clean(diskInfo.DiskIndex, false);
            output += DPFunctions.Convert(diskInfo.DiskIndex, ConvertOptions.GPT);
            output += DPFunctions.Delete(diskInfo.DiskIndex, 1, false, true);

            output += DPFunctions.CreatePartition(diskInfo.DiskIndex, CreatePartitionOptions.PRIMARY, sizeInMB, isNoErr);
            output += DPFunctions.Format(diskInfo.DiskIndex, 1, fileSystem, volumeName, isQuickFormatting, isCompressed, isOverride, isNoWait, isNoErr);
            output += DPFunctions.Assign(diskInfo.DiskIndex, 1, isNoErr);

            return output;
        }

        public static string EasyDiskFormat(DiskInfo diskInfo, FileSystem fileSystem,
            string volumeName, bool isQuickFormatting,
            bool isCompressed, bool isOverride, bool isNoWait, bool isNoErr)
        {
            string output = string.Empty;

            output += OUTPUT_CONSOLE_APPLIATION_NAME + "Easy disk format\n";
            if (!diskInfo.IsOnline)
            {
                output += DPFunctions.OnOfflineDisk(diskInfo.DiskIndex, true, false);
            }
            output += DPFunctions.Clean(diskInfo.DiskIndex, false);
            output += DPFunctions.Convert(diskInfo.DiskIndex, ConvertOptions.GPT);
            output += DPFunctions.Delete(diskInfo.DiskIndex, 1, false, true);

            output += DPFunctions.CreatePartition(diskInfo.DiskIndex, CreatePartitionOptions.PRIMARY, 0, isNoErr);
            output += DPFunctions.Format(diskInfo.DiskIndex, 1, fileSystem, volumeName, isQuickFormatting, isCompressed, isOverride, isNoWait, isNoErr);
            output += DPFunctions.Assign(diskInfo.DiskIndex, 1, isNoErr);

            return output;
        }
    }
}
