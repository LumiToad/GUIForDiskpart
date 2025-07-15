using System;
using System.Collections.Generic;
using System.Reflection;

using GUIForDiskpart.Utils;


namespace GUIForDiskpart.Model.Data
{
    public class DefragAnalysis
    {
        private const string DEFRAG_INFO_KEY = "---DEFRAG ANALYSIS---";
        private const string DEFRAG_INFO_VALUE = "Win32_Volume > Win32_DefragAnalysis";
        private const string KEY_PREFIX = "DEFRAG";

        public ulong AverageFileSize { get; set; }
        public double AverageFreeSpacePerExtent { get; set; }
        public double AverageFragmentsPerFile { get; set; }
        public ulong ClusterSize { get; set; }
        public ulong ExcessFolderFragments { get; set; }
        public uint FilePercentFragmentation { get; set; }
        public ulong FragmentedFolders { get; set; }
        public ulong FreeSpace { get; set; }
        public uint FreeSpacePercent { get; set; }
        public uint FreeSpacePercentFragmentation { get; set; }
        public ulong LargestFreeSpaceExtent { get; set; }
        public uint MFTPercentInUse { get; set; }
        public ulong MFTRecordCount { get; set; }
        public ulong PageFileSize { get; set; }
        public ulong TotalExcessFragments { get; set; }
        public ulong TotalFiles { get; set; }
        public ulong TotalFolders { get; set; }
        public ulong TotalFragmentedFiles { get; set; }
        public ulong TotalFreeSpaceExtents { get; set; }
        public ulong TotalMFTFragments { get; set; }
        public ulong TotalMFTSize { get; set; }
        public ulong TotalPageFileFragments { get; set; }
        public uint TotalPercentFragmentation { get; set; }
        public ulong TotalUnmovableFiles { get; set; }
        public ulong UsedSpace { get; set; }
        public string VolumeName { get; set; }
        public ulong VolumeSize { get; set; }
        public ulong AvailableForShrink
        { get { return FreeSpace - (FreeSpace - LargestFreeSpaceExtent); } }
        public ulong AvailableForExtend { get; set; }

        public void PrintToConsole()
        {
            Console.WriteLine(GetOutputAsString());
        }

        public string GetOutputAsString()
        {
            string output = string.Empty;

            output += "___DefragAnalysis___" + "\n";
            output += "AverageFileSize: " + AverageFileSize + "\n";
            output += "AverageFreeSpacePerExtent: " + AverageFreeSpacePerExtent + "\n";
            output += "ClusterSize: " + ClusterSize + "\n";
            output += "ExcessFolderFragments: " + ExcessFolderFragments + "\n";
            output += "FilePercentFragmentation: " + FilePercentFragmentation + "\n";
            output += "FragmentedFolders: " + FragmentedFolders + "\n";
            output += "FreeSpace: " + FreeSpace + "\n";
            output += "FreeSpacePercent: " + FreeSpacePercent + "\n";
            output += "FreeSpacePercent: " + FreeSpacePercent + "\n";
            output += "LargestFreeSpaceExtent: " + LargestFreeSpaceExtent + "\n";
            output += "MFTPercentInUse: " + MFTPercentInUse + "\n";
            output += "MFTRecordCount: " + MFTRecordCount + "\n";
            output += "PageFileSize: " + PageFileSize + "\n";
            output += "TotalExcessFragments: " + TotalExcessFragments + "\n";
            output += "TotalFiles: " + TotalFiles + "\n";
            output += "TotalFolders: " + TotalFolders + "\n";
            output += "TotalFragmentedFiles: " + TotalFragmentedFiles + "\n";
            output += "TotalFreeSpaceExtents: " + TotalFreeSpaceExtents + "\n";
            output += "TotalMFTFragments: " + TotalMFTFragments + "\n";
            output += "TotalPageFileFragments: " + TotalPageFileFragments + "\n";
            output += "TotalPercentFragmentation: " + TotalPercentFragmentation + "\n";
            output += "TotalUnmovableFiles: " + TotalUnmovableFiles + "\n";
            output += "UsedSpace: " + UsedSpace + "\n";
            output += "VolumeName: " + VolumeName + "\n";
            output += "VolumeSize: " + VolumeSize + "\n";

            output += "_________________" + "\n";

            return output;
        }

        public Dictionary<string, object?> GetKeyValuePairs()
        {
            Dictionary<string, object?> data = new Dictionary<string, object?>();
            PropertyInfo[] defragAnalysis = typeof(DefragAnalysis).GetProperties();

            data.Add(DEFRAG_INFO_KEY, DEFRAG_INFO_VALUE);

            foreach (PropertyInfo property in defragAnalysis)
            {
                string key = $"{KEY_PREFIX} {property.Name}";
                object? value = property.GetValue(this);

                if (data.ContainsKey(key)) continue;

                if
                    (
                    key.Contains("AverageFreeSpacePerExtent") ||
                    key.Contains("FreeSpace") ||
                    key.Contains("LargestFreeSpaceExtent") ||
                    key.Contains("TotalMFTSize") ||
                    key.Contains("UsedSpace") ||
                    key.Contains("VolumeSize") ||
                    key.Contains("AvailableForShrink") ||
                    key.Contains("AvailableForExtend")
                    )
                {
                    value = ByteFormatter.BytesToUnitAsString(Convert.ToInt64(value));
                }
                data.Add(key, value);
            }

            return data;
        }
    }
}
