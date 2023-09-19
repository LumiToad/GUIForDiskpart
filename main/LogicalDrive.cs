using System;

namespace GUIForDiskpart.main
{
    public class LogicalDrive
    {
        private string driveName;
        public string DriveName { get; set; }

        private string diskInterface;
        public string DiskInterface { get; set; }

        private string mediaType;
        public string MediaType { get; set; }

        private UInt32 mediaSignature;
        public UInt32 MediaSignature { get; set; }

        private string mediaStatus;
        public string MediaStatus { get; set; }

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

        public UInt64 totalSpace;
        public UInt64 TotalSpace { get; set; }

        private string volumeName;
        public string VolumeName { get; set; }

        private string volumeSerial;
        public string VolumeSerial { get; set; }
       
        public void PrintToConsole()
        {
            Console.WriteLine("DiskInterface: {0}", DiskInterface);
            Console.WriteLine("MediaType: {0}", MediaType);
            Console.WriteLine("MediaSignature: {0}", MediaSignature);

            Console.WriteLine("DriveName: {0}", DriveName);
            Console.WriteLine("DriveId: {0}", DriveId);
            Console.WriteLine("DriveCompressed: {0}", DriveCompressed);
            Console.WriteLine("DriveType: {0}", DriveType);
            Console.WriteLine("FileSystem: {0}", FileSystem);
            Console.WriteLine("FreeSpace: {0}", FreeSpace);
            //Console.WriteLine("DriveMediaType: {0}", DriveMediaType);
            Console.WriteLine("VolumeName: {0}", VolumeName);
            Console.WriteLine("VolumeSerial: {0}", VolumeSerial);

            Console.WriteLine(new string('-', 79));
        }

        public string GetOutputAsString()
        {
            string output = string.Empty;
            output += "DiskInterface: " + DiskInterface + "\n";
            output += "MediaType: " + MediaType + "\n";
            output += "MediaSignature: " + MediaSignature + "\n";
            
            output += "DriveName: " + DriveName + "\n";
            output += "DriveId: " + DriveId + "\n";
            output += "DriveCompressed: " + DriveCompressed + "\n";
            output += "DriveType: " + DriveType + "\n";
            output += "FileSystem: " + FileSystem + "\n";
            output += "FreeSpace: " + FreeSpace + "\n";
            //output += "DriveMediaType: " + DriveMediaType + "\n";
            output += "VolumeName: " + VolumeName + "\n";
            output += "VolumeSerial: " + VolumeSerial + "\n";
            output += "_________________" + "\n";

            return output;
        }
    }
}
