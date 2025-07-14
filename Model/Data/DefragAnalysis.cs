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

        private ulong averageFileSize;
        public ulong AverageFileSize
        { get { return averageFileSize; } set { averageFileSize = value; } }

        private double averageFreeSpacePerExtent;
        public double AverageFreeSpacePerExtent
        { get { return averageFreeSpacePerExtent; } set { averageFreeSpacePerExtent = value; } }

        private double averageFragmentsPerFile;
        public double AverageFragmentsPerFile
        { get { return averageFragmentsPerFile; } set { averageFragmentsPerFile = value; } }

        private ulong clusterSize;
        public ulong ClusterSize
        { get { return clusterSize; } set { clusterSize = value; } }

        private ulong excessFolderFragments;
        public ulong ExcessFolderFragments
        { get { return excessFolderFragments; } set { excessFolderFragments = value; } }

        private uint filePercentFragmentation;
        public uint FilePercentFragmentation
        { get { return filePercentFragmentation; } set { filePercentFragmentation = value; } }

        private ulong fragmentedFolders;
        public ulong FragmentedFolders
        { get { return fragmentedFolders; } set { fragmentedFolders = value; } }

        private ulong freeSpace;
        public ulong FreeSpace
        { get { return freeSpace; } set { freeSpace = value; } }

        private uint freeSpacePercent;
        public uint FreeSpacePercent
        { get { return freeSpacePercent; } set { freeSpacePercent = value; } }

        private uint freeSpacePercentFragmentation;
        public uint FreeSpacePercentFragmentation { get => freeSpacePercentFragmentation; set => freeSpacePercentFragmentation = value; }

        private ulong largestFreeSpaceExtent;
        public ulong LargestFreeSpaceExtent { get => largestFreeSpaceExtent; set => largestFreeSpaceExtent = value; }

        private uint mftPercentInUse;
        public uint MFTPercentInUse { get => mftPercentInUse; set => mftPercentInUse = value; }

        private ulong mftRecordCount;
        public ulong MFTRecordCount { get => mftRecordCount; set => mftRecordCount = value; }

        private ulong pageFileSize;
        public ulong PageFileSize { get => pageFileSize; set => pageFileSize = value; }

        private ulong totalExcessFragments;
        public ulong TotalExcessFragments { get => totalExcessFragments; set => totalExcessFragments = value; }

        private ulong totalFiles;
        public ulong TotalFiles { get => totalFiles; set => totalFiles = value; }

        private ulong totalFolders;
        public ulong TotalFolders { get => totalFolders; set => totalFolders = value; }

        private ulong totalFragmentedFiles;
        public ulong TotalFragmentedFiles { get => totalFragmentedFiles; set => totalFragmentedFiles = value; }

        private ulong totalFreeSpaceExtents;
        public ulong TotalFreeSpaceExtents { get => totalFreeSpaceExtents; set => totalFreeSpaceExtents = value; }

        private ulong totalMFTFragments;
        public ulong TotalMFTFragments { get => totalMFTFragments; set => totalMFTFragments = value; }

        private ulong totalMFTSize;
        public ulong TotalMFTSize { get => totalMFTSize; set => totalMFTSize = value; }

        private ulong totalPageFileFragments;
        public ulong TotalPageFileFragments { get => totalPageFileFragments; set => totalPageFileFragments = value; }

        private uint totalPercentFragmentation;
        public uint TotalPercentFragmentation { get => totalPercentFragmentation; set => totalPercentFragmentation = value; }

        private ulong totalUnmoveableFiles;
        public ulong TotalUnmovableFiles { get => totalUnmoveableFiles; set => totalUnmoveableFiles = value; }

        private ulong usedSpace;
        public ulong UsedSpace { get => usedSpace; set => usedSpace = value; }

        private string volumeName;
        public string VolumeName { get => volumeName; set => volumeName = value; }

        private ulong volumeSize;
        public ulong VolumeSize { get => volumeSize; set => volumeSize = value; }

        public ulong AvailableForShrink
        { get { return FreeSpace - (FreeSpace - LargestFreeSpaceExtent); } }

        private ulong availableForExtend;
        public ulong AvailableForExtend
        { get => availableForExtend; set => availableForExtend = value; }

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
