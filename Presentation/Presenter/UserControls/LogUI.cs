using System;
using System.Windows;
using System.Windows.Controls;

using GUIForDiskpart.Utils;


namespace GUIForDiskpart.Presentation.Presenter
{
    public class LogUI<T> : UCPresenter<T> where T : View.UserControls.LogUI
    {
        public LogUI(T userControl) : base(userControl) { }

        public void Print(string text)
        {
            UserControl.TextBox.Text += $"[{DateTime.Now}]\n{text}\n\n";
        }

        private void OnSaveLog_Click(object sender, RoutedEventArgs e)
        {
            FileUtils.SaveAsTextfile(UserControl.TextBox.Text, "log");
        }

        private void OnLogUI_TextChanged(object sender, TextChangedEventArgs e)
        {
            UserControl.TextBox.ScrollToEnd();
        }

        #region IPresenter

        protected override void RegisterEventsInternal()
        {
            UserControl.ESaveLog += OnSaveLog_Click;
            UserControl.ELogUI += OnLogUI_TextChanged;
        }

        #endregion IPresenter
    }
}
