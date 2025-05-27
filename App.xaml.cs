using GUIForDiskpart.main;
using GUIForDiskpart.userControls;
using GUIForDiskpart.windows;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace GUIForDiskpart
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private const string WEBSITE_URL = "https://github.com/LumiToad/GUIForDiskpart";
        private const string WIKI_URL = "https://github.com/LumiToad/GUIForDiskpart/wiki";
        private const string BUILD_STAGE = "Beta";

        private StartupLoadingWindow startup;

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
                DiskRetriever.SetupDiskChangedWatcher();
                DiskRetriever.OnDiskChanged += OnDiskChanged;
            }
            catch (Exception ex)
            {
                FileUtilites.SaveExceptionCrashLog(ex);
            }
        }

        private string GetBuildNumberString()
        {
            string build = "";

            build += Assembly.GetExecutingAssembly().GetName().Version.ToString();
            build += " - " + BUILD_STAGE;

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
