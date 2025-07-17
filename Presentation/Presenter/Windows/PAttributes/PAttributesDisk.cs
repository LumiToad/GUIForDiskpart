global using PAttributesDisk =
    GUIForDiskpart.Presentation.Presenter.Windows.PAttributesDisk<GUIForDiskpart.Presentation.View.Windows.WAttributesDisk>;

using System.Windows;

using GUIForDiskpart.Model.Logic.Diskpart;


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
    /// App.Instance.WIM.CreateWPresenter&lt;PAttributesDisk&gt;(true, Disk);
    /// </code>
    /// </para>
    /// </summary>
    public class PAttributesDisk<T> : WPresenter<T> where T : WAttributesDisk
    {
        private PLog Log;

        public DiskModel Disk { get; private set; }

        #region OnClick

        private void OnSetButton_Click(object sender, RoutedEventArgs e)
        {
            string output = string.Empty;

            output += DPFunctions.AttributesDisk(Disk.DiskIndex, true, false);

            MainWindow.Log.Print(output);
            Close();
        }

        private void OnClearButton_Click(object sender, RoutedEventArgs e)
        {
            string output = string.Empty;

            output += DPFunctions.AttributesDisk(Disk.DiskIndex, false, false);

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
            Log.Print(Disk.GetOutputAsString(), true);
        }

        protected override void AddCustomArgs(params object?[] args)
        {
            Disk = (DiskModel)args[0];
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

