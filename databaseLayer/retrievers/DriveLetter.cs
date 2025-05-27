namespace GUIForDiskpart.Database.Retrievers
{
    public class DriveLetter
    {
        public char[] GetUsedDriveLetters()
        {
            string letters = string.Empty;

            foreach (DiskModel diskModel in DiskService.PhysicalDrives)
            {
                foreach (PartitionModel partition in diskModel.Partitions)
                {
                    if (!partition.IsLogicalDisk) continue;
                    if (!string.IsNullOrEmpty(partition.LDModel.DriveLetter))
                    {
                        letters += partition.LDModel.DriveLetter[0];
                    }
                }
            }

            return letters.ToCharArray();
        }
    }
}

