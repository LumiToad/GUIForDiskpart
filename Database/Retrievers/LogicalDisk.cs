using System.Management;


namespace GUIForDiskpart.Database.Retrievers
{
    public class LogicalDisk
    {
        private const string LD_QUERY_FORMAT_STRING = "associators of {{{0}}} where AssocClass = Win32_LogicalDiskToPartition";

        public ManagementObject? LDQuery(ManagementObject partition)
        {
            var logicalDriveQueryText = string.Format(LD_QUERY_FORMAT_STRING, partition.Path.RelativePath);
            var logicalDriveQuery = new ManagementObjectSearcher(logicalDriveQueryText);

            ManagementObject? RetVal = null;

            // The logicalDriveQuery.Get() returns a collection,
            // which in this case (Partition) can never contain more than one object!
            foreach (ManagementObject logicalDisk in logicalDriveQuery.Get())
            {
                RetVal = logicalDisk;
            }

            return RetVal;
        }
    }
}
