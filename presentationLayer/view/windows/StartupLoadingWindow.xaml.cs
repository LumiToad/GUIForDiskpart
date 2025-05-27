using GUIForDiskpart.main;
using System;
using System.Windows;

namespace GUIForDiskpart.Presentation.View.Windows
{
    /// <summary>
    /// Interaction logic for StartupLoadingWindow.xaml
    /// </summary>
    public partial class StartupLoadingWindow : Window
    {
        public StartupLoadingWindow()
        {
            InitializeComponent();
            ChangeLoadingText("Drives");
        }

        public void ChangeLoadingText(string text)
        {
            LoadingLabel.Content = $"Retrieving: {text}";
        }
    }
}
