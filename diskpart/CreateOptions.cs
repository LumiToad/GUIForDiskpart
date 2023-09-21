namespace GUIForDiskpart.diskpart
{
    public enum CreatePartitionOptions
    {
        EFI = 10,
        EXTENDED = 20,
        LOGICAL = 30,
        MSR = 40,
        PRIMARY = 50
    }

    public enum CreateVolumeOptions
    {
        RAID = 10,
        SIMPLE = 20,
        STRIPE = 30,
        MIRROR = 40,
    }
}
