global using PDelete =
   GUIForDiskpart.Presentation.Presenter.Windows.PDelete<GUIForDiskpart.Presentation.View.Windows.WDelete>;

using GUIForDiskpart.Model.Data;
using GUIForDiskpart.Model.Logic.Diskpart;
using GUIForDiskpart.Presentation.Presenter.UserControls;
using GUIForDiskpart.Presentation.View.UserControls;

using System.Windows;


namespace GUIForDiskpart.Presentation.Presenter.Windows
{
    public class PDelete<T> : WPresenter<T> where T : WDelete
    {
        private PLog<UCLog> Log;

        public WSMModel WSMPartition { get; private set; }

        const string DELETE_TEXT = "Delete the whole partition! ALL DATA WILL BE LOST!";
        const string CLEAN_ALL_TEXT = "Delete and override the whole partition! MAKES DATA RESCUE CLOSE TO IMPOSSIBLE!";

        private void ExecuteDelete(bool value)
        {
            if (!value) return;

            string output = string.Empty;

            output += DPFunctions.Delete(WSMPartition.DiskNumber, WSMPartition.PartitionNumber, true, (bool)Window.CleanAll.IsChecked);

            MainWindow.Log.Print(output);
            MainWindow.DisplayDiskData(false);

            Close();
        }

        #region OnClick

        private void OnConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            string text = DELETE_TEXT;
            if ((bool)Window.CleanAll.IsChecked)
            {
                text = CLEAN_ALL_TEXT;
            }
            string confirmKey = $"Drive: {WSMPartition.DiskNumber} Partition: {WSMPartition.PartitionNumber}";

            var secCheck = App.Instance.WIM.CreateWPresenter<PSecurityCheck>(true, text, confirmKey);
            secCheck.Window.Owner = Window;
            secCheck.ESecCheck += ExecuteDelete;
        }

        private void OnCancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        #endregion OnClick

        #region WPresenter

        public override void Setup()
        {
            Log.Print(WSMPartition.GetOutputAsString());
        }

        protected override void AddCustomArgs(params object?[] args)
        {
            WSMPartition = (WSMModel)args[0];
        }

        protected override void RegisterEventsInternal()
        {
            base.RegisterEventsInternal();

            Window.EConfirm += OnConfirmButton_Click;
        }

        public override void InitPresenters()
        {
            Log = CreateUCPresenter<PLog>(Window.Log);
        }

        #endregion WPresenter
    }
}
