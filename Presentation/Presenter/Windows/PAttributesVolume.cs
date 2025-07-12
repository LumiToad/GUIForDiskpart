global using PAttributesVolume = 
    GUIForDiskpart.Presentation.Presenter.Windows.PAttributesVolume<GUIForDiskpart.Presentation.View.Windows.WAttributesVolume>;

using System.Windows;

using GUIForDiskpart.Model.Data;
using GUIForDiskpart.Model.Logic.Diskpart;
using GUIForDiskpart.Presentation.View.UserControls;


namespace GUIForDiskpart.Presentation.Presenter.Windows
{
    public class PAttributesVolume<T> : WPresenter<T> where T : WAttributesVolume
    {
        private PLog<UCLog> Log;
        const string MBR_TEXT_EVERY_VOL = "Will effect EVERY Volume on MBR drives!";
        const string MBR_TEXT_THIS_VOL = "Will effect just THIS Volume on GPT drives";

        public WSMPartition WSMPartition { get; private set; }

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

            output += DPFunctions.AttributesVolume(WSMPartition.DriveLetter, false, (string)Window.Attributes.SelectedItem, true);

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
            PopulateAttributesCombobox();
            Log.Print(WSMPartition.GetOutputAsString());
            Window.MBRLabel.Text = (WSMPartition.PartitionTable == "MBR" ? MBR_TEXT_EVERY_VOL : MBR_TEXT_THIS_VOL);
            Window.DriveLetterLabel.Content = WSMPartition.DriveLetter + ":/";
        }

        protected override void AddCustomArgs(params object?[] args)
        {
            WSMPartition = (WSMPartition)args[0];
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
            Log = CreateUCPresenter<PLog<UCLog>>(Window.Log);
        }

        #endregion WPresenter
    }
}

