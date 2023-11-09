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
                foreach (WSMPartition wsmPartition in diskInfo.WSMPartitions)
                {
                    if (wsmPartition.WMIPartition == null) continue;
                    if (wsmPartition.WMIPartition.LogicalDriveInfo == null) continue;
                    if (!string.IsNullOrEmpty(wsmPartition.WMIPartition.LogicalDriveInfo.DriveLetter))
                    {
                        letters += wsmPartition.WMIPartition.LogicalDriveInfo.DriveLetter[0];
                    }
                }
            }

            return letters.ToCharArray();
        }
    }
}

