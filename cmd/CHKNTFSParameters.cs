namespace GUIForDiskpart.cmd
{
    public class CHKNTFSParameters
    {
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
    }
}
