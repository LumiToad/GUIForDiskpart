using System.Windows;

using GUIForDiskpart.Model.Logic.Diskpart;


namespace GUIForDiskpart.Presentation.View.Windows
{
    /// <summary>
    /// Interaction logic for CleanWindow.xaml
    /// </summary>
    public partial class WClean : Window
    {
        public delegate void DOnClick(object sender, RoutedEventArgs e);
        public event DOnClick EConfirm;
        public event DOnClick ECancel;

        public WClean()
        {
            InitializeComponent();
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e) => EConfirm?.Invoke(sender, e);
        private void CancelButton_Click(object sender, RoutedEventArgs e) => ECancel?.Invoke(sender, e);
    }
}
