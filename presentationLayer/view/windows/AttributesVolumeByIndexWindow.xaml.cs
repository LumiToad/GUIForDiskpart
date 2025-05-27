using GUIForDiskpart.Database.Data.diskpart;
using GUIForDiskpart.Model.Logic.Diskpart;
using System;
using System.Text.RegularExpressions;
using System.Windows;

namespace GUIForDiskpart.Presentation.View.Windows
{
    /// <summary>
    /// Interaction logic for AttributesVolumeByIndexWindow.xaml
    /// </summary>
    public partial class AttributesVolumeByIndexWindow : Window
    {
        MainWindow MainWindow => (MainWindow)Application.Current.MainWindow;

        int selectedVolume = -1;

        public AttributesVolumeByIndexWindow()
        {
            InitializeComponent();
            PopulateAttributesCombobox();
            MBRLabel.Text = "Will effect EVERY Volume on MBR drives!, will effect just THIS Volume on GPT drives";
            ConsoleReturn.AddTextToOutputConsole(DPFunctions.List(DPListType.VOLUME));
        }

        private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            int lastCarretIndex = VolumeNumber.CaretIndex;

            if (!int.TryParse(VolumeNumber.Text, out selectedVolume))
            {
                VolumeNumber.Text = Regex.Replace(VolumeNumber.Text, "[^0-9]", "");
                VolumeNumber.CaretIndex = lastCarretIndex;
            }

            if (VolumeNumber.Text.Length > 5)
            {
                VolumeNumber.Text = VolumeNumber.Text.Remove(5);
                VolumeNumber.CaretIndex = lastCarretIndex;
            }

            if (!string.IsNullOrEmpty(VolumeNumber.Text))
            {
                selectedVolume = Convert.ToInt32(VolumeNumber.Text);
            }
            else selectedVolume = -1;
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
            if (selectedVolume == -1) return;
            string output = string.Empty;

            output += DPFunctions.AttributesVolume(selectedVolume, true, (string)Attributes.SelectedItem, true);

            MainWindow.AddTextToOutputConsole(output);
            this.Close();
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedVolume == -1) return;

            string output = string.Empty;

            output += DPFunctions.AttributesVolume(selectedVolume, false, (string)Attributes.SelectedItem, true);

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
