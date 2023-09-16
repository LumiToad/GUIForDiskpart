using GUIForDiskpart.diskpart;

namespace GUIForDiskpart.main
{
    public class MainProgram
    {
        private CommandExecuter commandExecuter = new CommandExecuter();
        
        public DPFunctions dpFunctions;
        public DriveRetriever driveRetriever;

        public void Initialize()
        {
            dpFunctions = new DPFunctions(commandExecuter);
            driveRetriever = new DriveRetriever();
        }


    }
}
