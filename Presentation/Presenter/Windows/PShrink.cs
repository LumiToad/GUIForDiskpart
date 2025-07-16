global using PShrink =
   GUIForDiskpart.Presentation.Presenter.Windows.PShrink<GUIForDiskpart.Presentation.View.Windows.WShrink>;
using GUIForDiskpart.Model.Logic.Diskpart;
using GUIForDiskpart.Presentation.Presenter.UserControls;
using GUIForDiskpart.Presentation.View.UserControls;
using GUIForDiskpart.Utils;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;


namespace GUIForDiskpart.Presentation.Presenter.Windows
{
    /// <summary>
    /// Must be instanced with App.Instance.WIM.CreateWPresenter method.
    /// See code example:
    /// <code>
    /// App.Instance.WIM.CreateWPresenter&lt;PShrink&gt;(true, Partition);
    /// </code>
    /// </summary>
    public class PShrink<T> : WPresenter<T> where T : WShrink
    {
        private PLog<UCLog> Log;

        public PartitionModel Partition { get; private set; }

        private UInt64 availableForShrinkInMB;

        private void SetSliderMinMax(Slider slider, double min, double max)
        {
            if (slider == null) return;
            slider.Minimum = min;
            slider.Maximum = max;
        }

        private void ChangeFormattedLabel(Label label, TextBox textBox)
        {
            label.Content = $"{Convert.ToInt64(textBox.Text)} MB";
        }

        private void OnTextChanged(Slider slider, TextBox textBox, Label label, TextChangedEventArgs e)
        {
            if (slider == null || textBox == null) return;
            if (!double.TryParse(textBox.Text, out double value))
            {
                foreach (var item in e.Changes)
                {
                    int index = item.Offset;
                    if (index < 0 || index >= textBox.Text.Length) continue;
                    textBox.Text = textBox.Text.Remove(index, 1);
                }
                return;
            }

            if (Convert.ToUInt64(textBox.Text) > availableForShrinkInMB)
            {
                textBox.Text = availableForShrinkInMB.ToString();
            }

            slider.Value = value;
            ChangeFormattedLabel(label, textBox);
        }

        private void OnMinimumSizeValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            OnTextChanged(Window.MinimumSlider, Window.MinimumSizeValue, Window.MinFormatted, e);
        }

        private void OnDesiredSizeValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            OnTextChanged(Window.DesiredSlider, Window.DesiredSizeValue, Window.DesiredFormatted, e);
        }

        private void OnMinimumSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Window.MinimumSizeValue.Text = Math.Floor(Window.MinimumSlider.Value).ToString();
            ChangeFormattedLabel(Window.MinFormatted, Window.MinimumSizeValue);
        }

        private void OnDesiredSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Window.DesiredSizeValue.Text = Math.Floor(Window.DesiredSlider.Value).ToString();
            ChangeFormattedLabel(Window.DesiredFormatted, Window.DesiredSizeValue);
        }

        #region OnClick

        private void OnConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            string output = string.Empty;

            char driveLetter = Partition.WSM.DriveLetter;
            uint desiredMB = Convert.ToUInt32(Window.DesiredSizeValue.Text);
            uint minimumMB = Convert.ToUInt32(Window.MinimumSizeValue.Text);

            output += DPFunctions.Shrink(driveLetter, desiredMB, minimumMB, false, false);

            MainWindow.Log.Print(output);
            MainWindow.DisplayDiskData(false);

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
            Partition.DefragAnalysis = DAService.AnalyzeVolumeDefrag(Partition);
            double available = (ByteFormatter.BytesToUnit(Partition.DefragAnalysis.AvailableForShrink, Unit.MB));
            availableForShrinkInMB = (ulong)available;

            string output = string.Empty;
            output += Partition.WSM.GetOutputAsString();
            output += Partition.DefragAnalysis.GetOutputAsString();
            Log.Print(output);
            SetSliderMinMax(Window.MinimumSlider, 0.0d, available);
            SetSliderMinMax(Window.DesiredSlider, 0.0d, available);
            Window.AvailableLabel.Content = $"{availableForShrinkInMB}";
        }

        protected override void AddCustomArgs(params object?[] args)
        {
            Partition = (PartitionModel)args[0];
        }

        protected override void RegisterEventsInternal()
        {
            base.RegisterEventsInternal();

            Window.EConfirm += OnConfirmButton_Click;
            Window.ECancel += OnCancelButton_Click;
            Window.EMinTextChanged += OnMinimumSizeValue_TextChanged;
            Window.EDesiredTextChanged += OnDesiredSizeValue_TextChanged;
            Window.EMinSlider += OnMinimumSlider_ValueChanged;
            Window.EDesiredSlider += OnDesiredSlider_ValueChanged;
        }

        public override void InitPresenters()
        {
            Log = CreateUCPresenter<PLog<UCLog>>(Window.Log);
        }

        #endregion WPresenter
    }
}
