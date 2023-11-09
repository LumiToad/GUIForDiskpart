using System;
using System.Collections.Generic;
using System.Reflection;

namespace GUIForDiskpart.main
{
    public class DiskInfo
    {
        private const string wmiInfoString = "---WINDOWS MANAGEMENT INSTRUMENTATION INFO---";

        readonly private string deviceID;
        public string DeviceId { get { return deviceID; } }

        readonly private string physicalName;
        public string PhysicalName { get { return physicalName; } }

        readonly private string diskName;
        public string DiskName { get { return diskName; } }

        readonly private string diskModel;
        public string DiskModel { get { return diskModel; } }

        readonly private string mediaStatus;
        public string MediaStatus { get { return mediaStatus; } }

        readonly private bool mediaLoaded;
        public bool MediaLoaded { get { return mediaLoaded; } }

        readonly private UInt64 totalSpace;
        public UInt64 TotalSpace { get { return totalSpace; } }

        public Int64 UnpartSpace => CalcUnpartSpace();

        readonly private UInt32 wmiPartitionCount;
        public UInt32 WMIPartitionCount { get { return wmiPartitionCount; } }

        public int WSMPartitionCount => wsmPartitions.Count;

        readonly private string interfaceType;
        public string InterfaceType { get { return interfaceType; } }

        readonly private UInt32 mediaSignature;
        public UInt32 MediaSignature { get { return mediaSignature; } }

        readonly private string caption;
        public string Caption { get { return caption; } }

        readonly private string mediaType;
        public string MediaType { get { return mediaType; } }

        readonly private UInt16 availability;
        public UInt16 Availability { get => availability;}

        readonly private UInt32 bytesPerSector;
        public UInt32 BytesPerSector { get => bytesPerSector; }

        readonly private string compressionMethod;
        public string CompressionMethod { get => compressionMethod; }

        readonly private UInt32 configManagerErrorCode;
        public UInt32 ConfigManagerErrorCode { get => configManagerErrorCode; }

        readonly private bool configManagerUserConfig;
        public bool ConfigManagerUserConfig { get => configManagerUserConfig; }

        readonly private string creationClassName;
        public string CreationClassName { get => creationClassName; }

        readonly private UInt64 defaultBlockSize;
        public UInt64 DefaultBlockSize { get => defaultBlockSize;}

        readonly private string description;
        public string Description { get => description; }

        readonly private bool errorCleared;
        public bool ErrorCleared { get => errorCleared; }

        readonly private string errorDescription;
        public string ErrorDescription { get => errorDescription; }

        readonly private string errorMethodology;
        public string ErrorMethodology { get => errorMethodology; }

        readonly private string firmwareRevision;
        public string FirmwareRevision { get => firmwareRevision; }

        readonly private UInt32 diskIndex;
        public UInt32 DiskIndex { get => diskIndex; }

        readonly private DateTime installDate;
        public DateTime InstallDate { get => installDate; }

        readonly private UInt32 lastErrorCode;
        public UInt32 LastErrorCode { get => lastErrorCode; }

        readonly private string manufacturer;
        public string Manufacturer { get => manufacturer; }

        readonly private UInt64 maxBlockSize;
        public UInt64 MaxBlockSize { get => maxBlockSize; }

        readonly private UInt64 maxMediaSize;
        public UInt64 MaxMediaSize { get => maxMediaSize; }

        readonly private UInt64 minBlockSize;
        public UInt64 MinBlockSize { get => minBlockSize; }

        readonly private bool needsCleaning;
        public bool NeedsCleaning { get => needsCleaning; }

        readonly private UInt32 numberOfMediaSupported;
        public UInt32 NumberOfMediaSupported { get => numberOfMediaSupported; }

        readonly private string pnpDeviceID;
        public string PNPDeviceID { get => pnpDeviceID; }

        readonly private bool powerManagementSupported;
        public bool PowerManagementSupported { get => powerManagementSupported; }

