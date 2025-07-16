global using PCHKDSK =
    GUIForDiskpart.Presentation.Presenter.Windows.PCHKDSK<GUIForDiskpart.Presentation.View.Windows.WCHKDSK>;

using System.Windows;

using GUIForDiskpart.Database.Data.CMD;
using GUIForDiskpart.Model.Data;
using GUIForDiskpart.Model.Logic.CMD;
using GUIForDiskpart.Presentation.Presenter.UserControls;
using GUIForDiskpart.Presentation.View.UserControls;
using GUIForDiskpart.Utils;


namespace GUIForDiskpart.Presentation.Presenter.Windows
{
    /// <summary>
    /// Constructed with:
    /// <value><c>PartitionModel</c> Partition</value>
    /// <br/><br/>
    /// Must be instanced with <c>App.Instance.WIM.CreateWPresenter</c> method.<br/>
    /// See code example:
    /// <para>
    /// <code>
    /// App.Instance.WIM.CreateWPresenter&lt;PCHKDSK&gt;(true, Partition);
    /// </code>
    /// </para>
    /// </summary>
    public class PCHKDSK<T> : WPresenter<T> where T : WCHKDSK
    {
        private PLog<UCLog> Log;

        public PartitionModel Partition { get; private set; }

        private string oldKBValue = "0";

        private void WriteTextBoxPara(bool? value, string parameter)
        {
            if (value == true)
            {
                Window.TextBoxPara.Text += $" {parameter} ";
            }
            else
            {
                if (Window.TextBoxPara.Text.Contains(parameter))
                {
                    Window.TextBoxPara.Text = Window.TextBoxPara.Text.Replace($" {parameter} ", "");
                }
            }
        }

