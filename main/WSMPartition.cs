using System;
using System.Collections.Generic;
using System.Reflection;

namespace GUIForDiskpart.main
{
    public class WSMPartition
    {
        private const string wsmInfoKey = "---WINDOWS STORAGE MANAGEMENT INFO---";
        private const string wsmInfoValue = "---MSFT_Storage, obtained via Powershell---";
        private const string keyPrefix = "WSM";

        private UInt32 diskNumber;
        public UInt32 DiskNumber { get { return diskNumber; } set { diskNumber = value; } }

        private UInt32 partitionNumber;
        public UInt32 PartitionNumber { get { return partitionNumber; } set { partitionNumber = value; } }

        private char driveLetter;
        public char DriveLetter { get { return driveLetter; } set { driveLetter = value; } }

        private UInt16 operationalStatus;
        public UInt16 OperationalStatus { get { return operationalStatus; } set { operationalStatus = value; } }

        private UInt16 transitionState;
        public UInt16 TransitionState { get { return transitionState; } set { transitionState = value; } }

        private UInt64 size;
        public UInt64 Size { get { return size; } set { size = value; } }

        private UInt16? mbrType;
        public UInt16? MBRType { get { return mbrType; } set { mbrType = value; } }

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
        public bool? NoDefaultDriveLetter { get {  return noDefaultDriveLetter; } set {  noDefaultDriveLetter = value; } }

        private UInt64 offset;
        public UInt64 Offset { get { return offset; } set { offset = value; } }

        public string FormattedSize => ByteFormatter.FormatBytes(Size);

        public string PartitionType => GetPartitionType();

        public string PartitionTable => GetPartitionTable();

        private string GetPartitionType()
        {
            
            string result = "Type unknown...";
            
            if (PartitionTable == "MBR")
            {
                result = WSM_MBR_PartitionTypes.GetTypeByUInt16(MBRType);
            }
            else if (PartitionTable == "GPT")
            {
                result = WSM_GPT_PartitionTypes.GetTypeByGUID(GPTType);
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

            data.Add(wsmInfoKey, wsmInfoValue);

            foreach (PropertyInfo property in wsmProperties)
            {
                string key = $"{keyPrefix} {property.Name}";
                object? value = property.GetValue(this);

                if (data.ContainsKey(key)) continue;
                if (key == $"{keyPrefix} Size") continue;
                
                if (key == $"{keyPrefix} FormattedSize")
                {
                    key = $"{keyPrefix} TotalSpace";
                }

                data.Add(key, value);
            }

            return data;
        }
    }
}
