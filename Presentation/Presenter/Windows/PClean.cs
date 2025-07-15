global using PClean =
   GUIForDiskpart.Presentation.Presenter.Windows.PClean<GUIForDiskpart.Presentation.View.Windows.WClean>;

using System.Windows;

using GUIForDiskpart.Model.Logic.Diskpart;
using GUIForDiskpart.Presentation.Presenter.UserControls;
using GUIForDiskpart.Presentation.View.UserControls;


namespace GUIForDiskpart.Presentation.Presenter.Windows
{
    public class PClean<T> : WPresenter<T> where T : WClean
    {
        private PLog<UCLog> Log;

        const string DESCRIPTION_CLEAN = "Clean the whole drive! ALL DATA WILL BE LOST!";
        const string DESCRIPTION_CLEAN_ALL = "Clean and override the whole drive! MAKES DATA RESCUE CLOSE TO IMPOSSIBLE!";

        public DiskModel DiskModel { get; private set; }

        private void ExecuteClean(bool value)
        {
            if (!value) return;

            string output = string.Empty;

            output += DPFunctions.Clean(DiskModel.DiskIndex, (bool)Window.CleanAll.IsChecked);

            MainWindow.Log.Print(output);
            MainWindow.DisplayDiskData(false);

            Close();
        }

        #region OnClick

        private void OnConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            string text = (bool)Window.CleanAll.IsChecked ? DESCRIPTION_CLEAN_ALL : DESCRIPTION_CLEAN;
            string confirmKey = DiskModel.PhysicalName;

            var secCheck = App.Instance.WIM.CreateWPresenter<PSecurityCheck>(true, text, confirmKey);
            secCheck.ESecCheck += ExecuteClean;
        }

        private void OnCancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        #endregion OnClick

        #region WPresenter

        public override void Setup()
        {
            Log.Print(DiskModel.GetOutputAsString(), true);
        }

        protected override void AddCustomArgs(params object?[] args)
        {
            DiskModel = (DiskModel)args[0];
        }

        protected override void RegisterEventsInternal()
        {
            base.RegisterEventsInternal();

            Window.EConfirm += OnConfirmButton_Click;
            Window.ECancel += OnCancelButton_Click;
        }

        public override void InitPresenters()
        {
            Log = CreateUCPresenter<PLog>(Window.Log);
        }

        #endregion WPresenter
    }
}
