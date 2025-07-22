using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using GUIForDiskpart.Utils;


namespace GUIForDiskpart.Presentation.View.UserControls
{
    /// <summary>
    /// Interaktionslogik für PhysicalDriveEntryUI.xaml
    /// </summary>
    public partial class UCPhysicalDriveEntry : UserControl
    {
        public delegate void DOnClick(object sender, RoutedEventArgs e);
        public event DOnClick EOnline_Click;
        public event DOnClick EOffline_Click;
        public event DOnClick EDetail_Click;
        public event DOnClick EListPart_Click;
        public event DOnClick EClean_Click;
        public event DOnClick EConvert_Click;
        public event DOnClick ECreatePart_Click;
        public event DOnClick ECreateVolume_Click;
        public event DOnClick EEasyFormat_Click;
        public event DOnClick EButton_Click;
        public event DOnClick EOpenContextMenu_Click;

        public UCPhysicalDriveEntry()
        {
            InitializeComponent();
        }

        private void Online_Click(object sender, RoutedEventArgs e) => EOnline_Click(sender, e);
        private void Offline_Click(object sender, RoutedEventArgs e) => EOffline_Click(sender, e);
        private void Detail_Click(object sender, RoutedEventArgs e) => EDetail_Click(sender, e);
        private void ListPart_Click(object sender, RoutedEventArgs e) => EListPart_Click(sender, e);
        private void Clean_Click(object sender, RoutedEventArgs e) => EClean_Click(sender, e);
        private void Convert_Click(object sender, RoutedEventArgs e) => EConvert_Click(sender, e);
        private void CreatePart_Click(object sender, RoutedEventArgs e) => ECreatePart_Click(sender, e);
        private void CreateVolume_Click(object sender, RoutedEventArgs e) => ECreateVolume_Click(sender, e);
        private void EasyFormat_Click(object sender, RoutedEventArgs e) => EEasyFormat_Click(sender, e);
        private void Button_Click(object sender, RoutedEventArgs e) => EButton_Click(sender, e);
        private void OpenContextMenu_Click(object sender, RoutedEventArgs e) => EOpenContextMenu_Click(sender, e);
    }
}
