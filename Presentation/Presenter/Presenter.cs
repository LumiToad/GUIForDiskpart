using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;
using System.Reflection;

using GUIForDiskpart.Utils;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.ComponentModel;


namespace GUIForDiskpart.Presentation.Presenter
{
    public class Presenter
    {
        public PMainWindow? MainWindow
        {
            get 
            {
                if (App.Instance.WIM.HasGUIFDMainWinLoaded)
                {
                    return App.Instance.WIM.GetPresenter<PMainWindow>();
                }
                else return null;
            }
        }

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

        public static dynamic New<WPresenterClass>(params object?[] args) where WPresenterClass : WPresenter<T>, new()
        {
            var wPresenter = new WPresenterClass();
            wPresenter.AddCustomArgs(args);
            return wPresenter;
        }

        public virtual void Setup() { }

        protected virtual void AddCustomArgs(params object?[] args) { }
        public virtual void InitPresenters() { }

        /// <summary>
        /// Returns either the specified generic UserControlClass OR a List of those!
        /// Out parameter isList will tell you.
        /// </summary>
        /// <typeparam name="UserControlClass"></typeparam>
        /// <returns></returns>
        public dynamic GetUCPresenter<UserControlClass>(out bool isList) where UserControlClass : UserControl
        {
            isList = (key_UC_Value_P[typeof(UserControlClass)] is List<UserControlClass>);
            return key_UC_Value_P[typeof(UserControlClass)];
        }

        protected UCPresenterClass CreateUCPresenter<UCPresenterClass>(UserControl userControl, params object?[] args) where UCPresenterClass : new()
        {
            var method = GetType().GetMethod("CreateUCPresenterInternal", BindingFlags.NonPublic | BindingFlags.Instance);
            method = method.MakeGenericMethod(userControl.GetType(), typeof(UCPresenterClass));

            var retVal = method.Invoke(this, new object?[] { userControl, args} );
            return (UCPresenterClass)retVal;
        }

        protected PType CreateUCPresenterInternal<UCType, PType>(UserControl userControl, params object?[] args)
            where UCType : UserControl
            where PType : UCPresenter<UCType>, new()
        {
            var ucPresenter = new PType();
            ucPresenter.AddUserControl(userControl);
            ucPresenter.AddCustomArgs(args);
            ucPresenter.Setup();

            if (key_UC_Value_P.ContainsKey(typeof(UCType)))
            {
                var foundValue = key_UC_Value_P[typeof(UCType)];
                if (foundValue is List<UCType>)
                {
                    ((List<PType>)foundValue).Add(ucPresenter);
                }
                else
                {
                    List<PType> collection = new();
                    collection.Add(key_UC_Value_P[typeof(UCType)] as PType);

                    key_UC_Value_P.Remove(typeof(UCType));
                    key_UC_Value_P.Add(typeof(UCType), collection);
                }
            }
            else
            {
                key_UC_Value_P.Add(typeof(UCType), ucPresenter);
            }

            return ucPresenter;
        }

        /// <summary>
        /// Closes the Window and terminates its presenter.
        /// </summary>
        public void Close()
        {
            App.Instance.WIM.RemovePresenter(this);
            Window.Close();
        }

        public void Close(object? sender, CancelEventArgs e) => App.Instance.WIM.RemovePresenter(this);
    }

    public class UCPresenter<T> : Presenter where T : UserControl
    {
        public T UserControl { get; private set; }

        public virtual void Setup() { }

        public virtual void AddCustomArgs(params object?[] args) { }

        public void AddUserControl(UserControl userControl)
        {
            UserControl = (T)userControl;
            RegisterEvents();
        }
    }
}
