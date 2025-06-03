using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

using GUIForDiskpart.Presentation.Presenter;


namespace GUIForDiskpart.Model.Logic
{
    public class WindowInstanceManager
    {
        private StartupLoadingWindow? startup;

        private Dictionary<Type, object> presenters = new();

        public dynamic this[Type type]
        {
            get
            {
                MethodInfo method = typeof(WindowInstanceManager).GetMethod("GetPresenter");
                method = method.MakeGenericMethod(type);

                return method.Invoke(this, null);
            }
        }

        public void CreateWindow(WPresenter<Window> wPresenter)
        {
            wPresenter.Window = new();
            SetupGUIFDWindow(wPresenter);
        }

        public void RegisterGUIFDMainWin(GUIFDMainWin mainWin)
        {
            if (presenters.ContainsKey(typeof(MainWindow<GUIFDMainWin>))) return;
            MainWindow<GUIFDMainWin> MainWinPresenter = new MainWindow<GUIFDMainWin>(mainWin, mainWin.MainLog);
            SetupGUIFDWindow(MainWinPresenter, false);
            OnWindowContentRendered(mainWin, null);
        }

        private void SetupGUIFDWindow<T>(WPresenter<T> wPresenter, bool isFocus = true) where T : Window
        {
            wPresenter.Window.Show();
            if (isFocus) 
            {
                wPresenter.Window.Focus();        
            }
            wPresenter.Window.ContentRendered += OnWindowContentRendered;

            wPresenter.RegisterEvents();
            presenters.Add(wPresenter.GetType(), wPresenter);
        }

        private void OnWindowContentRendered(object? sender, EventArgs e)
        {
            Window w = (Window)sender;
            w.ContentRendered -= OnWindowContentRendered;
        }

        public T GetPresenter<T>()
        {
            return (T)presenters[typeof(T)];
        }

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
    }
}
