namespace GUIForDiskpart.Database.Data.CMD
{
    public class Basic
    {
        #region Shutdown

        public const string CMD_SHUTDOWN = "SHUTDOWN";
        public const string SHUTDOWN_NO_TIMER = "/s";
        public const string SHUTDOWN_FORCE = "/f";
        public const string SHUTDOWN_TIMER = "/t";
        public const string SHUTDOWN_RESTART = "/r";

        #endregion Shutdown

        public const string START_BROWSER = "start ";
    }

    #region CHKDSK

    public class CHKDSKParameters
    {
        public const string CMD_CHKDSK = "CHKDSK";
        public const string FIXERRORS = "/f";
        public const string DISPLAYNAMEOFEACHFILE = "/v";
        public const string LOCATEBAD = "/r";
        public const string FIXERRORSFORCEDISMOUNT = "/x";
        public const string LESSVIGOROUS_NTFS = "/i";
        public const string NOCYCLECHECKNTFS = "/c";
        public const string LOGFILESIZE_NTFS = "/l:";
        public const string CLEARBADSECTORLIST_NTFS = "/b";
        public const string SCAN_NTFS = "/scan";
        public const string FORCEOFFLINEFIX_NTFS = "/forceofflinefix";
        public const string FULLPERFORMANCE = "/perf";
        public const string SPOTFIX = "/spotfix";
        public const string SDCLEAN_NTFS = "/sdcleanup";
        public const string OFFLINESCANANDFIX = "/offlinescanandfix";
        public const string FREEORPHANED_FATFAMILY = "/freeorphanedchains";
        public const string MARKCLEAN_FATFAMILY = "/markclean";
    }

    #endregion CHKDSK

    #region CHKNTFS

    public class CHKNTFSParameters
    {
        public const string CMD_CHKNTFS = "CHKNTFS";
        public const string DEFAULTSETTINGS = "/d";
        public const string SETTIMER = "/t:";
        public const string EXCLUDEVOLUMES = "/x";
        public const string QUEUEMULTIVOLUME = "/c";

    }

    #endregion CHKNTFS
}
