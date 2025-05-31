using GUIForDiskpart.Database.Retrievers;
using Markdig.Helpers;
using System.Drawing;
using System.Windows;
using System.Windows.Media;

using GUIForDiskpart.Model.Data;
using GUIForDiskpart.Model.Logic.Diskpart;
using GUIForDiskpart.Service;
namespace GUIForDiskpart.Presentation.View.Windows
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
                if (wsmPartition != null && wsmPartition.DriveLetter == '\0')
                {
                    RemoveButton.IsEnabled = false;
                    RemoveButton.Foreground = System.Windows.Media.Brushes.DarkGray;
                }
            }
        }

        public AssignLetterWindow(WSMPartition wsmPartition)
        {
            InitializeComponent();

            PopulateDriveLetterBox();
            DiskService.OnDiskChanged += PopulateDriveLetterBox;
            
            WSMPartition = wsmPartition;
        }

        private void PopulateDriveLetterBox()
        {
            DriveLetterBox.Items.Clear();
            DriveLetterBox.Items.Add("Auto Select");
            
            foreach (char availableLetter in DriveLetterService.GetAvailableDriveLetters())
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

            MainWindow.LogPrint(output);
            MainWindow.RetrieveAndShowDiskData(false);
        }

        private void ExecuteRemove()
        {
            string output = string.Empty;
            char letter = wsmPartition.DriveLetter;

            output += DPFunctions.Remove(letter, false, true);

            MainWindow.LogPrint(output);
            MainWindow.RetrieveAndShowDiskData(false);
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            ExecuteAssign();
            this.Close();
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            ExecuteRemove();
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
