using Microsoft.PowerShell;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Management.Automation;
using System.Management.Automation.Runspaces;


namespace GUIForDiskpart.main
{
    public static class CommandExecuter
    {
        private const string exeSuffix = ".exe";

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

            process.StandardInput.WriteLine("exit");

            output += ReadProcessStandardOutput(process);
            int exitCode = 0;
            exitCode = process.ExitCode;
            output += "Exit Code: " + exitCode.ToString() + "\n";

            return output;
        }

        public static string IssueCommand(string processType, string command)
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

        public static string GetChoiceYNString()
        {
            var info = new ProcessStartInfo("choice");
            info.UseShellExecute = false;
            info.RedirectStandardOutput = true;
            info.CreateNoWindow = true;

            var proc = Process.Start(info);

            var result = "";
            while (proc.StandardOutput.Peek() != -1)
            {
                result += (char)proc.StandardOutput.Read();
            }
            proc.StandardOutput.Close();
            proc.Close();

            return result;
        }

        public static string IssueCommandSeperateCMDWindow(string command)
        {
            string output = string.Empty;

            Process process = new Process();
            process.StartInfo.FileName = ProcessType.CMD + exeSuffix;
            process.StartInfo.RedirectStandardInput = true;
            process.Start();
            process.StandardInput.WriteLine($"{command} & echo Please close this window & pause >nul");

            output += $"{process.ProcessName.ToUpper()}{exeSuffix} started with parameters:\n";
            output += $"{command} & echo Please close this window & pause >nul";

            Console.WriteLine(output);
            return output;
        }

        private static Process CreateProcess(string processType)
        {
            Process newProcess = new Process();
            newProcess.StartInfo.FileName = processType + exeSuffix;
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
