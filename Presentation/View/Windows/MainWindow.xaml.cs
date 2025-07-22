using System;
using System.Windows;

using GUIForDiskpart.Presentation.View.UserControls;


namespace GUIForDiskpart.Presentation.View.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public delegate void DOnClick(object sender, RoutedEventArgs e);
        public event DOnClick EListPart_Click;
        public event DOnClick EListVolume_Click;
        public event DOnClick EListDisk_Click;
        public event DOnClick EListVDisk_Click;
        public event DOnClick ECreateVDisk_Click;
        public event DOnClick EAttachVDisk_Click;
        public event DOnClick EChildVDisk_Click;
        public event DOnClick ECopyVDisk_Click;
        public event DOnClick EAttributesVolume_Click;
        public event DOnClick ERetrieveDiskData_Click;
        public event DOnClick EScanVolume_Click;
        public event DOnClick ESaveLog_Click;
        public event DOnClick ESaveEntryData_Click;
        public event DOnClick EQuit_Click;
        public event DOnClick EWebsite_Click;
        public event DOnClick EWiki_Click;
        public event DOnClick EAbout_Click;

        public delegate void DDiskEntryOnClick(UCPhysicalDriveEntry entry);
        public event DDiskEntryOnClick EDriveEntry_Click;

        public delegate void DPartitionEntryOnClick(UCPartitionEntry entry);
        public event DPartitionEntryOnClick EPartitionEntry_Click;

        public delegate void DUnallocatedEntryOnClick(UCUnallocatedEntry entry);
        public event DUnallocatedEntryOnClick EUnallocatedEntry_Click;

        public delegate void DContentRendered(EventArgs e);
        public event DContentRendered ERendered;


        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);
            App.Instance.OnMainWindowLoaded();
            ERendered?.Invoke(e);
        }

        #region EntriesClick

        public void DiskEntry_Click(UCPhysicalDriveEntry entry) => EDriveEntry_Click?.Invoke(entry);
        public void PartitionEntry_Click(UCPartitionEntry entry) => EPartitionEntry_Click?.Invoke(entry);
        public void UnallocatedEntry_Click(UCUnallocatedEntry entry) => EUnallocatedEntry_Click?.Invoke(entry);
        public void ListPart_Click(object sender, RoutedEventArgs e) => EListPart_Click?.Invoke(sender, e);

        #endregion EntriesClick

        #region TopBarDiskPartMenu
        
        public void ListVolume_Click(object sender, RoutedEventArgs e) => EListVolume_Click?.Invoke(sender, e);
        public void ListDisk_Click(object sender, RoutedEventArgs e) => EListDisk_Click?.Invoke(sender, e);
        public void ListVDisk_Click(object sender, RoutedEventArgs e) => EListVDisk_Click?.Invoke(sender, e);
        public void CreateVDisk_Click(object sender, RoutedEventArgs e) => ECreateVDisk_Click?.Invoke(sender, e);
        public void AttachVDisk_Click(object sender, RoutedEventArgs e) => EAttachVDisk_Click?.Invoke(sender, e);
        public void ChildVDisk_Click(object sender, RoutedEventArgs e) => EChildVDisk_Click?.Invoke(sender, e);
        public void CopyVDisk_Click(object sender, RoutedEventArgs e) => ECopyVDisk_Click?.Invoke(sender, e);
        public void AttributesVolume_Click(object sender, RoutedEventArgs e) => EAttributesVolume_Click?.Invoke(sender, e);

        #endregion TopBarDiskPartMenu

        #region TopBarCommandsMenu

        public void RetrieveDiskData_Click(object sender, RoutedEventArgs e) => ERetrieveDiskData_Click?.Invoke(sender, e);
        public void ScanVolume_Click(object sender, RoutedEventArgs e) => EScanVolume_Click?.Invoke(sender, e);

        #endregion TopBarCommandsMenu

        #region TopBarFileMenu

        public void SaveLog_Click(object sender, RoutedEventArgs e) => ESaveLog_Click?.Invoke(sender, e);
        public void SaveEntryData_Click(object sender, RoutedEventArgs e) => ESaveEntryData_Click?.Invoke(sender, e);
        public void Quit_Click(object sender, RoutedEventArgs e) => EQuit_Click?.Invoke(sender, e);

        #endregion TopBarFileMenu

        #region TopBarHelpMenu

        public void Website_Click(object sender, RoutedEventArgs e) => EWebsite_Click?.Invoke(sender, e);
        public void Wiki_Click(object sender, RoutedEventArgs e) => EWiki_Click?.Invoke(sender, e);
        public void About_Click(object sender, RoutedEventArgs e) => EAbout_Click?.Invoke(sender, e);

        #endregion TopBarHelpMenu
    }
}
