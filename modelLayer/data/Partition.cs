using System;
using System.Collections.Generic;

namespace GUIForDiskpart.Model.Data
{
    public class Partition
    {
        private DiskModel assignedDiskModel;
        public DiskModel AssignedDiskModel
        {
            get => assignedDiskModel;
            set => assignedDiskModel = value;
        }

        private WSMPartition wsmPartition;
        public WSMPartition WSMPartition
        {
            get => wsmPartition;
            set => wsmPartition = value;
        }

        private WMIPartition wmiPartition;
        public WMIPartition WMIPartition
        {
            get => wmiPartition;
            set => wmiPartition = value;
        }

        public LDModel LDModel => WMIPartition.LDModel;

        private DAModel defragAnalysis;
        public DAModel DefragAnalysis
        {
            get => defragAnalysis;
            set => defragAnalysis = value;
        }

        public bool HasWSMPartition
        { get { return WSMPartition == null ? false : true; } }

        public bool HasWMIPartition
        { get { return WMIPartition == null ? false : true; } }

        public bool IsLogicalDisk
        {
            get
            {
                if (!HasWMIPartition) return false;
                return LDModel == null ? false : true;
            }
        }

        public bool HasDefragAnalysis
        { get { return DefragAnalysis == null ? false : true; } }

        public bool IsVolume
        {
            get
            {
                if (HasWSMPartition && IsLogicalDisk) return true;
                return false;
            }
        }

        public void PrintToConsole()
        {
            Console.WriteLine(GetOutputAsString());
        }

        public string GetOutputAsString()
        {
            string fullOutput = string.Empty;

            if (HasWSMPartition)
            {
                fullOutput += WSMPartition.GetOutputAsString();
            }

            if (HasWMIPartition)
            {
                fullOutput += WMIPartition.GetOutputAsString();
            }

            if (IsLogicalDisk)
            {
                fullOutput += LDModel.GetOutputAsString();
            }

            if (HasDefragAnalysis)
            {
                fullOutput += DefragAnalysis.GetOutputAsString();
            }

            return fullOutput;
        }


        public Dictionary<string, object?> GetKeyValuePairs()
        {
            Dictionary<string, object?> data = new Dictionary<string, object?>();

            if (HasWSMPartition)
            {
                AppendDataToDict(data, WSMPartition.GetKeyValuePairs());
            }

            if (HasWMIPartition)
            {
                AppendDataToDict(data, WMIPartition.GetKeyValuePairs());
            }

            if (IsLogicalDisk)
            {
                AppendDataToDict(data, LDModel.GetKeyValuePairs());
            }

            if (HasDefragAnalysis)
            {
                AppendDataToDict(data, DefragAnalysis.GetKeyValuePairs());
            }

            return data;
        }

        private void AppendDataToDict(Dictionary<string, object?> appedTarget, Dictionary<string, object?> toAppend)
        {
            foreach (string key in toAppend.Keys)
            {
                appedTarget.Add(key, toAppend[key]);
            }
        }
    }
}
