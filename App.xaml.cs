using System;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

using GUIForDiskpart.Utils;
using GUIForDiskpart.Model.Logic;
using GUIForDiskpart.Presentation.Presenter;


namespace GUIForDiskpart
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /* --- Inherited from Application ---
        public Window MainWindow { get; set; } 
           ---------------------------------- */

        public static App Instance { get; private set; }
        public WindowInstanceManager WIM { get; private set; } = new();

        private StartupLoadingWindow? startup;

        // Entry point of the whole application!
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Instance = this;
            
            Initialize();
        }

        private void Initialize()
        {
            WIM.ShowStartupScreen();

            try
            {
                RetrieveAndShowDiskData(true);
                DiskService.SetupDiskChangedWatcher();
            }
            catch (Exception ex)
            {
                FileUtils.SaveExceptionCrashLog(ex);
            }

            WIM.CreateWPresenter<PMainWindow<GUIFDMainWin>>();
        }

        public void OnMainWindowLoaded()
        {
            var pMainWin = WIM.GetPresenter<PMainWindow<GUIFDMainWin>>();
            DiskService.OnDiskChanged += pMainWin.OnDiskChanged;
            pMainWin.DisplayDiskData(true);

            WIM.StartupWindowClose();
        }

        // Retriever
        #region RetrieveDisk
        //private void OnDiskChanged()
        //{
        //    RetrieveAndShowDiskData(false);
        //}

        // Retriever
        public void RetrieveAndShowDiskData(bool outputText)
        {
            RetrieveAndShowDiskData_Internal(outputText);
        }

        // Retriever
        private void RetrieveAndShowDiskData_Internal(bool outputText)
        {
            DiskService.ReLoadDisks();

            //WIM.GetPresenter<MainWindow<GUIFDMainWin>>().RetrieveAndShowDiskData(outputText);

            //Todo -> View!
            //AddEntrysToStackPanel<DiskModel>(DiskStackPanel, DiskService.PhysicalDrives);

            //if (outputText)
            //{
            //    AddTextToOutputConsole(DiskService.GetDiskOutput());
            //}

            //Todo -> View!
            //DiskEntry_Click((PhysicalDiskEntryUI)DiskStackPanel.Children[0]);
        }

        #endregion RetrieveDisk
    }
}
