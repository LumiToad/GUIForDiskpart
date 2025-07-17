global using PAttributesVolume = 
    GUIForDiskpart.Presentation.Presenter.Windows.PAttributesVolume<GUIForDiskpart.Presentation.View.Windows.WAttributesVolume>;

using System.Windows;

using GUIForDiskpart.Model.Data;
using GUIForDiskpart.Model.Logic.Diskpart;
using GUIForDiskpart.Presentation.View.UserControls;


namespace GUIForDiskpart.Presentation.Presenter.Windows
{
    /// <summary>
    /// Constructed with:
    /// <value><c>PartitionModel</c> Partition</value>
    /// <br/><br/>
    /// Must be instanced with <c>App.Instance.WIM.CreateWPresenter</c> method.<br/>
    /// See code example:
    /// <para>
    /// <code>
    /// App.Instance.WIM.CreateWPresenter&lt;PAttributesVolume&gt;(true, Partition);
    /// </code>
    /// </para>
    /// </summary>
    public class PAttributesVolume<T> : WPresenter<T> where T : WAttributesVolume
    {
        private PLog Log;
        const string MBR_TEXT_EVERY_VOL = "Will effect EVERY Volume on MBR drives!";
        const string MBR_TEXT_THIS_VOL = "Will effect just THIS Volume on GPT drives";

        public PartitionModel Partition { get; private set; }
        public WSMModel WSMPartition { get; private set; }

        char? driveLetter;

        private void PopulateAttributesCombobox()
        {
            Window.Attributes.Items.Add(DPAttributes.HIDDEN);
            Window.Attributes.Items.Add(DPAttributes.READONLY);
            Window.Attributes.Items.Add(DPAttributes.NODEFAULTDRIVELETTER);
            Window.Attributes.Items.Add(DPAttributes.SHADOWCOPY);
        }

        #region OnClick

        private void OnSetButton_Click(object sender, RoutedEventArgs e)
        {
            string output = string.Empty;

            output += DPFunctions.AttributesVolume(WSMPartition.DriveLetter, true, (string)Window.Attributes.SelectedItem, true);

            MainWindow.Log.Print(output);
            Close();
        }

        private void OnClearButton_Click(object sender, RoutedEventArgs e)
        {
            string output = string.Empty;

            output += DPFunctions.AttributesVolume((char)driveLetter, false, (string)Window.Attributes.SelectedItem, true);

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
            WSMPartition = Partition.WSM;
            driveLetter = Partition.GetDriveLetter();

            PopulateAttributesCombobox();
            Log.Print(WSMPartition.GetOutputAsString(), true);
            Window.MBRLabel.Text = (WSMPartition.PartitionTable == "MBR" ? MBR_TEXT_EVERY_VOL : MBR_TEXT_THIS_VOL);
            Window.DriveLetterLabel.Content = driveLetter + ":/";
        }

        protected override void AddCustomArgs(params object?[] args)
        {
            Partition = (Partition)args[0];
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

