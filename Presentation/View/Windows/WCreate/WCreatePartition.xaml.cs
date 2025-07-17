using System.Windows;
using System.Windows.Controls;


namespace GUIForDiskpart.Presentation.View.Windows
{
    /// <summary>
    /// Interaktionslogik für CreatePartition.xaml
    /// </summary>
    public partial class WCreatePartition : Window
    {
        public delegate void DOnTextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e);
        public event DOnTextChanged ETextChanged;

        public delegate void DOnClick(object sender, RoutedEventArgs e);
        public event DOnClick ECreate;
        public event DOnClick ECancel;

        public WCreatePartition()
        {
            InitializeComponent();
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e) => ECreate?.Invoke(sender, e);
        private void CancelButton_Click(object sender, RoutedEventArgs e) => ECancel?.Invoke(sender, e);
        private void SizeValue_TextChanged(object sender, TextChangedEventArgs e) => ETextChanged?.Invoke(sender, e);
    }
}
