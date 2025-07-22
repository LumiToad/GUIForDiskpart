using System;
using System.Collections.Generic;
using System.Reflection;
using GUIForDiskpart.Database.Data;
using GUIForDiskpart.Utils;


namespace GUIForDiskpart.Model.Data
{
    public class Disk
    {
        #region EntryDataHeaders

        private const string WMI_INFO_KEY = "---WINDOWS MANAGEMENT INSTRUMENTATION INFO---";
        private const string WMI_INFO_VALUE = "---Win32_DiskPartition---";
        private const string WMI_KEY_PREFIX = "WMI";
        private const string MSFT_INFO_KEY = "---WINDOWS STORAGE MANAGEMENT INFO---";
        private const string MSFT_INFO_VALUE = "---WMI > MSFT_Disk / MSFT_PhysicalDisk---";
        private const string MSFT_KEY_PREFIX = "WSM-MSFT";
        
        #endregion EntryDataHeaders

        #region MSFT_OPERATIONALSTATUS

        private readonly string[] MSFT_OP_STATUS =
        {
            "Unknown",                      // 0
            "Other",                        // 1
            "OK",                           // 2
            "Degraded",                     // 3
            "Stressed",                     // 4
            "Predictive Failure",           // 5
            "Error",                        // 6
            "Non-Recoverable Error",        // 7
            "Starting",                     // 8
            "Stopping",                     // 9
            "Stopped",                      // 10
            "In Service",                   // 11
            "No Contact",                   // 12
            "Lost Communication",           // 13
            "Aborted",                      // 14
            "Dormant",                      // 15
            "Supporting Entity in Error",   // 16
            "Completed",                    // 17
            "Online",                       // 0xD011
            "Not Ready",                    // 0xD012
            "No Media",                     // 0xD013
            "Offline",                      // 0xD014
            "Failed",                       // 0xD015
        };

        #endregion MSFT_OPERATIONALSTATUS

        #region MSFT_MEDIATYPES

        private const string MSFT_MEDIATYPE_NULL = "Could not resolve... (Virtual Disk or device name is insufficient)";
        private const string MSFT_MEDIATYPE_0 = "Unspecified";
        private const string MSFT_MEDIATYPE_3 = "HDD (Hard Disk Drive)";
        private const string MSFT_MEDIATYPE_4 = "SSD (Solid-State Drive)";
        private const string MSFT_MEDIATYPE_5 = "SCM (Storage Class Memory)";

        #endregion MSFT_MEDIATYPES

        #region DiskProperties

        public string DeviceID { get; set; }
        public string PhysicalName { get; set; }
        public string DiskModelText { get; set; }
        public string MediaStatus { get; set; }
        public ushort[] OperationalStatusValues { get; set; }
        public bool MediaLoaded { get; set; }
        public ulong TotalSpace { get; set; }
        public string InterfaceType { get; set; }
        public uint MediaSignature { get; set; }
        public string Caption { get; set; }
        public string MediaType { get; set; }
        public ushort? MSFTMediaType { get; set; }
        public ushort Availability { get; set; }
        public uint BytesPerSector { get; set; }
        public string CompressionMethod { get; set; }
        public uint ConfigManagerErrorCode { get; set; }
        public bool ConfigManagerUserConfig { get; set; }
        public string CreationClassName { get; set; }
        public ulong DefaultBlockSize { get; set; }
        public string Description { get; set; }
        public bool ErrorCleared { get; set; }
        public string ErrorDescription { get; set; }
        public string ErrorMethodology { get; set; }
        public string FirmwareRevision { get; set; }
        public uint DiskIndex { get; set; }
        public DateTime InstallDate { get; set; }
        public uint LastErrorCode { get; set; }
        public string Manufacturer { get; set; }
        public ulong MaxBlockSize { get; set; }
        public ulong MaxMediaSize { get; set; }
        public ulong MinBlockSize { get; set; }
        public bool NeedsCleaning { get; set; }
        public uint NumberOfMediaSupported { get; set; }
        public string PNPDeviceID { get; set; }
        public bool PowerManagementSupported { get; set; }
        public uint SCSIBus { get; set; }
        public ushort SCSILogicalUnit { get; set; }
        public ushort SCSIPort { get; set; }
        public ushort SCSITargetID { get; set; }
        public uint SectorsPerTrack { get; set; }
        public string SerialNumber { get; set; }
        public ushort StatusInfo { get; set; }
        public string SystemCreationClassName { get; set; }
        public string SystemName { get; set; }
        public ulong TotalCylinders { get; set; }
        public uint TotalHeads { get; set; }
        public ulong TotalSectors { get; set; }
        public ulong TotalTracks { get; set; }
        public uint TracksPerCylinder { get; set; }
        public int PartitionCount => Partitions.Count;
        public string DeviceMediaType => GetDeviceMediaType();
        public string OperationalStatus => GetOperationalStatus(OperationalStatusValues);
        public bool IsOnline => OperationalStatus.Contains(CommonStrings.ONLINE);
        public long UnallocatedSpace => CalcUnallocatedSpace();
        public ulong FreeSpace { get { return GetLogicalFreeSpace(); } }
        public ulong UsedSpace { get { return TotalSpace - System.Convert.ToUInt64(UnallocatedSpace) - FreeSpace; } }
        public string FormattedTotalSpace => ByteFormatter.BytesToAsString(TotalSpace);
        public string FormattedFreeSpace => ByteFormatter.BytesToAsString(GetLogicalFreeSpace());
        public string FormattedUsedSpace => ByteFormatter.BytesToAsString(UsedSpace);
        public string FormattedUnallocatedSpace => ByteFormatter.BytesToAsString(UnallocatedSpace);

