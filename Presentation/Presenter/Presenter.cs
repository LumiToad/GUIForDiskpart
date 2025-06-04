using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;
using System.Reflection;

using GUIForDiskpart.Utils;


namespace GUIForDiskpart.Presentation.Presenter
{
    public class Presenter
    {
        protected bool hasRegistered = false;

        public void RegisterEvents()
        {
            if (hasRegistered) return;
            hasRegistered = true;

            RegisterEventsInternal();
        }

        protected virtual void RegisterEventsInternal()
        {
            StackFrame? sf = new StackTrace(true).GetFrame(0);
            if (sf.GetMethod().DeclaringType != typeof(Presenter))
            {
                ErrorUtils.NotImplementedException();
            }
        }
    }

    public class WPresenter<T> : Presenter where T : Window
    {
        public T Window;
        private Dictionary<Type, object> key_UC_Value_P = new();

        public WPresenter(T window) => Window = window;

        public dynamic GetUCPresenter<UCType>() where UCType : UserControl
        {
            return key_UC_Value_P[typeof(UCType)];
        }

        protected PType CreateUCPresenter<PType>(UserControl userControl) where PType : new()
        {
            var method = GetType().GetMethod("CreateUCPresenterInternal", BindingFlags.NonPublic | BindingFlags.Instance);
            method = method.MakeGenericMethod(userControl.GetType(), typeof(PType));

            var retVal = method.Invoke(this, new object[] { userControl });
            return (PType)retVal;
        }

        protected PType CreateUCPresenterInternal<UCType, PType>(UserControl userControl)
            where UCType : UserControl
            where PType : UCPresenter<UCType>, new()
        {
            var ucPresenter = new PType();
            ucPresenter.AddUserControl(userControl);
            key_UC_Value_P.Add(typeof(UCType), ucPresenter);

            return ucPresenter;
        }
    }

    public class UCPresenter<T> : Presenter where T : UserControl
    {
        public T UserControl { get; private set; }

        public void AddUserControl(UserControl userControl)
        {
            UserControl = (T)userControl;
            RegisterEvents();
        }
    }
}
