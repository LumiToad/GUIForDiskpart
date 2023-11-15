using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Reflection;

namespace GUIForDiskpart.main
{
    public class DiskInfo
    {
        private const string wmiInfoKey = "---WINDOWS MANAGEMENT INSTRUMENTATION INFO---";
        private const string wmiInfoValue = "---Win32_DiskPartition---";
        private const string wmiKeyPrefix = "WMI";
        private const string msftInfoKey = "---WINDOWS STORAGE MANAGEMENT INFO---";
        private const string msftInfoValue = "---WMI > MSFT_Disk / MSFT_PhysicalDisk---";
        private const string msftKeyPrefix = "WSM-MSFT";

        private string deviceID;
        private string physicalName;
        private string diskModel;
        private string mediaStatus;
        private UInt16[] operationalStatusValues;
        private bool mediaLoaded;
        private UInt64 totalSpace;
        private string interfaceType;
        private UInt32 mediaSignature;
        private string caption;
        private string mediaType;
        private UInt16? msftMediaType;
        private UInt16 availability;
        private UInt32 bytesPerSector;
        private string compressionMethod;
        private UInt32 configManagerErrorCode;
        private bool configManagerUserConfig;
        private string creationClassName;
        private UInt64 defaultBlockSize;
        private string description;
        private bool errorCleared;
        private string errorDescription;
        private string errorMethodology;
        private string firmwareRevision;
        private UInt32 diskIndex;
        private DateTime installDate;
        private UInt32 lastErrorCode;
        private string manufacturer;
        private UInt64 maxBlockSize;
        private UInt64 maxMediaSize;
        private UInt64 minBlockSize;
        private bool needsCleaning;
        private UInt32 numberOfMediaSupported;
        private string pnpDeviceID;
        private bool powerManagementSupported;
        private UInt32 scsiBus;
        private UInt16 scsiLogicalUnit;
        private UInt16 scsiPort;
        private UInt16 scsiTargetId;
        private UInt32 sectorsPerTrack;
        private string serialNumber;
        private UInt16 statusInfo;
        private string systemCreationClassName;
        private string systemName;
        private UInt64 totalCylinders;
        private UInt32 totalHeads;
        private UInt64 totalSectors;
        private UInt64 totalTracks;
        private UInt32 tracksPerCylinder;

        public string DeviceMediaType => GetDeviceMediaType();
        public string OperationalStatus => GetOperationalStatus(operationalStatusValues);
        public bool IsOnline => OperationalStatus.Contains("Online");
        public string DeviceID { get => deviceID; set => deviceID = value; }
        public string PhysicalName { get => physicalName; set => physicalName = value; }
        public string DiskModel { get => diskModel; set => diskModel = value; }
        public string MediaStatus { get => mediaStatus; set => mediaStatus = value; }
        public UInt16[] OperationalStatusValues { get => operationalStatusValues; set => operationalStatusValues = value; }
        public bool MediaLoaded { get => mediaLoaded; set => mediaLoaded = value; }
        public UInt64 TotalSpace { get => totalSpace; set => totalSpace = value; }
        public Int64 UnallocatedSpace => CalcUnallocatedSpace();
        public int PartitionCount => Partitions.Count;
        public string InterfaceType { get => interfaceType; set => interfaceType = value; }
        public UInt32 MediaSignature { get => mediaSignature; set => mediaSignature = value; }
        public string Caption { get => caption; set => caption = value; }
        public string MediaType { get => mediaType; set => mediaType = value; }
        public UInt16? MSFTMediaType { get => msftMediaType; set => msftMediaType = value; }
        public UInt16 Availability { get => availability; set => availability = value; }
        public UInt32 BytesPerSector { get => bytesPerSector; set => bytesPerSector = value; }
        public string CompressionMethod { get => compressionMethod; set => compressionMethod = value; }
        public UInt32 ConfigManagerErrorCode { get => configManagerErrorCode; set => configManagerErrorCode = value; }
        public bool ConfigManagerUserConfig { get => configManagerUserConfig; set => configManagerUserConfig = value; }
        public string CreationClassName { get => creationClassName; set => creationClassName = value; }
        public UInt64 DefaultBlockSize { get => defaultBlockSize; set => defaultBlockSize = value; }
        public string Description { get => description; set => description = value; }
        public bool ErrorCleared { get => errorCleared; set => errorCleared = value; }
        public string ErrorDescription { get => errorDescription; set => errorDescription = value; }
        public string ErrorMethodology { get => errorMethodology; set => errorMethodology = value; }
        public string FirmwareRevision { get => firmwareRevision; set => firmwareRevision = value; }
        public UInt32 DiskIndex { get => diskIndex; set => diskIndex = value; }
        public DateTime InstallDate { get => installDate; set => installDate = value; }
        public UInt32 LastErrorCode { get => lastErrorCode; set => lastErrorCode = value; }
        public string Manufacturer { get => manufacturer; set => manufacturer = value; }
        public UInt64 MaxBlockSize { get => maxBlockSize; set => maxBlockSize = value; }
        public UInt64 MaxMediaSize { get => maxMediaSize; set => maxMediaSize = value; }
        public UInt64 MinBlockSize { get => minBlockSize; set => minBlockSize = value; }
        public bool NeedsCleaning { get => needsCleaning; set => needsCleaning = value; }
        public UInt32 NumberOfMediaSupported { get => numberOfMediaSupported; set => numberOfMediaSupported = value; }
        public string PNPDeviceID { get => pnpDeviceID; set => pnpDeviceID = value; }
        public bool PowerManagementSupported { get => powerManagementSupported; set => powerManagementSupported = value; }
        public UInt32 SCSIBus { get => scsiBus; set => scsiBus = value; }
        public UInt16 SCSILogicalUnit { get => scsiLogicalUnit; set => scsiLogicalUnit = value; }
        public UInt16 SCSIPort { get => scsiPort; set => scsiPort = value; }
        public UInt16 SCSITargetID { get => scsiTargetId; set => scsiTargetId = value; }
        public UInt32 SectorsPerTrack { get => sectorsPerTrack; set => sectorsPerTrack = value; }
        public string SerialNumber { get => serialNumber; set => serialNumber = value; }
        public UInt16 StatusInfo { get => statusInfo; set => statusInfo = value; }
        public string SystemCreationClassName { get => systemCreationClassName; set => systemCreationClassName = value; }
        public string SystemName { get => systemName; set => systemName = value; }
        public UInt64 TotalCylinders { get => totalCylinders; set => totalCylinders = value; }
        public UInt32 TotalHeads { get => totalHeads; set => totalHeads = value; }
        public UInt64 TotalSectors { get => totalSectors; set => totalSectors = value; }
        public UInt64 TotalTracks { get => totalTracks; set => totalTracks = value; }
        public UInt32 TracksPerCylinder { get => tracksPerCylinder; set => tracksPerCylinder = value; }
        public UInt64 FreeSpace { get { return GetLogicalFreeSpace(); } }
        public UInt64 UsedSpace { get { return (TotalSpace - Convert.ToUInt64(UnallocatedSpace)) - FreeSpace; } }
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

