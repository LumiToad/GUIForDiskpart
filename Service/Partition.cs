using System;
using System.Collections.Generic;
using System.Management;
using System.Management.Automation;

using GUIForDiskpart.Model.Data;
using PartitionRetriever = GUIForDiskpart.Database.Retrievers.Partition;

namespace GUIForDiskpart.Service
{
    public static class Partition
    {
        private static PartitionRetriever partitionRetriever = new();

        public static List<PartitionModel> GetAllPartitions(ManagementObject disk, DiskModel diskModel)
        {
            List<PartitionModel> partitions = new List<PartitionModel>();
            List<WSMPartition> wsmPartitions = GetAllWSMPartitions(diskModel);
            List<WMIPartition> wmiPartitions = GetAllWMIPartitions(disk, diskModel);

            foreach (WSMPartition wsmPart in wsmPartitions)
            {
                PartitionModel partition = new PartitionModel();
                partition.WSMPartition = wsmPart;
                partition.AssignedDiskModel = diskModel;

                foreach (WMIPartition wmiPart in wmiPartitions)
                {
                    if (wmiPart.StartingOffset == wsmPart.Offset)
                    {
                        partition.WMIPartition = wmiPart;
                    }
                }

                partitions.Add(partition);
            }

            return partitions;
        }

        private static List<WSMPartition> GetAllWSMPartitions(DiskModel diskModel)
        {
            List<WSMPartition> wsmPartitions = new List<WSMPartition>();

            foreach (PSObject psObject in partitionRetriever.WSMPartitionQuery(diskModel))
            {
                wsmPartitions.Add(GetWSMPartition(psObject));
            }

            return wsmPartitions;
        }

        private static List<WMIPartition> GetAllWMIPartitions(ManagementObject disk, DiskModel diskModel)
        {
            List<WMIPartition> wmiPartitions = new List<WMIPartition>();

            partitionRetriever.WMIPartitionQuery(disk, diskModel, ref wmiPartitions);

            return wmiPartitions;
        }

        private static WSMPartition GetWSMPartition(PSObject psObject)
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

        public static WMIPartition GetWMIPartition(ManagementObject partition, uint diskIndex)
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
    }
}
