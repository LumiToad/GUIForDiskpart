using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Management;
using System.Management.Automation;

using GUIForDiskpart.Model.Logic;


namespace GUIForDiskpart.Database.Retrievers
{
    public class Disk
    {
        private const string WIN32_VOL_CHANGE_EVENT_QUERY = 
            "SELECT * FROM Win32_VolumeChangeEvent WHERE EventType = 2 or EventType = 3";
        private const string WIN32_DISKDRIVE_QUERY = "select * from Win32_DiskDrive";
        private const string OP_TYPE_PATH = @"root\Microsoft\Windows\Storage";

        private const string MEDIA_TYPE_QUERY = $"$Query.MediaType";
        private static string MediaTypeQuery(string name) =>
            $"$Query = Get-CimInstance -Query \"select * from MSFT_PhysicalDisk WHERE FriendlyName Like '%{name}%'\" -Namespace root\\Microsoft\\Windows\\Storage";
        private static string OPSelectQuery(uint diskIndex) => $"select * from MSFT_Disk WHERE Number={diskIndex}";

        public void SetupDiskChangedWatcher()
        {
            try
            {
                WqlEventQuery query = new WqlEventQuery(WIN32_VOL_CHANGE_EVENT_QUERY);
                ManagementEventWatcher watcher = new ManagementEventWatcher();
                watcher.Query = query;
                watcher.EventArrived += DiskService.ExecuteOnDiskChanged;
                watcher.Options.Timeout = TimeSpan.FromSeconds(3);
                watcher.Start();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        public List<ManagementObject> GetAllWMIObjects()
        {
            ManagementObjectSearcher diskDriveQuery = new ManagementObjectSearcher(WIN32_DISKDRIVE_QUERY);

            List<ManagementObject> retVal = new();
            foreach (ManagementObject disk in diskDriveQuery.Get())
            {
                retVal.Add(disk);
            }

            return retVal;
        }

        public ushort? GetMediaTypeValue(string friendlyName)
        {
            if (string.IsNullOrEmpty(friendlyName)) return null;

            ushort? result = null;

            foreach (var name in friendlyName.Split(new[] { ' ', '-', '_', ':' }))
            {
                string[] commands = new string[2];
                commands[0] += MediaTypeQuery(name);
                commands[1] += MEDIA_TYPE_QUERY;
                List<PSObject> psObjects = CommandExecuter.IssuePowershellCommand(commands);
                PSObject? data = psObjects[0];

                if (data == null) continue;
                result = (ushort)data.BaseObject;
            }
            return result;
        }

        public ushort[] GetOperationalStatus(uint diskIndex)
        {
            ManagementScope scope = new ManagementScope(OP_TYPE_PATH);
            SelectQuery query = new SelectQuery(OPSelectQuery(diskIndex));
            ManagementObjectSearcher msftDiskQuery = new ManagementObjectSearcher(scope, query);
            foreach (ManagementObject msftDisk in msftDiskQuery.Get())
            {
                return (ushort[])msftDisk.Properties["OperationalStatus"].Value;
            }
            return new ushort[1] { 0 };
        }
    }
}
