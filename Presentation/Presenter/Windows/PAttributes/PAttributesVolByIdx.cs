global using PAttributesVolByIdx =
    GUIForDiskpart.Presentation.Presenter.Windows.PAttributesVolByIdx<GUIForDiskpart.Presentation.View.Windows.WAttributesVolByIdx>;

using System;
using System.Windows;

using GUIForDiskpart.Model.Logic.Diskpart;
using System.Windows.Controls;
using GUIForDiskpart.Utils;


namespace GUIForDiskpart.Presentation.Presenter.Windows
{
    /// <summary>
    /// Constructed with:
    /// NONE - Has no arguments!
    /// <br/><br/>
    /// Must be instanced with <c>App.Instance.WIM.CreateWPresenter</c> method.<br/>
    /// See code example:
    /// <para>
    /// <code>
    /// App.Instance.WIM.CreateWPresenter&lt;PAttributesVolByIndex&gt;(true);
    /// </code>
    /// </para>
    /// </summary>
    public class PAttributesVolByIdx<T> : WPresenter<T> where T : WAttributesVolByIdx
    {
        private PLog Log;

        const string MBR_TEXT = "Will effect EVERY Volume on MBR drives!, will effect just THIS Volume on GPT drives";
        int selectedVolume = -1;
        
        private void PopulateAttributesCombobox()
        {
            Window.Attributes.Items.Add(DPAttributes.HIDDEN);
            Window.Attributes.Items.Add(DPAttributes.READONLY);
            Window.Attributes.Items.Add(DPAttributes.NODEFAULTDRIVELETTER);
            Window.Attributes.Items.Add(DPAttributes.SHADOWCOPY);
        }

        private void OnTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int lastCarretIndex = Window.VolumeNumber.CaretIndex;

            if (!int.TryParse(Window.VolumeNumber.Text, out selectedVolume))
            {
                Window.VolumeNumber.Text = Window.VolumeNumber.Text.RemoveAllButNumbers();
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
            Log = CreateUCPresenter<PLog>(Window.Log);
        }

        #endregion WPresenter
    }
}
