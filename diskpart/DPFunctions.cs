﻿using GUIForDiskpart.main;
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

        public static string ListPart(uint? index)
        {
            string[] commands = new string[2];

            commands[0] = "Select DISK " + index;
            commands[1] = "List " + DPListType.PARTITION;

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

            commands[0] = "Create VOLUME " + option + " DISK=" + diskIndex + " ";

            if (sizeInMB > 0)
            {
                commands[0] += "SIZE=" + sizeInMB + " ";
            }

            if (isNoErr)
            {
                commands[0] += "NOERR ";
            }

            return ExecuteInternal(commands);
        }

        public static string CreateVolume(uint[] diskIndexes, string option, UInt64 sizeInMB, uint alignInKB, bool isNoErr)
        {
            string[] commands = new string[1];

            commands[0] = "Create VOLUME SIMPLE" + option + " ";

            if (sizeInMB > 0)
            {
                commands[0] += "SIZE=" + sizeInMB + " ";
            }

            string disksString = string.Empty;

            for (int i = 0; i < diskIndexes.Length; i++)
            {
                disksString += diskIndexes;
                if (i != diskIndexes.Length - 1)
                {
                    disksString += ",";
                }
            }

            commands[0] += "ALIGN=" + alignInKB + " ";

            if (isNoErr)
            {
                commands[0] += "NOERR ";
            }

            return ExecuteInternal(commands);
        }

        #region VDisk

        public static string CreateVDisk(string savePath, bool isFixed, int maxSizeInMB, string sddlFlags, bool isNoErr)
        {
            string[] commands = new string[1];

            commands[0] = "Create VDISK FILE=" + savePath + " ";

            if (isFixed)
            {
                commands[0] += "FIXED ";
            }
            else
            {
                commands[0] += "EXPANDABLE ";
            }

            commands[0] += "MAXIMUM=" + maxSizeInMB + " ";

            commands[0] += "SD=" + sddlFlags + " ";

            if (isNoErr)
            {
                commands[0] += "NOERR ";
            }

            return ExecuteInternal(commands);
        }

        public static string CreateVDisk(string savePath, bool isFixed, int maxSizeInMB, bool isNoErr)
        {
            string[] commands = new string[1];

            commands[0] = "Create VDISK FILE=" + savePath + " ";

            if (isFixed)
            {
                commands[0] += "TYPE=FIXED ";
            }
            else
            {
                commands[0] += "TYPE=EXPANDABLE ";
            }

            commands[0] += "MAXIMUM=" + maxSizeInMB + " ";

            if (isNoErr)
            {
                commands[0] += "NOERR ";
            }

            return ExecuteInternal(commands);
        }

        public static string CreateDifferencingVDisk(string savePath, string parentFilePath, bool isNoErr)
        {
            string[] commands = new string[1];

            commands[0] = "Create VDISK FILE=" + savePath + " PARENT=" + parentFilePath + " ";

            if (isNoErr)
            {
                commands[0] += "NOERR ";
            }

            return ExecuteInternal(commands);
        }

        public static string CreateCopyVDisk(string savePath, string sourceFilePath, bool isNoErr)
        {
            string[] commands = new string[1];

            commands[0] = "Create VDISK FILE=" + savePath + " SOURCE=" + sourceFilePath + " ";

            if (isNoErr)
            {
                commands[0] += "NOERR ";
            }

            return ExecuteInternal(commands);
        }

        public static string AttachVDisk(string filePath, bool isReadOnly, string sddlFlags, bool isNoErr)
        {
            string[] commands = new string[2];

            commands[0] = "Select VDISK FILE=" + filePath + " ";
            commands[1] = "Attach VDISK ";

            if (isReadOnly)
            {
                commands[1] += "readonly ";
            }

            commands[1] += sddlFlags + " ";

            if (isNoErr)
            {
                commands[0] += "NOERR ";
                commands[1] += "NOERR ";
            }

            return ExecuteInternal(commands);
        }

        public static string AttachVDisk(string filePath, bool isReadOnly, bool isNoErr)
        {
            string[] commands = new string[2];

            commands[0] = "Select VDISK FILE=" + filePath + " ";
            commands[1] = "Attach VDISK ";

            if (isReadOnly)
            {
                commands[1] += "readonly ";
            }

            commands[1] += "usefilesd ";

            if (isNoErr)
            {
                commands[0] += "NOERR ";
                commands[1] += "NOERR ";
            }

            return ExecuteInternal(commands);
        }

        public static string CompactVDisk(string filePath, bool isNoErr)
        {
            string[] commands = new string[2];

            commands[0] = "Select VDISK FILE=" + filePath + " ";
            commands[1] = "Compact VDISK ";

            if (isNoErr)
            {
                commands[0] += "NOERR ";
            }

            return ExecuteInternal(commands);
        }

        public static string DetachVDisk(string filePath, bool isReadOnly, bool isNoErr)
        {
            string[] commands = new string[2];

            commands[0] = "Select VDISK FILE=" + filePath + " ";
            commands[1] = "Detach VDISK ";

            if (isNoErr)
            {
                commands[0] += "NOERR ";
                commands[1] += "NOERR ";
            }

            return ExecuteInternal(commands);
        }

        public static string DetailVDisk(string filePath, bool isNoErr)
        {
            string[] commands = new string[2];

            commands[0] = "Select VDISK FILE=" + filePath + " ";
            commands[1] = "Detail VDISK ";

            if (isNoErr)
            {
                commands[0] += "NOERR ";
            }

            return ExecuteInternal(commands);
        }

        public static string ExpandVDisk(string filePath, int maxSizeInMB, bool isNoErr)
        {
            string[] commands = new string[2];

            commands[0] = "Select VDISK FILE=" + filePath + " ";
            commands[1] = "Expand VDISK MAXIMUM=" + maxSizeInMB + " ";

            if (isNoErr)
            {
                commands[0] += "NOERR ";
            }

            return ExecuteInternal(commands);
        }

        public static string MergeVDisk(string filePath, int depth, bool isNoErr)
        {
            string[] commands = new string[2];

            commands[0] = "Select VDISK FILE=" + filePath + " ";
            commands[1] = "Merge VDISK DEPTH=" + depth + " ";

            if (isNoErr)
            {
                commands[0] += "NOERR ";
            }

            return ExecuteInternal(commands);
        }

        #endregion VDisk

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
