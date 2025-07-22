using System.Windows;


namespace GUIForDiskpart.Presentation.View.Windows
{
    /// <summary>
    /// Interaktionslogik für AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        string buildNumber;

        public AboutWindow(string buildNumber)
        {
            InitializeComponent();
            this.buildNumber = buildNumber;
            SetBuildNumberToUI();
        }

        private void SetBuildNumberToUI()
        {
            BuildNumberValue.Content = buildNumber;
        }
    }
}
