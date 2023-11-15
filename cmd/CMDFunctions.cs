using GUIForDiskpart.main;
using System;

namespace GUIForDiskpart.cmd
{
    public static class CMDFunctions
    {
        private static char? yesChar;
        private static char? noChar;

        static CMDFunctions()
        {
            SetYesNoLetters();
        }
        
        private static void SetYesNoLetters()
        {
            string command = "CHOICE";
            string cmdOutput = CommandExecuter.IssueCommandNoWait(ProcessType.CMD, command);
            foreach (char c in new[] {'[', ']', ',', '?'})
            {
                cmdOutput = cmdOutput.Replace(c.ToString(),"");
            }
            cmdOutput = cmdOutput.Trim();

            if (!string.IsNullOrEmpty(cmdOutput))
            {
                yesChar = cmdOutput[0];
                noChar = cmdOutput[1];
            }
            else
            {
                yesChar = 'Y';
                noChar = 'N';
            }
        }

        public static string CHKDSK(char driveLetter, string parameters, string logFileLocation)
        {
            string command = string.Empty;
            string closingCommand = $" DIR > \"{logFileLocation}\" & echo A log file has been created here: {logFileLocation} ";

            command = $"echo {yesChar} | CHKDSK {driveLetter}: {parameters} & {closingCommand}";

            return ExecuteInternal(command);
        }

        public static string CHKDSK(char driveLetter, string parameters)
        {
            string command = string.Empty;

            command = $"echo {yesChar} | CHKDSK {driveLetter}: {parameters} ";

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