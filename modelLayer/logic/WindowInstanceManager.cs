using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;


namespace GUIForDiskpart.Model.Logic
{
    public class WindowInstanceManager
    {
        private Dictionary<System.Type, IGUIFDWindow> managedWindows = new();

        public dynamic this[System.Type type]
        {
            get
            {
                MethodInfo method = typeof(WindowInstanceManager).GetMethod("GetWindow");
                method = method.MakeGenericMethod(type);

                return method.Invoke(this, null);
            }
        }

        public Window? CreateWindow(IGUIFDWindow guifdWindow)
        {
            Window window = (Window)guifdWindow;
            if (window == null) return null;

            window = new();

            SetupGUIFDWindow(guifdWindow, window);

            return window;
        }

        public void RegisterGUIFDMainWin()
        {
            if (managedWindows.ContainsKey(GUIFDMainWin.Instance.GetType())) return;
            SetupGUIFDWindow(GUIFDMainWin.Instance, GUIFDMainWin.Instance);
            OnWindowContentRendered(GUIFDMainWin.Instance, null);
        }

        private void SetupGUIFDWindow(IGUIFDWindow guifdWindow, Window window)
        {
            window.Show();
            window.Focus();

            window.ContentRendered += OnWindowContentRendered;
            managedWindows.Add(guifdWindow.GetType(), guifdWindow);
        }

        private void OnWindowContentRendered(object? sender, EventArgs e)
        {
            IGUIFDWindow window = (IGUIFDWindow)sender;
            window.InitializeGUIFDWindow();

            Window w = (Window)sender;
            w.ContentRendered -= OnWindowContentRendered;
        }

        public T GetWindow<T>()
        {
            Type type = typeof(T);
            return (T)managedWindows[type];
        }

        private void ShowStartupScreen()
        {
            /*
            Thread startupWindowThread = new Thread(new ThreadStart(StartupWindowThreadEntryPoint));
            startupWindowThread.SetApartmentState(ApartmentState.STA);
            startupWindowThread.IsBackground = true;
            startupWindowThread.Start();
            */
        }
        /*
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
        */
    }
}
