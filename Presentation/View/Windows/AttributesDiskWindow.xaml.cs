using GUIForDiskpart.Model.Logic.Diskpart;
using GUIForDiskpart.Presentation.Presenter;
using System.Windows;

namespace GUIForDiskpart.Presentation.View.Windows
{
    /// <summary>
    /// Interaction logic for AttributesWindow.xaml
    /// </summary>
    public partial class AttributesDiskWindow : Window
    {
        MainWindow<GUIFDMainWin> MainWindow = App.Instance.WIM.GetPresenter<MainWindow<GUIFDMainWin>>();

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

        public AttributesDiskWindow(DiskModel diskModel)
        {
            InitializeComponent();
            diskModel = DiskModel;
        }

        private void SetButton_Clear(object sender, RoutedEventArgs e)
        {
            string output = string.Empty;

            output += DPFunctions.AttributesDisk(DiskModel.DiskIndex, true, false);

            MainWindow.Log.Print(output);
            this.Close();
        }

        private void ClearButton_Clear(object sender, RoutedEventArgs e)
        {
            string output = string.Empty;

            output += DPFunctions.AttributesDisk(DiskModel.DiskIndex, false, false);

            MainWindow.Log.Print(output);
            this.Close();
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
