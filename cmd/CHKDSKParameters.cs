namespace GUIForDiskpart.cmd
{
    public class CHKDSKParameters
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
    }
}
