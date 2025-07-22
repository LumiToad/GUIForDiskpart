using System;
using System.Collections.Generic;

using GUIForDiskpart.Database.Data;
using GUIForDiskpart.Utils;


namespace GUIForDiskpart.Model.Logic.Diskpart
{
    public static class DPFunctions
    {
        private static List<string> dpInfoExcludeStrings = new List<string>();

        static DPFunctions()
        {
            GetDPInfoExcludeStrings();
        }

        #region ListingAndDetail

        public static string List(string type)
        {
            string[] commands = new string[1];

            commands[0] = $"{Basics.LIST} {type} ";

            return ExecuteInternal(commands);
        }

        public static string ListPart(uint? index)
        {
            string[] commands = new string[2];

            commands[0] = $"{Basics.SELECT} {Basics.DISK} {index} ";
            commands[1] = $"{Basics.LIST} {Basics.PARTITION} ";

            return ExecuteInternal(commands);
        }

        public static string DetailDisk(uint diskIndex)
        {
            string[] commands = new string[2];

            commands[0] = $"{Basics.SELECT} {Basics.DISK} {diskIndex} ";
            commands[1] = $"{Basics.DETAIL} {Basics.DISK} ";

            return ExecuteInternal(commands);
        }

        public static string DetailPart(uint diskIndex, uint partIndex)
        {
            string[] commands = new string[3];

            commands[0] = $"{Basics.SELECT} {Basics.DISK} {diskIndex} ";
            commands[1] = $"{Basics.SELECT} {Basics.PART} {partIndex} ";
            commands[2] = $"{Basics.DETAIL} {Basics.PART} ";

            return ExecuteInternal(commands);
        }

        #endregion ListingAndDetail

        #region Create

        public static string CreatePartition(uint diskIndex, string option, ulong sizeInMB, bool isNoErr)
        {
            string[] commands = new string[2];

            commands[0] = $"{Basics.SELECT} {Basics.DISK} {diskIndex} ";
            commands[1] = $"{Create.CREATE} {Create.PARTITION} {option} ";

            if (sizeInMB > 0)
            {
                commands[1] += $"{Create.SIZE}= {sizeInMB} ";
            }

            if (isNoErr)
            {
                commands[1] += $"{Basics.NOERR} ";
            }

            return ExecuteInternal(commands);
        }

        public static string CreateVolume(uint diskIndex, string option, ulong sizeInMB, bool isNoErr)
        {
            string[] commands = new string[1];

            commands[0] = $"{Create.CREATE} {Create.VOLUME} {option} {Basics.DISK}={diskIndex} ";

            if (sizeInMB > 0)
            {
                commands[0] += $"{Create.SIZE}= {sizeInMB} ";
            }

            if (isNoErr)
            {
                commands[0] += $"{Basics.NOERR} ";
            }

            return ExecuteInternal(commands);
        }

