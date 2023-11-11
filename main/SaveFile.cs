﻿using Microsoft.Win32;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Media;

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

        public static Icon LoadIconFromFile(string fileName) 
        {
            string filename = fileName;
            FileStream fs = new FileStream(filename, FileMode.Open);
            Icon icon = new Icon(fs);
            fs.Close();
            return icon;
        }
    }
}
