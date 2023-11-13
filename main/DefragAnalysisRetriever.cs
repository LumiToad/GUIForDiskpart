using System;
using System.Collections.Generic;
using System.Reflection;

namespace GUIForDiskpart.main
{
    public static class DefragAnalysisRetriever
    {
        public static DefragAnalysis AnalyzeVolumeDefrag(Partition partition) 
        {
            string[] lines = GetOptimizeVolumeData(partition.WSMPartition.DriveLetter);
            Dictionary<string, string> data = LinesToDictionary(lines);

            DefragAnalysis result = DictionaryToClass(data);
            result.AvailableForExtend = Convert.ToUInt64(partition.AssignedDiskInfo.UnallocatedSpace);
            return result;
        }

        private static string[] GetOptimizeVolumeData(char driveLetter)
        {
            string[] commands = new string[3];
            commands[0] = $"$Defrag = (Invoke-CimMethod -Query \"SELECT * FROM Win32_Volume WHERE driveletter='{driveLetter}:'\" -MethodName DefragAnalysis)";
            commands[1] = $"$DefragAnalysis = $Defrag.DefragAnalysis";
            commands[2] = $"$DefragAnalysis | Out-String";

            string[]? lines = null;

            foreach (var item in CommandExecuter.IssuePowershellCommand(commands)) 
            {
                string fullOutput = item.ToString();
                lines = fullOutput.Split(new char[] {'\r',  '\n'});
            }

            return lines;
        }

        private static Dictionary<string, string> LinesToDictionary(string[]? lines)
        {
            Dictionary<string,string> result = new ();

            foreach (string line in lines)
            {
                if (!line.Contains(':')) continue;
                string property = line.Split(':', StringSplitOptions.RemoveEmptyEntries)[0];
                string value = line.Split(":", StringSplitOptions.RemoveEmptyEntries)[1];

                property = property.Trim();
                value = value.Trim();

                result.Add(property, value);
            }

            foreach (string key in result.Keys)
            {
                Console.WriteLine(key + " " + result[key]);
            }

            return result;
        }

        private static DefragAnalysis DictionaryToClass(Dictionary<string,string> dict)
        {
            DefragAnalysis defragAnalysis = new DefragAnalysis();

            //defragAnalysis.AverageFileSize = Convert.ToUInt64(dict["AverageFileSize"]);

            PropertyInfo[] defragAnalysisProperties = typeof(DefragAnalysis).GetProperties();
            foreach (PropertyInfo property in defragAnalysisProperties)
            {
                if (!dict.ContainsKey(property.Name)) continue;
                var item = Convert.ChangeType(dict[property.Name], property.PropertyType);
                property.SetValue(defragAnalysis, item);
            }

            return defragAnalysis;
        }
    }
}
