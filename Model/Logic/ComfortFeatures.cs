using GUIForDiskpart.Model.Logic.Diskpart;


namespace GUIForDiskpart.Model.Logic
{
    public static class ComfortFeatures
    {
        private const string OUTPUT_CONSOLE_APPLIATION_NAME = "GUIFD - ";

        public static string EasyDiskFormat(DiskModel diskModel, FSType fileSystem,
            string volumeName, char driveLetter, ulong sizeInMB, bool isQuickFormatting,
            bool isCompressed, bool isOverride, bool isNoWait, bool isNoErr)
        {
            string output = string.Empty;

            output += OUTPUT_CONSOLE_APPLIATION_NAME + "Easy disk format\n";
            if (!diskModel.IsOnline)
            {
                output += DPFunctions.OnOfflineDisk(diskModel.DiskIndex, true, false);
            }
            output += DPFunctions.Clean(diskModel.DiskIndex, false);
            output += DPFunctions.Convert(diskModel.DiskIndex, Convert.GPT);
            output += DPFunctions.Delete(diskModel.DiskIndex, 1, false, true);

            output += DPFunctions.CreatePartition(diskModel.DiskIndex, Create.PRIMARY, sizeInMB, isNoErr);
            output += DPFunctions.Format(diskModel.DiskIndex, 1, fileSystem, volumeName, isQuickFormatting, isCompressed, isOverride, isNoWait, isNoErr);
            output += DPFunctions.Assign(diskModel.DiskIndex, 1, driveLetter, isNoErr);

            return output;
        }

        public static string EasyDiskFormat(DiskModel diskModel, FSType fileSystem,
            string volumeName, ulong sizeInMB, bool isQuickFormatting,
            bool isCompressed, bool isOverride, bool isNoWait, bool isNoErr)
        {
            string output = string.Empty;

            output += OUTPUT_CONSOLE_APPLIATION_NAME + "Easy disk format\n";
            if (!diskModel.IsOnline)
            {
                output += DPFunctions.OnOfflineDisk(diskModel.DiskIndex, true, false);
            }
            output += DPFunctions.Clean(diskModel.DiskIndex, false);
            output += DPFunctions.Convert(diskModel.DiskIndex, Convert.GPT);
            output += DPFunctions.Delete(diskModel.DiskIndex, 1, false, true);

            output += DPFunctions.CreatePartition(diskModel.DiskIndex, Create.PRIMARY, sizeInMB, isNoErr);
            output += DPFunctions.Format(diskModel.DiskIndex, 1, fileSystem, volumeName, isQuickFormatting, isCompressed, isOverride, isNoWait, isNoErr);
            output += DPFunctions.Assign(diskModel.DiskIndex, 1, isNoErr);

            return output;
        }

        public static string EasyDiskFormat(DiskModel diskModel, FSType fileSystem,
            string volumeName, bool isQuickFormatting,
            bool isCompressed, bool isOverride, bool isNoWait, bool isNoErr)
        {
            string output = string.Empty;

            output += OUTPUT_CONSOLE_APPLIATION_NAME + "Easy disk format\n";
            if (!diskModel.IsOnline)
            {
                output += DPFunctions.OnOfflineDisk(diskModel.DiskIndex, true, false);
            }
            output += DPFunctions.Clean(diskModel.DiskIndex, false);
            output += DPFunctions.Convert(diskModel.DiskIndex, Convert.GPT);
            output += DPFunctions.Delete(diskModel.DiskIndex, 1, false, true);

            output += DPFunctions.CreatePartition(diskModel.DiskIndex, Create.PRIMARY, 0, isNoErr);
            output += DPFunctions.Format(diskModel.DiskIndex, 1, fileSystem, volumeName, isQuickFormatting, isCompressed, isOverride, isNoWait, isNoErr);
            output += DPFunctions.Assign(diskModel.DiskIndex, 1, isNoErr);

            return output;
        }
    }
}
