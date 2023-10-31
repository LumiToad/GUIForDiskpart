using System.Diagnostics;
using System.IO;

namespace GUIForDiskpart.main
{
    public static class CommandExecuter
    {
        //Process process = new Process();

        private const string exeSuffix = ".exe";

        /*

        public string IssueCommand(ProcessType processType, string command)
        {
            SetupProcessInfo(processType);
            
            string output = "";

            process.Start();
            output += ExecuteCommand(processType, command);
            process.StandardInput.WriteLine("exit");

            output += ReadProcessStandardOutput();
            int exitCode = ExitProcess();
            output += "Exit Code: " + exitCode.ToString() + "\n";

            return output;
        }

        */
        public static string IssueCommand(ProcessType processType, string[] commands)
        {
            var process = CreateProcess(processType);

            string output = "";

            process.Start();

            foreach (string command in commands)
            {
                process.StandardInput.WriteLine(command);

                output += processType.ToString().ToUpper() + " - " + command + "\n";
            }

            process.StandardInput.WriteLine("exit");

            output += ReadProcessStandardOutput(process);
            int exitCode = 0;
            exitCode = process.ExitCode;
            output += "Exit Code: " + exitCode.ToString() + "\n";

            return output;
        }

        public static string IssueCommand(ProcessType processType, string command)
        {
            var process = CreateProcess(processType);

            string output = "";

            process.Start();

            process.StandardInput.WriteLine(command);
            output += processType.ToString().ToUpper() + " - " + command + "\n";
            
            process.StandardInput.WriteLine("exit");

            output += ReadProcessStandardOutput(process);
            int exitCode = ExitProcess(process);
            output += "Exit Code: " + exitCode.ToString() + "\n";

            return output;
        }

        /*
        private void SetupProcessInfo(ProcessType processType)
        {
            process.StartInfo.FileName = processType.ToString() + exeSuffix;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardInput = true;
        }
        */

        private static Process CreateProcess(ProcessType processType)
        {
            Process newProcess = new Process();
            newProcess.StartInfo.FileName = processType.ToString() + exeSuffix;
            newProcess.StartInfo.CreateNoWindow = true;
            newProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            newProcess.StartInfo.UseShellExecute = false;
            newProcess.StartInfo.RedirectStandardOutput = true;
            newProcess.StartInfo.RedirectStandardInput = true;

            return newProcess;
        }
        /*
        private string ExecuteCommand(ProcessType processType, string command)
        {
            process.StandardInput.WriteLine(command);

            return processType.ToString().ToUpper() + " - " + command + "\n";
        }
        */

        private static int ExitProcess(Process process)
        {
            process.WaitForExit();

            int exitCode = 0;
            exitCode = process.ExitCode;

            process.Close();
            return exitCode;
        }

        private static string ReadProcessStandardOutput(Process process)
        {
            StreamReader streamReader = process.StandardOutput;
            return streamReader.ReadToEnd();
        }
    }
}
