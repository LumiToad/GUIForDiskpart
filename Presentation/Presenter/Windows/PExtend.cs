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
    /// <summary>
    /// Constructed with:
    /// <value><c>PartitionModel</c> Partition</value>
    /// <br/><br/>
    /// Must be instanced with <c>App.Instance.WIM.CreateWPresenter</c> method.<br/>
    /// See code example:
    /// <para>
    /// <code>
    /// App.Instance.WIM.CreateWPresenter&lt;PExtend&gt;(true, Partition);
    /// </code>
    /// </para>
    /// </summary>
    public class PExtend<T> : WPresenter<T> where T : WExtend
    {
        private PLog<UCLog> Log;

        public PartitionModel Partition { get; private set; }

        private UInt64 availableInByte;
        private UInt64 desiredInByte;

        private void OnDesiredSizeValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            Window.DesiredSizeValue.Text = Window.DesiredSizeValue.Text.RemoveAllButNumbers();
            if (!UInt64.TryParse(Window.DesiredSizeValue.Text, out UInt64 value))
            {
                SetDesiredTextBox(0);
                value = 0;
            }

            desiredInByte = ByteFormatter.SizeFromTo<UInt64, UInt64>(value, Unit.MB, Unit.B);
            OnDesiredSizeChanged(sender);
        }

        private void OnDesiredSizeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            desiredInByte = System.Convert.ToUInt64(Window.DesiredSlider.Value);
            OnDesiredSizeChanged(sender);
        }

        private void OnDesiredSizeChanged(object sender)
        {
            if (desiredInByte > availableInByte)
            {
                desiredInByte = availableInByte;
                SetDesiredTextBox(desiredInByte);
            }

            if (sender == Window.DesiredSlider)
            {
                SetDesiredTextBox(desiredInByte);
            }

            if (sender == Window.DesiredSizeValue)
            {
                SetSliderValue(desiredInByte);
            }

            SetFormattedLabel(desiredInByte);
        }

        public void SetupSlider(double min, double max)
        {
            Window.DesiredSlider.Minimum = min;
            Window.DesiredSlider.Maximum = max;
        }

        public void SetSliderValue(double value)
        {
            Window.DesiredSlider.Value = value;
        }

        public void SetFormattedLabel(ulong size)
        {
            Window.DesiredFormatted.Content = ByteFormatter.BytesToAsString(size);
        }


        public void SetAvailableLabel(ulong size)
        {
            Window.AvailableLabel.Content = ByteFormatter.BytesToAsString(size);
        }

        public void SetDesiredTextBox(ulong size)
        {
            Window.DesiredSizeValue.Text = ByteFormatter.BytesToAsString(size, false, Unit.MB, 0);
        }

        #region OnClick

        private void OnConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            string output = string.Empty;

            uint desiredMB = ByteFormatter.BytesTo<UInt64, UInt32>(desiredInByte, Unit.MB);
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
            string output = string.Empty;

            output += Partition.WSM.GetOutputAsString();
            output += Partition.DefragAnalysis.GetOutputAsString();
            Log.Print(output);

            availableInByte = Partition.DefragAnalysis.AvailableForExtend;
            SetupSlider(0.0d, availableInByte);
            SetAvailableLabel(availableInByte);
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
