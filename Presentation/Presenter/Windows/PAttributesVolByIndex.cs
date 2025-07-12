using GUIForDiskpart.Model.Logic.Diskpart;
using GUIForDiskpart.Presentation.View.UserControls;
using System.Text.RegularExpressions;
using System;
using System.Windows;

namespace GUIForDiskpart.Presentation.Presenter.Windows
{
    public class PAttributesVolByIndex<T> : WPresenter<T> where T : WAttributesVolByIndex
    {
        private PLog<UCLog> Log;

        const string MBR_TEXT = "Will effect EVERY Volume on MBR drives!, will effect just THIS Volume on GPT drives";
        int selectedVolume = -1;
        

        private void PopulateAttributesCombobox()
        {
            Window.Attributes.Items.Add(DPAttributes.HIDDEN);
            Window.Attributes.Items.Add(DPAttributes.READONLY);
            Window.Attributes.Items.Add(DPAttributes.NODEFAULTDRIVELETTER);
            Window.Attributes.Items.Add(DPAttributes.SHADOWCOPY);
        }

        private void OnTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            int lastCarretIndex = Window.VolumeNumber.CaretIndex;

            if (!int.TryParse(Window.VolumeNumber.Text, out selectedVolume))
            {
                Window.VolumeNumber.Text = Regex.Replace(Window.VolumeNumber.Text, "[^0-9]", "");
                Window.VolumeNumber.CaretIndex = lastCarretIndex;
            }

            if (Window.VolumeNumber.Text.Length > 5)
            {
                Window.VolumeNumber.Text = Window.VolumeNumber.Text.Remove(5);
                Window.VolumeNumber.CaretIndex = lastCarretIndex;
            }

            if (!string.IsNullOrEmpty(Window.VolumeNumber.Text))
            {
                selectedVolume = Convert.ToInt32(Window.VolumeNumber.Text);
            }
            else selectedVolume = -1;
        }

        #region OnClick

        private void OnSetButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedVolume == -1) return;
            string output = string.Empty;

            output += DPFunctions.AttributesVolume(selectedVolume, true, (string)Window.Attributes.SelectedItem, true);

            MainWindow.Log.Print(output);
            Close();
        }

        private void OnClearButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedVolume == -1) return;

            string output = string.Empty;

            output += DPFunctions.AttributesVolume(selectedVolume, false, (string)Window.Attributes.SelectedItem, true);

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
            Window.MBRLabel.Text = MBR_TEXT;
            Log.Print(DPFunctions.List(DPList.VOLUME), true);
        }

        protected override void AddCustomArgs(params object?[] args)
        {
        }

        protected override void RegisterEventsInternal()
        {
            base.RegisterEventsInternal();

            Window.ESet += OnSetButton_Click;
            Window.EClear += OnClearButton_Click;
            Window.ECancel += OnCancelButton_Click;
            Window.ETextChanged += OnTextBox_TextChanged;
        }

        public override void InitPresenters()
        {
            Log = CreateUCPresenter<PLog<UCLog>>(Window.Log);
        }

        #endregion WPresenter
    }
}
