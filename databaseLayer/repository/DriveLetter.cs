using GUIForDiskpart.Model.Data;
using GUIForDiskpart.service;
using System;
using System.Collections.Generic;

namespace GUIForDiskpart.Database.Repository
{
    public static class DriveLetter
    {
        private const string VIABLE_DRIVE_LETTERS = "CDEFGHIJKLMNOPQRSTUVWXYZ";

        public static char[] GetAvailableDriveLetters()
        {
            string availableLetters = VIABLE_DRIVE_LETTERS;

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
                foreach (Model.Data.Partition partition in diskInfo.Partitions)
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

