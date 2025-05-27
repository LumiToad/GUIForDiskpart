using System.Windows;
using System.Windows.Controls;

namespace GUIForDiskpart.Utils
{
    public static class WPFUtils
    {
        public static MenuItem CreateContextMenuItem(Image image, string name, string header, bool isEnabled, RoutedEventHandler handler)
        {
            MenuItem menuItem = new MenuItem();
            menuItem.Name = name;
            menuItem.Header = header;
            menuItem.Icon = image;
            menuItem.Click += handler;
            menuItem.IsEnabled = isEnabled;

            return menuItem;
        }

        public static MenuItem CreateContextMenuItem(string name, string header, bool isEnabled, RoutedEventHandler handler)
        {
            return CreateContextMenuItem(null, name, header, isEnabled, handler);
        }
    }
}
