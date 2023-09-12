using System;
using System.Diagnostics;

namespace GUIForDiskpart.main
{
    internal class Test
    {
        public void TestFunc()
        {
            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = false;
            process.StartInfo.RedirectStandardInput = true;
            //process.StartInfo.RedirectStandardOutput = true;
            process.Start();
            process.StandardInput.WriteLine("diskpart");
            process.StandardInput.WriteLine("list volume");
            //string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            //Console.WriteLine(output);
        }
    }
}
