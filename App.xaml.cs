using System;
using System.Windows;

using GUIForDiskpart.Utils;
using GUIForDiskpart.Model.Logic;


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

        //private WStartupLoading? startup;

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
                DiskService.ReLoadDisks();
                DiskService.SetupDiskChangedWatcher();
            }
            catch (Exception ex)
            {
                FileUtils.SaveExceptionCrashLog(ex);
            }

            WIM.CreateWPresenter<PMainWindow>();
        }

        public void OnMainWindowLoaded()
        {
            WIM.StartupWindowClose();
        }
    }
}
