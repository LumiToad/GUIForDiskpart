using GUIForDiskpart.service;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Input;

namespace GUIForDiskpart.Model.Data
{
    public class LogicalDiskInfo
    {
        private const string LD_INFO_KEY = "---WINDOWS MANAGEMENT INSTRUMENTATION Logical Disk---";
        private const string LD_INFO_VALUE = "---Win32_LogicalDisk---";
        private const string KEY_PREFIX = "WMI-LD";

        private string driveLetter;
        public string DriveLetter { get; set; }

        private uint driveType;
        public uint DriveType { get; set; }

        private string fileSystem;
        public string FileSystem { get; set; }

        private ulong freeSpace;
        public ulong FreeSpace { get; set; }

        public ulong UsedSpace
        { get { return TotalSpace - FreeSpace; } }

        public ulong totalSpace;
        public ulong TotalSpace { get; set; }

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

            data.Add(LD_INFO_KEY, LD_INFO_VALUE);

            foreach (PropertyInfo property in wmiProperties)
            {
                string key = $"{KEY_PREFIX} {property.Name}";
                object? value = property.GetValue(this);

                if (data.ContainsKey(key)) continue;
                if (key == $"{KEY_PREFIX} TotalSpace") continue;
                if (key == $"{KEY_PREFIX} UsedSpace") continue;
                if (key == $"{KEY_PREFIX} FreeSpace") continue;

                if (key == $"{KEY_PREFIX} FormattedTotalSpace")
                {
                    key = $"{KEY_PREFIX} TotalSpace";
                }

                if (key == $"{KEY_PREFIX} FormattedUsedSpace")
                {
                    key = $"{KEY_PREFIX} UsedSpace";
                }

                if (key == $"{KEY_PREFIX} FormattedFreeSpace")
                {
                    key = $"{KEY_PREFIX} FreeSpace";
                }

                data.Add(key, value);
            }

            return data;
        }
    }
}
