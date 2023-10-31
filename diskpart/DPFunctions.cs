using GUIForDiskpart.main;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace GUIForDiskpart.diskpart
{
    public static class DPFunctions
    {
        private static List<string> dpInfoExcludeStrings = new List<string>();

        static DPFunctions()
        {
            GetDPInfoExcludeStrings();
        }

        public static string List(string type)
        {
            string[] commands = new string[1];

            commands[0] = "List " + type;

            return ExecuteInternal(commands);
        }

        public static string DetailDisk(uint diskIndex) 
        {
            string[] commands = new string[2];

            commands[0] = "Select " + "DISK " + diskIndex;
            commands[1] = "Detail " + "DISK ";

            return ExecuteInternal(commands);
        }

        public static string DetailPart(uint diskIndex, uint partIndex)
        {
            string[] commands = new string[3];

            commands[0] = "Select " + "DISK " + diskIndex;
            commands[1] = "Select " + "PART " + partIndex;
            commands[2] = "Detail " + "PART ";

            return ExecuteInternal(commands);
        }

        public static string CreatePartition(uint diskIndex, string option, UInt64 sizeInMB, bool isNoErr)
        {
            string[] commands = new string[2];

            commands[0] = "Select " + DPListType.DISK + " " + diskIndex;
            commands[1] = "Create PARTITION " +  option + " ";

            if (sizeInMB > 0)
            {
                commands[1] += "SIZE=" + sizeInMB + " ";
            }

            if (isNoErr)
            {
                commands[1] += "NOERR ";
            }

            return ExecuteInternal(commands);
        }

        public static string CreateVolume(uint diskIndex, string option, UInt64 sizeInMB, bool isNoErr)
        {
            string[] commands = new string[1];

            commands[0] = "Select " + DPListType.DISK + " " + diskIndex;
            commands[1] = "Create VOLUME " + option + " ";

            if (sizeInMB > 0)
            {
                commands[1] += "SIZE=" + sizeInMB + " ";
            }

            if (isNoErr)
            {
                commands[1] += "NOERR ";
            }

            return ExecuteInternal(commands);
        }

        public static string CreateVDisk(uint diskIndex, string filePath, bool isNoErr)
        {
            string[] commands = new string[1];

            commands[0] = "Select " + DPListType.DISK + " " + diskIndex;
            commands[1] = "Create VDISK FILE=" + filePath + " ";

            if (isNoErr)
            {
                commands[1] += "NOERR ";
            }

            return ExecuteInternal(commands);
        }

        public static string Format(uint diskIndex, uint partitionIndex, FileSystem fileSystem,
            string volumeName, bool isQuickFormatting, bool isCompressed, bool isOverride, bool isNoWait, bool isNoErr)
        {
            string[] commands = new string[3];

            commands[0] = "Select " + DPListType.DISK + " " + diskIndex;
            commands[1] = "Select " + DPListType.PARTITION + " " + partitionIndex;
            commands[2] = "Format " + "FS=" + fileSystem + " " ;

            if (volumeName != "")
            {
                commands[2] += "LABEL=" + volumeName + " ";
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

            return ExecuteInternal(commands);
        }

        public static string Delete(uint diskIndex, uint partitionIndex, bool isNoErr, bool isOverride)
        {
            string[] commands = new string[3];

            commands[0] = "Select " + DPListType.DISK + " " + diskIndex;
            commands[1] = "Select " + DPListType.PARTITION + " " + partitionIndex;
            commands[2] = "Delete " + "PART ";

            if (isNoErr)
            {
                commands[2] += "NOERR ";
            }

            if (isOverride)
            {
                commands[2] += "OVERRIDE ";
            }

            return ExecuteInternal(commands);
        }

        public static string Assign(uint diskIndex, int partitionIndex, char diskLetter, bool isNoErr)
        {
            string[] commands = new string[3];

            commands[0] = "Select " + DPListType.DISK + " " + diskIndex;
            commands[1] = "Select " + DPListType.PARTITION + " " + partitionIndex;
            commands[2] = "Assign " + "LETTER=" + diskLetter + " ";

            if (isNoErr)
            {
                commands[2] += "NOERR ";
            }

            return ExecuteInternal(commands);
        }

        public static string Assign(uint diskIndex, int partitionIndex, bool isNoErr)
        {
            string[] commands = new string[3];

            commands[0] = "Select " + DPListType.DISK + " " + diskIndex;
            commands[1] = "Select " + DPListType.PARTITION + " " + partitionIndex;
            commands[2] = "Assign ";

            if (isNoErr)
            {
                commands[2] += "NOERR ";
            }

            return ExecuteInternal(commands);
        }

        public static string Clean(uint diskIndex, bool isCleanAll)
        {
            string[] commands = new string[2];

            commands[0] = "Select " + DPListType.DISK + " " + diskIndex;
            commands[1] = "Clean ";

            if (isCleanAll)
            {
                commands[1] += "ALL";
            }

            return ExecuteInternal(commands);
        }

        public static string Convert(uint diskIndex, string options)
        {
            string[] commands = new string[2];

            commands[0] = "Select " + DPListType.DISK + " " + diskIndex;
            commands[1] = "Convert " + options;

            return ExecuteInternal(commands);
        }

        #region private

        private static string RemoveDPInfo(string info)
        {
            string fullOutput = info;
            string output = string.Empty;

            foreach (string line in dpInfoExcludeStrings)
            {
                if (line == "") continue;

                if (info.Contains(line) || info.Contains(" \n"))
                {
                    fullOutput = fullOutput.Replace(line, "");
                    output = Regex.Replace(fullOutput, @"[\r\n]+", "\n");
                }
            }

            return output;
        }

        private static void GetDPInfoExcludeStrings()
        {
            string output = CommandExecuter.IssueCommand(ProcessType.DISKPART, "");

            foreach (string line in output.Split('\r', StringSplitOptions.TrimEntries))
            {
                dpInfoExcludeStrings.Add(line);
            }
            dpInfoExcludeStrings.RemoveAt(0);
            dpInfoExcludeStrings.RemoveAt(dpInfoExcludeStrings.Count - 1);
        }

        private static string ExecuteInternal(string[] commands)
        {
            string fullOutput = CommandExecuter.IssueCommand(ProcessType.DISKPART, commands);
            string output = RemoveDPInfo(fullOutput);

            return output;
        }

        #endregion private
    }
}
