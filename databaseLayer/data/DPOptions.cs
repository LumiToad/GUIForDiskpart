namespace GUIForDiskpart.Database.Data.diskpart
{
    public class Attributes
    {
        public const string SET = "SET";
        public const string CLEAR = "CLEAR";
        public const string HIDDEN = "HIDDEN";
        public const string READONLY = "READONLY";
        public const string NODEFAULTDRIVELETTER = "NODEFAULTDRIVELETTER";
        public const string SHADOWCOPY = "SHADOWCOPY";
    }

    public static class Convert
    {
        public static string BASIC { get { return "BASIC"; } }
        public static string DYNAMIC { get { return "DYNAMIC"; } }
        public static string GPT { get { return "GPT"; } }
        public static string MBR { get { return "MBR"; } }
    }

    public static class Partition
    {
        public static string EFI { get { return "EFI"; } }
        public static string EXTENDED { get { return "EXTENDED"; } }
        public static string LOGICAL { get { return "LOGICAL"; } }
        public static string MSR { get { return "MSR"; } }
        public static string PRIMARY { get { return "PRIMARY"; } }
    }

    public static class Volume
    {
        public static string RAID { get { return "RAID"; } }
        public static string SIMPLE { get { return "SIMPLE"; } }
        public static string STRIPE { get { return "STRIPE"; } }
        public static string MIRROR { get { return "MIRROR"; } }
    }

    public static class List
    {
        public static string DISK { get { return "DISK"; } }
        public static string PARTITION { get { return "PARTITION"; } }
        public static string VOLUME { get { return "VOLUME"; } }
        public static string VDISK { get { return "VDISK"; } }
    }

    public static class SetIDO
    {
        public static string EFI { get => "c12a7328-f81f-11d2-ba4b-00a0c93ec93b"; }
        public static string BASIC { get => "ebd0a0a2-b9e5-4433-87c0-68b6b72699c7"; }
        public static string MSR { get => "e3c9e316-0b5c-4db8-817d-f92df00215ae"; }
        public static string LDM_META { get => "5808c8aa-7e8f-42e0-85d2-e1e90434cfb3"; }
        public static string LDM_DATA { get => "af9b60a0-1431-4f62-bc68-3311714a69ad"; }
        public static string CLUSTER_META { get => "db97dba9-0840-4bae-97f0-ffb9a327c7e1"; }
    }
}
