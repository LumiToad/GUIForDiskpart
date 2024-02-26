using GUIForDiskpart.main;
using System;
using System.IO;
using System.Windows.Forms;

namespace GUIForDiskpart.cmd
{
    public static class CMDFunctions
    {
        private static string YesNoFilename = "ynchoice";
        
        private static bool CheckYesNoFile()
        {
            if (FileUtilites.CheckFileExists(YesNoFilename, ".json")) return true;

            CreateYesNoFile();
            return FileUtilites.CheckFileExists(YesNoFilename, ".json");
        }

        private static string GetYesNoFromFile()
        {
            CMDConfigFile configFile = FileUtilites.LoadConfigFromJSON<CMDConfigFile>(YesNoFilename);
            return configFile.yesNo;
        }

        private static void CreateYesNoFile()
        {
            string cmdOutput = CommandExecuter.GetChoiceYNString();

            CMDConfigFile cmdConfigFile = new() { yesNo = $"{cmdOutput[0]}{cmdOutput[1]}" };
            FileUtilites.SaveConfigAsJSON(cmdConfigFile, YesNoFilename);
        }

        public static string CHKDSK(char driveLetter, string parameters, string logFileLocation)
        {
            string command = string.Empty;
            if (!CheckYesNoFile()) return command;
            string yesNo = GetYesNoFromFile();
            string closingCommand = $" DIR > \"{logFileLocation}\" & echo A log file has been created here: {logFileLocation} ";

            command = $"echo {yesNo[0]} | CHKDSK {driveLetter}: {parameters} & {closingCommand}";

            return ExecuteInternal(command);
        }

        public static string CHKDSK(char driveLetter, string parameters)
        {
            string command = string.Empty;
            if (!CheckYesNoFile()) return command;
            string yesNo = GetYesNoFromFile();

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