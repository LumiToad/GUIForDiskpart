using System;
using System.Collections.Generic;
using System.Reflection;

using GUIForDiskpart.Service;
using MBRTypes = GUIForDiskpart.Database.Data.Types.WSM_MBR_PartitionTypes;
using GPTTypes = GUIForDiskpart.Database.Data.Types.WSM_GPT_PartitionTypes;


namespace GUIForDiskpart.Model.Data
{
    public class WSMPartition
    {
        private const string WSM_INFO_KEY = "---WINDOWS STORAGE MANAGEMENT INFO---";
        private const string WSM_INFO_VALUE = "---MSFT_Storage, obtained via Powershell---";
        private const string KEY_PREFIX = "WSM";

        private uint diskNumber;
        public uint DiskNumber { get { return diskNumber; } set { diskNumber = value; } }

        private uint partitionNumber;
        public uint PartitionNumber { get { return partitionNumber; } set { partitionNumber = value; } }

        private char driveLetter;
        public char DriveLetter { get { return driveLetter; } set { driveLetter = value; } }

        private ushort operationalStatus;
        public ushort OperationalStatus { get { return operationalStatus; } set { operationalStatus = value; } }

        private ushort transitionState;
        public ushort TransitionState { get { return transitionState; } set { transitionState = value; } }

        private ulong size;
        public ulong Size { get { return size; } set { size = value; } }

        private ushort? mbrType;
        public ushort? MBRType { get { return mbrType; } set { mbrType = value; } }

        private string? gptType;
        public string? GPTType { get { return gptType; } set { gptType = value; } }

        private string guid;
        public string GUID { get { return guid; } set { guid = value; } }

        private bool isReadOnly;
        public bool IsReadOnly { get { return isReadOnly; } set { isReadOnly = value; } }

        private bool isOffline;
        public bool IsOffline { get { return isOffline; } set { isOffline = value; } }

        private bool isSystem;
        public bool IsSystem { get { return isSystem; } set { isSystem = value; } }

        private bool isBoot;
        public bool IsBoot { get { return isBoot; } set { isBoot = value; } }

        private bool isActive;
        public bool IsActive { get { return isActive; } set { isActive = value; } }

        private bool isHidden;
        public bool IsHidden { get { return isHidden; } set { isHidden = value; } }

        private bool isShadowCopy;
        public bool IsShadowCopy { get { return isShadowCopy; } set { isShadowCopy = value; } }

        private bool? noDefaultDriveLetter;
        public bool? NoDefaultDriveLetter { get { return noDefaultDriveLetter; } set { noDefaultDriveLetter = value; } }

        private ulong offset;
        public ulong Offset { get { return offset; } set { offset = value; } }

        public string FormattedSize => ByteFormatter.FormatBytes(Size);

        public string PartitionType => GetPartitionType();

        public string PartitionTable => GetPartitionTable();

        private string GetPartitionType()
        {

            string result = "Type unknown...";

            if (PartitionTable == "MBR")
            {
                result = MBRTypes.GetTypeByUInt16(MBRType);
            }
            else if (PartitionTable == "GPT")
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

            if (mbrType != null)
            {
                result = "MBR";
            }

            if (!string.IsNullOrWhiteSpace(gptType))
            {
                result = "GPT";
            }

            return result;
        }

        public Dictionary<string, object?> GetKeyValuePairs()
        {
            Dictionary<string, object?> data = new Dictionary<string, object?>();
            PropertyInfo[] wsmProperties = typeof(WSMPartition).GetProperties();

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
