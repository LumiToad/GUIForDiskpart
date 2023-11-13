using GUIForDiskpart.diskpart;
using GUIForDiskpart.main;
using System.Windows;

namespace GUIForDiskpart.windows
{
    /// <summary>
    /// Interaction logic for AssignLetterWindow.xaml
    /// </summary>
    public partial class AssignLetterWindow : Window
    {
        MainWindow MainWindow => (MainWindow)Application.Current.MainWindow;

        WSMPartition wsmPartition;
        WSMPartition WSMPartition
        {
            get {return wsmPartition;}
            set
            {
                wsmPartition = value;
                AddTextToConsole(wsmPartition.GetOutputAsString());
            }
        }

        public AssignLetterWindow(WSMPartition wsmPartition)
        {
            InitializeComponent();

            PopulateDriveLetterBox();
            DiskRetriever.OnDiskChanged += PopulateDriveLetterBox;
            
            WSMPartition = wsmPartition;
        }

        private void PopulateDriveLetterBox()
        {
            DriveLetterBox.Items.Clear();
            DriveLetterBox.Items.Add("Auto Select");
            
            foreach (char availableLetter in DriveLetterManager.GetAvailableDriveLetters())
            {
                DriveLetterBox.Items.Add(availableLetter);
            }
            DriveLetterBox.SelectedItem = DriveLetterBox.Items[0];
        }

        private void ExecuteAssign()
        {
            string output = string.Empty;

            char? letter = null;
            if (DriveLetterBox.SelectedValue != "Auto Select")
            {
                letter = (char?)DriveLetterBox.SelectedValue;
            }

            output += DPFunctions.Assign(WSMPartition.DiskNumber, WSMPartition.PartitionNumber, letter, true);

            MainWindow.AddTextToOutputConsole(output);
            MainWindow.RetrieveAndShowDiskData(false);
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            ExecuteAssign();
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
