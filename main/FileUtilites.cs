using Microsoft.Win32;
using System;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Windows;
//using System.Windows.Forms;

namespace GUIForDiskpart.main
{
    public static class FileUtilites
    {
        private static string AppDataPath => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        private static string FullGUIFDPath = $"{AppDataPath}\\GUIFD";
        private static string FullCrashPath = $"{FullGUIFDPath}\\crash_logs";
        private static string FullConfigPath = $"{FullGUIFDPath}\\config";
        private static string JsonExtension = ".json";

        public static void SaveAsTextfile(string text, string filename)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text file (*.txt)|*.txt";

            string currentDateTime = GetFormattedDateTimeString();
            saveFileDialog.FileName = $"guifd_{filename}_{currentDateTime}";

            if (saveFileDialog.ShowDialog() == true)
            {
                System.IO.File.WriteAllText(saveFileDialog.FileName, text);
            }
        }

        public static string GetSaveAsTextFilePath(string name)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text file (*.txt)|*.txt";

            string currentDateTime = GetFormattedDateTimeString();
            saveFileDialog.FileName = $"{name}_{currentDateTime}";

            if (saveFileDialog.ShowDialog() == true)
            {
                return saveFileDialog.FileName;
            }
            return "";
        }

        private static string GetFormattedDateTimeString()
        {
            return DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss");
        }

        public static FileStream LoadFromFile(string filePath) 
        {
            FileStream fileStream = File.OpenRead(filePath);
            return fileStream;
        }

        public static string LoadConfigFile(string filename) 
        {
            string output = File.ReadAllText($"{FullConfigPath}\\{filename}");
            Console.WriteLine(output);
            return output;
        }

        public static void SaveConfigFile(string text, string filename)
        {
            //could crash, when being called AND a file already inside!
            Directory.CreateDirectory($"{FullConfigPath}");

            File.WriteAllText($"{FullConfigPath}\\{filename}", text);
            System.Threading.Thread.Sleep(100);
        }

        public static bool CheckFileExists(string filename)
        {
            Console.WriteLine($"{FullConfigPath}\\{filename}");
            return File.Exists($"{FullConfigPath}\\{filename}");
        }

        public static bool CheckFileExists(string filename, string extension)
        {
            return CheckFileExists(filename + extension);
        }

        public static void SaveConfigAsJSON<T>(T data, string filename)
        {
            var options = new JsonSerializerOptions { WriteIndented = true, IncludeFields = true };
            string dataAsJSON = JsonSerializer.Serialize(data, options);
            SaveConfigFile(dataAsJSON, filename + JsonExtension);
        }

        public static T LoadConfigFromJSON<T>(string filename)
        {
            string loadedFile = LoadConfigFile(filename + JsonExtension);
            var options = new JsonSerializerOptions { WriteIndented = true, IncludeFields = true };
            return JsonSerializer.Deserialize<T>(loadedFile, options);
        }

        public static void SaveExceptionCrashLog(Exception ex)
        {
            string? fullCrash = string.Empty;
            fullCrash += "Exception thrown by method: " + ex.TargetSite + "\n";
            fullCrash += ex.Message + "\n";
            fullCrash += ex.Source + "\n";
            fullCrash += ex.StackTrace + "\n";
            fullCrash += ex.InnerException + "\n";
            fullCrash += ex.Data + "\n";
            fullCrash += "Handle result: " + ex.HResult + "\n";

            string currentDateTime = GetFormattedDateTimeString();
            string fileName = $"crashLog_{currentDateTime}.txt";

            if (!Directory.Exists(FullCrashPath)) 
            {
                Directory.CreateDirectory(FullCrashPath);
            }

            File.WriteAllText($"{FullCrashPath}\\{fileName}", fullCrash);
            MessageBoxResult result = MessageBox.Show
                (
                    $"GUIFD has crashed. A crash-log has been saved at {FullCrashPath}\\{fileName}", "ERROR!", MessageBoxButton.OK
                );
        }

        public static Stream GetEmbeddedResourceStream(string fileName)
        {
            string fullFileName = $"GUIForDiskpart.embeddedResources.{fileName}";
            var thisAssembly = Assembly.GetExecutingAssembly();
            var stream = thisAssembly.GetManifestResourceStream(fullFileName);
            return stream;
        }
    }
}
