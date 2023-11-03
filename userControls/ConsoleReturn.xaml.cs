using System;
using System.Windows.Controls;

namespace GUIForDiskpart.userControls
{
    /// <summary>
    /// Interaction logic for ConsoleReturn.xaml
    /// </summary>
    public partial class ConsoleReturnUI : UserControl
    {
        public ConsoleReturnUI()
        {
            InitializeComponent();
        }

        private void ConsoleReturn_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            TextBox.ScrollToEnd();
        }

        public void AddTextToOutputConsole(string text)
        {
            TextBox.Text += "\n";
            TextBox.Text += "[" + DateTime.Now + "]\n";
            TextBox.Text += text;
        }
    }
}
