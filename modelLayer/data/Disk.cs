using System;
using System.Collections.Generic;
using System.Reflection;
using GUIForDiskpart.Service;

namespace GUIForDiskpart.Model.Data
{
    public class Disk
    {
        private const string WMI_INFO_KEY = "---WINDOWS MANAGEMENT INSTRUMENTATION INFO---";
        private const string WMI_INFO_VALUE = "---Win32_DiskPartition---";
        private const string WMI_KEY_PREFIX = "WMI";
        private const string MSFT_INFO_KEY = "---WINDOWS STORAGE MANAGEMENT INFO---";
        private const string MSFT_INFO_VALUE = "---WMI > MSFT_Disk / MSFT_PhysicalDisk---";
        private const string MSFT_KEY_PREFIX = "WSM-MSFT";

        private string deviceID;
        private string physicalName;
        private string diskModel;
        private string mediaStatus;
        private ushort[] operationalStatusValues;
        private bool mediaLoaded;
        private ulong totalSpace;
        private string interfaceType;
        private uint mediaSignature;
        private string caption;
        private string mediaType;
        private ushort? msftMediaType;
        private ushort availability;
        private uint bytesPerSector;
        private string compressionMethod;
        private uint configManagerErrorCode;
        private bool configManagerUserConfig;
        private string creationClassName;
        private ulong defaultBlockSize;
        private string description;
        private bool errorCleared;
        private string errorDescription;
        private string errorMethodology;
        private string firmwareRevision;
        private uint diskIndex;
        private DateTime installDate;
        private uint lastErrorCode;
        private string manufacturer;
        private ulong maxBlockSize;
        private ulong maxMediaSize;
        private ulong minBlockSize;
        private bool needsCleaning;
        private uint numberOfMediaSupported;
        private string pnpDeviceID;
        private bool powerManagementSupported;
        private uint scsiBus;
        private ushort scsiLogicalUnit;
        private ushort scsiPort;
        private ushort scsiTargetId;
        private uint sectorsPerTrack;
        private string serialNumber;
        private ushort statusInfo;
        private string systemCreationClassName;
        private string systemName;
        private ulong totalCylinders;
        private uint totalHeads;
        private ulong totalSectors;
        private ulong totalTracks;
        private uint tracksPerCylinder;

        public string DeviceMediaType => GetDeviceMediaType();
        public string OperationalStatus => GetOperationalStatus(operationalStatusValues);
        public bool IsOnline => OperationalStatus.Contains("Online");
        public string DeviceID { get => deviceID; set => deviceID = value; }
        public string PhysicalName { get => physicalName; set => physicalName = value; }
        public string DiskModel { get => diskModel; set => diskModel = value; }
        public string MediaStatus { get => mediaStatus; set => mediaStatus = value; }
        public ushort[] OperationalStatusValues { get => operationalStatusValues; set => operationalStatusValues = value; }
        public bool MediaLoaded { get => mediaLoaded; set => mediaLoaded = value; }
        public ulong TotalSpace { get => totalSpace; set => totalSpace = value; }
        public long UnallocatedSpace => CalcUnallocatedSpace();
        public int PartitionCount => Partitions.Count;
        public string InterfaceType { get => interfaceType; set => interfaceType = value; }
        public uint MediaSignature { get => mediaSignature; set => mediaSignature = value; }
        public string Caption { get => caption; set => caption = value; }
        public string MediaType { get => mediaType; set => mediaType = value; }
        public ushort? MSFTMediaType { get => msftMediaType; set => msftMediaType = value; }
        public ushort Availability { get => availability; set => availability = value; }
        public uint BytesPerSector { get => bytesPerSector; set => bytesPerSector = value; }
        public string CompressionMethod { get => compressionMethod; set => compressionMethod = value; }
        public uint ConfigManagerErrorCode { get => configManagerErrorCode; set => configManagerErrorCode = value; }
        public bool ConfigManagerUserConfig { get => configManagerUserConfig; set => configManagerUserConfig = value; }
        public string CreationClassName { get => creationClassName; set => creationClassName = value; }
        public ulong DefaultBlockSize { get => defaultBlockSize; set => defaultBlockSize = value; }
        public string Description { get => description; set => description = value; }
        public bool ErrorCleared { get => errorCleared; set => errorCleared = value; }
        public string ErrorDescription { get => errorDescription; set => errorDescription = value; }
        public string ErrorMethodology { get => errorMethodology; set => errorMethodology = value; }
        public string FirmwareRevision { get => firmwareRevision; set => firmwareRevision = value; }
        public uint DiskIndex { get => diskIndex; set => diskIndex = value; }
        public DateTime InstallDate { get => installDate; set => installDate = value; }
        public uint LastErrorCode { get => lastErrorCode; set => lastErrorCode = value; }
        public string Manufacturer { get => manufacturer; set => manufacturer = value; }
        public ulong MaxBlockSize { get => maxBlockSize; set => maxBlockSize = value; }
        public ulong MaxMediaSize { get => maxMediaSize; set => maxMediaSize = value; }
        public ulong MinBlockSize { get => minBlockSize; set => minBlockSize = value; }
        public bool NeedsCleaning { get => needsCleaning; set => needsCleaning = value; }
        public uint NumberOfMediaSupported { get => numberOfMediaSupported; set => numberOfMediaSupported = value; }
        public string PNPDeviceID { get => pnpDeviceID; set => pnpDeviceID = value; }
        public bool PowerManagementSupported { get => powerManagementSupported; set => powerManagementSupported = value; }
        public uint SCSIBus { get => scsiBus; set => scsiBus = value; }
        public ushort SCSILogicalUnit { get => scsiLogicalUnit; set => scsiLogicalUnit = value; }
        public ushort SCSIPort { get => scsiPort; set => scsiPort = value; }
        public ushort SCSITargetID { get => scsiTargetId; set => scsiTargetId = value; }
        public uint SectorsPerTrack { get => sectorsPerTrack; set => sectorsPerTrack = value; }
        public string SerialNumber { get => serialNumber; set => serialNumber = value; }
        public ushort StatusInfo { get => statusInfo; set => statusInfo = value; }
        public string SystemCreationClassName { get => systemCreationClassName; set => systemCreationClassName = value; }
        public string SystemName { get => systemName; set => systemName = value; }
        public ulong TotalCylinders { get => totalCylinders; set => totalCylinders = value; }
        public uint TotalHeads { get => totalHeads; set => totalHeads = value; }
        public ulong TotalSectors { get => totalSectors; set => totalSectors = value; }
        public ulong TotalTracks { get => totalTracks; set => totalTracks = value; }
        public uint TracksPerCylinder { get => tracksPerCylinder; set => tracksPerCylinder = value; }
        public ulong FreeSpace { get { return GetLogicalFreeSpace(); } }
        public ulong UsedSpace { get { return TotalSpace - Convert.ToUInt64(UnallocatedSpace) - FreeSpace; } }
        public string FormattedTotalSpace => ByteFormatter.FormatBytes(TotalSpace);
        public string FormattedFreeSpace => ByteFormatter.FormatBytes(GetLogicalFreeSpace());
        public string FormattedUsedSpace => ByteFormatter.FormatBytes(UsedSpace);
        public string FormattedUnallocatedSpace => ByteFormatter.FormatBytes(UnallocatedSpace);

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
            fullOutput += "DiskModel: " + "\t\t\t" + DiskModel + '\n';
            fullOutput += "MediaStatus: " + "\t\t\t" + MediaStatus + '\n';
            fullOutput += "MediaLoaded: " + "\t\t\t" + MediaLoaded + '\n';
            fullOutput += $"TotalSpace: {"\t\t\t"}{ByteFormatter.FormatBytes(TotalSpace)} {ByteFormatter.GetBytesAsStringAndUnit(TotalSpace)}\n";
            fullOutput += $"UnpartSpace: {"\t\t\t"}{ByteFormatter.FormatBytes(UnallocatedSpace)} {ByteFormatter.GetBytesAsStringAndUnit(UnallocatedSpace)}\n";
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
            long result = Convert.ToInt64(TotalSpace);