        public static string CreateVolume(uint[] diskIndexes, string option, ulong sizeInMB, uint alignInKB, bool isNoErr)
        {
            string[] commands = new string[1];

            commands[0] = $"{Create.CREATE} {Create.VOLUME} {Create.SIMPLE} {option} ";

            if (sizeInMB > 0)
            {
                commands[0] += $"{Create.SIZE}= {sizeInMB} ";
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

            commands[0] += $"{Create.ALIGN}= {alignInKB} ";

            if (isNoErr)
            {
                commands[0] += $"{Basics.NOERR} ";
            }

            return ExecuteInternal(commands);
        }

        #endregion Create

        #region VDisk

        public static string CreateVDisk(string savePath, bool isFixed, int maxSizeInMB, string sddlFlags, bool isNoErr)
        {
            string[] commands = new string[1];

            commands[0] = $"{Create.CREATE} {Create.VDISK} {Basics.FILE}={savePath} ";

            if (isFixed)
            {
                commands[0] += $"{Create.TYPE}={Create.FIXED} ";
            }
            else
            {
                commands[0] += $"{Create.TYPE}={Create.EXPANDABLE} ";
            }

            commands[0] += $"{Create.MAXIMUM}= {maxSizeInMB} ";

            commands[0] += $"{Create.SD}= {sddlFlags} ";

            if (isNoErr)
            {
                commands[0] += $"{Basics.NOERR} ";
            }

            return ExecuteInternal(commands);
        }

        public static string CreateVDisk(string savePath, bool isFixed, int maxSizeInMB, bool isNoErr)
        {
            string[] commands = new string[1];

            commands[0] = $"{Create.CREATE} {Create.VDISK} {Basics.FILE}={savePath} ";

            if (isFixed)
            {
                commands[0] += $"{Create.TYPE}={Create.FIXED} ";
            }
            else
            {
                commands[0] += $"{Create.TYPE}={Create.EXPANDABLE} ";
            }

            commands[0] += $"{Create.MAXIMUM}={maxSizeInMB} ";

            if (isNoErr)
            {
                commands[0] += $"{Basics.NOERR} ";
            }

            return ExecuteInternal(commands);
        }

        public static string CreateDifferencingVDisk(string savePath, string parentFilePath, bool isNoErr)
        {
            string[] commands = new string[1];

            commands[0] = $"{Create.CREATE} {Create.VDISK} {Basics.FILE}={savePath} {Create.PARENT}={parentFilePath} ";

            if (isNoErr)
            {
                commands[0] += $"{Basics.NOERR} ";
            }

            return ExecuteInternal(commands);
        }

        public static string CreateCopyVDisk(string savePath, string sourceFilePath, bool isNoErr)
        {
            string[] commands = new string[1];

            commands[0] = $"{Create.CREATE} {Create.VDISK} {Basics.FILE}={savePath} {Create.SOURCE}={sourceFilePath} ";

            if (isNoErr)
            {
                commands[0] += $"{Basics.NOERR} ";
            }

            return ExecuteInternal(commands);
        }

        public static string AttachVDisk(string filePath, bool isReadOnly, string sddlFlags, bool isNoErr)
        {
            string[] commands = new string[2];

            commands[0] = $"{Basics.SELECT} {VDisk.VDISK} {Basics.FILE}={filePath} ";
            commands[1] = $"{VDisk.ATTACH} {VDisk.VDISK} ";

            if (isReadOnly)
            {
                commands[1] += $"{VDisk.READONLY} ";
            }

            commands[1] += $"{sddlFlags} ";

            if (isNoErr)
            {
                commands[0] += $"{Basics.NOERR} ";
                commands[1] += $"{Basics.NOERR} ";
            }

            return ExecuteInternal(commands);
        }

        public static string AttachVDisk(string filePath, bool isReadOnly, bool isNoErr)
        {
            string[] commands = new string[2];

            commands[0] = $"{Basics.SELECT} {VDisk.VDISK} {Basics.FILE}={filePath} ";
            commands[1] = $"{VDisk.ATTACH} {VDisk.VDISK} ";

            if (isReadOnly)
            {
                commands[1] += $"{VDisk.READONLY} ";
            }

            commands[1] += $"{VDisk.USEFILESD} ";

            if (isNoErr)
            {
                commands[0] += $"{Basics.NOERR} ";
                commands[1] += $"{Basics.NOERR} ";
            }

            return ExecuteInternal(commands);
        }

        public static string CompactVDisk(string filePath, bool isNoErr)
        {
            string[] commands = new string[2];

            commands[0] = $"{Basics.SELECT} {VDisk.VDISK} {Basics.FILE}={filePath} ";
            commands[1] = $"{VDisk.COMPACT} {VDisk.VDISK} ";

            if (isNoErr)
            {
                commands[0] += $"{Basics.NOERR} ";
            }

            return ExecuteInternal(commands);
        }

        public static string DetachVDisk(string filePath, bool isReadOnly, bool isNoErr)
        {
            string[] commands = new string[2];

            commands[0] = $"{Basics.SELECT} {VDisk.VDISK} {Basics.FILE}={filePath} ";
            commands[1] = $"{VDisk.DETACH} {VDisk.VDISK} ";

            if (isNoErr)
            {
                commands[0] += $"{Basics.NOERR} ";
                commands[1] += $"{Basics.NOERR} ";
            }

            return ExecuteInternal(commands);
        }

        public static string DetailVDisk(string filePath, bool isNoErr)
        {
            string[] commands = new string[2];

            commands[0] = $"{Basics.SELECT} {VDisk.VDISK} {Basics.FILE}={filePath} ";
            commands[1] = $"{Basics.DETAIL} {VDisk.VDISK} ";

            if (isNoErr)
            {
                commands[0] += $"{Basics.NOERR} ";
            }

            return ExecuteInternal(commands);
        }

        public static string ExpandVDisk(string filePath, int maxSizeInMB, bool isNoErr)
        {
            string[] commands = new string[2];

            commands[0] = $"{Basics.SELECT} {VDisk.VDISK} {Basics.FILE}={filePath} ";
            commands[1] = $"{VDisk.EXPAND} {VDisk.VDISK} {VDisk.MAXIMUM}={maxSizeInMB} ";

            if (isNoErr)
            {
                commands[0] += $"{Basics.NOERR} ";
            }

            return ExecuteInternal(commands);
        }

        public static string MergeVDisk(string filePath, int depth, bool isNoErr)
        {
            string[] commands = new string[2];

            commands[0] = $"{Basics.SELECT} {VDisk.VDISK} {Basics.FILE}={filePath} ";
            commands[1] = $"{VDisk.MERGE} {VDisk.VDISK} {VDisk.DEPTH}={depth} ";

            if (isNoErr)
            {
                commands[0] += $"{Basics.NOERR} ";
            }

            return ExecuteInternal(commands);
        }

        #endregion VDisk

        #region Various

        public static string Format(uint diskIndex, uint partitionIndex, FSType fileSystem,
            string volumeName, bool isQuickFormatting, bool isCompressed, bool isOverride, bool isNoWait, bool isNoErr)
        {
            string[] commands = new string[3];

            commands[0] = $"{Basics.SELECT} {Basics.DISK} {diskIndex} ";
            commands[1] = $"{Basics.SELECT} {Basics.PARTITION} {partitionIndex} ";
            commands[2] = $"{Database.Data.DP.Format.FORMAT} {Database.Data.DP.Format.FS}={fileSystem} ";

            if (volumeName != "")
            {
                commands[2] += $"{Database.Data.DP.Format.LABEL}={volumeName} ";
            }

            if (isQuickFormatting)
            {
                commands[2] += $"{Database.Data.DP.Format.QUICK} ";
            }

            if (isCompressed && fileSystem == FSType.NTFS)
            {
                commands[2] += $"{Database.Data.DP.Format.COMPRESS} ";
            }

            if (isOverride)
            {
                commands[2] += $"{Database.Data.DP.Format.OVERRIDE} ";
            }

            if (isNoWait)
            {
                commands[2] += $"{Database.Data.DP.Format.NOWAIT} ";
            }

            if (isNoErr)
            {
                commands[2] += $"{Basics.NOERR} ";
            }

            return ExecuteInternal(commands);
        }

        public static string Delete(uint diskIndex, uint partitionIndex, bool isNoErr, bool isOverride)
        {
            string[] commands = new string[3];

            commands[0] = $"{Basics.SELECT} {Basics.DISK} {diskIndex}";
            commands[1] = $"{Basics.SELECT} {Basics.PARTITION} {partitionIndex}";
            commands[2] = $"{Database.Data.DP.Delete.DELETE} {Basics.PART} ";

            if (isNoErr)
            {
                commands[2] += $"{Basics.NOERR} ";
            }

            if (isOverride)
            {
                commands[2] += $"{Database.Data.DP.Delete.OVERRIDE} ";
            }

            return ExecuteInternal(commands);
        }

        public static string Assign(uint diskIndex, uint partitionIndex, char? diskLetter, bool isNoErr)
        {
            string[] commands = new string[3];

            commands[0] = $"{Basics.SELECT} {Basics.DISK} {diskIndex}";
            commands[1] = $"{Basics.SELECT} {Basics.PARTITION} {partitionIndex}";
            commands[2] = $"{Basics.ASSIGN} ";

            if (diskLetter != null)
            {
                commands[2] += $"{Basics.LETTER}={diskLetter} ";
            }

            if (isNoErr)
            {
                commands[2] += $"{Basics.NOERR} ";
            }

            return ExecuteInternal(commands);
        }

        public static string Assign(uint diskIndex, uint partitionIndex, bool isNoErr)
        {
            return Assign(diskIndex, partitionIndex, null, isNoErr);
        }

        public static string Remove(char driveLetter, bool isDismount, bool isNoErr)
        {
            string[] commands = new string[2];

            commands[0] = $"{Basics.SELECT} {Basics.VOLUME} {driveLetter}";
            commands[1] = $"{Database.Data.DP.Remove.REMOVE} {Basics.LETTER}={driveLetter} ";

            if (isDismount)
            {
                commands[1] += $"{Database.Data.DP.Remove.DISMOUNT} ";
            }

            if (isNoErr)
            {
                commands[1] += $"{Basics.NOERR} ";
            }

            return ExecuteInternal(commands);
        }

        public static string Remove(char driveLetter, string mountPath, bool isDismount, bool isNoErr)
        {
            string[] commands = new string[2];

            commands[0] = $"{Basics.SELECT} {Basics.VOLUME} {driveLetter}";
            commands[1] = $"{Database.Data.DP.Remove.REMOVE} {Database.Data.DP.Remove.MOUNT}={mountPath} ";

            if (isDismount)
            {
                commands[1] += $"{Database.Data.DP.Remove.DISMOUNT} ";
            }

            if (isNoErr)
            {
                commands[1] += $"{Basics.NOERR} ";
            }

            return ExecuteInternal(commands);
        }

        public static string RemoveAll(uint partitionIndex, bool isDismount, bool isNoErr)
        {
            string[] commands = new string[2];

            commands[0] = $"{Basics.SELECT} {Basics.PARTITION} {partitionIndex} ";
            commands[1] = $"{Database.Data.DP.Remove.REMOVE} {Basics.ALL} ";

            if (isDismount)
            {
                commands[1] += $"{Database.Data.DP.Remove.DISMOUNT} ";
            }

            if (isNoErr)
            {
                commands[1] += $"{Basics.NOERR} ";
            }

            return ExecuteInternal(commands);
        }

        public static string Clean(uint diskIndex, bool isCleanAll)
        {
            string[] commands = new string[2];

            commands[0] = $"{Basics.SELECT} {Basics.DISK} {diskIndex} ";
            commands[1] = $"{Basics.CLEAN} ";

            if (isCleanAll)
            {
                commands[1] += $"{Basics.ALL} ";
            }

            return ExecuteInternal(commands);
        }

        public static string Convert(uint diskIndex, string options)
        {
            string[] commands = new string[2];

            commands[0] = $"{Basics.SELECT} {Basics.DISK} {diskIndex} ";
            commands[1] = $"{Basics.CONVERT} {options} ";

            return ExecuteInternal(commands);
        }

        public static string Active(uint diskIndex, uint partitionIndex, bool isSetActive)
        {
            string[] commands = new string[3];

            commands[0] = $"{Basics.SELECT} {Basics.DISK} {diskIndex} ";
            commands[1] = $"{Basics.SELECT} {Basics.PARTITION} {partitionIndex} ";
            commands[2] = $"{(isSetActive ? Basics.ACTIVE : Basics.INACTIVE)} ";

            return ExecuteInternal(commands);
        }

        public static string AttributesVolume(char driveLetter, bool isSet, string option, bool isNoErr)
        {
            string[] commands = new string[2];

            commands[0] = $"{Basics.SELECT} {Basics.VOLUME} {driveLetter}";
            commands[1] = $"{Attributes.ATTRIBUTES} {Basics.VOLUME} ";

            commands[1] += $"{(isSet ? Attributes.SET : Attributes.CLEAR)} ";
            commands[1] += $"{option} ";

            if (isNoErr)
            {
                commands[1] += $"{Basics.NOERR} ";
            }

            return ExecuteInternal(commands);
        }

        public static string AttributesVolume(int volumeIndex, bool isSet, string option, bool isNoErr)
        {
            string[] commands = new string[2];

            commands[0] = $"{Basics.SELECT} {Basics.VOLUME} {volumeIndex}";
            commands[1] = $"{Attributes.ATTRIBUTES} {Basics.VOLUME} ";

            commands[1] += $"{(isSet ? Attributes.SET : Attributes.CLEAR)} ";
            commands[1] += $"{option} ";

            if (isNoErr)
            {
                commands[1] += $"{Basics.NOERR} ";
            }

            return ExecuteInternal(commands);
        }

        public static string AttributesDisk(uint diskIndex, bool isSet, bool isNoErr)
        {
            string[] commands = new string[2];

            commands[0] = $"{Basics.SELECT} {Basics.DISK} {diskIndex} ";
            commands[1] = $"{Attributes.ATTRIBUTES} {Basics.DISK} ";
            commands[1] += $"{(isSet ? Attributes.SET : Attributes.CLEAR)} ";
            commands[1] += $"{Attributes.READONLY} ";

            if (isNoErr)
            {
                commands[1] += $"{Basics.NOERR} ";
            }

            return ExecuteInternal(commands);
        }

        public static string Extend(uint diskIndex, char driveLetter, uint sizeInMB, bool isNoErr)
        {
            string[] commands = new string[3];

            commands[0] = $"{Basics.SELECT} {Basics.DISK} {diskIndex} ";
            commands[1] = $"{Basics.SELECT} {Basics.VOLUME} {driveLetter}";
            commands[2] = $"{Basics.EXTEND} ";

            if (sizeInMB > 0)
            {
                commands[2] += $"{Basics.SIZE}={sizeInMB} ";
            }

            if (isNoErr)
            {
                commands[2] += $"{Basics.NOERR} ";
            }

            return ExecuteInternal(commands);
        }

        public static string Extend(uint diskIndex, uint partitionIndex, uint sizeInMB, bool isNoErr)
        {
            string[] commands = new string[3];

            commands[0] = $"{Basics.SELECT} {Basics.DISK} {diskIndex} ";
            commands[1] = $"{Basics.SELECT} {Basics.PARTITION} {partitionIndex} ";
            commands[2] = $"{Basics.EXTEND} ";

            if (sizeInMB > 0)
            {
                commands[2] += $"{Basics.SIZE}={sizeInMB} ";
            }

            if (isNoErr)
            {
                commands[2] += $"{Basics.NOERR} ";
            }

            return ExecuteInternal(commands);
        }

        public static string Shrink(char driveLetter, uint desiredInMB, uint minimumInMB, bool isNoWait, bool isNoErr)
        {
            string[] commands = new string[2];

            commands[0] = $"{Basics.SELECT} {Basics.VOLUME} {driveLetter}";
            commands[1] = $"{Database.Data.DP.Shrink.SHRINK} ";

            if (desiredInMB > 0)
            {
                commands[1] += $"{Database.Data.DP.Shrink.DESIRED}={desiredInMB} ";
            }

            if (minimumInMB > 0)
            {
                commands[1] += $"{Database.Data.DP.Shrink.MINIMUM}={minimumInMB} ";
            }

            if (isNoWait)
            {
                commands[1] += $"{Database.Data.DP.Shrink.NOWAIT} ";
            }

            if (isNoErr)
            {
                commands[1] += $"{Basics.NOERR} ";
            }

            return ExecuteInternal(commands);
        }

        public static string OfflineVolume(char driveLetter, bool isNoErr)
        {
            string[] commands = new string[3];

            commands[0] = $"{Basics.SELECT} {Basics.VOLUME} {driveLetter}";
            commands[1] = $"{Database.Data.DP.Remove.REMOVE} {Basics.LETTER}={driveLetter} ";
            commands[2] = $"{CommonStrings.OFFLINE } {Basics.VOLUME} ";

            if (isNoErr)
            {
                commands[1] += $"{Basics.NOERR} ";
            }

            return ExecuteInternal(commands);
        }

        public static string OfflineVolume(uint diskIndex, uint partitionIndex, bool isNoErr)
        {
            string[] commands = new string[3];

            commands[0] = $"{Basics.SELECT} {Basics.DISK} {diskIndex} ";
            commands[1] = $"{Basics.SELECT} {Basics.PARTITION} {partitionIndex} ";
            commands[2] = $"{CommonStrings.OFFLINE} {Basics.VOLUME} ";

            if (isNoErr)
            {
                commands[1] += $"{Basics.NOERR} ";
            }

            return ExecuteInternal(commands);
        }

        public static string OnlineVolume(uint diskIndex, uint partitionIndex, bool isNoErr)
        {
            string[] commands = new string[3];

            commands[0] = $"{Basics.SELECT} {Basics.DISK} {diskIndex} ";
            commands[1] = $"{Basics.SELECT} {Basics.PARTITION} {partitionIndex} ";
            commands[2] = $"{CommonStrings.ONLINE} {Basics.VOLUME} ";

            if (isNoErr)
            {
                commands[1] += $"{Basics.NOERR} ";
            }

            return ExecuteInternal(commands);
        }

        public static string OnOfflineDisk(uint diskIndex, bool isSetOnline, bool isNoErr)
        {
            string[] commands = new string[2];

            commands[0] = $"{Basics.SELECT} {Basics.DISK} {diskIndex} ";
            commands[1] = $"{CommonStrings.OFFLINE} ";

            if (isSetOnline)
            {
                commands[1] = $"{CommonStrings.ONLINE} ";
            }

            commands[1] += $"{Basics.DISK} ";

            if (isNoErr)
            {
                commands[1] += $"{Basics.NOERR} ";
            }

            return ExecuteInternal(commands);
        }

        public static string SetID(uint partitionIndex, string option, bool isOverride, bool isNoErr)
        {
            string[] commands = new string[2];

            commands[0] = $"{Basics.SELECT} {Basics.PARTITION} {partitionIndex} ";
            commands[1] = $"{SetIDO.SET} {SetIDO.ID}={option} ";

            if (isOverride)
            {
                commands[1] += $"{SetIDO.OVERRIDE} ";
            }

            if (isNoErr)
            {
                commands[1] += $"{Basics.NOERR} ";
            }

            return ExecuteInternal(commands);
        }

        #endregion Various

        #region Private

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
                    output = fullOutput.UnifyWhiteSpace();
                }
            }

            return output;
        }

        private static void GetDPInfoExcludeStrings()
        {
            string output = CommandExecuter.IssueCommand(Database.Data.Types.ProcessType.DISKPART, "");

            foreach (string line in output.Split('\r', StringSplitOptions.TrimEntries))
            {
                dpInfoExcludeStrings.Add(line);
            }
            dpInfoExcludeStrings.RemoveAt(0);
            dpInfoExcludeStrings.RemoveAt(dpInfoExcludeStrings.Count - 1);
        }

        private static string ExecuteInternal(string[] commands)
        {
            string fullOutput = CommandExecuter.IssueCommand(Database.Data.Types.ProcessType.DISKPART, commands);
            string output = RemoveDPInfo(fullOutput);

            return output;
        }

        #endregion Private
    }
}
