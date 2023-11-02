using System;

namespace GUIForDiskpart.main
{
    public class LogicalDiskInfo
    {
        private string driveLetter;
        public string DriveLetter { get; set; }

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
            Console.WriteLine("Name: {0}", DriveLetter);
            Console.WriteLine("DriveType: {0}", DriveType);
            Console.WriteLine("FileSystem: {0}", FileSystem);
            Console.WriteLine("TotalSpace: {0}", TotalSpace);
            Console.WriteLine("FreeSpace: {0}", FreeSpace);
            Console.WriteLine("VolumeName: {0}", VolumeName);
            Console.WriteLine("VolumeSerial: {0}", VolumeSerial);
            Console.WriteLine(new string('-', 79));
        }

        public string GetOutputAsString()
        {
            string output = string.Empty;
            output += "\t\tVolumeName: " + VolumeName + "\n";
            output += "\t\tDriveLetter: " + DriveLetter + "\\" + "\n";
            output += "\t\tFileSystem: " + FileSystem + "\n";
            output += "\t\tTotalSpace: " + TotalSpace + "\n";
            output += "\t\tFreeSpace: " + FreeSpace + "\n";
            output += "\t\tDriveType: " + DriveType + "\n";
            output += "\t\tVolumeSerial: " + VolumeSerial + "\n";

            return output;
        }
    }
}
