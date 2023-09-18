using GUIForDiskpart.diskpart;
using System;

namespace GUIForDiskpart.main
{
    public class MainProgram
    {
        public DPFunctions dpFunctions;
        public DriveRetriever driveRetriever;

        public MainProgram() 
        {
            Initialize();
        }

        public void Initialize()
        {
            dpFunctions = new DPFunctions();
            driveRetriever = new DriveRetriever();
        }
    }
}
