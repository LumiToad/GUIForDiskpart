using System.Diagnostics;
using System.IO;

namespace GUIForDiskpart.main
{
    public class CommandExecuter
    {
        Process process = new Process();

        private const string exeSuffix = ".exe";

        public string IssueCommand(ProcessType processType, string command)
        {
            SetupProcessInfo(processType);
            
            string output = "";
            output += ExecuteCommand(processType, command);
            output += ReadProcessStandardOutput();
            int exitCode = ExitProcess();
            output += "Exit Code: " + exitCode.ToString() + "\n";

            return output;
        }

        private void SetupProcessInfo(ProcessType processType)
        {
            process.StartInfo.FileName = processType.ToString() + exeSuffix;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardInput = true;
        }

        private string ExecuteCommand(ProcessType processType, string command)
        {
            process.Start();
            process.StandardInput.WriteLine(command);
            process.StandardInput.WriteLine("exit");

            return processType.ToString().ToUpper() + " - " + command + "\n";
        }

        private int ExitProcess()
        {
            process.WaitForExit();

            int exitCode = 0;
            exitCode = process.ExitCode;

            process.Close();
            return exitCode;
        }

        private string ReadProcessStandardOutput()
        {
            StreamReader streamReader = process.StandardOutput;
            return streamReader.ReadToEnd();
        }
    }
}
