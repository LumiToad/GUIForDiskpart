using GUIForDiskpart.Model.Data;
using GUIForDiskpart.Model.Logic.Diskpart;
using GUIForDiskpart.Presentation.View.UserControls;
using Microsoft.VisualBasic.Logging;
using System.Windows;

namespace GUIForDiskpart.Presentation.Presenter.Windows
{
    public class PAssignLetter<T> : WPresenter<T> where T : WAssignLetter
    {
        private PLog<UCLog> Log;

        WSMPartition wsmPartition;
        WSMPartition WSMPartition
        { get { return wsmPartition; } set { wsmPartition = value; } }

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

            output += DPFunctions.Assign(WSMPartition.DiskNumber, WSMPartition.PartitionNumber, letter, true);

            MainWindow.Log.Print(output);
            MainWindow.DisplayDiskData(false);
        }

        private void ExecuteRemove()
        {
            string output = string.Empty;
            char letter = wsmPartition.DriveLetter;

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

            Log.Print(wsmPartition.GetOutputAsString(), true);
            if (wsmPartition != null && wsmPartition.DriveLetter == '\0')
            {
                Window.RemoveButton.IsEnabled = false;
                Window.RemoveButton.Foreground = System.Windows.Media.Brushes.DarkGray;
            }
        }

        protected override void AddCustomArgs(params object?[] args)
        {
            WSMPartition = (WSMPartition)args[0];
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
            Log = CreateUCPresenter<PLog<UCLog>>(Window.Log);
        }

        #endregion WPresenter
    }
}
