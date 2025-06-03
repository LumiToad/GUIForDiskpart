using GUIForDiskpart.Model.Logic;


namespace GUIForDiskpart.Database.Retrievers
{
    public class DefragAnalysis
    {
        public string[] OptimizeVolumeDataQuery(char driveLetter)
        {
            string[] commands = new string[3];
            commands[0] = $"$Defrag = (Invoke-CimMethod -Query \"SELECT * FROM Win32_Volume WHERE driveletter='{driveLetter}:'\" -MethodName DefragAnalysis)";
            commands[1] = $"$DefragAnalysis = $Defrag.DefragAnalysis";
            commands[2] = $"$DefragAnalysis | Out-String";

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
