using System.Collections.Generic;
using System;
using System.Windows;

using GUIForDiskpart.Utils;
using GUIForDiskpart.Presentation.Presenter;
using GUIForDiskpart.Presentation.View.UserControls;


namespace GUIForDiskpart.Presentation.View.Windows
{
    public interface IGUIFDWindow
    {
        public IPresenter WindowPresenter => GetWindowPresenter();
        public Dictionary<IGUIFDUserControl, List<IPresenter>> ChildPresenters => this.GetPresentersInChildren();

        public IPresenter GetWindowPresenter();
    }

    public static class GUIFDWindowExtensions
    {
        public static void InitializeGUIFDWindow(this IGUIFDWindow guifdWindow)
        {
            Window window = (Window)guifdWindow;
            window.Focus();

            GetPresentersInChildren(guifdWindow);
            guifdWindow.WindowPresenter.RegisterEvents();
        }

        public static Dictionary<IGUIFDUserControl, List<IPresenter>> GetPresentersInChildren(this IGUIFDWindow guifdWindow)
        {
            Window window = (Window)guifdWindow;
            Dictionary<IGUIFDUserControl, List<IPresenter>> retVal = new();

            foreach (var child in window.GetChildrenUserControls())
            {
                IGUIFDUserControl guifdUserControl = (IGUIFDUserControl)child;
                if (guifdUserControl == null) continue;

                retVal.Add(guifdUserControl, guifdUserControl.Presenters);
                foreach (IPresenter presenter in guifdUserControl.Presenters)
                {
                    presenter.RegisterEvents();
                }
            }
            
            return retVal;
        }
    }
}
