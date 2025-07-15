global using PExtend =
   GUIForDiskpart.Presentation.Presenter.Windows.PExtend<GUIForDiskpart.Presentation.View.Windows.WExtend>;

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

using GUIForDiskpart.Model.Logic.Diskpart;
using GUIForDiskpart.Presentation.Presenter.UserControls;
using GUIForDiskpart.Presentation.View.UserControls;
using GUIForDiskpart.Utils;


namespace GUIForDiskpart.Presentation.Presenter.Windows
{
    public class PExtend<T> : WPresenter<T> where T : WExtend
    {
        private PLog<UCLog> Log;

        public PartitionModel Partition { get; private set; }

        private UInt64 availableForExtendInMB;

        private void OnDesiredSizeValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            OnDesiredSizeChanged(Window.DesiredSlider, Window.DesiredSizeValue, Window.DesiredFormatted, e);
        }

        private void OnDesiredSizeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            CultureInfo cultureUS = new CultureInfo("en-US");
            Window.DesiredSizeValue.Text = Window.DesiredSlider.Value.ToString("N0").Replace(".", "");
            ChangeFormattedLabel(Window.DesiredFormatted, Window.DesiredSizeValue);
        }

        private void OnDesiredSizeChanged(Slider slider, TextBox textBox, Label label, TextChangedEventArgs e)
        {
            if (slider == null || textBox == null) return;
            if (!double.TryParse(textBox.Text, out double value))
            {
                foreach (var item in e.Changes)
                {
                    int index = item.Offset;
                    if (index < 0 || index > textBox.Text.Length) continue;
                    textBox.Text = textBox.Text.Remove(index, 1);
                }
                return;
            }

            if (Convert.ToUInt64(textBox.Text) > availableForExtendInMB)
            {
                textBox.Text = availableForExtendInMB.ToString();
            }

            slider.Value = value;
            ChangeFormattedLabel(label, textBox);
        }

        private void ChangeFormattedLabel(Label label, TextBox textBox)
        {
            Int64 toMB = Convert.ToInt64(textBox.Text) * 1024;
            toMB *= 1024;
            label.Content = ByteFormatter.BytesToUnitAsString(toMB);
        }

        private void SetSliderMinMax(Slider slider, double min, double max)
        {
            if (slider == null) return;
            slider.Minimum = min;
            slider.Maximum = max;
        }

        #region OnClick

        private void OnConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            string output = string.Empty;

            uint desiredMB = Convert.ToUInt32(Window.DesiredSizeValue.Text);
            uint diskIndex = Partition.WSM.DiskNumber;

            if (Partition.HasDriveLetter())
            {
                char driveLetter = Partition.GetDriveLetter();
                output += DPFunctions.Extend(diskIndex, driveLetter, desiredMB, false);
            }
            else
            {
                uint partitionIndex = Partition.WSM.PartitionNumber;
                output += DPFunctions.Extend(diskIndex, partitionIndex, desiredMB, false);
            }

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
            string output = string.Empty;

            output += Partition.WSM.GetOutputAsString();
            output += Partition.DefragAnalysis.GetOutputAsString();
            Log.Print(output);

            availableForExtendInMB = (Partition.DefragAnalysis.AvailableForExtend / 1024) / 1024;
            SetSliderMinMax(Window.DesiredSlider, 0.0d, Convert.ToDouble(availableForExtendInMB));
            Window.AvailableLabel.Content = ByteFormatter.BytesToUnitAsString(Partition.DefragAnalysis.AvailableForExtend);
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
            Window.ETextChanged += OnDesiredSizeValue_TextChanged;
            Window.ESliderValueChanged += OnDesiredSizeSlider_ValueChanged;
        }

        public override void InitPresenters()
        {
            Log = CreateUCPresenter<PLog>(Window.Log);
        }

        #endregion WPresenter
    }
}
