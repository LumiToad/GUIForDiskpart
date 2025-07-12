using GUIForDiskpart.Model.Logic.Diskpart;
using GUIForDiskpart.Presentation.Presenter;
using GUIForDiskpart.Utils;
using System.Collections.Generic;
using System.Management.Automation;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ScrollBar;

namespace GUIForDiskpart.Presentation.View.UserControls
{
    /// <summary>
    /// Interaktionslogik für PhysicalDriveEntryUI.xaml
    /// </summary>
    public partial class UCPhysicalDriveEntry : UserControl
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

        PMainWindow MainWindow = App.Instance.WIM[typeof(PMainWindow)];

        public UCPhysicalDriveEntry()
        {
            InitializeComponent();
        }

        public void SelectEntryRadioButton() => EntrySelected.IsChecked = true;

        public void SetValueInProgressBar(ulong totalSize, ulong usedSpace)
        {
            SizeBar.Maximum = totalSize;
            SizeBar.Minimum = 0;
            SizeBar.Value = usedSpace;
        }

        public void UpdateUI(DiskModel diskModel)
        {
            DiskIndex.Content = $"#{diskModel.DiskIndex}";
            DiskModelText.Content = diskModel.DiskModelText;
            TotalSpace.Content = diskModel.FormattedTotalSpace;
            WSMPartitionCount.Content = $"{diskModel.PartitionCount} partitions";
            SetValueInProgressBar(diskModel.TotalSpace, diskModel.UsedSpace);

            DiskIcon.Source = GetDiskIcon(diskModel);
            MediaTypeIcon.Source = GetMediaTypeIcon(diskModel);
        }

        private ImageSource? GetDiskIcon(DiskModel diskModel)
        {
            ImageSource? result = IconUtils.GetShellIconByType(Shell32IconType.Drive, true);

            if (diskModel.InterfaceType == "USB")
            {
                result = IconUtils.GetShellIconByType(Shell32IconType.USB, true);
            }

            return result;
        }

        private ImageSource? GetMediaTypeIcon(DiskModel diskModel)
        {
            ImageSource? result = IconUtils.GetShellIconByType(Shell32IconType.QuestionMark, true);

            switch (diskModel.MediaType)
            {
                case ("External hard disk media"):
                    result = IconUtils.GetShellIconByType(Shell32IconType.UpArrow, true);
                    break;
                case ("Removable Media"):
                    result = IconUtils.GetShellIconByType(Shell32IconType.UpArrow, true);
                    break;
                case ("Fixed hard disk media"):
                    result = IconUtils.GetShellIconByType(Shell32IconType.Fixed, true);
                    break;
                case ("Unknown"):
                    result = IconUtils.GetShellIconByType(Shell32IconType.QuestionMark, true);
                    break;
            }

            return result;
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
    }
}
