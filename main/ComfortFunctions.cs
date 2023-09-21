using GUIForDiskpart.diskpart;
using System;

namespace GUIForDiskpart.main
{
    public class ComfortFunctions
    {
        public DPFunctions dpFunctions;

        public string FormatWholeDrive(int diskIndex, int partitionCount, FileSystem fileSystem,
            string volumeName, char driveLetter, UInt64 sizeInMB, bool isQuickFormatting,
            bool isCompressed, bool isOverride, bool isNoWait, bool isNoErr)
        {
            string output = string.Empty;

            for (int i = 1; i < partitionCount; i++) 
            {
                dpFunctions.Delete(diskIndex, i);
            }

            output += dpFunctions.CreatePartition(diskIndex, CreatePartitionOptions.PRIMARY, sizeInMB, isNoErr);
            output += dpFunctions.Format(diskIndex, 1, fileSystem, volumeName, isQuickFormatting, isCompressed, isOverride, isNoWait, isNoErr);
            output += dpFunctions.Assign(diskIndex, 1, driveLetter, isNoErr);
            
            return output;
        }

        public string FormatWholeDrive(int diskIndex, int partitionCount, FileSystem fileSystem,
            string volumeName, UInt64 sizeInMB, bool isQuickFormatting,
            bool isCompressed, bool isOverride, bool isNoWait, bool isNoErr)
        {
            string output = string.Empty;

            for (int i = 1; i < partitionCount; i++)
            {
                dpFunctions.Delete(diskIndex, i);
            }

            output += dpFunctions.CreatePartition(diskIndex, CreatePartitionOptions.PRIMARY, sizeInMB, isNoErr);
            output += dpFunctions.Format(diskIndex, 1, fileSystem, volumeName, isQuickFormatting, isCompressed, isOverride, isNoWait, isNoErr);
            output += dpFunctions.Assign(diskIndex, 1, isNoErr);

            return output;
        }

        public string FormatWholeDrive(int diskIndex, int partitionCount, FileSystem fileSystem,
            string volumeName, bool isQuickFormatting,
            bool isCompressed, bool isOverride, bool isNoWait, bool isNoErr)
        {
            string output = string.Empty;

            for (int i = 1; i < partitionCount; i++)
            {
                dpFunctions.Delete(diskIndex, i);
            }

            output += dpFunctions.CreatePartition(diskIndex, CreatePartitionOptions.PRIMARY, 0, isNoErr);
            output += dpFunctions.Format(diskIndex, 1, fileSystem, volumeName, isQuickFormatting, isCompressed, isOverride, isNoWait, isNoErr);
            output += dpFunctions.Assign(diskIndex, 1, isNoErr);

            return output;
        }
    }
}
