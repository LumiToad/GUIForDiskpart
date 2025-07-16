using System.Windows;
using System.Windows.Controls;


namespace GUIForDiskpart.Presentation.View.Windows
{
    /// <summary>
    /// Interaktionslogik für FormatWindow.xaml
    /// </summary>
    public partial class WEasyFormat : Window
    {
        public delegate void DOnClick(object sender, RoutedEventArgs e);
        public event DOnClick EConfirm;
        public event DOnClick ECancel;

        public delegate void DOnTextChanged(object sender, TextChangedEventArgs e);
        public event DOnTextChanged ETextChanged;

        public delegate void DOnSelectionChanged(object sender, SelectionChangedEventArgs e);
        public event DOnSelectionChanged ESelectionChanged;

        public WEasyFormat()
        {
            InitializeComponent();
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e) => EConfirm?.Invoke(sender, e);
        private void CancelButton_Click(object sender, RoutedEventArgs e) => EConfirm?.Invoke(sender, e);
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) => ESelectionChanged?.Invoke(sender, e);
        private void SizeValue_TextChanged(object sender, TextChangedEventArgs e) => ETextChanged?.Invoke(sender, e);
    }
}
