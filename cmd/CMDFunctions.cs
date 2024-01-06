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

            string message = "GUIFD will now try to detect the default Yes / No key for your system language.\n\nThis requires a certain workaround, which will cause error beeps from your Windows Command Shell. Click \"Ok\", when you are ready, TURNING OFF THE VOLUME IS SUGGESTED.";
            string title = "Warning!";
            MessageBoxButtons buttons = MessageBoxButtons.OKCancel;
            DialogResult result = MessageBox.Show(message, title, buttons, MessageBoxIcon.Warning);
            if (result == DialogResult.OK)
            {
                CreateYesNoFile();
                return true;
            }
            return false;
        }

        private static string GetYesNoFromFile()
        {
            CMDConfigFile configFile = FileUtilites.LoadConfigFromJSON<CMDConfigFile>(YesNoFilename);
            return configFile.yesNo;
        }

        private static void CreateYesNoFile()
        {
            string command = "echo ABCDEFGHIJKLMNOPQRSTUVWXYZ | CHOICE";
            string cmdOutput = CommandExecuter.IssueCommandNoWait(ProcessType.CMD, command);

            foreach (char c in new[] { '[', ']', ',', '?' })
            {
                cmdOutput = cmdOutput.Replace(c.ToString(), "");
            }
            cmdOutput = cmdOutput.Trim();

            CMDConfigFile cmdConfigFile = new () { yesNo = $"{cmdOutput[0]}{cmdOutput[1]}" };
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