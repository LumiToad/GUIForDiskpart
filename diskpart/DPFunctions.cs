using GUIForDiskpart.main;
using System;

namespace GUIForDiskpart.diskpart
{
    public class DPFunctions
    {
        private CommandExecuter m_commandExecuter;

        public DPFunctions(CommandExecuter commandExecuter)
        {
            m_commandExecuter = commandExecuter;
        }

        public string List(DPListType type)
        {
            int exitcode = 0;

            string command = "list " + type.ToString();
            Console.Write(command);
            return m_commandExecuter.ExecuteCommand(command, ref exitcode);
        }
    }
}
