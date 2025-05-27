global using LDModel = GUIForDiskpart.Model.Data.LogicalDisk;
global using LDService = GUIForDiskpart.Service.LogicalDisk;

global using DiskModel = GUIForDiskpart.Model.Data.Disk;
global using DiskService = GUIForDiskpart.Service.Disk;

global using DAModel = GUIForDiskpart.Model.Data.DefragAnalysis;
global using DAService = GUIForDiskpart.Service.DefragAnalysis;

global using PartitionModel = GUIForDiskpart.Model.Data.Partition;
global using PartitionService = GUIForDiskpart.Service.Partition;


using System;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using GUIForDiskpart.Windows;
using GUIForDiskpart.Database;
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
                Service.Disk.SetupDiskChangedWatcher();
                Service.Disk.OnDiskChanged += OnDiskChanged;
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
            build += " - " + Database.Data.AppInfo.BUILD_STAGE;

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
            Service.Disk.ReloadDiskInformation();

            //Todo -> View!
            AddEntrysToStackPanel<DiskInfo>(DiskStackPanel, Database.Retrievers.Disk.PhysicalDrives);

            if (outputText)
            {
                AddTextToOutputConsole(Database.Retrievers.Disk.GetDiskOutput());
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
