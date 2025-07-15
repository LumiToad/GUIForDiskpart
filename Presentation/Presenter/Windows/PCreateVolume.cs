global using PCreateVolume =
   GUIForDiskpart.Presentation.Presenter.Windows.PCreateVolume<GUIForDiskpart.Presentation.View.Windows.WCreateVolume>;

using System.Windows;
using System.Windows.Controls;

using GUIForDiskpart.Model.Logic.Diskpart;
using GUIForDiskpart.Presentation.Presenter.UserControls;
using GUIForDiskpart.Presentation.View.UserControls;
using GUIForDiskpart.Utils;


namespace GUIForDiskpart.Presentation.Presenter.Windows
{
    public class PCreateVolume<T> : WPresenter<T> where T : WCreateVolume
    {
        private PLog<UCLog> Log;

        public DiskModel DiskModel { get; private set; }

        long size;

        private System.UInt64 GetSizeValue()
        {
            System.UInt64 size = 0;

            if (Window.SizeValue.Text != "")
            {
                System.UInt64.TryParse(Window.SizeValue.Text, out size);
            }

            return size;
        }

        private void OnSizeValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Window.SizeValue.Text.Length == 0) return;

            long enteredSize = System.Convert.ToInt64(Window.SizeValue.Text);
            if (enteredSize > size)
            {
                Window.SizeValue.Text = size.ToString();
            }
        }

        #region OnClick

        private void OnCreateButton_Click(object sender, RoutedEventArgs e)
        {
            string output = string.Empty;

            output += DPFunctions.CreateVolume(DiskModel.DiskIndex, DPVolume.SIMPLE, GetSizeValue(), false);

            MainWindow.Log.Print(output);
            MainWindow.DisplayDiskData(false);

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
            Window.SizeValue.Text = ByteFormatter.BytesToUnitAsString(size, false, Unit.MB, 0);

            Log.Print(DiskModel.GetOutputAsString());
        }

        protected override void AddCustomArgs(params object?[] args)
        {
            DiskModel = (DiskModel)args[0];
            size = (long)args[1];
        }

        protected override void RegisterEventsInternal()
        {
            base.RegisterEventsInternal();

            Window.ECreate += OnCreateButton_Click;
            Window.ECancel += OnCancelButton_Click;

            Window.ETextChanged += OnSizeValue_TextChanged;
        }

        public override void InitPresenters()
        {
            Log = CreateUCPresenter<PLog<UCLog>>(Window.Log);
        }

        #endregion WPresenter
    }
}
