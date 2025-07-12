using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GUIForDiskpart.Presentation.View.Windows
{
    /// <summary>
    /// Interaction logic for SecurityCheckWindow.xaml
    /// </summary>
    public partial class WSecurityCheck : Window
    {
        public delegate void DOnClick(object sender, RoutedEventArgs e);
        public event DOnClick EConfirm;
        public event DOnClick ECancel;

        public delegate void DOnTextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e);
        public event DOnTextChanged ETextChanged;

        public WSecurityCheck()
        {
            InitializeComponent();
            KeyUp += Confirm_KeyUp;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e) => ETextChanged?.Invoke(sender, e);

        private void Confirm_Click(object sender, RoutedEventArgs e) => EConfirm?.Invoke(sender, e);
        private void Cancel_Click(object sender, RoutedEventArgs e) => ECancel?.Invoke(sender, e);

        private void Confirm_KeyUp(object sender, KeyEventArgs e)
        {
            e.Handled = false;
            if (e.Key == Key.Enter && Confirm.IsEnabled)
            {
                EConfirm?.Invoke(sender, e);
            }
        }
    }
}
