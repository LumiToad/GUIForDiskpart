using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Input;

namespace GUIForDiskpart.main
{
    public class LogicalDiskInfo
    {
        private const string ldInfoKey = "---WINDOWS MANAGEMENT INSTRUMENTATION Logical Disk---";
        private const string ldInfoValue = "---Win32_LogicalDisk---";
        private const string keyPrefix = "WMI-LD";

        private string driveLetter;
        public string DriveLetter { get; set; }

        private UInt32 driveType;
        public UInt32 DriveType { get; set; }

        private string fileSystem;
        public string FileSystem { get; set; }

        private UInt64 freeSpace;
        public UInt64 FreeSpace { get; set; }

        public UInt64 UsedSpace
        { get { return TotalSpace - FreeSpace; } }

        public UInt64 totalSpace;
        public UInt64 TotalSpace { get; set; }

        private string volumeName;
        public string VolumeName { get; set; }

        private string volumeSerial;
        public string VolumeSerial { get; set; }

        public string FormattedTotalSpace => ByteFormatter.FormatBytes(TotalSpace);
        public string FormattedUsedSpace => ByteFormatter.FormatBytes(UsedSpace);
        public string FormattedFreeSpace => ByteFormatter.FormatBytes(FreeSpace);

        public void PrintToConsole()
        {
            Console.WriteLine(GetOutputAsString());
        }

        public string GetOutputAsString()
        {
            string output = string.Empty;
            output += "___Logical-Disk___" + "\n\n";
            output += "VolumeName: " + "\t\t\t" + VolumeName + "\n";
            output += "DriveLetter: " + "\t\t\t" + DriveLetter + "\\" + "\n";
            output += "FileSystem: " + "\t\t\t" + FileSystem + "\n";
            output += "TotalSpace: " + "\t\t\t" + TotalSpace + "\n";
            output += "FreeSpace: " + "\t\t\t" + FreeSpace + "\n";
            output += "DriveType: " + "\t\t\t" + DriveType + "\n";
            output += "VolumeSerial: " + "\t\t\t" + VolumeSerial + "\n";

            output += "_________________" + "\n\n";

            return output;
        }

        public Dictionary<string, object?> GetKeyValuePairs()
        {
            Dictionary<string, object?> data = new Dictionary<string, object?>();
            PropertyInfo[] wmiProperties = typeof(LogicalDiskInfo).GetProperties();

            data.Add(ldInfoKey, ldInfoValue);

            foreach (PropertyInfo property in wmiProperties)
            {
                string key = $"{keyPrefix} {property.Name}";
                object? value = property.GetValue(this);

                if (data.ContainsKey(key)) continue;
                if (key == $"{keyPrefix} TotalSpace") continue;
                if (key == $"{keyPrefix} UsedSpace") continue;
                if (key == $"{keyPrefix} FreeSpace") continue;

                if (key == $"{keyPrefix} FormattedTotalSpace")
                {
                    key = $"{keyPrefix} TotalSpace";
                }

                if (key == $"{keyPrefix} FormattedUsedSpace")
                {
                    key = $"{keyPrefix} UsedSpace";
                }

                if (key == $"{keyPrefix} FormattedFreeSpace")
                {
                    key = $"{keyPrefix} FreeSpace";
                }

                data.Add(key, value);
            }

            return data;
        }
    }
}
