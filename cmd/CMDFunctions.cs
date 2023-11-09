using GUIForDiskpart.main;
using System.ComponentModel.Design.Serialization;

namespace GUIForDiskpart.cmd
{
    public static class CMDFunctions
    {
        #region CHKDSK_Parameters

        private const string fixErrors = "/f";
        public static string FixErrors => fixErrors;
        
        private const string displayNameOfEachFile = "/v";
        public static string DisplayNameOfEachFile => displayNameOfEachFile;

        private const string locateBad = "/r";
        public static string LocateBad => locateBad;

        private const string fixErrorsForceDismount = "/x";
        public static string FixErrorsForceDismount => fixErrorsForceDismount;

        private const string lessVigorousNTFS = "/i";
        public static string LessVigorousNTFS => lessVigorousNTFS;

        private const string noCycleCheckNTFS = "/c";
        public static string NoCycleCheckNTFS => noCycleCheckNTFS;

        private const string logFileSizeNTFS = "/l:";
        public static string LogFileSizeNTFS => logFileSizeNTFS;

        private const string clearBadSectorListNTFS = "/b";
        public static string ClearBadSectorListNTFS => clearBadSectorListNTFS;

        private const string scanNTFS = "/scan";
        public static string ScanNTFS => scanNTFS;

        private const string forceOffLineFixNTFS = "/forceofflinefix";
        public static string ForceOffLineFixNTFS => forceOffLineFixNTFS;

        private const string fullPerformance = "/perf";
        public static string FullPerformance => fullPerformance;

        private const string spotFix = "/spotfix";
        public static string SpotFix => spotFix;

        private const string sdCleanNTFS = "/sdcleanup";
        public static string SDCleanNTFS => sdCleanNTFS;

        private const string offlineScanAndFix = "/offlinescanandfix";
        public static string OfflineScanAndFix => offlineScanAndFix;

        private const string freeOrphanedFATFamily = "/freeorphanedchains";
        public static string FreeOrphanedFATfamily => freeOrphanedFATFamily;

        private const string markCleanFATFamily = "/markclean";
        public static string MarkCleanFATfamily => markCleanFATFamily;

        #endregion CHKDSK_Parameters

        #region CHKNTFS_Parameters

        private const string defaultSettings = "/d";
        public static string DefaultSettings => defaultSettings;

        private const string setTimer = "/t:";
        public static string SetTimer => setTimer;

        private const string excludeVolumes = "/x";
        public static string ExcludeVolumes => excludeVolumes;

        private const string queueMultiVolume = "/c";
        public static string QueueMultiVolume => queueMultiVolume;

        #endregion CHKNTFS_Parameters

        public static string CHKDSK(char driveLetter, string parameters)
        {
            string command = string.Empty;

            command = $"CHKDSK {driveLetter}: {parameters}";

            return ExecuteInternal(command);
        }

        public static string CHKNTFS(string parameters) 
        {
            string command = string.Empty;

            command = $"CHKDSK {parameters}";

            return ExecuteInternal(command);
        }
        
        /*
        private static string ExecuteInternal(string command)
        {
            string fullOutput = CommandExecuter.IssueCommand(ProcessType.CMD, command);
            string output = (fullOutput);

            return output;
        }
        */

        private static string ExecuteInternal(string command)
        {
            string output = CommandExecuter.IssueCommandSeperateCMDWindow(command);

            return output;
        }
    }
}