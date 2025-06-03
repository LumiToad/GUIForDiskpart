using System.Windows;
using System.Windows.Controls;

namespace GUIForDiskpart.Presentation.Presenter
{
    public abstract class Presenter
    {
        protected bool hasRegistered = false;

        public void RegisterEvents()
        {
            if (hasRegistered) return;
            hasRegistered = true;

            RegisterEventsInternal();
        }

        protected abstract void RegisterEventsInternal();
    }

    public abstract class WPresenter<T> : Presenter where T : Window
    {
        public T Window;

        public WPresenter(T window) => Window = window;
    }

    public abstract class UCPresenter<T> : Presenter where T : UserControl
    {
        public readonly T UserControl;

        public UCPresenter(T userControl)
        {
            UserControl = userControl;
            RegisterEvents();
        }
    }
}
