using System.Globalization;
using System;
using System.Windows;
using System.Windows.Controls;
using GUIForDiskpart.Model.Data;
using GUIForDiskpart.Model.Logic.Diskpart;
using GUIForDiskpart.Service;

namespace GUIForDiskpart.Presentation.View.Windows
{
    /// <summary>
    /// Interaction logic for ExtendWindow.xaml
    /// </summary>
    public partial class ExtendWindow : Window
    {
        MainWindow MainWindow => (MainWindow)Application.Current.MainWindow;

        private PartitionModel partition;
        public PartitionModel Partition
        {
            get { return partition; }
            set
            {
                partition = value;
                Partition.DefragAnalysis = DAService.AnalyzeVolumeDefrag(Partition);
                availableForExtendInMB = (Partition.DefragAnalysis.AvailableForExtend / 1024) / 1024;
                AddTextToConsole();
                SetSliderMinMax(DesiredSlider, 0.0d, Convert.ToDouble(availableForExtendInMB));
                AvailableLabel.Content = ByteFormatter.FormatBytes(Partition.DefragAnalysis.AvailableForExtend);
            }
        }

        private UInt64 availableForExtendInMB;

        public ExtendWindow(PartitionModel partition)
        {
            InitializeComponent();
            Partition = partition;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            string output = string.Empty;

            uint desiredMB = Convert.ToUInt32(DesiredSizeValue.Text);
            uint diskIndex = Partition.WSMPartition.DiskNumber;

            if (Partition.WSMPartition.DriveLetter < 65)
            {
                uint partitionIndex = Partition.WSMPartition.PartitionNumber;
                output += DPFunctions.Extend(diskIndex, partitionIndex, desiredMB, false);
            }
            else
            {
                char driveLetter = Partition.WSMPartition.DriveLetter;
                output += DPFunctions.Extend(diskIndex, driveLetter, desiredMB, false);
            }

            MainWindow.AddTextToOutputConsole(output);
            MainWindow.RetrieveAndShowDiskData(false);

            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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

            if (Convert.ToUInt64(textBox.Text) > availableForExtendInMB)
            {
                textBox.Text = availableForExtendInMB.ToString();
            }

            slider.Value = value;
            ChangeFormattedLabel(label, textBox);
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
