using Microsoft.Win32;
using System;

namespace GUIForDiskpart.main
{
    public static class SaveFile
    {
        public static void SaveAsTextfile(string text, string name)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text file (*.txt)|*.txt";

            string currentDateTime = GetFormattedDateTimeString();
            saveFileDialog.FileName = $"guifd_{name}_{currentDateTime}";

            if (saveFileDialog.ShowDialog() == true)
            {
                System.IO.File.WriteAllText(saveFileDialog.FileName, text);
            }
        }

        private static string GetFormattedDateTimeString()
        {
            return DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss");
        }
    }
}
