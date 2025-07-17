namespace GUIForDiskpart.Presentation.Presenter.Windows.Components
{
    public class PCFormatBase
    {
        public PLog Log;

        public const string FS_NTFS = "NTFS";
        public const string FS_FAT32 = "FAT32";
        public const string FS_EXFAT = "exFAT";
        public const string FAT32_MAX_SIZE_TEXT = "32768";
        public const string FAT32_SIZE0 = $"Size will be {FAT32_MAX_SIZE_TEXT} MB -> {FS_FAT32} maximum.";
        public const string FAT32_OVER_SIZE = $"ERROR: {FS_FAT32} max size is {FAT32_MAX_SIZE_TEXT} MB!";
        public const string SEC_WIN_WARN_DRIVE = "Format the whole drive! ALL DATA WILL BE LOST!";
        public const string SEC_WIN_WARN_PART = "Format the partition! ALL DATA WILL BE LOST!";

        public ulong FAT32_Max => ulong.Parse(FAT32_MAX_SIZE_TEXT);
    }
}