        #endregion DiskProperties

        private List<Partition> partitions = new List<Partition>();
        public List<Partition> Partitions
        { get { return partitions; } }

        public void PrintToConsole()
        {
            Console.WriteLine(GetOutputAsString());
        }

        public string GetOutputAsString()
        {
            string fullOutput = string.Empty;

            fullOutput += "___Physical-Info___" + "\n\n";
            fullOutput += "HardwareDeviceID: " + "\t\t\t" + DeviceID + '\n';
            fullOutput += "PhysicalName: " + "\t\t\t" + PhysicalName + '\n';
            fullOutput += "DiskModel: " + "\t\t\t" + DiskModelText + '\n';
            fullOutput += "MediaStatus: " + "\t\t\t" + MediaStatus + '\n';
            fullOutput += "MediaLoaded: " + "\t\t\t" + MediaLoaded + '\n';
            fullOutput += $"TotalSpace: {"\t\t\t"}{ByteFormatter.BytesToAsString(TotalSpace)} {ByteFormatter.BytesToAsString(TotalSpace, true, Unit.B)}\n";
            fullOutput += $"UnpartSpace: {"\t\t\t"}{ByteFormatter.BytesToAsString(UnallocatedSpace)} {ByteFormatter.BytesToAsString(UnallocatedSpace, true, Unit.B)}\n";
            fullOutput += "WSMPartitionCount: " + "\t\t\t" + PartitionCount + " (includes hidden and reserved)" + '\n';
            fullOutput += "InterfaceType: " + "\t\t\t" + InterfaceType + '\n';
            fullOutput += "MediaSignature: " + "\t\t\t" + MediaSignature + '\n';
            fullOutput += "Caption: " + "\t\t\t" + Caption + '\n';
            fullOutput += "MediaType: " + "\t\t\t" + MediaType + '\n';
            fullOutput += "MSFTMediaType" + "\t\t\t" + MSFTMediaType + '\n';
            fullOutput += "Avaiability: " + "\t\t\t" + Availability + '\n';
            fullOutput += "BytesPerSector: " + "\t\t\t" + BytesPerSector + '\n';
            fullOutput += "CompressionMethod: " + "\t\t\t" + CompressionMethod + '\n';
            fullOutput += "ConfigManagerErrorCode: " + "\t\t\t" + ConfigManagerErrorCode + '\n';
            fullOutput += "ConfigManagerUserConfig: " + "\t\t\t" + ConfigManagerUserConfig + '\n';
            fullOutput += "CreationClassName: " + "\t\t\t" + CreationClassName + '\n';
            fullOutput += "DefaultBlockSize: " + "\t\t\t" + DefaultBlockSize + '\n';
            fullOutput += "Description: " + "\t\t\t" + Description + '\n';
            fullOutput += "ErrorCleared: " + "\t\t\t" + ErrorCleared + '\n';
            fullOutput += "ErrorDescription: " + "\t\t\t" + ErrorDescription + '\n';
            fullOutput += "ErrorMethodology: " + "\t\t\t" + ErrorMethodology + '\n';
            fullOutput += "FirmwareRevision: " + "\t\t\t" + FirmwareRevision + '\n';
            fullOutput += "DiskIndex: " + "\t\t\t" + DiskIndex + '\n';
            fullOutput += "InstallDate: " + "\t\t\t" + InstallDate + '\n';
            fullOutput += "LastErrorCode: " + "\t\t\t" + LastErrorCode + '\n';
            fullOutput += "Manufacturer: " + "\t\t\t" + Manufacturer + '\n';
            fullOutput += "MaxBlockSize: " + "\t\t\t" + MaxBlockSize + '\n';
            fullOutput += "MinBlockSize: " + "\t\t\t" + MinBlockSize + '\n';
            fullOutput += "MaxMediaSize: " + "\t\t\t" + MaxMediaSize + '\n';
            fullOutput += "NeedsCleaning: " + "\t\t\t" + NeedsCleaning + '\n';
            fullOutput += "NumberOfMediaSupported: " + "\t\t\t" + NumberOfMediaSupported + '\n';
            fullOutput += "PNPDeviceID: " + "\t\t\t" + PNPDeviceID + '\n';
            fullOutput += "PowerManagementSupported: " + "\t\t\t" + PowerManagementSupported + '\n';
            fullOutput += "SCSIBus: " + "\t\t\t" + SCSIBus + '\n';
            fullOutput += "SCSILogicalUnit: " + "\t\t\t" + SCSILogicalUnit + '\n';
            fullOutput += "SCSIPort: " + "\t\t\t" + SCSIPort + '\n';
            fullOutput += "SCSITargetID: " + "\t\t\t" + SCSITargetID + '\n';
            fullOutput += "SectorsPerTrack: " + "\t\t\t" + SectorsPerTrack + '\n';
            fullOutput += "SerialNumber: " + "\t\t\t" + SerialNumber + '\n';
            fullOutput += "StatusInfo: " + "\t\t\t" + StatusInfo + '\n';
            fullOutput += "SystemCreationClassName: " + "\t\t\t" + SystemCreationClassName + '\n';
            fullOutput += "SystemName: " + "\t\t\t" + SystemName + '\n';
            fullOutput += "TotalCylinders: " + "\t\t\t" + TotalCylinders + '\n';
            fullOutput += "TotalHeads: " + "\t\t\t" + TotalHeads + '\n';
            fullOutput += "TotalSectors: " + "\t\t\t" + TotalSectors + '\n';
            fullOutput += "TotalTracks: " + "\t\t\t" + TotalTracks + '\n';
            fullOutput += "TracksPerCylinder: " + "\t\t\t" + TracksPerCylinder + '\n';

            fullOutput += "_________________" + "\n\n";

            foreach (Partition partition in Partitions)
            {
                fullOutput += partition.GetOutputAsString();
            }

            return fullOutput;
        }