        private string GetOptionalParameters()
        {
            string result = string.Empty;

            if (Window.ParaShutdown.IsChecked == true)
            {
                string item = (string)Window.ParaShutdownSelection.SelectionBoxItem;
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

        private void ReplaceKBValueText()
        {
            if (Window.TextBoxPara.Text.Contains(oldKBValue))
            {
                Window.TextBoxPara.Text = Window.TextBoxPara.Text.Replace(oldKBValue, Window.TextBoxL.Text);
            }
            else
            {
                int index = Window.TextBoxPara.Text.IndexOf(CHKDSKParameters.LOGFILESIZE_NTFS);
                if (index == -1) return;
                Window.TextBoxPara.Text = Window.TextBoxPara.Text.Insert(index + 3, Window.TextBoxL.Text);
            }

            if (string.IsNullOrEmpty(Window.TextBoxL.Text)) return;
            oldKBValue = Window.TextBoxL.Text;
        }

        private void OnTextBoxL_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            ReplaceKBValueText();
        }

        #region OnChecked

        private void OnParaF_Checked(object sender, RoutedEventArgs e)
        {
            WriteTextBoxPara((bool)Window.ParaF.IsChecked, CHKDSKParameters.FIXERRORS);
        }

        private void OnParaR_Checked(object sender, RoutedEventArgs e)
        {
            WriteTextBoxPara(Window.ParaR.IsChecked, CHKDSKParameters.LOCATEBAD);
        }

        private void OnParaX_Checked(object sender, RoutedEventArgs e)
        {
            WriteTextBoxPara(Window.ParaX.IsChecked, CHKDSKParameters.FIXERRORSFORCEDISMOUNT);
        }

        private void OnParaV_Checked(object sender, RoutedEventArgs e)
        {
            WriteTextBoxPara(Window.ParaV.IsChecked, CHKDSKParameters.DISPLAYNAMEOFEACHFILE);
        }

        private void OnParaOfflineScanAndFix_Checked(object sender, RoutedEventArgs e)
        {
            WriteTextBoxPara(Window.ParaOfflineScanAndFix.IsChecked, CHKDSKParameters.OFFLINESCANANDFIX);
        }

        private void OnParaI_Checked(object sender, RoutedEventArgs e)
        {
            WriteTextBoxPara(Window.ParaI.IsChecked, CHKDSKParameters.LESSVIGOROUS_NTFS);
        }

        private void OnParaC_Checked(object sender, RoutedEventArgs e)
        {
            WriteTextBoxPara(Window.ParaC.IsChecked, CHKDSKParameters.NOCYCLECHECKNTFS);
        }

        private void OnParaL_Checked(object sender, RoutedEventArgs e)
        {
            if (Window.ParaL.IsChecked == false)
            {
                if (!string.IsNullOrEmpty(Window.TextBoxL.Text) && Window.TextBoxPara.Text.Contains(Window.TextBoxL.Text))
                {
                    Window.TextBoxPara.Text = Window.TextBoxPara.Text.Replace(Window.TextBoxL.Text, "");
                }
            }

            WriteTextBoxPara(Window.ParaL.IsChecked, CHKDSKParameters.LOGFILESIZE_NTFS);

            if (Window.ParaL.IsChecked == true)
            {
                ReplaceKBValueText();
            }
        }

        private void OnParaB_Checked(object sender, RoutedEventArgs e)
        {
            WriteTextBoxPara(Window.ParaB.IsChecked, CHKDSKParameters.CLEARBADSECTORLIST_NTFS);
        }

        private void OnParaScan_Checked(object sender, RoutedEventArgs e)
        {
            WriteTextBoxPara(Window.ParaScan.IsChecked, CHKDSKParameters.SCAN_NTFS);
            Window.ParaForceOff.IsChecked = false;
            WriteTextBoxPara(Window.ParaForceOff.IsChecked, CHKDSKParameters.FORCEOFFLINEFIX_NTFS);
        }

        private void OnParaForceOff_Checked(object sender, RoutedEventArgs e)
        {
            WriteTextBoxPara(Window.ParaForceOff.IsChecked, CHKDSKParameters.FORCEOFFLINEFIX_NTFS);
        }

        private void OnParaPerf_Checked(object sender, RoutedEventArgs e)
        {
            WriteTextBoxPara(Window.ParaPerf.IsChecked, CHKDSKParameters.FULLPERFORMANCE);
        }

        private void OnParaSpotfix_Checked(object sender, RoutedEventArgs e)
        {
            WriteTextBoxPara(Window.ParaSpotfix.IsChecked, CHKDSKParameters.SPOTFIX);
        }

        private void OnParaSD_Checked(object sender, RoutedEventArgs e)
        {
            WriteTextBoxPara(Window.ParaSD.IsChecked, CHKDSKParameters.SDCLEAN_NTFS);
        }

        private void OnParaFree_Checked(object sender, RoutedEventArgs e)
        {
            WriteTextBoxPara(Window.ParaFree.IsChecked, CHKDSKParameters.FREEORPHANED_FATFAMILY);
        }

        private void OnParaMarkClean_Checked(object sender, RoutedEventArgs e)
        {
            WriteTextBoxPara(Window.ParaMarkClean.IsChecked, CHKDSKParameters.MARKCLEAN_FATFAMILY);
        }

        #endregion OnChecked

        #region OnClick

        private void OnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void OnConfirm_Click(object sender, RoutedEventArgs e)
        {
            string output = string.Empty;

            Window.TextBoxPara.Text += GetOptionalParameters();

            if (string.IsNullOrEmpty(Window.TextBoxDir.Text))
            {
                output += CMDFunctions.CHKDSK(Partition.WSM.DriveLetter, Window.TextBoxPara.Text);
            }
            else
            {
                output += CMDFunctions.CHKDSK(Partition.WSM.DriveLetter, Window.TextBoxPara.Text, Window.TextBoxDir.Text);
            }

            MainWindow.Log.Print(output);

            this.Close();
        }

        private void OnBrowse_Click(object sender, RoutedEventArgs e)
        {
            Window.TextBoxDir.Text = FileUtils.GetSaveAsTextFilePath("CHKDSK");
        }

        private void OnJustScan_Click(object sender, RoutedEventArgs e)
        {
            if (Window.ParaF.IsChecked == false) Window.ParaF.IsChecked = true;
            if (Window.ParaR.IsChecked == false) Window.ParaR.IsChecked = true;
            if (Window.ParaX.IsChecked == false) Window.ParaX.IsChecked = true;
            if (Window.ParaScan.IsChecked == false) Window.ParaScan.IsChecked = true;
        }

        #endregion OnClick

        #region WPresenter

        public override void Setup()
        {
            Log.Print(Partition.GetOutputAsString(), true);
            Window.DriveLetterTop.Content = Partition.LDModel.VolumeName + " " + Partition.WSM.DriveLetter + ":\\";
            Window.DriveLetterBottom.Content = Partition.WSM.DriveLetter + ":\\";
        }

        protected override void AddCustomArgs(params object?[] args)
        {
            Partition = (Partition)args[0];
        }

        protected override void RegisterEventsInternal()
        {
            base.RegisterEventsInternal();

            Window.ETextChanged += OnTextBoxL_TextChanged;

            Window.EParaF += OnParaF_Checked;
            Window.EParaR += OnParaR_Checked;
            Window.EParaX += OnParaX_Checked;
            Window.EParaV += OnParaV_Checked;
            Window.EParaOfflineScanAndFix += OnParaOfflineScanAndFix_Checked;
            Window.EParaI += OnParaI_Checked;
            Window.EParaC += OnParaC_Checked;
            Window.EParaL += OnParaL_Checked;
            Window.EParaB += OnParaB_Checked;
            Window.EParaScan += OnParaScan_Checked;
            Window.EParaForceOff += OnParaForceOff_Checked;
            Window.EParaPerf += OnParaPerf_Checked;
            Window.EParaSpotfix += OnParaSpotfix_Checked;
            Window.EParaSD += OnParaSD_Checked;
            Window.EParaFree += OnParaFree_Checked;
            Window.EParaMarkClean += OnParaMarkClean_Checked;

            Window.EConfirm += OnConfirm_Click;
            Window.ECancel += OnCancel_Click;
            Window.EJustScan += OnJustScan_Click;
            Window.EBrowse += OnBrowse_Click;
        }

        public override void InitPresenters()
        {
            Log = CreateUCPresenter<PLog>(Window.Log);
        }

        #endregion WPresenter
    }
}
