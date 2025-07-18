using GUIForDiskpart.Database.Data;
using Microsoft.PowerShell;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Numerics;
using static System.Windows.Forms.AxHost;


namespace GUIForDiskpart.Model.Logic
{
    public static class CommandExecuter
    {
        private const string EXE_SUFFIX = ".exe";
        private const string CMD_ECHO_CLOSE_WINDOW = "echo Please close this window & pause >nul";
        private const string STARTED_WITH_PARAMS = "started with parameters:";

        public static List<object> IssuePowershellCommand(string command, string psParam)
        {
            InitialSessionState initialSessionState = InitialSessionState.CreateDefault();
            initialSessionState.ExecutionPolicy = ExecutionPolicy.Unrestricted;

            Runspace runspace = RunspaceFactory.CreateRunspace(initialSessionState);
            runspace.Open();
            List<object> objects = new List<object>();

            Pipeline pipeline = runspace.CreatePipeline();
            pipeline.Commands.AddScript($"{command} {psParam}");

            var results = pipeline.Invoke();

            foreach (var result in results)
            {
                objects.Add(result);
            }

            runspace.Close();


            return objects;
        }

        public static List<PSObject> IssuePowershellCommand(string[] commands)
        {
            InitialSessionState initialSessionState = InitialSessionState.CreateDefault();
            initialSessionState.ExecutionPolicy = ExecutionPolicy.Unrestricted;

            Runspace runspace = RunspaceFactory.CreateRunspace(initialSessionState);
            runspace.Open();
            List<PSObject> objects = new List<PSObject>();

            Pipeline pipeline = runspace.CreatePipeline();
            string fullCommand = string.Empty;
            foreach (string command in commands)
            {
                fullCommand += $"{command}; ";
            }

            pipeline.Commands.AddScript($"{fullCommand}");

            var results = pipeline.Invoke();

            foreach (var result in results)
            {
                objects.Add(result);
            }

            runspace.Close();

            return objects;
        }


        public static string IssueCommand(string processType, string[] commands)
        {
            var process = CreateProcess(processType);

            string output = "";

            process.Start();

            foreach (string command in commands)
            {
                process.StandardInput.WriteLine(command);

                output += processType.ToString().ToUpper() + " - " + command + "\n";
            }

            process.StandardInput.WriteLine(CMDBasic.EXIT);

            output += ReadProcessStandardOutput(process);
            int exitCode = 0;
            exitCode = process.ExitCode;
            output += $"{CommonStrings.EXIT_CODE} {exitCode.ToString()}\n";

            return output;
        }

        public static string IssueCommand(string processType, string command)
        {
            var process = CreateProcess(processType);

            string output = "";

            process.Start();

            process.StandardInput.WriteLine(command);
            output += processType.ToString().ToUpper() + " - " + command + "\n";

            process.StandardInput.WriteLine(CMDBasic.EXIT);

            output += ReadProcessStandardOutput(process);
            int exitCode = ExitProcess(process);
            output += $"{CommonStrings.EXIT_CODE} {exitCode.ToString()}\n";

            return output;
        }

        public static string GetChoiceYNString()
        {
            var info = new ProcessStartInfo(CMDBasic.CHOICE);
            info.UseShellExecute = false;
            info.RedirectStandardOutput = true;
            info.RedirectStandardInput = true;
            info.CreateNoWindow = true;

            var proc = Process.Start(info);

            var result = "";
            while (proc.StandardOutput.Peek() != -1)
            {
                result += (char)proc.StandardOutput.Read();
            }

            foreach (char c in new[] { '[', ']', ',', '?' })
            {
                result = result.Replace(c.ToString(), "");
            }
            result = result.Trim();

            proc.StandardInput.WriteLine(result[1]);
            proc.StandardOutput.Close();
            proc.Close();


            return result;
        }

        public static string IssueCommandSeperateCMDWindow(string command)
        {
            string output = string.Empty;

            Process process = new Process();
            process.StartInfo.FileName = Database.Data.Types.ProcessType.CMD + EXE_SUFFIX;
            process.StartInfo.RedirectStandardInput = true;
            process.Start();
            process.StandardInput.WriteLine($"{command} & {CMD_ECHO_CLOSE_WINDOW}");

            output += $"{process.ProcessName.ToUpper()}{EXE_SUFFIX} {STARTED_WITH_PARAMS}\n";
            output += $"{command} & {CMD_ECHO_CLOSE_WINDOW}";

            Console.WriteLine(output);
            return output;
        }

        private static Process CreateProcess(string processType)
        {
            Process newProcess = new Process();
            newProcess.StartInfo.FileName = processType + EXE_SUFFIX;
            newProcess.StartInfo.CreateNoWindow = true;
            newProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            newProcess.StartInfo.UseShellExecute = false;
            newProcess.StartInfo.RedirectStandardOutput = true;
            newProcess.StartInfo.RedirectStandardInput = true;

            return newProcess;
        }

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
