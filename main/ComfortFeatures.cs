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

            output += dpFunctions.Clean(drive.DriveIndex, false);
            output += dpFunctions.Convert(drive.DriveIndex, ConvertOptions.GPT);
            output += dpFunctions.Delete(drive.DriveIndex, 1, false, true);

            output += dpFunctions.CreatePartition(drive.DriveIndex, CreatePartitionOptions.PRIMARY, sizeInMB, isNoErr);
            output += dpFunctions.Format(drive.DriveIndex, 1, fileSystem, volumeName, isQuickFormatting, isCompressed, isOverride, isNoWait, isNoErr);
            output += dpFunctions.Assign(drive.DriveIndex, 1, driveLetter, isNoErr);
            
            return output;
        }

        public string EasyDriveFormat(DriveInfo drive, FileSystem fileSystem,
            string volumeName, UInt64 sizeInMB, bool isQuickFormatting,
            bool isCompressed, bool isOverride, bool isNoWait, bool isNoErr)
        {
            string output = string.Empty;

            output += dpFunctions.Clean(drive.DriveIndex, false);
            output += dpFunctions.Convert(drive.DriveIndex, ConvertOptions.GPT);
            output += dpFunctions.Delete(drive.DriveIndex, 1, false, true);

            output += dpFunctions.CreatePartition(drive.DriveIndex, CreatePartitionOptions.PRIMARY, sizeInMB, isNoErr);
            output += dpFunctions.Format(drive.DriveIndex, 1, fileSystem, volumeName, isQuickFormatting, isCompressed, isOverride, isNoWait, isNoErr);
            output += dpFunctions.Assign(drive.DriveIndex, 1, isNoErr);

            return output;
        }

        public string EasyDriveFormat(DriveInfo drive, FileSystem fileSystem,
            string volumeName, bool isQuickFormatting,
            bool isCompressed, bool isOverride, bool isNoWait, bool isNoErr)
        {
            string output = string.Empty;

            output += dpFunctions.Clean(drive.DriveIndex, false);
            output += dpFunctions.Convert(drive.DriveIndex, ConvertOptions.GPT);
            output += dpFunctions.Delete(drive.DriveIndex, 1, false, true);

            output += dpFunctions.CreatePartition(drive.DriveIndex, CreatePartitionOptions.PRIMARY, 0, isNoErr);
            output += dpFunctions.Format(drive.DriveIndex, 1, fileSystem, volumeName, isQuickFormatting, isCompressed, isOverride, isNoWait, isNoErr);
            output += dpFunctions.Assign(drive.DriveIndex, 1, isNoErr);

            return output;
        }
    }
}
