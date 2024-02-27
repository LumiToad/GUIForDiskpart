namespace GUIForDiskpart.cmd
{
    public class CHKDSKParameters
    {
        #region CHKDSK_Parameters

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

        #endregion CHKDSK_Parameters
    }
}
