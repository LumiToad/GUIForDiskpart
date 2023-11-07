using System;
using System.Collections.Generic;
using System.Reflection;

namespace GUIForDiskpart.main
{
    public class WSMPartition
    {
        private const string wsmInfoString = "---WINDOWS STORAGE MANAGEMENT INFO---";

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

        private WMIPartition wmiPartition;
        public WMIPartition WMIPartition 
        { get { return wmiPartition; } set { wmiPartition = value; } }

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

            output += "\t\t___WSMPartition___" + "\n";
            output += "\t\tDiskNumber: " + DiskNumber + "\n";
            output += "\t\tPartitionNumber: " + PartitionNumber + "\n";
            output += "\t\tDriveLetter: " + DriveLetter + "\n";
            output += "\t\tOperationalStatus: " + OperationalStatus + "\n";
            output += "\t\tTransitionState: " + TransitionState + "\n";
            output += "\t\tSize: " + Size + " bytes" + "\n";
            output += "\t\tMBR Type: " + GetPartitionType() + "\n";
            output += "\t\tGPT Type: " + GetPartitionType() + "\n";
            output += "\t\tGUID: " + GUID + "\n";
            output += "\t\tIsReadOnly: " + IsReadOnly + "\n";
            output += "\t\tIsOffline: " + IsOffline + "\n";
            output += "\t\tIsSystem: " + IsSystem + "\n";
            output += "\t\tIsBoot: " + IsBoot + "\n";
            output += "\t\tIsActive: " + IsActive + "\n";
            output += "\t\tIsHidden: " + IsHidden + "\n";
            output += "\t\tIsShadowCopy: " + IsShadowCopy + "\n";
            output += "\t\tNoDefaultDriveLetter: " + NoDefaultDriveLetter + "\n";
                         
            output += "\t\t_________________" + "\n";

            if (wmiPartition != null) 
            {
                output += wmiPartition.GetOutputAsString();
            }

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

            data.Add(wsmInfoString, "DRAWN FROM POWERSHELL, CORRESPONDS TO DISKPART");

            foreach (PropertyInfo property in wsmProperties)
            {
                string key = $"WSM {property.Name}";
                object? value = property.GetValue(this);

                if (data.ContainsKey(key)) continue;
                if (key == "Size") continue;
                
                if (key == "WSM FormattedSize")
                {
                    key = "WSM TotalSpace";
                }

                if (typeof(WMIPartition) == property.PropertyType) continue;
                

                data.Add(key, value);
            }

            if (WMIPartition != null)
            {
                Dictionary<string, object?> wmiKeyValuePairs = WMIPartition.GetKeyValuePairs();
                foreach (string key in wmiKeyValuePairs.Keys)
                {
                    data.Add(key, wmiKeyValuePairs[key]);
                }
            }

            return data;
        }
    }
}
