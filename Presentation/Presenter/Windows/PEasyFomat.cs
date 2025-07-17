global using PEasyFormat =
    GUIForDiskpart.Presentation.Presenter.Windows.PEasyFomat<GUIForDiskpart.Presentation.View.Windows.WEasyFormat>;

using System;
using System.Windows;
using System.Windows.Controls;

using GUIForDiskpart.Presentation.Presenter.Windows.Components;


namespace GUIForDiskpart.Presentation.Presenter.Windows
{
    /// <summary>
    /// Constructed with:
    /// <value><c>DiskModel</c> Disk</value>
    /// <br/><br/>
    /// Must be instanced with <c>App.Instance.WIM.CreateWPresenter</c> method.<br/>
    /// See code example:
    /// <para>
    /// <code>
    /// App.Instance.WIM.CreateWPresenter&lt;PEasyFomat&gt;(true, DiskModel);
    /// </code>
    /// </para>
    /// </summary>
    public class PEasyFomat<T> : WPresenter<T> where T : WEasyFormat
    {
        private PLog Log;
        private PCEasyFormat pcFormat;

        public DiskModel Disk { get; private set; }

        public UInt64 GetSizeValue()
        {
            UInt64 size = 0;

            if (Window.SizeValue.Text != "")
            {
                UInt64.TryParse(Window.SizeValue.Text, out size);
            }

            return size;
        }

        private void ExecuteFormat(bool value) => pcFormat.ExecuteFormat(value);
        private void EvaluteFAT32SizeBox() => pcFormat.EvaluteFAT32SizeBox();
        public void SetErrorMessage(string message) => pcFormat.SetErrorMessage(message);
        public void ClearErrorMessage() => pcFormat.ClearErrorMessage();
        private void OnSizeValue_TextChanged(object sender, TextChangedEventArgs e) => EvaluteFAT32SizeBox();

        #region OnClick

        private void OnConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            string confirmKey = Disk.PhysicalName;

            var secCheck = App.Instance.WIM.CreateWPresenter<PSecurityCheck>(true, PCFormatBase.SEC_WIN_WARN_DRIVE, confirmKey);
            secCheck.Window.Owner = Window;
            secCheck.ESecCheck += ExecuteFormat;
        }

        private void OnCancelButton_Click(object sender, RoutedEventArgs e) => Close();

        #endregion OnClick

        #region WPresenter

        public override void Setup()
        {
            Log.Print(Disk.GetOutputAsString(), true);
        }

        protected override void AddCustomArgs(params object?[] args)
        {
            Disk = (DiskModel)args[0];
        }

        protected override void RegisterEventsInternal()
        {
            base.RegisterEventsInternal();

            Window.EConfirm += OnConfirmButton_Click;
            Window.ECancel += OnConfirmButton_Click;
            Window.ESelectionChanged += pcFormat.OnComboBox_SelectionChanged;
            Window.ETextChanged += OnSizeValue_TextChanged;
        }

        public override void InitPresenters()
        {
            Log = CreateUCPresenter<PLog>(Window.Log);
        }

        public override void InitComponents()
        {
            pcFormat = new(Window, this as PEasyFormat);
        }

        #endregion WPresenter
    }
}
