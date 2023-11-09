using GUIForDiskpart.main;
using GUIForDiskpart.cmd;
using System.Windows;
using Microsoft.Win32;

namespace GUIForDiskpart.windows
{
    /// <summary>
    /// Interaction logic for CHKDSKWindow.xaml
    /// </summary>
    public partial class CHKDSKWindow : Window
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
                DriveLetterTop.Content = wsmPartition.WMIPartition.LogicalDriveInfo.VolumeName + " " + wsmPartition.DriveLetter + ":\\";
                DriveLetterBottom.Content = wsmPartition.DriveLetter + ":\\";
            }
        }

        private string oldKBValue = "0";

        public CHKDSKWindow(WSMPartition wsmPartition)
        {
            InitializeComponent();

            WSMPartition = wsmPartition;
        }

        private void AddTextToConsole(string text)
        {
            ConsoleReturn.AddTextToOutputConsole(text);
        }

        private void ParaF_Checked(object sender, RoutedEventArgs e)
        {
            WriteTextBoxPara((bool)ParaF.IsChecked, CMDFunctions.FixErrors);
        }

        private void ParaR_Checked(object sender, RoutedEventArgs e)
        {
            WriteTextBoxPara(ParaR.IsChecked, CMDFunctions.LocateBad);
        }

        private void ParaX_Checked(object sender, RoutedEventArgs e)
        {
            WriteTextBoxPara(ParaX.IsChecked, CMDFunctions.FixErrorsForceDismount);
        }

        private void ParaV_Checked(object sender, RoutedEventArgs e)
        {
            WriteTextBoxPara(ParaV.IsChecked, CMDFunctions.DisplayNameOfEachFile);
        }

        private void ParaOfflineScanAndFix_Checked(object sender, RoutedEventArgs e)
        {
            WriteTextBoxPara(ParaOfflineScanAndFix.IsChecked, CMDFunctions.OfflineScanAndFix);
        }

        private void ParaI_Checked(object sender, RoutedEventArgs e)
        {
            WriteTextBoxPara(ParaI.IsChecked, CMDFunctions.LessVigorousNTFS);
        }

        private void ParaC_Checked(object sender, RoutedEventArgs e)
        {
            WriteTextBoxPara(ParaC.IsChecked, CMDFunctions.NoCycleCheckNTFS);
        }

        private void ParaL_Checked(object sender, RoutedEventArgs e)
        {
            if (ParaL.IsChecked == false)
            { 
                if (!string.IsNullOrEmpty(TextBoxL.Text) && TextBoxPara.Text.Contains(TextBoxL.Text))
                {
                    TextBoxPara.Text = TextBoxPara.Text.Replace(TextBoxL.Text, "");
                }
            }
            
            WriteTextBoxPara(ParaL.IsChecked, CMDFunctions.LogFileSizeNTFS);
            
            if (ParaL.IsChecked == true)
            {
                ReplaceKBValueText();
            }
        }

        private void TextBoxL_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            ReplaceKBValueText();
        }

        private void ReplaceKBValueText()
        {
            if (TextBoxPara.Text.Contains(oldKBValue))
            {
                TextBoxPara.Text = TextBoxPara.Text.Replace(oldKBValue, TextBoxL.Text);
            }
            else
            {
                int index = TextBoxPara.Text.IndexOf(CMDFunctions.LogFileSizeNTFS);
                if (index == -1) return;
                TextBoxPara.Text = TextBoxPara.Text.Insert(index + 3, TextBoxL.Text);
            }

            if (string.IsNullOrEmpty(TextBoxL.Text)) return;
            oldKBValue = TextBoxL.Text;
        }

        private void ParaB_Checked(object sender, RoutedEventArgs e)
        {
            WriteTextBoxPara(ParaB.IsChecked, CMDFunctions.ClearBadSectorListNTFS);
        }

        private void ParaScan_Checked(object sender, RoutedEventArgs e)
        {
            WriteTextBoxPara(ParaScan.IsChecked, CMDFunctions.ScanNTFS);
            ParaForceOff.IsChecked = false;
            WriteTextBoxPara(ParaForceOff.IsChecked, CMDFunctions.ForceOffLineFixNTFS);
        }

        private void ParaForceOff_Checked(object sender, RoutedEventArgs e)
        {
            WriteTextBoxPara(ParaForceOff.IsChecked, CMDFunctions.ForceOffLineFixNTFS);
        }

        private void ParaPerf_Checked(object sender, RoutedEventArgs e)
        {
            WriteTextBoxPara(ParaPerf.IsChecked, CMDFunctions.FullPerformance);
        }

        private void ParaSpotfix_Checked(object sender, RoutedEventArgs e)
        {
            WriteTextBoxPara(ParaSpotfix.IsChecked, CMDFunctions.SpotFix);
        }

        private void ParaSD_Checked(object sender, RoutedEventArgs e)
        {
            WriteTextBoxPara(ParaSD.IsChecked, CMDFunctions.SDCleanNTFS);
        }

        private void ParaFree_Checked(object sender, RoutedEventArgs e)
        {
            WriteTextBoxPara(ParaFree.IsChecked, CMDFunctions.FreeOrphanedFATfamily);
        }

        private void ParaMarkClean_Checked(object sender, RoutedEventArgs e)
        {
            WriteTextBoxPara(ParaMarkClean.IsChecked, CMDFunctions.MarkCleanFATfamily);
        }

        private void WriteTextBoxPara(bool? value, string parameter)
        {
            if (value == true)
            {
                TextBoxPara.Text += $" {parameter} ";
            }
            else
            {
                if (TextBoxPara.Text.Contains(parameter))
                {
                    TextBoxPara.Text = TextBoxPara.Text.Replace($" {parameter} ", "");
                }
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            string output = string.Empty;

            string command = TextBoxPara.Text;
            if (!string.IsNullOrEmpty(TextBoxDir.Text))
            {
                command += $" > {TextBoxDir.Text} ";
            }

            output += CMDFunctions.CHKDSK(WSMPartition.DriveLetter, TextBoxPara.Text);

            MainWindow.AddTextToOutputConsole(output);

            this.Close();
        }

        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text file (*.txt)|*.txt";

            if (saveFileDialog.ShowDialog() == true)
            {
                TextBoxDir.Text = saveFileDialog.FileName;
            }
        }
    }
}