        readonly private UInt32 scsiBus;
        public UInt32 SCSIBus { get => scsiBus; }

        readonly private UInt16 scsiLogicalUnit;
        public UInt16 SCSILogicalUnit { get => scsiLogicalUnit; }

        readonly private UInt16 scsiPort;
        public UInt16 SCSIPort { get => scsiPort; }

        readonly private UInt16 scsiTargetId;
        public UInt16 SCSITargetID { get => scsiTargetId; }

        readonly private UInt32 sectorsPerTrack;
        public UInt32 SectorsPerTrack { get => sectorsPerTrack; }

        readonly private string serialNumber;
        public string SerialNumber { get => serialNumber; }

        readonly private UInt16 statusInfo;
        public UInt16 StatusInfo { get => statusInfo; }

        readonly private string systemCreationClassName;
        public string SystemCreationClassName { get => systemCreationClassName; }

        readonly private string systemName;
        public string SystemName { get => systemName; }

        readonly private UInt64 totalCylinders;
        public UInt64 TotalCylinders { get => totalCylinders; }

        readonly private UInt32 totalHeads;
        public UInt32 TotalHeads { get => totalHeads; }

        readonly private UInt64 totalSectors;
        public UInt64 TotalSectors { get => totalSectors; }

        readonly private UInt64 totalTracks;
        public UInt64 TotalTracks { get => totalTracks; }

        readonly private UInt32 tracksPerCylinder;
        public UInt32 TracksPerCylinder { get => tracksPerCylinder; }

        private List<WSMPartition> wsmPartitions = new List<WSMPartition>();
        public List<WSMPartition> WSMPartitions { get { return wsmPartitions; } set { wsmPartitions = value; } }

        public UInt64 FreeSpace { get { return GetLogicalFreeSpace(); } }
        public UInt64 UsedSpace { get { return TotalSpace - FreeSpace; } }
        
        public string FormattedFreeSpace => ByteFormatter.FormatBytes(GetLogicalFreeSpace());
        public string FormattedTotalSpace => ByteFormatter.FormatBytes(TotalSpace);
        public string FormattedUnpartSpace => ByteFormatter.FormatBytes(UnpartSpace);
        public string FormattedUsedSpace => ByteFormatter.FormatBytes(UsedSpace);

