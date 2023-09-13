using GUIForDiskpart.main;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Shapes;

namespace GUIForDiskpart.diskpart
{
    public class DPFunctions
    {
        private CommandExecuter m_commandExecuter;
        private List<string> dpInfoExcludeStrings = new List<string>();

        public DPFunctions(CommandExecuter commandExecuter)
        {
            m_commandExecuter = commandExecuter;
            GetDPInfoExcludeStrings();
        }

        private void GetDPInfoExcludeStrings()
        {
            string output = m_commandExecuter.IssueCommand(ProcessType.diskpart, "");

            

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

            string output = m_commandExecuter.IssueCommand(ProcessType.diskpart, command);
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
