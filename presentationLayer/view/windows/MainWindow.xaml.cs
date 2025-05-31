using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

using GUIForDiskpart.Presentation.View.Windows;
using GUIForDiskpart.Presentation.View.UserControls;
using GUIForDiskpart.Utils;
using GUIForDiskpart.Presentation.Presenter;
using System.Management.Automation.Remoting;



namespace GUIForDiskpart.Presentation.View.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IGUIFDWindow
    {
        public static GUIFDMainWin Instance { get; private set; }
        public Presenter.LogUI Log => GetLogPresenter();

        private Presenter.MainWindow windowPresenter;

        public MainWindow()
        {
            InitializeComponent();
            Instance = this;
            windowPresenter = new(this);
        }

        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);
            App.Instance.WIM.RegisterGUIFDMainWin();
            App.Instance.OnMainWindowLoaded();
        }

        private Presenter.LogUI GetLogPresenter()
        {
            return (Presenter.LogUI)this.AsGUIFDWindow().ChildPresenters[MainLog][0];
        }

        // JUST FOR TESTING!!!
        public void RetrieveAndShowDiskData(bool value)
        {
            Presenter.MainWindow presenter = (Presenter.MainWindow)GetWindowPresenter();
            presenter.RetrieveAndShowDiskData(value);
        }

        public void LogPrint(string text) => Log.Print(text);

        #region EntriesClick

        public void DiskEntry_Click(PhysicalDiskEntryUI entry) => windowPresenter.OnDiskEntry_Click(entry);
        public void PartitionEntry_Click(PartitionEntryUI entry) => windowPresenter.OnPartitionEntry_Click(entry);
        public void UnallocatedEntry_Click(UnallocatedEntryUI entry) => windowPresenter.OnUnallocatedEntry_Click(entry);

        public void ListPart_Click(object sender, RoutedEventArgs e) => windowPresenter.OnListPart_Click(sender, e);

        #endregion EntriesClick

        #region TopBarDiskPartMenu

        public void ListVolume_Click(object sender, RoutedEventArgs e) => windowPresenter.OnListVolume_Click(sender, e);
        public void ListDisk_Click(object sender, RoutedEventArgs e) => windowPresenter.OnListDisk_Click(sender, e);
        public void ListVDisk_Click(object sender, RoutedEventArgs e) => windowPresenter.OnListVDisk_Click(sender, e);
        public void CreateVDisk_Click(object sender, RoutedEventArgs e) => windowPresenter.OnCreateVDisk_Click(sender, e);
        public void AttachVDisk_Click(object sender, RoutedEventArgs e) => windowPresenter.OnAttachVDisk_Click(sender, e);
        public void ChildVDisk_Click(object sender, RoutedEventArgs e) => windowPresenter.OnChildVDisk_Click(sender, e);
        public void CopyVDisk_Click(object sender, RoutedEventArgs e) => windowPresenter.OnCopyVDisk_Click(sender, e);
        public void AttributesVolume_Click(object sender, RoutedEventArgs e) => windowPresenter.OnAttributesVolume_Click(sender, e);

        #endregion TopBarDiskPartMenu

        #region TopBarCommandsMenu

        public void RetrieveDiskData_Click(object sender, RoutedEventArgs e) => windowPresenter.OnRetrieveDiskData_Click(sender, e);
        public void ScanVolume_Click(object sender, RoutedEventArgs e) => windowPresenter.OnScanVolume_Click(sender, e);

        #endregion TopBarCommandsMenu

        #region TopBarFileMenu

        public void SaveLog_Click(object sender, RoutedEventArgs e) => DummyClick(); //windowPresenter.OnSaveLog_Click(sender, e);
        public void SaveEntryData_Click(object sender, RoutedEventArgs e) => windowPresenter.OnSaveEntryData_Click(sender, e);
        public void Quit_Click(object sender, RoutedEventArgs e) => windowPresenter.OnQuit_Click(sender, e);

        //System.Windows.Application.Current.Shutdown();

        #endregion TopBarFileMenu

        #region TopBarHelpMenu

        public void Website_Click(object sender, RoutedEventArgs e) => windowPresenter.OnWebsite_Click(sender, e);
        public void Wiki_Click(object sender, RoutedEventArgs e) => windowPresenter.OnWiki_Click(sender, e);
        public void About_Click(object sender, RoutedEventArgs e) => windowPresenter.OnAbout_Click(sender, e);

        #endregion TopBarHelpMenu

        private void DummyClick() { }

        public IPresenter GetWindowPresenter()
        {
            return windowPresenter;
        }
    }
}
