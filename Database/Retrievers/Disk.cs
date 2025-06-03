using System;
using System.Collections.Generic;
using System.Management;
using System.Management.Automation;

using GUIForDiskpart.Model.Logic;


namespace GUIForDiskpart.Database.Retrievers
{
    public class Disk
    {
        public void SetupDiskChangedWatcher()
        {
            try
            {
                WqlEventQuery query = new WqlEventQuery("SELECT * FROM Win32_VolumeChangeEvent WHERE EventType = 2 or EventType = 3");
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
            ManagementObjectSearcher diskDriveQuery = new ManagementObjectSearcher("select * from Win32_DiskDrive");

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
                commands[0] += $"$Query = Get-CimInstance -Query \"select * from MSFT_PhysicalDisk WHERE FriendlyName Like '%{name}%'\" -Namespace root\\Microsoft\\Windows\\Storage";
                commands[1] += $"$Query.MediaType";
                List<PSObject> psObjects = CommandExecuter.IssuePowershellCommand(commands);
                PSObject? data = psObjects[0];

                if (data == null) continue;
                result = (ushort)data.BaseObject;
            }
            return result;
        }

        public ushort[] GetOperationalStatus(uint diskIndex)
        {
            ManagementScope scope = new ManagementScope(@"root\Microsoft\Windows\Storage");
            SelectQuery query = new SelectQuery($"select * from MSFT_Disk WHERE Number={diskIndex}");
            ManagementObjectSearcher msftDiskQuery = new ManagementObjectSearcher(scope, query);
            foreach (ManagementObject msftDisk in msftDiskQuery.Get())
            {
                return (ushort[])msftDisk.Properties["OperationalStatus"].Value;
            }
            return new ushort[1] { 0 };
        }
    }
}
