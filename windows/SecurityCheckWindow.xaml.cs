using System.Windows;
using System.Windows.Controls;

namespace GUIForDiskpart.windows
{
    /// <summary>
    /// Interaction logic for SecurityCheckWindow.xaml
    /// </summary>
    public partial class SecurityCheckWindow : Window
    {
        public delegate void Result(bool result);
        public event Result OnClick;

        public SecurityCheckWindow(string todo, string confirmKey)
        {
            InitializeComponent();
            AboutTo.Content = todo;
            ConfirmText.Content = confirmKey;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextBox.Text == (string)ConfirmText.Content)
            {
                Confirm.IsEnabled = true;
            }
            else
            {
                Confirm.IsEnabled = false;
            }
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            OnClick(true);
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            OnClick(false);
            this.Close();
        }
    }
}
