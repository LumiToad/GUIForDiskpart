using GUIForDiskpart.Model.Logic.Diskpart;
using GUIForDiskpart.Presentation.Presenter;
using System.Windows;

namespace GUIForDiskpart.Presentation.View.Windows
{
    /// <summary>
    /// Interaction logic for AttributesWindow.xaml
    /// </summary>
    public partial class WAttributesDisk : Window
    {
        public delegate void DOnClick(object sender, RoutedEventArgs e);
        public event DOnClick ESet;
        public event DOnClick EClear;
        public event DOnClick ECancel;

        private void SetButton_Click(object sender, RoutedEventArgs e) => ESet?.Invoke(sender, e);
        private void ClearButton_Click(object sender, RoutedEventArgs e) => EClear?.Invoke(sender, e);
        private void CancelButton_Click(object sender, RoutedEventArgs e) => ECancel?.Invoke(sender, e);
    }
}
