using GUIForDiskpart.Model.Data;
using GUIForDiskpart.Model.Logic.Diskpart;
using GUIForDiskpart.service;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace GUIForDiskpart.Presentation.View.Windows
{
    /// <summary>
    /// Interaction logic for ShrinkWindow.xaml
    /// </summary>
    public partial class ShrinkWindow : Window
    {
        MainWindow MainWindow => (MainWindow)Application.Current.MainWindow;

        private Model.Data.Partition partition;
        public Model.Data.Partition Partition
        {
            get { return partition; }
            set
            {
                partition = value;
                Partition.DefragAnalysis = DefragAnalysisRetriever.AnalyzeVolumeDefrag(Partition);
                availableForShrinkInMB = (Partition.DefragAnalysis.AvailableForShrink / 1024) / 1024;
                AddTextToConsole();
                SetSliderMinMax(MinimumSlider, 0.0d, Convert.ToDouble(availableForShrinkInMB));
                SetSliderMinMax(DesiredSlider, 0.0d, Convert.ToDouble(availableForShrinkInMB));
                AvailableLabel.Content = ByteFormatter.FormatBytes(Partition.DefragAnalysis.AvailableForShrink);
            }
        }

        private UInt64 availableForShrinkInMB;

        public ShrinkWindow(Model.Data.Partition partition)
        {
            InitializeComponent();
            Partition = partition;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            string output = string.Empty;

            char driveLetter = Partition.WSMPartition.DriveLetter;
            uint desiredMB = Convert.ToUInt32(DesiredSizeValue.Text);
            uint minimumMB = Convert.ToUInt32(MinimumSizeValue.Text);

            output += DPFunctions.Shrink(driveLetter, desiredMB, minimumMB, false, false);

            MainWindow.AddTextToOutputConsole(output);
            MainWindow.RetrieveAndShowDiskData(false);

            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MinimumSizeValue_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            OnTextChanged(MinimumSlider, MinimumSizeValue, MinFormatted, e);
        }

        private void DesiredSizeValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            OnTextChanged(DesiredSlider, DesiredSizeValue, DesiredFormatted, e);
        }

        private void OnTextChanged(Slider slider, TextBox textBox, Label label, TextChangedEventArgs e)
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

            if (Convert.ToUInt64(textBox.Text) > availableForShrinkInMB)
            {
                textBox.Text = availableForShrinkInMB.ToString();
            }

            slider.Value = value;
            ChangeFormattedLabel(label, textBox);
        }

        private void MinimumSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            CultureInfo cultureUS = new CultureInfo("en-US");
            MinimumSizeValue.Text = MinimumSlider.Value.ToString("N0").Replace(".","");
            ChangeFormattedLabel(MinFormatted, MinimumSizeValue);
        }

        private void DesiredSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            CultureInfo cultureUS = new CultureInfo("en-US");
            DesiredSizeValue.Text = DesiredSlider.Value.ToString("N0").Replace(".", "");
            ChangeFormattedLabel(DesiredFormatted, DesiredSizeValue);
        }

        private void ChangeFormattedLabel(Label label, TextBox textBox)
        {
            Int64 toMB = Convert.ToInt64(textBox.Text) * 1024;
            toMB *= 1024;
            label.Content = ByteFormatter.FormatBytes(toMB);
        }

        public void AddTextToConsole()
        {
            string output = string.Empty;

            output += Partition.WSMPartition.GetOutputAsString();
            output += Partition.DefragAnalysis.GetOutputAsString();
            ConsoleReturn.AddTextToOutputConsole(output);
        }

        private void SetSliderMinMax(Slider slider, double min, double max)
        {
            if (slider == null) return;
            slider.Minimum = min; 
            slider.Maximum = max;
        }
    }
}
