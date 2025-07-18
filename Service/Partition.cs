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
            List<WSMModel> wsmPartitions = GetAllWSMPartitions(diskModel);
            List<WMIModel> wmiPartitions = GetAllWMIPartitions(disk, diskModel);

            foreach (WSMModel wsmPart in wsmPartitions)
            {
                PartitionModel partition = new PartitionModel();
                partition.WSM = wsmPart;
                partition.AssignedDiskModel = diskModel;

                foreach (WMIModel wmiPart in wmiPartitions)
                {
                    if (wmiPart.StartingOffset == wsmPart.Offset)
                    {
                        partition.WMI = wmiPart;
                    }
                }

                partitions.Add(partition);
            }

            return partitions;
        }

        private static List<WSMModel> GetAllWSMPartitions(DiskModel diskModel)
        {
            List<WSMModel> wsmPartitions = new List<WSMModel>();

            foreach (PSObject psObject in partitionRetriever.WSMPartitionQuery(diskModel))
            {
                wsmPartitions.Add(GetWSMPartition(psObject));
            }

            return wsmPartitions;
        }

        private static List<WMIModel> GetAllWMIPartitions(ManagementObject disk, DiskModel diskModel)
        {
            List<WMIModel> wmiPartitions = new List<WMIModel>();

            partitionRetriever.WMIPartitionQuery(disk, diskModel, ref wmiPartitions);

            return wmiPartitions;
        }

        private static WSMModel GetWSMPartition(PSObject psObject)
        {
            WSMModel wsmPartition = new WSMModel();

            wsmPartition.DiskNumber = System.Convert.ToUInt32(psObject.Properties["DiskNumber"].Value);
            wsmPartition.PartitionNumber = System.Convert.ToUInt32(psObject.Properties["PartitionNumber"].Value);
            wsmPartition.DriveLetter = System.Convert.ToChar(psObject.Properties["DriveLetter"].Value);
            wsmPartition.OperationalStatus = System.Convert.ToUInt16(psObject.Properties["OperationalStatus"].Value);
            wsmPartition.TransitionState = System.Convert.ToUInt16(psObject.Properties["TransitionState"].Value);
            wsmPartition.Size = System.Convert.ToUInt64(psObject.Properties["Size"].Value);
            wsmPartition.MBRType = System.Convert.ToUInt16(psObject.Properties["MbrType"].Value);
            wsmPartition.GPTType = System.Convert.ToString(psObject.Properties["GptType"].Value);
            wsmPartition.IsReadOnly = System.Convert.ToBoolean(psObject.Properties["IsReadOnly"].Value);
            wsmPartition.IsOffline = System.Convert.ToBoolean(psObject.Properties["IsOffline"].Value);
            wsmPartition.IsSystem = System.Convert.ToBoolean(psObject.Properties["IsSystem"].Value);
            wsmPartition.IsBoot = System.Convert.ToBoolean(psObject.Properties["IsBoot"].Value);
            wsmPartition.IsActive = System.Convert.ToBoolean(psObject.Properties["IsActive"].Value);
            wsmPartition.IsHidden = System.Convert.ToBoolean(psObject.Properties["IsHidden"].Value);
            wsmPartition.IsShadowCopy = System.Convert.ToBoolean(psObject.Properties["IsShadowCopy"].Value);
            wsmPartition.NoDefaultDriveLetter = System.Convert.ToBoolean(psObject.Properties["NoDefaultDriveLetter"].Value);
            wsmPartition.Offset = System.Convert.ToUInt64(psObject.Members["Offset"].Value);

            wsmPartition.PrintToConsole();

            return wsmPartition;
        }

        public static WMIModel GetWMIPartition(ManagementObject partition, uint diskIndex)
        {
            WMIModel newPartition = new WMIModel();

            newPartition.Availability = System.Convert.ToUInt16(partition.Properties["Availability"].Value);
            newPartition.StatusInfo = System.Convert.ToUInt16(partition.Properties["StatusInfo"].Value);
            newPartition.BlockSize = System.Convert.ToUInt64(partition.Properties["BlockSize"].Value);
            newPartition.Bootable = System.Convert.ToBoolean(partition.Properties["Bootable"].Value);
            newPartition.BootPartition = System.Convert.ToBoolean(partition.Properties["BootPartition"].Value);
            newPartition.Caption = System.Convert.ToString(partition.Properties["Caption"].Value);

            newPartition.ConfigManagerErrorCode = System.Convert.ToUInt32(partition.Properties["ConfigManagerErrorCode"].Value);
            newPartition.ConfigManagerUserConfig = System.Convert.ToBoolean(partition.Properties["ConfigManagerUserConfig"].Value);
            newPartition.CreationClassName = System.Convert.ToString(partition.Properties["CreationClassName"].Value);
            newPartition.Description = System.Convert.ToString(partition.Properties["Description"].Value);
            newPartition.DeviceID = System.Convert.ToString(partition.Properties["DeviceID"].Value);
            newPartition.DiskIndex = System.Convert.ToUInt32(partition.Properties["DiskIndex"].Value);

            newPartition.ErrorCleared = System.Convert.ToBoolean(partition.Properties["ErrorCleared"].Value);
            newPartition.ErrorDescription = System.Convert.ToString(partition.Properties["ErrorDescription"].Value);
            newPartition.ErrorMethodology = System.Convert.ToString(partition.Properties["ErrorMethodology"].Value);

            newPartition.HiddenSectors = System.Convert.ToUInt32(partition.Properties["ConfigManagerErrorCode"].Value);
            newPartition.PartitionIndex = System.Convert.ToUInt32(partition.Properties["Index"].Value);

            newPartition.InstallDate = System.Convert.ToDateTime(partition.Properties["InstallDate"].Value);
            newPartition.LastErrorCode = System.Convert.ToUInt32(partition.Properties["LastErrorCode"].Value);
            newPartition.PartitionName = System.Convert.ToString(partition.Properties["Name"].Value);
            newPartition.NumberOfBlocks = System.Convert.ToUInt64(partition.Properties["NumberOfBlocks"].Value);

            newPartition.PNPDeviceID = System.Convert.ToString(partition.Properties["PNPDeviceID"].Value);
            newPartition.PowerManagementSupported = System.Convert.ToBoolean(partition.Properties["PowerManagementSupported"].Value);
            newPartition.PrimaryPartition = System.Convert.ToBoolean(partition.Properties["PrimaryPartition"].Value);
            newPartition.Purpose = System.Convert.ToString(partition.Properties["Purpose"].Value);
            newPartition.RewritePartition = System.Convert.ToBoolean(partition.Properties["RewritePartition"].Value);
            newPartition.Size = System.Convert.ToUInt64(partition.Properties["Size"].Value);
            newPartition.StartingOffset = System.Convert.ToUInt64(partition.Properties["StartingOffset"].Value);
            newPartition.Status = System.Convert.ToString(partition.Properties["Status"].Value);
            newPartition.SystemCreationClassName = System.Convert.ToString(partition.Properties["SystemCreationClassName"].Value);
            newPartition.SystemName = System.Convert.ToString(partition.Properties["SystemName"].Value);
            newPartition.Type = System.Convert.ToString(partition.Properties["Type"].Value);

            return newPartition;
        }
    }
}
