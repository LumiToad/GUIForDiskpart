using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace GUIForDiskpart.main
{
    public class ProcessHandler
    {
        private const string executeableSuffix = ".exe";

        List<Process> activeProcesses = new List<Process>();

        public void CallProcess(ProcessType processType)
        {
            Process process = new Process();

            SetupProcess(processType, process);
            StartProcess(process);

            activeProcesses.Add(process);
        }

        public void KillProcess(ProcessType processType, bool killByForce)
        {
            Process process = GetProcessByEnum(processType);

            if (activeProcesses.Contains(process))
            {
                if (!killByForce) 
                {
                    IssueCommand(,"exit");
                }
                else
                {
                    process.Kill();
                }
            }
        }

        private void SetupProcess(ProcessType processType, Process process)
        {
            process.StartInfo.FileName = processType.ToString() + executeableSuffix;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
        }

        private bool StartProcess(Process process)
        {
            if (!process.Start())
            { return false; }

            return true;
        }

        public string IssueCommand(ProcessType processType, string command)
        {
            string output = string.Empty;

            Process process = GetProcessByEnum(processType);

            process.StandardInput.WriteLine(command);
            output = process.StandardOutput.ReadToEnd();

            return "";
        }

        private Process GetProcessByEnum(ProcessType processType)
        {
            foreach (Process process in activeProcesses) 
            {
                if (process.ProcessName == processType.ToString() + executeableSuffix)
                {
                    return process; 
                }
            }

            return null;
        }
    }
}
