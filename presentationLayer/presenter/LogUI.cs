using System;
using System.Windows;
using System.Windows.Controls;

using GUIForDiskpart.Utils;


namespace GUIForDiskpart.Presentation.Presenter
{
    public class LogUI : IPresenter
    {
        private View.UserControls.LogUI Log;

        public LogUI(View.UserControls.LogUI log)
        {
            Log = log;
            RegisterEvents();
        }
        public void Print(string text)
        {
            Log.TextBox.Text += $"[{DateTime.Now}]\n{text}\n\n";
        }

        private void SaveLog(object sender, RoutedEventArgs e)
        {
            FileUtils.SaveAsTextfile(Log.TextBox.Text, "log");
        }

        private void LogUI_TextChanged(object sender, TextChangedEventArgs e)
        {
            Log.TextBox.ScrollToEnd();
        }

        #region IPresenter

        public static IPresenter? New(params object[] args)
        {
            if (args.Length == 1)
            {
                View.UserControls.LogUI logUIView = (View.UserControls.LogUI)args[0];
                if (logUIView != null) return new LogUI(logUIView);
            }

            return null;
        }

        public void RegisterEvents()
        {
            Log.SaveLog_Click += SaveLog;
            Log.LogUI_TextChanged += LogUI_TextChanged;
        }

        #endregion IPresenter
    }
}
