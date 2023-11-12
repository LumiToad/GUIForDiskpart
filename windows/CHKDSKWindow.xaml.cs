using GUIForDiskpart.cmd;
using GUIForDiskpart.main;
using System.Windows;

namespace GUIForDiskpart.windows
{
    /// <summary>
    /// Interaction logic for CHKDSKWindow.xaml
    /// </summary>
    public partial class CHKDSKWindow : Window
    {
        MainWindow MainWindow => (MainWindow)Application.Current.MainWindow;

        private Partition partition;
        public Partition Partition
        {
            get { return partition; }
            set
            {
                partition = value;
                AddTextToConsole(Partition.GetOutputAsString());
                DriveLetterTop.Content = Partition.LogicalDiskInfo.VolumeName + " " + Partition.WSMPartition.DriveLetter + ":\\";
                DriveLetterBottom.Content = Partition.WSMPartition.DriveLetter + ":\\";
            }
        }

        private string oldKBValue = "0";

        public CHKDSKWindow(Partition partition)
        {
            InitializeComponent();

            Partition = partition;
        }

        private void AddTextToConsole(string text)
        {
            ConsoleReturn.AddTextToOutputConsole(text);
        }

        private void ParaF_Checked(object sender, RoutedEventArgs e)
        {
            WriteTextBoxPara((bool)ParaF.IsChecked, CHKDSKParameters.FixErrors);
        }

        private void ParaR_Checked(object sender, RoutedEventArgs e)
        {
            WriteTextBoxPara(ParaR.IsChecked, CHKDSKParameters.LocateBad);
        }

        private void ParaX_Checked(object sender, RoutedEventArgs e)
        {
            WriteTextBoxPara(ParaX.IsChecked, CHKDSKParameters.FixErrorsForceDismount);
        }

        private void ParaV_Checked(object sender, RoutedEventArgs e)
        {
            WriteTextBoxPara(ParaV.IsChecked, CHKDSKParameters.DisplayNameOfEachFile);
        }

        private void ParaOfflineScanAndFix_Checked(object sender, RoutedEventArgs e)
        {
            WriteTextBoxPara(ParaOfflineScanAndFix.IsChecked, CHKDSKParameters.OfflineScanAndFix);
        }

        private void ParaI_Checked(object sender, RoutedEventArgs e)
        {
            WriteTextBoxPara(ParaI.IsChecked, CHKDSKParameters.LessVigorousNTFS);
        }

        private void ParaC_Checked(object sender, RoutedEventArgs e)
        {
            WriteTextBoxPara(ParaC.IsChecked, CHKDSKParameters.NoCycleCheckNTFS);
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
            
            WriteTextBoxPara(ParaL.IsChecked, CHKDSKParameters.LogFileSizeNTFS);
            
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
                int index = TextBoxPara.Text.IndexOf(CHKDSKParameters.LogFileSizeNTFS);
                if (index == -1) return;
                TextBoxPara.Text = TextBoxPara.Text.Insert(index + 3, TextBoxL.Text);
            }

            if (string.IsNullOrEmpty(TextBoxL.Text)) return;
            oldKBValue = TextBoxL.Text;
        }

        private void ParaB_Checked(object sender, RoutedEventArgs e)
        {
            WriteTextBoxPara(ParaB.IsChecked, CHKDSKParameters.ClearBadSectorListNTFS);
        }

        private void ParaScan_Checked(object sender, RoutedEventArgs e)
        {
            WriteTextBoxPara(ParaScan.IsChecked, CHKDSKParameters.ScanNTFS);
            ParaForceOff.IsChecked = false;
            WriteTextBoxPara(ParaForceOff.IsChecked, CHKDSKParameters.ForceOffLineFixNTFS);
        }

        private void ParaForceOff_Checked(object sender, RoutedEventArgs e)
        {
            WriteTextBoxPara(ParaForceOff.IsChecked, CHKDSKParameters.ForceOffLineFixNTFS);
        }

        private void ParaPerf_Checked(object sender, RoutedEventArgs e)
        {
            WriteTextBoxPara(ParaPerf.IsChecked, CHKDSKParameters.FullPerformance);
        }

        private void ParaSpotfix_Checked(object sender, RoutedEventArgs e)
        {
            WriteTextBoxPara(ParaSpotfix.IsChecked, CHKDSKParameters.SpotFix);
        }

        private void ParaSD_Checked(object sender, RoutedEventArgs e)
        {
            WriteTextBoxPara(ParaSD.IsChecked, CHKDSKParameters.SDCleanNTFS);
        }

        private void ParaFree_Checked(object sender, RoutedEventArgs e)
        {
            WriteTextBoxPara(ParaFree.IsChecked, CHKDSKParameters.FreeOrphanedFATfamily);
        }

        private void ParaMarkClean_Checked(object sender, RoutedEventArgs e)
        {
            WriteTextBoxPara(ParaMarkClean.IsChecked, CHKDSKParameters.MarkCleanFATfamily);
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            string output = string.Empty;

            TextBoxPara.Text += GetOptionalParameters();
            
            if (string.IsNullOrEmpty(TextBoxDir.Text)) 
            {
                output += CMDFunctions.CHKDSK(Partition.WSMPartition.DriveLetter, TextBoxPara.Text);
            }
            else
            {
                output += CMDFunctions.CHKDSK(Partition.WSMPartition.DriveLetter, TextBoxPara.Text, TextBoxDir.Text);
            }

            MainWindow.AddTextToOutputConsole(output);

            this.Close();
        }

        private string GetOptionalParameters()
        {
            string result = string.Empty;

            if (ParaShutdown.IsChecked == true)
            {
                string item = (string)ParaShutdownSelection.SelectionBoxItem;
                string shutdown = $" & shutdown {CMDBasicCommands.ShutdownForce} ";
                switch (item)
                {
                    case ("Shutdown"):
                        shutdown += $"{CMDBasicCommands.ShutdownNoTimer} ";
                        break;
                    case ("Restart"):
                        shutdown += $"{CMDBasicCommands.ShutdownRestart} ";
                        break;
                }
                result += shutdown;
            }        

            return result;
        }

        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            TextBoxDir.Text = FileUtilites.GetSaveAsTextFilePath("CHKDSK");
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
    }
}
