namespace GUIForDiskpart.diskpart
{
    public static class CreatePartitionOptions
    {
        public static string EFI { get { return "EFI"; } }
        public static string EXTENDED { get { return "EXTENDED"; } }
        public static string LOGICAL { get { return "LOGICAL"; } }
        public static string MSR { get { return "MSR"; } }
        public static string PRIMARY { get { return "PRIMARY"; } }
    }

    public static class CreateVolumeOptions
    {
        public static string RAID { get { return "RAID"; } }
        public static string SIMPLE { get { return "SIMPLE"; } }
        public static string STRIPE { get { return "STRIPE"; } }
        public static string MIRROR { get { return "MIRROR"; } }
    }
}
