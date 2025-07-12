using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

using GUIForDiskpart.Presentation.Presenter;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;


namespace GUIForDiskpart.Model.Logic
{
    public class WindowInstanceManager
    {
        private WStartupLoading? startup;

        private Dictionary<Type, object> presenters = new();

        public bool HasGUIFDMainWinLoaded => presenters.ContainsKey(typeof(PMainWindow));

        public dynamic this[Type type]
        {
            get
            {
                MethodInfo method = typeof(WindowInstanceManager).GetMethod("GetPresenter");
                method = method.MakeGenericMethod(type);

                return method.Invoke(this, null);
            }
        }

        public PresenterClass CreateWPresenter<PresenterClass>(bool isFocus = true, params object?[] args) where PresenterClass : new()
        {
            var method = GetType().GetMethod("CreateWPresenterInternal", BindingFlags.NonPublic | BindingFlags.Instance);
            Type wType = typeof(PresenterClass).GenericTypeArguments[0];

            method = method.MakeGenericMethod(typeof(PresenterClass), wType);
            
            var retVal = method.Invoke(this, new object?[2] { isFocus, args });
            return (PresenterClass)retVal;
        }

        protected PType CreateWPresenterInternal<PType, WType>(bool isFocus, object?[] packedArgs)
            where PType : WPresenter<WType>, new()
            where WType : Window, new()
        {
            PType wPresenter = WPresenter<WType>.New<PType>((packedArgs.Length > 0) ? packedArgs[0] : null);
            SetupWindow<WType>(wPresenter, isFocus);

            return wPresenter;
        }

        private void SetupWindow<T>(WPresenter<T> wPresenter, bool isFocus = true) where T : Window, new()
        {
            wPresenter.Window = new();
            wPresenter.InitPresenters();
            wPresenter.Window.Show();
            if (wPresenter.Window != App.Instance.MainWindow)
            {
                wPresenter.Window.Owner = App.Instance.MainWindow;
            }
            if (isFocus) 
            {
                wPresenter.Window.Focus();        
            }
            wPresenter.Window.ContentRendered += OnWindowContentRendered;
            wPresenter.Window.Closing += wPresenter.Close;

            wPresenter.RegisterEvents();
            wPresenter.Setup();
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

        public void ShowStartupScreen()
        {
            Thread startupWindowThread = new Thread(new ThreadStart(StartupWindowThreadEntryPoint));
            startupWindowThread.SetApartmentState(ApartmentState.STA);
            startupWindowThread.IsBackground = true;
            startupWindowThread.Start();
        }
        
        public void StartupWindowClose()
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
            startup = new WStartupLoading();
            startup.Show();
            Dispatcher.Run();
        }

        public void RemovePresenter<T>(WPresenter<T> wPresenter) where T : Window
        {
            presenters.Remove(wPresenter.GetType());
        }
    }
}
