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

        public PresenterClass CreateWPresenter<PresenterClass>(params object?[] args) where PresenterClass : new()
        {
            var method = GetType().GetMethod("CreateWPresenterInternal", BindingFlags.NonPublic | BindingFlags.Instance);
            Type wType = typeof(PresenterClass).GenericTypeArguments[0];

            method = method.MakeGenericMethod(typeof(PresenterClass), wType);
            
            var retVal = method.Invoke(this, new object?[] { args });
            return (PresenterClass)retVal;
        }

        protected PType CreateWPresenterInternal<PType, WType>(object?[] packedArgs)
            where PType : WPresenter<WType>, new()
            where WType : Window, new()
        {
            PType wPresenter = WPresenter<WType>.New<PType>((packedArgs.Length > 0) ? packedArgs[0] : null);
            SetupWindow<WType>(wPresenter);

            return wPresenter;
        }

        private void SetupWindow<T>(WPresenter<T> wPresenter, bool isFocus = true) where T : Window, new()
        {
            wPresenter.Window = new();
            wPresenter.InitPresenters();
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
            startup = new StartupLoadingWindow();
            startup.Show();
            Dispatcher.Run();
        }
    }
}
