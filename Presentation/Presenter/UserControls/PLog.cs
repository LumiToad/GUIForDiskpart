global using PLog =
    GUIForDiskpart.Presentation.Presenter.UserControls.PLog<GUIForDiskpart.Presentation.View.UserControls.UCLog>;

using System;
using System.Windows;
using System.Windows.Controls;

using GUIForDiskpart.Utils;


namespace GUIForDiskpart.Presentation.Presenter.UserControls
{
    /// <summary>
    /// Constructed with:
    /// NONE - Has no parameters!
    /// <br/><br/>
    /// Must be instanced with <c>CreateUCPresenter</c> method of a <c>WPresenter</c> derived class.<br/>
    /// If the UserControl is already present at compile time, this class should be instanced in the <c>InitPresenters</c> method. <br/>
    /// See code example:
    /// <para>
    /// <code>
    /// public override void InitPresenters()
    /// {
    ///     someProperty = CreateUCPresenter&lt;PSomething&gt;(Window.SomeUserControl);
    /// }
    /// </code>
    /// </para>
    /// </summary>
    public class PLog<T> : UCPresenter<T> where T : View.UserControls.UCLog
    {
        public void Print(string text, bool scrollTop = false)
        {
            UserControl.TextBox.Text += $"[{DateTime.Now}]\n{text}\n\n";
            if (scrollTop) 
            {
                UserControl.TextBox.ScrollToVerticalOffset(0.0d);
            }
        }

        private void OnSaveLog_Click(object sender, RoutedEventArgs e)
        {
            FileUtils.SaveAsTextfile(UserControl.TextBox.Text, "log");
        }

        private void OnLogUI_TextChanged(object sender, TextChangedEventArgs e)
        {
            UserControl.TextBox.ScrollToEnd();
        }

        #region UCPresenter

        protected override void RegisterEventsInternal()
        {
            UserControl.ESaveLog += OnSaveLog_Click;
            UserControl.ELogUI += OnLogUI_TextChanged;
        }

        #endregion UCPresenter
    }
}
