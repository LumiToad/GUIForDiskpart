using System;
using System.Windows;
using System.Windows.Controls;

using GUIForDiskpart;
using GUIForDiskpart.Utils;


namespace GUIForDiskpart.Presentation.View.UserControls
{
    /// <summary>
    /// Interaction logic for ConsoleReturn.xaml
    /// </summary>
    public partial class ConsoleReturnUI : UserControl
    {
        MainWindow MainWindow => (MainWindow)Application.Current.MainWindow;

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

        private void SaveLog_Click(object sender, RoutedEventArgs e) 
        {
            SaveLog();
        }

        public void SaveLog()
        {
            string log = TextBox.Text;

            FileUtils.SaveAsTextfile(log, "log");
        }
    }
}
