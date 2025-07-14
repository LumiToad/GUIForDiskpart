using GUIForDiskpart.Database.Data.Diskpart;
using GUIForDiskpart.Model.Logic.Diskpart;
using GUIForDiskpart.Presentation.Presenter;
using System.Windows;
using System.Windows.Controls;

namespace GUIForDiskpart.Presentation.View.Windows
{
    /// <summary>
    /// Interaktionslogik für ConvertDriveWindow.xaml
    /// </summary>
    public partial class WConvertDrive : Window
    {
        public delegate void DOnClick(object sender, RoutedEventArgs e);
        public event DOnClick EConfirm;
        public event DOnClick ECancel;

        public WConvertDrive()
        {
            InitializeComponent();
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e) => EConfirm?.Invoke(sender, e);
        private void CancelButton_Click(object sender, RoutedEventArgs e) => ECancel?.Invoke(sender, e);
    }
}
