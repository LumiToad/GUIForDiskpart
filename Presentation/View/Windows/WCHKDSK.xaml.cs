using GUIForDiskpart.Database.Data;
using GUIForDiskpart.Database.Data.CMD;
using GUIForDiskpart.Model.Data;
using GUIForDiskpart.Model.Logic.CMD;
using GUIForDiskpart.Presentation.Presenter;
using GUIForDiskpart.Utils;
using System.Windows;

namespace GUIForDiskpart.Presentation.View.Windows
{
    /// <summary>
    /// Interaction logic for CHKDSKWindow.xaml
    /// </summary>
    public partial class WCHKDSK : Window
    {
        PMainWindow MainWindow = App.Instance.WIM.GetPresenter<PMainWindow>();

        private Partition partition;
        public Partition Partition
        {
            get { return partition; }
            set
            {
                partition = value;
                AddTextToConsole(Partition.GetOutputAsString());
                DriveLetterTop.Content = Partition.LDModel.VolumeName + " " + Partition.WSMPartition.DriveLetter + ":\\";
                DriveLetterBottom.Content = Partition.WSMPartition.DriveLetter + ":\\";
            }
        }

        private string oldKBValue = "0";

        public WCHKDSK(Partition partition)
        {
            InitializeComponent();

            Partition = partition;
        }

        private void AddTextToConsole(string text)
        {
            ConsoleReturn.Print(text);
        }

        private void ParaF_Checked(object sender, RoutedEventArgs e)
        {
            WriteTextBoxPara((bool)ParaF.IsChecked, CHKDSKParameters.FIXERRORS);
        }

        private void ParaR_Checked(object sender, RoutedEventArgs e)
        {
            WriteTextBoxPara(ParaR.IsChecked, CHKDSKParameters.LOCATEBAD);
        }

        private void ParaX_Checked(object sender, RoutedEventArgs e)
        {
            WriteTextBoxPara(ParaX.IsChecked, CHKDSKParameters.FIXERRORSFORCEDISMOUNT);
        }

        private void ParaV_Checked(object sender, RoutedEventArgs e)
        {
            WriteTextBoxPara(ParaV.IsChecked, CHKDSKParameters.DISPLAYNAMEOFEACHFILE);
        }

        private void ParaOfflineScanAndFix_Checked(object sender, RoutedEventArgs e)
        {
            WriteTextBoxPara(ParaOfflineScanAndFix.IsChecked, CHKDSKParameters.OFFLINESCANANDFIX);
        }

        private void ParaI_Checked(object sender, RoutedEventArgs e)
        {
            WriteTextBoxPara(ParaI.IsChecked, CHKDSKParameters.LESSVIGOROUS_NTFS);
        }

        private void ParaC_Checked(object sender, RoutedEventArgs e)
        {
            WriteTextBoxPara(ParaC.IsChecked, CHKDSKParameters.NOCYCLECHECKNTFS);
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
            
            WriteTextBoxPara(ParaL.IsChecked, CHKDSKParameters.LOGFILESIZE_NTFS);
            
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
                int index = TextBoxPara.Text.IndexOf(CHKDSKParameters.LOGFILESIZE_NTFS);
                if (index == -1) return;
                TextBoxPara.Text = TextBoxPara.Text.Insert(index + 3, TextBoxL.Text);
            }

            if (string.IsNullOrEmpty(TextBoxL.Text)) return;
            oldKBValue = TextBoxL.Text;
        }

        private void ParaB_Checked(object sender, RoutedEventArgs e)
        {
            WriteTextBoxPara(ParaB.IsChecked, CHKDSKParameters.CLEARBADSECTORLIST_NTFS);
        }

        private void ParaScan_Checked(object sender, RoutedEventArgs e)
        {
            WriteTextBoxPara(ParaScan.IsChecked, CHKDSKParameters.SCAN_NTFS);
            ParaForceOff.IsChecked = false;
            WriteTextBoxPara(ParaForceOff.IsChecked, CHKDSKParameters.FORCEOFFLINEFIX_NTFS);
        }

        private void ParaForceOff_Checked(object sender, RoutedEventArgs e)
        {
            WriteTextBoxPara(ParaForceOff.IsChecked, CHKDSKParameters.FORCEOFFLINEFIX_NTFS);
        }

        private void ParaPerf_Checked(object sender, RoutedEventArgs e)
        {
            WriteTextBoxPara(ParaPerf.IsChecked, CHKDSKParameters.FULLPERFORMANCE);
        }

        private void ParaSpotfix_Checked(object sender, RoutedEventArgs e)
        {
            WriteTextBoxPara(ParaSpotfix.IsChecked, CHKDSKParameters.SPOTFIX);
        }

        private void ParaSD_Checked(object sender, RoutedEventArgs e)
        {
            WriteTextBoxPara(ParaSD.IsChecked, CHKDSKParameters.SDCLEAN_NTFS);
        }

        private void ParaFree_Checked(object sender, RoutedEventArgs e)
        {
            WriteTextBoxPara(ParaFree.IsChecked, CHKDSKParameters.FREEORPHANED_FATFAMILY);
        }

        private void ParaMarkClean_Checked(object sender, RoutedEventArgs e)
        {
            WriteTextBoxPara(ParaMarkClean.IsChecked, CHKDSKParameters.MARKCLEAN_FATFAMILY);
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

            MainWindow.Log.Print(output);

            this.Close();
        }

        private string GetOptionalParameters()
        {
            string result = string.Empty;

            if (ParaShutdown.IsChecked == true)
            {
                string item = (string)ParaShutdownSelection.SelectionBoxItem;
                string shutdown = $" & {CMDBasic.CMD_SHUTDOWN} {CMDBasic.SHUTDOWN_FORCE} ";
                switch (item)
                {
                    case ("Shutdown"):
                        shutdown += $"{CMDBasic.SHUTDOWN_NO_TIMER} ";
                        break;
                    case ("Restart"):
                        shutdown += $"{CMDBasic.SHUTDOWN_RESTART} ";
                        break;
                }
                result += shutdown;
            }        

            return result;
        }

        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            TextBoxDir.Text = FileUtils.GetSaveAsTextFilePath("CHKDSK");
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

        private void JustScan_Click(object sender, RoutedEventArgs e)
        {
            if (ParaF.IsChecked == false) ParaF.IsChecked = true;
            if (ParaR.IsChecked == false) ParaR.IsChecked = true;
            if (ParaX.IsChecked == false) ParaX.IsChecked = true;
            if (ParaScan.IsChecked == false) ParaScan.IsChecked = true;
        }
    }
}
