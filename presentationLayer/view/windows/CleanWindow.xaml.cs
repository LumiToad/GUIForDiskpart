using GUIForDiskpart.Model.Logic.Diskpart;

using System.Windows;

namespace GUIForDiskpart.Presentation.View.Windows
{
    /// <summary>
    /// Interaction logic for CleanWindow.xaml
    /// </summary>
    public partial class CleanWindow : Window
    {
        MainWindow MainWindow => (MainWindow)Application.Current.MainWindow;

        private DiskModel diskModel;
        public DiskModel DiskModel
        {
            get { return diskModel; }
            set
            {
                diskModel = value;
                AddTextToConsole(DiskModel.GetOutputAsString());
            }
        }

        public CleanWindow(DiskModel diskModel)
        {
            InitializeComponent();

            diskModel = DiskModel;
        }

        private void ExecuteClean(bool value)
        {
            if (!value) return;

            string output = string.Empty;

            output += DPFunctions.Clean(DiskModel.DiskIndex, (bool)CleanAll.IsChecked);

            MainWindow.AddTextToOutputConsole(output);
            MainWindow.RetrieveAndShowDiskData(false);

            this.Close();
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            string todo = "Clean the whole drive! ALL DATA WILL BE LOST!";
            if ((bool)CleanAll.IsChecked)
            {
                todo = "Clean and override the whole drive! MAKES DATA RESCUE CLOSE TO IMPOSSIBLE!";
            }
            string confirmKey = DiskModel.PhysicalName;

            SecurityCheckWindow securityCheckWindow = new SecurityCheckWindow(ExecuteClean,todo, confirmKey);
            securityCheckWindow.Owner = this;
            securityCheckWindow.Show();
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
