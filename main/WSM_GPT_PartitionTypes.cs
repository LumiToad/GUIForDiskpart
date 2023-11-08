namespace GUIForDiskpart.main
{
    public static class WSM_GPT_PartitionTypes
    {
        private const string systemPartition = "{c12a7328-f81f-11d2-ba4b-00a0c93ec93b}";
        public static string SystemPartition
        { get { return systemPartition; } }

        private const string microsoftReserved = "{e3c9e316-0b5c-4db8-817d-f92df00215ae}";
        public static string MicrosoftReserved
        { get { return microsoftReserved; } }

        private const string basicData = "{ebd0a0a2-b9e5-4433-87c0-68b6b72699c7}";
        public static string BasicData
        { get { return basicData; } }

        private const string ldmMetaData = "{5808c8aa-7e8f-42e0-85d2-e1e90434cfb3}";
        public static string LDMMetaData
        { get { return ldmMetaData; } }

        private const string ldmData = "{af9b60a0-1431-4f62-bc68-3311714a69ad}";
        public static string LDMData
        { get { return ldmData; } }

        private const string microsoftRecovery = "{de94bba4-06d1-4d40-a16a-bfd50179d6ac}";
        public static string MicrosoftRecovery
        { get { return microsoftReserved; } }

        private const string androidGUIDOne = "{193d1ea4-b3ca-11e4-b075-10604b889dcf}";
        public static string AndroidGUIDOne
        { get { return androidGUIDOne; } }

        public static string GetTypeByGUID(string? guid)
        {
            string result = string.Empty;

            switch (guid) 
            {
                case (systemPartition):
                    result = "System";
                    break;
                case (microsoftReserved):
                    result = "Reserved";
                    break;
                case (basicData):
                    result = "Basic";
                    break;
                case (ldmMetaData):
                    result = "LDM MetaData";
                    break;
                case (ldmData):
                    result = "LDM Data";
                    break;
                case (microsoftRecovery):
                    result = "Recovery";
                    break;
                case (androidGUIDOne):
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
