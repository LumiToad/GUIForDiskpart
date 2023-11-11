namespace GUIForDiskpart.cmd
{
    public class CMDBasicCommands
    {
        #region CMD_Basic_Commands

        private const string shutdownNoTimer = "/s";
        public static string ShutdownNoTimer => shutdownNoTimer;

        private const string shutdownForce = "/f";
        public static string ShutdownForce => shutdownForce;

        private const string shutdownTimer = "/t";
        public static string ShutdownTimer => shutdownTimer;

        private const string shutdownRestart = "/r";
        public static string ShutdownRestart => shutdownRestart;

        #endregion CMD_Basic_Commands
    }
}
