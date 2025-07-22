using System.Windows;


namespace GUIForDiskpart.Presentation.View.Windows
{
    /// <summary>
    /// Interaction logic for AttributesVolumeByIndexWindow.xaml
    /// </summary>
    public partial class WAttributesVolByIdx : Window
    {
        public delegate void DOnClick(object sender, RoutedEventArgs e);
        public event DOnClick ESet;
        public event DOnClick EClear;
        public event DOnClick ECancel;

        public delegate void DOnTextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e);
        public event DOnTextChanged ETextChanged;

        public WAttributesVolByIdx()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e) => ETextChanged?.Invoke(sender, e);
        private void SetButton_Click(object sender, RoutedEventArgs e) => ESet?.Invoke(sender, e);
        private void ClearButton_Click(object sender, RoutedEventArgs e) => EClear?.Invoke(sender, e);
        private void CancelButton_Click(object sender, RoutedEventArgs e) => ECancel?.Invoke(sender, e);
    }
}
