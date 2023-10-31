using Microsoft.PowerShell;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Management.Automation.Runspaces;

namespace GUIForDiskpart.main
{
    public static class CommandExecuter
    {
        private const string exeSuffix = ".exe";

        /*
        public static List<Object> IssuePowershellCommand(string command, string psParam)
        {
            PowerShell ps = PowerShell.Create();
            List<object> objects = new List<object>();

            ps.AddCommand("Set-ExecutionPolicy")
                .AddParameter("ExecutionPolicy ", ExecutionPolicy.Unrestricted)
                .AddParameter("Scope ", ExecutionPolicyScope.Process)
                ;

            ps.AddCommand(command)
                .AddParameter(psParam)
                ;

            IAsyncResult asyncpl = ps.BeginInvoke();

            foreach (PSObject result in ps.EndInvoke(asyncpl))
            {
                objects.Add(result);
            }

            return objects;
        }
        */

        public static List<Object> IssuePowershellCommand(string command, string psParam)
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
