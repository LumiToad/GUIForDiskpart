using NUnit.Framework.Internal;

using DiskModel = GUIForDiskpart.Model.Data.Disk;
using GUIForDiskpart.Database.Data.DP;
using GUIForDiskpart.Service;


namespace GUIFD_TDD
{
    [TestFixture]
    public class DPFunctions
    {
        static List<DiskModel> pDisks;

        [TestCase(Basics.DISK, ExpectedResult = true)]
        [TestCase(Basics.VOLUME, ExpectedResult = true)]
        [TestCase(Basics.VDISK, ExpectedResult = true)]
        [TestCase("", ExpectedResult = true)]
        public bool List(string command)
        {
            string expected_commandName = $"DISKPART - LIST {command}";
            const string expected_ExitCode = $"Exit Code: 0";

            var output = GUIForDiskpart.Model.Logic.Diskpart.DPFunctions.List(command);
            bool result = 
                output.Contains(command) &&
                output.Contains(expected_ExitCode)
                ;

            Console.WriteLine(output, expected_ExitCode);
            return result;
        }

        [TestCaseSource(nameof(GetPDiskList))]
        public void ListPart(DiskModel disk)
        {
            //Arrange

            string expected_ExitCode = "Exit Code: 0";
            string expected_PartitionLabelString = "###";

            string[] expected_CommandNames =
        {
                $"SELECT DISK {disk.DiskIndex}",
                $"DISKPART - LIST {Basics.PARTITION}",
                };

            string error_UnexpectedOutput =
                $"Error at Disk {disk.DiskIndex}!\n" +
                $"The standard output for this disk was expected to contain the following strings:\n";

            string fullOutput = string.Empty;

            //Act

            string currentOutput = GUIForDiskpart.Model.Logic.Diskpart.DPFunctions.ListPart(disk.DiskIndex);
            fullOutput += currentOutput;

            bool isCorrectCommandNames =
                currentOutput.Contains(expected_CommandNames[0]) &&
                currentOutput.Contains(expected_CommandNames[1])
                ;
            bool isCorrectExitCode = currentOutput.Contains(expected_ExitCode);
            bool isCorrectOutput = currentOutput.Contains(expected_PartitionLabelString);

            Console.WriteLine(fullOutput);

            //Assert

            if (isCorrectCommandNames && isCorrectExitCode && isCorrectOutput) 
                Assert.Pass();
           
            Assert.Fail(
                error_UnexpectedOutput +
                $"- {expected_CommandNames[0]}\n" +
                $"- {expected_CommandNames[1]}\n" +
                $"- {expected_PartitionLabelString}"
                );
        }

        public static List<DiskModel> GetPDiskList()
        {
            if (pDisks == null)
            {
                Disk.ReLoadDisks();
                pDisks = Disk.PhysicalDrives;
            }

            return pDisks;
        }
    }
}