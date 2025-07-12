using GUIForDiskpart.Model.Data;
using GUIForDiskpart.Model.Logic.Diskpart;
using GUIForDiskpart.Presentation.Presenter;
using System.Windows;

namespace GUIForDiskpart.Presentation.View.Windows
{
    /// <summary>
    /// Interaction logic for DeleteWindow.xaml
    /// </summary>
    public partial class WDelete : Window
    {
        PMainWindow MainWindow = App.Instance.WIM.GetPresenter<PMainWindow>();

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

        public WDelete(WSMPartition wsmPartition)
        {
            InitializeComponent();

            WSMPartition = wsmPartition;
        }

        private void ExecuteDelete(bool value)
        {
            if (!value) return;

            string output = string.Empty;

            output += DPFunctions.Delete(WSMPartition.DiskNumber, WSMPartition.PartitionNumber, true, (bool)CleanAll.IsChecked);

            MainWindow.Log.Print(output);
            MainWindow.DisplayDiskData(false);

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

            WSecurityCheck securityCheckWindow = new WSecurityCheck(ExecuteDelete, todo, confirmKey);
            securityCheckWindow.Owner = this;
            securityCheckWindow.Show();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AddTextToConsole(string text)
        {
            ConsoleReturn.Print(text);
        }
    }
}
