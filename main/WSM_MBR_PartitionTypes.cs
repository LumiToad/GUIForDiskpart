using System;

namespace GUIForDiskpart.main
{
    public static class WSM_MBR_PartitionTypes
    {
        public static string GetTypeByUInt16(UInt16? type)
        {
            string result = string.Empty;

            switch (type)
            {
                case (1):
                    result = "FAT12";
                    break;
                case (4):
                    result = "FAT16";
                    break;
                case (5):
                    result = "Extended";
                    break;
                case (6):
                    result = "Huge";
                    break;
                case (7):
                    result = "IFS";
                    break;
                case (12):
                    result = "FAT32";
                    break;
                case (14):
                    result = "16-Bit FAT INT13";
                    break;
                case (15):
                    result = "Extended INT13";
                    break;
                case (82):
                    result = "Linux SWAP";
                    break;
                case (83):
                    result = "Linux FileSystem";
                    break;
            }

            return result;
        }
    }
}
