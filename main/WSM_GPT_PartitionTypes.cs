﻿namespace GUIForDiskpart.main
{
    public static class WSM_GPT_PartitionTypes
    {
        public const string SYSTEMPARTITION = "{c12a7328-f81f-11d2-ba4b-00a0c93ec93b}";
        public const string MICROSOFTRESERVED = "{e3c9e316-0b5c-4db8-817d-f92df00215ae}";
        public const string BASICDATA = "{ebd0a0a2-b9e5-4433-87c0-68b6b72699c7}";
        public const string LDMMETADATA = "{5808c8aa-7e8f-42e0-85d2-e1e90434cfb3}";
        public const string LDMDATA = "{af9b60a0-1431-4f62-bc68-3311714a69ad}";
        public const string MICROSOFTRECOVERY = "{de94bba4-06d1-4d40-a16a-bfd50179d6ac}";
        public const string ANDROIDGUIDONE = "{193d1ea4-b3ca-11e4-b075-10604b889dcf}";

        public static string GetTypeByGUID(string? guid)
        {
            string result = string.Empty;

            switch (guid) 
            {
                case (SYSTEMPARTITION):
                    result = "System";
                    break;
                case (MICROSOFTRESERVED):
                    result = "Reserved";
                    break;
                case (BASICDATA):
                    result = "Basic";
                    break;
                case (LDMMETADATA):
                    result = "LDM MetaData";
                    break;
                case (LDMDATA):
                    result = "LDM Data";
                    break;
                case (MICROSOFTRECOVERY):
                    result = "Recovery";
                    break;
                case (ANDROIDGUIDONE):
                    result = "...Android?";
                    break;
            }

            if (string.IsNullOrEmpty(guid))
            {
                result = "...Unknown";
            }

            return result;
        }
    }
}
