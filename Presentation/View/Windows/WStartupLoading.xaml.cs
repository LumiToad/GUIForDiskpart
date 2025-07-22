using System.Windows;

namespace GUIForDiskpart.Presentation.View.Windows
{
    /// <summary>
    /// Interaction logic for StartupLoadingWindow.xaml
    /// </summary>
    public partial class WStartupLoading : Window
    {
        public WStartupLoading()
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
