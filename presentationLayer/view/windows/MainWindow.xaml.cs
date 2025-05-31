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



namespace GUIForDiskpart.Presentation.View.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IGUIFDWindow
    {
        public static GUIFDMainWin Instance { get; private set; }
        public Presenter.LogUI Log => GetLogPresenter();

        public MainWindow()
        {
            InitializeComponent();
            Instance = this;
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

        public void RetrieveAndShowDiskData(bool value) => DummyClick();

        public void LogPrint(string text) => DummyClick();
            //MainLog.Print(text);

        private void Window_Closing(object sender, CancelEventArgs e) => DummyClick();
        // StartupWindowClose();

        #region EntriesClick
        public void DiskEntry_Click(PhysicalDiskEntryUI entry) => DummyClick();

        public void PartitionEntry_Click(PartitionEntryUI entry) => DummyClick();

        public void UnallocatedEntry_Click(UnallocatedEntryUI entry) => DummyClick();

        private void ListPart_Click(object sender, RoutedEventArgs e) => DummyClick();

        #endregion EntriesClick

        #region TopBarDiskPartMenu

        private void ListVolume_Click(object sender, RoutedEventArgs e) => DummyClick();

        private void ListDisk_Click(object sender, RoutedEventArgs e) => DummyClick();

        private void ListVDisk_Click(object sender, RoutedEventArgs e) => DummyClick();

        private void CreateVDisk_Click(object sender, RoutedEventArgs e) => DummyClick();

        private void AttachVDisk_Click(object sender, RoutedEventArgs e) => DummyClick();

        private void ChildVDisk_Click(object sender, RoutedEventArgs e) => DummyClick();

        private void CopyVDisk_Click(object sender, RoutedEventArgs e) => DummyClick();

        private void AttributesVolume_Click(object sender, RoutedEventArgs e) => DummyClick();

        #endregion TopBarDiskPartMenu

        #region TopBarCommandsMenu

        private void RetrieveDiskData_Click(object sender, RoutedEventArgs e) => DummyClick();

        private void ScanVolume_Click(object sender, RoutedEventArgs e) => DummyClick();

        #endregion TopBarCommandsMenu

        #region TopBarFileMenu

        private void SaveLog_Click(object sender, RoutedEventArgs e) => DummyClick();

        private void SaveEntryData_Click(object sender, RoutedEventArgs e) => DummyClick();

        private void Quit_Click(object sender, RoutedEventArgs e) => DummyClick();
        //System.Windows.Application.Current.Shutdown();
        
        #endregion TopBarFileMenu

        #region TopBarHelpMenu

        private void Website_Click(object sender, RoutedEventArgs e) => DummyClick();

        private void Wiki_Click(object sender, RoutedEventArgs e) => DummyClick();

        private void About_Click(object sender, RoutedEventArgs e) => DummyClick();

        #endregion TopBarHelpMenu

        private void DummyClick() { }
    }
}
