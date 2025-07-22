using GUIForDiskpart.Database.Data.CMD;

namespace GUIForDiskpart.Model.Logic.CMD
{
    public static class CMDFunctions
    {
        private const string LOG_FILE_CREATED = "A log file has been created here";

        public static string CHKDSK(char driveLetter, string parameters, string logFileLocation)
        {
            string command = string.Empty;
            string yesNo = CommandExecuter.GetChoiceYNString();
            string closingCommand = $" {CMDBasic.DIR} > \"{logFileLocation}\" & {CMDBasic.ECHO} {LOG_FILE_CREATED}: {logFileLocation} ";

            command = $"{CMDBasic.ECHO} {yesNo[0]} | {CHKDSKParameters.CMD_CHKDSK} {driveLetter}: {parameters} & {closingCommand}";

            return ExecuteInternal(command);
        }

        public static string CHKDSK(char driveLetter, string parameters)
        {
            string command = string.Empty;
            string yesNo = CommandExecuter.GetChoiceYNString();

            command = $"{CMDBasic.ECHO} {yesNo[0]} | {CHKDSKParameters.CMD_CHKDSK} {driveLetter}: {parameters} ";

            return ExecuteInternal(command);
        }

        public static string CHKNTFS(string parameters)
        {
            string command = string.Empty;

            command = $"{CHKDSKParameters.CMD_CHKDSK} {parameters}";

            return ExecuteInternal(command);
        }

        private static string ExecuteInternal(string command)
        {
            string output = CommandExecuter.IssueCommandSeperateCMDWindow(command);

            return output;
        }
    }
}