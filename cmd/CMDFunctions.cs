using GUIForDiskpart.main;
using System;
using System.IO;
using System.Windows.Forms;

namespace GUIForDiskpart.cmd
{
    public static class CMDFunctions
    {
        public static string CHKDSK(char driveLetter, string parameters, string logFileLocation)
        {
            string command = string.Empty;
            string yesNo = CommandExecuter.GetChoiceYNString();
            string closingCommand = $" DIR > \"{logFileLocation}\" & echo A log file has been created here: {logFileLocation} ";

            command = $"echo {yesNo[0]} | CHKDSK {driveLetter}: {parameters} & {closingCommand}";

            return ExecuteInternal(command);
        }

        public static string CHKDSK(char driveLetter, string parameters)
        {
            string command = string.Empty;
            string yesNo = CommandExecuter.GetChoiceYNString();

            command = $"echo {yesNo[0]} | CHKDSK {driveLetter}: {parameters} ";

            return ExecuteInternal(command);
        }

        public static string CHKNTFS(string parameters) 
        {
            string command = string.Empty;

            command = $"CHKDSK {parameters}";

            return ExecuteInternal(command);
        }

        private static string ExecuteInternal(string command)
        {
            string output = CommandExecuter.IssueCommandSeperateCMDWindow(command);

            return output;
        }
    }
}