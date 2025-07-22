using System;
using System.Collections.Generic;
using System.Reflection;

using DARetriever = GUIForDiskpart.Database.Retrievers.DefragAnalysis;

namespace GUIForDiskpart.Service
{
    public static class DefragAnalysis
    {
        private static DARetriever daRetriever = new();

        public static DAModel AnalyzeVolumeDefrag(PartitionModel partition)
        {
            string[] lines = daRetriever.OptimizeVolumeDataQuery(partition.WSM.DriveLetter);
            Dictionary<string, string> data = LinesToDictionary(lines);

            Model.Data.DefragAnalysis result = DictionaryToClass(data);
            result.AvailableForExtend = System.Convert.ToUInt64(partition.AssignedDiskModel.UnallocatedSpace);
            return result;
        }

        private static Dictionary<string, string> LinesToDictionary(string[]? lines)
        {
            Dictionary<string, string> result = new();

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

        private static DAModel DictionaryToClass(Dictionary<string, string> dict)
        {
            DAModel defragAnalysis = new();

            PropertyInfo[] defragAnalysisProperties = typeof(DAModel).GetProperties();
            foreach (PropertyInfo property in defragAnalysisProperties)
            {
                if (!dict.ContainsKey(property.Name)) continue;
                var item = System.Convert.ChangeType(dict[property.Name], property.PropertyType);
                property.SetValue(defragAnalysis, item);
            }

            return defragAnalysis;
        }
    }
}
