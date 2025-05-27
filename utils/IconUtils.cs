using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace GUIForDiskpart.Utils
{
    public enum Shell32IconType
    {
        None = -1,
        Fixed = 12,
        System = 15,
        Drive = 79,
        NetworkDrive = 9,
        USB = 26,
        UpArrow = 146,
        QuestionMark = 154,
        Blocked = 219,
        Warning = 235,
        Save = 258,
        Checkmark = 301,
    }

    public enum SystemIconType
    {
        Application = 0,
        Asterisk = 1,
        Error = 2,
        Exclamation = 3,
        Hand = 4,
        Information = 5,
        Question = 6,
        Shield = 7,
        Warning = 8,
        WinLogo = 9
    }

    public static class IconUtils
    {
        public static System.Windows.Controls.Image Diskpart => GetImageFromFile("..\\..\\..\\resources\\diskpart.png");
        public static System.Windows.Controls.Image CMD => GetImageFromFile("..\\..\\..\\resources\\cmd.png");
        public static System.Windows.Controls.Image GUIFD => GetImageFromFile("..\\..\\..\\resources\\guifd.png");
        public static System.Windows.Controls.Image Commandline => GetImageFromFile("..\\..\\..\\resources\\commandline.png");

        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        private static extern int ExtractIconEx(string lpszFile, int nIconIndex, out IntPtr phiconLarge, IntPtr phiconSmall, int nIcons);

        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        private static extern int ExtractIconEx(string lpszFile, int nIconIndex, IntPtr phiconLarge, out IntPtr phiconSmall, int nIcons);

        public static ImageSource GetShellIconByIndex(int index, bool largeIcon)
        {
            Icon? icon = Extract("shell32.dll", index, largeIcon);
            return icon.ToImageSource();
        }

        public static ImageSource GetShellIconByType(Shell32IconType type, bool largeIcon)
        {
            Icon? icon = Extract("shell32.dll", (int)type, largeIcon);
            return icon.ToImageSource();
        }

        public static ImageSource GetSystemIconByType(SystemIconType type)
        {
            Icon icon = SystemIcons.Warning;

            switch (type)
            {
                case SystemIconType.Application:
                    icon = SystemIcons.Application;
                    break;
                case SystemIconType.Asterisk:
                    icon = SystemIcons.Asterisk;
                    break;
                case SystemIconType.Error:
                    icon = SystemIcons.Error;
                    break;
                case SystemIconType.Exclamation:
                    icon = SystemIcons.Exclamation;
                    break;
                case SystemIconType.Hand:
                    icon = SystemIcons.Hand;
                    break;
                case SystemIconType.Information:
                    icon = SystemIcons.Information;
                    break;
                case SystemIconType.Question:
                    icon = SystemIcons.Question;
                    break;
                case SystemIconType.Shield:
                    icon = SystemIcons.Shield;
                    break;
                case SystemIconType.Warning:
                    icon = SystemIcons.Warning;
                    break;
                case SystemIconType.WinLogo:
                    var stream = FileUtils.GetEmbeddedResourceStream("winLogo.ico");
                    icon = new Icon(stream);
                    stream.Close();
                    break;
            }

            return icon.ToImageSource();
        }

        public static Icon LoadIconFromFile(string filePath)
        {
            FileStream fileStream = FileUtils.LoadFromFile(filePath);
            Icon icon = new Icon(fileStream);
            fileStream.Close();
            return icon;
        }

        private static Icon? Extract(string filePath, int index, bool largeIcon = true)
        {
            if (filePath == null)
                throw new ArgumentNullException(nameof(filePath));

            IntPtr hIcon;
            if (largeIcon)
            {
                ExtractIconEx(filePath, index, out hIcon, IntPtr.Zero, 1);
            }
            else
            {
                ExtractIconEx(filePath, index, IntPtr.Zero, out hIcon, 1);
            }

            return hIcon != IntPtr.Zero ? Icon.FromHandle(hIcon) : null;
        }

        public static ImageSource ToImageSource(this Icon icon)
        {
            ImageSource imageSource = Imaging.CreateBitmapSourceFromHIcon(
                icon.Handle,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            return imageSource;
        }

        public static System.Windows.Controls.Image GetImageFromFile(string filePath)
        {
            System.Windows.Controls.Image image = new System.Windows.Controls.Image();
            image.Source = new BitmapImage(new Uri(filePath, UriKind.Relative));

            return image;
        }
    }
}
