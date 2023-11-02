using System;
using System.Collections.Generic;
using System.Management;
using System.Management.Automation;

namespace GUIForDiskpart.main
{
    public static class PartitionRetriever
    {
        public static void GetAndAddWMIPartitionsToDisk(ManagementObject disk, DiskInfo physicalDisk)
        {
            var partitionQueryText = string.Format("associators of {{{0}}} where AssocClass = Win32_DiskDriveToDiskPartition", disk.Path.RelativePath);
            var partitionQuery = new ManagementObjectSearcher(partitionQueryText);

            foreach (ManagementObject partitionManagementObject in partitionQuery.Get())
            {
                WMIPartition newPartitionInfo = RetrieveWMIPartitions(partitionManagementObject, physicalDisk.DiskIndex);

                LogicalDiskRetriever.GetAndAddLogicalDisks(partitionManagementObject, newPartitionInfo);

                physicalDisk.AddPartitionToList(newPartitionInfo);
            }
        }

        public static void GetAndAddWSMPartitionToDisk(DiskInfo disk)
        {
            List<WSMPartition> wsmPartitions = new List<WSMPartition>();

            foreach (PSObject psObject in GetWSMPartitionsByDisk(disk))
            {
                WSMPartition newWSMPartition = new WSMPartition();

                newWSMPartition.DiskNumber = Convert.ToUInt32(psObject.Properties["DiskNumber"].Value);
                newWSMPartition.PartitionNumber = Convert.ToUInt32(psObject.Properties["PartitionNumber"].Value);
                newWSMPartition.DriveLetter = Convert.ToChar(psObject.Properties["DriveLetter"].Value);
                newWSMPartition.OperationalStatus = Convert.ToUInt16(psObject.Properties["OperationalStatus"].Value);
                newWSMPartition.TransitionState = Convert.ToUInt16(psObject.Properties["TransitionState"].Value);
                newWSMPartition.Size = Convert.ToUInt64(psObject.Properties["Size"].Value);
                newWSMPartition.MBRType = Convert.ToUInt16(psObject.Properties["MbrType"].Value);
                newWSMPartition.GPTType = Convert.ToString(psObject.Properties["GptType"].Value);
                newWSMPartition.IsReadOnly = Convert.ToBoolean(psObject.Properties["IsReadOnly"].Value);
                newWSMPartition.IsOffline = Convert.ToBoolean(psObject.Properties["IsOffline"].Value);
                newWSMPartition.IsSystem = Convert.ToBoolean(psObject.Properties["IsSystem"].Value);
                newWSMPartition.IsBoot = Convert.ToBoolean(psObject.Properties["IsBoot"].Value);
                newWSMPartition.IsActive = Convert.ToBoolean(psObject.Properties["IsActive"].Value);
                newWSMPartition.IsHidden = Convert.ToBoolean(psObject.Properties["IsHidden"].Value);
                newWSMPartition.IsShadowCopy = Convert.ToBoolean(psObject.Properties["IsShadowCopy"].Value);
                newWSMPartition.NoDefaultDriveLetter = Convert.ToBoolean(psObject.Properties["NoDefaultDriveLetter"].Value);

                newWSMPartition.PrintToConsole();

                disk.WSMPartitions.Add(newWSMPartition);
            }
        }

        private static WMIPartition RetrieveWMIPartitions(ManagementObject partition, uint diskIndex)
        {
            WMIPartition newPartition = new WMIPartition();

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

        private static UInt32? GetWSMPartitionNumber(DiskInfo physicalDisk, WMIPartition newPartition)
        {
            foreach (PSObject psObject in GetWSMPartitionsByDisk(physicalDisk))
            {
                UInt16 mbrType = Convert.ToUInt16(psObject.Properties["MbrType"].Value);
                string? gptType = Convert.ToString(psObject.Properties["GptType"].Value);

                Console.WriteLine($"mbrType: {mbrType} gptType: {gptType}");

                if ((UInt64)psObject.Properties["Offset"].Value == newPartition.StartingOffset)
                {
                    return (UInt32)psObject.Properties["PartitionNumber"].Value;
                }
            }
            return null;
        }

        private static List<object> GetWSMPartitionsByDisk(DiskInfo diskInfo)
        {
            string diskIndex = Convert.ToString(diskInfo.DiskIndex);

            return CommandExecuter.IssuePowershellCommand("Get-Partition", diskIndex);
        }
    }
}