        public DiskInfo(
            string deviceID,
            string physicalName,
            string caption,
            string diskModel,
            string mediaStatus,
            bool mediaLoaded,
            ulong totalSpace,
            uint partitionCount,
            string interfaceType,
            uint mediaSignature,
            string diskName,
            string mediaType,
            ushort availability,
            uint bytesPerSector,
            string compressionMethod,
            uint configManagerErrorCode,
            bool configManagerUserConfig,
            string creationClassName,
            ulong defaultBlockSize,
            string description,
            bool errorCleared,
            string errorDescription,
            string errorMethodology,
            string firmwareRevision,
            uint index,
            DateTime installDate,
            uint lastErrorCode,
            string manufacturer,
            ulong maxBlockSize,
            ulong maxMediaSize,
            ulong minBlockSize,
            bool needsCleaning,
            uint numberOfMediaSupported,
            string pnpDeviceID,
            bool powerManagementSupported,
            uint scsiBus,
            ushort scsiLogicalUnit,
            ushort scsiPort,
            ushort scsiTargetId,
            uint sectorsPerTrack,
            string serialNumber,
            ushort statusInfo,
            string systemCreationClassName,
            string systemName,
            ulong totalCylinders,
            uint totalHeads,
            ulong totalSectors,
            ulong totalTracks,
            uint tracksPerCylinder)
        {
            this.deviceID = deviceID;
            this.physicalName = physicalName;
            this.caption = caption;
            this.diskModel = diskModel;
            this.mediaStatus = mediaStatus;
            this.mediaLoaded = mediaLoaded;
            this.totalSpace = totalSpace;
            this.wmiPartitionCount = partitionCount;
            this.interfaceType = interfaceType;
            this.mediaSignature = mediaSignature;
            this.diskName = diskName;
            this.mediaType = mediaType;
            this.availability = availability;
            this.bytesPerSector = bytesPerSector;
            this.compressionMethod = compressionMethod;
            this.configManagerErrorCode = configManagerErrorCode;
            this.configManagerUserConfig = configManagerUserConfig;
            this.creationClassName = creationClassName;
            this.defaultBlockSize = defaultBlockSize;
            this.description = description;
            this.errorCleared = errorCleared;
            this.errorDescription = errorDescription;
            this.errorMethodology = errorMethodology;
            this.firmwareRevision = firmwareRevision;
            this.diskIndex = index;
            this.installDate = installDate;
            this.lastErrorCode = lastErrorCode;
            this.manufacturer = manufacturer;
            this.maxBlockSize = maxBlockSize;
            this.maxMediaSize = maxMediaSize;
            this.minBlockSize = minBlockSize;
            this.needsCleaning = needsCleaning;
            this.numberOfMediaSupported = numberOfMediaSupported;
            this.pnpDeviceID = pnpDeviceID;
            this.powerManagementSupported = powerManagementSupported;
            this.scsiBus = scsiBus;
            this.scsiLogicalUnit = scsiLogicalUnit;
            this.scsiPort = scsiPort;
            this.scsiTargetId = scsiTargetId;
            this.sectorsPerTrack = sectorsPerTrack;
            this.serialNumber = serialNumber;
            this.statusInfo = statusInfo;
            this.systemCreationClassName = systemCreationClassName;
            this.systemName = systemName;
            this.totalCylinders = totalCylinders;
            this.totalHeads = totalHeads;
            this.totalSectors = totalSectors;
            this.totalTracks = totalTracks;
            this.tracksPerCylinder = tracksPerCylinder;
        }

        public void PrintToConsole()
        {
            Console.WriteLine(GetOutputAsString());
        }

