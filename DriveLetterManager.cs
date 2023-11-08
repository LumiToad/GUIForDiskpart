using GUIForDiskpart.main;
using System.Collections.Generic;

namespace GUIForDiskpart.main
{
    

    public static class DriveLetterManager
    {
        private const string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public static string Alphabet
        { get => alphabet; }

        public static char[] GetAvailableDriveLetters(List<DiskInfo> list)
        {
            string availableLetters = string.Empty;

            char[] usedLetters = GetUsedDriveLetters(list);

            foreach (char usedLetter in usedLetters) 
            {
                foreach (char letter in Alphabet)
                {
                    if (usedLetter != letter)
                    {
                        availableLetters += letter;
                    }
                }
            }

            return availableLetters.ToCharArray();
        }

        public static char[] GetUsedDriveLetters(List<DiskInfo> list)
        {
            string letters = string.Empty;

            foreach (DiskInfo diskInfo in list)
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

