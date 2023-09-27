using GUIForDiskpart.main;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace GUIForDiskpart.diskpart
{
    public class DPFunctions
    {
        private CommandExecuter commandExecuter;
        private List<string> dpInfoExcludeStrings = new List<string>();

        public DPFunctions()
        {
            commandExecuter = new CommandExecuter();
;
            GetDPInfoExcludeStrings();
        }

        private void GetDPInfoExcludeStrings()
        {
            string output = commandExecuter.IssueCommand(ProcessType.diskpart, "");

            foreach (string line in output.Split('\r', StringSplitOptions.TrimEntries)) 
            {
                dpInfoExcludeStrings.Add(line);
            }
            dpInfoExcludeStrings.RemoveAt(0);
            dpInfoExcludeStrings.RemoveAt(dpInfoExcludeStrings.Count - 1);
        }

        public string List(DPListType type)
        {
            string command = "List " + type.ToString();

            string output = commandExecuter.IssueCommand(ProcessType.diskpart, command);
            output = RemoveDPInfo(output);
            
            return output;
        }

        public string DetailDisk(int diskIndex) 
        {
            string[] commands = new string[2];

            commands[0] = "Select " + "DISK " + diskIndex.ToString();
            commands[1] = "Detail " + "DISK ";

            string output = commandExecuter.IssueCommand(ProcessType.diskpart, commands);
            output = RemoveDPInfo(output);

            return output;
        }

        public string DetailPart(int diskIndex, int partIndex)
        {
            string[] commands = new string[3];

            commands[0] = "Select " + "DISK " + diskIndex.ToString();
            commands[1] = "Select " + "PART " + partIndex.ToString();
            commands[2] = "Detail " + "PART ";

            string output = commandExecuter.IssueCommand(ProcessType.diskpart, commands);
            output = RemoveDPInfo(output);

            return output;
        }

        private string RemoveDPInfo(string info)
        {
            string output = info;
            
            foreach(string line in dpInfoExcludeStrings)
            {
                if (line == "") continue;

                if (info.Contains(line) || info.Contains(" \n"))
                {
                    output = output.Replace(line, "");
                    output = Regex.Replace(output, @"[\r\n]+", "\n");
                }
            }

            return output;
        }

        public string CreatePartition(int diskIndex, CreatePartitionOptions option, UInt64 sizeInMB, bool isNoErr)
        {
            string[] commands = new string[2];

            commands[0] = "Select " + DPListType.DISK.ToString() + " " + diskIndex.ToString();
            commands[1] = "Create PARTITION " +  option.ToString() + " ";

            if (sizeInMB > 0)
            {
                commands[1] += "SIZE=" + sizeInMB.ToString() + " ";
            }

            if (isNoErr)
            {
                commands[1] += "NOERR ";
            }

            string output = commandExecuter.IssueCommand(ProcessType.diskpart, commands);
            output = RemoveDPInfo(output);

            return output;
        }

        public string CreateVolume(int diskIndex, CreateVolumeOptions option, UInt64 sizeInMB, bool isNoErr)
        {
            string[] commands = new string[1];

            commands[0] = "Select " + DPListType.DISK.ToString() + " " + diskIndex.ToString();
            commands[1] = "Create VOLUME " + option.ToString() + " ";

            if (sizeInMB > 0)
            {
                commands[1] += "SIZE=" + sizeInMB.ToString() + " ";
            }

            if (isNoErr)
            {
                commands[1] += "NOERR ";
            }

            string output = commandExecuter.IssueCommand(ProcessType.diskpart, commands);
            output = RemoveDPInfo(output);

            return output;
        }

        public string CreateVDisk(int diskIndex, string filePath, bool isNoErr)
        {
            string[] commands = new string[1];

            commands[0] = "Select " + DPListType.DISK.ToString() + " " + diskIndex.ToString();
            commands[1] = "Create VDISK FILE=" + filePath + " ";

            if (isNoErr)
            {
                commands[1] += "NOERR ";
            }

            string output = commandExecuter.IssueCommand(ProcessType.diskpart, commands);
            output = RemoveDPInfo(output);

            return output;
        }

        public string Format(int diskIndex, int partitionIndex, FileSystem fileSystem,
            string volumeName, bool isQuickFormatting, bool isCompressed, bool isOverride, bool isNoWait, bool isNoErr)
        {
            string[] commands = new string[3];

            commands[0] = "Select " + DPListType.DISK.ToString() + " " + diskIndex.ToString();
            commands[1] = "Select " + DPListType.PARTITION.ToString() + " " + partitionIndex.ToString();
            commands[2] = "Format " + "FS=" + fileSystem.ToString() + " " ;

            if (volumeName != "")
            {
                commands[2] += "LABEL=" + volumeName.ToString() + " ";
            }

            if (isQuickFormatting)
            {
                commands[2] += "QUICK ";
            }

            if (isCompressed && fileSystem == FileSystem.NTFS) 
            {
                commands[2] += "COMPRESS ";
            }

            if (isOverride) 
            {
                commands[2] += "OVERRIDE ";
            }

            if (isNoWait) 
            {
                commands[2] += "NOWAIT ";
            }

            if (isNoErr) 
            {
                commands[2] += "NOERR ";
            }

            string output = commandExecuter.IssueCommand(ProcessType.diskpart, commands);
            output = RemoveDPInfo(output);

            return output;
        }

        public string Delete(int diskIndex, int partitionIndex, bool isNoErr, bool isOverride)
        {
            string[] commands = new string[3];

            commands[0] = "Select " + DPListType.DISK.ToString() + " " + diskIndex.ToString();
            commands[1] = "Select " + DPListType.PARTITION.ToString() + " " + partitionIndex.ToString();
            commands[2] = "Delete " + "PART ";

            if (isNoErr)
            {
                commands[2] += "NOERR ";
            }

            if (isOverride)
            {
                commands[2] += "OVERRIDE ";
            }

            string output = commandExecuter.IssueCommand(ProcessType.diskpart, commands);
            output = RemoveDPInfo(output);

            return output;
        }

        public string Assign(int diskIndex, int partitionIndex, char driveLetter, bool isNoErr)
        {
            string[] commands = new string[3];

            commands[0] = "Select " + DPListType.DISK.ToString() + " " + diskIndex.ToString();
            commands[1] = "Select " + DPListType.PARTITION.ToString() + " " + partitionIndex.ToString();
            commands[2] = "Assign " + "LETTER=" + driveLetter + " ";

            if (isNoErr)
            {
                commands[2] += "NOERR ";
            }

            string output = commandExecuter.IssueCommand(ProcessType.diskpart, commands);
            output = RemoveDPInfo(output);

            return output;
        }

        public string Assign(int diskIndex, int partitionIndex, bool isNoErr)
        {
            string[] commands = new string[3];

            commands[0] = "Select " + DPListType.DISK.ToString() + " " + diskIndex.ToString();
            commands[1] = "Select " + DPListType.PARTITION.ToString() + " " + partitionIndex.ToString();
            commands[2] = "Assign ";

            if (isNoErr)
            {
                commands[2] += "NOERR ";
            }

            string output = commandExecuter.IssueCommand(ProcessType.diskpart, commands);
            output = RemoveDPInfo(output);

            return output;
        }

        public string Clean(int diskIndex, bool isCleanAll)
        {
            string[] commands = new string[2];

            commands[0] = "Select " + DPListType.DISK.ToString() + " " + diskIndex.ToString();
            commands[1] = "Clean ";

            if (isCleanAll)
            {
                commands[1] += "ALL";
            }

            string output = commandExecuter.IssueCommand(ProcessType.diskpart, commands);
            output = RemoveDPInfo(output);

            return output;
        }

        public string Convert(int diskIndex, ConvertOptions options)
        {
            string[] commands = new string[2];

            commands[0] = "Select " + DPListType.DISK.ToString() + " " + diskIndex.ToString();
            commands[1] = "Convert " + options.ToString();

            string output = commandExecuter.IssueCommand(ProcessType.diskpart, commands);
            output = RemoveDPInfo(output);

            return output;
        }
    }
}
