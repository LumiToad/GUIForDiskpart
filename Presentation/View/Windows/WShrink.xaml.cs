using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

using GUIForDiskpart.Model.Data;
using GUIForDiskpart.Model.Logic.Diskpart;
using GUIForDiskpart.Service;
using GUIForDiskpart.Utils;


namespace GUIForDiskpart.Presentation.View.Windows
{
    /// <summary>
    /// Interaction logic for ShrinkWindow.xaml
    /// </summary>
    public partial class WShrink : Window
    {
        public delegate void DOnClick(object sender, RoutedEventArgs e);
        public event DOnClick EConfirm;
        public event DOnClick ECancel;

        public delegate void DOnTextChanged(object sender, TextChangedEventArgs e);
        public event DOnTextChanged EMinTextChanged;
        public event DOnTextChanged EDesiredTextChanged;

        public delegate void DOnSliderChanged(object sender, RoutedPropertyChangedEventArgs<double> e);
        public event DOnSliderChanged EMinSlider;
        public event DOnSliderChanged EDesiredSlider;

        public WShrink()
        {
            InitializeComponent();
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e) => EConfirm?.Invoke(sender, e);
        private void CancelButton_Click(object sender, RoutedEventArgs e) => ECancel?.Invoke(sender, e);
        private void MinimumSizeValue_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e) => EMinTextChanged?.Invoke(sender, e);
        private void DesiredSizeValue_TextChanged(object sender, TextChangedEventArgs e) => EDesiredTextChanged?.Invoke(sender, e);
        private void MinimumSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) => EMinSlider?.Invoke(sender, e);
        private void DesiredSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) => EDesiredSlider?.Invoke(sender, e);
    }
}
