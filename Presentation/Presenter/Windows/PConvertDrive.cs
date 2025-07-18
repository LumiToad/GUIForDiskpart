global using PConvertDrive =
   GUIForDiskpart.Presentation.Presenter.Windows.PConvertDrive<GUIForDiskpart.Presentation.View.Windows.WConvertDrive>;

using System.Windows;
using System.Windows.Controls;
using GUIForDiskpart.Database.Data.Types;
using GUIForDiskpart.Model.Logic.Diskpart;
using GUIForDiskpart.Presentation.Presenter.UserControls;
using GUIForDiskpart.Presentation.View.UserControls;
using GUIForDiskpart.Utils;


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
    /// App.Instance.WIM.CreateWPresenter&lt;PConvertDrive&gt;(true, Disk);
    /// </code>
    /// </para>
    /// </summary>
    public class PConvertDrive<T> : WPresenter<T> where T : WConvertDrive
    {
        private PLog<UCLog> Log;

        public DiskModel Disk { get; private set; }

        #region OnClick

        private void OnConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            string option = WPFUtils.ComboBoxSelectionAsString(Window.ConvertOptionValue);

            switch (option)
            {
                case (CommonTypes.GPT):
                    option = Convert.GPT;
                    break;
                case (CommonTypes.MBR):
                    option = Convert.MBR;
                    break;
                case ("BASIC"):
                    option = Convert.BASIC;
                    break;
                case ("DYNAMIC"):
                    option = Convert.DYNAMIC;
                    break;
            }

            string output = string.Empty;
            output = DPFunctions.Convert(Disk.DiskIndex, option);

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

            Window.ECancel += OnCancelButton_Click;
            Window.EConfirm += OnConfirmButton_Click;
        }

        public override void InitPresenters()
        {
            Log = CreateUCPresenter<PLog>(Window.Log);
        }

        #endregion WPresenter
    }
}
