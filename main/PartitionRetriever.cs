using System;
using System.Collections.Generic;
using System.Management;
using System.Management.Automation;

namespace GUIForDiskpart.main
{
    public static class PartitionRetriever
    {
        /*
        public static void GetPartitionsAndAddToDisk(ManagementObject disk, DiskInfo diskInfo)
        {
            List<WSMPartition> wsmPartitions = GetWSMPartitions(diskInfo);
            List<WMIPartition> wmiPartitions = GetWMIPartitions(disk, diskInfo);

            foreach (WSMPartition wsmPart in wsmPartitions)
            {
                foreach (WMIPartition wmiPart in wmiPartitions)
                {
                    if (wmiPart.StartingOffset == wsmPart.Offset)
                    {
                        wsmPart.WMIPartition = wmiPart;
                    }
                }
            }

            foreach (WSMPartition wsmPart in wsmPartitions)
            {
                diskInfo.WSMPartitions.Add(wsmPart);
            }
        }
        */

        public static void GetPartitionsAndAddToDisk(ManagementObject disk, DiskInfo diskInfo)
        {
            List<Partition> partitions = new List<Partition>();
            List<WSMPartition> wsmPartitions = GetWSMPartitions(diskInfo);
            List<WMIPartition> wmiPartitions = GetWMIPartitions(disk, diskInfo);

            foreach (WSMPartition wsmPart in wsmPartitions)
            {
                Partition partition = new Partition();
                partition.WSMPartition = wsmPart;
                
                foreach (WMIPartition wmiPart in wmiPartitions)
                {
                    if (wmiPart.StartingOffset == wsmPart.Offset)
                    {
                        partition.WMIPartition = wmiPart;
                    }
                }
                diskInfo.Partitions.Add(partition);
            }
        }

        private static List<WSMPartition> GetWSMPartitions(DiskInfo diskInfo)
        {
            List<WSMPartition> wsmPartitions = new List<WSMPartition>();

            foreach (PSObject psObject in GetWSMPartitionsByDisk(diskInfo))
            {
                wsmPartitions.Add(RetrieveWSMPartition(psObject));
            }

            return wsmPartitions;
        }

        private static List<WMIPartition> GetWMIPartitions(ManagementObject disk, DiskInfo diskInfo)
        {
            List<WMIPartition> wmiPartitions = new List<WMIPartition>();

            var partitionQueryText = string.Format("associators of {{{0}}} where AssocClass = Win32_DiskDriveToDiskPartition", disk.Path.RelativePath);
            var partitionQuery = new ManagementObjectSearcher(partitionQueryText);

            foreach (ManagementObject partitionManagementObject in partitionQuery.Get())
            {
                WMIPartition wmiPartition = RetrieveWMIPartitions(partitionManagementObject, diskInfo.DiskIndex);

                LogicalDiskRetriever.GetAndAddLogicalDisks(partitionManagementObject, wmiPartition);

                wmiPartitions.Add(wmiPartition);
            }

            return wmiPartitions;
        }

        private static WSMPartition RetrieveWSMPartition(PSObject psObject)
        {
            WSMPartition wsmPartition = new WSMPartition();

            wsmPartition.DiskNumber = Convert.ToUInt32(psObject.Properties["DiskNumber"].Value);
            wsmPartition.PartitionNumber = Convert.ToUInt32(psObject.Properties["PartitionNumber"].Value);
            wsmPartition.DriveLetter = Convert.ToChar(psObject.Properties["DriveLetter"].Value);
            wsmPartition.OperationalStatus = Convert.ToUInt16(psObject.Properties["OperationalStatus"].Value);
            wsmPartition.TransitionState = Convert.ToUInt16(psObject.Properties["TransitionState"].Value);
            wsmPartition.Size = Convert.ToUInt64(psObject.Properties["Size"].Value);
            wsmPartition.MBRType = Convert.ToUInt16(psObject.Properties["MbrType"].Value);
            wsmPartition.GPTType = Convert.ToString(psObject.Properties["GptType"].Value);
            wsmPartition.IsReadOnly = Convert.ToBoolean(psObject.Properties["IsReadOnly"].Value);
            wsmPartition.IsOffline = Convert.ToBoolean(psObject.Properties["IsOffline"].Value);
            wsmPartition.IsSystem = Convert.ToBoolean(psObject.Properties["IsSystem"].Value);
            wsmPartition.IsBoot = Convert.ToBoolean(psObject.Properties["IsBoot"].Value);
            wsmPartition.IsActive = Convert.ToBoolean(psObject.Properties["IsActive"].Value);
            wsmPartition.IsHidden = Convert.ToBoolean(psObject.Properties["IsHidden"].Value);
            wsmPartition.IsShadowCopy = Convert.ToBoolean(psObject.Properties["IsShadowCopy"].Value);
            wsmPartition.NoDefaultDriveLetter = Convert.ToBoolean(psObject.Properties["NoDefaultDriveLetter"].Value);
            wsmPartition.Offset = Convert.ToUInt64(psObject.Members["Offset"].Value);

            wsmPartition.PrintToConsole();

            return wsmPartition;
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

        private static List<object> GetWSMPartitionsByDisk(DiskInfo diskInfo)
        {
            string diskIndex = Convert.ToString(diskInfo.DiskIndex);

            return CommandExecuter.IssuePowershellCommand("Get-Partition", diskIndex);
        }
    }
}
