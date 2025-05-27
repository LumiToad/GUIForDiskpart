using System.Windows;
using GUIForDiskpart.Database.Data.Diskpart;
using GUIForDiskpart.Model.Data;
using GUIForDiskpart.Model.Logic.Diskpart;

namespace GUIForDiskpart.Presentation.View.Windows
{
    /// <summary>
    /// Interaction logic for AttributesVolumeWindow.xaml
    /// </summary>
    public partial class AttributesVolumeWindow : Window
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

        public AttributesVolumeWindow(WSMPartition partition)
        {
            InitializeComponent();
            WSMPartition = partition;
            PopulateAttributesCombobox();
            MBRLabel.Text = (WSMPartition.PartitionTable == "MBR" ? "Will effect EVERY Volume on MBR drives!" : "Will effect just THIS Volume on GPT drives");
            DriveLetterLabel.Content = WSMPartition.DriveLetter + ":/";
        }

        private void PopulateAttributesCombobox()
        {
            Attributes.Items.Add(AttributesOptions.HIDDEN);
            Attributes.Items.Add(AttributesOptions.READONLY);
            Attributes.Items.Add(AttributesOptions.NODEFAULTDRIVELETTER);
            Attributes.Items.Add(AttributesOptions.SHADOWCOPY);
        }

        private void SetButton_Click(object sender, RoutedEventArgs e)
        {
            string output = string.Empty;

            output += DPFunctions.AttributesVolume(WSMPartition.DriveLetter, true, (string)Attributes.SelectedItem, true);

            MainWindow.AddTextToOutputConsole(output);
            this.Close();
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            string output = string.Empty;

            output += DPFunctions.AttributesVolume(WSMPartition.DriveLetter, false, (string)Attributes.SelectedItem, true);

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
