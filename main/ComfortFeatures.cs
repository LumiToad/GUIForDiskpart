using GUIForDiskpart.diskpart;
using System;

namespace GUIForDiskpart.main
{
    public class ComfortFeatures
    {
        public DPFunctions dpFunctions;
        private const string outputConsoleAppliationName = "GUIFD - ";

        public string EasyDriveFormat(DriveInfo drive, FileSystem fileSystem,
            string volumeName, char driveLetter, UInt64 sizeInMB, bool isQuickFormatting,
            bool isCompressed, bool isOverride, bool isNoWait, bool isNoErr)
        {
            string output = string.Empty;

            output += outputConsoleAppliationName + "Easy drive format\n";

            output += dpFunctions.Clean(drive.DiskIndex, false);
            output += dpFunctions.Convert(drive.DiskIndex, ConvertOptions.GPT);
            output += dpFunctions.Delete(drive.DiskIndex, 1, false, true);

            output += dpFunctions.CreatePartition(drive.DiskIndex, CreatePartitionOptions.PRIMARY, sizeInMB, isNoErr);
            output += dpFunctions.Format(drive.DiskIndex, 1, fileSystem, volumeName, isQuickFormatting, isCompressed, isOverride, isNoWait, isNoErr);
            output += dpFunctions.Assign(drive.DiskIndex, 1, driveLetter, isNoErr);
            
            return output;
        }

        public string EasyDriveFormat(DriveInfo drive, FileSystem fileSystem,
            string volumeName, UInt64 sizeInMB, bool isQuickFormatting,
            bool isCompressed, bool isOverride, bool isNoWait, bool isNoErr)
        {
            string output = string.Empty;

            output += dpFunctions.Clean(drive.DiskIndex, false);
            output += dpFunctions.Convert(drive.DiskIndex, ConvertOptions.GPT);
            output += dpFunctions.Delete(drive.DiskIndex, 1, false, true);

            output += dpFunctions.CreatePartition(drive.DiskIndex, CreatePartitionOptions.PRIMARY, sizeInMB, isNoErr);
            output += dpFunctions.Format(drive.DiskIndex, 1, fileSystem, volumeName, isQuickFormatting, isCompressed, isOverride, isNoWait, isNoErr);
            output += dpFunctions.Assign(drive.DiskIndex, 1, isNoErr);

            return output;
        }

        public string EasyDiskFormat(DriveInfo drive, FileSystem fileSystem,
            string volumeName, bool isQuickFormatting,
            bool isCompressed, bool isOverride, bool isNoWait, bool isNoErr)
        {
            string output = string.Empty;

            output += dpFunctions.Clean(drive.DiskIndex, false);
            output += dpFunctions.Convert(drive.DiskIndex, ConvertOptions.GPT);
            output += dpFunctions.Delete(drive.DiskIndex, 1, false, true);

            output += dpFunctions.CreatePartition(drive.DiskIndex, CreatePartitionOptions.PRIMARY, 0, isNoErr);
            output += dpFunctions.Format(drive.DiskIndex, 1, fileSystem, volumeName, isQuickFormatting, isCompressed, isOverride, isNoWait, isNoErr);
            output += dpFunctions.Assign(drive.DiskIndex, 1, isNoErr);

            return output;
        }
    }
}
