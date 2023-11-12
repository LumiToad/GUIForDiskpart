using GUIForDiskpart.main;
using System;
using System.Collections.Generic;

namespace GUIForDiskpart.main
{
    public static class DriveLetterManager
    {
        private const string viableDriveLetters = "CDEFGHIJKLMNOPQRSTUVWXYZ";

        public static char[] GetAvailableDriveLetters()
        {
            string availableLetters = viableDriveLetters;

            char[] usedLetters = GetUsedDriveLetters();

            foreach (char usedLetter in usedLetters) 
            {
                if (availableLetters.Contains(usedLetter))
                {
                    availableLetters = availableLetters.Replace(usedLetter.ToString(), "");
                    availableLetters.TrimEnd();
                }
            }

            return availableLetters.ToCharArray();
        }

        public static char[] GetUsedDriveLetters()
        {
            string letters = string.Empty;

            foreach (DiskInfo diskInfo in DiskRetriever.PhysicalDrives)
            {
                foreach (Partition partition in diskInfo.Partitions)
                {
                    if (!partition.IsLogicalDisk) continue;
                    if (!string.IsNullOrEmpty(partition.LogicalDiskInfo.DriveLetter))
                    {
                        letters += partition.LogicalDiskInfo.DriveLetter[0];
                    }
                }
            }

            return letters.ToCharArray();
        }
    }
}

