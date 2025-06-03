using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


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

        public static List<UserControl> GetChildrenUserControls(this System.Windows.Window window)
        {
            List<UserControl> retVal = new();

            DependencyObject obj = window;
            if (obj != null)
            {
                GetChildrenControlsInternal(obj, ref retVal);
            }

            return retVal;
        }

        // https://pvq.app/questions/874380/wpf-how-do-i-loop-through-the-all-controls-in-a-window - ask mixtral
        private static void GetChildrenControlsInternal(DependencyObject obj, ref List<UserControl> retVal)
        {
            int childCount = VisualTreeHelper.GetChildrenCount(obj);
            if (childCount == 0) return;

            for (int i = 0; i < childCount; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                var userControl = child as UserControl;

                if (userControl != null && !retVal.Contains(userControl))
                {
                    retVal.Add(userControl);
                }

                GetChildrenControlsInternal(child, ref retVal);
            }
        }
    }
}
