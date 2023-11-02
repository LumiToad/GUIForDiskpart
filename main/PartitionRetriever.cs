using System;
using System.Collections.Generic;
using System.Management;
using System.Management.Automation;

namespace GUIForDiskpart.main
{
    public static class PartitionRetriever
    {
        public static void GetAndAddPartitionsToDisk(ManagementObject disk, DiskInfo physicalDisk)
        {
            var partitionQueryText = string.Format("associators of {{{0}}} where AssocClass = Win32_DiskDriveToDiskPartition", disk.Path.RelativePath);
            var partitionQuery = new ManagementObjectSearcher(partitionQueryText);

            foreach (ManagementObject partitionManagementObject in partitionQuery.Get())
            {
                PartitionInfo newPartitionInfo = RetrieveWMIPartitions(partitionManagementObject, physicalDisk.DiskIndex);
                newPartitionInfo.WSMPartitionNumber = (UInt32)GetWSMPartitionNumber(physicalDisk, newPartitionInfo);

                LogicalDiskRetriever.GetAndAddLogicalDisks(partitionManagementObject, newPartitionInfo);

                physicalDisk.AddPartitionToList(newPartitionInfo);
            }
        }

        private static PartitionInfo RetrieveWMIPartitions(ManagementObject partition, uint diskIndex)
        {
            PartitionInfo newPartition = new PartitionInfo();

            newPartition.Availability = Convert.ToUInt16(partition.Properties["Availability"].Value);
            newPartition.StatusInfo = Convert.ToUInt16(partition.Properties["StatusInfo"].Value);
            newPartition.BlockSize = Convert.ToUInt64(partition.Properties["BlockSize"].Value);
            newPartition.Bootable = Convert.ToBoolean(partition.Properties["Bootable"].Value);
            newPartition.BootPartition = Convert.ToBoolean(partition.Properties["BootPartition"].Value);
            newPartition.Caption = Convert.ToString(partition.Properties["Caption"].Value);

            newPartition.ConfigManagerErrorCode = Convert.ToUInt32(partition.Properties["ConfigManagerErrorCode"].Value);
            newPartition.ConfigManagerUserConfig = Convert.ToBoolean(partition.Properties["ConfigManagerUserConfig"].Value);
            newPartition.CreationClassName = Convert.ToString(partition.Properties["CreationClassName"].Value);
            newPartition.Description = Convert.ToString(partition.Properties["Description"].Value);
            newPartition.DeviceID = Convert.ToString(partition.Properties["DeviceID"].Value);
            newPartition.DiskIndex = Convert.ToUInt32(partition.Properties["DiskIndex"].Value);

            newPartition.ErrorCleared = Convert.ToBoolean(partition.Properties["ErrorCleared"].Value);
            newPartition.ErrorDescription = Convert.ToString(partition.Properties["ErrorDescription"].Value);
            newPartition.ErrorMethodology = Convert.ToString(partition.Properties["ErrorMethodology"].Value);

            newPartition.HiddenSectors = Convert.ToUInt32(partition.Properties["ConfigManagerErrorCode"].Value);
            newPartition.PartitionIndex = Convert.ToUInt32(partition.Properties["Index"].Value);

            newPartition.InstallDate = Convert.ToDateTime(partition.Properties["InstallDate"].Value);
            newPartition.LastErrorCode = Convert.ToUInt32(partition.Properties["LastErrorCode"].Value);
            newPartition.PartitionName = Convert.ToString(partition.Properties["Name"].Value);
            newPartition.NumberOfBlocks = Convert.ToUInt64(partition.Properties["NumberOfBlocks"].Value);

            newPartition.PNPDeviceID = Convert.ToString(partition.Properties["PNPDeviceID"].Value);
            newPartition.PowerManagementSupported = Convert.ToBoolean(partition.Properties["PowerManagementSupported"].Value);
            newPartition.PrimaryPartition = Convert.ToBoolean(partition.Properties["PrimaryPartition"].Value);
            newPartition.Purpose = Convert.ToString(partition.Properties["Purpose"].Value);
            newPartition.RewritePartition = Convert.ToBoolean(partition.Properties["RewritePartition"].Value);
            newPartition.Size = Convert.ToUInt64(partition.Properties["Size"].Value);
            newPartition.StartingOffset = Convert.ToUInt64(partition.Properties["StartingOffset"].Value);
            newPartition.Status = Convert.ToString(partition.Properties["Status"].Value);
            newPartition.SystemCreationClassName = Convert.ToString(partition.Properties["SystemCreationClassName"].Value);
            newPartition.SystemName = Convert.ToString(partition.Properties["SystemName"].Value);
            newPartition.Type = Convert.ToString(partition.Properties["Type"].Value);

            return newPartition;
        }

        private static UInt32? GetWSMPartitionNumber(DiskInfo physicalDisk, PartitionInfo newPartition)
        {
            foreach (PSObject psObject in GetWSMPartitions(physicalDisk))
            {
                if ((UInt64)psObject.Properties["Offset"].Value == newPartition.StartingOffset)
                {
                    return (UInt32)psObject.Members["PartitionNumber"].Value;
                }
            }
            return null;
        }

        

        private static List<object> GetWSMPartitions(DiskInfo diskInfo)
        {
            string diskIndex = Convert.ToString(diskInfo.DiskIndex);

            return CommandExecuter.IssuePowershellCommand("Get-Partition", diskIndex);
        }
    }
}
