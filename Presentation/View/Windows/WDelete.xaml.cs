using GUIForDiskpart.Model.Data;
using GUIForDiskpart.Model.Logic.Diskpart;
using GUIForDiskpart.Presentation.Presenter;
using System.Windows;
using static System.Net.Mime.MediaTypeNames;

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
            string text = "Delete the whole partition! ALL DATA WILL BE LOST!";
            if ((bool)CleanAll.IsChecked)
            {
                text = "Delete and override the whole partition! MAKES DATA RESCUE CLOSE TO IMPOSSIBLE!";
            }
            string confirmKey = $"Drive: {WSMPartition.DiskNumber} Partition: {WSMPartition.PartitionNumber}";

            var secCheck = App.Instance.WIM.CreateWPresenter<PSecurityCheck>(true, text, confirmKey);
            secCheck.Window.Owner = this;
            secCheck.ESecCheck += ExecuteDelete;
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
