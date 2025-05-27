using GUIForDiskpart.Model.Logic.Diskpart;
using System.Windows;

namespace GUIForDiskpart.Presentation.View.Windows
{
    /// <summary>
    /// Interaction logic for AttributesWindow.xaml
    /// </summary>
    public partial class AttributesDiskWindow : Window
    {
        Window? MainWindow = GUIForDiskpart.App.AppInstance.MainWindow;

        private DiskModel diskModel;
        public DiskModel DiskModel
        {
            get { return DiskModel; }
            set
            {
                diskModel = value;
                AddTextToConsole(DiskModel.GetOutputAsString());
            }
        }

        public AttributesDiskWindow(DiskModel diskModel)
        {
            InitializeComponent();
            diskModel = DiskModel;
        }

        private void SetButton_Clear(object sender, RoutedEventArgs e)
        {
            string output = string.Empty;

            output += DPFunctions.AttributesDisk(DiskModel.DiskIndex, true, false);

            MainWindow.AddTextToOutputConsole(output);
            this.Close();
        }

        private void ClearButton_Clear(object sender, RoutedEventArgs e)
        {
            string output = string.Empty;

            output += DPFunctions.AttributesDisk(DiskModel.DiskIndex, false, false);

            MainWindow.AddTextToOutputConsole(output);
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AddTextToConsole(string text)
        {
            ConsoleReturn.AddTextToOutputConsole(text);
        }
    }
}