        private long CalcUnallocatedSpace()
        {
            long result = System.Convert.ToInt64(TotalSpace);

            foreach (Partition partition in Partitions)
            {
                result -= System.Convert.ToInt64(partition.WSM.Size);
            }

            return result > 0 ? result : 0;
        }

        public Dictionary<string, object?> GetKeyValuePairs()
        {
            Dictionary<string, object?> data = new Dictionary<string, object?>();
            PropertyInfo[] properties = typeof(DiskModel).GetProperties();

            data.Add(MSFT_INFO_KEY, MSFT_INFO_VALUE);
            data.Add($"{MSFT_KEY_PREFIX} DeviceMediaType", DeviceMediaType);
            data.Add($"{MSFT_KEY_PREFIX} OperationalStatus", OperationalStatus);
            data.Add($"{MSFT_KEY_PREFIX} {CommonStrings.ONLINE} / {CommonStrings.OFFLINE}",
                IsOnline ? CommonStrings.ONLINE : CommonStrings.OFFLINE);

            data.Add(WMI_INFO_KEY, WMI_INFO_VALUE);

            foreach (PropertyInfo property in properties)
            {
                string key = $"{WMI_KEY_PREFIX} {property.Name}";
                object? value = property.GetValue(this);

                if (key.Contains("DeviceMediaType")) continue;
                if (key.Contains("OperationalStatus")) continue;
                if (key.Contains("OperationalStatusValues")) continue;
                if (key.Contains("IsOnline")) continue;
                if (key.Contains("MSFTMediaType")) continue;
                if (key.Contains("TotalSpace")) continue;
                if (key.Contains("UnallocatedSpace")) continue;
                if (key.Contains("FreeSpace")) continue;
                if (key.Contains("UsedSpace")) continue;

                if (typeof(List<Partition>) == property.PropertyType) continue;

                if (key.Contains("Formatted"))
                {
                    key = key.Replace("Formatted", "");
                }

                if (data.ContainsKey(key)) continue;
                data.Add(key, value);
            }

            return data;
        }

        private ulong GetLogicalFreeSpace()
        {
            ulong freeSpaceResult = new ulong();

            foreach (Partition partition in Partitions)
            {
                if (partition.HasWMIPartition && partition.IsLogicalDisk)
                {
                    freeSpaceResult += partition.LDModel.FreeSpace;
                }
            }

            return freeSpaceResult;
        }

        private string GetDeviceMediaType()
        {
            string result = string.Empty;

            switch (MSFTMediaType)
            {
                case null:
                    result = MSFT_MEDIATYPE_NULL;
                    break;
                case 0:
                    result = MSFT_MEDIATYPE_0;
                    break;
                case 3:
                    result = MSFT_MEDIATYPE_3;
                    break;
                case 4:
                    result = MSFT_MEDIATYPE_4;
                    break;
                case 5:
                    result = MSFT_MEDIATYPE_5;
                    break;
            }
            return result;
        }

        private string GetOperationalStatus(ushort[] operationalStatus)
        {
            string result = string.Empty;

            foreach (ushort status in operationalStatus)
            {
                if (status > 17)
                {
                    ushort calcStatus = 0xD010;
                    calcStatus = (ushort)(status - calcStatus);
                    calcStatus += 18;

                    result += MSFT_OP_STATUS[calcStatus];
                }
                else
                {
                    result += MSFT_OP_STATUS[status];
                }

                if (status == operationalStatus[operationalStatus.Length - 1]) continue;
                result += ", ";
            }

            return result;
        }
    }
}
