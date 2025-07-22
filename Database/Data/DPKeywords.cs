namespace GUIForDiskpart.Database.Data.DP
{
    public class Basics
    {
        public const string LIST = "LIST";
        public const string ASSIGN = "ASSIGN";
        public const string DETAIL = "DETAIL";
        public const string CLEAN = "CLEAN";
        public const string SELECT = "SELECT";
        public const string CONVERT = "CONVERT";
        public const string EXTEND = "EXTEND";

        public const string PART = "PART";
        public const string DISK = "DISK";
        public const string PARTITION = "PARTITION";
        public const string VOLUME = "VOLUME";

        public const string VDISK = "VDISK";
        public const string NOERR = "NOERR";
        public const string FILE = "FILE";
        public const string LETTER = "LETTER";
        public const string ACTIVE = "ACTIVE";
        public const string INACTIVE = "INACTIVE";
        public const string SIZE = "SIZE";
        public const string ALL = "ALL";
    }

    public class Shrink
    {
        public const string SHRINK = "SHRINK";

        public const string DESIRED = "DESIRED";
        public const string MINIMUM = "MINIMUM";
        public const string NOWAIT = "NOWAIT";
    }

    public class Format
    {
        public const string FORMAT = "FORMAT";
        
        public const string FS = "FS";
        public const string LABEL = "LABEL";
        public const string QUICK = "QUICK";
        public const string COMPRESS = "COMPRESS";
        public const string OVERRIDE = "OVERRIDE";
        public const string NOWAIT = "NOWAIT";
    }

    public class Delete
    {
        public const string DELETE = "DELETE";

        public const string OVERRIDE = "OVERRIDE";
    }

    public class Remove
    {
        public const string REMOVE = "REMOVE";

        public const string MOUNT = "MOUNT";
        public const string DISMOUNT = "DISMOUNT";
    }

    public class Attributes
    {
        public const string ATTRIBUTES = "ATTRIBUTES";

        public const string SET = "SET";
        public const string CLEAR = "CLEAR";
        public const string HIDDEN = "HIDDEN";
        public const string READONLY = "READONLY";
        public const string NODEFAULTDRIVELETTER = "NODEFAULTDRIVELETTER";
        public const string SHADOWCOPY = "SHADOWCOPY";
    }

    public class Convert
    {
        public const string CONVERT = "CONVERT";

        public const string BASIC = "BASIC";
        public const string DYNAMIC = "DYNAMIC";
        public const string GPT = "GPT";
        public const string MBR = "MBR";
    }

    public class Create
    {
        public const string CREATE = "CREATE";
        public const string PARTITION = "PARTITION";
        public const string VOLUME = "VOLUME";
        public const string VDISK = "VDISK";

        public const string EFI = "EFI";
        public const string EXTENDED = "EXTENDED";
        public const string LOGICAL = "LOGICAL";
        public const string MSR = "MSR";
        public const string PRIMARY = "PRIMARY";
        
        public const string RAID = "RAID";
        public const string SIMPLE = "SIMPLE";
        public const string STRIPE = "STRIPE";
        public const string MIRROR = "MIRROR";

        public const string FIXED = "FIXED";
        public const string EXPANDABLE = "EXPANDABLE";
        public const string MAXIMUM = "MAXIMUM";
        public const string SD = "SD";
        public const string PARENT = "PARENT";
        public const string SOURCE = "SOURCE";

        public const string SIZE = "SIZE";
        public const string ALIGN = "ALIGN";
        public const string TYPE = "TYPE";
    }

    public class VDisk
    {
        public const string VDISK = "VDISK";

        public const string ATTACH = "ATTACH";
        public const string DETACH = "DETACH";
        public const string READONLY = "READONLY";
        public const string USEFILESD = "USEFILESD";
        public const string COMPACT = "COMPACT";
        public const string EXPAND = "EXPAND";
        public const string MAXIMUM = "MAXIMUM";
        public const string MERGE = "MERGE";
        public const string DEPTH = "DEPTH";
    }

    public class SetIDO
    {
        public const string SET = "SET";
        public const string ID = "ID";

        public const string OVERRIDE = "OVERRIDE";

        public const string EFI = "c12a7328-f81f-11d2-ba4b-00a0c93ec93b";
        public const string BASIC = "ebd0a0a2-b9e5-4433-87c0-68b6b72699c7";
        public const string MSR = "e3c9e316-0b5c-4db8-817d-f92df00215ae";
        public const string LDM_META = "5808c8aa-7e8f-42e0-85d2-e1e90434cfb3";
        public const string LDM_DATA = "af9b60a0-1431-4f62-bc68-3311714a69ad";
        public const string CLUSTER_META = "db97dba9-0840-4bae-97f0-ffb9a327c7e1";
    }
}
