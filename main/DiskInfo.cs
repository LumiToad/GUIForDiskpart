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

        public Int64 UnallocatedSpace => CalcUnallocatedSpace();

        readonly private UInt32 wmiPartitionCount;
        public UInt32 WMIPartitionCount { get { return wmiPartitionCount; } }

        public int PartitionCount => Partitions.Count;

        readonly private string interfaceType;
        public string InterfaceType { get { return interfaceType; } }

        readonly private UInt32 mediaSignature;
        public UInt32 MediaSignature { get { return mediaSignature; } }

        readonly private string caption;
        public string Caption { get { return caption; } }

        readonly private string mediaType;
        public string MediaType { get { return mediaType; } }

        readonly private UInt16? msftMediaType;
        public UInt16? MSFTMediaType { get { return msftMediaType; } }

        public string DeviceMediaType => GetDeviceMediaType();

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

        private List<Partition> partitions = new List<Partition>();
        public List<Partition> Partitions
        { get { return partitions; } }

        public UInt64 FreeSpace { get { return GetLogicalFreeSpace(); } }
        public UInt64 UsedSpace { get { return (TotalSpace - Convert.ToUInt64(UnallocatedSpace)) - FreeSpace; } }
        
        public string FormattedFreeSpace => ByteFormatter.FormatBytes(GetLogicalFreeSpace());
        public string FormattedTotalSpace => ByteFormatter.FormatBytes(TotalSpace);
        public string FormattedUnallocatedSpace => ByteFormatter.FormatBytes(UnallocatedSpace);
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
            ushort? msftMediaType,
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
            this.msftMediaType = msftMediaType;
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
            string fullOutput = string.Empty;

            fullOutput += "HardwareDeviceID: " + DeviceId + '\n';
            fullOutput += "PhysicalName: " + PhysicalName + '\n';
            fullOutput += "DiskName: " + DiskName + '\n';
            fullOutput += "DiskModel: " + DiskModel + '\n';
            fullOutput += "MediaStatus: " + MediaStatus + '\n';
            fullOutput += "MediaLoaded: " + MediaLoaded + '\n';
            fullOutput += $"TotalSpace: {ByteFormatter.FormatBytes(TotalSpace)} {ByteFormatter.GetBytesAsStringAndUnit(TotalSpace)}\n";
            fullOutput += $"UnpartSpace: {ByteFormatter.FormatBytes(UnallocatedSpace)} {ByteFormatter.GetBytesAsStringAndUnit(UnallocatedSpace)}\n";
            fullOutput += "WMIPartitionCount: " + WMIPartitionCount + '\n';
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

            data.Add(wmiInfoString, "THESE DATA ARE MOSTLY HARDWARE RELATED");

            foreach (PropertyInfo property in properties)
            {
                string key = property.Name;
                object? value = property.GetValue(this);

                if (data.ContainsKey(key)) continue;
                if (key == "TotalSpace") continue;
                if (key == "UnallocatedSpace") continue;
                if (key == "FreeSpace") continue;
                if (key == "UsedSpace") continue;
                if (key == "MSFTMediaType") continue;
                if (typeof(List<Partition>) == property.PropertyType) continue;

                if (key.Contains("Formatted"))
                {
                    key = key.Replace("Formatted", ""); 
                }

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
    }
}
