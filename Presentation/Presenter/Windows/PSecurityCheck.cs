global using PSecurityCheck =
   GUIForDiskpart.Presentation.Presenter.Windows.PSecurityCheck<GUIForDiskpart.Presentation.View.Windows.WSecurityCheck>;

using System.Windows;
using System.Windows.Controls;


namespace GUIForDiskpart.Presentation.Presenter.Windows
{
    /// <summary>
    /// Constructed with:
    /// <value><c>string</c> aboutToText</value>,
    /// <value><c>string</c> confirmText</value>
    /// <br/><br/>
    /// Must be instanced with <c>App.Instance.WIM.CreateWPresenter</c> method.<br/>
    /// See code example:
    /// <para>
    /// <code>
    /// App.Instance.WIM.CreateWPresenter&lt;PSecurityCheck&gt;(true, Partition);
    /// </code>
    /// </para>
    /// </summary>
    public class PSecurityCheck<T> : WPresenter<T> where T : WSecurityCheck
    {
        public delegate void SecCheckResult(bool result);
        public event SecCheckResult ESecCheck;

        private string aboutToText;
        private string confirmText;

        private void OnTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Window.TextBox.Text == (string)Window.ConfirmText.Content)
            {
                Window.Confirm.IsEnabled = true;
            }
            else
            {
                Window.Confirm.IsEnabled = false;
            }
        }

        #region OnClick

        private void OnConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            ESecCheck?.Invoke(true);
            Close();
        }

        private void OnCancelButton_Click(object sender, RoutedEventArgs e)
        {
            ESecCheck?.Invoke(false);
            Close();
        }
        #endregion OnClick

        #region WPresenter

        public override void Setup()
        {
            Window.AboutTo.Content = aboutToText;
            Window.ConfirmText.Content = confirmText;
        }

        protected override void AddCustomArgs(params object?[] args)
        {
            aboutToText = (string)args[0];
            confirmText = (string)args[1];
        }

        protected override void RegisterEventsInternal()
        {
            base.RegisterEventsInternal();

            Window.EConfirm += OnConfirmButton_Click;
            Window.ECancel += OnCancelButton_Click;
            Window.ETextChanged += OnTextBox_TextChanged;
        }

        public override void InitPresenters()
        {
        }

        #endregion WPresenter
    }
}
