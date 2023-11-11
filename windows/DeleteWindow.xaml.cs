using GUIForDiskpart.diskpart;
using GUIForDiskpart.main;
using System.Windows;

namespace GUIForDiskpart.windows
{
    /// <summary>
    /// Interaction logic for DeleteWindow.xaml
    /// </summary>
    public partial class DeleteWindow : Window
    {
        MainWindow MainWindow => (MainWindow)Application.Current.MainWindow;

        private WSMPartition wsmPartition;
        public WSMPartition WSMPartition
        {
            get { return wsmPartition; }
            set
            {
                wsmPartition = value;
                AddTextToConsole(wsmPartition.GetOutputAsString());
            }
        }

        public DeleteWindow(WSMPartition wsmPartition)
        {
            InitializeComponent();

            WSMPartition = wsmPartition;
        }

        private void ExecuteDelete(bool value)
        {
            if (!value) return;

            string output = string.Empty;

            output += DPFunctions.Delete(WSMPartition.DiskNumber, WSMPartition.PartitionNumber, true, (bool)CleanAll.IsChecked);

            MainWindow.AddTextToOutputConsole(output);
            MainWindow.RetrieveAndShowDiskData(true);

            this.Close();
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            string todo = "Delete the whole partition! ALL DATA WILL BE LOST!";
            if ((bool)CleanAll.IsChecked)
            {
                todo = "Delete and override the whole partition! MAKES DATA RESCUE CLOSE TO IMPOSSIBLE!";
            }
            string confirmKey = $"Drive: {WSMPartition.DiskNumber} Partition: {WSMPartition.PartitionNumber}";

            SecurityCheckWindow securityCheckWindow = new SecurityCheckWindow(todo, confirmKey);
            securityCheckWindow.Owner = this;
            securityCheckWindow.OnClick += ExecuteDelete;
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
