using GUIForDiskpart.diskpart;
using System;

namespace GUIForDiskpart.main
{
    public class MainProgram
    {
        public DPFunctions dpFunctions;
        public ComfortFeatures comfortFunctions;
        public DriveRetriever driveRetriever;

        public MainProgram() 
        {
            Initialize();
        }

        public void Initialize()
        {
            dpFunctions = new DPFunctions();
            driveRetriever = new DriveRetriever();
            driveRetriever.Initialize();
            comfortFunctions = new ComfortFeatures();
            comfortFunctions.dpFunctions = dpFunctions;
        }
    }
}
