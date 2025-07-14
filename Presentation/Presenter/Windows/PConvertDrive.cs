global using PConvertDrive =
   GUIForDiskpart.Presentation.Presenter.Windows.PConvertDrive<GUIForDiskpart.Presentation.View.Windows.WConvertDrive>;

using System.Windows;
using System.Windows.Controls;

using GUIForDiskpart.Model.Logic.Diskpart;
using GUIForDiskpart.Presentation.Presenter.UserControls;
using GUIForDiskpart.Presentation.View.UserControls;
using GUIForDiskpart.Utils;


namespace GUIForDiskpart.Presentation.Presenter.Windows
{
    public class PConvertDrive<T> : WPresenter<T> where T : WConvertDrive
    {
        private PLog<UCLog> Log;

        public DiskModel DiskModel { get; private set; }

        #region OnClick

        private void OnConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            string option = DPConvert.GPT;

            switch (WPFUtils.ComboBoxSelectionAsString(Window.ConvertOptionValue))
            {
                case ("GPT"):
                    option = DPConvert.GPT;
                    break;
                case ("MBR"):
                    option = DPConvert.MBR;
                    break;
                case ("BASIC"):
                    option = DPConvert.BASIC;
                    break;
                case ("DYNAMIC"):
                    option = DPConvert.DYNAMIC;
                    break;
            }

            string output = string.Empty;
            output = DPFunctions.Convert(DiskModel.DiskIndex, option);

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

            Window.ECancel += OnCancelButton_Click;
            Window.EConfirm += OnConfirmButton_Click;
        }

        public override void InitPresenters()
        {
            Log = CreateUCPresenter<PLog<UCLog>>(Window.Log);
        }

        #endregion WPresenter
    }
}
