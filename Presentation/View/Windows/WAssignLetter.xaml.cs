using System.Windows;

using GUIForDiskpart.Model.Data;
using GUIForDiskpart.Model.Logic.Diskpart;
using GUIForDiskpart.Presentation.Presenter;
namespace GUIForDiskpart.Presentation.View.Windows
{
    /// <summary>
    /// Interaction logic for AssignLetterWindow.xaml
    /// </summary>
    public partial class WAssignLetter : Window
    {
        public delegate void DOnClick(object sender, RoutedEventArgs e);
        public event DOnClick EConfirm;
        public event DOnClick ERemove;
        public event DOnClick ECancel;

        public WAssignLetter()
        {
            InitializeComponent();
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e) => EConfirm?.Invoke(sender, e);
        private void RemoveButton_Click(object sender, RoutedEventArgs e) => ERemove?.Invoke(sender, e);
        private void CancelButton_Click(object sender, RoutedEventArgs e) => ECancel?.Invoke(sender, e);
    }
}
