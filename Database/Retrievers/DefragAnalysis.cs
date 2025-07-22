using GUIForDiskpart.Model.Logic;


namespace GUIForDiskpart.Database.Retrievers
{
    public class DefragAnalysis
    {
        private static string PowerShellDefragQuery(char driveLetter) => 
            $"$Defrag = (Invoke-CimMethod -Query \"SELECT * FROM Win32_Volume WHERE driveletter='{driveLetter}:'\" -MethodName DefragAnalysis)";

        private const string DEFRAG_VAR_A = "$DefragAnalysis = $Defrag.DefragAnalysis";
        private const string DEFRAG_VAR_B = "$DefragAnalysis | Out-String";

        public string[] OptimizeVolumeDataQuery(char driveLetter)
        {
            string[] commands = new string[3];
            commands[0] = PowerShellDefragQuery(driveLetter);
            commands[1] = DEFRAG_VAR_A;
            commands[2] = DEFRAG_VAR_B;

            string[]? lines = null;

            foreach (var item in CommandExecuter.IssuePowershellCommand(commands))
            {
                string fullOutput = item.ToString();
                lines = fullOutput.Split(new char[] { '\r', '\n' });
            }

            return lines;
        }
    }
}
