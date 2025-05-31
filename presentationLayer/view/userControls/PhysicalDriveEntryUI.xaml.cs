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
    public partial class PhysicalDiskEntryUI : UserControl, IGUIFDUserControl
    {
        private DiskModel diskModel;
        public DiskModel DiskModel
        { 
            get { return diskModel; } 
            set 
            { 
                diskModel = value; 
                DriveDataToThisUI();
                PopulateContextMenu();
            }
        }

        public bool? IsSelected { get { return EntrySelected.IsChecked; } }

        public PhysicalDiskEntryUI(DiskModel diskModel)
        {
            InitializeComponent();
            DiskModel = diskModel;
        }

        private void DriveDataToThisUI()
        {
            DiskIndex.Content = $"#{DiskModel.DiskIndex}";
            DiskModelText.Content = diskModel.DiskModelText;
            TotalSpace.Content = DiskModel.FormattedTotalSpace;
            WSMPartitionCount.Content = $"{DiskModel.PartitionCount} partitions";
            SetValueInProgressBar(DiskModel.TotalSpace, DiskModel.UsedSpace);

            DiskIcon.Source = GetDiskIcon();
            MediaTypeIcon.Source = GetMediaTypeIcon();
        }

        private void PopulateContextMenu()
        {
            string header = string.Empty;
            string name = string.Empty;

            header = "DISKPART - Online";
            name = "DPOnline";

            if (DiskModel.IsOnline) 
            {
                header = "DISKPART - Offline";
                name = "DPOffline";
            }
            MenuItem menuItem = WPFUtils.CreateContextMenuItem(IconUtils.Diskpart, name, header, true, OnOffline_Click);
            ContextMenu.Items.Add(menuItem);
        }

        private void OnOffline_Click(object sender, RoutedEventArgs e)
        {
            string output = string.Empty;

            if (DiskModel.IsOnline) 
            {
                output += DPFunctions.OnOfflineDisk(DiskModel.DiskIndex, false, false);
            }
            else 
            {
                output += DPFunctions.OnOfflineDisk(DiskModel.DiskIndex, true, false);
            }

            GUIFDMainWin.Instance.LogPrint(output);
            GUIFDMainWin.Instance.RetrieveAndShowDiskData(false);
        }

        private void Detail_Click(object sender, RoutedEventArgs e)
        {
            GUIFDMainWin.Instance.LogPrint(DPFunctions.DetailDisk(DiskModel.DiskIndex));
        }

        private void ListPart_Click(object sender, RoutedEventArgs e)
        {
            GUIFDMainWin.Instance.LogPrint(DPFunctions.ListPart(DiskModel.DiskIndex));
        }

        private void Clean_Click(object sender, RoutedEventArgs e)
        {
            CleanWindow cleanWindow = new CleanWindow(diskModel);
            cleanWindow.Owner = GUIFDMainWin.Instance;
            cleanWindow.Focus();

            cleanWindow.Show();
        }

        private void Convert_Click(object sender, RoutedEventArgs e)
        {
            ConvertDriveWindow convertDriveWindow = new ConvertDriveWindow(diskModel);
            convertDriveWindow.Owner = GUIFDMainWin.Instance;
            convertDriveWindow.Focus();
            
            convertDriveWindow.Show();
        }

        private void CreatePart_Click(object sender, RoutedEventArgs e)
        {
            CreatePartitionWindow createPartitionWindow = new CreatePartitionWindow(diskModel);
            createPartitionWindow.Owner = GUIFDMainWin.Instance;
            createPartitionWindow.Focus();

            createPartitionWindow.Show();
        }

        private void CreateVolume_Click(object sender, RoutedEventArgs e)
        {
            CreateVolumeWindow createVolumeWindow = new CreateVolumeWindow(diskModel);
            createVolumeWindow.Owner = GUIFDMainWin.Instance;
            createVolumeWindow.Focus();

            createVolumeWindow.Show();
        }

        private void EasyFormat_Click(object sender, RoutedEventArgs e)
        {
            FormatDriveWindow formatWindow = new FormatDriveWindow(diskModel);
            formatWindow.Owner = GUIFDMainWin.Instance;
            formatWindow.Focus();

            formatWindow.Show();
        }

        // Presenter
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SelectEntryRadioButton();
            (GUIFDMainWin.Instance.GetWindowPresenter() as Presenter.MainWindow).OnDiskEntry_Click(this);
        }

        public void SelectEntryRadioButton()
        {
            EntrySelected.IsChecked = true;
        }

        private ImageSource? GetDiskIcon()
        {
            ImageSource? result = IconUtils.GetShellIconByType(Shell32IconType.Drive, true);

            if (DiskModel.InterfaceType == "USB")
            {
                result = IconUtils.GetShellIconByType(Shell32IconType.USB, true);
            }

            return result;
        }

        private ImageSource? GetMediaTypeIcon()
        {
            ImageSource? result = IconUtils.GetShellIconByType(Shell32IconType.QuestionMark, true);

            switch (DiskModel.MediaType)
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

        private void OpenContextMenu_Click(object sender, RoutedEventArgs e)
        {
            ContextMenu.IsOpen = !ContextMenu.IsOpen;
        }

        private void SetValueInProgressBar(ulong totalSize, ulong usedSpace)
        {
            SizeBar.Maximum = totalSize;
            SizeBar.Minimum = 0;
            SizeBar.Value = usedSpace;
        }

        #region IGUIFDUserControl

        List<IPresenter> IGUIFDUserControl.GetPresenters()
        {
            List<IPresenter> presenters = new();
            return presenters;
        }

        #endregion IGUIFDUserControl
    }
}
