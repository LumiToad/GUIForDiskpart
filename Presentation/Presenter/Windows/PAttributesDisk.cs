global using PAttributesDisk =
    GUIForDiskpart.Presentation.Presenter.Windows.PAttributesDisk<GUIForDiskpart.Presentation.View.Windows.WAttributesDisk>;

using System.Windows;

using GUIForDiskpart.Model.Logic.Diskpart;
using GUIForDiskpart.Presentation.View.UserControls;


namespace GUIForDiskpart.Presentation.Presenter.Windows
{
    public class PAttributesDisk<T> : WPresenter<T> where T : WAttributesDisk
    {
        PMainWindow MainWindow = App.Instance.WIM.GetPresenter<PMainWindow>();

        private PLog Log;

        public DiskModel DiskModel { get; private set; }

        #region OnClick

        private void OnSetButton_Click(object sender, RoutedEventArgs e)
        {
            string output = string.Empty;

            output += DPFunctions.AttributesDisk(DiskModel.DiskIndex, true, false);

            MainWindow.Log.Print(output);
            Close();
        }

        private void OnClearButton_Click(object sender, RoutedEventArgs e)
        {
            string output = string.Empty;

            output += DPFunctions.AttributesDisk(DiskModel.DiskIndex, false, false);

            MainWindow.Log.Print(output);
            Close();
        }

        private void OnCancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        #endregion OnClick

        #region WPresenter

        public override void Setup()
        {
            Log.Print(DiskModel.GetOutputAsString());
        }

        protected override void AddCustomArgs(params object?[] args)
        {
            DiskModel = (DiskModel)args[0];
        }

        protected override void RegisterEventsInternal()
        {
            base.RegisterEventsInternal();

            Window.ESet += OnSetButton_Click;
            Window.EClear += OnClearButton_Click;
            Window.ECancel += OnCancelButton_Click;
        }

        public override void InitPresenters()
        {
            Log = CreateUCPresenter<PLog>(Window.Log);
        }

        #endregion WPresenter
    }
}

