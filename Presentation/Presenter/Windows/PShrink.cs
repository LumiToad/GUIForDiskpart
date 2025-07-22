global using PShrink =
   GUIForDiskpart.Presentation.Presenter.Windows.PShrink<GUIForDiskpart.Presentation.View.Windows.WShrink>;

using System;
using System.Windows;
using System.Windows.Controls;

using GUIForDiskpart.Model.Logic.Diskpart;
using GUIForDiskpart.Presentation.Presenter.UserControls;
using GUIForDiskpart.Presentation.View.UserControls;
using GUIForDiskpart.Utils;


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
    /// App.Instance.WIM.CreateWPresenter&lt;PShrink&gt;(true, Partition);
    /// </code>
    /// </para>
    /// </summary>
    public class PShrink<T> : WPresenter<T> where T : WShrink
    {
        private PLog<UCLog> Log;

        public PartitionModel Partition { get; private set; }

        private UInt64 availableInByte;
        private UInt64 desiredInByte;
        private UInt64 minimumInByte;

        private void OnDesiredSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            desiredInByte = System.Convert.ToUInt64(Window.DesiredSlider.Value);
            OnSizeChanged(sender, Window.DesiredSlider, Window.DesiredSizeValue, Window.DesiredFormatted, ref desiredInByte);
        }

        private void OnMinimumSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            minimumInByte = System.Convert.ToUInt64(Window.MinimumSlider.Value);
            OnSizeChanged(sender, Window.MinimumSlider, Window.MinimumSizeValue, Window.MinFormatted, ref minimumInByte);
        }

        private void OnSizeChanged(object sender, Slider slider, TextBox textBox, Label label, ref ulong value)
        {
            if (value > availableInByte)
            {
                value = availableInByte;
                SetTextBox(value, textBox);
            }

            if (sender == slider)
            {
                SetTextBox(value, textBox);
            }

            if (sender == textBox)
            {
                SetSliderValue(value, slider);
            }

            SetFormattedLabel(value, label);
        }

        private void OnTextChanged(object sender, Slider slider, TextBox textBox, Label label, ref ulong bytes)
        {
            textBox.Text = textBox.Text.RemoveAllButNumbers();
            if (!UInt64.TryParse(textBox.Text, out UInt64 value))
            {
                SetTextBox(0, textBox);
                value = 0;
            }

            bytes = ByteFormatter.SizeFromTo<UInt64, UInt64>(value, Unit.MB, Unit.B);
            OnSizeChanged(sender, slider, textBox, label, ref bytes);
        }

        private void OnDesiredSizeValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            Window.DesiredSizeValue.Text = Window.DesiredSizeValue.Text.RemoveAllButNumbers();
            OnTextChanged(sender, Window.DesiredSlider, Window.DesiredSizeValue, Window.DesiredFormatted, ref desiredInByte);
        }

        private void OnMinimumSizeValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            Window.MinimumSizeValue.Text = Window.MinimumSizeValue.Text.RemoveAllButNumbers();
            OnTextChanged(sender, Window.MinimumSlider, Window.MinimumSizeValue, Window.MinFormatted, ref minimumInByte);
        }

        public void SetSliderValue(double value, Slider slider)
        {
            slider.Value = value;
        }

        public void SetFormattedLabel(ulong size, Label label)
        {
            label.Content = ByteFormatter.BytesToAsString(size);
        }

        public void SetAvailableLabel(ulong size)
        {
            Window.AvailableLabel.Content = ByteFormatter.BytesToAsString(size);
        }

        public void SetTextBox(ulong size, TextBox textBox)
        {
            textBox.Text = ByteFormatter.BytesToAsString(size, false, Unit.MB, 0);
        }

        public void SetupMinimumSlider(double min, double max)
        {
            Window.MinimumSlider.Minimum = min;
            Window.MinimumSlider.Maximum = max;
        }

        public void SetupDesiredSlider(double min, double max)
        {
            Window.DesiredSlider.Minimum = min;
            Window.DesiredSlider.Maximum = max;
        }

        #region OnClick

        private void OnConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            string output = string.Empty;

            char driveLetter = Partition.WSM.DriveLetter;
            uint desiredMB = ByteFormatter.BytesTo<UInt64, UInt32>(desiredInByte, Unit.MB);
            uint minimumMB = ByteFormatter.BytesTo<UInt64, UInt32>(minimumInByte, Unit.MB);

            output += DPFunctions.Shrink(driveLetter, desiredMB, minimumMB, false, false);

            MainWindow.Log.Print(output);
            MainWindow.UpdatePanels(false);

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
            availableInByte = Partition.DefragAnalysis.AvailableForShrink;

            string output = string.Empty;
            output += Partition.WSM.GetOutputAsString();
            output += Partition.DefragAnalysis.GetOutputAsString();
            Log.Print(output, true);

            SetupMinimumSlider(0.0d, availableInByte);
            SetupDesiredSlider(0.0d, availableInByte);
            Window.AvailableLabel.Content = $"{ByteFormatter.BytesToAsString(availableInByte)}";
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
