global using PAssignLetter =
    GUIForDiskpart.Presentation.Presenter.Windows.PAssignLetter<GUIForDiskpart.Presentation.View.Windows.WAssignLetter>;

using System.Windows;

using GUIForDiskpart.Model.Data;
using GUIForDiskpart.Model.Logic.Diskpart;


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
    /// App.Instance.WIM.CreateWPresenter&lt;PAssignLetter&gt;(true, WSM);
    /// </code>
    /// </para>
    /// </summary>
    public class PAssignLetter<T> : WPresenter<T> where T : WAssignLetter
    {
        private PLog Log;

        public WSMModel WSM { get; private set; }

        private void PopulateDriveLetterBox()
        {
            Window.DriveLetterBox.Items.Clear();
            Window.DriveLetterBox.Items.Add("Auto Select");

            foreach (char availableLetter in DriveLetterService.GetAvailableDriveLetters())
            {
                Window.DriveLetterBox.Items.Add(availableLetter);
            }
            Window.DriveLetterBox.SelectedItem = Window.DriveLetterBox.Items[0];
        }

        private void ExecuteAssign()
        {
            string output = string.Empty;

            char? letter = null;
            if (Window.DriveLetterBox.SelectedValue != "Auto Select")
            {
                letter = (char?)Window.DriveLetterBox.SelectedValue;
            }

            output += DPFunctions.Assign(WSM.DiskNumber, WSM.PartitionNumber, letter, true);

            MainWindow.Log.Print(output);
            MainWindow.DisplayDiskData(false);
        }

        private void ExecuteRemove()
        {
            string output = string.Empty;
            char letter = WSM.DriveLetter;

            output += DPFunctions.Remove(letter, false, true);

            MainWindow.Log.Print(output);
            MainWindow.DisplayDiskData(false);
        }

        #region OnClick

        private void OnConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            ExecuteAssign();
            Close();
        }

        private void OnRemoveButton_Click(object sender, RoutedEventArgs e)
        {
            ExecuteRemove();
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
            PopulateDriveLetterBox();
            DiskService.OnDiskChanged += PopulateDriveLetterBox;

            Log.Print(WSM.GetOutputAsString(), true);
            if (WSM != null && WSM.DriveLetter == '\0')
            {
                Window.RemoveButton.IsEnabled = false;
                Window.RemoveButton.Foreground = System.Windows.Media.Brushes.DarkGray;
            }
        }

        protected override void AddCustomArgs(params object?[] args)
        {
            WSM = (WSMModel)args[0];
        }

        protected override void RegisterEventsInternal()
        {
            base.RegisterEventsInternal();

            Window.EConfirm += OnConfirmButton_Click;
            Window.ERemove += OnRemoveButton_Click;
            Window.ECancel += OnCancelButton_Click;
        }

        public override void InitPresenters()
        {
            Log = CreateUCPresenter<PLog>(Window.Log);
        }

        #endregion WPresenter
    }
}
