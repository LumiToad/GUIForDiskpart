using System;
using System.Windows;
using GUIForDiskpart.main;

namespace GUIForDiskpart
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainProgram mainProgram;

        public MainWindow()
        {
            InitializeComponent();

            Initialize();
        }

        private void Initialize()
        {
            mainProgram = new MainProgram();
            mainProgram.Initialize();
            
        }

        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
            ConsoleReturn.Text += mainProgram.dpFunctions.List(diskpart.DPListType.VOLUME);
        }

        private void ConsoleReturn_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            ConsoleReturn.ScrollToEnd();
        }
    }
}
