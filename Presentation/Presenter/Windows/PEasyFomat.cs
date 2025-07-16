global using PEasyFormat =
    GUIForDiskpart.Presentation.Presenter.Windows.PEasyFomat<GUIForDiskpart.Presentation.View.Windows.WEasyFormat>;

using System;
using System.Windows;
using System.Windows.Controls;

using GUIForDiskpart.Model.Logic;
using GUIForDiskpart.Presentation.Presenter.Windows.Components;


namespace GUIForDiskpart.Presentation.Presenter.Windows
{
    public class PEasyFomat<T> : WPresenter<T> where T : WEasyFormat
    {
        private PLog Log;
        private readonly PCFormat pcFormat = new();

        public DiskModel DiskModel { get; private set; }

        public UInt64 GetSizeValue()
        {
            UInt64 size = 0;

            if (Window.SizeValue.Text != "")
            {
                UInt64.TryParse(Window.SizeValue.Text, out size);
            }

            return size;
        }

        private void ExecuteFormat(bool value) => pcFormat.ExecuteFormat(value, Window, this as PEasyFormat);
        private void EvaluteFAT32SizeBox() => pcFormat.EvaluteFAT32SizeBox(Window, this as PEasyFormat);
        public void SetErrorMessage(string message) => pcFormat.SetErrorMessage(message, Window);
        public void ClearErrorMessage() => pcFormat.ClearErrorMessage(Window);
        private void OnSizeValue_TextChanged(object sender, TextChangedEventArgs e) => EvaluteFAT32SizeBox();
        private void OnComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) => 
            pcFormat.OnComboBox_SelectionChanged(Window, this as PEasyFormat);

        #region OnClick

        private void OnConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            string confirmKey = DiskModel.PhysicalName;

            var secCheck = App.Instance.WIM.CreateWPresenter<PSecurityCheck>(true, PCFormat.SEC_WIN_WARN_DRIVE, confirmKey);
            secCheck.Window.Owner = Window;
            secCheck.ESecCheck += ExecuteFormat;
        }

        private void OnCancelButton_Click(object sender, RoutedEventArgs e) => Close();

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
            Window.ECancel += OnConfirmButton_Click;
            Window.ESelectionChanged += OnComboBox_SelectionChanged;
            Window.ETextChanged += OnSizeValue_TextChanged;
        }

        public override void InitPresenters()
        {
            Log = CreateUCPresenter<PLog>(Window.Log);
        }

        #endregion WPresenter
    }
}
