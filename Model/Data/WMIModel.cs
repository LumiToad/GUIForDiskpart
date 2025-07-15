using System;
using System.Collections.Generic;
using System.Reflection;

namespace GUIForDiskpart.Model.Data
{
    public class WMIModel
    {
        private const string WMI_INFO_KEY = "---WINDOWS MANAGEMENT INSTRUMENTATION INFO---";
        private const string WMI_INFO_VALUE = "---Win32_DiskPartition---";
        private const string KEY_PREFIX = "WMI";

        public ushort Availability { get; set; }
        public ushort StatusInfo { get; set; }
        public ulong BlockSize { get; set; }
        public string Caption { get; set; }
        public uint ConfigManagerErrorCode { get; set; }
        public bool ConfigManagerUserConfig { get; set; }
        public string CreationClassName { get; set; }
        public string Description { get; set; }
        public string DeviceID { get; set; }
        public uint DiskIndex { get; set; }
        public bool ErrorCleared { get; set; }
        public string ErrorDescription { get; set; }
        public string ErrorMethodology { get; set; }
        public uint HiddenSectors { get; set; }
        public uint PartitionIndex { get; set; }
        public DateTime InstallDate { get; set; }
        public uint LastErrorCode { get; set; }
        public string Name { get; set; }
        public ulong NumberOfBlocks { get; set; }
        public string PNPDeviceID { get; set; }
        public bool PowerManagementSupported { get; set; }
        public string Purpose { get; set; }
        public bool RewritePartition { get; set; }
        public ulong StartingOffset { get; set; }
        public string SystemCreationClassName { get; set; }
        public string SystemName { get; set; }
        public string PartitionName { get; set; }
        public bool Bootable { get; set; }
        public bool BootPartition { get; set; }
        public bool PrimaryPartition { get; set; }
        public ulong Size { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }

        public bool IsLogicalPartition => GetLogicalPartition();

        public LDModel logicalDiskModel;
        public LDModel LDModel { get { return logicalDiskModel; } }

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

            if (logicalDiskModel != null)
            {
                output += logicalDiskModel.GetOutputAsString();
            }

            output += "_________________" + "\n\n";

            return output;
        }

        public void AddLogicalDisk(LDModel disk)
        {
            logicalDiskModel = disk;
        }

        private bool GetLogicalPartition()
        { return logicalDiskModel != null; }

        public Dictionary<string, object?> GetKeyValuePairs()
        {
            Dictionary<string, object?> data = new Dictionary<string, object?>();
            PropertyInfo[] wmiProperties = typeof(WMIModel).GetProperties();

            data.Add(WMI_INFO_KEY, WMI_INFO_VALUE);

            foreach (PropertyInfo property in wmiProperties)
            {
                string key = $"{KEY_PREFIX} {property.Name}";
                object? value = property.GetValue(this);

                if (data.ContainsKey(key)) continue;
                if (key == $"{KEY_PREFIX} Size") continue;

                if (typeof(LDModel) == property.PropertyType) continue;

                data.Add(key, value);
            }

            return data;
        }
    }
}
