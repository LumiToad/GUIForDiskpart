using System.Windows;


namespace GUIForDiskpart.Presentation.View.Windows
{
    /// <summary>
    /// Interaction logic for CHKDSKWindow.xaml
    /// </summary>
    public partial class WCHKDSK : Window
    {
        public delegate void DOnClick(object sender, RoutedEventArgs e);
        public event DOnClick EConfirm;
        public event DOnClick ECancel;
        public event DOnClick EJustScan;
        public event DOnClick EBrowse;

        public delegate void DOnChecked(object sender, RoutedEventArgs e);
        public event DOnChecked EParaF;
        public event DOnChecked EParaR;
        public event DOnChecked EParaX;
        public event DOnChecked EParaV;
        public event DOnChecked EParaOfflineScanAndFix;
        public event DOnChecked EParaI;
        public event DOnChecked EParaC;
        public event DOnChecked EParaL;
        public event DOnChecked EParaB;
        public event DOnChecked EParaScan;
        public event DOnChecked EParaForceOff;
        public event DOnChecked EParaPerf;
        public event DOnChecked EParaSpotfix;
        public event DOnChecked EParaSD;
        public event DOnChecked EParaFree;
        public event DOnChecked EParaMarkClean;

        public delegate void DOnTextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e);
        public event DOnTextChanged ETextChanged;

        public WCHKDSK()
        {
            InitializeComponent();
        }

        private void TextBoxL_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e) => ETextChanged?.Invoke(sender, e);

        #region OnChecked

        private void ParaF_Checked(object sender, RoutedEventArgs e) => EParaF?.Invoke(sender, e);
        private void ParaR_Checked(object sender, RoutedEventArgs e) => EParaR?.Invoke(sender, e);
        private void ParaX_Checked(object sender, RoutedEventArgs e) => EParaX?.Invoke(sender, e);
        private void ParaV_Checked(object sender, RoutedEventArgs e) => EParaV?.Invoke(sender, e);
        private void ParaOfflineScanAndFix_Checked(object sender, RoutedEventArgs e) => EParaOfflineScanAndFix?.Invoke(sender, e);
        private void ParaI_Checked(object sender, RoutedEventArgs e) => EParaI?.Invoke(sender, e);
        private void ParaC_Checked(object sender, RoutedEventArgs e) => EParaC?.Invoke(sender, e);
        private void ParaL_Checked(object sender, RoutedEventArgs e) => EParaL?.Invoke(sender, e);
        private void ParaB_Checked(object sender, RoutedEventArgs e) => EParaB?.Invoke(sender, e);
        private void ParaScan_Checked(object sender, RoutedEventArgs e) => EParaScan?.Invoke(sender, e);
        private void ParaForceOff_Checked(object sender, RoutedEventArgs e) => EParaForceOff?.Invoke(sender, e);
        private void ParaPerf_Checked(object sender, RoutedEventArgs e) => EParaPerf?.Invoke(sender, e);
        private void ParaSpotfix_Checked(object sender, RoutedEventArgs e) => EParaSpotfix?.Invoke(sender, e);
        private void ParaSD_Checked(object sender, RoutedEventArgs e) => EParaSD?.Invoke(sender, e);
        private void ParaFree_Checked(object sender, RoutedEventArgs e) => EParaFree?.Invoke(sender, e);
        private void ParaMarkClean_Checked(object sender, RoutedEventArgs e) => EParaMarkClean?.Invoke(sender, e);

        #endregion OnChecked

        #region OnClick

        private void Confirm_Click(object sender, RoutedEventArgs e) => EConfirm?.Invoke(sender, e);
        private void Cancel_Click(object sender, RoutedEventArgs e) => ECancel?.Invoke(sender, e);
        private void JustScan_Click(object sender, RoutedEventArgs e) => EJustScan?.Invoke(sender, e);
        private void Browse_Click(object sender, RoutedEventArgs e) => EBrowse?.Invoke(sender, e);

        #endregion OnClick
    }
}
