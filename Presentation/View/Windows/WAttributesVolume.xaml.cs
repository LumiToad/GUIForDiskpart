using System.Windows;
using GUIForDiskpart.Database.Data.Diskpart;
using GUIForDiskpart.Model.Data;
using GUIForDiskpart.Model.Logic.Diskpart;
using GUIForDiskpart.Presentation.Presenter;

namespace GUIForDiskpart.Presentation.View.Windows
{
    /// <summary>
    /// Interaction logic for AttributesVolumeWindow.xaml
    /// </summary>
    public partial class WAttributesVolume : Window
    {
        public delegate void DOnClick(object sender, RoutedEventArgs e);
        public event DOnClick ESet;
        public event DOnClick EClear;
        public event DOnClick ECancel;

        public WAttributesVolume()
        {
            InitializeComponent();
        }

        private void SetButton_Click(object sender, RoutedEventArgs e) => ESet?.Invoke(sender, e);
        private void ClearButton_Click(object sender, RoutedEventArgs e) => EClear?.Invoke(sender, e);
        private void CancelButton_Click(object sender, RoutedEventArgs e) => ECancel?.Invoke(sender, e);
    }
}
