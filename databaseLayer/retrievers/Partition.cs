using System;
using System.Collections.Generic;
using System.Management;

using GUIForDiskpart.Model.Data;
using GUIForDiskpart.Model.Logic;


namespace GUIForDiskpart.Database.Retrievers
{
    public class Partition
    {
        public void WMIPartitionQuery(ManagementObject disk, DiskModel diskInfo, ref List<WMIPartition> list)
        {
            var partitionQueryText = string.Format("associators of {{{0}}} where AssocClass = Win32_DiskDriveToDiskPartition", disk.Path.RelativePath);
            var partitionQuery = new ManagementObjectSearcher(partitionQueryText);

            foreach (ManagementObject partitionManagementObject in partitionQuery.Get())
            {
                WMIPartition wmiPartition = PartitionService.GetWMIPartition(partitionManagementObject, diskInfo.DiskIndex);
                LDModel ld = LDService.GetLogicalDisk(partitionManagementObject);
                wmiPartition.AddLogicalDisk(ld);

                list.Add(wmiPartition);
            }
        }

        public List<object> WSMPartitionQuery(DiskModel diskInfo)
        {
            string diskIndex = Convert.ToString(diskInfo.DiskIndex);

            return CommandExecuter.IssuePowershellCommand("Get-Partition", diskIndex);
        }
    }
}
