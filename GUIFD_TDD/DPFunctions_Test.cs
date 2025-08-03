using GUIForDiskpart.Database.Data.DP;
using GUIForDiskpart.Model.Logic.Diskpart;
using GUIForDiskpart.Service;


namespace GUIFD_TDD
{
    public class DPFunctions_Test
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void List_Disk()
        {
            //Arrange

            string criteria_commandName = "DISKPART - LIST DISK";
            string criteria_ExitCode = "Exit Code:";

            //Act

            bool result = TestBody_List(criteria_commandName, criteria_ExitCode, Basics.DISK);

            //Assert

            Assert.IsTrue(result);
        }

        [Test]
        public void List_Volume()
        {
            //Arrange

            string criteria_commandName = "DISKPART - LIST VOLUME";
            string criteria_ExitCode = "Exit Code:";

            //Act

            bool result = TestBody_List(criteria_commandName, criteria_ExitCode, Basics.VOLUME);

            //Assert

            Assert.IsTrue(result);
        }

        [Test]
        public void List_VDISK()
        {
            //Arrange

            string criteria_commandName = "DISKPART - LIST VDISK";
            string criteria_ExitCode = "Exit Code:";

            //Act

            bool result = TestBody_List(criteria_commandName, criteria_ExitCode, Basics.VDISK);

            //Assert

            Assert.IsTrue(result);
        }

        [Test]
        public void ListPart_DiskIndex0()
        {
            //Arrange

            Disk.ReLoadDisks();


            string criteria_ExitCode = "Exit Code:";
            string criteria_OutputOutOfRange = "###";

            string fullOutput = string.Empty;
            bool isOutputOutOfRange;
            int cleared = 0;

            //Act

            for (uint i = 0; i < Disk.PhysicalDrives.Count; i++)
            {
                string[] criteria_commandNames = 
                    {
                $"SELECT DISK {i}",
                "DISKPART - LIST PARTITION",
                };

                string currentOutput = DPFunctions.ListPart(i);
                fullOutput += currentOutput;

                bool isCorrectCommandNames =
                    currentOutput.Contains(criteria_commandNames[0]) && currentOutput.Contains(criteria_commandNames[1]);
                bool hasExitCode = currentOutput.Contains(criteria_ExitCode);
                isOutputOutOfRange = !currentOutput.Contains(criteria_OutputOutOfRange);

                //Assert

                if (isOutputOutOfRange)
                {
                    Assert.Fail("Disk Index was out of range!");
                }

                if (isCorrectCommandNames && hasExitCode)
                    cleared++;
                else
                {
                    Console.WriteLine(fullOutput);
                    Assert.Fail(
                        $"Error at Disk {cleared}!\n" +
                        $"The standard output for this disk was expected to contain the following strings:\n" +
                        $"- {criteria_commandNames[0]}\n" +
                        $"- {criteria_commandNames[1]}\n");
                    break;
                }
            }
            
            Console.WriteLine(fullOutput);
            Assert.IsTrue(cleared == Disk.PhysicalDrives.Count);
        }

        private bool TestBody_List(string criteria_commandName, string criteria_ExitCode, string command)
        {
            var output = DPFunctions.List(command);
            bool isCorrectCommandName = output.Contains(criteria_commandName);
            bool hasExitCode = output.Contains(criteria_ExitCode);

            Console.WriteLine(output);
            return (isCorrectCommandName && hasExitCode);
        }
    }
}