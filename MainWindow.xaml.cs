using System;
using System.Management;
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

        private void ConsoleReturn_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            ConsoleReturn.ScrollToEnd();
        }

        private void AddTextToOutputConsole(string text)
        {
            ConsoleReturn.Text += text;
        }

        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
            ConsoleReturn.Text += mainProgram.dpFunctions.List(diskpart.DPListType.VOLUME);
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ConsoleReturn.Text += mainProgram.dpFunctions.List(diskpart.DPListType.DISK);

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ConsoleReturn.Text += mainProgram.dpFunctions.List(diskpart.DPListType.PARTITION);

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            ConsoleReturn.Text += mainProgram.dpFunctions.List(diskpart.DPListType.VDISK);

        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            mainProgram.driveRetriever.RetrieveDrives();
            AddTextToOutputConsole(mainProgram.driveRetriever.GetLogicalDrivesOutput());
        }
    }
}
