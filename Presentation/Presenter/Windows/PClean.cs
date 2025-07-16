global using PClean =
   GUIForDiskpart.Presentation.Presenter.Windows.PClean<GUIForDiskpart.Presentation.View.Windows.WClean>;

using System.Windows;

using GUIForDiskpart.Model.Logic.Diskpart;
using GUIForDiskpart.Presentation.Presenter.UserControls;
using GUIForDiskpart.Presentation.View.UserControls;


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
    /// App.Instance.WIM.CreateWPresenter&lt;PClean&gt;(true, Disk);
    /// </code>
    /// </para>
    /// </summary>
    public class PClean<T> : WPresenter<T> where T : WClean
    {
        private PLog<UCLog> Log;

        const string DESCRIPTION_CLEAN = "Clean the whole drive! ALL DATA WILL BE LOST!";
        const string DESCRIPTION_CLEAN_ALL = "Clean and override the whole drive! MAKES DATA RESCUE CLOSE TO IMPOSSIBLE!";

        public DiskModel Disk { get; private set; }

        private void ExecuteClean(bool value)
        {
            if (!value) return;

            string output = string.Empty;

            output += DPFunctions.Clean(Disk.DiskIndex, (bool)Window.CleanAll.IsChecked);

            MainWindow.Log.Print(output);
            MainWindow.DisplayDiskData(false);

            Close();
        }

        #region OnClick

        private void OnConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            string text = (bool)Window.CleanAll.IsChecked ? DESCRIPTION_CLEAN_ALL : DESCRIPTION_CLEAN;
            string confirmKey = Disk.PhysicalName;

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
            Window.ECancel += OnCancelButton_Click;
        }

        public override void InitPresenters()
        {
            Log = CreateUCPresenter<PLog>(Window.Log);
        }

        #endregion WPresenter
    }
}
