using System.Management;


namespace GUIForDiskpart.Database.Retrievers
{
    public class LogicalDisk
    {
        public ManagementObject? LDQuery(ManagementObject partition)
        {
            var logicalDriveQueryText = string.Format("associators of {{{0}}} where AssocClass = Win32_LogicalDiskToPartition", partition.Path.RelativePath);
            var logicalDriveQuery = new ManagementObjectSearcher(logicalDriveQueryText);

            ManagementObject? RetVal = null;

            // The logicalDriveQuery.Get() returns a collection, which in this case can never contain more than one object!
            // Todo: check if this is true lol
            foreach (ManagementObject logicalDisk in logicalDriveQuery.Get())
            {
                RetVal = logicalDisk;
            }

            return RetVal;
        }
    }
}
