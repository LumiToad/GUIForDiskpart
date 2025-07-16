global using PFormatPartition =
   GUIForDiskpart.Presentation.Presenter.Windows.PFormatPartition<GUIForDiskpart.Presentation.View.Windows.WFormatPartition>;
using System;
using System.Windows;
using System.Windows.Controls;

using GUIForDiskpart.Model.Data;
using GUIForDiskpart.Model.Logic.Diskpart;
using GUIForDiskpart.Presentation.Presenter.Windows.Components;


namespace GUIForDiskpart.Presentation.Presenter.Windows
{
    /// <summary>
    /// Constructed with:
    /// <value><c>WSMModel</c> WSM</value>
    /// <br/><br/>
    /// Must be instanced with <c>App.Instance.WIM.CreateWPresenter</c> method.<br/>
    /// See code example:
    /// <para>
    /// <code>
    /// App.Instance.WIM.CreateWPresenter&lt;PFormatPartition&gt;(true, WSM);
    /// </code>
    /// </para>
    /// </summary>
    public class PFormatPartition<T> : WPresenter<T> where T : WFormatPartition
    {
        private PLog Log;
        private readonly PCFormat pcFormat = new();

        public WSMModel WSM { get; private set; }

        private void ExecuteFormat(bool value) => pcFormat.ExecuteFormat(value, Window, this as PFormatPartition);
        public void SetErrorMessage(string message) => pcFormat.SetErrorMessage(message, Window);
        public void ClearErrorMessage() => pcFormat.ClearErrorMessage(Window);

        private void OnComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) =>
            pcFormat.OnComboBox_SelectionChanged(Window, this as PFormatPartition);

        #region OnClick

        private void OnConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            string confirmKey = $"Drive: {WSM.DiskNumber} Partition: {WSM.PartitionNumber}";

            var secCheck = App.Instance.WIM.CreateWPresenter<PSecurityCheck>(true, PCFormat.SEC_WIN_WARN_PART, confirmKey);
            secCheck.Window.Owner = Window;
            secCheck.ESecCheck += ExecuteFormat;
        }

        private void OnCancelButton_Click(object sender, RoutedEventArgs e) => Close();

        #endregion OnClick

        #region WPresenter

        public override void Setup()
        {
            Log.Print(WSM.GetOutputAsString(), true);
        }

        protected override void AddCustomArgs(params object?[] args)
        {
            WSM = (WSMModel)args[0];
        }

        protected override void RegisterEventsInternal()
        {
            base.RegisterEventsInternal();

            Window.EConfirm += OnConfirmButton_Click;
            Window.ECancel += OnCancelButton_Click;
            Window.ESelectionChanged += OnComboBox_SelectionChanged;
        }

        public override void InitPresenters()
        {
            Log = CreateUCPresenter<PLog>(Window.Log);
        }

        #endregion WPresenter
    }
}

