namespace GUIForDiskpart.Database.Retrievers
{
    public class DriveLetter
    {
        public char[] GetUsedDriveLetters()
        {
            string letters = string.Empty;

            foreach (DiskModel diskInfo in DiskService.PhysicalDrives)
            {
                foreach (PartitionModel partition in diskInfo.Partitions)
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