        public string GetOutputAsString()
        {
            string output = string.Empty;

            output += "HardwareDeviceID: " + DeviceId + '\n';
            output += "PhysicalName: " + PhysicalName + '\n';
            output += "DiskName: " + DiskName + '\n';
            output += "DiskModel: " + DiskModel + '\n';
            output += "MediaStatus: " + MediaStatus + '\n';
            output += "MediaLoaded: " + MediaLoaded + '\n';
            output += $"TotalSpace: {ByteFormatter.FormatBytes(TotalSpace)} {ByteFormatter.GetBytesAsStringAndUnit(TotalSpace)}\n";
            output += $"UnpartSpace: {ByteFormatter.FormatBytes(UnpartSpace)} {ByteFormatter.GetBytesAsStringAndUnit(UnpartSpace)}\n";
            output += "WMIPartitionCount: " + WMIPartitionCount + '\n';
            output += "WSMPartitionCount: " + WSMPartitionCount + " (includes hidden and reserved)" + '\n';
            output += "InterfaceType: " + InterfaceType + '\n';
            output += "MediaSignature: " + MediaSignature + '\n';
            output += "Caption: " + Caption + '\n';
            output += "MediaType: " + MediaType + '\n';
            output += "Avaiability: " + Availability + '\n';
            output += "BytesPerSector: " + BytesPerSector + '\n';
            output += "CompressionMethod: " + CompressionMethod + '\n';
            output += "ConfigManagerErrorCode: " + ConfigManagerErrorCode + '\n';
            output += "ConfigManagerUserConfig: " + ConfigManagerUserConfig + '\n';
            output += "CreationClassName: " + CreationClassName + '\n';
            output += "DefaultBlockSize: " + DefaultBlockSize + '\n';
            output += "Description: " + Description + '\n';
            output += "ErrorCleared: " + ErrorCleared + '\n';
            output += "ErrorDescription: " + ErrorDescription + '\n';
            output += "ErrorMethodology: " + ErrorMethodology + '\n';
            output += "FirmwareRevision: " + FirmwareRevision + '\n';
            output += "DiskIndex: " + DiskIndex + '\n';
            output += "InstallDate: " + InstallDate + '\n';
            output += "LastErrorCode: " + LastErrorCode + '\n';
            output += "Manufacturer: " + Manufacturer + '\n';
            output += "MaxBlockSize: " + MaxBlockSize + '\n';
            output += "MinBlockSize: " + MinBlockSize + '\n';
            output += "MaxMediaSize: " + MaxMediaSize + '\n';
            output += "NeedsCleaning: " + NeedsCleaning + '\n';
            output += "NumberOfMediaSupported: " + NumberOfMediaSupported + '\n';
            output += "PNPDeviceID: " + PNPDeviceID + '\n';
            output += "PowerManagementSupported: " + PowerManagementSupported + '\n';
            output += "SCSIBus: " + SCSIBus + '\n';
            output += "SCSILogicalUnit: " + SCSILogicalUnit + '\n';
            output += "SCSIPort: " + SCSIPort + '\n';
            output += "SCSITargetID: " + SCSITargetID + '\n';
            output += "SectorsPerTrack: " + SectorsPerTrack + '\n';
            output += "SerialNumber: " + SerialNumber + '\n';
            output += "StatusInfo: " + StatusInfo + '\n';
            output += "SystemCreationClassName: " + SystemCreationClassName + '\n';
            output += "SystemName: " + SystemName + '\n';
            output += "TotalCylinders: " + TotalCylinders + '\n';
            output += "TotalHeads: " + TotalHeads + '\n';
            output += "TotalSectors: " + TotalSectors + '\n';
            output += "TotalTracks: " + TotalTracks + '\n';
            output += "TracksPerCylinder: " + TracksPerCylinder + '\n';

            output += "_________________" + '\n';
            
            foreach (WSMPartition wsmPartition in wsmPartitions)
            {
                output += wsmPartition.GetOutputAsString();
            }

            return output;
        }

        public Int64 CalcUnpartSpace()
        {
            Int64 result = Convert.ToInt64(TotalSpace);

            foreach (WSMPartition wsmPartition in wsmPartitions)
            {
                result -= Convert.ToInt64(wsmPartition.Size);
            }

            return result > 0 ? result : 0;
        }

        public Dictionary<string, object?> GetKeyValuePairs() 
        {
            Dictionary<string, object?> data = new Dictionary<string, object?>();
            PropertyInfo[] properties = typeof(DiskInfo).GetProperties();

            data.Add(wmiInfoString, "THESE DATA ARE MOSTLY HARDWARE RELATED");

            foreach (PropertyInfo property in properties)
            {
                string key = property.Name;
                object? value = property.GetValue(this);

                if (data.ContainsKey(key)) continue;
                if (key == "TotalSpace") continue;
                if (key == "UnpartSpace") continue;
                if (key == "FreeSpace") continue;
                if (key == "UsedSpace") continue;
                if (typeof(List<WSMPartition>) == property.PropertyType) continue;

                if (key.Contains("Formatted"))
                {
                    Console.WriteLine("FOUND!");
                    key = key.Replace("Formatted", ""); 
                }

                data.Add(key, value);
            }

            return data;
        }

        private UInt64 GetLogicalFreeSpace()
        {
            UInt64 freeSpaceResult = new UInt64();

            foreach (WSMPartition partition in WSMPartitions)
            {
                if (partition.WMIPartition != null && partition.WMIPartition.LogicalDriveInfo != null) 
                {
                    freeSpaceResult += partition.WMIPartition.LogicalDriveInfo.FreeSpace;
                }
            }

            return freeSpaceResult;
        }
    }
}
