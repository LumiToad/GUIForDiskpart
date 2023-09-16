using System;

namespace GUIForDiskpart.main
{
    public class LogicalDrive : PhysicalDrive
    {
        private string driveName;
        public string DriveName { get; set; }

        private int driveNumber;
        public int DriveNumber { get; set; }

        private string diskInterface;
        public string DiskInterface { get; set; }

        private string mediaType;
        public string MediaType { get; set; }

        private UInt32 mediaSignature;
        public UInt32 MediaSignature { get; set; }

        private string driveId;
        public string DriveId { get; set; }

        private bool driveCompressed;
        public bool DriveCompressed { get; set; }

        private UInt32 driveType;
        public UInt32 DriveType { get; set; }

        private string fileSystem;
        public string FileSystem { get; set; }

        private UInt64 freeSpace;
        public UInt64 FreeSpace { get; set; }

        private UInt32 driveMediaType;
        public UInt32 DriveMediaType { get; set; }

        private string volumeName;
        public string VolumeName { get; set; }

        private string volumeSerial;
        public string VolumeSerial { get; set; }
       
        public void PrintToConsole()
        {
            Console.WriteLine("PhysicalName: {0}", PhysicalName);
            Console.WriteLine("DriveNumber: {0}", DriveNumber);
            Console.WriteLine("DiskName: {0}", DiskName);
            Console.WriteLine("DiskModel: {0}", DiskModel);
            Console.WriteLine("DiskInterface: {0}", DiskInterface);
            Console.WriteLine("MediaLoaded: {0}", MediaLoaded);
            Console.WriteLine("MediaType: {0}", MediaType);
            Console.WriteLine("MediaSignature: {0}", MediaSignature);
            Console.WriteLine("MediaStatus: {0}", MediaStatus);

            Console.WriteLine("DriveName: {0}", DriveName);
            Console.WriteLine("DriveId: {0}", DriveId);
            Console.WriteLine("DriveCompressed: {0}", DriveCompressed);
            Console.WriteLine("DriveType: {0}", DriveType);
            Console.WriteLine("FileSystem: {0}", FileSystem);
            Console.WriteLine("FreeSpace: {0}", FreeSpace);
            Console.WriteLine("TotalSpace: {0}", TotalSpace);
            Console.WriteLine("DriveMediaType: {0}", DriveMediaType);
            Console.WriteLine("VolumeName: {0}", VolumeName);
            Console.WriteLine("VolumeSerial: {0}", VolumeSerial);

            Console.WriteLine(new string('-', 79));
        }

        public string GetOutputAsString()
        {
            string output = string.Empty;
            output += "PhysicalName: " + PhysicalName + "\n";
            output += "DriveNumber: " + DriveNumber + "\n";
            output += "DiskName: " + DiskName + "\n";
            output += "DiskModel: " + DiskModel + "\n";
            output += "DiskInterface: " + DiskInterface + "\n";
            output += "MediaLoaded: " + MediaLoaded + "\n";
            output += "MediaType: " + MediaType + "\n";
            output += "MediaSignature: " + MediaSignature + "\n";
            output += "MediaStatus: " + MediaStatus + "\n";
            
            output += "DriveName: " + DriveName + "\n";
            output += "DriveId: " + DriveId + "\n";
            output += "DriveCompressed: " + DriveCompressed + "\n";
            output += "DriveType: " + DriveType + "\n";
            output += "FileSystem: " + FileSystem + "\n";
            output += "FreeSpace: " + FreeSpace + "\n";
            output += "TotalSpace: " + TotalSpace + "\n";
            output += "DriveMediaType: " + DriveMediaType + "\n";
            output += "VolumeName: " + VolumeName + "\n";
            output += "VolumeSerial: " + VolumeSerial + "\n";
            output += "_________________" + "\n";

            return output;
        }

        public void AddPhysicalDriveInfo(PhysicalDrive physicalDrive)
        {
            DeviceId = physicalDrive.DeviceId;
            PhysicalName = physicalDrive.PhysicalName;
            DiskName = physicalDrive.DiskName;
            DiskModel = physicalDrive.DiskModel;
            MediaStatus = physicalDrive.MediaStatus;
            MediaLoaded = physicalDrive.MediaLoaded;
            TotalSpace = physicalDrive.TotalSpace;
        }

        public void ParseDriveNumber()
        {
            DriveNumber = int.Parse(PhysicalName.Substring(PhysicalName.Length - 1));
        }
    }
}