            fullOutput += "HardwareDeviceID: " + DeviceID + '\n';
            fullOutput += "PhysicalName: " + PhysicalName + '\n';
            fullOutput += "DiskModel: " + DiskModel + '\n';
            fullOutput += "MediaStatus: " + MediaStatus + '\n';
            fullOutput += "MediaLoaded: " + MediaLoaded + '\n';
            fullOutput += $"TotalSpace: {ByteFormatter.FormatBytes(TotalSpace)} {ByteFormatter.GetBytesAsStringAndUnit(TotalSpace)}\n";
            fullOutput += $"UnpartSpace: {ByteFormatter.FormatBytes(UnallocatedSpace)} {ByteFormatter.GetBytesAsStringAndUnit(UnallocatedSpace)}\n";
            fullOutput += "WSMPartitionCount: " + PartitionCount + " (includes hidden and reserved)" + '\n';
            fullOutput += "InterfaceType: " + InterfaceType + '\n';
            fullOutput += "MediaSignature: " + MediaSignature + '\n';
            fullOutput += "Caption: " + Caption + '\n';
            fullOutput += "MediaType: " + MediaType + '\n';
            fullOutput += "MSFTMediaType" + MSFTMediaType + '\n';
            fullOutput += "Avaiability: " + Availability + '\n';
            fullOutput += "BytesPerSector: " + BytesPerSector + '\n';
            fullOutput += "CompressionMethod: " + CompressionMethod + '\n';
            fullOutput += "ConfigManagerErrorCode: " + ConfigManagerErrorCode + '\n';
            fullOutput += "ConfigManagerUserConfig: " + ConfigManagerUserConfig + '\n';
            fullOutput += "CreationClassName: " + CreationClassName + '\n';
            fullOutput += "DefaultBlockSize: " + DefaultBlockSize + '\n';
            fullOutput += "Description: " + Description + '\n';
            fullOutput += "ErrorCleared: " + ErrorCleared + '\n';
            fullOutput += "ErrorDescription: " + ErrorDescription + '\n';
            fullOutput += "ErrorMethodology: " + ErrorMethodology + '\n';
            fullOutput += "FirmwareRevision: " + FirmwareRevision + '\n';
            fullOutput += "DiskIndex: " + DiskIndex + '\n';
            fullOutput += "InstallDate: " + InstallDate + '\n';
            fullOutput += "LastErrorCode: " + LastErrorCode + '\n';
            fullOutput += "Manufacturer: " + Manufacturer + '\n';
            fullOutput += "MaxBlockSize: " + MaxBlockSize + '\n';
            fullOutput += "MinBlockSize: " + MinBlockSize + '\n';
            fullOutput += "MaxMediaSize: " + MaxMediaSize + '\n';
            fullOutput += "NeedsCleaning: " + NeedsCleaning + '\n';
            fullOutput += "NumberOfMediaSupported: " + NumberOfMediaSupported + '\n';
            fullOutput += "PNPDeviceID: " + PNPDeviceID + '\n';
            fullOutput += "PowerManagementSupported: " + PowerManagementSupported + '\n';
            fullOutput += "SCSIBus: " + SCSIBus + '\n';
            fullOutput += "SCSILogicalUnit: " + SCSILogicalUnit + '\n';
            fullOutput += "SCSIPort: " + SCSIPort + '\n';
            fullOutput += "SCSITargetID: " + SCSITargetID + '\n';
            fullOutput += "SectorsPerTrack: " + SectorsPerTrack + '\n';
            fullOutput += "SerialNumber: " + SerialNumber + '\n';
            fullOutput += "StatusInfo: " + StatusInfo + '\n';
            fullOutput += "SystemCreationClassName: " + SystemCreationClassName + '\n';
            fullOutput += "SystemName: " + SystemName + '\n';
            fullOutput += "TotalCylinders: " + TotalCylinders + '\n';
            fullOutput += "TotalHeads: " + TotalHeads + '\n';
            fullOutput += "TotalSectors: " + TotalSectors + '\n';
            fullOutput += "TotalTracks: " + TotalTracks + '\n';
            fullOutput += "TracksPerCylinder: " + TracksPerCylinder + '\n';

