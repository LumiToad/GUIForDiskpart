global using PEasyFormat =
   GUIForDiskpart.Presentation.Presenter.Windows.PEasyFormat<GUIForDiskpart.Presentation.View.Windows.WEasyFormat>;

using System.Windows;


namespace GUIForDiskpart.Presentation.Presenter.Windows
{
    public class PEasyFormat<T> : WPresenter<T> where T : WEasyFormat
    {
        private PLog Log;

        /// <summary>
        /// TODOOOOOO!!!!!
        /// </summary>

        #region OnClick

        private void OnConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        #endregion OnClick

        #region WPresenter

        public override void Setup()
        {
        }

        protected override void AddCustomArgs(params object?[] args)
        {
        }

        protected override void RegisterEventsInternal()
        {
            base.RegisterEventsInternal();

            //Window.EConfirm += OnConfirmButton_Click;
        }

        public override void InitPresenters()
        {
            Log = CreateUCPresenter<PLog>(Window.Log);
        }

        #endregion WPresenter
    }
}
