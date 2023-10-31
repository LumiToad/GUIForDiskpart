using GUIForDiskpart.diskpart;
using System;

namespace GUIForDiskpart.main
{
    public class MainProgram
    {

        public ComfortFeatures comfortFunctions;
        public DriveRetriever driveRetriever;

        public MainProgram() 
        {
            Initialize();
        }

        public void Initialize()
        {

            driveRetriever = new DriveRetriever();
            driveRetriever.Initialize();
            comfortFunctions = new ComfortFeatures();

        }
    }
}