            fullOutput += "_________________" + '\n';

            foreach (Partition partition in Partitions)
            {
                fullOutput += partition.GetOutputAsString();
            }

            return fullOutput;
        }

        private Int64 CalcUnallocatedSpace()
        {
            Int64 result = Convert.ToInt64(TotalSpace);

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

            data.Add(msftInfoKey, msftInfoValue);
            data.Add($"{msftKeyPrefix} DeviceMediaType", DeviceMediaType);
            data.Add($"{msftKeyPrefix} OperationalStatus", OperationalStatus);
            data.Add($"{msftKeyPrefix} Online / Offline", (IsOnline) ? "Online" : "Offline");

            data.Add(wmiInfoKey, wmiInfoValue);

            foreach (PropertyInfo property in properties)
            {
                string key = $"{wmiKeyPrefix} {property.Name}";
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

        private UInt64 GetLogicalFreeSpace()
        {
            UInt64 freeSpaceResult = new UInt64();

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
                case (null):
                    result = "Could not resolve... (Virtual Disk or device name is insufficient)";
                    break;
                case (0):
                    result = "Unspecified";
                    break;
                case (3):
                    result = "HDD (Hard Disk Drive)";
                    break;
                case (4):
                    result = "SSD (Solid-State Drive)";
                    break;
                case (5):
                    result = "SCM (Storage Class Memory)";
                    break;
            }
            return result;
        }

        private string GetOperationalStatus(UInt16[] operationalStatus)
        {
            string result = string.Empty;

            foreach (UInt16 status in operationalStatus)
            {
                switch (status)
                {
                    case (0):
                        result += "Unknown";
                        break;
                    case (1):
                        result += "Other";
                        break;
                    case (2):
                        result += "OK";
                        break;
                    case (3):
                        result += "Degraded";
                        break;
                    case (4):
                        result += "Stressed";
                        break;
                    case (5):
                        result += "Predictive Failure";
                        break;
                    case (6):
                        result += "Error";
                        break;
                    case (7):
                        result += "Non-Recoverable Error";
                        break;
                    case (8):
                        result += "Starting";
                        break;
                    case (9):
                        result += "Stopping";
                        break;
                    case (10):
                        result += "Stopped";
                        break;
                    case (11):
                        result += "In Service";
                        break;
                    case (12):
                        result += "No Contact";
                        break;
                    case (13):
                        result += "Lost Communication";
                        break;
                    case (14):
                        result += "Aborted";
                        break;
                    case (15):
                        result += "Dormant";
                        break;
                    case (16):
                        result += "Supporting Entity in Error";
                        break;
                    case (17):
                        result += "Completed";
                        break;
                    case (0xD010):
                        result += "Online";
                        break;
                    case (0xD011):
                        result += "Not Ready";
                        break;
                    case (0xD012):
                        result += "No Media";
                        break;
                    case (0xD013):
                        result += "Offline";
                        break;
                    case (0xD014):
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
