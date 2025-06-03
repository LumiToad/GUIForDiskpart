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
            ShowStartupScreen();
        }

        private void Initialize()
        {
            try
            {
                RetrieveAndShowDiskData(true);
                DiskService.SetupDiskChangedWatcher();
            }
            catch (Exception ex)
            {
                FileUtils.SaveExceptionCrashLog(ex);
            }
        }

        public void OnMainWindowLoaded()
        {
            //DiskService.OnDiskChanged += (GUIFDMainWin.Instance.GetWindowPresenter() as GUIForDiskpart.Presentation.Presenter.MainWindow).OnDiskChanged;

            WIM.GetPresenter<MainWindow<GUIFDMainWin>>().Log.Print("Success!");
            Current.Dispatcher.Invoke(RetrieveAndShowDiskData_Internal, true);
            StartupWindowClose();
            
        }

        // View
        public void AddTextToOutputConsole(string text)
        {

// GUIFDMainWin.Instance.Log.Print(text);
        }

        // Retriever
        #region RetrieveDisk
        private void OnDiskChanged()
        {
            RetrieveAndShowDiskData(false);
        }

        // Retriever
        public void RetrieveAndShowDiskData(bool outputText)
        {
            
        }

        // Retriever
        private void RetrieveAndShowDiskData_Internal(bool outputText)
        {
            DiskService.ReLoadDisks();

            WIM.GetPresenter<MainWindow<GUIFDMainWin>>().RetrieveAndShowDiskData(outputText);

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

        // Fürs Erste Main
        #region StartupWindow

        private void ShowStartupScreen()
        {
            Thread startupWindowThread = new Thread(new ThreadStart(StartupWindowThreadEntryPoint));
            startupWindowThread.SetApartmentState(ApartmentState.STA);
            startupWindowThread.IsBackground = true;
            startupWindowThread.Start();
        }

        private void StartupWindowClose()
        {
            if (startup.Dispatcher.CheckAccess())
            {
                startup.Close();
                MainWindow.Focus();
            }
            else
            {
                startup.Dispatcher.Invoke(DispatcherPriority.Normal, new ThreadStart(startup.Close));
                MainWindow.Focus();
            }
        }

        public void StartupWindowThreadEntryPoint()
        {
            startup = new StartupLoadingWindow();
            startup.Show();
            startup.Focus();
            Dispatcher.Run();
        }

        #endregion StartupWindow
    }
}
