using System.Globalization;
using System;
using System.Windows;
using System.Windows.Controls;


namespace GUIForDiskpart.Presentation.View.Windows
{
    /// <summary>
    /// Interaction logic for ExtendWindow.xaml
    /// </summary>
    public partial class WExtend : Window
    {
        public delegate void DOnClick(object sender, RoutedEventArgs e);
        public event DOnClick EConfirm;
        public event DOnClick ECancel;

        public delegate void DOnTextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e);
        public event DOnTextChanged ETextChanged;

        public delegate void DOnSliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e);
        public event DOnSliderValueChanged ESliderValueChanged;

        public WExtend()
        {
            InitializeComponent();
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e) => EConfirm?.Invoke(sender, e);
        private void CancelButton_Click(object sender, RoutedEventArgs e) => ECancel?.Invoke(sender, e);
        private void DesiredSizeValue_TextChanged(object sender, TextChangedEventArgs e) => ETextChanged?.Invoke(sender, e);
        private void DesiredSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) => ESliderValueChanged?.Invoke(sender, e);

    }
}
