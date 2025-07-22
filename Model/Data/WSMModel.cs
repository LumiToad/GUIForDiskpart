using System;
using System.Collections.Generic;
using System.Reflection;

using GUIForDiskpart.Utils;
using GUIForDiskpart.Database.Data.Types;

using MBRTypes = GUIForDiskpart.Database.Data.Types.WSM_MBR_PartitionTypes;
using GPTTypes = GUIForDiskpart.Database.Data.Types.WSM_GPT_PartitionTypes;


namespace GUIForDiskpart.Model.Data
{
    public class WSMModel
    {
        private const string WSM_INFO_KEY = "---WINDOWS STORAGE MANAGEMENT INFO---";
        private const string WSM_INFO_VALUE = "---MSFT_Storage, obtained via Powershell---";
        private const string KEY_PREFIX = "WSM";

        private const string PARTITION_TYPE_UNKNOWN_TXT = "Type unknown...";

        public uint DiskNumber { get; set; }
        public uint PartitionNumber { get; set; }
        public char DriveLetter { get; set; }
        public ushort OperationalStatus { get; set; }
        public ushort TransitionState { get; set; }
        public ulong Size { get; set; }
        public ushort? MBRType { get; set; }
        public string? GPTType { get; set; }
        public string GUID { get; set; }
        public bool IsReadOnly { get; set; }
        public bool IsOffline { get; set; }
        public bool IsSystem { get; set; }
        public bool IsBoot { get; set; }
        public bool IsActive { get; set; }
        public bool IsHidden { get; set; }
        public bool IsShadowCopy { get; set; }
        public bool? NoDefaultDriveLetter { get; set; }
        public ulong Offset { get; set; }

        public string FormattedSize => ByteFormatter.BytesToAsString(Size);
        public string PartitionType => GetPartitionType();
        public string PartitionTable => GetPartitionTable();

        private string GetPartitionType()
        {

            string result = PARTITION_TYPE_UNKNOWN_TXT;

            if (PartitionTable == CommonTypes.MBR)
            {
                result = MBRTypes.GetTypeByUInt16(MBRType);
            }
            else if (PartitionTable == CommonTypes.GPT)
            {
                result = GPTTypes.GetTypeByGUID(GPTType);
            }

            return result;
        }

        public void PrintToConsole()
        {
            Console.WriteLine(GetOutputAsString());
        }

        public string GetOutputAsString()
        {
            string output = string.Empty;

            output += "___WSM-Partition___" + "\n\n";
            output += "DiskNumber: " + "\t\t\t" + DiskNumber + "\n";
            output += "PartitionNumber: " + "\t\t\t" + PartitionNumber + "\n";
            output += "DriveLetter: " + "\t\t\t" + DriveLetter + "\n";
            output += "OperationalStatus: " + "\t\t\t" + OperationalStatus + "\n";
            output += "TransitionState: " + "\t\t\t" + TransitionState + "\n";
            output += "Size: " + "\t\t\t" + Size + " bytes" + "\n";
            output += "MBR Type: " + "\t\t\t" + GetPartitionType() + "\n";
            output += "GPT Type: " + "\t\t\t" + GetPartitionType() + "\n";
            output += "GUID: " + "\t\t\t" + GUID + "\n";
            output += "IsReadOnly: " + "\t\t\t" + IsReadOnly + "\n";
            output += "IsOffline: " + "\t\t\t" + IsOffline + "\n";
            output += "IsSystem: " + "\t\t\t" + IsSystem + "\n";
            output += "IsBoot: " + "\t\t\t" + IsBoot + "\n";
            output += "IsActive: " + "\t\t\t" + IsActive + "\n";
            output += "IsHidden: " + "\t\t\t" + IsHidden + "\n";
            output += "IsShadowCopy: " + "\t\t\t" + IsShadowCopy + "\n";
            output += "NoDefaultDriveLetter: " + "\t\t\t" + NoDefaultDriveLetter + "\n";

            output += "_________________" + "\n\n";

            return output;
        }

        private string GetPartitionTable()
        {
            string result = string.Empty;

            if (MBRType != null)
            {
                result = CommonTypes.MBR;
            }

            if (!string.IsNullOrWhiteSpace(GPTType))
            {
                result = CommonTypes.GPT;
            }

            return result;
        }

        public Dictionary<string, object?> GetKeyValuePairs()
        {
            Dictionary<string, object?> data = new Dictionary<string, object?>();
            PropertyInfo[] wsmProperties = typeof(WSMModel).GetProperties();

            data.Add(WSM_INFO_KEY, WSM_INFO_VALUE);

            foreach (PropertyInfo property in wsmProperties)
            {
                string key = $"{KEY_PREFIX} {property.Name}";
                object? value = property.GetValue(this);

                if (data.ContainsKey(key)) continue;
                if (key == $"{KEY_PREFIX} Size") continue;

                if (key == $"{KEY_PREFIX} FormattedSize")
                {
                    key = $"{KEY_PREFIX} TotalSpace";
                }

                data.Add(key, value);
            }

            return data;
        }
    }
}
