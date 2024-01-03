using System;
using System.Collections.Generic;
using System.Reflection;

namespace GUIForDiskpart.main
{
    public class WMIPartition
    {
        private const string wmiInfoKey = "---WINDOWS MANAGEMENT INSTRUMENTATION INFO---";
        private const string wmiInfoValue = "---Win32_DiskPartition---";
        private const string keyPrefix = "WMI";

        private UInt16 availability;
        public UInt16 Availability { get { return availability; } set { availability = value; } }

        private UInt16 statusInfo;
        public UInt16 StatusInfo { get { return statusInfo; } set { statusInfo = value; } }

        private UInt64 blockSize;
        public UInt64 BlockSize { get { return blockSize; } set { blockSize = value; } }

        private string caption;
        public string Caption { get { return caption; } set { caption = value; } }

        private UInt32 configManagerErrorCode;
        public UInt32 ConfigManagerErrorCode { get { return configManagerErrorCode; } set { configManagerErrorCode = value; } }

        private bool configManagerUserConfig;
        public bool ConfigManagerUserConfig { get { return configManagerUserConfig; } set { configManagerUserConfig = value; } }

        private string creationClassName;
        public string CreationClassName { get { return creationClassName; } set { creationClassName = value; } }

        private string description;
        public string Description { get { return description; } set { description = value; } }

        private string deviceID;
        public string DeviceID { get { return deviceID; } set { deviceID = value; } }

        private UInt32 diskIndex;
        public UInt32 DiskIndex { get { return diskIndex; } set { diskIndex = value; } }

        private bool errorCleared;
        public bool ErrorCleared { get { return errorCleared; } set { errorCleared = value; } }

        private string errorDescription;
        public string ErrorDescription { get { return errorDescription; } set { errorDescription = value; } }

        private string errorMethodology;
        public string ErrorMethodology { get { return errorMethodology; } set { errorMethodology = value; } }

        private UInt32 hiddenSectors;
        public UInt32 HiddenSectors { get { return hiddenSectors; } set { hiddenSectors = value; } }

        private UInt32 partitionIndex;
        public UInt32 PartitionIndex { get { return partitionIndex; } set { partitionIndex = value; } }

        private DateTime installDate;
        public DateTime InstallDate { get { return installDate; } set { installDate = value; } }

        private UInt32 lastErrorCode;
        public UInt32 LastErrorCode { get { return lastErrorCode; } set { lastErrorCode = value; } }

        private string name;
        public string Name { get { return name; } set { name = value; } }

        private UInt64 numberOfBlocks;
        public UInt64 NumberOfBlocks { get { return numberOfBlocks; } set { numberOfBlocks = value; } }

        private string pnpDeviceID;
        public string PNPDeviceID { get { return pnpDeviceID; } set { pnpDeviceID = value; } }

        private bool powerManagementSupported;
        public bool PowerManagementSupported { get { return powerManagementSupported; } set { powerManagementSupported = value; } }

        private string purpose;
        public string Purpose { get { return purpose; } set { purpose = value; } }

        private bool rewritePartition;
        public bool RewritePartition { get { return rewritePartition; } set { rewritePartition = value; } }

        private UInt64 startingOffset;
        public UInt64 StartingOffset { get { return startingOffset; } set { startingOffset = value; } }

        private string systemCreationClassName;
        public string SystemCreationClassName { get { return systemCreationClassName; } set { systemCreationClassName = value; } }

        private string systemName;
        public string SystemName { get { return systemName; } set { systemName = value; } }

        private string partitionName;
        public string PartitionName { get {  return partitionName; } set { partitionName = value; } }

        private bool bootable;
        public bool Bootable { get { return bootable; } set { bootable = value; } }

        private bool bootPartition;
        public bool BootPartition { get { return bootPartition; } set { bootPartition = value; } }

        private bool primaryPartition;
        public bool PrimaryPartition { get { return primaryPartition; } set { primaryPartition = value; } }

        private UInt64 size;
        public UInt64 Size { get { return size; } set { size = value; } }

        private string status;
        public string Status { get { return status; } set { status = value; } }

        private string type;
        public string Type { get { return type; } set { type = value; } }

        public bool IsLogicalPartition => GetLogicalPartition();

        public LogicalDiskInfo logicalDiskInfo;
        public LogicalDiskInfo LogicalDiskInfo { get { return logicalDiskInfo; } }

        public void PrintToConsole()
        {
            Console.WriteLine(GetOutputAsString());
        }

        public string GetOutputAsString()
        {
            string output = string.Empty;

            output += "___WMI-Partition___" + "\n\n";
            output += "PartitionName: " + "\t\t\t" + PartitionName + "\n";
            output += "Bootable: " + "\t\t\t" + Bootable + "\n";
            output += "BootPartition: " + "\t\t\t" + BootPartition + "\n";
            output += "PrimaryPartition: " + "\t\t\t" + PrimaryPartition + "\n";
            output += "TotalSize: " + "\t\t\t" + Size + "\n";
            output += "MediaStatus: " + "\t\t\t" + Status + "\n";
            output += "MediaType: " + "\t\t\t" + Type + "\n";

            if (logicalDiskInfo != null) 
            { 
                output += logicalDiskInfo.GetOutputAsString();
            }

            output += "_________________" + "\n\n";
            
            return output;
        }

        public void AddLogicalDisk(LogicalDiskInfo disk)
        {
            logicalDiskInfo = disk;
        }

        private bool GetLogicalPartition()
        {  return logicalDiskInfo != null; }

        public Dictionary<string, object?> GetKeyValuePairs()
        {
            Dictionary<string, object?> data = new Dictionary<string, object?>();
            PropertyInfo[] wmiProperties = typeof(WMIPartition).GetProperties();

            data.Add(wmiInfoKey, wmiInfoValue);

            foreach (PropertyInfo property in wmiProperties)
            {
                string key = $"{keyPrefix} {property.Name}";
                object? value = property.GetValue(this);

                if (data.ContainsKey(key)) continue;
                if (key == $"{keyPrefix} Size") continue;

                if (typeof(LogicalDiskInfo) == property.PropertyType) continue;
                
                data.Add(key, value);
            }

            return data;
        }
    }
}
