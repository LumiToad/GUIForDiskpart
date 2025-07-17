using System;
using System.Windows;
using System.Windows.Controls;

using GUIForDiskpart.Model.Logic.Diskpart;
using GUIForDiskpart.Presentation.Presenter;


namespace GUIForDiskpart.Presentation.View.Windows
{
    /// <summary>
    /// Interaction logic for CreateVolumeWindow.xaml
    /// </summary>
    public partial class WCreateVolume : Window
    {
        public delegate void DOnTextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e);
        public event DOnTextChanged ETextChanged;

        public delegate void DOnClick(object sender, RoutedEventArgs e);
        public event DOnClick ECreate;
        public event DOnClick ECancel;

        public WCreateVolume()
        {
            InitializeComponent();
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e) => ECreate?.Invoke(sender, e);
        private void CancelButton_Click(object sender, RoutedEventArgs e) => ECancel?.Invoke(sender, e);
        private void SizeValue_TextChanged(object sender, TextChangedEventArgs e) => ETextChanged?.Invoke(sender, e);
    }
}
