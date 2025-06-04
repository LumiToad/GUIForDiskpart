using GUIForDiskpart.Model.Logic.Diskpart;
using GUIForDiskpart.Presentation.Presenter;
using GUIForDiskpart.Utils;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GUIForDiskpart.Presentation.View.UserControls
{
    /// <summary>
    /// Interaktionslogik für PhysicalDriveEntryUI.xaml
    /// </summary>
    public partial class UCPhysicalDrive : UserControl
    {
        public delegate void DOnClick(object sender, RoutedEventArgs e);
        public event DOnClick EOnOffline_Click;
        public event DOnClick EDetail_Click;
        public event DOnClick EListPart_Click;
        public event DOnClick EClean_Click;
        public event DOnClick EConvert_Click;
        public event DOnClick ECreatePart_Click;
        public event DOnClick ECreateVolume_Click;
        public event DOnClick EEasyFormat_Click;
        public event DOnClick EButton_Click;
        public event DOnClick EOpenContextMenu_Click;


        PMainWindow<GUIFDMainWin> MainWindow = App.Instance.WIM[typeof(PMainWindow<GUIFDMainWin>)];
        public bool? IsSelected { get { return EntrySelected.IsChecked; } }

        public UCPhysicalDrive()
        {
            InitializeComponent();
        }

        public void OnOffline_Click(object sender, RoutedEventArgs e) => EOnOffline_Click(sender, e);
        private void Detail_Click(object sender, RoutedEventArgs e) => EDetail_Click(sender, e);
        private void ListPart_Click(object sender, RoutedEventArgs e) => EListPart_Click(sender, e);
        private void Clean_Click(object sender, RoutedEventArgs e) => EClean_Click(sender, e);
        private void Convert_Click(object sender, RoutedEventArgs e) => EConvert_Click(sender, e);
        private void CreatePart_Click(object sender, RoutedEventArgs e) => ECreatePart_Click(sender, e);
        private void CreateVolume_Click(object sender, RoutedEventArgs e) => ECreateVolume_Click(sender, e);
        private void EasyFormat_Click(object sender, RoutedEventArgs e) => EEasyFormat_Click(sender, e);
        private void Button_Click(object sender, RoutedEventArgs e) => EButton_Click(sender, e);
        private void OpenContextMenu_Click(object sender, RoutedEventArgs e) => EOpenContextMenu_Click(sender, e);
        
        public void SelectEntryRadioButton() => EntrySelected.IsChecked = true;

        public void SetValueInProgressBar(ulong totalSize, ulong usedSpace)
        {
            SizeBar.Maximum = totalSize;
            SizeBar.Minimum = 0;
            SizeBar.Value = usedSpace;
        }
    }
}
