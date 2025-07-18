using GUIForDiskpart.Database.Data.Types;

namespace GUIForDiskpart.Presentation.Presenter.Windows.Components
{
    public class PCFormatBase
    {
        public const string FAT32_MAX_SIZE_TEXT = "32768";
        public const string FAT32_SIZE0 = $"Size will be {FAT32_MAX_SIZE_TEXT} MB -> {FSTypeStrings.FS_FAT32} maximum.";
        public const string FAT32_OVER_SIZE = $"ERROR: {FSTypeStrings.FS_FAT32} max size is {FAT32_MAX_SIZE_TEXT} MB!";
        public const string SEC_WIN_WARN_DRIVE = "Format the whole drive! ALL DATA WILL BE LOST!";
        public const string SEC_WIN_WARN_PART = "Format the partition! ALL DATA WILL BE LOST!";

        public ulong FAT32_Max => ulong.Parse(FAT32_MAX_SIZE_TEXT);
    }
}
