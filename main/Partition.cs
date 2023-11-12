using System;
using System.Collections.Generic;
using System.Windows.Markup;

namespace GUIForDiskpart.main
{
    public class Partition
    {
        private WSMPartition wsmPartition;
        public WSMPartition WSMPartition
        { 
            get { return wsmPartition; } 
            set { wsmPartition = value; }
        }

        private WMIPartition wmiPartition;
        public WMIPartition WMIPartition 
        { 
            get { return wmiPartition; } 
            set { wmiPartition = value; }
        }

        public LogicalDiskInfo LogicalDiskInfo => WMIPartition.LogicalDiskInfo;

        public bool HasWSMPartition
        { get { return (WSMPartition == null) ? false : true; } }

        public bool HasWMIPartition
        { get { return (WMIPartition == null) ? false : true; } }

        public bool IsLogicalDisk
        { 
            get 
            {
                if (!HasWMIPartition) return false;
                return (LogicalDiskInfo == null) ? false : true; 
            } 
        }

        public bool IsVolume
        { get { if (HasWMIPartition && IsLogicalDisk) return true; return false; } }

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
                fullOutput += LogicalDiskInfo.GetOutputAsString();
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
                AppendDataToDict(data, LogicalDiskInfo.GetKeyValuePairs());
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
