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

        private WSMModel wsm;
        public WSMModel WSM
        {
            get => wsm;
            set => wsm = value;
        }

        private WMIModel wmi;
        public WMIModel WMI
        {
            get => wmi;
            set => wmi = value;
        }

        public LDModel LDModel => WMI.LDModel;

        private DAModel defragAnalysis;
        public DAModel DefragAnalysis
        {
            get => defragAnalysis;
            set => defragAnalysis = value;
        }

        public bool HasWSMPartition
        { get { return WSM == null ? false : true; } }

        public bool HasWMIPartition
        { get { return WMI == null ? false : true; } }

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

        public bool HasDriveLetter()
        {
            bool retVal = false;
            if (WMI == null && WSM == null) return retVal;
            if (WSM != null) retVal = WSM.DriveLetter > 65;
            if (!retVal && WMI != null) retVal = WMI.LDModel.DriveLetter[0] > 65;

            return retVal;
        }

        public char GetDriveLetter()
        {
            char retVal = ' ';
            if (!HasDriveLetter()) return retVal;

            if (WSM.DriveLetter > 65)
            {
                retVal = WSM.DriveLetter;
            }
            else if (WMI.LDModel.DriveLetter[0] > 65)
            {
                retVal = WMI.LDModel.DriveLetter[0];
            }

            return retVal;
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
                fullOutput += WSM.GetOutputAsString();
            }

            if (HasWMIPartition)
            {
                fullOutput += WMI.GetOutputAsString();
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
                AppendDataToDict(data, WSM.GetKeyValuePairs());
            }

            if (HasWMIPartition)
            {
                AppendDataToDict(data, WMI.GetKeyValuePairs());
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
