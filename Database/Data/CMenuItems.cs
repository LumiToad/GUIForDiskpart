using System.Collections.Generic;
using System.Management.Automation;
using GUIForDiskpart.Utils;


namespace GUIForDiskpart.Database.Data
{
    public struct CMenuItemData
    {
        public System.Windows.Controls.Image Image;
        public string Header;
        public bool IsEnabled;
        public string Tooltip;
    }

    public class CMenuItems
    {
        public const string DPOffline = "DPOffline";
        public const string DPOnline = "DPOnline";
        public const string DPActive = "DPActive";
        public const string DPInactive = "DPInactive";
        public const string DPAttributes = "DPAttributes";
        public const string DPShrink = "DPShrink";
        public const string DPExtend = "DPExtend";
        public const string CMDCHDSK = "CMDCHDSK";
        public const string PSAnalyzeDefrag = "PSAnalyzeDefrag";

        private static Dictionary<string, CMenuItemData> K_Name_V_Data = new()
        {
            {DPOffline, new CMenuItemData()
            {
                Image = IconUtils.Diskpart,
                Header = "DISKPART - Offline",
                IsEnabled = true,
                Tooltip = "Takes an online disk or volume to the offline state."
            }
            },

            {DPOnline, new CMenuItemData()
            {
                Image = IconUtils.Diskpart,
                Header = "DISKPART - Online",
                IsEnabled = true,
                Tooltip = "Takes an offline disk or volume to the online state."
            }
            },

            {DPActive, new CMenuItemData()
            {
                Image = IconUtils.Diskpart,
                Header = "DISKPART - Active",
                IsEnabled = true,
                Tooltip = "Marks the disk's partition with focus, as active."
            }
            },

            {DPInactive, new CMenuItemData()
            {
                Image = IconUtils.Diskpart,
                Header = "DISKPART - Inactive",
                IsEnabled = true,
                Tooltip = "Marks the system partition or boot partition with focus as inactive on basic master boot record (MBR) disks."
            }
            },

            {DPAttributes, new CMenuItemData()
            {
                Image = IconUtils.Diskpart,
                Header = "DISKPART - Attributes",
                IsEnabled = true,
                Tooltip = "Displays, sets, or clears the attributes of a disk or volume."
            }
            },

            {DPShrink, new CMenuItemData()
            {
                Image = IconUtils.Diskpart,
                Header = "DISKPART - Shrink",
                IsEnabled = true,
                Tooltip = "Reduces the size of the selected volume by the amount you specify."
            }
            },

            {DPExtend, new CMenuItemData()
            {
                Image = IconUtils.Diskpart,
                Header = "DISKPART - Extend",
                IsEnabled = true,
                Tooltip = "Extends the volume or partition with focus, along with its file system, into free (unallocated) space on a disk."
            }
            },

            {CMDCHDSK, new CMenuItemData()
            {
                Image = IconUtils.CMD,
                Header = "CMD - CHKDSK",
                IsEnabled = true,
                Tooltip = "Checks the file system and file system metadata of a volume for logical and physical errors."
            }
            },

            {PSAnalyzeDefrag, new CMenuItemData()
            {
                Image = IconUtils.Commandline,
                Header = "Powershell - DefragAnalysis",
                IsEnabled = true,
                Tooltip =   "Will analyze the fragmentation of this volume.\n" +
                            "Will not actually start a defragmentation process.\n" +
                            "Retrieved data will be shown in the list entrys below.\n" +
                            "Can take a while!"
            }
            }
        };

        public static CMenuItemData GetCMenuItemData(string cMenuItemName)
        {
            if (K_Name_V_Data.ContainsKey(cMenuItemName))
            { 
                return K_Name_V_Data[cMenuItemName];
            }
            return new CMenuItemData();
        }
    }
}