            foreach (Partition partition in Partitions)
            {
                result -= Convert.ToInt64(partition.WSMPartition.Size);
            }

            return result > 0 ? result : 0;
        }

        public Dictionary<string, object?> GetKeyValuePairs()
        {
            Dictionary<string, object?> data = new Dictionary<string, object?>();
            PropertyInfo[] properties = typeof(DiskInfo).GetProperties();

            data.Add(MSFT_INFO_KEY, MSFT_INFO_VALUE);
            data.Add($"{MSFT_KEY_PREFIX} DeviceMediaType", DeviceMediaType);
            data.Add($"{MSFT_KEY_PREFIX} OperationalStatus", OperationalStatus);
            data.Add($"{MSFT_KEY_PREFIX} Online / Offline", IsOnline ? "Online" : "Offline");

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
                    freeSpaceResult += partition.LogicalDiskInfo.FreeSpace;
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
                    result = "Could not resolve... (Virtual Disk or device name is insufficient)";
                    break;
                case 0:
                    result = "Unspecified";
                    break;
                case 3:
                    result = "HDD (Hard Disk Drive)";
                    break;
                case 4:
                    result = "SSD (Solid-State Drive)";
                    break;
                case 5:
                    result = "SCM (Storage Class Memory)";
                    break;
            }
            return result;
        }

        private string GetOperationalStatus(ushort[] operationalStatus)
        {
            string result = string.Empty;

            foreach (ushort status in operationalStatus)
            {
                switch (status)
                {
                    case 0:
                        result += "Unknown";
                        break;
                    case 1:
                        result += "Other";
                        break;
                    case 2:
                        result += "OK";
                        break;
                    case 3:
                        result += "Degraded";
                        break;
                    case 4:
                        result += "Stressed";
                        break;
                    case 5:
                        result += "Predictive Failure";
                        break;
                    case 6:
                        result += "Error";
                        break;
                    case 7:
                        result += "Non-Recoverable Error";
                        break;
                    case 8:
                        result += "Starting";
                        break;
                    case 9:
                        result += "Stopping";
                        break;
                    case 10:
                        result += "Stopped";
                        break;
                    case 11:
                        result += "In Service";
                        break;
                    case 12:
                        result += "No Contact";
                        break;
                    case 13:
                        result += "Lost Communication";
                        break;
                    case 14:
                        result += "Aborted";
                        break;
                    case 15:
                        result += "Dormant";
                        break;
                    case 16:
                        result += "Supporting Entity in Error";
                        break;
                    case 17:
                        result += "Completed";
                        break;
                    case 0xD010:
                        result += "Online";
                        break;
                    case 0xD011:
                        result += "Not Ready";
                        break;
                    case 0xD012:
                        result += "No Media";
                        break;
                    case 0xD013:
                        result += "Offline";
                        break;
                    case 0xD014:
                        result += "Failed";
                        break;
                }
                if (status == operationalStatus[operationalStatus.Length - 1]) continue;
                result += ", ";
            }

            return result;
        }
    }
}
