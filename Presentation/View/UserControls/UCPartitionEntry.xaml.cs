using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

using GUIForDiskpart.Model.Data;
using GUIForDiskpart.Model.Logic.Diskpart;
using GUIForDiskpart.Presentation.Presenter;
using GUIForDiskpart.Service;
using GUIForDiskpart.Utils;


namespace GUIForDiskpart.Presentation.View.UserControls
{
    /// <summary>
    /// Interaktionslogik für PartitionEntry.xaml
    /// </summary>
    public partial class UCPartitionEntry : UserControl
    {
        public delegate void DOnClick(object sender, RoutedEventArgs e);
        public event DOnClick EOnline;
        public event DOnClick EOffline;
        public event DOnClick EExtend;
        public event DOnClick EShrink;
        public event DOnClick EAnalyzeDefrag;
        public event DOnClick EAttributes;
        public event DOnClick EActive;
        public event DOnClick EInactive;
        public event DOnClick EButton;
        public event DOnClick EDetail;
        public event DOnClick EFormat;
        public event DOnClick EDelete;
        public event DOnClick EAssign;
        public event DOnClick EScanVolume;
        public event DOnClick EOpenContextMenu;

        private const string PARTITIONBORDER = "#FF00C4B4";
        private const string LOGICALBORDER = "#FF0A70C5";

        private const string BASICBACKGROUND = "#FFBBBBBB";
        private const string SELECTBACKGROUND = "#FF308EBF";

        public bool? IsSelected { get { return EntrySelected.IsChecked; } }

        public UCPartitionEntry()
        {
            InitializeComponent();
        }

        public void UpdateUI(PartitionModel partition, string driveName, string fileSystem)
        {
            PartitionNumber.Content = $"#{partition.WSM.PartitionNumber}";
            DriveNameAndLetter.Content = driveName;
            TotalSpace.Content = partition.WSM.FormattedSize;
            FileSystemText.Content = fileSystem;
            PartitionType.Content = $"{partition.WSM.PartitionTable}: {partition.WSM.PartitionType}";
        }

        private void Online_Click(object sender, RoutedEventArgs e) => EOnline?.Invoke(sender, e);
        private void Offline_Click(object sender, RoutedEventArgs e) => EOffline?.Invoke(sender, e);
        private void Extend_Click(object sender, RoutedEventArgs e) => EExtend?.Invoke(sender, e);
        private void Shrink_Click(object sender, RoutedEventArgs e) => EShrink?.Invoke(sender, e);
        private void AnalyzeDefrag_Click(object sender, RoutedEventArgs e) => EAnalyzeDefrag?.Invoke(sender, e);
        private void Attributes_Click(object sender, RoutedEventArgs e) => EAttributes?.Invoke(sender, e);
        private void Active_Click(object sender, RoutedEventArgs e) => EActive?.Invoke(sender, e);
        private void Inactive_Click(object sender, RoutedEventArgs e) => EInactive?.Invoke(sender, e);
        private void Button_Click(object sender, RoutedEventArgs e) => EButton?.Invoke(sender, e);
        private void Detail_Click(object sender, RoutedEventArgs e) => EDetail?.Invoke(sender, e);
        private void Format_Click(object sender, RoutedEventArgs e) => EFormat?.Invoke(sender, e);
        private void Delete_Click(object sender, RoutedEventArgs e) => EDelete?.Invoke(sender, e);
        private void Assign_Click(object sender, RoutedEventArgs e) => EAssign?.Invoke(sender, e);
        private void ScanVolume_Click(object sender, RoutedEventArgs e) => EScanVolume?.Invoke(sender, e);
        private void OpenContextMenu_Click(object sender, RoutedEventArgs e) => EOpenContextMenu?.Invoke(sender, e);
    }
}
