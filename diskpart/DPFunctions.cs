using GUIForDiskpart.main;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Shapes;

namespace GUIForDiskpart.diskpart
{
    public class DPFunctions
    {
        private CommandExecuter commandExecuter;
        private List<string> dpInfoExcludeStrings = new List<string>();

        public DPFunctions()
        {
            commandExecuter = new CommandExecuter();
;
            GetDPInfoExcludeStrings();
        }

        private void GetDPInfoExcludeStrings()
        {
            string output = commandExecuter.IssueCommand(ProcessType.diskpart, "");

            foreach (string line in output.Split('\r', StringSplitOptions.TrimEntries)) 
            {
                dpInfoExcludeStrings.Add(line);
            }
            dpInfoExcludeStrings.RemoveAt(0);
            dpInfoExcludeStrings.RemoveAt(dpInfoExcludeStrings.Count - 1);
        }

        public string List(DPListType type)
        {
            string command = "List " + type.ToString();

            string output = commandExecuter.IssueCommand(ProcessType.diskpart, command);
            output = RemoveDPInfo(output);

            return output;
        }

        public string Detail(DPListType type, int selection) 
        {
            string[] commands = new string[2];

            commands[0] = "Select " + type.ToString() + " " + selection.ToString();
            commands[1] = "Detail " + type.ToString();

            string output = commandExecuter.IssueCommand(ProcessType.diskpart, commands);
            output = RemoveDPInfo(output);

            return output;
        }

        private string RemoveDPInfo(string info)
        {
            string output = info;
            
            foreach(string line in dpInfoExcludeStrings)
            {
                if (line == "") continue;

                if (info.Contains(line) || info.Contains(" \n"))
                {
                    output = output.Replace(line, "");
                    output = Regex.Replace(output, @"[\r\n]+", "\n");
                }
            }

            return output;
        }
    }
}
