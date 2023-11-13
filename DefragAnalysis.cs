using GUIForDiskpart.main;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace GUIForDiskpart
{
    public class DefragAnalysis
    {
        private const string defragInfoKey = "---DEFRAG ANALYSIS---";
        private const string defragInfoValue = "Win32_Volume > Win32_DefragAnalysis";
        private const string keyPrefix = "DEFRAG";

        private UInt64 averageFileSize;
        public UInt64 AverageFileSize
        { get { return averageFileSize; } set {  averageFileSize = value; } }

        private double averageFreeSpacePerExtent;
        public double AverageFreeSpacePerExtent
        { get { return averageFreeSpacePerExtent; } set { averageFreeSpacePerExtent = value; } }

        private double averageFragmentsPerFile;
        public double AverageFragmentsPerFile
        { get { return averageFragmentsPerFile; } set { averageFragmentsPerFile = value; } }

        private UInt64 clusterSize;
        public UInt64 ClusterSize
        { get { return clusterSize; } set { clusterSize = value; } }

        private UInt64 excessFolderFragments;
        public UInt64 ExcessFolderFragments
        { get { return excessFolderFragments; } set { excessFolderFragments = value; } }
        
        private UInt32 filePercentFragmentation;
        public UInt32 FilePercentFragmentation
        { get { return filePercentFragmentation; } set { filePercentFragmentation = value; } }
        
        private UInt64 fragmentedFolders;
        public UInt64 FragmentedFolders
        { get { return fragmentedFolders; } set { fragmentedFolders = value; } }

        private UInt64 freeSpace;
        public UInt64 FreeSpace
        { get { return freeSpace; } set { freeSpace = value; } }

        private UInt32 freeSpacePercent;
        public UInt32 FreeSpacePercent
        { get { return freeSpacePercent; } set { freeSpacePercent = value; } }

        private UInt32 freeSpacePercentFragmentation;
        public UInt32 FreeSpacePercentFragmentation { get => freeSpacePercentFragmentation; set => freeSpacePercentFragmentation = value; }

        private UInt64 largestFreeSpaceExtent;
        public UInt64 LargestFreeSpaceExtent { get => largestFreeSpaceExtent; set => largestFreeSpaceExtent = value; }

        private UInt32 mftPercentInUse;
        public UInt32 MFTPercentInUse { get => mftPercentInUse; set => mftPercentInUse = value; }

        private UInt64 mftRecordCount;
        public UInt64 MFTRecordCount { get => mftRecordCount; set => mftRecordCount = value; }

        private UInt64 pageFileSize;
        public UInt64 PageFileSize { get => pageFileSize; set => pageFileSize = value; }

        private UInt64 totalExcessFragments;
        public UInt64 TotalExcessFragments { get => totalExcessFragments; set => totalExcessFragments = value; }

        private UInt64 totalFiles;
        public UInt64 TotalFiles { get => totalFiles; set => totalFiles = value; }

        private UInt64 totalFolders;
        public UInt64 TotalFolders { get => totalFolders; set => totalFolders = value; }

        private UInt64 totalFragmentedFiles;
        public UInt64 TotalFragmentedFiles { get => totalFragmentedFiles; set => totalFragmentedFiles = value; }

        private UInt64 totalFreeSpaceExtents;
        public UInt64 TotalFreeSpaceExtents { get => totalFreeSpaceExtents; set => totalFreeSpaceExtents = value; }

        private UInt64 totalMFTFragments;
        public UInt64 TotalMFTFragments { get => totalMFTFragments; set => totalMFTFragments = value; }

        private UInt64 totalMFTSize;
        public UInt64 TotalMFTSize { get => totalMFTSize; set => totalMFTSize = value; }

        private UInt64 totalPageFileFragments;
        public UInt64 TotalPageFileFragments { get => totalPageFileFragments; set => totalPageFileFragments = value; }

        private UInt32 totalPercentFragmentation;
        public UInt32 TotalPercentFragmentation { get => totalPercentFragmentation; set => totalPercentFragmentation = value; }

        private UInt64 totalUnmoveableFiles;
        public UInt64 TotalUnmovableFiles { get => totalUnmoveableFiles; set => totalUnmoveableFiles = value; }

        private UInt64 usedSpace;
        public UInt64 UsedSpace { get => usedSpace; set => usedSpace = value; }

        private string volumeName;
        public string VolumeName { get => volumeName; set => volumeName = value; }

        private UInt64 volumeSize;
        public UInt64 VolumeSize { get => volumeSize; set => volumeSize = value; }

        public UInt64 AvailableForShrink
        { get { return FreeSpace - (FreeSpace - LargestFreeSpaceExtent); } }

        private UInt64 availableForExtend;
        public UInt64 AvailableForExtend
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

            data.Add(defragInfoKey, defragInfoValue);

            foreach (PropertyInfo property in defragAnalysis)
            {
                string key = $"{keyPrefix} {property.Name}";
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
                    value = ByteFormatter.FormatBytes(Convert.ToInt64(value));
                }
                data.Add(key, value);
            }

            return data;
        }
    }
}
