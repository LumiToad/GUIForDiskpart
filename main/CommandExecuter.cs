using System.Diagnostics;
using System.IO;

namespace GUIForDiskpart.main
{
    public class CommandExecuter
    {
        ProcessStartInfo processInfo;
        Process process = new Process();

        public string ExecuteCommand(string command, ref int exitCode)
        {
            string output = string.Empty;
            processInfo = new ProcessStartInfo("diskpart.exe");
            processInfo.CreateNoWindow = true;
            processInfo.WindowStyle = ProcessWindowStyle.Hidden;
            processInfo.UseShellExecute = false;
            processInfo.RedirectStandardOutput = true;
            processInfo.RedirectStandardInput = true;
            process.StartInfo = processInfo;

            process = Process.Start(processInfo);
            process.StandardInput.WriteLine(command);
            process.StandardInput.WriteLine("exit");
            StreamReader streamReader = process.StandardOutput;
            output = streamReader.ReadToEnd();
            exitCode = process.ExitCode;
            process.Close();
            return output;
        }
    }
}
