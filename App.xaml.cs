using System;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

using GUIForDiskpart.Windows;
using GUIForDiskpart.Database.Data;
using GUIForDiskpart.Utils;

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
        private StartupLoadingWindow? startup;


        // Entry point of the whole application!
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ShowStartupScreen();
            Initialize();

            if (startup != null)
            {
                StartupWindowClose();
                MainWindow.Focus();
            }
        }

        private void Initialize()
        {
            try
            {
                RetrieveAndShowDiskData(true);
                service.Disk.SetupDiskChangedWatcher();
                Disk.OnDiskChanged += OnDiskChanged;
            }
            catch (Exception ex)
            {
                FileUtils.SaveExceptionCrashLog(ex);
            }
        }

        private string GetBuildNumberString()
        {
            string build = "";

            build += Assembly.GetExecutingAssembly().GetName().Version.ToString();
            build += " - " + AppInfo.BUILD_STAGE;

            return build;
        }

        public void AddTextToOutputConsole(string text)
        {
            //Todo -> View!
            MainWindow.ConsoleReturn.AddTextToOutputConsole(text);
        }

        #region RetrieveDisk
        private void OnDiskChanged()
        {
            RetrieveAndShowDiskData(false);
        }

        public void RetrieveAndShowDiskData(bool outputText)
        {
            Application.Current.Dispatcher.Invoke(RetrieveAndShowDiskData_Internal, outputText);
        }

        private void RetrieveAndShowDiskData_Internal(bool outputText)
        {
            DiskRetriever.ReloadDiskInformation();

            //Todo -> View!
            AddEntrysToStackPanel<DiskInfo>(DiskStackPanel, DiskRetriever.PhysicalDrives);

            if (outputText)
            {
                AddTextToOutputConsole(DiskRetriever.GetDiskOutput());
            }

            //Todo -> View!
            DiskEntry_Click((PhysicalDiskEntryUI)DiskStackPanel.Children[0]);
        }

        #endregion RetrieveDisk

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
            }
            else
            {
                startup.Dispatcher.Invoke(DispatcherPriority.Normal, new ThreadStart(startup.Close));
            }
        }

        public void StartupWindowThreadEntryPoint()
        {
            startup = new StartupLoadingWindow();
            startup.Show();
            Dispatcher.Run();
        }

        #endregion StartupWindow
    }
}
